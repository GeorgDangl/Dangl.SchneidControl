using Microsoft.EntityFrameworkCore;

namespace Dangl.SchneidControl.Data
{
    public class DataEntry
    {
        public long Id { get; set; }

        public DateTime CreatedAtUtc { get; set; }

        public LogEntryType LogEntryType { get; set; }

        public int Value { get; set; }

        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DataEntry>()
                .HasIndex(p => p.LogEntryType);

            modelBuilder.Entity<DataEntry>()
                .HasIndex(p => p.CreatedAtUtc);

            modelBuilder.Entity<DataEntry>()
                .Property(p => p.LogEntryType)
                .HasConversion<string>();
        }
    }
}
