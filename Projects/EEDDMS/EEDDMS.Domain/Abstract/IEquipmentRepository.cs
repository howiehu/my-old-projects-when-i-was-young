using System.Linq;
using EEDDMS.Domain.Entities;

namespace EEDDMS.Domain.Abstract
{
    public interface IEquipmentRepository
    {
        IQueryable<Equipment> Equipments { get; }

        void SaveEquipment(Equipment equipment);

        void DeleteEquipment(Equipment equipment);
    }
}
