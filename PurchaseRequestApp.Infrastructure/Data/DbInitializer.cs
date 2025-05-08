using PurchaseRequestApp.Domain.Entities;

namespace PurchaseRequestApp.Infrastructure.Data
{
    public static class DbInitializer
    {
        public static void Seed(AppDbContext context)
        {
            if (!context.CompanyMasters.Any())
            {
                context.CompanyMasters.AddRange(
                    new CompanyMaster { Id = Guid.NewGuid(), Name = "Global Tech Pvt Ltd" },
                    new CompanyMaster { Id = Guid.NewGuid(), Name = "Innova Solutions" }
                );
            }

            if (!context.DepartmentMasters.Any())
            {
                context.DepartmentMasters.AddRange(
                    new DepartmentMaster { Id = Guid.NewGuid(), Name = "Engineering" },
                    new DepartmentMaster { Id = Guid.NewGuid(), Name = "IT Support" },
                    new DepartmentMaster { Id = Guid.NewGuid(), Name = "Operations" }
                );
            }

            if (!context.ItemMasters.Any())
            {
                context.ItemMasters.AddRange(
                    new ItemMaster { Id = Guid.NewGuid(), ItemCode = "ITM001", ItemName = "Laptop", UOM = "Nos" },
                    new ItemMaster { Id = Guid.NewGuid(), ItemCode = "ITM002", ItemName = "Monitor", UOM = "Nos" },
                    new ItemMaster { Id = Guid.NewGuid(), ItemCode = "ITM003", ItemName = "Mouse", UOM = "Nos" }
                );
            }

            if (!context.MakeMasters.Any())
            {
                context.MakeMasters.AddRange(
                    new MakeMaster { Id = Guid.NewGuid(), Name = "Dell" },
                    new MakeMaster { Id = Guid.NewGuid(), Name = "HP" },
                    new MakeMaster { Id = Guid.NewGuid(), Name = "Logitech" }
                );
            }

            context.SaveChanges();
        }
    }
}
