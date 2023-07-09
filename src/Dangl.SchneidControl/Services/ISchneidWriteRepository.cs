using Dangl.Data.Shared;
using Dangl.SchneidControl.Models.Enums;

namespace Dangl.SchneidControl.Services
{
    public interface ISchneidWriteRepository
    {
        Task<RepositoryResult> SetTransferStationModeAsync(TransferStationStatus transferStationMode);

        Task<RepositoryResult> SetTargetBufferTopTemperatureAsync(decimal targetTemperature);

        Task<RepositoryResult> SetTargetBoilerTemperatureAsync(decimal targetTemperature);
    }
}
