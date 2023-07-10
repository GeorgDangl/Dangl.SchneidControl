using System.ComponentModel.DataAnnotations;

namespace Dangl.SchneidControl.Models.Controllers.Stats
{
    public class DataEntry
    {
        [Required]
        public DateTime CreatedAtUtc { get; set; }

        [Required]
        public decimal Value { get; set; }
    }
}
