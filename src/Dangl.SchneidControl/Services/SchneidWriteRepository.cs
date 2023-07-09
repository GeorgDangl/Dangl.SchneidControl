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

        public async Task<RepositoryResult> SetTargetBoilerTemperatureAsync(decimal targetTemperature)
        {
            var shortValue = Convert.ToInt16(targetTemperature);
            if (shortValue < 50 || shortValue > 70)
            {
                return RepositoryResult.Fail("Please specify a range between 50 and 65");
            }

            try
            {
                await _modbusConnectionManager.SetInteger16ValueAsync(80, shortValue);
                return RepositoryResult.Success();
            }
            catch (Exception e)
            {
                return RepositoryResult.Fail(e.ToString());
            }
        }

        public async Task<RepositoryResult> SetTargetBufferTopTemperatureAsync(decimal targetTemperature)
        {
            var shortValue = Convert.ToInt16(targetTemperature);
            if (shortValue < 50 || shortValue > 70)
            {
                return RepositoryResult.Fail("Please specify a range between 50 and 70");
            }

            try
            {
                await _modbusConnectionManager.SetInteger16ValueAsync(55, shortValue);
                return RepositoryResult.Success();
            }
            catch (Exception e)
            {
                return RepositoryResult.Fail(e.ToString());
            }
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
