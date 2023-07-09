using Dangl.SchneidControl.Models.Controllers.Status;
using Dangl.SchneidControl.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Dangl.SchneidControl.Controllers
{
    [ApiController]
    [Route("/api/status")]
    public class StatusController : ControllerBase
    {
        private readonly StatsEnabledService _statsEnabledService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public StatusController(StatsEnabledService statsEnabledService,
            IWebHostEnvironment webHostEnvironment)
        {
            _statsEnabledService = statsEnabledService;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet("")]
        [ProducesResponseType(typeof(Status), (int)HttpStatusCode.OK)]
        public IActionResult GetStatus()
        {
            return Ok(new Status
            {
                Environment = _webHostEnvironment.EnvironmentName,
                IsHealthy = true,
                StatsEnabled = _statsEnabledService.CheckIfStatsAreEnabled()
            });
        }
    }
}
