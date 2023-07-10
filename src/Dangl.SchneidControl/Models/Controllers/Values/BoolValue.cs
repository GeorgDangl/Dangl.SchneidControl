using System.ComponentModel.DataAnnotations;

namespace Dangl.SchneidControl.Models.Controllers.Values
{
    public class BoolValue
    {
        [Required]
        public bool Value { get; set; }
    }
}
