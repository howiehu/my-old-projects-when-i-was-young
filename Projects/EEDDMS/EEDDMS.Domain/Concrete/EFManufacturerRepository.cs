using System;
using System.Data;
using System.Linq;
using EEDDMS.Domain.Abstract;
using EEDDMS.Domain.Entities;

namespace EEDDMS.Domain.Concrete
{
    public class EFManufacturerRepository : IManufacturerRepository
    {
        private EFDbContext context = new EFDbContext();

        public IQueryable<Manufacturer> Manufacturers
        {
            get { return context.Manufacturers; }
        }

        public void SaveManufacturer(Manufacturer manufacturer)
        {
            if (manufacturer.Id == Guid.Empty)
            {
                context.Manufacturers.Add(manufacturer);
            }
            else
            {
                manufacturer.ModifyDate = DateTime.Now;

                context.Entry(manufacturer).State = EntityState.Modified;
            }

            context.SaveChanges();
        }

        public void DeleteManufacturer(Manufacturer manufacturer)
        {
            context.Manufacturers.Remove(manufacturer);

            context.SaveChanges();
        }
    }
}
