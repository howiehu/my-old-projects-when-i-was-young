using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EEDDMS.WebSite.Models
{
    /// <summary>
    /// 树状结构节点
    /// </summary>
    public abstract class TreeNode
    {
        /// <summary>
        /// 层号
        /// </summary>
        public int LevelNumber { get; set; }

        /// <summary>
        /// WBS序号
        /// </summary>
        public string WbsNumber { get; set; }

        /// <summary>
        /// 在同级节点中的位置
        /// </summary>
        public NodeSiblingPosition IsFirstOrLastNode { get; set; }
    }

    /// <summary>
    /// 同级节点位置
    /// </summary>
    public enum NodeSiblingPosition
    {
        /// <summary>
        /// 唯一
        /// </summary>
        Only,

        /// <summary>
        /// 第一个
        /// </summary>
        First,

        /// <summary>
        /// 中间
        /// </summary>
        Middle,

        /// <summary>
        /// 最后一个
        /// </summary>
        Last
    }
}