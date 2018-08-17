using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EEDDMS.Domain.Abstract;
using EEDDMS.Domain.Entities;
using System.Data;

namespace EEDDMS.Domain.Concrete
{
    public class EFEquipmentDetailRepository:IEquipmentDetailRepository
    {
        private EFDbContext context = new EFDbContext();

        public IQueryable<EquipmentDetail> EquipmentDetails
        {
            get { return context.EquipmentDetails; }
        }

        public void SaveEquipmentDetail(EquipmentDetail equipmentDetail)
        {
            if (equipmentDetail.Id == Guid.Empty)
            {
                context.EquipmentDetails.Add(equipmentDetail);
            }
            else
            {
                equipmentDetail.ModifyDate = DateTime.Now;

                context.Entry(equipmentDetail).State = EntityState.Modified;
            }

            context.SaveChanges();
        }

        public void RecycleEquipmentDetail(EquipmentDetail equipmentDetail)
        {
            equipmentDetail.IsDeleted = true;
            equipmentDetail.DeleteDate = DateTime.Now;

            context.Entry(equipmentDetail).State = EntityState.Modified;

            context.SaveChanges();
        }

        public void DeleteEquipmentDetail(EquipmentDetail equipmentDetail)
        {
            context.EquipmentDetails.Remove(equipmentDetail);

            context.SaveChanges();
        }
    }
}
