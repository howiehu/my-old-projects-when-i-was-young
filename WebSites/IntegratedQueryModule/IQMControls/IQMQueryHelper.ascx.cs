using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HKRSoft.IntegratedQueryModule;
using System.Data;

public partial class IQMQueryHelper : System.Web.UI.UserControl
{
    public IQMData IQMData { get; set; }

    public List<IQMData> IQMDataList { get; set; }

    private List<IQMDataValueUrlRef> dataValueUrlRef;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (this.IQMDataList != null)
            {
                this.IQMDataList.ForEach(x => drpIqmDataList.Items.Add(x.IQMDataTable.Name));
                pnlIqmDataList.Visible = true;
                this.IQMData = this.IQMDataList.Find(x => x.IQMDataTable.Name.Equals(drpIqmDataList.SelectedValue));
            }
            else
            {
                pnlIqmDataList.Visible = false;
            }

            foreach (IQMDataColumn column in this.IQMData.IQMDataTable.DataColumns)
            {
                ListItem item = new ListItem(column.Alias, column.Name);
                lstItems.Items.Add(item);
                if (!column.IsSumColumn)
                {
                    ListItem itemGroupBy = new ListItem(column.Alias, column.Name);
                    cblGroupBy.Items.Add(itemGroupBy);
                }
            }

            lstItems.Items[0].Selected = true;
            ShowValueInputPanelByItemType();
            ResetGroupByCheckBoxList();

            btnResetGrid.Enabled = false;
            btnGroupByClose.Enabled = false;
            pnlGroupBy.Visible = false;
        }

        if (drpIqmDataList.SelectedItem != null)
        {
            this.IQMData = this.IQMDataList.Find(x => x.IQMDataTable.Name.Equals(drpIqmDataList.SelectedValue));
        }

        objDataSource.TypeName = this.IQMData.GetType().Namespace + "." + this.IQMData.GetType().Name;
        objDataSource.SelectMethod = "GetData";
        objDataSource.SelectCountMethod = "GetDataCount";
        objDataSource.SelectParameters.Clear();
        objDataSource.SelectParameters.Add(new ControlParameter("sqlWhereClause", "hidSqlWhereClause", "Value"));
        objDataSource.SelectParameters.Add(new ControlParameter("sqlGroupByClause", "hidSqlGroupByClause", "Value"));
        objDataSource.StartRowIndexParameterName = "startRowIndex";
        objDataSource.MaximumRowsParameterName = "maximumRows";
        objDataSource.EnablePaging = true;
        grdView.PageSize = int.Parse(drpPage.SelectedValue);
        grdView.Caption = "<span style='font-family: 黑体; font-size: 14pt;'>" + this.IQMData.IQMDataTable.Name + "</span>";

        dataValueUrlRef = this.IQMData.GetValueUrlRefList(hidSqlWhereClause.Value);
    }

    protected void lstItems_SelectedIndexChanged(object sender, EventArgs e)
    {
        ShowValueInputPanelByItemType();
    }

    protected void btnTextValue_Click(object sender, EventArgs e)
    {
        TextValueButtonClick();
        ShowValueInputPanelByItemType();
    }

    protected void btnMoneyValue_Click(object sender, EventArgs e)
    {
        MoneyValueButtonClick();
        ShowValueInputPanelByItemType();
    }

    protected void btnDateValue_Click(object sender, EventArgs e)
    {
        DateValueButtonClick();
        ShowValueInputPanelByItemType();
    }

    protected void btnNumberValue_Click(object sender, EventArgs e)
    {
        NumberValueButtonClick();
        ShowValueInputPanelByItemType();
    }

    protected void btnAliasValue_Click(object sender, EventArgs e)
    {
        AliasValueButtonClick();
        ShowValueInputPanelByItemType();
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        ResetControls();
    }

    protected void btnOk_Click(object sender, EventArgs e)
    {
        hidSqlWhereClause.Value = hidResult.Value;
        hidSqlGroupByClause.Value = string.Empty;
        ResetControls();
        if (string.IsNullOrEmpty(hidSqlWhereClause.Value))
        {
            btnResetGrid.Enabled = false;
        }
        else
        {
            btnResetGrid.Enabled = true;
        }
    }

    protected void btnResetGrid_Click(object sender, EventArgs e)
    {
        hidSqlWhereClause.Value = string.Empty;
        hidSqlGroupByClause.Value = string.Empty;
        btnResetGrid.Enabled = false;
        objDataSource.DataBind();
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ResetControls();
    }

    private void GenerateLogical(out string logical, out string logicalSql)
    {
        logical = string.Empty;
        logicalSql = string.Empty;
        if (rbAnd.Checked)
        {
            logical = rbAnd.Text;
            logicalSql = "AND";
        }
        else if (rbOr.Checked)
        {
            logical = rbOr.Text;
            logicalSql = "OR";
        }
        else if (rbAndNot.Checked)
        {
            logical = rbAndNot.Text;
            logicalSql = "AND NOT";
        }
        else if (rbOrNot.Checked)
        {
            logical = rbOrNot.Text;
            logicalSql = "OR NOT";
        }
    }

    private void GenerateTextRange(out string range, out string rangeSql)
    {
        range = string.Empty;
        rangeSql = string.Empty;
        if (rbLikeAll.Checked)
        {
            range = rbLikeAll.Text;
            rangeSql = "LIKE '%" + txtTextValue.Text + "%'";
        }
        else if (rbLikeFront.Checked)
        {
            range = rbLikeFront.Text;
            rangeSql = "LIKE '" + txtTextValue.Text + "%'";
        }
        else if (rbLikeBehind.Checked)
        {
            range = rbLikeBehind.Text;
            rangeSql = "LIKE '%" + txtTextValue.Text + "'";
        }
        else if (rbEqual.Checked)
        {
            range = rbEqual.Text;
            rangeSql = "= '" + txtTextValue.Text + "'";
        }
    }

    private void GenerateItemName(out string itemName, out string itemNameSql)
    {
        itemName = lstItems.SelectedItem.Text;
        itemNameSql = lstItems.SelectedValue;
    }

    private void ResetControls()
    {
        hidResult.Value = string.Empty;
        txtResult.Text = string.Empty;

        foreach (ListItem item in lstItems.Items)
        {
            item.Selected = false;
        }

        lstItems.Items[0].Selected = true;
        ShowValueInputPanelByItemType();
    }

    private void ShowValueInputPanelByItemType()
    {
        txtTextValue.Text = string.Empty;
        txtMoneyValueStart.Text = string.Empty;
        txtMoneyValueEnd.Text = string.Empty;
        txtDateValueStart.Text = string.Empty;
        txtDateValueEnd.Text = string.Empty;
        txtNumberValueStart.Text = string.Empty;
        txtNumberValueEnd.Text = string.Empty;

        pnlTextValue.Visible = false;
        pnlMoneyValue.Visible = false;
        pnlDateValue.Visible = false;
        pnlAliasValue.Visible = false;
        pnlNumberValue.Visible = false;

        rbAnd.Checked = true;
        rbOr.Checked = false;
        rbAndNot.Checked = false;
        rbOrNot.Checked = false;

        rbLikeAll.Checked = true;
        rbLikeFront.Checked = false;
        rbLikeBehind.Checked = false;
        rbEqual.Checked = false;

        if (string.IsNullOrEmpty(txtResult.Text))
        {
            rbAnd.Enabled = true;
            rbOr.Enabled = false;
            rbAndNot.Enabled = true;
            rbOrNot.Enabled = false;
        }
        else
        {
            rbAnd.Enabled = true;
            rbOr.Enabled = true;
            rbAndNot.Enabled = true;
            rbOrNot.Enabled = true;
        }

        rbLikeAll.Enabled = false;
        rbLikeFront.Enabled = false;
        rbLikeBehind.Enabled = false;
        rbEqual.Enabled = false;

        IQMDataColumn column = this.IQMData.IQMDataTable.DataColumns.Find(x => lstItems.SelectedValue.Equals(x.Name));
        switch (column.ColumnType)
        {
            case IQMDataColumn.IQMDataColumnType.Text:
                pnlTextValue.Visible = true;
                rbLikeAll.Enabled = true;
                rbLikeFront.Enabled = true;
                rbLikeBehind.Enabled = true;
                rbEqual.Enabled = true;
                break;
            case IQMDataColumn.IQMDataColumnType.Date:
                pnlDateValue.Visible = true;
                break;
            case IQMDataColumn.IQMDataColumnType.Money:
                pnlMoneyValue.Visible = true;
                break;
            case IQMDataColumn.IQMDataColumnType.YearOrMonth:
            case IQMDataColumn.IQMDataColumnType.Number:
                pnlNumberValue.Visible = true;
                break;
            case IQMDataColumn.IQMDataColumnType.ValueAlias:
                drpAliasValue.Items.Clear();
                drpAliasValue.Items.Add(new ListItem(string.Empty, string.Empty));
                if (column.ValueAlias.Count != 0)
                {
                    column.ValueAlias.ForEach(x => drpAliasValue.Items.Add(new ListItem(x.Alias, x.Value)));
                }
                pnlAliasValue.Visible = true;
                break;
        }
    }

    private void GenerateQueryStingWhenValueIsEmpty(string itemName, string itemNameSql, out string logical, out string logicalSql)
    {
        logical = string.Empty;
        logicalSql = string.Empty;
        if (txtResult.Text == string.Empty)
        {
            if (rbAndNot.Checked)
            {
                txtResult.Text += "<" + itemName + "> 不等于 '空值'";
                hidResult.Value += "WHERE (NOT (" + itemNameSql + " IS NULL) OR (RTRIM(" + itemNameSql + ") = ''))";
            }
            else
            {
                txtResult.Text += "<" + itemName + "> 等于 '空值'";
                hidResult.Value += "WHERE (" + itemNameSql + " IS NULL) OR (RTRIM(" + itemNameSql + ") = '')";
            }
        }
        else
        {
            GenerateLogical(out logical, out logicalSql);
            txtResult.Text += " | " + logical + " <" + itemName + "> 等于 '空值'";
            hidResult.Value += " " + logicalSql + " (" + itemNameSql + " IS NULL) OR (RTRIM(" + itemNameSql + ") = '')";
        }
    }

    private void TextValueButtonClick()
    {
        string itemName;
        string itemNameSql;
        string logical;
        string logicalSql;
        string range;
        string rangeSql;

        GenerateItemName(out itemName, out itemNameSql);
        GenerateTextRange(out range, out rangeSql);

        if (string.IsNullOrEmpty(txtTextValue.Text))
        {
            GenerateQueryStingWhenValueIsEmpty(itemName, itemNameSql, out logical, out logicalSql);
        }
        else
        {
            if (txtResult.Text == string.Empty)
            {
                txtResult.Text += "<" + itemName + "> " + range + " '" + txtTextValue.Text + "'";
                hidResult.Value += "WHERE (" + itemNameSql + " " + rangeSql + ")";
            }
            else
            {
                GenerateLogical(out logical, out logicalSql);
                txtResult.Text += " | " + logical + " <" + itemName + "> " + range + " '" + txtTextValue.Text + "'";
                hidResult.Value += " " + logicalSql + " (" + itemNameSql + " " + rangeSql + ")";
            }
        }
    }

    private void MoneyValueButtonClick()
    {
        string itemName;
        string itemNameSql;
        string logical;
        string logicalSql;

        GenerateItemName(out itemName, out itemNameSql);

        if (string.IsNullOrEmpty(txtMoneyValueStart.Text) && string.IsNullOrEmpty(txtMoneyValueEnd.Text))
        {
            GenerateQueryStingWhenValueIsEmpty(itemName, itemNameSql, out logical, out logicalSql);
        }
        else if (!string.IsNullOrEmpty(txtMoneyValueStart.Text) && !string.IsNullOrEmpty(txtMoneyValueEnd.Text))
        {
            if (txtResult.Text == string.Empty)
            {
                txtResult.Text += "<" + itemName + "> 从 '" + txtMoneyValueStart.Text + "' 到 '" + txtMoneyValueEnd.Text + "'";
                hidResult.Value += "WHERE (" + itemNameSql + " >= '" + txtMoneyValueStart.Text + "' AND " + itemNameSql + " <= '" + txtMoneyValueEnd.Text + "')";
            }
            else
            {
                GenerateLogical(out logical, out logicalSql);
                txtResult.Text += " | " + logical + " <" + itemName + "> 从 '" + txtMoneyValueStart.Text + "' 到 '" + txtMoneyValueEnd.Text + "'";
                hidResult.Value += " " + logicalSql + " (" + itemNameSql + " >= '" + txtMoneyValueStart.Text + "' AND " + itemNameSql + " <= '" + txtMoneyValueEnd.Text + "')";
            }
        }
        else if (!string.IsNullOrEmpty(txtMoneyValueStart.Text) && string.IsNullOrEmpty(txtMoneyValueEnd.Text))
        {
            if (txtResult.Text == string.Empty)
            {
                txtResult.Text += "<" + itemName + "> 等于 '" + txtMoneyValueStart.Text + "'";
                hidResult.Value += "WHERE (" + itemNameSql + " = '" + txtMoneyValueStart.Text + "')";
            }
            else
            {
                GenerateLogical(out logical, out logicalSql);
                txtResult.Text += " | " + logical + " <" + itemName + "> 等于 '" + txtMoneyValueStart.Text + "'";
                hidResult.Value += " " + logicalSql + " (" + itemNameSql + " = '" + txtMoneyValueStart.Text + "')";
            }
        }
        else if (string.IsNullOrEmpty(txtMoneyValueStart.Text) && !string.IsNullOrEmpty(txtMoneyValueEnd.Text))
        {
            if (txtResult.Text == string.Empty)
            {
                txtResult.Text += "<" + itemName + "> 等于 '" + txtMoneyValueEnd.Text + "'";
                hidResult.Value += "WHERE (" + itemNameSql + " = '" + txtMoneyValueEnd.Text + "')";
            }
            else
            {
                GenerateLogical(out logical, out logicalSql);
                txtResult.Text += " | " + logical + " <" + itemName + "> 等于 '" + txtMoneyValueEnd.Text + "'";
                hidResult.Value += " " + logicalSql + " (" + itemNameSql + " = '" + txtMoneyValueEnd.Text + "')";
            }
        }
    }

    private void NumberValueButtonClick()
    {
        string itemName;
        string itemNameSql;
        string logical;
        string logicalSql;

        GenerateItemName(out itemName, out itemNameSql);

        if (string.IsNullOrEmpty(txtNumberValueStart.Text) && string.IsNullOrEmpty(txtNumberValueEnd.Text))
        {
            GenerateQueryStingWhenValueIsEmpty(itemName, itemNameSql, out logical, out logicalSql);
        }
        else if (!string.IsNullOrEmpty(txtNumberValueStart.Text) && !string.IsNullOrEmpty(txtNumberValueEnd.Text))
        {
            if (txtResult.Text == string.Empty)
            {
                txtResult.Text += "<" + itemName + "> 从 '" + txtNumberValueStart.Text + "' 到 '" + txtNumberValueEnd.Text + "'";
                hidResult.Value += "WHERE (" + itemNameSql + " >= '" + txtNumberValueStart.Text + "' AND " + itemNameSql + " <= '" + txtNumberValueEnd.Text + "')";
            }
            else
            {
                GenerateLogical(out logical, out logicalSql);
                txtResult.Text += " | " + logical + " <" + itemName + "> 从 '" + txtNumberValueStart.Text + "' 到 '" + txtNumberValueEnd.Text + "'";
                hidResult.Value += " " + logicalSql + " (" + itemNameSql + " >= '" + txtNumberValueStart.Text + "' AND " + itemNameSql + " <= '" + txtNumberValueEnd.Text + "')";
            }
        }
        else if (!string.IsNullOrEmpty(txtNumberValueStart.Text) && string.IsNullOrEmpty(txtNumberValueEnd.Text))
        {
            if (txtResult.Text == string.Empty)
            {
                txtResult.Text += "<" + itemName + "> 等于 '" + txtNumberValueStart.Text + "'";
                hidResult.Value += "WHERE (" + itemNameSql + " = '" + txtNumberValueStart.Text + "')";
            }
            else
            {
                GenerateLogical(out logical, out logicalSql);
                txtResult.Text += " | " + logical + " <" + itemName + "> 等于 '" + txtNumberValueStart.Text + "'";
                hidResult.Value += " " + logicalSql + " (" + itemNameSql + " = '" + txtNumberValueStart.Text + "')";
            }
        }
        else if (string.IsNullOrEmpty(txtNumberValueStart.Text) && !string.IsNullOrEmpty(txtNumberValueEnd.Text))
        {
            if (txtResult.Text == string.Empty)
            {
                txtResult.Text += "<" + itemName + "> 等于 '" + txtNumberValueEnd.Text + "'";
                hidResult.Value += "WHERE (" + itemNameSql + " = '" + txtNumberValueEnd.Text + "')";
            }
            else
            {
                GenerateLogical(out logical, out logicalSql);
                txtResult.Text += " | " + logical + " <" + itemName + "> 等于 '" + txtNumberValueEnd.Text + "'";
                hidResult.Value += " " + logicalSql + " (" + itemNameSql + " = '" + txtNumberValueEnd.Text + "')";
            }
        }
    }

    private void DateValueButtonClick()
    {
        string itemName;
        string itemNameSql;
        string logical;
        string logicalSql;

        GenerateItemName(out itemName, out itemNameSql);

        if (string.IsNullOrEmpty(txtDateValueStart.Text) && string.IsNullOrEmpty(txtDateValueEnd.Text))
        {
            GenerateQueryStingWhenValueIsEmpty(itemName, itemNameSql, out logical, out logicalSql);
        }
        else if (!string.IsNullOrEmpty(txtDateValueStart.Text) && !string.IsNullOrEmpty(txtDateValueEnd.Text))
        {
            if (txtResult.Text == string.Empty)
            {
                txtResult.Text += "<" + itemName + "> 从 '" + txtDateValueStart.Text + "' 到 '" + txtDateValueEnd.Text + "'";
                hidResult.Value += "WHERE (" + itemNameSql + " >= '" + txtDateValueStart.Text + "' AND " + itemNameSql + " <= '" + txtDateValueEnd.Text + "')";
            }
            else
            {
                GenerateLogical(out logical, out logicalSql);
                txtResult.Text += " | " + logical + " <" + itemName + "> 从 '" + txtDateValueStart.Text + "' 到 '" + txtDateValueEnd.Text + "'";
                hidResult.Value += " " + logicalSql + " (" + itemNameSql + " >= '" + txtDateValueStart.Text + "' AND " + itemNameSql + " <= '" + txtDateValueEnd.Text + "')";
            }
        }
        else if (!string.IsNullOrEmpty(txtDateValueStart.Text) && string.IsNullOrEmpty(txtDateValueEnd.Text))
        {
            if (txtResult.Text == string.Empty)
            {
                txtResult.Text += "<" + itemName + "> 等于 '" + txtDateValueStart.Text + "'";
                hidResult.Value += "WHERE (" + itemNameSql + " = '" + txtDateValueStart.Text + "')";
            }
            else
            {
                GenerateLogical(out logical, out logicalSql);
                txtResult.Text += " | " + logical + " <" + itemName + "> 等于 '" + txtDateValueStart.Text + "'";
                hidResult.Value += " " + logicalSql + " (" + itemNameSql + " = '" + txtDateValueStart.Text + "')";
            }
        }
        else if (string.IsNullOrEmpty(txtDateValueStart.Text) && !string.IsNullOrEmpty(txtDateValueEnd.Text))
        {
            if (txtResult.Text == string.Empty)
            {
                txtResult.Text += "<" + itemName + "> 等于 '" + txtDateValueEnd.Text + "'";
                hidResult.Value += "WHERE (" + itemNameSql + " = '" + txtDateValueEnd.Text + "')";
            }
            else
            {
                GenerateLogical(out logical, out logicalSql);
                txtResult.Text += " | " + logical + " <" + itemName + "> 等于 '" + txtDateValueEnd.Text + "'";
                hidResult.Value += " " + logicalSql + " (" + itemNameSql + " = '" + txtDateValueEnd.Text + "')";
            }
        }
    }

    private void AliasValueButtonClick()
    {
        string itemName;
        string itemNameSql;
        string logical;
        string logicalSql;

        GenerateItemName(out itemName, out itemNameSql);

        if (string.IsNullOrEmpty(drpAliasValue.SelectedValue))
        {
            GenerateQueryStingWhenValueIsEmpty(itemName, itemNameSql, out logical, out logicalSql);
        }
        else
        {
            if (txtResult.Text == string.Empty)
            {
                txtResult.Text += "<" + itemName + "> 等于 '" + drpAliasValue.SelectedItem.Text + "'";
                hidResult.Value += "WHERE (" + itemNameSql + " = '" + drpAliasValue.SelectedValue + "')";
            }
            else
            {
                GenerateLogical(out logical, out logicalSql);
                txtResult.Text += " | " + logical + " <" + itemName + "> 等于 '" + drpAliasValue.SelectedItem.Text + "'";
                hidResult.Value += " " + logicalSql + " (" + itemNameSql + " = '" + drpAliasValue.SelectedValue + "')";
            }
        }
    }

    protected void btnGroupBy_Click(object sender, EventArgs e)
    {
        pnlGroupBy.Visible = true;
    }

    protected void btnGroupByCancel_Click(object sender, EventArgs e)
    {
        pnlGroupBy.Visible = false;
    }

    protected void btnGroupByReset_Click(object sender, EventArgs e)
    {
        ResetGroupByCheckBoxList();
    }

    private void ResetGroupByCheckBoxList()
    {
        foreach (ListItem item in cblGroupBy.Items)
        {
            item.Selected = false;
        }
    }

    protected void btnGroupByOk_Click(object sender, EventArgs e)
    {
        string groupByString = string.Empty;
        foreach (ListItem item in cblGroupBy.Items)
        {
            if (item.Selected)
            {
                groupByString += item.Value + ",";
            }
        }
        if (!string.IsNullOrEmpty(groupByString))
        {
            groupByString = groupByString.Substring(0, groupByString.Length - 1);
        }
        hidSqlGroupByClause.Value = groupByString;
        pnlGroupBy.Visible = false;

        if (string.IsNullOrEmpty(hidSqlGroupByClause.Value))
        {
            btnGroupByClose.Enabled = false;
        }
        else
        {
            btnGroupByClose.Enabled = true;
        }
    }

    protected void btnGroupByClose_Click(object sender, EventArgs e)
    {
        hidSqlGroupByClause.Value = string.Empty;
        pnlGroupBy.Visible = false;
        btnGroupByClose.Enabled = false;
    }

    protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
            DataRowView rowView = (DataRowView)e.Row.DataItem;
            DataColumnCollection columns = rowView.Row.Table.Columns;
            for (int i = 1; i <= columns.Count - 1; i++)
            {
                TableCell cell = e.Row.Cells[i];

                IQMDataColumn iqmDataColumn = this.IQMData.IQMDataTable.DataColumns.Find(x => x.Alias.Equals(columns[i].ColumnName));
                cell.HorizontalAlign = iqmDataColumn.HorizontalAlign;

                if (cell.Text.Equals("&nbsp;"))
                {
                    cell.Text = "-";
                }
                else
                {
                    if (iqmDataColumn.ValueUrl != null)
                    {
                        IQMDataValueUrlRef urlRef = this.dataValueUrlRef.Find(x => x.RowNumber.Equals(e.Row.Cells[0].Text) && x.ColumnName.Equals(columns[i].ColumnName));
                        HyperLink link = new HyperLink();
                        link.Text = cell.Text;
                        link.NavigateUrl = urlRef.Url;
                        cell.Controls.Add(link);
                    }
                }
            }
        }
    }

    protected void grdView_DataBound(object sender, EventArgs e)
    {
        lblSumResult.Text = "符合条件的记录数：" + this.IQMData.GetDataCount(hidSqlWhereClause.Value, hidSqlGroupByClause.Value).ToString();

        if (this.IQMData.IQMDataTable.DataColumns.Exists(x => x.IsSumColumn))
        {
            Dictionary<string, List<string>> dictColumnValue = new Dictionary<string, List<string>>();

            DataSet dataSet = this.IQMData.GetSumData(hidSqlWhereClause.Value, hidSqlGroupByClause.Value);

            DataRow row = dataSet.Tables[this.IQMData.IQMDataTable.Name].Rows[0];

            foreach (IQMDataColumn dataColumn in this.IQMData.IQMDataTable.DataColumns.FindAll(x => x.IsSumColumn))
            {
                if (dataColumn.ColumnType == IQMDataColumn.IQMDataColumnType.Money)
                {
                    string sumResult = dataColumn.Alias + "总计：" + decimal.Parse(row[dataColumn.Alias].ToString()).ToString("N2");
                    lblSumResult.Text += "<br />" + sumResult;
                }
                else
                {
                    string sumResult = dataColumn.Alias + "总计：" + int.Parse(row[dataColumn.Alias].ToString());
                    lblSumResult.Text += "<br />" + sumResult;
                }
            }
        }
    }

    protected void drpIqmDataList_SelectedIndexChanged(object sender, EventArgs e)
    {
        lstItems.Items.Clear();
        cblGroupBy.Items.Clear();
        foreach (IQMDataColumn column in this.IQMData.IQMDataTable.DataColumns)
        {
            ListItem item = new ListItem(column.Alias, column.Name);
            lstItems.Items.Add(item);
            if (!column.IsSumColumn)
            {
                ListItem itemGroupBy = new ListItem(column.Alias, column.Name);
                cblGroupBy.Items.Add(itemGroupBy);
            }
        }
    }
    protected void btnExportToExcel_Click(object sender, EventArgs e)
    {
        DataSet dataSet = this.IQMData.GetData(hidSqlWhereClause.Value, hidSqlGroupByClause.Value);

        string tableName = this.IQMData.IQMDataTable.Name;
        DataTable table = dataSet.Tables[tableName];

        HttpResponse response = Page.Response;
        response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
        response.ContentType = "application/vnd.ms-excel";
        response.AppendHeader("Content-Disposition", string.Format("attachment; filename=Result.xls"));

        string excelTitle = tableName + "\n";

        string excelColumnNames = string.Empty;
        int columnCount = table.Columns.Count;
        for (int i = 0; i < columnCount; i++)
        {
            if (i == (columnCount - 1))
            {
                excelColumnNames += table.Columns[i].ColumnName + "\n";
            }
            else
            {
                excelColumnNames += table.Columns[i].ColumnName + "\t";
            }
        }

        string excelRows = string.Empty;
        foreach (DataRow row in table.Rows)
        {
            int rowLength = row.ItemArray.Length;
            for (int i = 0; i < rowLength; i++)
            {
                if (i == (rowLength - 1))
                {
                    excelRows += row[i].ToString().Trim() + "\n";
                }
                else
                {
                    excelRows += row[i].ToString().Trim() + "\t";
                }
            }
        }

        string[] sumResults = lblSumResult.Text.Split(new string[] { "<br />" }, StringSplitOptions.RemoveEmptyEntries);
        string excelBottom = string.Empty;
        foreach (string result in sumResults)
        {
            excelBottom += result + "\n";
        }

        string excelContent = excelTitle + excelColumnNames + excelRows + excelBottom;

        response.Write(excelContent);

        response.End();
    }
}