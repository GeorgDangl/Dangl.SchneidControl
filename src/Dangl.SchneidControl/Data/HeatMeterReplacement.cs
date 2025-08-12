namespace Dangl.SchneidControl.Data
{
    public class HeatMeterReplacement
    {
        public long Id { get; set; }

        public DateTime ReplacedAtUtc { get; set; }

        public int OldMeterValue { get; set; }

        public int InitialValue { get; set; }
    }
}
