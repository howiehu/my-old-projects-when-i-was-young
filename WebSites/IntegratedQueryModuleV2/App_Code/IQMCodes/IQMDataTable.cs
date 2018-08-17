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
        public IQMDataTable()
        {
            this.ColumnNames = new List<string>();
            this.Rows = new List<IQMDataRow>();
            this.SumResults = new List<IQMDataSumResult>();
        }
        /// <summary>
        /// 
        /// </summary>
        public List<string> ColumnNames { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<IQMDataRow> Rows { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int DataTotal { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<IQMDataSumResult> SumResults { get; set; }
        
    }
}