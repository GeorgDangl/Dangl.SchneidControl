using System.ComponentModel.DataAnnotations;

namespace Dangl.SchneidControl.Models.Controllers.Consumption
{
    public class ConsumptionDataEntry
    {
        [Required]
        public DateTime CreatedAtUtc { get; set; }

        [Required]
        public decimal Value { get; set; }
    }
}
