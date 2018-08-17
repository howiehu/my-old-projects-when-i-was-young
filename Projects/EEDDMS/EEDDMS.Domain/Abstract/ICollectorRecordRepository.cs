using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EEDDMS.Domain.Entities;

namespace EEDDMS.Domain.Abstract
{
    public interface ICollectorRecordRepository
    {
        IQueryable<CollectorRecord> CollectorRecords { get; }

        void SaveCollectorRecord(CollectorRecord collectorRecord);

        void DeleteCollectorRecord(CollectorRecord collectorRecord);
    }
}
