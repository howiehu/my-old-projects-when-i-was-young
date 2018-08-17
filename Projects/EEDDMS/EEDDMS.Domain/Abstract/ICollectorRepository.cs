using System.Linq;
using EEDDMS.Domain.Entities;

namespace EEDDMS.Domain.Abstract
{
    public interface ICollectorRepository
    {
        IQueryable<Collector> Collectors { get; }

        void SaveCollector(Collector collector);

        void DeleteCollector(Collector collector);
    }
}
