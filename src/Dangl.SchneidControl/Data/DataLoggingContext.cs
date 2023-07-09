using Microsoft.EntityFrameworkCore;

namespace Dangl.SchneidControl.Data
{
    public class DataLoggingContext : DbContext
    {
        public DbSet<DataEntry> DataEntries { get; set; }

        public DataLoggingContext(DbContextOptions<DataLoggingContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            DataEntry.OnModelCreating(builder);
        }
    }
}
