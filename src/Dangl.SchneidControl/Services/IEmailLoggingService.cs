using Dangl.SchneidControl.Data;

namespace Dangl.SchneidControl.Services
{
    public interface IEmailLoggingService
    {
        Task SaveInformationAboutSentEmailAsync(EmailType emailType, string recipient);
    }
}
