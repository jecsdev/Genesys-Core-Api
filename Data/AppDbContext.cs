using Genesis_Core_Api.Models;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Genesis_Core_Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<Affiliate> Affiliates { get; set; }
        public DbSet<Dependent> Dependents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Company
            modelBuilder.Entity<Company>()
                .HasIndex(c => c.Name)
                .IsUnique();

            // Affiliate
            modelBuilder.Entity<Affiliate>()
                .HasIndex(a => a.AffiliateNumber)
                .IsUnique();

            modelBuilder.Entity<Affiliate>()
                .HasIndex(a => a.Identification)
                .IsUnique();

            modelBuilder.Entity<Affiliate>()
                .HasOne<Company>()
                .WithMany()
                .HasForeignKey(a => a.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            // Dependent
            modelBuilder.Entity<Dependent>()
                .HasIndex(d => d.Identification)
                .IsUnique();

            modelBuilder.Entity<Dependent>()
                .HasOne(d => d.Affiliate)
                .WithMany(a => a.Dependents)
                .HasForeignKey(d => d.AffiliateId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
