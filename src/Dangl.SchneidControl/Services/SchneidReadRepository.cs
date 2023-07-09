using Dangl.Data.Shared;
using Dangl.SchneidControl.Models.Controllers.Values;
using Dangl.SchneidControl.Models.Enums;

namespace Dangl.SchneidControl.Services
{
    public class SchneidReadRepository : ISchneidReadRepository
    {
        private readonly ModbusConnectionManager _modbusConnectionManager;

        public SchneidReadRepository(ModbusConnectionManager modbusConnectionManager)
        {
            _modbusConnectionManager = modbusConnectionManager;
        }

        public Task<RepositoryResult<DecimalValue>> GetOuterTemperaturAsync()
        {
            return GetDecimal16BitValueAsync(600, 1, "°C");
        }

        public Task<RepositoryResult<DecimalValue>> GetTotalEnergyConsumptionAsync()
        {
            return GetDecimal32BitValueAsync(730, 1, "kWh");
        }

        public Task<RepositoryResult<DecimalValue>> GetCurrentHeatingPowerDrawAsync()
        {
            return GetDecimal32BitValueAsync(734, 0, "W");
        }

        public Task<RepositoryResult<DecimalValue>> GetTargetBufferTopTemperatureAsync()
        {
            return GetDecimal16BitValueAsync(55, 0, "°C");
        }

        public Task<RepositoryResult<DecimalValue>> GetTargetBoilerTemperatureAsync()
        {
            return GetDecimal16BitValueAsync(80, 0, "°C");
        }

        public Task<RepositoryResult<DecimalValue>> GetBufferTemperatureTopAsync()
        {
            return GetDecimal16BitValueAsync(704, 1, "°C");
        }

        public Task<RepositoryResult<DecimalValue>> GetBufferTemperatureBottomAsync()
        {
            return GetDecimal16BitValueAsync(705, 1, "°C");
        }

        public Task<RepositoryResult<DecimalValue>> GetBoilerTemperatureTopAsync()
        {
            return GetDecimal16BitValueAsync(690, 1, "°C");
        }

        public Task<RepositoryResult<DecimalValue>> GetBoilerTemperatureBottomAsync()
        {
            return GetDecimal16BitValueAsync(691, 1, "°C");
        }

        public Task<RepositoryResult<DecimalValue>> GetAdvanceTemperatureAsync()
        {
            return GetDecimal16BitValueAsync(602, 1, "°C");
        }

        public Task<RepositoryResult<DecimalValue>> GetPrimaryRefluxTemperatureAsync()
        {
            return GetDecimal16BitValueAsync(601, 1, "°C");
        }

        public Task<RepositoryResult<DecimalValue>> GetSecondaryRefluxTemperatureAsync()
        {
            return GetDecimal16BitValueAsync(603, 1, "°C");
        }

        public Task<RepositoryResult<DecimalValue>> GetValveOpeningAsync()
        {
            return GetDecimal16BitValueAsync(709, 0, "%");
        }

        public async Task<RepositoryResult<EnumValue<TransferStationStatus>>> GetTransferStationStatusAsync()
        {
            var integerValue = await GetDecimal16BitValueAsync(215, 0, string.Empty);
            if (!integerValue.IsSuccess)
            {
                return RepositoryResult<EnumValue<TransferStationStatus>>.Fail(integerValue.ErrorMessage);
            }

            var integer = Convert.ToInt32(integerValue.Value.Value);
            var enumValue = (TransferStationStatus)integer;
            return RepositoryResult<EnumValue<TransferStationStatus>>.Success(new EnumValue<TransferStationStatus> { Value = enumValue });
        }

        public async Task<RepositoryResult<EnumValue<HeatingCircuitStatus>>> GetHeatingCircuitStatus00Async()
        {
            var integerValue = await GetDecimal16BitValueAsync(617, 0, string.Empty);
            if (!integerValue.IsSuccess)
            {
                return RepositoryResult<EnumValue<HeatingCircuitStatus>>.Fail(integerValue.ErrorMessage);
            }

            var integer = Convert.ToInt32(integerValue.Value.Value);
            var enumValue = (HeatingCircuitStatus)integer;
            return RepositoryResult<EnumValue<HeatingCircuitStatus>>.Success(new EnumValue<HeatingCircuitStatus> { Value = enumValue });
        }

        public async Task<RepositoryResult<EnumValue<HeatingCircuitStatus>>> GetHeatingCircuitStatus01Async()
        {
            var integerValue = await GetDecimal16BitValueAsync(627, 0, string.Empty);
            if (!integerValue.IsSuccess)
            {
                return RepositoryResult<EnumValue<HeatingCircuitStatus>>.Fail(integerValue.ErrorMessage);
            }

            var integer = Convert.ToInt32(integerValue.Value.Value);
            var enumValue = (HeatingCircuitStatus)integer;
            return RepositoryResult<EnumValue<HeatingCircuitStatus>>.Success(new EnumValue<HeatingCircuitStatus> { Value = enumValue });
        }

        public async Task<RepositoryResult<BoolValue>> GetPumpStatusHeatingCircuit00Async()
        {
            var integerValue = await GetDecimal16BitValueAsync(616, 0, string.Empty);
            if (!integerValue.IsSuccess)
            {
                return RepositoryResult<BoolValue>.Fail(integerValue.ErrorMessage);
            }

            return RepositoryResult<BoolValue>.Success(new BoolValue { Value = integerValue.Value.Value == 1m });
        }

        public async Task<RepositoryResult<BoolValue>> GetPumpStatusHeatingCircuit01Async()
        {
            var integerValue = await GetDecimal16BitValueAsync(626, 0, string.Empty);
            if (!integerValue.IsSuccess)
            {
                return RepositoryResult<BoolValue>.Fail(integerValue.ErrorMessage);
            }

            return RepositoryResult<BoolValue>.Success(new BoolValue { Value = integerValue.Value.Value == 1m });
        }

        private Task<RepositoryResult<DecimalValue>> GetDecimal16BitValueAsync(ushort address,
            int decimalPlaces,
            string unit)
        {
            return GetModbusValueAndConvertToDecimalAsync(() => _modbusConnectionManager.GetInteger16ValueAsync(address), decimalPlaces, unit);
        }

        private Task<RepositoryResult<DecimalValue>> GetDecimal32BitValueAsync(ushort address,
            int decimalPlaces,
            string unit)
        {
            return GetModbusValueAndConvertToDecimalAsync(() => _modbusConnectionManager.GetInteger32ValueAsync(address), decimalPlaces, unit);
        }

        private async Task<RepositoryResult<DecimalValue>> GetModbusValueAndConvertToDecimalAsync(Func<Task<uint>> action, int decimalPlaces, string unit)
        {
            try
            {
                var integerValue = await action();
                var decimalValue = Convert.ToDecimal(integerValue);
                while (decimalPlaces > 0)
                {
                    decimalValue /= 10;
                    decimalPlaces--;
                }

                return RepositoryResult<DecimalValue>.Success(new DecimalValue
                {
                    Value = decimalValue,
                    Unit = unit
                });
            }
            catch (Exception e)
            {
                return RepositoryResult<DecimalValue>.Fail(e.ToString());
            }
        }

        private async Task<RepositoryResult<DecimalValue>> GetModbusValueAndConvertToDecimalAsync(Func<Task<int>> action, int decimalPlaces, string unit)
        {
            try
            {
                var integerValue = await action();
                var decimalValue = Convert.ToDecimal(integerValue);
                while (decimalPlaces > 0)
                {
                    decimalValue /= 10;
                    decimalPlaces--;
                }

                return RepositoryResult<DecimalValue>.Success(new DecimalValue
                {
                    Value = decimalValue,
                    Unit = unit
                });
            }
            catch (Exception e)
            {
                return RepositoryResult<DecimalValue>.Fail(e.ToString());
            }
        }
    }
}
