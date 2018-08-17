using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EEDDMS.Domain.Entities;
using EEDDMS.Domain.Abstract;
using System.Data;

namespace EEDDMS.Domain.Concrete
{
    public class EFEquipmentRepository : IEquipmentRepository
    {
        private EFDbContext context = new EFDbContext();

        public IQueryable<Equipment> Equipments
        {
            get { return context.Equipments; }
        }

        public void SaveEquipment(Equipment equipment)
        {
            if (equipment.Id == Guid.Empty)
            {
                context.Equipments.Add(equipment);
            }
            else
            {
                equipment.ModifyDate = DateTime.Now;

                context.Entry(equipment).State = EntityState.Modified;
            }

            context.SaveChanges();
        }

        public void DeleteEquipment(Equipment equipment)
        {
            context.Equipments.Remove(equipment);

            context.SaveChanges();
        }
    }
}
