using System.ComponentModel.DataAnnotations;

namespace Dangl.SchneidControl.Models.Controllers.Values
{
    public class EnumValue<T> where T: Enum
    {
        [Required]
        public T Value { get; set; }
    }
}
