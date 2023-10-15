using Dangl.Data.Shared;
using Dangl.SchneidControl.Models.Controllers.Values;
using Dangl.SchneidControl.Models.Enums;
using Dangl.SchneidControl.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Dangl.SchneidControl.Controllers
{
    [ApiController]
    [Route("/api/values")]
    public class ValuesController : ControllerBase
    {
        public ValuesController(ISchneidReadRepository schneidReadRepository)
        {
            _schneidReadRepository = schneidReadRepository;
        }

        private readonly ISchneidReadRepository _schneidReadRepository;

        [HttpGet("outer-temperature")]
        [ProducesResponseType(typeof(DecimalValue), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        public Task<IActionResult> GetOuterTemperatureAsync()
        {
            return RepositoryResponseAsync(() => _schneidReadRepository.GetOuterTemperaturAsync());
        }

        [HttpGet("total-energy-consumption")]
        [ProducesResponseType(typeof(DecimalValue), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        public Task<IActionResult> GetTotalEnergyConsumptionAsync()
        {
            return RepositoryResponseAsync(() => _schneidReadRepository.GetTotalEnergyConsumptionAsync());
        }

        [HttpGet("heating-power")]
        [ProducesResponseType(typeof(DecimalValue), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        public Task<IActionResult> GetCurrentHeatingPowerAsync()
        {
            return RepositoryResponseAsync(() => _schneidReadRepository.GetCurrentHeatingPowerDrawAsync());
        }

        [HttpGet("advance-temperature")]
        [ProducesResponseType(typeof(DecimalValue), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        public Task<IActionResult> GetAdvanceTemperatureAsync()
        {
            return RepositoryResponseAsync(() => _schneidReadRepository.GetAdvanceTemperatureAsync());
        }

        [HttpGet("primary-reflux-temperature")]
        [ProducesResponseType(typeof(DecimalValue), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        public Task<IActionResult> GetPrimaryRefluxTemperatureAsync()
        {
            return RepositoryResponseAsync(() => _schneidReadRepository.GetPrimaryRefluxTemperatureAsync());
        }

        [HttpGet("secondary-reflux-temperature")]
        [ProducesResponseType(typeof(DecimalValue), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        public Task<IActionResult> GetSecondaryRefluxTemperatureAsync()
        {
            return RepositoryResponseAsync(() => _schneidReadRepository.GetSecondaryRefluxTemperatureAsync());
        }

        [HttpGet("valve-opening")]
        [ProducesResponseType(typeof(DecimalValue), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        public Task<IActionResult> GetValveOpeningAsync()
        {
            return RepositoryResponseAsync(() => _schneidReadRepository.GetValveOpeningAsync());
        }

        [HttpGet("transfer-station-status")]
        [ProducesResponseType(typeof(EnumValue<TransferStationStatus>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        public Task<IActionResult> GetTransferStationStatusAsync()
        {
            return RepositoryResponseAsync(() => _schneidReadRepository.GetTransferStationStatusAsync());
        }

        [HttpGet("buffer-temperature-top")]
        [ProducesResponseType(typeof(DecimalValue), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        public Task<IActionResult> GetBufferTemperatureTopAsync()
        {
            return RepositoryResponseAsync(() => _schneidReadRepository.GetBufferTemperatureTopAsync());
        }

        [HttpGet("buffer-temperature-bottom")]
        [ProducesResponseType(typeof(DecimalValue), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        public Task<IActionResult> GetBufferTemperatureBottomAsync()
        {
            return RepositoryResponseAsync(() => _schneidReadRepository.GetBufferTemperatureBottomAsync());
        }

        [HttpGet("boiler-temperature-top")]
        [ProducesResponseType(typeof(DecimalValue), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        public Task<IActionResult> GetBoilerTemperatureTopAsync()
        {
            return RepositoryResponseAsync(() => _schneidReadRepository.GetBoilerTemperatureTopAsync());
        }

        [HttpGet("boiler-temperature-bottom")]
        [ProducesResponseType(typeof(DecimalValue), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        public Task<IActionResult> GetBoilerTemperatureBottomAsync()
        {
            return RepositoryResponseAsync(() => _schneidReadRepository.GetBoilerTemperatureBottomAsync());
        }

        [HttpGet("target-buffer-temperature")]
        [ProducesResponseType(typeof(DecimalValue), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        public Task<IActionResult> GetTargetBufferTopTemperatureAsync()
        {
            return RepositoryResponseAsync(() => _schneidReadRepository.GetTargetBufferTopTemperatureAsync());
        }

        [HttpGet("target-boiler-temperature")]
        [ProducesResponseType(typeof(DecimalValue), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        public Task<IActionResult> GetTargetBoilerTemperatureAsync()
        {
            return RepositoryResponseAsync(() => _schneidReadRepository.GetTargetBoilerTemperatureAsync());
        }

        [HttpGet("circuit-status/{circuitId}")]
        [ProducesResponseType(typeof(EnumValue<HeatingCircuitStatus>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetHeatingCircuitStatusAsync(int circuitId)
        {
            if (circuitId == 0)
            {
                return await RepositoryResponseAsync(() => _schneidReadRepository.GetHeatingCircuitStatus00Async());
            }
            else if (circuitId == 1)
            {
                return await RepositoryResponseAsync(() => _schneidReadRepository.GetHeatingCircuitStatus01Async());
            }
            else if (circuitId == 2)
            {
                return await RepositoryResponseAsync(() => _schneidReadRepository.GetHeatingCircuitStatus02Async());
            }
            else
            {
                return BadRequest(new ApiError($"Invalid circuit ID: {circuitId}"));
            }
        }

        [HttpGet("pump-status/{pumpId}")]
        [ProducesResponseType(typeof(BoolValue), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetPumpStatusHeatingCircuitAsync(int pumpId)
        {
            if (pumpId == 0)
            {
                return await RepositoryResponseAsync(() => _schneidReadRepository.GetPumpStatusHeatingCircuit00Async());
            }
            else if (pumpId == 1)
            {
                return await RepositoryResponseAsync(() => _schneidReadRepository.GetPumpStatusHeatingCircuit01Async());
            }
            else if (pumpId == 2)
            {
                return await RepositoryResponseAsync(() => _schneidReadRepository.GetPumpStatusHeatingCircuit02Async());
            }
            else
            {
                return BadRequest(new ApiError($"Invalid pump ID: {pumpId}"));
            }
        }

        [HttpGet("boiler-loading-pump-status")]
        [ProducesResponseType(typeof(BoolValue), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetBoilerLoadingPumpStatusAsync()
        {
            return await RepositoryResponseAsync(() => _schneidReadRepository.GetBoilerLoadingPumpStatusAsync());
        }

        [HttpGet("buffer-loading-pump-status")]
        [ProducesResponseType(typeof(BoolValue), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetBufferLoadingPumpStatusAsync()
        {
            return await RepositoryResponseAsync(() => _schneidReadRepository.GetBufferLoadingPumpStatusAsync());
        }

        private async Task<IActionResult> RepositoryResponseAsync<T>(Func<Task<RepositoryResult<T>>> action)
        {
            var result = await action();
            if (!result.IsSuccess)
            {
                return BadRequest(new ApiError(result.ErrorMessage));
            }

            return Ok(result.Value);
        }
    }
}
