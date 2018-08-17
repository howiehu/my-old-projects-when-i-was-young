using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace HKRSoft.IntegratedQueryModule
{
    /// <summary>
    ///IQMTestData 的摘要说明
    /// </summary>
    public class IQMZbPlanYearData : IQMData
    {
        public IQMZbPlanYearData()
        {
            IQMDataTable table = new IQMDataTable("年度招标计划数据", "iqm_zb_plan_year_v", IntegratedQueryModule.IQMDataTable.IQMSourceType.View);
            table.DataColumns.Add(new IQMDataColumn("departName", "部门名称", HorizontalAlign.Center));

            IQMDataValueUrl planYearUrl = new IQMDataValueUrl("~/zb/zb_plan_year_edit.aspx");
            planYearUrl.QueryStringList = new List<string>();
            planYearUrl.QueryStringList.Add("mainid");
            table.DataColumns.Add(new IQMDataColumn("planYear", "计划年份", planYearUrl, IQMDataColumn.IQMDataColumnType.YearOrMonth));

            table.DataColumns.Add(new IQMDataColumn("zbName", "项目名称"));
            table.DataColumns.Add(new IQMDataColumn("zbBudget", "预算金额（元）", IQMDataColumn.IQMDataColumnType.Money));

            IQMDataColumn businName = new IQMDataColumn("businName", "招标方式", IQMDataColumn.IQMDataColumnType.ValueAlias);
            businName.ValueAlias.Add(new IQMDataValueAlias("委托招标", "委托招标"));
            businName.ValueAlias.Add(new IQMDataValueAlias("公开招标", "公开招标"));
            businName.ValueAlias.Add(new IQMDataValueAlias("邀请招标", "邀请招标"));
            businName.ValueAlias.Add(new IQMDataValueAlias("直接合同谈判", "直接合同谈判"));
            table.DataColumns.Add(businName);

            table.DataColumns.Add(new IQMDataColumn("zbStartDate", "招标计划开始时间", IQMDataColumn.IQMDataColumnType.Date));
            table.DataColumns.Add(new IQMDataColumn("zbEndDate", "招标计划结束时间", IQMDataColumn.IQMDataColumnType.Date));

            table.DataColumns.Add(new IQMDataColumn("createPerson", "部门录入人", HorizontalAlign.Center));
            table.DataColumns.Add(new IQMDataColumn("createDate", "送审日期", IQMDataColumn.IQMDataColumnType.Date));
            table.DataColumns.Add(new IQMDataColumn("zbAuditPerson", "招标中心审核人", HorizontalAlign.Center));
            table.DataColumns.Add(new IQMDataColumn("zbAuditDate", "审核日期", IQMDataColumn.IQMDataColumnType.Date));

            table.DataColumns.Add(new IQMDataColumn("memo", "备注"));
            this.IQMDataTable = table;
        }
    }
}