using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EEDDMS.Domain.Abstract;
using EEDDMS.Domain.Entities;
using System.Data;

namespace EEDDMS.Domain.Concrete
{
    public class EFEquipmentClassRepository : IEquipmentClassRepository
    {
        private EFDbContext context = new EFDbContext();

        public IQueryable<EquipmentClass> EquipmentClasses
        {
            get { return context.EquipmentClasses; }
        }

        public void SaveEquipmentClass(EquipmentClass equipmentClass)
        {
            Guid? parentId = equipmentClass.ParentId;

            if (equipmentClass.Id == Guid.Empty)
            {
                List<EquipmentClass> list = this.ReSortSiblingEntitis(parentId).ToList();

                //将序号排到已有实体之后
                if (list.Count > 0)
                {
                    equipmentClass.SN = list.Last().SN;

                    equipmentClass.SN++;
                }

                context.EquipmentClasses.Add(equipmentClass);
            }
            else
            {
                equipmentClass.ModifyDate = DateTime.Now;

                context.Entry(equipmentClass).State = EntityState.Modified;
            }

            context.SaveChanges();
        }

        public void DeleteEquipmentClass(EquipmentClass equipmentClass)
        {
            Guid? parentId = equipmentClass.ParentId;

            //反转删除元素树
            List<EquipmentClass> list = new List<EquipmentClass>();

            this.AddToDeleteList(equipmentClass, list);

            list.Reverse();

            list.ForEach(e => context.EquipmentClasses.Remove(e));

            context.SaveChanges();

            //同级节点重新排序
            this.ReSortSiblingEntitis(parentId);

            context.SaveChanges();
        }

        /// <summary>
        /// 在同级元素中上下移动
        /// </summary>
        /// <param name="id">需要移动的设备分类Id</param>
        /// <param name="isMoveDown">是否向下移动</param>
        public void MoveUpAndDownEquipmentClass(Guid id, bool isMoveDown)
        {
            EquipmentClass equipmentClass = context.EquipmentClasses.FirstOrDefault(i => i.Id == id);

            List<EquipmentClass> list = equipmentClass.ParentId.HasValue ?
                context.EquipmentClasses.Where(c => c.ParentId == equipmentClass.ParentId).ToList() :
                context.EquipmentClasses.Where(c => c.ParentId == null).ToList();

            int siblingCount = list.Count;

            if (siblingCount > 1)
            {
                EquipmentClass sibilngItem = new EquipmentClass();

                int result = siblingCount - equipmentClass.SN;

                if (!isMoveDown)
                {
                    if (result >= 1)
                    {
                        sibilngItem = list.Find(c => c.SN == equipmentClass.SN - 1);
                        sibilngItem.SN++;

                        equipmentClass.SN--;
                    }
                }
                else
                {
                    if (result == siblingCount || result > 1)
                    {
                        sibilngItem = list.Find(c => c.SN == equipmentClass.SN + 1);
                        sibilngItem.SN--;

                        equipmentClass.SN++;
                    }
                }

                context.Entry(equipmentClass).State = EntityState.Modified;
                context.Entry(sibilngItem).State = EntityState.Modified;

                context.SaveChanges();
            }
        }

        /// <summary>
        /// 同级实体重新排序
        /// </summary>
        /// <param name="parentId">父实体ID</param>
        /// <returns>已经重新进行排序的实体List</returns>
        private IList<EquipmentClass> ReSortSiblingEntitis(Guid? parentId)
        {

            List<EquipmentClass> list = parentId.HasValue ?
                context.EquipmentClasses.Where(e => e.ParentId == parentId).OrderBy(e => e.SN).ToList() :
                context.EquipmentClasses.Where(e => e.ParentId == null).OrderBy(e => e.SN).ToList();

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
        private void ReSort(IList<EquipmentClass> equipmentClassList)
        {
            //先按序号排序
            List<EquipmentClass> list = equipmentClassList.OrderBy(e => e.SN).ToList();

            //迭代重新设置序号
            int sn = 0;
            foreach (EquipmentClass item in list)
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

        private void AddToDeleteList(EquipmentClass equipmentClass, IList<EquipmentClass> list)
        {
            list.Add(equipmentClass);

            if (equipmentClass.Children.Count > 0)
            {
                foreach (EquipmentClass item in equipmentClass.Children)
                {
                    this.AddToDeleteList(item, list);
                }
            }
        }
    }
}
