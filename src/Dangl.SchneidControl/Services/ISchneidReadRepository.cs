﻿using Dangl.Data.Shared;
using Dangl.SchneidControl.Models.Controllers.Values;
using Dangl.SchneidControl.Models.Enums;

namespace Dangl.SchneidControl.Services
{
    public interface ISchneidReadRepository
    {
        Task<RepositoryResult<DecimalValue>> GetOuterTemperaturAsync();

        Task<RepositoryResult<DecimalValue>> GetTotalEnergyConsumptionAsync();

        Task<RepositoryResult<DecimalValue>> GetCurrentHeatingPowerDrawAsync();

        Task<RepositoryResult<EnumValue<TransferStationStatus>>> GetTransferStationStatusAsync();

        Task<RepositoryResult<DecimalValue>> GetTargetBufferTopTemperatureAsync();

        Task<RepositoryResult<DecimalValue>> GetTargetBoilerTemperatureAsync();

        Task<RepositoryResult<EnumValue<HeatingCircuitStatus>>> GetHeatingCircuitStatus00Async();

        Task<RepositoryResult<EnumValue<HeatingCircuitStatus>>> GetHeatingCircuitStatus01Async();

        Task<RepositoryResult<EnumValue<HeatingCircuitStatus>>> GetHeatingCircuitStatus02Async();

        Task<RepositoryResult<DecimalValue>> GetHeatingCircuit00AdvanceTemperatureAsync();

        Task<RepositoryResult<DecimalValue>> GetHeatingCircuit01AdvanceTemperatureAsync();

        Task<RepositoryResult<DecimalValue>> GetHeatingCircuit02AdvanceTemperatureAsync();

        Task<RepositoryResult<BoolValue>> GetPumpStatusHeatingCircuit00Async();

        Task<RepositoryResult<BoolValue>> GetPumpStatusHeatingCircuit01Async();

        Task<RepositoryResult<BoolValue>> GetPumpStatusHeatingCircuit02Async();

        Task<RepositoryResult<BoolValue>> GetBufferLoadingPumpStatusAsync();

        Task<RepositoryResult<BoolValue>> GetBoilerLoadingPumpStatusAsync();

        Task<RepositoryResult<DecimalValue>> GetBufferTemperatureTopAsync();

        Task<RepositoryResult<DecimalValue>> GetBufferTemperatureBottomAsync();

        Task<RepositoryResult<DecimalValue>> GetBoilerTemperatureTopAsync();

        Task<RepositoryResult<DecimalValue>> GetBoilerTemperatureBottomAsync();

        Task<RepositoryResult<DecimalValue>> GetAdvanceTemperatureAsync();

        Task<RepositoryResult<DecimalValue>> GetPrimaryRefluxTemperatureAsync();

        Task<RepositoryResult<DecimalValue>> GetSecondaryRefluxTemperatureAsync();

        Task<RepositoryResult<DecimalValue>> GetValveOpeningAsync();
    }
}
