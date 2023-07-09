using Dangl.Data.Shared;
using Dangl.SchneidControl.Data;
using Dangl.SchneidControl.Models.Controllers.Stats;
using Dangl.SchneidControl.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Dangl.SchneidControl.Controllers
{
    [ApiController]
    [Route("/api/stats")]
    public class StatsController : ControllerBase
    {
        private readonly IStatsRepository _statsRepository;

        public StatsController(IStatsRepository statsRepository)
        {
            _statsRepository = statsRepository;
        }

        [HttpGet("")]
        [ProducesResponseType(typeof(Stats), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetStatsAsync(DateTime? startTime, DateTime? endTime, LogEntryType logEntryType)
        {
            var actual = await _statsRepository.GetStatsAsync(startTime, endTime, logEntryType);
            if (!actual.IsSuccess)
            {
                return BadRequest(new ApiError(actual.ErrorMessage));
            }

            return Ok(actual.Value);
        }
    }
}
