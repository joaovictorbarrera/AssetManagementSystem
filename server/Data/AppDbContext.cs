using Microsoft.EntityFrameworkCore;
using ThreatlockerAssetManagementSystem.Models.Entities;

namespace ThreatlockerAssetManagementSystem.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Asset> Assets => Set<Asset>();
        public DbSet<User> Users => Set<User>();
        public DbSet<CheckoutRequest> CheckoutRequests => Set<CheckoutRequest>();
        public DbSet<AssetHistory> AssetHistories => Set<AssetHistory>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureAsset(modelBuilder);
            ConfigureUser(modelBuilder);
            ConfigureCheckoutRequest(modelBuilder);
            ConfigureAssetHistory(modelBuilder);
        }

        private static void ConfigureAsset(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Asset>(entity =>
            {
                entity.HasKey(a => a.Id);

                entity.HasIndex(a => a.AssetTag)
                    .IsUnique();

                entity.Property(a => a.Category)
                    .HasConversion<string>();

                entity.Property(a => a.Status)
                    .HasConversion<string>();

                entity.Property(a => a.Condition)
                    .HasConversion<string>();

                entity.HasOne(a => a.AssignedToUser)
                    .WithMany(u => u.AssignedAssets)
                    .HasForeignKey(a => a.AssignedToUserId);

                entity.HasData(SeedData.Assets);
            });
        }

        private static void ConfigureUser(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);

                entity.HasIndex(u => u.EmailAddress)
                    .IsUnique();

                entity.Property(u => u.Role)
                    .HasConversion<string>();

                entity.HasData(SeedData.Users);
            });
        }

        private static void ConfigureCheckoutRequest(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CheckoutRequest>(entity =>
            {
                entity.HasKey(c => c.Id);

                entity.Property(c => c.AssetCategory)
                    .HasConversion<string>();

                entity.Property(c => c.Status)
                    .HasConversion<string>();

                entity.HasOne(u => u.RequestedByUser)
                    .WithMany(u => u.RequestedCheckoutRequests)
                    .HasForeignKey(c => c.RequestedByUserId);

                entity.HasOne(u => u.ReviewedByUser)
                    .WithMany()
                    .HasForeignKey(c => c.ReviewedByUserId);

                entity.HasOne(c => c.AssignedAsset)
                    .WithMany()
                    .HasForeignKey(c => c.AssignedAssetId);

                entity.HasData(SeedData.CheckoutRequests);
            });
        }

        private static void ConfigureAssetHistory(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AssetHistory>(entity =>
            {
                entity.HasKey(h => h.Id);

                entity.HasOne(h => h.Asset)
                    .WithMany(a => a.HistoryEntries)
                    .HasForeignKey(h => h.AssetId);

                entity.HasOne(h => h.User)
                    .WithMany()
                    .HasForeignKey(h => h.UserId);
            });
        }
    }
}