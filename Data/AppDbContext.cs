using Genesis_Core_Api.Models;
using Microsoft.EntityFrameworkCore;

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
        public DbSet<User> Users { get; set; }
        public DbSet<ServicePlan> ServicePlans { get; set; }
        public DbSet<PlanBenefit> PlanBenefits { get; set; }
        public DbSet<AffiliatePayment> AffiliatePayments { get; set; }

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
                .HasOne(a => a.Company)
                .WithMany(c => c.Affiliates)
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

            // User
            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .HasConversion<string>();

            // ServicePlan → PlanBenefit
            modelBuilder.Entity<PlanBenefit>()
                .HasOne(b => b.ServicePlan)
                .WithMany(p => p.Benefits)
                .HasForeignKey(b => b.ServicePlanId)
                .OnDelete(DeleteBehavior.Cascade);

            // ServicePlan → Affiliate
            modelBuilder.Entity<Affiliate>()
                .HasOne(a => a.ServicePlan)
                .WithMany(p => p.Affiliates)
                .HasForeignKey(a => a.ServicePlanId)
                .OnDelete(DeleteBehavior.SetNull);

            // AffiliatePayment — precisión decimal
            modelBuilder.Entity<AffiliatePayment>()
                .Property(p => p.Amount)
                .HasPrecision(18, 2);

            // AffiliatePayment → Affiliate
            modelBuilder.Entity<AffiliatePayment>()
                .HasOne(p => p.Affiliate)
                .WithMany(a => a.Payments)
                .HasForeignKey(p => p.AffiliateId)
                .OnDelete(DeleteBehavior.Cascade);

            // ServicePlan — precisión decimales
            modelBuilder.Entity<ServicePlan>()
                .Property(p => p.BasePrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<ServicePlan>()
                .Property(p => p.ExtraDependentPrice)
                .HasPrecision(18, 2);

            // PaymentStatus como string
            modelBuilder.Entity<AffiliatePayment>()
                .Property(p => p.Status)
                .HasConversion<string>();
        }
    }
}