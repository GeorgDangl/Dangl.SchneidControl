using Microsoft.EntityFrameworkCore;

namespace Dangl.SchneidControl.Data
{
    public class EmailEntry
    {
        public long Id { get; set; }

        public DateTime CreatedAtUtc { get; set; }

        public EmailType EmailType { get; set; }

        public string Recipient { get; set; }

        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EmailEntry>()
                .HasIndex(p => p.EmailType);

            modelBuilder.Entity<EmailEntry>()
                .HasIndex(p => p.CreatedAtUtc);

            modelBuilder.Entity<EmailEntry>()
                .Property(p => p.EmailType)
                .HasConversion<string>();
        }
    }
}
