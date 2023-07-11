using CsvHelper.Configuration;
using CsvHelper;
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

        public async Task<RepositoryResult<Stats>> GetStatsAsync(DateTime? startUtc, DateTime? endUtc, LogEntryType type)
        {
            if (!Enum.IsDefined(type))
            {
                return RepositoryResult<Stats>.Fail($"The entry type is now defined: {type}");
            }

            if (startUtc != null)
            {
                var offset = TimeZoneInfo.Local.GetUtcOffset(startUtc.Value);
                startUtc = startUtc.Value.Subtract(offset);
            }

            if (endUtc != null)
            {
                var offset = TimeZoneInfo.Local.GetUtcOffset(endUtc.Value);
                endUtc = endUtc.Value.Subtract(offset);
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
                    CreatedAtUtc = dbEntry.CreatedAtUtc.ToLocalTime(),
                    Value = GetDataEntryValueForElement(type, dbEntry.Value)
                })
                    .ToList()
            };

            return RepositoryResult<Stats>.Success(stats);


            throw new NotImplementedException();
        }

        private static string GetUnitForLogEntryType(LogEntryType type)
        {
            switch (type)
            {
                case LogEntryType.HeatingCircuit1Pump:
                case LogEntryType.HeatingCircuit0Pump:
                case LogEntryType.BoilerLoadingPump:
                case LogEntryType.BufferLoadingPump:
                    return "An";

                case LogEntryType.HeatingPowerDraw:
                    return "W";

                case LogEntryType.BoilerTemperature:
                case LogEntryType.BufferTemperature:
                case LogEntryType.OuterTemperature:
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
                case LogEntryType.HeatingCircuit0Pump:
                case LogEntryType.HeatingPowerDraw:
                case LogEntryType.ValveOpening:
                case LogEntryType.BoilerLoadingPump:
                case LogEntryType.BufferLoadingPump:
                    return decimalValue;

                case LogEntryType.TotalEnergyConsumption:
                case LogEntryType.BoilerTemperature:
                case LogEntryType.BufferTemperature:
                case LogEntryType.OuterTemperature:
                    return decimalValue / 10;

                default:
                    throw new NotImplementedException();
            }
        }

        public async Task<RepositoryResult<FileResultContainer>> ExportToExcelAsync(DateTime? startUtc, DateTime? endUtc, LogEntryType type)
        {
            var entries = await GetStatsAsync(startUtc, endUtc, type);
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

        public async Task<RepositoryResult<FileResultContainer>> ExportToCsvAsync(DateTime? startUtc, DateTime? endUtc, LogEntryType type)
        {
            var entries = await GetStatsAsync(startUtc, endUtc, type);
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
