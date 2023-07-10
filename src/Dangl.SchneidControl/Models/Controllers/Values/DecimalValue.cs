using System.ComponentModel.DataAnnotations;

namespace Dangl.SchneidControl.Models.Controllers.Values
{
    public class DecimalValue
    {
        [Required]
        public decimal Value { get; set; }

        [Required]
        public string Unit { get; set; }
    }
}
