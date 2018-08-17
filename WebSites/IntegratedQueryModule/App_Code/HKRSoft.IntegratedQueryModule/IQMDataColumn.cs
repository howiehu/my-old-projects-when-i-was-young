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
        /// <param name="name">字段名</param>
        /// <param name="alias">字段别名</param>
        public IQMDataColumn(string name, string alias)
        {
            this.Name = name;
            this.Alias = alias;
            this.ColumnType = IQMDataColumnType.Text;
            this.ValueAlias = new List<IQMDataValueAlias>();
            this.IsSumColumn = false;
            this.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.NotSet;
        }

        public IQMDataColumn(string name, string alias, HorizontalAlign horizontalAlign)
        {
            this.Name = name;
            this.Alias = alias;
            this.ColumnType = IQMDataColumnType.Text;
            this.ValueAlias = new List<IQMDataValueAlias>();
            this.IsSumColumn = false;
            this.HorizontalAlign = horizontalAlign;
        }

        public IQMDataColumn(string name, string alias, IQMDataValueUrl valueUrl, HorizontalAlign horizontalAlign)
        {
            this.Name = name;
            this.Alias = alias;
            this.ColumnType = IQMDataColumnType.Text;
            this.ValueAlias = new List<IQMDataValueAlias>();
            this.IsSumColumn = false;
            this.HorizontalAlign = horizontalAlign;
            this.ValueUrl = valueUrl;
        }
        /// <summary>
        /// [综合查询模块]字段结构对象（初始化时字段类型为文本类型并指定是否为汇总字段）
        /// </summary>
        /// <param name="name">字段名</param>
        /// <param name="alias">字段别名</param>
        /// <param name="isSumColumn">是否为汇总字段</param>
        public IQMDataColumn(string name, string alias, bool isSumColumn)
        {
            this.Name = name;
            this.Alias = alias;
            this.ColumnType = IQMDataColumnType.Text;
            this.ValueAlias = new List<IQMDataValueAlias>();
            this.IsSumColumn = isSumColumn;
            this.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.NotSet;
        }

        public IQMDataColumn(string name, string alias, HorizontalAlign horizontalAlign, bool isSumColumn)
        {
            this.Name = name;
            this.Alias = alias;
            this.ColumnType = IQMDataColumnType.Text;
            this.ValueAlias = new List<IQMDataValueAlias>();
            this.IsSumColumn = isSumColumn;
            this.HorizontalAlign = horizontalAlign;
        }
        /// <summary>
        /// [综合查询模块]字段结构对象（初始化时指定字段类型）
        /// </summary>
        /// <param name="name">字段名</param>
        /// <param name="alias">字段别名</param>
        /// <param name="fieldType">字段类型</param>
        public IQMDataColumn(string name, string alias, IQMDataColumnType fieldType)
        {
            this.Name = name;
            this.Alias = alias;
            this.ColumnType = fieldType;
            this.ValueAlias = new List<IQMDataValueAlias>();
            switch (fieldType)
            {
                case IQMDataColumnType.Text:
                    this.IsSumColumn = false;
                    this.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.NotSet;
                    break;
                case IQMDataColumnType.Date:
                    this.IsSumColumn = false;
                    this.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                    break;
                case IQMDataColumnType.Money:
                    this.IsSumColumn = true;
                    this.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
                    break;
                case IQMDataColumnType.Number:
                    this.IsSumColumn = true;
                    this.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
                    break;
                case IQMDataColumnType.YearOrMonth:
                    this.IsSumColumn = false;
                    this.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                    break;
                case IQMDataColumnType.ValueAlias:
                    this.IsSumColumn = false;
                    this.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                    break;
            }
        }
        public IQMDataColumn(string name, string alias, IQMDataValueUrl valueUrl, IQMDataColumnType fieldType)
        {
            this.Name = name;
            this.Alias = alias;
            this.ColumnType = fieldType;
            this.ValueAlias = new List<IQMDataValueAlias>();
            this.ValueUrl = valueUrl;
            switch (fieldType)
            {
                case IQMDataColumnType.Text:
                    this.IsSumColumn = false;
                    this.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.NotSet;
                    break;
                case IQMDataColumnType.Date:
                    this.IsSumColumn = false;
                    this.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                    break;
                case IQMDataColumnType.Money:
                    this.IsSumColumn = true;
                    this.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
                    break;
                case IQMDataColumnType.Number:
                    this.IsSumColumn = true;
                    this.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
                    break;
                case IQMDataColumnType.YearOrMonth:
                    this.IsSumColumn = false;
                    this.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                    break;
                case IQMDataColumnType.ValueAlias:
                    this.IsSumColumn = false;
                    this.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                    break;
            }
        }
        /// <summary>
        /// [综合查询模块]字段结构对象（初始化时指定字段内容别名）
        /// </summary>
        /// <param name="name">字段名</param>
        /// <param name="alias">字段别名</param>
        /// <param name="valueAlias">字段内容别名对象列表</param>
        public IQMDataColumn(string name, string alias, List<IQMDataValueAlias> valueAlias)
        {
            this.Name = name;
            this.Alias = alias;
            this.ColumnType = IQMDataColumnType.Text;
            this.ValueAlias = valueAlias;
            this.IsSumColumn = false;
            this.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
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
        /// 字段内容别名对象列表
        /// </summary>
        public List<IQMDataValueAlias> ValueAlias { get; set; }
        /// <summary>
        /// 字段类型
        /// </summary>
        public IQMDataColumnType ColumnType { get; set; }
        /// <summary>
        /// 是否为汇总字段
        /// </summary>
        public bool IsSumColumn { get; set; }
        /// <summary>
        /// 指定水平对齐方式
        /// </summary>
        public HorizontalAlign HorizontalAlign { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public IQMDataValueUrl ValueUrl { get; set; }

        /// <summary>
        /// 字段类型枚举
        /// </summary>
        public enum IQMDataColumnType
        {
            /// <summary>
            /// 文字类型
            /// </summary>
            Text,
            /// <summary>
            /// 日期类型
            /// </summary>
            Date,
            /// <summary>
            /// 金额类型
            /// </summary>
            Money,
            /// <summary>
            /// 内容别名类型
            /// </summary>
            ValueAlias,
            /// <summary>
            /// 纯年份或纯月份类型
            /// </summary>
            YearOrMonth,
            /// <summary>
            /// 数量类型
            /// </summary>
            Number
        }
    }
}