using Dangl.Data.Shared;
using Dangl.SchneidControl.Data;
using Dangl.SchneidControl.Extensions;
using Dangl.SchneidControl.Models.Controllers.Consumption;
using Dangl.SchneidControl.Models.Controllers.Stats;
using Dangl.SchneidControl.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace Dangl.SchneidControl.Services
{
    public class ConsumptionRepository : IConsumptionRepository
    {
        private readonly DataLoggingContext _context;

        public ConsumptionRepository(DataLoggingContext context)
        {
            _context = context;
        }

        public async Task<RepositoryResult<Consumption>> GetConsumptionAsync(DateTime? startUtc, DateTime? endUtc, ConsumptionResolution resolution, int utcTimeZoneOffset)
        {
            if (!Enum.IsDefined(resolution))
            {
                return RepositoryResult<Consumption>.Fail($"The resolution type is now defined: {resolution}");
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
                    && e.LogEntryType == LogEntryType.TotalEnergyConsumption)
                .OrderBy(e => e.CreatedAtUtc)
                .Select(e => new Models.Controllers.Stats.DataEntry
                {
                    CreatedAtUtc = e.CreatedAtUtc,
                    Value = e.Value
                })
                .ToListAsync();

            if (!dbEntries.Any())
            {
                return RepositoryResult<Consumption>.Fail($"There are no entries for the selected duration.");
            }

                var replacements = await _context.HeatMeterReplacements
                    .OrderBy(e => e.ReplacedAtUtc)
                    .ToListAsync();
                StatsRepository.CorrectStatsForHeatMeterReplacements(replacements,
                    dbEntries,
                    correctDecimalPlaces: false);

            // Group by resolution
            var entries = new List<ConsumptionDataEntry>();
            var previousDbValue = dbEntries.First();
            var hasSet = false;
            foreach (var dbEntry in dbEntries.Skip(1))
            {
                var end = previousDbValue.CreatedAtUtc.GetEndOfResolution(resolution);
                if (dbEntry.CreatedAtUtc < end)
                {
                    hasSet = false;
                }
                else
                {
                    entries.Add(new ConsumptionDataEntry
                    {
                        Value = Convert.ToDecimal(dbEntry.Value - previousDbValue.Value) / 10,
                        CreatedAtUtc = previousDbValue.CreatedAtUtc.GetStartOfResolution(resolution)
                    });
                    previousDbValue = dbEntry;
                    hasSet = true;
                }
            }

            if (!hasSet)
            {
                entries.Add(new ConsumptionDataEntry
                {
                    Value = Convert.ToDecimal(dbEntries.Last().Value - previousDbValue.Value) / 10,
                    CreatedAtUtc = previousDbValue.CreatedAtUtc.GetStartOfResolution(resolution)
                });
            }

            var result = new Consumption
            {
                EndUtc = endUtc,
                StartUtc = startUtc,
                Resolution = resolution,
                Unit = "kWh",
                Entries = entries
            };
            return RepositoryResult<Consumption>.Success(result);
        }
    }
}
