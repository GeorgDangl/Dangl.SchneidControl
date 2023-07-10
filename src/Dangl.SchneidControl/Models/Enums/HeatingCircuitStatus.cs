namespace Dangl.SchneidControl.Models.Enums
{
    public enum HeatingCircuitStatus
    {
        OffOrFrostControl = 0,

        Heating = 1,

        ResidualHeat = 2,

        Lowering = 3,

        WarmWaterSubordinate = 4,

        FrostControl = 5,

        Locked = 6,

        Manual = 7,

        BakeOut = 8,

        Voltage = 9,

        Cooling = 10
    }
}
