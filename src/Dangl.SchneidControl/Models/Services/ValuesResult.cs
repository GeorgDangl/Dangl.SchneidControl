using Dangl.SchneidControl.Models.Controllers.Values;

namespace Dangl.SchneidControl.Models.Services
{
    public class ValuesResult
    {
        public DecimalValue? BufferTemperatureTop { get; set; }

        public DecimalValue? CurrentHeatingPowerDraw { get; set; }
    }
}
