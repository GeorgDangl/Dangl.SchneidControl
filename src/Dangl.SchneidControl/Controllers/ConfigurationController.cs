using Dangl.Data.Shared;
using Dangl.SchneidControl.Models.Enums;
using Dangl.SchneidControl.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Dangl.SchneidControl.Controllers
{
    [ApiController]
    [Route("/api/config")]
    public class ConfigurationController : ControllerBase
    {
        private readonly ISchneidWriteRepository _schneidWriteRepository;

        public ConfigurationController(ISchneidWriteRepository schneidWriteRepository)
        {
            _schneidWriteRepository = schneidWriteRepository;
        }

        [HttpPost("")]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> SetTransferStationModeAsync([FromQuery] TransferStationStatus transferStationMode)
        {
            if (!Enum.IsDefined(transferStationMode))
            {
                return BadRequest(new ApiError($"Invalid enum value given for {nameof(transferStationMode)}"));
            }

            var repoResult = await _schneidWriteRepository.SetTransferStationModeAsync(transferStationMode);
            if (!repoResult.IsSuccess)
            {
                return BadRequest(new ApiError(repoResult.ErrorMessage));
            }

            return NoContent();
        }
    }
}
