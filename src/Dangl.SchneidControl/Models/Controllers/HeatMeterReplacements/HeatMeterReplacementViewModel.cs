namespace Dangl.SchneidControl.Models.Controllers.HeatMeterReplacements
{
    public class HeatMeterReplacementViewModel
    {
        public long Id { get; set; }
        public DateTime ReplacedAtUtc { get; set; }
        public int InitialValue { get; set; }
        public int OldMeterValue { get; set; }
    }
}
