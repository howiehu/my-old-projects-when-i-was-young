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
    public class IQMZbPlanMonthData : IQMData
    {
        public IQMZbPlanMonthData()
        {
            IQMDataTable table = new IQMDataTable("月度招标计划数据", "iqm_zb_plan_month_v", IntegratedQueryModule.IQMDataTable.IQMSourceType.View);

            table.DataColumns.Add(new IQMDataColumn("departName", "部门名称", HorizontalAlign.Center));
            table.DataColumns.Add(new IQMDataColumn("planYear", "计划年份", IQMDataColumn.IQMDataColumnType.YearOrMonth));

            IQMDataValueUrl planMonthUrl = new IQMDataValueUrl("~/zb/zb_plan_month_edit.aspx");
            planMonthUrl.QueryStringList = new List<string>();
            planMonthUrl.QueryStringList.Add("mainid");
            table.DataColumns.Add(new IQMDataColumn("planMonth", "计划月份", planMonthUrl, IQMDataColumn.IQMDataColumnType.YearOrMonth));

            table.DataColumns.Add(new IQMDataColumn("zbName", "项目名称"));

            table.DataColumns.Add(new IQMDataColumn("zbBudget", "预算金额（元）", IQMDataColumn.IQMDataColumnType.Money));
            table.DataColumns.Add(new IQMDataColumn("planName", "预算名称"));

            IQMDataColumn businName = new IQMDataColumn("businName", "招标方式", IQMDataColumn.IQMDataColumnType.ValueAlias);
            businName.ValueAlias.Add(new IQMDataValueAlias("委托招标", "委托招标"));
            businName.ValueAlias.Add(new IQMDataValueAlias("公开招标", "公开招标"));
            businName.ValueAlias.Add(new IQMDataValueAlias("邀请招标", "邀请招标"));
            businName.ValueAlias.Add(new IQMDataValueAlias("直接合同谈判", "直接合同谈判"));
            table.DataColumns.Add(businName);

            IQMDataColumn zbStatus = new IQMDataColumn("zbStatus", "招标状态", IQMDataColumn.IQMDataColumnType.ValueAlias);
            zbStatus.ValueAlias.Add(new IQMDataValueAlias("0", "未招标"));
            zbStatus.ValueAlias.Add(new IQMDataValueAlias("1", "招标方案编辑"));
            zbStatus.ValueAlias.Add(new IQMDataValueAlias("2", "招标方案审核"));
            zbStatus.ValueAlias.Add(new IQMDataValueAlias("3", "招标方案通过"));
            zbStatus.ValueAlias.Add(new IQMDataValueAlias("4", "文件会签中"));
            zbStatus.ValueAlias.Add(new IQMDataValueAlias("5", "文件会签完成"));
            zbStatus.ValueAlias.Add(new IQMDataValueAlias("6", "抽取专家"));
            zbStatus.ValueAlias.Add(new IQMDataValueAlias("7", "定标编辑"));
            zbStatus.ValueAlias.Add(new IQMDataValueAlias("8", "定标审核"));
            zbStatus.ValueAlias.Add(new IQMDataValueAlias("9", "中标通知"));
            zbStatus.ValueAlias.Add(new IQMDataValueAlias("10", "招标结束"));
            table.DataColumns.Add(zbStatus);

            table.DataColumns.Add(new IQMDataColumn("zbStartDate", "招标计划开始时间", IQMDataColumn.IQMDataColumnType.Date));
            table.DataColumns.Add(new IQMDataColumn("zbEndDate", "招标计划结束时间", IQMDataColumn.IQMDataColumnType.Date));

            IQMDataValueUrl planYearUrl = new IQMDataValueUrl("~/zb/zb_plan_year_edit.aspx");
            planYearUrl.QueryStringList = new List<string>();
            planYearUrl.QueryStringList.Add("mainidYear");
            table.DataColumns.Add(new IQMDataColumn("planYearYear", "挂接年度计划", planYearUrl, IQMDataColumn.IQMDataColumnType.YearOrMonth));
            table.DataColumns.Add(new IQMDataColumn("zbNameYear", "挂接年度计划项目名称"));

            table.DataColumns.Add(new IQMDataColumn("createPerson", "部门录入人", HorizontalAlign.Center));
            table.DataColumns.Add(new IQMDataColumn("createDate", "送审日期", IQMDataColumn.IQMDataColumnType.Date));
            table.DataColumns.Add(new IQMDataColumn("zbAuditPerson", "财务部审核人", HorizontalAlign.Center));
            table.DataColumns.Add(new IQMDataColumn("zbAuditDate", "审核日期", IQMDataColumn.IQMDataColumnType.Date));

            IQMDataColumn lxFlag = new IQMDataColumn("LxFlag", "招标中心立项状态", IQMDataColumn.IQMDataColumnType.ValueAlias);
            lxFlag.ValueAlias.Add(new IQMDataValueAlias("0", "未立项"));
            lxFlag.ValueAlias.Add(new IQMDataValueAlias("1", "已立项"));
            table.DataColumns.Add(lxFlag);
            table.DataColumns.Add(new IQMDataColumn("LxUser", "立项审核人", HorizontalAlign.Center));
            table.DataColumns.Add(new IQMDataColumn("LxDate", "立项日期", IQMDataColumn.IQMDataColumnType.Date));

            table.DataColumns.Add(new IQMDataColumn("memo", "备注"));
            this.IQMDataTable = table;
        }
    }
}