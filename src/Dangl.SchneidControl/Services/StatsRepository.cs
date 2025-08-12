﻿using CsvHelper;
using Dangl.Data.Shared;
using Dangl.SchneidControl.Data;
using Dangl.SchneidControl.Models.Controllers.Stats;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text;
using Dangl.SchneidControl.Models.Stats;
using ClosedXML.Excel;

namespace Dangl.SchneidControl.Services
{
    public class StatsRepository : IStatsRepository
    {
        private readonly DataLoggingContext _context;

        public StatsRepository(DataLoggingContext context)
        {
            _context = context;
        }

        public async Task<RepositoryResult<Stats>> GetStatsAsync(DateTime? startUtc, DateTime? endUtc, LogEntryType type, int utcTimeZoneOffset)
        {
            if (!Enum.IsDefined(type))
            {
                return RepositoryResult<Stats>.Fail($"The entry type is now defined: {type}");
            }

            var userTimeZoneOffset = TimeSpan.FromMinutes(utcTimeZoneOffset);

            if (startUtc != null)
            {
                startUtc = startUtc.Value.Add(userTimeZoneOffset);
            }

            if (endUtc != null)
            {
                endUtc = endUtc.Value.Add(userTimeZoneOffset);
            }

            var dbEntries = await _context
                .DataEntries
                .Where(e => (startUtc == null || e.CreatedAtUtc >= startUtc)
                    && (endUtc == null || e.CreatedAtUtc <= endUtc)
                    && e.LogEntryType == type)
                .OrderBy(e => e.CreatedAtUtc)
                .Select(e => new
                {
                    e.CreatedAtUtc,
                    e.Value
                })
                .ToListAsync();

            var stats = new Stats
            {
                StartUtc = startUtc,
                EndUtc = endUtc,
                LogEntryType = type,
                Unit = GetUnitForLogEntryType(type),
                Entries = dbEntries.Select(dbEntry => new Models.Controllers.Stats.DataEntry
                    {
                        CreatedAtUtc = dbEntry.CreatedAtUtc,
                        Value = GetDataEntryValueForElement(type, dbEntry.Value)
                    })
                    .ToList()
            };

            if (type == LogEntryType.OuterTemperature
                || type == LogEntryType.BufferTemperature
                || type == LogEntryType.AdvanceTemperature
                || type == LogEntryType.HeatingCircuit1AdvanceTemperature
                || type == LogEntryType.HeatingCircuit2AdvanceTemperature
                || type == LogEntryType.BoilerTemperature)
            {
                // We're sometimes getting really high or really low readings from the ModBus module
                RemoveErrorenousTemperatures(stats);
            }
            else if (type == LogEntryType.HeatingPowerDraw)
            {
                RemoveErrorenousHeatingPowerDraw(stats);
            }
            else if (type == LogEntryType.TotalEnergyConsumption)
            {
                var replacements = await _context.HeatMeterReplacements
                    .OrderBy(e => e.ReplacedAtUtc)
                    .ToListAsync();
                CorrectStatsForHeatMeterReplacements(replacements,
                    stats.Entries,
                    correctDecimalPlaces: true);
            }

            var roundingDecimalPrecision = type switch
            {
                LogEntryType.BufferLoadingPump => 0,
                LogEntryType.BoilerLoadingPump => 0,
                LogEntryType.HeatingCircuit1Pump => 0,
                LogEntryType.HeatingCircuit2Pump => 0,
                LogEntryType.ValveOpening => 0,
                LogEntryType.TotalEnergyConsumption => 0,
                LogEntryType.HeatingPowerDraw => 0,
                LogEntryType.AdvanceTemperature => 1,
                LogEntryType.HeatingCircuit1AdvanceTemperature => 1,
                LogEntryType.HeatingCircuit2AdvanceTemperature => 1,
                LogEntryType.BufferTemperature => 1,
                LogEntryType.OuterTemperature => 1,
                LogEntryType.BoilerTemperature => 1,
                _ => 0
            };

            ReduceEntriesIfRequired(stats, 1000, roundingDecimalPrecision);

            return RepositoryResult<Stats>.Success(stats);
        }

        public static void CorrectStatsForHeatMeterReplacements(List<HeatMeterReplacement> replacements,
            List<Models.Controllers.Stats.DataEntry> entries,
            bool correctDecimalPlaces)
        {
            HeatMeterReplacement? currentReplacement = null;
            HeatMeterReplacement? nextReplacement = null;
            var totalToAdd = 0m;
            if (replacements.Any())
            {
                foreach (var entry in entries)
                {
                    // If we don't have a current replacement, we'll set it, if required
                    if (currentReplacement == null)
                    {
                        if (replacements[0].ReplacedAtUtc <= entry.CreatedAtUtc)
                        {
                            currentReplacement = replacements[0];
                            totalToAdd -= correctDecimalPlaces ? currentReplacement.InitialValue / 10 : currentReplacement.InitialValue;
                            totalToAdd += correctDecimalPlaces ? currentReplacement.OldMeterValue / 10 : currentReplacement.OldMeterValue;

                            if (replacements.Count > 1)
                            {
                                nextReplacement = replacements[1];
                            }
                        }
                    }
                    else if (replacements.Count > 1 && nextReplacement != null)
                    {
                        if (nextReplacement.ReplacedAtUtc <= entry.CreatedAtUtc)
                        {
                            currentReplacement = nextReplacement;
                            totalToAdd -= correctDecimalPlaces ? currentReplacement.InitialValue / 10 : currentReplacement.InitialValue;
                            totalToAdd += correctDecimalPlaces ? currentReplacement.OldMeterValue / 10 : currentReplacement.OldMeterValue;
                            nextReplacement = replacements
                                .SkipWhile(e => e != currentReplacement)
                                .Skip(1)
                                .FirstOrDefault();
                        }
                    }

                    if (currentReplacement != null)
                    {
                        entry.Value += totalToAdd;
                    }
                }
            }
        }

        private void RemoveErrorenousTemperatures(Stats stats)
        {
            stats.Entries = stats
                .Entries
                .Where(e => e.Value > -100 && e.Value < 120)
                .ToList();
        }
        private void RemoveErrorenousHeatingPowerDraw(Stats stats)
        {
            stats.Entries = stats
                .Entries
                .Where(e => e.Value > -100 && e.Value < 100_000)
                .ToList();
        }

        private void ReduceEntriesIfRequired(Stats stats, int maxEntries, int decimalPrecision)
        {
            var originalEntries = stats.Entries;
            if (originalEntries.Count <= maxEntries)
            {
                return;
            }

            stats.Entries = new List<Models.Controllers.Stats.DataEntry>(maxEntries);
            var entriesToMerge = Convert.ToDouble(originalEntries.Count) / maxEntries;
            var originalEntriesArray = originalEntries.ToArray();
            var lastIndex = 0;
            for (var i = 0; i < maxEntries; i++)
            {
                var endIndex = Convert.ToInt32(Math.Ceiling((i + 1) * entriesToMerge));
                if (endIndex >= originalEntriesArray.Length)
                {
                    endIndex = originalEntriesArray.Length;
                }

                var entriesRange = originalEntriesArray[lastIndex..endIndex];

                var newEntry = new Models.Controllers.Stats.DataEntry
                {
                    CreatedAtUtc = entriesRange.First().CreatedAtUtc,
                    Value = Math.Round(entriesRange.Average(e => e.Value), decimalPrecision)
                };
                stats.Entries.Add(newEntry);
                lastIndex = endIndex;
            }
        }

        private static string GetUnitForLogEntryType(LogEntryType type)
        {
            switch (type)
            {
                case LogEntryType.HeatingCircuit1Pump:
                case LogEntryType.HeatingCircuit2Pump:
                case LogEntryType.BoilerLoadingPump:
                case LogEntryType.BufferLoadingPump:
                    return "An";

                case LogEntryType.HeatingPowerDraw:
                    return "W";

                case LogEntryType.BoilerTemperature:
                case LogEntryType.BufferTemperature:
                case LogEntryType.OuterTemperature:
                case LogEntryType.HeatingCircuit1AdvanceTemperature:
                case LogEntryType.HeatingCircuit2AdvanceTemperature:
                case LogEntryType.AdvanceTemperature:
                    return "°C";

                case LogEntryType.ValveOpening:
                    return "%";

                case LogEntryType.TotalEnergyConsumption:
                    return "kWh";

                default:
                    throw new NotImplementedException();
            }
        }

        private static decimal GetDataEntryValueForElement(LogEntryType type, int databaseValue)
        {
            var decimalValue = Convert.ToDecimal(databaseValue);


            switch (type)
            {
                case LogEntryType.HeatingCircuit1Pump:
                case LogEntryType.HeatingCircuit2Pump:
                case LogEntryType.HeatingPowerDraw:
                case LogEntryType.ValveOpening:
                case LogEntryType.BoilerLoadingPump:
                case LogEntryType.BufferLoadingPump:
                    return decimalValue;

                case LogEntryType.TotalEnergyConsumption:
                case LogEntryType.BoilerTemperature:
                case LogEntryType.BufferTemperature:
                case LogEntryType.OuterTemperature:
                case LogEntryType.AdvanceTemperature:
                case LogEntryType.HeatingCircuit1AdvanceTemperature:
                case LogEntryType.HeatingCircuit2AdvanceTemperature:
                    return decimalValue / 10;

                default:
                    throw new NotImplementedException();
            }
        }

        public async Task<RepositoryResult<FileResultContainer>> ExportToExcelAsync(DateTime? startUtc, DateTime? endUtc, LogEntryType type, int utcTimeZoneOffset)
        {
            var entries = await GetStatsAsync(startUtc, endUtc, type, utcTimeZoneOffset);
            if (!entries.IsSuccess)
            {
                return RepositoryResult<FileResultContainer>.Fail(entries.ErrorMessage);
            }

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Auswertung");

            worksheet.Cell(1,1).SetValue("Datum");
            worksheet.Cell(1,2).SetValue($"Wert ({entries.Value.Unit})");

            var currentLine = 2;
            foreach (var entry in entries.Value.Entries)
            {
                worksheet.Cell(currentLine, 1).SetValue(entry.CreatedAtUtc);
                worksheet.Cell(currentLine, 2).SetValue(entry.Value);
                currentLine++;
            }

            var outputStream = new MemoryStream();
            workbook.SaveAs(outputStream);
            outputStream.Position = 0;

            var mimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            return RepositoryResult<FileResultContainer>.Success(new FileResultContainer(outputStream, "Export.xlsx", mimeType));
        }

        public async Task<RepositoryResult<FileResultContainer>> ExportToCsvAsync(DateTime? startUtc, DateTime? endUtc, LogEntryType type, int utcTimeZoneOffset)
        {
            var entries = await GetStatsAsync(startUtc, endUtc, type, utcTimeZoneOffset);
            if (!entries.IsSuccess)
            {
                return RepositoryResult<FileResultContainer>.Fail(entries.ErrorMessage);
            }

            var csvEntries = entries
                .Value
                .Entries
                .Select(e => new CsvOutputEntry
                {
                    DateUtc = e.CreatedAtUtc,
                    Value = e.Value,
                })
                .ToList();

            var outputStream = new MemoryStream();
            using var streamWriter = new StreamWriter(outputStream, Encoding.UTF8, 1024, true);
            using var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture, leaveOpen: true);
            await csvWriter.WriteRecordsAsync(csvEntries);
            await csvWriter.FlushAsync();
            await streamWriter.FlushAsync();
            outputStream.Position = 0;
            return RepositoryResult<FileResultContainer>.Success(new FileResultContainer(outputStream, "Export.csv", "application/octet-stream"));
        }
    }
}
