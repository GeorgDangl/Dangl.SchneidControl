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

        [HttpGet("excel")]
        [ProducesResponseType(typeof(FileStreamResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetExcelAsync(DateTime? startTime, DateTime? endTime, LogEntryType logEntryType)
        {
            var repoResult = await _statsRepository.ExportToExcelAsync(startTime, endTime, logEntryType);
            if (!repoResult.IsSuccess)
            {
                return BadRequest(new ApiError(repoResult.ErrorMessage));
            }

            return File(repoResult.Value.Stream, repoResult.Value.MimeType, repoResult.Value.FileName);
        }

        [HttpGet("csv")]
        [ProducesResponseType(typeof(FileStreamResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetCsvAsync(DateTime? startTime, DateTime? endTime, LogEntryType logEntryType)
        {
            var repoResult = await _statsRepository.ExportToCsvAsync(startTime, endTime, logEntryType);
            if (!repoResult.IsSuccess)
            {
                return BadRequest(new ApiError(repoResult.ErrorMessage));
            }

            return File(repoResult.Value.Stream, repoResult.Value.MimeType, repoResult.Value.FileName);
        }
    }
}
