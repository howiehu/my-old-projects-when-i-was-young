using System.Linq;
using EEDDMS.Domain.Entities;
using System;

namespace EEDDMS.Domain.Abstract
{
    public interface IEquipmentClassRepository
    {
        IQueryable<EquipmentClass> EquipmentClasses { get; }

        void SaveEquipmentClass(EquipmentClass equipmentClass);

        void DeleteEquipmentClass(EquipmentClass equipmentClass);

        void MoveUpAndDownEquipmentClass(Guid id, bool isMoveDown);
    }
}
