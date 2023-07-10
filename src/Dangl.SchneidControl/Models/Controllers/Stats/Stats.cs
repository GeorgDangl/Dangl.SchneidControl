using Dangl.SchneidControl.Data;
using System.ComponentModel.DataAnnotations;

namespace Dangl.SchneidControl.Models.Controllers.Stats
{
    public class Stats
    {
        [Required]
        public string Unit { get; set; }

        [Required]
        public LogEntryType LogEntryType { get; set; }

        public DateTime? StartUtc { get; set; }

        public DateTime? EndUtc { get; set; }

        [Required]
        public List<DataEntry> Entries { get; set; }
    }
}
