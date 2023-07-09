using CsvHelper.Configuration.Attributes;

namespace Dangl.SchneidControl.Models.Stats
{
    public class CsvOutputEntry
    {
        [Name("Datum"), Index(0)]
        public DateTime DateUtc { get; set; }

        [Name("Wert"), Index(1)]
        public decimal Value { get; set; }
    }
}
