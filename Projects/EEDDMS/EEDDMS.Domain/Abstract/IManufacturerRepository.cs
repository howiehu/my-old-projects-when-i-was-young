using System.Linq;
using EEDDMS.Domain.Entities;

namespace EEDDMS.Domain.Abstract
{
    public interface IManufacturerRepository
    {
        IQueryable<Manufacturer> Manufacturers { get; }

        void SaveManufacturer(Manufacturer manufacturer);

        void DeleteManufacturer(Manufacturer manufacturer);
    }
}
