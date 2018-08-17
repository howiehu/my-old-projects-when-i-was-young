using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Web.UI.WebControls;

namespace HKRSoft.IntegratedQueryModule
{
    /// <summary>
    /// [综合查询模块]字段结构对象
    /// </summary>
    public class IQMDataColumn
    {
        /// <summary>
        /// [综合查询模块]字段结构对象（初始化时字段类型为文本类型）
        /// </summary>
        public IQMDataColumn()
        {
            this.Name = string.Empty;
            this.Alias = string.Empty;
            this.ColumnType = string.Empty;
            this.IsSumColumn = false;
        }

        /// <summary>
        /// 字段名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 字段别名（用于显示的名称）
        /// </summary>
        public string Alias { get; set; }
        /// <summary>
        /// 字段类型
        /// </summary>
        public string ColumnType { get; set; }
        /// <summary>
        /// 是否为汇总字段
        /// </summary>
        public bool IsSumColumn { get; set; }
        /// <summary>
        /// 字段内容别名对象列表
        /// </summary>
        public List<IQMDataValueAlias> ValueAlias { get; set; }
        /// <summary>
        /// 文本对齐属性
        /// </summary>
        public string TextAlign { get; set; }
    }
}