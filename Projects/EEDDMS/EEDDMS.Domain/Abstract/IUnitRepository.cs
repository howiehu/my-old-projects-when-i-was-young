using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EEDDMS.Domain.Entities;

namespace EEDDMS.Domain.Abstract
{
    public interface IUnitRepository
    {
        IQueryable<Unit> Units { get; }

        void SaveUnit(Unit unit);

        void DeleteUnit(Unit unit);

        void MoveUpAndDownUnit(Guid id, bool isMoveDown);
    }
}
