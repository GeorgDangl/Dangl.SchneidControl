using Dangl.SchneidControl.Configuration;
using Dangl.SchneidControl.Data;
using Dangl.SchneidControl.Models.Enums;

namespace Dangl.SchneidControl.Services
{
    public class DataLoggingScheduler : IHostedService
    {
        private const int SAVE_INTERVAL_MINUTES = 5;
        private readonly IServiceProvider _serviceProvider;
        private bool _isRunning = false;
        private readonly IConfiguration _configuration;

        public DataLoggingScheduler(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _serviceProvider = serviceProvider;
            _configuration = configuration;
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
                    var schneidControlSettings = _configuration.Get<SchneidControlSettings>();
                    using var scope = _serviceProvider.CreateScope();
                    var dataLoggingService = scope.ServiceProvider.GetRequiredService<IDataLoggingService>();
                    var valuesResult = await dataLoggingService.ReadAndSaveValuesAsync();
                    if (valuesResult.BufferTemperatureTop?.Value > schneidControlSettings?.MainBufferMaximumTemperature &&
                        valuesResult.TransferStationStatus?.Value != TransferStationStatus.OffOrFrostControl &&
                        valuesResult.TransferStationStatus?.Value != TransferStationStatus.Maintenance)
                    {
                        if (schneidControlSettings?.EmailRecipients?.Count > 0)
                        {
                            var emailSender = scope.ServiceProvider.GetRequiredService<IEmailSender>();
                            var emailLoggingService = scope.ServiceProvider.GetRequiredService<IEmailLoggingService>();
                            foreach (var emailRecipient in schneidControlSettings.EmailRecipients)
                            {
                                var lastEmailSentTime = await emailLoggingService.GetTimeOfLastSentEmailAsync(EmailType.LowTemperatureWarning, emailRecipient);
                                if (lastEmailSentTime == null || ((DateTime.UtcNow - lastEmailSentTime.Value) > TimeSpan.FromHours(12)))
                                {
                                    await emailLoggingService.SaveInformationAboutSentEmailAsync(EmailType.LowTemperatureWarning, emailRecipient);
                                    // TODO: add proper subject and message texts
                                    await emailSender.SendEmailAsync(emailRecipient,
                                        "Too low temperature.",
                                        "The temperature is too low and this message is really informative.");
                                }
                            }
                        }
                        else
                        {
                            // TODO: should we do anything in this case?
                        }
                    }

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
