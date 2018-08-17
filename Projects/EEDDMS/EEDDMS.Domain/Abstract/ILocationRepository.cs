using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EEDDMS.Domain.Entities;

namespace EEDDMS.Domain.Abstract
{
    public interface ILocationRepository
    {
        IQueryable<Location> Locations { get; }

        void SaveLocation(Location location);

        void DeleteLocation(Location location);

        IQueryable<Location> QuickQuery(string queryCondition);
    }
}
