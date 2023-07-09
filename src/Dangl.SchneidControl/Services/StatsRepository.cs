using Dangl.Data.Shared;
using Dangl.SchneidControl.Data;
using Dangl.SchneidControl.Models.Controllers.Stats;
using Microsoft.EntityFrameworkCore;

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

            var dbEntries = await _context
                .DataEntries
                .Where(e => (startUtc == null || e.CreatedAtUtc >= startUtc)
                    && (endUtc == null || e.CreatedAtUtc <= startUtc)
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

            return RepositoryResult<Stats>.Success(stats);


            throw new NotImplementedException();
        }

        private static string GetUnitForLogEntryType(LogEntryType type)
        {
            switch (type)
            {
                case LogEntryType.HeatingCircuit1Pump:
                case LogEntryType.HeatingCircuit0Pump:
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
                case LogEntryType.TotalEnergyConsumption:
                    return decimalValue;

                case LogEntryType.BoilerTemperature:
                case LogEntryType.BufferTemperature:
                case LogEntryType.OuterTemperature:
                    return decimalValue / 10;

                default:
                    throw new NotImplementedException();
            }
        }
    }
}
