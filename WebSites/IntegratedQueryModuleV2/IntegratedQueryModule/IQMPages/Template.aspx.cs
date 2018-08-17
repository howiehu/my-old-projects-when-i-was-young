using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Runtime.Serialization.Json;
using HKRSoft.IntegratedQueryModule;
using System.Text;

public partial class Template : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        List<IQMQueryParameter> list = new List<IQMQueryParameter>(); //必须新建一个IQMQueryParameter对象列表

        string test = "99921fac-5662-46b3-83da-e19eac0d313b"; //可选，仅供演示，可以采用读取Session等各种办法获取需要用到的字符串变量值

        //可选，调用IList<T>.Add()方法添加IQMQueryParameter对象
        list.Add(new IQMQueryParameter
        {
            XmlDocName = "IQMzb",
            QueryParameters =
            { 
                test //字符串参数列表
            }
        });

        this.RegisterQueryParameter(list); //必须调用改方法注册客户端脚本
    }

    /// <summary>
    /// 注册综合查询附加参数
    /// </summary>
    /// <param name="list">IQMQueryParameter对象列表</param>
    private void RegisterQueryParameter(List<IQMQueryParameter> list)
    {
        if (!ClientScript.IsStartupScriptRegistered("script"))
        {
            string jsonString = IQMCore.JsonSerializer(list);

            StringBuilder strBuilder = new StringBuilder();
            strBuilder.Append("<script type=\"text/javascript\">");
            strBuilder.Append("var queryParameters = " + "'" + jsonString + "';");
            strBuilder.Append("</script>");

            string javascript = strBuilder.ToString();
            ClientScript.RegisterStartupScript(this.GetType(), "script", javascript);
        }
    }
}