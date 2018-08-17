using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EEDDMS.Domain.Entities;

namespace EEDDMS.Domain.Abstract
{
    public interface IEquipmentRecordRepository
    {
        IQueryable<EquipmentRecord> EquipmentRecords { get; }

        void SaveEquipmentRecord(EquipmentRecord equipmentRecord);

        void DeleteEquipmentRecord(EquipmentRecord equipmentRecord);
    }
}
