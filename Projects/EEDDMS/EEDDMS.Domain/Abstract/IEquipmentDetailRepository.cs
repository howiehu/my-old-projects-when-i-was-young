using System.Linq;
using EEDDMS.Domain.Entities;

namespace EEDDMS.Domain.Abstract
{
    public interface IEquipmentDetailRepository
    {
        IQueryable<EquipmentDetail> EquipmentDetails { get; }

        void SaveEquipmentDetail(EquipmentDetail equipmentDetail);

        void RecycleEquipmentDetail(EquipmentDetail equipmentDetail);

        void DeleteEquipmentDetail(EquipmentDetail equipmentDetail);
    }
}
