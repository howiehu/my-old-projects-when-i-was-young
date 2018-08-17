using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HKRSoft.IntegratedQueryModule;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        List<IQMData> iqmDataList = new List<IQMData>();
        iqmDataList.Add(new IQMZbPlanYearData());
        iqmDataList.Add(new IQMZbPlanMonthData());
        iqmDataList.Add(new IQMZbPlanTempData());
        iqmQueryHelper.IQMDataList = iqmDataList;
    }
}