using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EEDDMS.Domain.Abstract;
using EEDDMS.Domain.Entities;
using System.Data;

namespace EEDDMS.Domain.Concrete
{
    public class EFCollectorRepository : ICollectorRepository
    {
        private EFDbContext context = new EFDbContext();

        public IQueryable<Collector> Collectors
        {
            get { return context.Collectors; }
        }

        public void SaveCollector(Collector collector)
        {
            if (collector.Id == Guid.Empty)
            {
                collector.Id = Guid.NewGuid();
                context.Collectors.Add(collector);
            }
            else
            {
                collector.ModifyDate = DateTime.Now;

                context.Entry(collector).State = EntityState.Modified;
            }

            //寻找是否存在采集器记录（按照起始时间反向排序取第一个返回结果）
            CollectorRecord record = context.CollectorRecords
                    .Where(c => c.CollectorId == collector.Id && c.EndDate == null)
                    .FirstOrDefault();

            if (collector.EquipmentId == null)
            {
                if (record != null) //如果采集器没有绑定设备ID，但找到了采集器记录信息，则说明是解除绑定操作
                {
                    record.EndDate = DateTime.Now;

                    context.Entry(record).State = EntityState.Modified;
                }
            }
            else
            {
                if (record != null)
                {
                    if (record.EquipmentId != collector.EquipmentId) //如果采集器有绑定设备ID，且找到了采集器记录信息，同时两者的绑定设备ID不同，则说明是变更绑定操作
                    {
                        record.EndDate = DateTime.Now;

                        context.Entry(record).State = EntityState.Modified;

                        context.CollectorRecords.Add(new CollectorRecord
                        {
                            CollectorId = collector.Id,
                            EquipmentId = collector.EquipmentId,
                            StartDate = DateTime.Now
                        });
                    }
                }
                else //如果采集器有绑定设备ID，但没找到采集器记录信息，则说明是新增采集器的绑定操作
                {
                    context.CollectorRecords.Add(new CollectorRecord
                    {
                        CollectorId = collector.Id,
                        EquipmentId = collector.EquipmentId,
                        StartDate = DateTime.Now
                    });
                }
            }

            context.SaveChanges();
        }

        public void DeleteCollector(Collector collector)
        {
            context.Collectors.Remove(collector);

            context.SaveChanges();
        }
    }
}
