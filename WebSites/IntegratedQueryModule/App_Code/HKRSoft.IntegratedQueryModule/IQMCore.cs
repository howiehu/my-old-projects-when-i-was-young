using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace HKRSoft.IntegratedQueryModule
{
    /// <summary>
    /// [综合查询模块]核心功能
    /// </summary>
    public class IQMCore
    {
        public IQMCore()
        {

        }

        /// <summary>
        /// 获取Web.config中的数据库连接字符串
        /// </summary>
        public static string ConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["ppmsdbConnectionString"].ConnectionString;
            }
        }

        /// <summary>
        /// 静态方法：利用IQMDataColumn对象列表格式化DataTable
        /// </summary>
        /// <param name="dataTable">所需要格式化的DataTable</param>
        /// <param name="iqmDbColumn">用于格式化DataTable的IQMDataColumn对象列表</param>
        /// <returns>被格式化后的DataTable</returns>
        private static DataTable FormatDataTable(IQMDataTable iqmDataTable)
        {
            DataTable dataTable = new DataTable(iqmDataTable.Name);
            if (string.IsNullOrEmpty(iqmDataTable.SqlGroupByClause))
            {
                dataTable.Columns.Add(new DataColumn("序号")); //增加一个序号列
                //遍历IQMDataColumn对象列表中的IQMDataColumn对象定制列样式
                foreach (IQMDataColumn dataColumn in iqmDataTable.DataColumns)
                {
                    //根据IQMDataColumn对象定制生成DataColumn对象
                    DataColumn column = new DataColumn(dataColumn.Alias); //将DataColumn对象的标题文字设置为IQMDataColumn对象的别名
                    dataTable.Columns.Add(column); //将已设置好的DataColumn对象添加进DataTable对象
                }
                return dataTable;
            }
            else
            {
                dataTable.Columns.Add(new DataColumn("序号")); //增加一个序号列
                string[] groupByColumnNames = iqmDataTable.SqlGroupByClause.Split(new char[] { ',' });
                foreach (string columnName in groupByColumnNames)
                {
                    IQMDataColumn dataColumn = iqmDataTable.DataColumns.Find(x => x.Name.Equals(columnName));
                    DataColumn column = new DataColumn(dataColumn.Alias);
                    dataTable.Columns.Add(column);
                }
                foreach (IQMDataColumn dataColumn in iqmDataTable.DataColumns)
                {
                    if ((dataColumn.ColumnType == IQMDataColumn.IQMDataColumnType.Money) || dataColumn.IsSumColumn)
                    {
                        DataColumn column = new DataColumn(dataColumn.Alias);
                        dataTable.Columns.Add(column);
                    }
                }

                return dataTable;
            }
        }

        /// <summary>
        /// 方法：利用IQMDataColumn对象列表生成SQL SELECT语句的列名及列名别名部分字符串
        /// </summary>
        /// <param name="iqmDataTable">IQMDataColumn对象列表</param>
        /// <param name="sectionPrimaryKey">列名部分字符串</param>
        /// <param name="sectionColumnAliasName">列名别名部分字符串</param>
        private static void GenerateSql(IQMDataTable iqmDataTable, out string sectionColumnName, out string sectionColumnAliasName)
        {
            sectionColumnName = string.Empty;
            sectionColumnAliasName = string.Empty;

            foreach (IQMDataColumn dataColumn in iqmDataTable.DataColumns)
            {
                //根据IQMDataColumn的字段名和字段别名拼接列名部分
                sectionColumnName += dataColumn.Name + ", ";
                //根据IQMDataColumn的字段名和字段别名拼接列名别名部分
                sectionColumnAliasName += dataColumn.Name + " AS '" + dataColumn.Alias + "', ";
            }

            //去掉拼接完成后的各部分最后的逗号和空格以防止出错
            sectionColumnName = sectionColumnName.Substring(0, sectionColumnName.Length - 2);
            sectionColumnAliasName = sectionColumnAliasName.Substring(0, sectionColumnAliasName.Length - 2);
        }

        /// <summary>
        /// 方法：利用IQMDataColumn对象列表生成SQL SELECT语句的列名、列名别名及列名汇总部分字符串
        /// </summary>
        /// <param name="iqmDataTable">IQMDataColumn对象列表</param>
        /// <param name="sectionPrimaryKey">列名部分字符串</param>
        /// <param name="sectionColumnAliasName">列名别名部分字符串</param>
        /// /// <param name="sectionSumColumnName">列名汇总部分字符串</param>
        private static void GenerateSql(IQMDataTable iqmDataTable, out string sectionColumnName, out string sectionColumnAliasName, out string sectionSumColumnName)
        {
            sectionColumnName = string.Empty;
            sectionColumnAliasName = string.Empty;
            sectionSumColumnName = string.Empty;

            string[] groupByColumnNames = iqmDataTable.SqlGroupByClause.Split(new char[] { ',' });
            foreach (string columnName in groupByColumnNames)
            {
                IQMDataColumn dataColumn = iqmDataTable.DataColumns.Find(x => x.Name.Equals(columnName));
                //根据IQMDataColumn的字段名和字段别名拼接列名部分
                sectionColumnName += dataColumn.Name + ", ";
                //根据IQMDataColumn的字段名和字段别名拼接列名别名部分
                sectionColumnAliasName += dataColumn.Name + " AS '" + dataColumn.Alias + "', ";
            }

            foreach (IQMDataColumn dataColumn in iqmDataTable.DataColumns)
            {
                if (dataColumn.IsSumColumn || dataColumn.ColumnType == IQMDataColumn.IQMDataColumnType.Money)
                {
                    sectionSumColumnName += "SUM(" + dataColumn.Name + ") AS " + dataColumn.Name + ", ";
                    sectionColumnAliasName += dataColumn.Name + " AS '" + dataColumn.Alias + "', ";
                }
            }

            //去掉拼接完成后的各部分最后的逗号和空格以防止出错
            sectionColumnName = sectionColumnName.Substring(0, sectionColumnName.Length - 2);
            sectionColumnAliasName = sectionColumnAliasName.Substring(0, sectionColumnAliasName.Length - 2);
            if (!string.IsNullOrEmpty(sectionSumColumnName))
            {
                sectionSumColumnName = ", " + sectionSumColumnName.Substring(0, sectionSumColumnName.Length - 2);
            }
        }

        /// <summary>
        /// 方法：根据IQMDataTable对象和分页参数读取数据到DataTable
        /// </summary>
        /// <param name="iqmDataTable">表数据对象</param>
        /// <returns>已格式化并包含全部数据的DataTable对象</returns>
        private static DataTable LoadDataToDataTable(IQMDataTable iqmDataTable)
        {
            if (string.IsNullOrEmpty(iqmDataTable.SqlGroupByClause))
            {
                return LoadDateNotGroup(iqmDataTable);
            }
            else
            {
                return LoadDateHasGroup(iqmDataTable);
            }
        }

        /// <summary>
        /// 方法：根据IQMDataTable对象和分页参数读取数据到DataTable
        /// </summary>
        /// <param name="iqmDataTable">表数据对象</param>
        /// <param name="startRowIndex">该参数用于指示为数据源分页支持检索的第一条记录的标识符的值</param>
        /// <param name="maximumRows">该参数用于指示要检索的数据源分页支持的记录数</param>
        /// <returns>已格式化并分页读取数据后的DataTable对象</returns>
        private static DataTable LoadDataToDataTable(IQMDataTable iqmDataTable, int startRowIndex, int maximumRows)
        {
            if (string.IsNullOrEmpty(iqmDataTable.SqlGroupByClause))
            {
                return LoadDateNotGroup(iqmDataTable, startRowIndex, maximumRows);
            }
            else
            {
                return LoadDateHasGroup(iqmDataTable, startRowIndex, maximumRows);
            }
        }

        private static DataTable LoadDateHasGroup(IQMDataTable iqmDataTable)
        {
            DataTable table = FormatDataTable(iqmDataTable); //用于生成并返回DataTable
            string sectionColumnName = string.Empty; //用于生成SQL SELECT语句的列名部分
            string sectionColumnAliasName = string.Empty; //用于生成SQL SELECT语句的列名别名部分
            string sectionSumColumnName = string.Empty;

            GenerateSql(iqmDataTable, out sectionColumnName, out sectionColumnAliasName, out sectionSumColumnName);

            //根据表数据对象和分页参数填充DataTable
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                switch (iqmDataTable.SourceType)
                {
                    case IQMDataTable.IQMSourceType.Table:
                    case IQMDataTable.IQMSourceType.View:
                        command.CommandText = "SELECT RowNumber AS 序号, " + sectionColumnAliasName + " FROM (SELECT ROW_NUMBER() OVER(ORDER BY " + sectionColumnName + ") AS RowNumber, " + sectionColumnName + sectionSumColumnName + " FROM " + iqmDataTable.SourceName + " " + iqmDataTable.SqlWhereClause + "GROUP BY " + sectionColumnName + ") AS t1";
                        break;
                    case IQMDataTable.IQMSourceType.Function:
                        break;
                }
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                LoadDataToDataColumn(iqmDataTable, table, reader);
            }
            return table;
        }


        private static DataTable LoadDateHasGroup(IQMDataTable iqmDataTable, int startRowIndex, int maximumRows)
        {
            DataTable table = FormatDataTable(iqmDataTable); //用于生成并返回DataTable
            string sectionColumnName = string.Empty; //用于生成SQL SELECT语句的列名部分
            string sectionColumnAliasName = string.Empty; //用于生成SQL SELECT语句的列名别名部分
            string sectionSumColumnName = string.Empty;

            GenerateSql(iqmDataTable, out sectionColumnName, out sectionColumnAliasName, out sectionSumColumnName);

            //根据表数据对象和分页参数填充DataTable
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                switch (iqmDataTable.SourceType)
                {
                    case IQMDataTable.IQMSourceType.Table:
                    case IQMDataTable.IQMSourceType.View:
                        command.CommandText = "SELECT RowNumber AS 序号, " + sectionColumnAliasName + " FROM (SELECT ROW_NUMBER() OVER(ORDER BY " + sectionColumnName + ") AS RowNumber, " + sectionColumnName + sectionSumColumnName + " FROM " + iqmDataTable.SourceName + " " + iqmDataTable.SqlWhereClause + "GROUP BY " + sectionColumnName + ") AS t1 WHERE RowNumber BETWEEN (" + startRowIndex + " + 1) AND (" + startRowIndex + " + " + maximumRows + " + 1)";
                        break;
                    case IQMDataTable.IQMSourceType.Function:
                        break;
                }
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                LoadDataToDataColumn(iqmDataTable, table, reader);
            }
            return table;
        }

        private static DataTable LoadDateNotGroup(IQMDataTable iqmDataTable)
        {
            DataTable table = FormatDataTable(iqmDataTable); //用于生成并返回DataTable
            string sectionColumnName = string.Empty; //用于生成SQL SELECT语句的列名部分
            string sectionColumnAliasName = string.Empty; //用于生成SQL SELECT语句的列名别名部分

            GenerateSql(iqmDataTable, out sectionColumnName, out sectionColumnAliasName);

            //根据表数据对象和分页参数填充DataTable
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                switch (iqmDataTable.SourceType)
                {
                    case IQMDataTable.IQMSourceType.Table:
                    case IQMDataTable.IQMSourceType.View:
                        command.CommandText = "SELECT RowNumber AS 序号, " + sectionColumnAliasName + " FROM (SELECT ROW_NUMBER() OVER(ORDER BY " + sectionColumnName + ") AS RowNumber, " + sectionColumnName + " FROM " + iqmDataTable.SourceName + " " + iqmDataTable.SqlWhereClause + ") AS t1";
                        break;
                    case IQMDataTable.IQMSourceType.Function:
                        break;
                }
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                LoadDataToDataColumn(iqmDataTable, table, reader);
            }
            return table;
        }

        private static DataTable LoadDateNotGroup(IQMDataTable iqmDataTable, int startRowIndex, int maximumRows)
        {
            DataTable table = FormatDataTable(iqmDataTable); //用于生成并返回DataTable
            string sectionColumnName = string.Empty; //用于生成SQL SELECT语句的列名部分
            string sectionColumnAliasName = string.Empty; //用于生成SQL SELECT语句的列名别名部分

            GenerateSql(iqmDataTable, out sectionColumnName, out sectionColumnAliasName);

            //根据表数据对象和分页参数填充DataTable
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                switch (iqmDataTable.SourceType)
                {
                    case IQMDataTable.IQMSourceType.Table:
                    case IQMDataTable.IQMSourceType.View:
                        command.CommandText = "SELECT RowNumber AS 序号, " + sectionColumnAliasName + " FROM (SELECT ROW_NUMBER() OVER(ORDER BY " + sectionColumnName + ") AS RowNumber, " + sectionColumnName + " FROM " + iqmDataTable.SourceName + " " + iqmDataTable.SqlWhereClause + ") AS t1 WHERE RowNumber BETWEEN (" + startRowIndex + " + 1) AND (" + startRowIndex + " + " + maximumRows + " + 1)";
                        break;
                    case IQMDataTable.IQMSourceType.Function:
                        break;
                }
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                LoadDataToDataColumn(iqmDataTable, table, reader);
            }
            return table;
        }

        /// <summary>
        /// 静态方法：根据IQMDataTable对象将数据读入已格式化好的DataTable对象
        /// </summary>
        /// <param name="iqmDataTable">IQMDataTable对象</param>
        /// <param name="dataTable">已格式化好的DataTable对象</param>
        /// <param name="sqlDataReader">SqlDataReader对象</param>
        private static void LoadDataToDataColumn(IQMDataTable iqmDataTable, DataTable dataTable, SqlDataReader sqlDataReader)
        {
            if (string.IsNullOrEmpty(iqmDataTable.SqlGroupByClause))
            {
                if (sqlDataReader.HasRows)
                {
                    while (sqlDataReader.Read())
                    {
                        DataRow row = dataTable.NewRow();
                        object[] itemArray = new object[row.ItemArray.Length];
                        for (int i = 0; i <= dataTable.Columns.Count - 1; i++)
                        {
                            if (i == 0)
                            {
                                itemArray[i] = sqlDataReader[i];
                            }
                            else
                            {
                                if (iqmDataTable.DataColumns.Find(x => x.Alias.Equals(dataTable.Columns[i].ColumnName)).ColumnType == IQMDataColumn.IQMDataColumnType.Money)
                                {
                                    itemArray[i] = string.Format("{0:N2}", sqlDataReader.GetValue(i));
                                }
                                else
                                {
                                    List<IQMDataValueAlias> valueAlias = iqmDataTable.DataColumns.Find(x => x.Alias.Equals(dataTable.Columns[i].ColumnName)).ValueAlias;
                                    if (valueAlias.Count != 0) //如果IQMDataColumn对象存在字段内容别名，则用别名替代所读取的数据
                                    {
                                        foreach (IQMDataValueAlias va in valueAlias)
                                        {
                                            if (sqlDataReader.GetValue(i).ToString().Equals(va.Value))
                                            {
                                                itemArray[i] = va.Alias;
                                                break;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        itemArray[i] = sqlDataReader[i];
                                    }
                                }
                            }
                        }
                        row.ItemArray = itemArray;
                        dataTable.Rows.Add(row);
                    }
                }
            }
            else
            {
                if (sqlDataReader.HasRows)
                {
                    while (sqlDataReader.Read())
                    {
                        DataRow row = dataTable.NewRow();
                        object[] itemArray = new object[row.ItemArray.Length]; //DataRow的itemArray数组长度比IQMDataTable中的IQMDataColumn对象数量多1个
                        for (int i = 0; i <= dataTable.Columns.Count - 1; i++)
                        {
                            if (i == 0)
                            {
                                itemArray[i] = sqlDataReader[i];
                            }
                            else
                            {
                                if (iqmDataTable.DataColumns.Find(x => x.Alias.Equals(dataTable.Columns[i].ColumnName)).ColumnType == IQMDataColumn.IQMDataColumnType.Money)
                                {
                                    itemArray[i] = string.Format("{0:N2}", sqlDataReader.GetValue(i));
                                }
                                else
                                {
                                    List<IQMDataValueAlias> valueAlias = iqmDataTable.DataColumns.Find(x => x.Alias.Equals(dataTable.Columns[i].ColumnName)).ValueAlias;
                                    if (valueAlias.Count != 0) //如果IQMDataColumn对象存在字段内容别名，则用别名替代所读取的数据
                                    {
                                        foreach (IQMDataValueAlias va in valueAlias)
                                        {
                                            if (sqlDataReader.GetValue(i).ToString().Equals(va.Value))
                                            {
                                                itemArray[i] = va.Alias;
                                                break;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        itemArray[i] = sqlDataReader[i];
                                    }
                                }
                            }
                        }
                        row.ItemArray = itemArray;
                        dataTable.Rows.Add(row);
                    }
                }
            }
        }

        /// <summary>
        /// 静态方法：根据IQMDataTable对象读取数据
        /// </summary>
        /// <param name="iqmDataTable">IQMDataTable对象</param>
        /// <returns>包含全部数据的DataSet</returns>
        public static DataSet LoadToDataSet(IQMDataTable iqmDataTable)
        {
            DataTable dataTable = LoadDataToDataTable(iqmDataTable);
            DataSet dataSet = new DataSet();
            dataSet.Tables.Add(dataTable);
            return dataSet;
        }

        /// <summary>
        /// 静态方法：根据IQMDataTable对象和分页参数读取数据
        /// </summary>
        /// <param name="iqmDataTable">IQMDataTable对象</param>
        /// <param name="startRowIndex">该参数用于指示为数据源分页支持检索的第一条记录的标识符的值</param>
        /// <param name="maximumRows">该参数用于指示要检索的数据源分页支持的记录数</param>
        /// <returns>包含分页数据的DataSet</returns>
        public static DataSet LoadToDataSet(IQMDataTable iqmDataTable, int startRowIndex, int maximumRows)
        {
            DataTable dataTable = LoadDataToDataTable(iqmDataTable, startRowIndex, maximumRows);
            DataSet dataSet = new DataSet();
            dataSet.Tables.Add(dataTable);
            return dataSet;
        }

        public static DataSet LoadSumData(IQMDataTable iqmDataTable)
        {
            string sectionSumColumnName = string.Empty;
            DataTable table = new DataTable(iqmDataTable.Name);
            foreach (IQMDataColumn dataColumn in iqmDataTable.DataColumns)
            {
                if (dataColumn.IsSumColumn || dataColumn.ColumnType == IQMDataColumn.IQMDataColumnType.Money)
                {
                    sectionSumColumnName += "SUM(" + dataColumn.Name + ") AS '" + dataColumn.Alias + "', ";
                    table.Columns.Add(dataColumn.Alias);
                }
            }
            sectionSumColumnName = sectionSumColumnName.Substring(0, sectionSumColumnName.Length - 2);

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                switch (iqmDataTable.SourceType)
                {
                    case IQMDataTable.IQMSourceType.Table:
                    case IQMDataTable.IQMSourceType.View:
                        command.CommandText = "SELECT " + sectionSumColumnName + " FROM " + iqmDataTable.SourceName + " " + iqmDataTable.SqlWhereClause;
                        break;
                    case IQMDataTable.IQMSourceType.Function:
                        break;
                }
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        DataRow row = table.NewRow();
                        object[] itemArray = new object[row.ItemArray.Length];
                        for (int i = 0; i <= row.ItemArray.Length - 1; i++)
                        {
                            itemArray[i] = reader[i];
                        }
                        row.ItemArray = itemArray;
                        table.Rows.Add(row);
                    }
                }
            }

            DataSet dataSet = new DataSet();
            dataSet.Tables.Add(table);

            return dataSet;
        }

        public static int LoadDataCount(IQMDataTable iqmDataTable)
        {
            int dataCount;

            if (string.IsNullOrEmpty(iqmDataTable.SqlGroupByClause))
            {
                //根据表数据对象和分页参数填充DataTable
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    switch (iqmDataTable.SourceType)
                    {
                        case IQMDataTable.IQMSourceType.Table:
                        case IQMDataTable.IQMSourceType.View:
                            command.CommandText = "SELECT COUNT(*) FROM " + iqmDataTable.SourceName + " " + iqmDataTable.SqlWhereClause;
                            break;
                        case IQMDataTable.IQMSourceType.Function:
                            break;
                    }
                    connection.Open();
                    dataCount = (int)command.ExecuteScalar();
                }
            }
            else
            {
                //根据表数据对象和分页参数填充DataTable
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    switch (iqmDataTable.SourceType)
                    {
                        case IQMDataTable.IQMSourceType.Table:
                        case IQMDataTable.IQMSourceType.View:
                            command.CommandText = "SELECT COUNT(*) FROM (SELECT " + iqmDataTable.SqlGroupByClause + " FROM " + iqmDataTable.SourceName + " " + iqmDataTable.SqlWhereClause + "GROUP BY " + iqmDataTable.SqlGroupByClause + ") AS t1";
                            break;
                        case IQMDataTable.IQMSourceType.Function:
                            break;
                    }
                    connection.Open();
                    dataCount = (int)command.ExecuteScalar();
                }
            }

            return dataCount;
        }

        public static List<IQMDataValueUrlRef> LoadDataValueUrlRefList(IQMDataTable iqmDataTable, string sqlWhereClause)
        {

            string sectionColumnName = string.Empty; //用于生成SQL SELECT语句的列名部分
            string sectionColumnAliasName = string.Empty; //用于生成SQL SELECT语句的列名别名部分

            GenerateSql(iqmDataTable, out sectionColumnName, out sectionColumnAliasName);

            List<IQMDataValueUrlRef> refs = new List<IQMDataValueUrlRef>();
            List<IQMDataColumn> dataColumnList = iqmDataTable.DataColumns;
            List<IQMDataColumn> urlColumnList = new List<IQMDataColumn>();
            List<string> sqlColumnList = new List<string>();
            string sqlColumns = string.Empty;
            bool hasValueUrl = false;
            DataTable table = new DataTable();
            table.Columns.Add("序号");

            foreach (IQMDataColumn column in dataColumnList)
            {
                if (column.ValueUrl != null)
                {
                    hasValueUrl = true;
                    urlColumnList.Add(column);
                    foreach (string queryString in column.ValueUrl.QueryStringList)
                    {
                        if (!sqlColumnList.Contains(queryString))
                        {
                            sqlColumnList.Add(queryString);
                            table.Columns.Add(queryString);
                        }
                    }
                }
            }

            if (hasValueUrl)
            {
                sqlColumnList.ForEach(x => sqlColumns += x + ", ");
                sqlColumns = sqlColumns.Substring(0, sqlColumns.Length - 2);

                //根据表数据对象和分页参数填充DataTable
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    switch (iqmDataTable.SourceType)
                    {
                        case IQMDataTable.IQMSourceType.Table:
                        case IQMDataTable.IQMSourceType.View:
                            command.CommandText = "SELECT ROW_NUMBER() OVER(ORDER BY " + sectionColumnName + ") AS RowNumber, " + sqlColumns + " FROM " + iqmDataTable.SourceName + " " + iqmDataTable.SqlWhereClause;
                            break;
                        case IQMDataTable.IQMSourceType.Function:
                            break;
                    }
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            DataRow row = table.NewRow();
                            object[] itemArray = new object[row.ItemArray.Length];
                            for (int i = 0; i <= itemArray.Length - 1; i++)
                            {
                                itemArray[i] = reader[i];
                            }
                            row.ItemArray = itemArray;
                            foreach (IQMDataColumn item in urlColumnList)
                            {
                                IQMDataValueUrlRef urlRef = new IQMDataValueUrlRef();
                                urlRef.RowNumber = row[0].ToString();
                                urlRef.ColumnName = item.Alias;
                                string querys = string.Empty;
                                if (item.ValueUrl.QueryStringList.Count == 0)
                                {
                                    urlRef.Url = item.ValueUrl.Url;
                                }
                                else
                                {
                                    foreach (string query in item.ValueUrl.QueryStringList)
                                    {
                                        querys += query + "=" + row[query].ToString() + "&";
                                    }
                                    urlRef.Url = item.ValueUrl.Url + "?" + querys.Substring(0, querys.Length - 1);
                                }

                                refs.Add(urlRef);
                            }
                        }
                    }
                }
            }

            return refs;
        }
    }
}