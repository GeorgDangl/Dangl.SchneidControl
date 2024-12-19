using Dangl.SchneidControl.Models.Controllers.Values;
using Dangl.SchneidControl.Models.Enums;

namespace Dangl.SchneidControl.Models.Services
{
    public class ValuesResult
    {
        public DecimalValue? BufferTemperatureTop { get; set; }

        public EnumValue<TransferStationStatus>? TransferStationStatus { get; set; }
    }
}
