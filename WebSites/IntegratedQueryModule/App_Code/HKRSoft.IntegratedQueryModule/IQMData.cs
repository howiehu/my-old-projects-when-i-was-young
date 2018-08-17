using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.ComponentModel;
using System.Web.UI.WebControls;

namespace HKRSoft.IntegratedQueryModule
{
    /// <summary>
    /// [综合查询模块]数据对象
    /// </summary>
    public class IQMData : ILoadAndPagingByRowNumber
    {
        public IQMData()
        {

        }

        /// <summary>
        /// 表结构对象
        /// </summary>
        public IQMDataTable IQMDataTable { get; set; }

        /// <summary>
        /// 方法：读取全部数据
        /// </summary>
        /// <param name="sqlWhereClause">WHERE子句</param>
        /// <param name="sqlGroupByClause">GROUP BY子句</param>
        /// <returns>包含全部数据的DataSet</returns>
        public DataSet GetData(string sqlWhereClause, string sqlGroupByClause)
        {
            this.IQMDataTable.SqlWhereClause = sqlWhereClause;
            this.IQMDataTable.SqlGroupByClause = sqlGroupByClause;
            DataSet dataSet = IQMCore.LoadToDataSet(this.IQMDataTable);
            return dataSet;
        }

        /// <summary>
        /// 方法：根据起始行数和每页最大行数分页读取数据
        /// </summary>
        /// <param name="sqlWhereClause">WHERE子句</param>
        /// <param name="sqlGroupByClause">GROUP BY子句</param>
        /// <param name="startRowIndex">起始行数</param>
        /// <param name="maximumRows">每页最大行数</param>
        /// <returns>已经过分页的DataSet</returns>
        public DataSet GetData(string sqlWhereClause, string sqlGroupByClause, int startRowIndex, int maximumRows)
        {
            this.IQMDataTable.SqlWhereClause = sqlWhereClause;
            this.IQMDataTable.SqlGroupByClause = sqlGroupByClause;
            DataSet dataSet = IQMCore.LoadToDataSet(this.IQMDataTable, startRowIndex, maximumRows);
            return dataSet;
        }

        /// <summary>
        /// 方法：获取数据总行数
        /// </summary>
        /// <returns>数据总行数</returns>
        public int GetDataCount(string sqlWhereClause, string sqlGroupByClause)
        {
            this.IQMDataTable.SqlWhereClause = sqlWhereClause;
            this.IQMDataTable.SqlGroupByClause = sqlGroupByClause;
            int dataCount = IQMCore.LoadDataCount(this.IQMDataTable);
            return dataCount;
        }

        public List<IQMDataValueUrlRef> GetValueUrlRefList(string sqlWhereClause)
        {
            List<IQMDataValueUrlRef> refs = IQMCore.LoadDataValueUrlRefList(this.IQMDataTable, sqlWhereClause);
            return refs;
        }

        public DataSet GetSumData(string sqlWhereClause, string sqlGroupByClause)
        {
            this.IQMDataTable.SqlWhereClause = sqlWhereClause;
            this.IQMDataTable.SqlGroupByClause = sqlGroupByClause;
            DataSet dataSet = IQMCore.LoadSumData(this.IQMDataTable);
            return dataSet;
        }
    }
}