using Dangl.SchneidControl.Data;
using Microsoft.EntityFrameworkCore;

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

        public async Task<DateTime?> GetTimeOfLastSentEmailAsync(EmailType emailType, string recipient)
        {
            var emailEntry = await _context.EmailEntries
                .Where(x => x.EmailType == emailType && x.Recipient == recipient)
                .OrderByDescending(x => x.CreatedAtUtc)
                .FirstOrDefaultAsync();

            return emailEntry?.CreatedAtUtc;
        }
    }
}
