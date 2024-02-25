using Dangl.SchneidControl.Models.Enums;

namespace Dangl.SchneidControl.Extensions
{
    public static class ConsumptionResolutionExtensions
    {
        public static DateTime GetStartOfResolution(this DateTime currentDate, ConsumptionResolution resolution)
        {
            switch (resolution)
            {
                case ConsumptionResolution.Hourly:
                    return currentDate.ToNextHour().AddHours(-1);

                case ConsumptionResolution.Daily:
                    return currentDate.ToNextDay().AddDays(-1);

                case ConsumptionResolution.Weekly:
                    return currentDate.ToNextWeek().AddDays(-7);

                case ConsumptionResolution.Monthly:
                    return currentDate.ToNextMonth().AddMonths(-1);

                case ConsumptionResolution.Yearly:
                    return currentDate.ToNextYear().AddYears(-1);

                default:
                    throw new ArgumentOutOfRangeException(nameof(resolution), resolution, null);
            }
        }

        public static DateTime GetEndOfResolution(this DateTime currentDate, ConsumptionResolution resolution)
        {
            switch (resolution)
            {
                case ConsumptionResolution.Hourly:
                    return currentDate.ToNextHour();

                case ConsumptionResolution.Daily:
                    return currentDate.ToNextDay();

                case ConsumptionResolution.Weekly:
                    return currentDate.ToNextWeek();

                case ConsumptionResolution.Monthly:
                    return currentDate.ToNextMonth();

                case ConsumptionResolution.Yearly:
                    return currentDate.ToNextYear();

                default:
                    throw new ArgumentOutOfRangeException(nameof(resolution), resolution, null);
            }
        }

        private static DateTime ToNextHour(this DateTime currentDate)
        {
            if (currentDate.Nanosecond != 0)
            {
                currentDate = currentDate.AddMicroseconds((1000 - currentDate.Nanosecond) * 0.001);
            }

            if (currentDate.Microsecond != 0)
            {
                currentDate = currentDate.AddMicroseconds(1000 - currentDate.Microsecond);
            }

            if (currentDate.Millisecond != 0)
            {
                currentDate = currentDate.AddMilliseconds(1000 - currentDate.Millisecond);
            }

            if (currentDate.Second != 0)
            {
                currentDate = currentDate.AddSeconds(60 - currentDate.Second);
            }

            if (currentDate.Minute != 0)
            {
                currentDate = currentDate.AddMinutes(60 - currentDate.Minute);
            }
            else
            {
                currentDate = currentDate.AddHours(1);
            }

            return currentDate;
        }

        private static DateTime ToNextDay(this DateTime currentDate)
        {
            currentDate = currentDate.ToNextHour();
            if (currentDate.Hour != 0)
            {
                currentDate = currentDate.AddHours(24 - currentDate.Hour);
            }
            else
            {
                currentDate = currentDate.AddDays(1);
            }

            return currentDate;
        }

        private static DateTime ToNextWeek(this DateTime currentDate)
        {
            currentDate = currentDate.ToNextDay();

            while (currentDate.DayOfWeek != DayOfWeek.Monday)
            {
                currentDate = currentDate.ToNextDay();
            }

            return currentDate;
        }

        private static DateTime ToNextMonth(this DateTime currentDate)
        {
            currentDate = currentDate.ToNextDay();

            while (currentDate.Day != 1)
            {
                currentDate = currentDate.ToNextDay();
            }

            return currentDate;
        }

        private static DateTime ToNextYear(this DateTime currentDate)
        {
            currentDate = currentDate.ToNextMonth();

            while (currentDate.Month != 1)
            {
                currentDate = currentDate.ToNextMonth();
            }

            return currentDate;
        }
    }
}
