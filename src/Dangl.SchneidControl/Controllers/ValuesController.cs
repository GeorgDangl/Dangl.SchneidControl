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
        public ValuesController(ISchneidRepository schneidRepository)
        {
            _schneidRepository = schneidRepository;
        }

        private readonly ISchneidRepository _schneidRepository;

        [HttpGet("outer-temperature")]
        [ProducesResponseType(typeof(DecimalValue), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        public Task<IActionResult> GetOuterTemperatureAsync()
        {
            return RepositoryResponseAsync(() => _schneidRepository.GetOuterTemperaturAsync());
        }

        [HttpGet("total-energy-consumption")]
        [ProducesResponseType(typeof(DecimalValue), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        public Task<IActionResult> GetTotalEnergyConsumptionAsync()
        {
            return RepositoryResponseAsync(() => _schneidRepository.GetTotalEnergyConsumptionAsync());
        }

        [HttpGet("heating-power")]
        [ProducesResponseType(typeof(DecimalValue), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        public Task<IActionResult> GetCurrentHeatingPowerAsync()
        {
            return RepositoryResponseAsync(() => _schneidRepository.GetCurrentHeatingPowerDrawAsync());
        }

        [HttpGet("advance-temperature")]
        [ProducesResponseType(typeof(DecimalValue), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        public Task<IActionResult> GetAdvanceTemperatureAsync()
        {
            return RepositoryResponseAsync(() => _schneidRepository.GetAdvanceTemperatureAsync());
        }

        [HttpGet("primary-reflux-temperature")]
        [ProducesResponseType(typeof(DecimalValue), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        public Task<IActionResult> GetPrimaryRefluxTemperatureAsync()
        {
            return RepositoryResponseAsync(() => _schneidRepository.GetPrimaryRefluxTemperatureAsync());
        }

        [HttpGet("secondary-reflux-temperature")]
        [ProducesResponseType(typeof(DecimalValue), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        public Task<IActionResult> GetSecondaryRefluxTemperatureAsync()
        {
            return RepositoryResponseAsync(() => _schneidRepository.GetSecondaryRefluxTemperatureAsync());
        }

        [HttpGet("valve-opening")]
        [ProducesResponseType(typeof(DecimalValue), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        public Task<IActionResult> GetValveOpeningAsync()
        {
            return RepositoryResponseAsync(() => _schneidRepository.GetValveOpeningAsync());
        }

        [HttpGet("transfer-station-status")]
        [ProducesResponseType(typeof(EnumValue<TransferStationStatus>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        public Task<IActionResult> GetTransferStationStatusAsync()
        {
            return RepositoryResponseAsync(() => _schneidRepository.GetTransferStationStatusAsync());
        }

        [HttpGet("buffer-temperature-top")]
        [ProducesResponseType(typeof(DecimalValue), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        public Task<IActionResult> GetBufferTemperatureTopAsync()
        {
            return RepositoryResponseAsync(() => _schneidRepository.GetBufferTemperatureTopAsync());
        }

        [HttpGet("buffer-temperature-bottom")]
        [ProducesResponseType(typeof(DecimalValue), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        public Task<IActionResult> GetBufferTemperatureBottomAsync()
        {
            return RepositoryResponseAsync(() => _schneidRepository.GetBufferTemperatureBottomAsync());
        }

        [HttpGet("boiler-temperature-top")]
        [ProducesResponseType(typeof(DecimalValue), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        public Task<IActionResult> GetBoilerTemperatureTopAsync()
        {
            return RepositoryResponseAsync(() => _schneidRepository.GetBoilerTemperatureTopAsync());
        }

        [HttpGet("boiler-temperature-bottom")]
        [ProducesResponseType(typeof(DecimalValue), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        public Task<IActionResult> GetBoilerTemperatureBottomAsync()
        {
            return RepositoryResponseAsync(() => _schneidRepository.GetBoilerTemperatureBottomAsync());
        }

        [HttpGet("target-buffer-temperature")]
        [ProducesResponseType(typeof(DecimalValue), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        public Task<IActionResult> GetTargetBufferTopTemperatureAsync()
        {
            return RepositoryResponseAsync(() => _schneidRepository.GetTargetBufferTopTemperatureAsync());
        }

        [HttpGet("target-boiler-temperature")]
        [ProducesResponseType(typeof(DecimalValue), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        public Task<IActionResult> GetTargetBoilerTemperatureAsync()
        {
            return RepositoryResponseAsync(() => _schneidRepository.GetTargetBoilerTemperatureAsync());
        }

        [HttpGet("circuit-status/{circuitId}")]
        [ProducesResponseType(typeof(EnumValue<HeatingCircuitStatus>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetHeatingCircuitStatusAsync(int circuitId)
        {
            if (circuitId == 0)
            {
                return await RepositoryResponseAsync(() => _schneidRepository.GetHeatingCircuitStatus00Async());
            }
            else if (circuitId == 1)
            {
                return await RepositoryResponseAsync(() => _schneidRepository.GetHeatingCircuitStatus01Async());
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
                return await RepositoryResponseAsync(() => _schneidRepository.GetPumpStatusHeatingCircuit00Async());
            }
            else if (pumpId == 1)
            {
                return await RepositoryResponseAsync(() => _schneidRepository.GetPumpStatusHeatingCircuit01Async());
            }
            else
            {
                return BadRequest(new ApiError($"Invalid pump ID: {pumpId}"));
            }
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
