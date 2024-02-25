using Dangl.SchneidControl.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Dangl.SchneidControl.Models.Controllers.Consumption
{
    public class Consumption
    {
        [Required]
        public string Unit { get; set; }

        public DateTime? StartUtc { get; set; }

        public DateTime? EndUtc { get; set; }

        [Required]
        public ConsumptionResolution Resolution { get; set; }

        [Required]
        public List<ConsumptionDataEntry> Entries { get; set; }
    }
}
