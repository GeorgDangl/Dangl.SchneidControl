using Dangl.Data.Shared;
using Dangl.SchneidControl.Data;
using Dangl.SchneidControl.Models.Controllers.Stats;

namespace Dangl.SchneidControl.Services
{
    public interface IStatsRepository
    {
        Task<RepositoryResult<Stats>> GetStatsAsync(DateTime? startUtc, DateTime? endUtc, LogEntryType type, int utcTimeZoneOffset);

        Task<RepositoryResult<FileResultContainer>> ExportToExcelAsync(DateTime? startUtc, DateTime? endUtc, LogEntryType type, int utcTimeZoneOffset);

        Task<RepositoryResult<FileResultContainer>> ExportToCsvAsync(DateTime? startUtc, DateTime? endUtc, LogEntryType type, int utcTimeZoneOffset);
    }
}
