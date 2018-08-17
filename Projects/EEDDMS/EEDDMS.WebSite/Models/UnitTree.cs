using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EEDDMS.Domain.Entities;
using EEDDMS.Domain.Abstract;

namespace EEDDMS.WebSite.Models
{
    /// <summary>
    /// 单位信息树状结构
    /// </summary>
    public class UnitTree
    {
        private IUnitRepository repository;

        public UnitTree(IUnitRepository repo)
        {
            this.repository = repo;
            this.UnitTreeNodeNodes = new HashSet<UnitTreeNode>();
            this.FillUnitNodes();
        }

        public ICollection<UnitTreeNode> UnitTreeNodeNodes { get; set; }

        /// <summary>
        /// 填充EquipmentClassNodes属性集合
        /// </summary>
        private void FillUnitNodes()
        {
            foreach (Unit item in this.repository.Units.Where(c => c.ParentId == null).OrderBy(c => c.SN).ToList())
            {
                int levelNumber = 0;
                string wbsNumber = string.Empty;
                this.CreateUnitNode(wbsNumber, levelNumber, item);
            }
        }

        /// <summary>
        /// 创建EquipmentClassTreeNode并添加至EquipmentClassNodes属性集合
        /// </summary>
        /// <param name="parentWbsNumber">父节点Wbs序号</param>
        /// <param name="levelNumber">层号</param>
        /// <param name="unit">设备分类实体对象</param>
        private void CreateUnitNode(string parentWbsNumber, int levelNumber, Unit unit)
        {
            UnitTreeNode node = new UnitTreeNode { LevelNumber = levelNumber, Unit = unit };

            int siblingCount; //用于保存同级节点数量

            //根据层级判断是否为根节点，并生成节点层号和Wbs序号
            if (levelNumber == 0)
            {
                node.LevelNumber = 0;
                node.WbsNumber = (unit.SN + 1).ToString();

                siblingCount = this.repository.Units.Where(c => c.ParentId == null).ToList().Count;
            }
            else
            {
                node.LevelNumber = levelNumber;
                node.WbsNumber = parentWbsNumber + "." + (unit.SN + 1).ToString();

                siblingCount = this.repository.Units.Where(c => c.ParentId == unit.ParentId).ToList().Count;
            }

            //根据同级节点数量和设备分类实体对象的序号判断在同级节点中的位置
            if (siblingCount > 1)
            {
                int result = siblingCount - unit.SN;

                if (result == siblingCount)
                {
                    node.IsFirstOrLastNode = NodeSiblingPosition.First;
                }
                else if (result == 1)
                {
                    node.IsFirstOrLastNode = NodeSiblingPosition.Last;
                }
                else if (result > 1)
                {
                    node.IsFirstOrLastNode = NodeSiblingPosition.Middle;
                }
            }
            else
            {
                node.IsFirstOrLastNode = NodeSiblingPosition.Only;
            }

            //将节点加入树状结构
            this.UnitTreeNodeNodes.Add(node);

            //如果存在子元素则按照子元素序号顺序递归生成相应子节点
            if (unit.Children.Count != 0)
            {
                unit.Children.OrderBy(c => c.SN).ToList().ForEach(c => this.CreateUnitNode(node.WbsNumber, node.LevelNumber + 1, c));
            }
        }
    }

    /// <summary>
    /// 单位信息树状节点
    /// </summary>
    public class UnitTreeNode : TreeNode
    {
        /// <summary>
        /// 设备分类
        /// </summary>
        public Unit Unit { get; set; }
    }
}