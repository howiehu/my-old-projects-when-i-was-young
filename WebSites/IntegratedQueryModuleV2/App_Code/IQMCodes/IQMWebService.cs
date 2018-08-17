using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using HKRSoft.IntegratedQueryModule;
using System.Web.Script.Services;

/// <summary>
///IQMWebService 的摘要说明
/// </summary>
[WebService(Namespace = "http://www.hkrsoft.com.cn/webservices/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
//若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。 
[System.Web.Script.Services.ScriptService]
public class IQMWebService : System.Web.Services.WebService
{

    public IQMWebService()
    {

        //如果使用设计的组件，请取消注释以下行 
        //InitializeComponent(); 
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
    public List<IQMTableAlias> GetTableAlias(string xmlDocNames)
    {
        List<IQMTableAlias> tableAliasList = IQMCore.GetTableAlias(xmlDocNames);
        return tableAliasList;
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
    public List<IQMDataColumn> GetColumns(string xmlDocName)
    {
        List<IQMDataColumn> columns = IQMCore.GetAllColumns(xmlDocName);
        return columns;
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
    public IQMDataTable GetData(string xmlDocName, string startRowIndex, string maximumRows, string selectColumns, string whereClause, string groupColumns, string orderClause, string havingClause, string queryParameter)
    {
        IQMDataTable table = IQMCore.GetData(xmlDocName, startRowIndex, maximumRows, selectColumns, whereClause, groupColumns, orderClause, havingClause, queryParameter);
        return table;
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
    public List<string> GetAutocompleteValue(string xmlDocName, string columnName)
    {
        List<string> list = IQMCore.GetColumnDistinctValue(xmlDocName, columnName);
        return list;
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
    public string GetExcelName(string xmlDocName, string selectColumns, string whereClause, string groupColumns, string orderClause, string havingClause, string queryParameter)
    {
        string excelName = IQMCore.GetExcel(xmlDocName, selectColumns, whereClause, groupColumns, orderClause, havingClause, queryParameter);
        return excelName;
    }
}
