using System.Data.Entity;
using EEDDMS.Domain.Entities;

namespace EEDDMS.Domain.Concrete
{
    public class EFDbContext : DbContext
    {
        public DbSet<Manufacturer> Manufacturers { get; set; }

        public DbSet<EquipmentClass> EquipmentClasses { get; set; }

        public DbSet<Equipment> Equipments { get; set; }

        public DbSet<EquipmentDetail> EquipmentDetails { get; set; }

        public DbSet<EquipmentRecord> EquipmentRecords { get; set; }

        public DbSet<Collector> Collectors { get; set; }

        public DbSet<CollectorRecord> CollectorRecords { get; set; }

        public DbSet<Location> Locations { get; set; }

        public DbSet<Unit> Units { get; set; }
    }
}
