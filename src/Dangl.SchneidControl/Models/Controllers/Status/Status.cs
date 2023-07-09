using System.ComponentModel.DataAnnotations;

namespace Dangl.SchneidControl.Models.Controllers.Status
{
    public class Status
    {
        [Required]
        public bool StatsEnabled { get; set; }

        [Required]
        public string Environment { get; set; }

        [Required]
        public bool IsHealthy { get; set; }
    }
}
