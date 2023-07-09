using Dangl.Data.Shared;
using Dangl.SchneidControl.Models.Enums;

namespace Dangl.SchneidControl.Services
{
    public class SchneidWriteRepository : ISchneidWriteRepository
    {
        private readonly ModbusConnectionManager _modbusConnectionManager;

        public SchneidWriteRepository(ModbusConnectionManager modbusConnectionManager)
        {
            _modbusConnectionManager = modbusConnectionManager;
        }

        public Task<RepositoryResult> SetTargetBoilerTemperatureAsync(decimal targetTemperature)
        {
            throw new NotImplementedException();
        }

        public Task<RepositoryResult> SetTargetBufferTopTemperatureAsync(decimal targetTemperature)
        {
            throw new NotImplementedException();
        }

        public async Task<RepositoryResult> SetTransferStationModeAsync(TransferStationStatus transferStationMode)
        {
            if (!Enum.IsDefined(transferStationMode))
            {
                return RepositoryResult.Fail($"Invalid enum value given for {nameof(transferStationMode)}.");
            }

            var integerValue = (int)transferStationMode;
            var shortValue = Convert.ToInt16(integerValue);

            try
            {
                await _modbusConnectionManager.SetInteger16ValueAsync(215, shortValue);
                return RepositoryResult.Success();
            }
            catch (Exception e)
            {
                return RepositoryResult.Fail(e.ToString());
            }
        }
    }
}
