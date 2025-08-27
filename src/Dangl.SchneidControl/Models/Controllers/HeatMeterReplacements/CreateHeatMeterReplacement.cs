namespace Dangl.SchneidControl.Models.Controllers.HeatMeterReplacements
{
    public class CreateHeatMeterReplacement
    {
        public DateTime ReplacedAtUtc { get; set; }

        public int InitialValue { get; set; }

        public int OldMeterValue { get; set; }
    }
}
