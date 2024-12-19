using Dangl.SchneidControl.Configuration;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

namespace Dangl.SchneidControl.Services
{
    public class EmailSender : IEmailSender
    {
        public EmailSender(IOptions<SchneidControlSettings> optionsAccessor,
            ILoggerFactory loggerFactory)
        {
            _smtpSettings = optionsAccessor.Value.SmtpSettings;
            _logger = loggerFactory.CreateLogger<EmailSender>();
        }

        private readonly SmtpSettings _smtpSettings;
        private readonly ILogger _logger;

        public async Task SendEmailAsync(string email, string subject, string htmlBody)
        {
            if (!EmailSettingsCorrect())
            {
                _logger.LogError("The SMTP settings for email sending are not configured correctly, can not send email" +
                    Environment.NewLine + "Recipient: " + email +
                    Environment.NewLine + "Subject: " + subject);
                return;
            }

            var message = new MimeMessage();
            message.Subject = subject;
            message.To.Add(MailboxAddress.Parse(email));
            message.From.Add(new MailboxAddress(_smtpSettings.FromName, _smtpSettings.FromAddress));
            message.Body = new TextPart(TextFormat.Html)
            {
                Text = htmlBody
            };
            using (var smtpClient = new SmtpClient())
            {
                if (_smtpSettings.IgnoreTlsCertificateErrors)
                {
                    smtpClient.ServerCertificateValidationCallback = (s, c, h, e) => true;
                }

                await smtpClient.ConnectAsync(_smtpSettings.ServerAddress, _smtpSettings.ServerPort, _smtpSettings.UseTls);
                if (_smtpSettings.RequiresAuthentication)
                {
                    await smtpClient.AuthenticateAsync(_smtpSettings.Username, _smtpSettings.Password);
                }
                await smtpClient.SendAsync(message);
            }
        }

        private bool EmailSettingsCorrect()
        {
            var emailSettingsCorrect = !string.IsNullOrWhiteSpace(_smtpSettings?.ServerAddress)
                && _smtpSettings?.ServerPort != 0;
            if (!emailSettingsCorrect)
            {
                return false;
            }

            if (!_smtpSettings?.RequiresAuthentication ?? false)
            {
                return true;
            }

            return !string.IsNullOrWhiteSpace(_smtpSettings?.Username)
                && !string.IsNullOrWhiteSpace(_smtpSettings?.Password);
        }
    }
}
