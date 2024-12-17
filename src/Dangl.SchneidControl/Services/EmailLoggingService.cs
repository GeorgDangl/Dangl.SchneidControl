using Dangl.SchneidControl.Data;

namespace Dangl.SchneidControl.Services
{
    public class EmailLoggingService : IEmailLoggingService
    {
        private readonly DataLoggingContext _context;

        public EmailLoggingService(DataLoggingContext context)
        {
            _context = context;
        }

        public async Task SaveInformationAboutSentEmailAsync(EmailType emailType, string recipient)
        {
            await _context.EmailEntries.AddAsync(new EmailEntry
            {
                CreatedAtUtc = DateTime.UtcNow,
                EmailType = emailType,
                Recipient = recipient
            });

            await _context.SaveChangesAsync();
        }
    }
}
