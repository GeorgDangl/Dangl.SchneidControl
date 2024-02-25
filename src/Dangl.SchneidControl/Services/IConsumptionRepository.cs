using Dangl.Data.Shared;
using Dangl.SchneidControl.Models.Controllers.Consumption;
using Dangl.SchneidControl.Models.Enums;

namespace Dangl.SchneidControl.Services
{
    public interface IConsumptionRepository
    {
        Task<RepositoryResult<Consumption>> GetConsumptionAsync(DateTime? startUtc, DateTime? endUtc, ConsumptionResolution resolution, int utcTimeZoneOffset);
    }
}
