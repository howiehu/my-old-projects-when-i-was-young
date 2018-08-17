using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HKRSoft.IntegratedQueryModule
{
    /// <summary>
    /// [综合查询模块]核心功能
    /// </summary>
    public class IQMCore
    {
        public IQMCore()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //

        }

        /// <summary>
        /// 数据源类型枚举
        /// </summary>
        private enum IQMSourceType
        {
            /// <summary>
            /// 表、视图、函数
            /// </summary>
            TVF,
            /// <summary>
            /// 存储过程
            /// </summary>
            SP
        }

        /// <summary>
        /// 私有属性：获取web.config中的综合查询模块数据库连接字符串
        /// </summary>
        private static string ConnectionString
        {
            get { return ConfigurationManager.ConnectionStrings["IQMConnectionString"].ConnectionString; }
        }

        /// <summary>
        /// 私有属性：获取综合查询模块XML配置文档所在路径
        /// </summary>
        private static string IQMXmlDocumentsPatch
        {
            get { return System.Web.HttpContext.Current.Server.MapPath("~/IntegratedQueryModule/IQMXmlDocuments/"); }
        }

        private static string IQMTempPatch
        {
            get { return System.Web.HttpContext.Current.Server.MapPath("~/IntegratedQueryModule/IQMTemp/"); }
        }

        internal static IQMDataTable GetData(string xmlDocName, string startRowIndex, string maximumRows, string selectColumns, string whereClause, string groupColumns, string orderClause, string havingClause, string queryParameter)
        {
            /* ========================================== 局部变量 ========================================== */

            #region 局部变量

            string sourceName = string.Empty; ; //用于保存数据源名称
            IQMSourceType sourceType = IQMSourceType.TVF; //用于保存数据源类型
            IQMDataTable table = new IQMDataTable(); //用于此方法的返回值
            List<IQMDataColumn> tableColumns = new List<IQMDataColumn>(); //用于存放表中的全部列结构
            List<IQMDataColumn> sumColumnList = new List<IQMDataColumn>(); //用于保存合计列
            Dictionary<string, IQMDataValueUrl> columnValueUrlDict = new Dictionary<string, IQMDataValueUrl>(); //用于保存带有链接的数据的键值对

            #endregion

            /* ========================================== XML文档信息提取 ========================================== */

            #region XML文档信息提取

            //根据传入的XML文档名读取XML文档
            string xmlDocPatch = IQMXmlDocumentsPatch + xmlDocName + ".xml";
            XElement tableElms = XElement.Load(xmlDocPatch);
            XNamespace xmlns = tableElms.Name.Namespace;

            //根据XML文档配置信息填充IQMDataTable对象属性
            IEnumerable<XAttribute> tableAttrs = tableElms.Attributes();
            foreach (XAttribute attr in tableAttrs) //遍历XML文档中根元素的全部属性
            {
                switch (attr.Name.ToString())
                {
                    case "SourceName":
                        if (string.IsNullOrEmpty(queryParameter))
                        {
                            sourceName = attr.Value;
                        }
                        else
                        {
                            queryParameter = queryParameter.Substring(0, queryParameter.Length - 1);
                            sourceName = attr.Value + "(" + queryParameter + ")";
                        }
                        break;
                    case "SourceType":
                        sourceType = (IQMSourceType)Enum.Parse(typeof(IQMSourceType), attr.Value);
                        break;
                }
            }

            //在IQMDataTable的Columns中添加序号行信息
            IQMDataColumn rowNumColumn = new IQMDataColumn();
            rowNumColumn.Name = "RowNumber";
            rowNumColumn.Alias = "序号";
            rowNumColumn.ColumnType = "number";
            rowNumColumn.TextAlign = "center";
            tableColumns.Add(rowNumColumn);

            //根据XML文档配置信息填充IQMDataTable的Columns对象属性
            IEnumerable<XElement> columnElms = tableElms.Element(xmlns + "Columns").Elements();
            foreach (XElement columnElm in columnElms) //遍历XML文档中的全部Column元素
            {
                IQMDataColumn column = new IQMDataColumn();
                foreach (XAttribute attr in columnElm.Attributes()) //遍历Column元素的全部属性
                {
                    switch (attr.Name.ToString())
                    {
                        case "Name":
                            column.Name = attr.Value;
                            break;
                        case "Alias":
                            column.Alias = attr.Value;
                            break;
                        case "ColumnType":
                            column.ColumnType = attr.Value;
                            break;
                        case "IsSumColumn":
                            if (attr.Value == "0" || attr.Value.ToLower() == "false")
                            {
                                column.IsSumColumn = false;
                            }
                            else if (attr.Value == "1" || attr.Value.ToLower() == "true")
                            {
                                column.IsSumColumn = true;
                            }
                            break;
                        case "TextAlign":
                            column.TextAlign = attr.Value;
                            break;
                    }
                }
                //从Column的子元素中提取数据内容别名和数据链接信息
                if (columnElm.Elements().Count() > 0) //判断Column元素是否有子元素
                {
                    foreach (XElement childElm in columnElm.Elements()) //遍历Column元素的全部子元素
                    {
                        switch (childElm.Name.LocalName.ToString())
                        {
                            case "ValueAlias": //如果是ValueAlias元素则读取其中信息
                                List<IQMDataValueAlias> valueAliasList = new List<IQMDataValueAlias>();
                                foreach (XElement valueAliasElm in childElm.Elements())
                                {
                                    IQMDataValueAlias valueAlias = new IQMDataValueAlias();
                                    foreach (XAttribute attr in valueAliasElm.Attributes())
                                    {
                                        switch (attr.Name.ToString())
                                        {
                                            case "Value":
                                                valueAlias.Value = attr.Value;
                                                break;
                                            case "Alias":
                                                valueAlias.Alias = attr.Value;
                                                break;
                                        }
                                    }
                                    valueAliasList.Add(valueAlias);
                                }
                                column.ValueAlias = valueAliasList;
                                break;
                            case "ValueUrl": //如果是ValueUrl元素则读取其中信息
                                IQMDataValueUrl valueUrl = new IQMDataValueUrl();
                                valueUrl.Url = childElm.Attribute("Url").Value;
                                foreach (XElement valueUrlElm in childElm.Elements())
                                {
                                    valueUrl.Keys.Add(valueUrlElm.Value);
                                }
                                if (string.IsNullOrEmpty(groupColumns)) //如果是在Group条件下则不添加内容链接
                                {
                                    columnValueUrlDict.Add(childElm.Parent.Attribute("Name").Value, valueUrl);
                                }
                                break;
                        }
                    }
                }
                tableColumns.Add(column);
            }

            sumColumnList = tableColumns.FindAll(c => c.IsSumColumn); //填充合计列对象列表

            #endregion

            /* ========================================== 数据查询 ========================================== */

            #region 数据查询

            string sqlAllColumns = string.Empty;
            string sqlQueryParameters = string.Empty;
            string sqlSelectColumns = string.Empty;

            //生成含有全部列名的字符串
            foreach (IQMDataColumn column in tableColumns)
            {
                if (!column.Name.Equals("RowNumber"))
                {
                    sqlAllColumns += column.Name + ", ";
                }
            }
            sqlAllColumns = sqlAllColumns.Substring(0, sqlAllColumns.Length - 2);

            //生成链接参数字符串
            List<string> queryParameterList = new List<string>();
            foreach (KeyValuePair<string, IQMDataValueUrl> pair in columnValueUrlDict)
            {
                foreach (string key in pair.Value.Keys)
                {
                    if (!queryParameterList.Contains(key))
                    {
                        queryParameterList.Add(key);
                    }
                }
            }
            queryParameterList.ForEach(x => sqlQueryParameters += "," + x);

            //根据传入的列选择参数填充IQMDataTable的SelectColumns属性并生成Select字符串
            List<string> selectColumnsList = new List<string>();
            if (string.IsNullOrEmpty(selectColumns))
            {
                sqlSelectColumns = sqlAllColumns;
                tableColumns.ForEach(c => selectColumnsList.Add(c.Name));
            }
            else
            {
                sqlSelectColumns = selectColumns;

                selectColumnsList.Add("RowNumber");
                string[] selectColumnsArray = selectColumns.Split(new char[] { ',' });
                foreach (string columnName in selectColumnsArray)
                {
                    selectColumnsList.Add(columnName);
                }
            }
            table.ColumnNames = selectColumnsList;

            //拼接合计查询字符串
            string sumColumns = string.Empty;
            string sqlSumColumns = string.Empty;
            string sqlSumResultColumns = string.Empty;
            foreach (IQMDataColumn column in sumColumnList)
            {
                sumColumns += "," + column.Name;
                sqlSumColumns += ", SUM(" + column.Name + ") AS " + column.Name;
                sqlSumResultColumns += "SUM(" + column.Name + ") AS " + column.Name + ", ";
            }
            if (!string.IsNullOrEmpty(sqlSumResultColumns))
            {
                sqlSumResultColumns = sqlSumResultColumns.Substring(0, sqlSumResultColumns.Length - 2);
            }


            //根据传入的查询条件参数进行数据查询
            string sqlGetCount = string.Empty;
            string sqlGetData = string.Empty;
            string sqlGetSumResult = string.Empty;

            if (string.IsNullOrEmpty(startRowIndex) && string.IsNullOrEmpty(maximumRows))
            {
                #region 用于Excel导出的查询逻辑
                if (string.IsNullOrEmpty(groupColumns)) //无Group子句条件下的查询
                {
                    #region 无Group子句条件下的查询
                    if (string.IsNullOrEmpty(orderClause)) //无Order子句条件下的查询
                    {
                        #region 无Order子句条件下的查询
                        switch (sourceType)
                        {
                            case IQMSourceType.TVF:
                                sqlGetCount = "SELECT COUNT(*) FROM " + sourceName + " " + whereClause;
                                sqlGetData = "SELECT RowNumber, " + sqlSelectColumns + sqlQueryParameters + " FROM (SELECT ROW_NUMBER() OVER(ORDER BY " + sqlSelectColumns + ") AS RowNumber, " + sqlSelectColumns + sqlQueryParameters + " FROM " + sourceName + " " + whereClause + ") AS t1";
                                sqlGetSumResult = "SELECT " + sqlSumResultColumns + " FROM " + sourceName + " " + whereClause;
                                FillIQMDataTableForExcel(table, tableColumns, sumColumnList, sqlGetCount, sqlGetData, sqlGetSumResult);
                                break;
                            case IQMSourceType.SP:
                                break;
                        }
                        #endregion
                    }
                    else //有Order子句条件下的查询
                    {
                        #region 有Order子句条件下的查询
                        switch (sourceType)
                        {
                            case IQMSourceType.TVF:
                                sqlGetCount = "SELECT COUNT(*) FROM " + sourceName + " " + whereClause;
                                sqlGetData = "SELECT RowNumber, " + sqlSelectColumns + sqlQueryParameters + " FROM (SELECT TOP (100) PERCENT ROW_NUMBER() OVER(" + orderClause + ") AS RowNumber, " + sqlSelectColumns + sqlQueryParameters + " FROM " + sourceName + " " + whereClause + " " + orderClause + ")";
                                sqlGetSumResult = "SELECT " + sqlSumResultColumns + " FROM " + sourceName + " " + whereClause;
                                FillIQMDataTableForExcel(table, tableColumns, sumColumnList, sqlGetCount, sqlGetData, sqlGetSumResult);
                                break;
                            case IQMSourceType.SP:
                                break;
                        }
                        #endregion
                    }
                    #endregion
                }
                else //有Group子句条件下的查询
                {
                    #region //有Group子句条件下的查询
                    string sqlGroupColumns = groupColumns; //Group子句字符串与传入参数内容相同

                    //拼接Group条件下的Select字符串
                    sqlSelectColumns = groupColumns + sumColumns;

                    //拼接Group和Sum的完整字符串
                    string sqlGroupSumColumns = sqlGroupColumns + sqlSumColumns;

                    //根据新的Select字符串生成IQMDataTable的SelectColumns
                    selectColumnsList.Clear();
                    selectColumnsList.Add("RowNumber");
                    string[] selectColumnsArray = sqlSelectColumns.Split(new char[] { ',' });
                    foreach (string columnName in selectColumnsArray)
                    {
                        selectColumnsList.Add(columnName);
                    }

                    //根据传入参数生成不同条件下的查询字符串
                    if (string.IsNullOrEmpty(orderClause)) //无Order子句条件下的查询
                    {
                        #region 无Order子句条件下的查询
                        if (string.IsNullOrEmpty(havingClause)) //无Having子句条件下的查询
                        {
                            #region 无Having子句条件下的查询
                            switch (sourceType)
                            {
                                case IQMSourceType.TVF:
                                    sqlGetCount = "SELECT COUNT(*) FROM (SELECT " + sqlGroupColumns + " FROM " + sourceName + " " + whereClause + " GROUP BY " + sqlGroupColumns + ") AS T1";
                                    sqlGetData = "SELECT RowNumber, " + sqlSelectColumns + " FROM (SELECT ROW_NUMBER() OVER(ORDER BY " + sqlGroupColumns + ") AS RowNumber, " + sqlGroupSumColumns + " FROM " + sourceName + " " + whereClause + " GROUP BY " + sqlGroupColumns + ") AS t1";
                                    sqlGetSumResult = "SELECT " + sqlSumResultColumns + " FROM " + sourceName + " " + whereClause;
                                    FillIQMDataTableForExcel(table, tableColumns, sumColumnList, sqlGetCount, sqlGetData, sqlGetSumResult);
                                    break;
                                case IQMSourceType.SP:
                                    break;
                            }
                            #endregion
                        }
                        else //有Having子句条件下的查询
                        {
                            #region 有Having子句条件下的查询
                            switch (sourceType)
                            {
                                case IQMSourceType.TVF:
                                    sqlGetCount = "SELECT COUNT(*) FROM (SELECT " + sqlGroupColumns + " FROM " + sourceName + " " + whereClause + " GROUP BY " + sqlGroupColumns + " " + havingClause + ") AS T1";
                                    sqlGetData = "SELECT RowNumber, " + sqlSelectColumns + " FROM (SELECT ROW_NUMBER() OVER(ORDER BY " + sqlGroupColumns + ") AS RowNumber, " + sqlGroupSumColumns + " FROM " + sourceName + " " + whereClause + " GROUP BY " + sqlGroupColumns + " " + havingClause + ") AS t1";
                                    sqlGetSumResult = "SELECT " + sqlSumResultColumns + " FROM " + sourceName + " " + whereClause + " GROUP BY " + sqlGroupColumns + " " + havingClause;
                                    FillIQMDataTableForExcel(table, tableColumns, sumColumnList, sqlGetCount, sqlGetData, sqlGetSumResult);
                                    break;
                                case IQMSourceType.SP:
                                    break;
                            }
                            #endregion
                        }
                        #endregion
                    }
                    else //有Order子句条件下的查询
                    {
                        #region 有Order子句条件下的查询
                        if (string.IsNullOrEmpty(havingClause)) //无Having子句条件下的查询
                        {
                            #region //无Having子句条件下的查询
                            switch (sourceType)
                            {
                                case IQMSourceType.TVF:
                                    sqlGetCount = "SELECT COUNT(*) FROM (SELECT " + sqlGroupColumns + " FROM " + sourceName + " " + whereClause + " GROUP BY " + sqlGroupColumns + ") AS T1";
                                    sqlGetData = "SELECT RowNumber, " + sqlSelectColumns + " FROM (SELECT TOP (100) PERCENT ROW_NUMBER() OVER(" + orderClause + ") AS RowNumber, " + sqlGroupSumColumns + " FROM " + sourceName + " " + whereClause + " GROUP BY " + sqlGroupColumns + " " + orderClause + ") AS t1";
                                    sqlGetSumResult = "SELECT " + sqlSumResultColumns + " FROM " + sourceName + " " + whereClause;
                                    FillIQMDataTableForExcel(table, tableColumns, sumColumnList, sqlGetCount, sqlGetData, sqlGetSumResult);
                                    break;
                                case IQMSourceType.SP:
                                    break;
                            }
                            #endregion
                        }
                        else //有Having子句条件下的查询
                        {
                            #region 有Having子句条件下的查询
                            switch (sourceType)
                            {
                                case IQMSourceType.TVF:
                                    sqlGetCount = "SELECT COUNT(*) FROM (SELECT " + sqlGroupColumns + " FROM " + sourceName + " " + whereClause + " GROUP BY " + sqlGroupColumns + " " + havingClause + ") AS T1";
                                    sqlGetData = "SELECT RowNumber, " + sqlSelectColumns + " FROM (SELECT TOP (100) PERCENT ROW_NUMBER() OVER(" + orderClause + ") AS RowNumber, " + sqlGroupSumColumns + " FROM " + sourceName + " " + whereClause + " GROUP BY " + sqlGroupColumns + " " + havingClause + " " + orderClause + ") AS t1";
                                    sqlGetSumResult = "SELECT " + sqlSumResultColumns + " FROM " + sourceName + " " + whereClause + " GROUP BY " + sqlGroupColumns + " " + havingClause;
                                    FillIQMDataTableForExcel(table, tableColumns, sumColumnList, sqlGetCount, sqlGetData, sqlGetSumResult);
                                    break;
                                case IQMSourceType.SP:
                                    break;
                            }
                            #endregion
                        }
                        #endregion
                    }
                    #endregion
                }
                #endregion
            }
            else
            {
                #region 带分页的查询逻辑
                if (string.IsNullOrEmpty(groupColumns)) //无Group子句条件下的查询
                {
                    #region 无Group子句条件下的查询
                    if (string.IsNullOrEmpty(orderClause)) //无Order子句条件下的查询
                    {
                        #region 无Order子句条件下的查询
                        switch (sourceType)
                        {
                            case IQMSourceType.TVF:
                                sqlGetCount = "SELECT COUNT(*) FROM " + sourceName + " " + whereClause;
                                sqlGetData = "SELECT RowNumber, " + sqlSelectColumns + sqlQueryParameters + " FROM (SELECT ROW_NUMBER() OVER(ORDER BY " + sqlSelectColumns + ") AS RowNumber, " + sqlSelectColumns + sqlQueryParameters + " FROM " + sourceName + " " + whereClause + ") AS t1 WHERE RowNumber BETWEEN (" + startRowIndex + " + 1) AND (" + startRowIndex + " + " + maximumRows + ")";
                                sqlGetSumResult = "SELECT " + sqlSumResultColumns + " FROM " + sourceName + " " + whereClause;
                                FillIQMDataTable(table, tableColumns, sumColumnList, columnValueUrlDict, sqlGetCount, sqlGetData, sqlGetSumResult);
                                break;
                            case IQMSourceType.SP:
                                break;
                        }
                        #endregion
                    }
                    else //有Order子句条件下的查询
                    {
                        #region 有Order子句条件下的查询
                        switch (sourceType)
                        {
                            case IQMSourceType.TVF:
                                sqlGetCount = "SELECT COUNT(*) FROM " + sourceName + " " + whereClause;
                                sqlGetData = "SELECT RowNumber, " + sqlSelectColumns + sqlQueryParameters + " FROM (SELECT TOP (100) PERCENT ROW_NUMBER() OVER(" + orderClause + ") AS RowNumber, " + sqlSelectColumns + sqlQueryParameters + " FROM " + sourceName + " " + whereClause + " " + orderClause + ") AS t1 WHERE RowNumber BETWEEN (" + startRowIndex + " + 1) AND (" + startRowIndex + " + " + maximumRows + ")";
                                sqlGetSumResult = "SELECT " + sqlSumResultColumns + " FROM " + sourceName + " " + whereClause;
                                FillIQMDataTable(table, tableColumns, sumColumnList, columnValueUrlDict, sqlGetCount, sqlGetData, sqlGetSumResult);
                                break;
                            case IQMSourceType.SP:
                                break;
                        }
                        #endregion
                    }
                    #endregion
                }
                else //有Group子句条件下的查询
                {
                    #region //有Group子句条件下的查询
                    string sqlGroupColumns = groupColumns; //Group子句字符串与传入参数内容相同

                    //拼接Group条件下的Select字符串
                    sqlSelectColumns = groupColumns + sumColumns;

                    //拼接Group和Sum的完整字符串
                    string sqlGroupSumColumns = sqlGroupColumns + sqlSumColumns;

                    //根据新的Select字符串生成IQMDataTable的SelectColumns
                    selectColumnsList.Clear();
                    selectColumnsList.Add("RowNumber");
                    string[] selectColumnsArray = sqlSelectColumns.Split(new char[] { ',' });
                    foreach (string columnName in selectColumnsArray)
                    {
                        selectColumnsList.Add(columnName);
                    }

                    //根据传入参数生成不同条件下的查询字符串
                    if (string.IsNullOrEmpty(orderClause)) //无Order子句条件下的查询
                    {
                        #region 无Order子句条件下的查询
                        if (string.IsNullOrEmpty(havingClause)) //无Having子句条件下的查询
                        {
                            #region 无Having子句条件下的查询
                            switch (sourceType)
                            {
                                case IQMSourceType.TVF:
                                    sqlGetCount = "SELECT COUNT(*) FROM (SELECT " + sqlGroupColumns + " FROM " + sourceName + " " + whereClause + " GROUP BY " + sqlGroupColumns + ") AS T1";
                                    sqlGetData = "SELECT RowNumber, " + sqlSelectColumns + " FROM (SELECT ROW_NUMBER() OVER(ORDER BY " + sqlGroupColumns + ") AS RowNumber, " + sqlGroupSumColumns + " FROM " + sourceName + " " + whereClause + " GROUP BY " + sqlGroupColumns + ") AS t1 WHERE RowNumber BETWEEN (" + startRowIndex + " + 1) AND (" + startRowIndex + " + " + maximumRows + ")";
                                    sqlGetSumResult = "SELECT " + sqlSumResultColumns + " FROM " + sourceName + " " + whereClause;
                                    FillIQMDataTable(table, tableColumns, sumColumnList, columnValueUrlDict, sqlGetCount, sqlGetData, sqlGetSumResult);
                                    break;
                                case IQMSourceType.SP:
                                    break;
                            }
                            #endregion
                        }
                        else //有Having子句条件下的查询
                        {
                            #region 有Having子句条件下的查询
                            switch (sourceType)
                            {
                                case IQMSourceType.TVF:
                                    sqlGetCount = "SELECT COUNT(*) FROM (SELECT " + sqlGroupColumns + " FROM " + sourceName + " " + whereClause + " GROUP BY " + sqlGroupColumns + " " + havingClause + ") AS T1";
                                    sqlGetData = "SELECT RowNumber, " + sqlSelectColumns + " FROM (SELECT ROW_NUMBER() OVER(ORDER BY " + sqlGroupColumns + ") AS RowNumber, " + sqlGroupSumColumns + " FROM " + sourceName + " " + whereClause + " GROUP BY " + sqlGroupColumns + " " + havingClause + ") AS t1 WHERE RowNumber BETWEEN (" + startRowIndex + " + 1) AND (" + startRowIndex + " + " + maximumRows + ")";
                                    sqlGetSumResult = "SELECT " + sqlSumResultColumns + " FROM " + sourceName + " " + whereClause + " GROUP BY " + sqlGroupColumns + " " + havingClause;
                                    FillIQMDataTable(table, tableColumns, sumColumnList, columnValueUrlDict, sqlGetCount, sqlGetData, sqlGetSumResult);
                                    break;
                                case IQMSourceType.SP:
                                    break;
                            }
                            #endregion
                        }
                        #endregion
                    }
                    else //有Order子句条件下的查询
                    {
                        #region 有Order子句条件下的查询
                        if (string.IsNullOrEmpty(havingClause)) //无Having子句条件下的查询
                        {
                            #region //无Having子句条件下的查询
                            switch (sourceType)
                            {
                                case IQMSourceType.TVF:
                                    sqlGetCount = "SELECT COUNT(*) FROM (SELECT " + sqlGroupColumns + " FROM " + sourceName + " " + whereClause + " GROUP BY " + sqlGroupColumns + ") AS T1";
                                    sqlGetData = "SELECT RowNumber, " + sqlSelectColumns + " FROM (SELECT TOP (100) PERCENT ROW_NUMBER() OVER(" + orderClause + ") AS RowNumber, " + sqlGroupSumColumns + " FROM " + sourceName + " " + whereClause + " GROUP BY " + sqlGroupColumns + " " + orderClause + ") AS t1 WHERE RowNumber BETWEEN (" + startRowIndex + " + 1) AND (" + startRowIndex + " + " + maximumRows + ")";
                                    sqlGetSumResult = "SELECT " + sqlSumResultColumns + " FROM " + sourceName + " " + whereClause;
                                    FillIQMDataTable(table, tableColumns, sumColumnList, columnValueUrlDict, sqlGetCount, sqlGetData, sqlGetSumResult);
                                    break;
                                case IQMSourceType.SP:
                                    break;
                            }
                            #endregion
                        }
                        else //有Having子句条件下的查询
                        {
                            #region 有Having子句条件下的查询
                            switch (sourceType)
                            {
                                case IQMSourceType.TVF:
                                    sqlGetCount = "SELECT COUNT(*) FROM (SELECT " + sqlGroupColumns + " FROM " + sourceName + " " + whereClause + " GROUP BY " + sqlGroupColumns + " " + havingClause + ") AS T1";
                                    sqlGetData = "SELECT RowNumber, " + sqlSelectColumns + " FROM (SELECT TOP (100) PERCENT ROW_NUMBER() OVER(" + orderClause + ") AS RowNumber, " + sqlGroupSumColumns + " FROM " + sourceName + " " + whereClause + " GROUP BY " + sqlGroupColumns + " " + havingClause + " " + orderClause + ") AS t1 WHERE RowNumber BETWEEN (" + startRowIndex + " + 1) AND (" + startRowIndex + " + " + maximumRows + ")";
                                    sqlGetSumResult = "SELECT " + sqlSumResultColumns + " FROM " + sourceName + " " + whereClause + " GROUP BY " + sqlGroupColumns + " " + havingClause;
                                    FillIQMDataTable(table, tableColumns, sumColumnList, columnValueUrlDict, sqlGetCount, sqlGetData, sqlGetSumResult);
                                    break;
                                case IQMSourceType.SP:
                                    break;
                            }
                            #endregion
                        }
                        #endregion
                    }
                    #endregion
                }
                #endregion
            }

            #endregion

            return table;
        }

        private static void FillIQMDataTable(IQMDataTable table, List<IQMDataColumn> tableColumns, List<IQMDataColumn> sumColumnList, Dictionary<string, IQMDataValueUrl> columnValueUrlDict, string sqlGetCount, string sqlGetData, string sqlGetSumResult)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                connection.Open();

                /* ========================================== 查询总记录数并填充IQMDataTable的DataTotal属性 ========================================== */

                #region 查询总记录数并填充IQMDataTable的DataTotal属性

                command.CommandText = sqlGetCount;
                int dataCount = (int)command.ExecuteScalar();
                table.DataTotal = dataCount;

                #endregion

                /* ========================================== 查询数据填充IQMDataTable ========================================== */

                #region 查询数据填充IQMDataTable

                command.CommandText = sqlGetData;
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        //获取IQMDataTable中新的IQMDataRow对象
                        IQMDataRow dataRow = new IQMDataRow();
                        List<string> columnNames = table.ColumnNames;
                        for (int i = 0; i < columnNames.Count; i++)
                        {
                            dataRow.Cells.Add(new IQMDataCell());
                        }

                        //从数据库读取并填充IQMDataRow
                        for (int i = 0; i < dataRow.Cells.Count; i++)
                        {
                            IQMDataCell dataCell = dataRow.Cells[i];
                            if (i == 0) //填充序号
                            {
                                dataCell.Value = reader[i].ToString();
                            }
                            else //根据字段类型格式化数据内容
                            {
                                IQMDataColumn dataColumn = tableColumns.Find(x => x.Name.Equals(columnNames[i]));
                                if (string.IsNullOrEmpty(reader[i].ToString()))
                                {
                                    if (dataColumn.ColumnType.Equals("money"))
                                    {
                                        dataCell.Value = "0.00";
                                    }
                                    else if (dataColumn.ColumnType.Equals("number"))
                                    {
                                        dataCell.Value = "0";
                                    }
                                    else
                                    {
                                        dataCell.Value = "-";
                                    }
                                }
                                else
                                {
                                    if (dataColumn.ColumnType.Equals("money")) //格式化金额类型数据
                                    {
                                        dataCell.Value = string.Format("{0:N2}", reader.GetValue(i));
                                    }
                                    else
                                    {
                                        List<IQMDataValueAlias> valueAliasList = dataColumn.ValueAlias;
                                        if (valueAliasList != null && valueAliasList.Count != 0) //如果IQMDataColumn对象存在字段内容别名，则用别名替代所读取的数据
                                        {
                                            IQMDataValueAlias valueAlias = valueAliasList.Find(x => x.Value.Equals(reader.GetValue(i).ToString()));
                                            if (valueAlias != null)
                                            {
                                                dataCell.Value = valueAlias.Alias;
                                            }
                                            else
                                            {
                                                dataCell.Value = "<a style='color:#FF0000'>读取错误</a>";
                                            }
                                        }
                                        else
                                        {
                                            dataCell.Value = reader[i].ToString();
                                        }
                                    }
                                }
                            }
                        }

                        //根据内容链接对象键值对列表填充IQMDataRow的ValueUrl属性
                        foreach (KeyValuePair<string, IQMDataValueUrl> pair in columnValueUrlDict)
                        {
                            IQMDataValueUrl valueUrl = pair.Value;
                            string url = valueUrl.Url + "?";
                            int urlLengthOld = url.Length;
                            int urlLengthNew = 0;

                            if (pair.Value.Keys.Count != 0)
                            {
                                foreach (string key in pair.Value.Keys)
                                {
                                    string keyValue = reader[key].ToString();
                                    if (!string.IsNullOrEmpty(keyValue))
                                    {
                                        url += key + "=" + reader[key].ToString() + "&";
                                    }
                                }
                                //如果参数值为空则不显示内容链接
                                urlLengthNew = url.Length;
                                if (urlLengthOld == urlLengthNew)
                                {
                                    url = string.Empty;
                                }
                            }
                            int columnIndex = columnNames.FindIndex(c => c.Equals(pair.Key));
                            IQMDataCell dataCell = dataRow.Cells[columnIndex];
                            if (!string.IsNullOrEmpty(dataCell.Value) && !string.IsNullOrEmpty(url))
                            {
                                dataCell.ValueUrl = url.Substring(0, url.Length - 1);
                            }
                        }

                        table.Rows.Add(dataRow);
                    }
                    reader.Close();

                    /* ========================================== 查询汇总结果填充IQMDataTable ========================================== */

                    #region 查询汇总结果填充IQMDataTable

                    if (sumColumnList.Count > 0)
                    {
                        command.CommandText = sqlGetSumResult;
                        SqlDataReader readerSumResult = command.ExecuteReader();
                        if (readerSumResult.HasRows)
                        {
                            while (readerSumResult.Read())
                            {
                                foreach (IQMDataColumn dataColumn in sumColumnList)
                                {
                                    IQMDataSumResult result = new IQMDataSumResult();
                                    result.Name = dataColumn.Alias;
                                    if (string.IsNullOrEmpty(readerSumResult[dataColumn.Name].ToString()))
                                    {
                                        if (dataColumn.ColumnType.Equals("money"))
                                        {
                                            result.Result = "0.00";
                                        }
                                        else if (dataColumn.ColumnType.Equals("number"))
                                        {
                                            result.Result = "0";
                                        }
                                        else
                                        {
                                            result.Result = "-";
                                        }
                                    }
                                    else
                                    {
                                        if (dataColumn.ColumnType.Equals("money")) //格式化金额类型数据
                                        {
                                            result.Result = string.Format("{0:N2}", readerSumResult[dataColumn.Name]);
                                        }
                                        else
                                        {
                                            result.Result = readerSumResult[dataColumn.Name].ToString();
                                        }
                                    }
                                    table.SumResults.Add(result);
                                }
                            }
                        }
                        readerSumResult.Close();
                    }

                    #endregion
                }

                #endregion
            }
        }

        private static void FillIQMDataTableForExcel(IQMDataTable table, List<IQMDataColumn> tableColumns, List<IQMDataColumn> sumColumnList, string sqlGetCount, string sqlGetData, string sqlGetSumResult)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                connection.Open();

                /* ========================================== 查询总记录数并填充IQMDataTable的DataTotal属性 ========================================== */

                #region 查询总记录数并填充IQMDataTable的DataTotal属性

                command.CommandText = sqlGetCount;
                int dataCount = (int)command.ExecuteScalar();
                table.DataTotal = dataCount;

                #endregion

                /* ========================================== 查询数据填充IQMDataTable ========================================== */

                #region 查询数据填充IQMDataTable

                command.CommandText = sqlGetData;
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        //获取IQMDataTable中新的IQMDataRow对象
                        IQMDataRow dataRow = new IQMDataRow();
                        List<string> columnNames = table.ColumnNames;
                        for (int i = 0; i < columnNames.Count; i++)
                        {
                            dataRow.Cells.Add(new IQMDataCell());
                        }

                        //从数据库读取并填充IQMDataRow
                        for (int i = 0; i < dataRow.Cells.Count; i++)
                        {
                            IQMDataCell dataCell = dataRow.Cells[i];
                            if (i == 0) //填充序号
                            {
                                dataCell.Value = reader[i].ToString();
                            }
                            else //根据字段类型格式化数据内容
                            {
                                IQMDataColumn dataColumn = tableColumns.Find(x => x.Name.Equals(columnNames[i]));
                                if (string.IsNullOrEmpty(reader[i].ToString()))
                                {
                                    if (dataColumn.ColumnType.Equals("money"))
                                    {
                                        dataCell.Value = "0.00";
                                    }
                                    else if (dataColumn.ColumnType.Equals("number"))
                                    {
                                        dataCell.Value = "0";
                                    }
                                    else
                                    {
                                        dataCell.Value = "";
                                    }
                                }
                                else
                                {
                                    if (dataColumn.ColumnType.Equals("money")) //格式化金额类型数据
                                    {
                                        dataCell.Value = string.Format("{0:N2}", reader.GetValue(i));
                                    }
                                    else
                                    {
                                        List<IQMDataValueAlias> valueAlias = dataColumn.ValueAlias;
                                        if (valueAlias != null && valueAlias.Count != 0) //如果IQMDataColumn对象存在字段内容别名，则用别名替代所读取的数据
                                        {
                                            foreach (IQMDataValueAlias va in valueAlias)
                                            {
                                                if (reader.GetValue(i).ToString().Equals(va.Value))
                                                {
                                                    dataCell.Value = va.Alias;
                                                    break;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            dataCell.Value = reader[i].ToString();
                                        }
                                    }
                                }
                            }
                        }

                        table.Rows.Add(dataRow);
                    }
                    reader.Close();

                    /* ========================================== 查询汇总结果填充IQMDataTable ========================================== */

                    #region 查询汇总结果填充IQMDataTable

                    if (sumColumnList.Count > 0)
                    {
                        command.CommandText = sqlGetSumResult;
                        SqlDataReader readerSumResult = command.ExecuteReader();
                        if (readerSumResult.HasRows)
                        {
                            while (readerSumResult.Read())
                            {
                                foreach (IQMDataColumn dataColumn in sumColumnList)
                                {
                                    IQMDataSumResult result = new IQMDataSumResult();
                                    result.Name = dataColumn.Alias;
                                    if (string.IsNullOrEmpty(readerSumResult[dataColumn.Name].ToString()))
                                    {
                                        if (dataColumn.ColumnType.Equals("money"))
                                        {
                                            result.Result = "0.00";
                                        }
                                        else if (dataColumn.ColumnType.Equals("number"))
                                        {
                                            result.Result = "0";
                                        }
                                        else
                                        {
                                            result.Result = "";
                                        }
                                    }
                                    else
                                    {
                                        if (dataColumn.ColumnType.Equals("money")) //格式化金额类型数据
                                        {
                                            result.Result = string.Format("{0:N2}", readerSumResult[dataColumn.Name]);
                                        }
                                        else
                                        {
                                            result.Result = readerSumResult[dataColumn.Name].ToString();
                                        }
                                    }
                                    table.SumResults.Add(result);
                                }
                            }
                        }
                        readerSumResult.Close();
                    }

                    #endregion
                }

                #endregion
            }
        }

        internal static List<IQMTableAlias> GetTableAlias(string xmlDocNames)
        {
            List<IQMTableAlias> tableAliasList = new List<IQMTableAlias>();

            try
            {
                string[] xmlDocNameArray = xmlDocNames.Split(new char[] { ',' });
                foreach (string xmlDocName in xmlDocNameArray)
                {
                    IQMTableAlias tableAlias = new IQMTableAlias();
                    tableAlias.XmlDocName = xmlDocName;

                    //根据传入的XML文档名读取XML文档
                    string xmlDocPatch = IQMXmlDocumentsPatch + xmlDocName + ".xml";
                    XElement tableElms = XElement.Load(xmlDocPatch);
                    XAttribute attrAlias = tableElms.Attribute("Alias");
                    tableAlias.TableAlias = attrAlias.Value;

                    tableAliasList.Add(tableAlias);
                }
            }
            catch (FileNotFoundException) { } //如果找不到则忽略继续

            return tableAliasList;
        }

        internal static List<IQMDataColumn> GetAllColumns(string xmlDocName)
        {
            List<IQMDataColumn> columnList = new List<IQMDataColumn>();

            //根据传入的XML文档名读取XML文档
            string xmlDocPatch = IQMXmlDocumentsPatch + xmlDocName + ".xml";
            XElement tableElms = XElement.Load(xmlDocPatch);
            XNamespace xmlns = tableElms.Name.Namespace;


            //在IQMDataTable的Columns中添加序号行信息
            IQMDataColumn rowNumColumn = new IQMDataColumn();
            rowNumColumn.Name = "RowNumber";
            rowNumColumn.Alias = "序号";
            rowNumColumn.ColumnType = "number";
            rowNumColumn.TextAlign = "center";
            columnList.Add(rowNumColumn);

            //根据XML文档配置信息填充IQMDataTable的Columns对象属性
            IEnumerable<XElement> columnElms = tableElms.Element(xmlns + "Columns").Elements();
            foreach (XElement columnElm in columnElms) //遍历XML文档中的全部Column元素
            {
                IQMDataColumn column = new IQMDataColumn();
                foreach (XAttribute attr in columnElm.Attributes()) //遍历Column元素的全部属性
                {
                    switch (attr.Name.ToString())
                    {
                        case "Name":
                            column.Name = attr.Value;
                            break;
                        case "Alias":
                            column.Alias = attr.Value;
                            break;
                        case "ColumnType":
                            column.ColumnType = attr.Value;
                            break;
                        case "IsSumColumn":
                            if (attr.Value == "0" || attr.Value.ToLower() == "false")
                            {
                                column.IsSumColumn = false;
                            }
                            else if (attr.Value == "1" || attr.Value.ToLower() == "true")
                            {
                                column.IsSumColumn = true;
                            }
                            break;
                        case "TextAlign":
                            column.TextAlign = attr.Value;
                            break;
                    }
                }
                //从Column的子元素中提取数据内容别名和数据链接信息
                if (columnElm.Elements().Count() > 0) //判断Column元素是否有子元素
                {
                    foreach (XElement childElm in columnElm.Elements()) //遍历Column元素的全部子元素
                    {
                        if (childElm.Name.Equals(xmlns + "ValueAlias"))
                        {
                            List<IQMDataValueAlias> valueAliasList = new List<IQMDataValueAlias>();
                            foreach (XElement valueAliasElm in childElm.Elements())
                            {
                                IQMDataValueAlias valueAlias = new IQMDataValueAlias();
                                foreach (XAttribute attr in valueAliasElm.Attributes())
                                {
                                    switch (attr.Name.ToString())
                                    {
                                        case "Value":
                                            valueAlias.Value = attr.Value;
                                            break;
                                        case "Alias":
                                            valueAlias.Alias = attr.Value;
                                            break;
                                    }
                                }
                                valueAliasList.Add(valueAlias);
                            }
                            column.ValueAlias = valueAliasList;
                        }
                    }
                }
                columnList.Add(column);
            }

            return columnList;
        }

        internal static List<string> GetColumnDistinctValue(string xmlDocName, string columnName)
        {
            List<string> valueList = new List<string>();

            string sourceName = string.Empty; ; //用于保存数据源名称
            IQMSourceType sourceType = IQMSourceType.TVF; //用于保存数据源类型

            //根据传入的XML文档名读取XML文档
            string xmlDocPatch = IQMXmlDocumentsPatch + xmlDocName + ".xml";
            XElement tableElms = XElement.Load(xmlDocPatch);
            XNamespace xmlns = tableElms.Name.Namespace;

            //根据XML文档配置信息填充IQMDataTable对象属性
            IEnumerable<XAttribute> tableAttrs = tableElms.Attributes();
            foreach (XAttribute attr in tableAttrs) //遍历XML文档中根元素的全部属性
            {
                switch (attr.Name.ToString())
                {
                    case "SourceName":
                        sourceName = attr.Value;
                        break;
                    case "SourceType":
                        sourceType = (IQMSourceType)Enum.Parse(typeof(IQMSourceType), attr.Value);
                        break;
                }
            }

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                connection.Open();

                switch (sourceType)
                {
                    case IQMSourceType.TVF:
                        command.CommandText = "SELECT DISTINCT " + columnName + " FROM " + sourceName + " WHERE (" + columnName + " IS NOT NULL) AND (" + columnName + " <> '')";
                        break;
                    case IQMSourceType.SP:
                        break;
                }

                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        valueList.Add(reader[0].ToString());
                    }
                    reader.Close();
                }
            }

            return valueList;
        }

        internal static string GetExcel(string xmlDocName, string selectColumns, string whereClause, string groupColumns, string orderClause, string havingClause, string queryParameter)
        {

            string currentTime = DateTime.Now.ToString("yyyyMMddhhmmss");
            string excelFileName = xmlDocName + currentTime + ".xlsx";
            string excelPath = IQMTempPatch + excelFileName;

            if (!Directory.Exists(IQMTempPatch))
            {
                Directory.CreateDirectory(IQMTempPatch);
            }

            foreach (string filename in Directory.GetFiles(IQMTempPatch, "*.xlsx"))
            {
                FileInfo info = new FileInfo(filename);
                if (info.LastWriteTime <= DateTime.Now.AddMinutes(-5))
                {
                    info.Delete();
                }
            }

            string startRowIndex = "";
            string maximumRows = "";
            IQMDataTable iqmDataTable = GetData(xmlDocName, startRowIndex, maximumRows, selectColumns, whereClause, groupColumns, orderClause, havingClause, queryParameter);
            List<IQMDataColumn> iqmColumnList = GetAllColumns(xmlDocName);

            #region Excel文件生成逻辑

            using (SpreadsheetDocument excel = SpreadsheetDocument.Create(excelPath, SpreadsheetDocumentType.Workbook))
            {
                WorkbookPart workbookPart = excel.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();

                WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                worksheetPart.Worksheet = new Worksheet(new SheetData());


                Sheets sheets = excel.WorkbookPart.Workbook.AppendChild<Sheets>(new Sheets());

                Sheet sheet = new Sheet()
                {
                    Id = excel.WorkbookPart.GetIdOfPart(worksheetPart),
                    SheetId = 1,
                    Name = "查询结果"
                };
                sheets.Append(sheet);

                SheetData sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();

                SharedStringTablePart shareStringPart = excel.WorkbookPart.AddNewPart<SharedStringTablePart>();

                //WorkbookStylesPart workbookStylesPart = excel.WorkbookPart.AddNewPart<WorkbookStylesPart>();
                //workbookStylesPart.Stylesheet = GenerateStylesheet();
                //workbookStylesPart.Stylesheet.Save();

                #region 生成列名行
                Row columnRow = new Row();
                foreach (string columnName in iqmDataTable.ColumnNames)
                {
                    IQMDataColumn column = iqmColumnList.Find(c => c.Name.Equals(columnName));

                    Cell cell = new Cell();
                    cell.DataType = new EnumValue<CellValues>(CellValues.SharedString);

                    int index = InsertSharedStringItem(column.Alias, shareStringPart);
                    cell.CellValue = new CellValue(index.ToString());

                    columnRow.Append(cell);
                }
                sheetData.Append(columnRow);
                #endregion

                #region 生成数据行
                foreach (IQMDataRow iqmRow in iqmDataTable.Rows)
                {
                    Row row = new Row();
                    for (int i = 0; i < iqmRow.Cells.Count; i++)
                    {
                        Cell cell = new Cell();

                        //IQMDataColumn column = iqmColumnList.Find(c => c.Name.Equals(iqmDataTable.ColumnNames[i]));
                        //if (column.ColumnType == "year" || column.ColumnType == "month" || column.ColumnType == "day")
                        //{
                        //    cell.CellValue = new CellValue(iqmRow.Cells[i].Value);
                        //    cell.DataType = new EnumValue<CellValues>(CellValues.Number);
                        //}
                        //else if (column.ColumnType == "number")
                        //{
                        //    cell.CellValue = new CellValue(iqmRow.Cells[i].Value);
                        //    cell.DataType = new EnumValue<CellValues>(CellValues.Number);
                        //}
                        //else if (column.ColumnType == "money")
                        //{
                        //    cell.CellValue = new CellValue(iqmRow.Cells[i].Value);
                        //    cell.DataType = new EnumValue<CellValues>(CellValues.Number);
                        //    cell.StyleIndex = new UInt32Value(1U);
                        //}
                        //else if (column.ColumnType == "date")
                        //{
                        //    cell.CellValue = new CellValue(iqmRow.Cells[i].Value);
                        //    //cell.DataType = new EnumValue<CellValues>(CellValues.Date);
                        //    cell.StyleIndex = new UInt32Value(2U);
                        //}
                        //else
                        //{
                        //    int index = InsertSharedStringItem(iqmRow.Cells[i].Value, shareStringPart);
                        //    cell.CellValue = new CellValue(index.ToString());
                        //    cell.DataType = new EnumValue<CellValues>(CellValues.SharedString);
                        //}

                        int index = InsertSharedStringItem(iqmRow.Cells[i].Value, shareStringPart);
                        cell.CellValue = new CellValue(index.ToString());
                        cell.DataType = new EnumValue<CellValues>(CellValues.SharedString);

                        row.Append(cell);
                    }

                    sheetData.Append(row);
                }
                #endregion

                sheetData.Append(new Row());

                foreach (IQMDataSumResult iqmSum in iqmDataTable.SumResults)
                {
                    Row row = new Row();

                    int indexName = InsertSharedStringItem(iqmSum.Name, shareStringPart);
                    row.Append(new Cell() { CellValue = new CellValue(indexName.ToString()), DataType = new EnumValue<CellValues>(CellValues.SharedString) });

                    int indexResult = InsertSharedStringItem(iqmSum.Result, shareStringPart);
                    row.Append(new Cell() { CellValue = new CellValue(indexResult.ToString()), DataType = new EnumValue<CellValues>(CellValues.SharedString) });

                    sheetData.Append(row);
                }

                workbookPart.Workbook.Save();

                excel.Close();
            }

            #endregion

            return excelFileName;
        }

        private static int InsertSharedStringItem(string text, SharedStringTablePart shareStringPart)
        {
            // If the part does not contain a SharedStringTable, create one.
            if (shareStringPart.SharedStringTable == null)
            {
                shareStringPart.SharedStringTable = new SharedStringTable();
            }

            int i = 0;

            // Iterate through all the items in the SharedStringTable. If the text already exists, return its index.
            foreach (SharedStringItem item in shareStringPart.SharedStringTable.Elements<SharedStringItem>())
            {
                if (item.InnerText == text)
                {
                    return i;
                }

                i++;
            }

            // The text does not exist in the part. Create the SharedStringItem and return its index.
            shareStringPart.SharedStringTable.AppendChild(new SharedStringItem(new DocumentFormat.OpenXml.Spreadsheet.Text(text)));
            shareStringPart.SharedStringTable.Save();

            return i;
        }

        private static Stylesheet GenerateStylesheet()
        {
            Stylesheet stylesheet = new Stylesheet() { MCAttributes = new MarkupCompatibilityAttributes() { Ignorable = "x14ac" } };
            stylesheet.AddNamespaceDeclaration("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
            stylesheet.AddNamespaceDeclaration("x14ac", "http://schemas.microsoft.com/office/spreadsheetml/2009/9/ac");

            #region 生成日期型数字格式
            NumberingFormats numberingFormats = new NumberingFormats() { Count = (UInt32Value)1U };
            NumberingFormat numberingFormatDate = new NumberingFormat() { NumberFormatId = (UInt32Value)177U, FormatCode = "yyyy\\-mm\\-dd;@" };
            numberingFormats.Append(numberingFormatDate);
            #endregion

            Fonts fonts = new Fonts() { Count = (UInt32Value)1U, KnownFonts = true };
            Font font = new Font()
            {
                FontSize = new FontSize() { Val = 11D },
                FontName = new FontName() { Val = "宋体" },
                FontScheme = new FontScheme() { Val = FontSchemeValues.Minor }
            };
            fonts.Append(font);

            Fills fills = new Fills() { Count = (UInt32Value)1U };
            Fill fill = new Fill()
            {
                PatternFill = new PatternFill() { PatternType = PatternValues.None }
            };
            fills.Append(fill);

            Borders borders = new Borders() { Count = (UInt32Value)1U };
            Border border = new Border()
            {
                LeftBorder = new LeftBorder(),
                RightBorder = new RightBorder(),
                TopBorder = new TopBorder(),
                BottomBorder = new BottomBorder(),
                DiagonalBorder = new DiagonalBorder()
            };
            borders.Append(border);

            CellStyleFormats cellStyleFormats = new CellStyleFormats() { Count = (UInt32Value)1U };
            CellFormat cellFormat = new CellFormat() { NumberFormatId = (UInt32Value)0U, FontId = (UInt32Value)0U, FillId = (UInt32Value)0U, BorderId = (UInt32Value)0U };

            cellStyleFormats.Append(cellFormat);

            CellFormats cellFormats = new CellFormats() { Count = (UInt32Value)3U };
            CellFormat cellFormat1 = new CellFormat()
            {
                NumberFormatId = (UInt32Value)0U,
                FontId = (UInt32Value)0U,
                FillId = (UInt32Value)0U,
                BorderId = (UInt32Value)0U,
                FormatId = (UInt32Value)0U
            };
            CellFormat cellFormatMoney = new CellFormat()
            {
                NumberFormatId = (UInt32Value)4U,
                FontId = (UInt32Value)0U,
                FillId = (UInt32Value)0U,
                BorderId = (UInt32Value)0U,
                FormatId = (UInt32Value)0U,
                ApplyNumberFormat = true
            };
            CellFormat cellFormatDate = new CellFormat()
            {
                NumberFormatId = (UInt32Value)177U,
                FontId = (UInt32Value)0U,
                FillId = (UInt32Value)0U,
                BorderId = (UInt32Value)0U,
                FormatId = (UInt32Value)0U,
                ApplyNumberFormat = true
            };

            cellFormats.Append(cellFormat1);
            cellFormats.Append(cellFormatMoney);
            cellFormats.Append(cellFormatDate);

            CellStyles cellStyles = new CellStyles() { Count = (UInt32Value)1U };
            CellStyle cellStyle = new CellStyle() { Name = "常规", FormatId = (UInt32Value)0U, BuiltinId = (UInt32Value)0U };

            cellStyles.Append(cellStyle);

            stylesheet.Append(numberingFormats);
            stylesheet.Append(fonts);
            stylesheet.Append(fills);
            stylesheet.Append(borders);
            stylesheet.Append(cellStyleFormats);
            stylesheet.Append(cellFormats);
            stylesheet.Append(cellStyles);

            return stylesheet;
        }

        public static string JsonSerializer<T>(T t)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));

            string jsonString = string.Empty;

            using (MemoryStream ms = new MemoryStream())
            {
                ser.WriteObject(ms, t);

                jsonString = Encoding.UTF8.GetString(ms.ToArray());
            }

            return jsonString;
        }
    }
}