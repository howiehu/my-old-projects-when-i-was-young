using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EEDDMS.Domain.Entities;
using EEDDMS.Domain.Abstract;
using System.Data;

namespace EEDDMS.Domain.Concrete
{
    public class EFEquipmentRecordRepository : IEquipmentRecordRepository
    {
        private EFDbContext context = new EFDbContext();

        public IQueryable<EquipmentRecord> EquipmentRecords
        {
            get { return context.EquipmentRecords; }
        }

        public void SaveEquipmentRecord(EquipmentRecord equipmentRecord)
        {
            if (equipmentRecord.Id == Guid.Empty)
            {
                context.EquipmentRecords.Add(equipmentRecord);
            }
            else
            {
                equipmentRecord.ModifyDate = DateTime.Now;

                context.Entry(equipmentRecord).State = EntityState.Modified;
            }

            context.SaveChanges();
        }

        public void DeleteEquipmentRecord(EquipmentRecord equipmentRecord)
        {
            context.EquipmentRecords.Remove(equipmentRecord);

            context.SaveChanges();
        }
    }
}
