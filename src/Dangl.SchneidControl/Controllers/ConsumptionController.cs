using Dangl.Data.Shared;
using Dangl.SchneidControl.Models.Controllers.Consumption;
using Dangl.SchneidControl.Models.Enums;
using Dangl.SchneidControl.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Dangl.SchneidControl.Controllers
{
    [ApiController]
    [Route("/api/consumption")]
    public class ConsumptionController : ControllerBase
    {
        private readonly IConsumptionRepository _consumptionRepository;

        public ConsumptionController(IConsumptionRepository consumptionRepository)
        {
            _consumptionRepository = consumptionRepository;
        }

        [HttpGet("")]
        [ProducesResponseType(typeof(Consumption), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetStatsAsync(DateTime? startTime,
            DateTime? endTime,
            ConsumptionResolution resolution,
            int utcTimeZoneOffset = 0)
        {
            var actual = await _consumptionRepository
                .GetConsumptionAsync(startTime,
                endTime,
                resolution,
                utcTimeZoneOffset);
            if (!actual.IsSuccess)
            {
                return BadRequest(new ApiError(actual.ErrorMessage));
            }

            return Ok(actual.Value);
        }
    }
}
