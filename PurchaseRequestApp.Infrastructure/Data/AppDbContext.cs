using Microsoft.EntityFrameworkCore;
using PurchaseRequestApp.Domain.Entities;

namespace PurchaseRequestApp.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

        // DbSets
        public DbSet<PurchaseRequest> PurchaseRequests { get; set; }
        public DbSet<PurchaseRequestItem> PurchaseRequestItems { get; set; }
        public DbSet<CompanyMaster> CompanyMasters { get; set; }
        public DbSet<DepartmentMaster> DepartmentMasters { get; set; }
        public DbSet<ItemMaster> ItemMasters { get; set; }
        public DbSet<MakeMaster> MakeMasters { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PurchaseRequest>(entity =>
            {
                entity.Property(p => p.DocumentNo).HasMaxLength(30).IsRequired();
                entity.Property(p => p.IndentType).HasMaxLength(20);
                entity.Property(p => p.ChargeType).HasMaxLength(20);
                entity.Property(p => p.RequestedBy).HasMaxLength(100);
                entity.Property(p => p.IndentTags).HasMaxLength(500);
            });

            modelBuilder.Entity<PurchaseRequestItem>(entity =>
            {
                entity.Property(p => p.Qty).HasColumnType("decimal(10,3)");
                entity.Property(p => p.Rate).HasColumnType("decimal(15,2)");
            });

            modelBuilder.Entity<CompanyMaster>().Property(c => c.Name).HasMaxLength(100);
            modelBuilder.Entity<DepartmentMaster>().Property(d => d.Name).HasMaxLength(100);
            modelBuilder.Entity<ItemMaster>().Property(i => i.ItemName).HasMaxLength(100);
            modelBuilder.Entity<ItemMaster>().Property(i => i.ItemCode).HasMaxLength(50);
            modelBuilder.Entity<MakeMaster>().Property(m => m.Name).HasMaxLength(100);
        }
    }
}
