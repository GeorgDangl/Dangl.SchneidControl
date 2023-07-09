namespace Dangl.SchneidControl.Services
{
    public class DataLoggingScheduler : IHostedService
    {
        private const int SAVE_INTERVAL_MINUTES = 5;
        private readonly IServiceProvider _serviceProvider;
        private bool _isRunning = false;

        public DataLoggingScheduler(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _isRunning = true;
            Task.Run(async () =>
            {
                DateTime lastStart;
                while (_isRunning)
                {
                    lastStart = DateTime.UtcNow;
                    using var scope = _serviceProvider.CreateScope();
                    var dataLoggingService = scope.ServiceProvider.GetRequiredService<IDataLoggingService>();
                    await dataLoggingService.ReadAndSaveValuesAsync();

                    var nextExecution = lastStart.AddMinutes(SAVE_INTERVAL_MINUTES);
                    var timeToDelay = Convert.ToInt32((nextExecution - DateTime.UtcNow).TotalMilliseconds);
                    if (timeToDelay > 0)
                    {
                        await Task.Delay(timeToDelay);
                    }
                }
            });

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _isRunning = false;
            return Task.CompletedTask;
        }
    }
}
