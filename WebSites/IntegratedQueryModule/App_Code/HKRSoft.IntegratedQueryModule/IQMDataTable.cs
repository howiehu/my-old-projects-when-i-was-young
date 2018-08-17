using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace HKRSoft.IntegratedQueryModule
{
    /// <summary>
    /// [综合查询模块]表结构对象
    /// </summary>
    public class IQMDataTable
    {
        /// <summary>
        /// [综合查询模块]表结构对象
        /// </summary>
        /// <param name="name">表名称</param>
        /// <param name="sourceName">数据源名称</param>
        /// <param name="sourceType">数据源类型</param>
        public IQMDataTable(string name, string sourceName, IQMSourceType sourceType)
        {
            this.Name = name;
            this.SourceName = sourceName;
            this.SourceType = sourceType;
            this.DataColumns = new List<IQMDataColumn>();
        }
        /// <summary>
        /// [综合查询模块]表结构对象（初始化时指定字段结构对象列表）
        /// </summary>
        /// <param name="name">表名称</param>
        /// <param name="sourceName">数据源名称</param>
        /// <param name="sourceType">数据源类型</param>
        /// <param name="dbColumns">字段结构对象列表</param>
        public IQMDataTable(string name, string sourceName, IQMSourceType sourceType, List<IQMDataColumn> dbColumns)
        {
            this.Name = name;
            this.SourceName = sourceName;
            this.SourceType = sourceType;
            this.DataColumns = dbColumns;
        }

        /// <summary>
        /// 表名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 数据源名称
        /// </summary>
        public string SourceName { get; set; }
        /// <summary>
        /// 数据源类型
        /// </summary>
        public IQMSourceType SourceType { get; set; }
        /// <summary>
        /// 数据查询用的WHERE子句
        /// </summary>
        public string SqlWhereClause { get; set; }
        /// <summary>
        /// 数据查询用的GROUPBY子句
        /// </summary>
        public string SqlGroupByClause { get; set; }
        /// <summary>
        /// 字段结构对象列表
        /// </summary>
        public List<IQMDataColumn> DataColumns { get; set; }

        /// <summary>
        /// 数据源类型枚举
        /// </summary>
        public enum IQMSourceType
        {
            /// <summary>
            /// 表
            /// </summary>
            Table,
            /// <summary>
            /// 视图
            /// </summary>
            View,
            /// <summary>
            /// 函数
            /// </summary>
            Function
        }
    }
}