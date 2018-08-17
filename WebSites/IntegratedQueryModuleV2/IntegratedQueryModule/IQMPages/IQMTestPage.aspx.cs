using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HKRSoft.IntegratedQueryModule;
using System.Text;

public partial class IQMPages_IQMTestPage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        List<IQMQueryParameter> list = new List<IQMQueryParameter>();

        //string test = "99921fac-5662-46b3-83da-e19eac0d313b";

        list.Add(new IQMQueryParameter
        {
            XmlDocName = "IQMzb",
            QueryParameters =
            { 
                "99921fac-5662-46b3-83da-e19eac0d313b"
            }
        });

        list.Add(new IQMQueryParameter
        {
            XmlDocName = "IQMzbHthq",
            QueryParameters =
            { 
                "b992cad8-b0c0-4a29-86b4-00d4a158a35f"
            }
        });

        this.RegisterQueryParameter(list);
    }

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