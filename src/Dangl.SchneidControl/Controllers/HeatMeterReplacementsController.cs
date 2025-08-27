using Dangl.SchneidControl.Data;
using Dangl.SchneidControl.Models.Controllers.HeatMeterReplacements;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Dangl.SchneidControl.Controllers
{
    [ApiController]
    [Route("/api/heat-meter-replacements")]
    public class HeatMeterReplacementsController : ControllerBase
    {
        private readonly DataLoggingContext _context;

        public HeatMeterReplacementsController(DataLoggingContext context)
        {
            _context = context;
        }

        [HttpGet("")]
        [ProducesResponseType(typeof(List<HeatMeterReplacementViewModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetHeatMeterReplacementsAsync()
        {
            var heatMeterReplacements = await _context
                .HeatMeterReplacements
                .OrderByDescending(e => e.ReplacedAtUtc)
                .Select(e => new HeatMeterReplacementViewModel
                {
                    Id = e.Id,
                    InitialValue = e.InitialValue / 10,
                    OldMeterValue = e.OldMeterValue / 10,
                    ReplacedAtUtc = e.ReplacedAtUtc
                })
                .ToListAsync();
            return Ok(heatMeterReplacements);
        }

        [HttpDelete("")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteHeatMeterReplacemenetsAsync(long heatMeterReplacementId)
        {
            var existingEntry = await _context
                .HeatMeterReplacements
                .FirstOrDefaultAsync(e => e.Id == heatMeterReplacementId);
            if (existingEntry == null)
            {
                return BadRequest();
            }

            _context.HeatMeterReplacements.Remove(existingEntry!);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost("")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> CreateHeatMeterReplacemenetAsync([FromBody] CreateHeatMeterReplacement createModel)
        {
            _context.HeatMeterReplacements.Add(new HeatMeterReplacement
            {
                InitialValue = createModel.InitialValue * 10,
                OldMeterValue = createModel.OldMeterValue * 10  ,
                ReplacedAtUtc = createModel.ReplacedAtUtc
            });

            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
