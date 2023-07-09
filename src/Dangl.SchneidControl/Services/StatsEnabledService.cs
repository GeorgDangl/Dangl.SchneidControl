namespace Dangl.SchneidControl.Services
{
    public class StatsEnabledService
    {
        private readonly bool _statsAreEnabled;

        public StatsEnabledService(bool statsAreEnabled)
        {
            _statsAreEnabled = statsAreEnabled;
        }

        public bool CheckIfStatsAreEnabled()
        {
            return _statsAreEnabled;
        }
    }
}
