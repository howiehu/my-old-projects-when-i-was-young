using System.Data;
using System.ComponentModel;

namespace HKRSoft.IntegratedQueryModule
{
    /// <summary>
    /// 接口：数据分页读取统一接口
    /// </summary>
    interface ILoadAndPagingByRowNumber
    {
        DataSet GetData(string sqlWhereClause, string sqlGroupByClause, int startRowIndex, int maximumRows);
        int GetDataCount(string sqlWhereClause, string sqlGroupByClause);
    }
}