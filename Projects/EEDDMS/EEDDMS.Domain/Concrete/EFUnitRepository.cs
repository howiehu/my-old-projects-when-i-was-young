using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EEDDMS.Domain.Abstract;
using EEDDMS.Domain.Entities;
using System.Data;

namespace EEDDMS.Domain.Concrete
{
    public class EFUnitRepository : IUnitRepository
    {
        private EFDbContext context = new EFDbContext();

        public IQueryable<Unit> Units
        {
            get { return context.Units; }
        }

        public void SaveUnit(Unit unit)
        {
            Guid? parentId = unit.ParentId;

            if (unit.Id == Guid.Empty)
            {
                List<Unit> list = this.ReSortSiblingEntitis(parentId).ToList();

                //将序号排到已有实体之后
                if (list.Count > 0)
                {
                    unit.SN = list.Last().SN;

                    unit.SN++;
                }

                context.Units.Add(unit);
            }
            else
            {
                unit.ModifyDate = DateTime.Now;

                context.Entry(unit).State = EntityState.Modified;
            }

            context.SaveChanges();
        }

        public void DeleteUnit(Unit unit)
        {
            Guid? parentId = unit.ParentId;

            //反转删除元素树
            List<Unit> list = new List<Unit>();

            this.AddToDeleteList(unit, list);

            list.Reverse();

            list.ForEach(e => context.Units.Remove(e));

            context.SaveChanges();

            //同级节点重新排序
            this.ReSortSiblingEntitis(parentId);

            context.SaveChanges();
        }

        /// <summary>
        /// 在同级元素中上下移动
        /// </summary>
        /// <param name="id">需要移动的单位信息Id</param>
        /// <param name="isMoveDown">是否向下移动</param>
        public void MoveUpAndDownUnit(Guid id, bool isMoveDown)
        {
            Unit unit = context.Units.FirstOrDefault(i => i.Id == id);

            List<Unit> list = unit.ParentId.HasValue ?
                context.Units.Where(c => c.ParentId == unit.ParentId).ToList() :
                context.Units.Where(c => c.ParentId == null).ToList();

            int siblingCount = list.Count;

            if (siblingCount > 1)
            {
                Unit sibilngItem = new Unit();

                int result = siblingCount - unit.SN;

                if (!isMoveDown)
                {
                    if (result >= 1)
                    {
                        sibilngItem = list.Find(c => c.SN == unit.SN - 1);
                        sibilngItem.SN++;

                        unit.SN--;
                    }
                }
                else
                {
                    if (result == siblingCount || result > 1)
                    {
                        sibilngItem = list.Find(c => c.SN == unit.SN + 1);
                        sibilngItem.SN--;

                        unit.SN++;
                    }
                }

                context.Entry(unit).State = EntityState.Modified;
                context.Entry(sibilngItem).State = EntityState.Modified;

                context.SaveChanges();
            }
        }

        /// <summary>
        /// 同级实体重新排序
        /// </summary>
        /// <param name="parentId">父实体ID</param>
        /// <returns>已经重新进行排序的实体List</returns>
        private IList<Unit> ReSortSiblingEntitis(Guid? parentId)
        {

            List<Unit> list = parentId.HasValue ?
                context.Units.Where(e => e.ParentId == parentId).OrderBy(e => e.SN).ToList() :
                context.Units.Where(e => e.ParentId == null).OrderBy(e => e.SN).ToList();

            if (list.Count > 0)
            {
                this.ReSort(list);
            }
            return list;
        }

        /// <summary>
        /// 同级重新排序
        /// </summary>
        /// <param name="equipmentClassList">需要重新排序的实体列表</param>
        private void ReSort(IList<Unit> unitList)
        {
            //先按序号排序
            List<Unit> list = unitList.OrderBy(e => e.SN).ToList();

            //迭代重新设置序号
            int sn = 0;
            foreach (Unit item in list)
            {
                //如果原序号与新序号不相等则赋予新序号并改变其编辑状态
                if (item.SN != sn)
                {
                    item.SN = sn;
                    context.Entry(item).State = EntityState.Modified;
                }

                sn++;
            }
        }

        private void AddToDeleteList(Unit unit, IList<Unit> list)
        {
            list.Add(unit);

            if (unit.Children.Count > 0)
            {
                foreach (Unit item in unit.Children)
                {
                    this.AddToDeleteList(item, list);
                }
            }
        }
    }
}
