
using Microsoft.EntityFrameworkCore;
using VeariumTraders.Models;

namespace VeariumTraders.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<BulkEnquiry> BulkEnquiries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BulkEnquiry>()
                .Property(x => x.RequiredQuantity)
                .HasPrecision(18, 2);
        }
    }
}
