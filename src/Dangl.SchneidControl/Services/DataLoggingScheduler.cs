using Dangl.SchneidControl.Configuration;
using Dangl.SchneidControl.Data;

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
                DateTime? lastEmailSent = null;
                while (_isRunning)
                {
                    lastStart = DateTime.UtcNow;
                    using var scope = _serviceProvider.CreateScope();
                    var dataLoggingService = scope.ServiceProvider.GetRequiredService<IDataLoggingService>();
                    var valuesResult = await dataLoggingService.ReadAndSaveValuesAsync();
                    if (valuesResult.BufferTemperatureTop?.Value > 50 && valuesResult.BufferTemperatureTop?.Unit == "°C" && valuesResult.CurrentHeatingPowerDraw?.Value != 0)
                    {
                        var schneidControlSettings = _configuration.Get<SchneidControlSettings>();
                        if (schneidControlSettings?.EmailRecipients?.Count > 0)
                        {
                            if (lastEmailSent == null || ((DateTime.UtcNow - lastEmailSent.Value) > TimeSpan.FromHours(12)))
                            {
                                var emailSender = scope.ServiceProvider.GetRequiredService<IEmailSender>();
                                var emailLoggingService = scope.ServiceProvider.GetRequiredService<IEmailLoggingService>();
                                foreach (var emailRecipient in schneidControlSettings.EmailRecipients)
                                {
                                    await emailSender.SendEmailAsync(emailRecipient,
                                        "Too high temperature.",
                                        "The temperature is too high and this message is really informative.");
                                    await emailLoggingService.SaveInformationAboutSentEmailAsync(EmailType.Warning, emailRecipient);
                                }

                                lastEmailSent = DateTime.UtcNow;
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
