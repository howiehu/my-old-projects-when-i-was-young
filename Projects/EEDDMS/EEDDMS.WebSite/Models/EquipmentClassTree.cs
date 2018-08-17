using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EEDDMS.Domain.Entities;
using EEDDMS.Domain.Abstract;

namespace EEDDMS.WebSite.Models
{
    /// <summary>
    /// 设备分类树状结构
    /// </summary>
    public class EquipmentClassTree
    {
        private IEquipmentClassRepository repository;

        public EquipmentClassTree(IEquipmentClassRepository repo)
        {
            this.repository = repo;
            this.EquipmentClassTreeNodes = new HashSet<EquipmentClassTreeNode>();
            this.FillEquipmentClassNodes();
        }

        public ICollection<EquipmentClassTreeNode> EquipmentClassTreeNodes { get; set; }

        /// <summary>
        /// 填充EquipmentClassNodes属性集合
        /// </summary>
        private void FillEquipmentClassNodes()
        {
            foreach (EquipmentClass item in this.repository.EquipmentClasses.Where(c => c.ParentId == null).OrderBy(c => c.SN).ToList())
            {
                int levelNumber = 0;
                string wbsNumber = string.Empty;
                this.CreateEquipmentClassNode(wbsNumber, levelNumber, item);
            }
        }

        /// <summary>
        /// 创建EquipmentClassTreeNode并添加至EquipmentClassNodes属性集合
        /// </summary>
        /// <param name="parentWbsNumber">父节点Wbs序号</param>
        /// <param name="levelNumber">层号</param>
        /// <param name="equipmentClass">设备分类实体对象</param>
        private void CreateEquipmentClassNode(string parentWbsNumber, int levelNumber, EquipmentClass equipmentClass)
        {
            EquipmentClassTreeNode node = new EquipmentClassTreeNode { LevelNumber = levelNumber, EquipmentClass = equipmentClass };

            int siblingCount; //用于保存同级节点数量

            //根据层级判断是否为根节点，并生成节点层号和Wbs序号
            if (levelNumber == 0)
            {
                node.LevelNumber = 0;
                node.WbsNumber = (equipmentClass.SN + 1).ToString();

                siblingCount = this.repository.EquipmentClasses.Where(c => c.ParentId == null).ToList().Count;
            }
            else
            {
                node.LevelNumber = levelNumber;
                node.WbsNumber = parentWbsNumber + "." + (equipmentClass.SN + 1).ToString();

                siblingCount = this.repository.EquipmentClasses.Where(c => c.ParentId == equipmentClass.ParentId).ToList().Count;
            }

            //根据同级节点数量和设备分类实体对象的序号判断在同级节点中的位置
            if (siblingCount > 1)
            {
                int result = siblingCount - equipmentClass.SN;

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
            this.EquipmentClassTreeNodes.Add(node);

            //如果存在子元素则按照子元素序号顺序递归生成相应子节点
            if (equipmentClass.Children.Count != 0)
            {
                equipmentClass.Children.OrderBy(c => c.SN).ToList().ForEach(c => this.CreateEquipmentClassNode(node.WbsNumber, node.LevelNumber + 1, c));
            }
        }
    }

    /// <summary>
    /// 设备分类树状节点
    /// </summary>
    public class EquipmentClassTreeNode : TreeNode
    {
        /// <summary>
        /// 设备分类
        /// </summary>
        public EquipmentClass EquipmentClass { get; set; }
    }
}