using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EEDDMS.Domain.Abstract;
using EEDDMS.Domain.Entities;
using System.Data;

namespace EEDDMS.Domain.Concrete
{
    public class EFCollectorRecordRepository : ICollectorRecordRepository
    {
        private EFDbContext context = new EFDbContext();

        public IQueryable<CollectorRecord> CollectorRecords
        {
            get { return context.CollectorRecords; }
        }

        public void SaveCollectorRecord(CollectorRecord collectorRecord)
        {
            if (collectorRecord.Id == Guid.Empty)
            {
                context.CollectorRecords.Add(collectorRecord);
            }
            else
            {
                collectorRecord.ModifyDate = DateTime.Now;

                context.Entry(collectorRecord).State = EntityState.Modified;
            }

            context.SaveChanges();
        }

        public void DeleteCollectorRecord(CollectorRecord collectorRecord)
        {
            context.CollectorRecords.Remove(collectorRecord);

            context.SaveChanges();
        }
    }
}
