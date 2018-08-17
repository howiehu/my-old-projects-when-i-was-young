<%@ Control Language="C#" AutoEventWireup="true" CodeFile="IQMQueryHelper.ascx.cs"
    Inherits="IQMQueryHelper" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<style type="text/css">
    .HelperDiv
    {
        width: 600px;
        height: 400px;
        font-size: 10pt;
        background-color: #E7E7FF;
        border-width: 3px;
        border-style: solid;
        border-color: Gray;
        padding: 3px;
    }
    .HelperTitleText
    {
        color: #FFFFFF;
    }
    .modalBackground
    {
        background-color: Gray;
        filter: alpha(opacity=70);
        opacity: 0.7;
    }
</style>
<asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnableScriptGlobalization="True" CombineScripts="false">
    </asp:ToolkitScriptManager>
<asp:Panel ID="pnlQueryHelper" runat="server">
    <div class="HelperDiv">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <table style="width: 100%;">
                    <tr>
                        <th colspan="6" style="background-color: #4A3C8C;">
                            <span class="HelperTitleText">组合条件查询助手</span>
                        </th>
                    </tr>
                    <tr>
                        <th style="width: 140">
                            项目列表
                        </th>
                        <th style="width: 60">
                            查询条件
                        </th>
                        <td style="width: 100">
                            <asp:RadioButton ID="rbAnd" runat="server" Text="并且" GroupName="logicalRadioButtons"
                                Checked="true" />
                        </td>
                        <td style="width: 100">
                            <asp:RadioButton ID="rbOr" runat="server" Text="或者" GroupName="logicalRadioButtons" />
                        </td>
                        <td style="width: 100">
                            <asp:RadioButton ID="rbAndNot" runat="server" Text="并且不是" GroupName="logicalRadioButtons" />
                        </td>
                        <td style="width: 100">
                            <asp:RadioButton ID="rbOrNot" runat="server" Text="或者不是" GroupName="logicalRadioButtons" />
                        </td>
                    </tr>
                    <tr>
                        <td rowspan="4" style="width: 140; vertical-align: top;">
                            <asp:ListBox ID="lstItems" runat="server" Width="100%" Rows="20" AutoPostBack="True"
                                OnSelectedIndexChanged="lstItems_SelectedIndexChanged" />
                        </td>
                        <th>
                            查询精度
                        </th>
                        <td>
                            <asp:RadioButton ID="rbLikeAll" runat="server" Text="包含" GroupName="rangeRadioButtons"
                                Checked="true" />
                        </td>
                        <td>
                            <asp:RadioButton ID="rbLikeFront" runat="server" Text="头部包含" GroupName="rangeRadioButtons" />
                        </td>
                        <td>
                            <asp:RadioButton ID="rbLikeBehind" runat="server" Text="尾部包含" GroupName="rangeRadioButtons" />
                        </td>
                        <td>
                            <asp:RadioButton ID="rbEqual" runat="server" Text="等于" GroupName="rangeRadioButtons" />
                        </td>
                    </tr>
                    <tr>
                        <th>
                            查询内容
                        </th>
                        <td colspan="4" style="text-align: center;">
                            <asp:Panel ID="pnlTextValue" runat="server">
                                <asp:TextBox ID="txtTextValue" runat="server" Width="210" />
                                &nbsp;
                                <asp:Button ID="btnTextValue" runat="server" Text="添加" OnClick="btnTextValue_Click"
                                    BorderStyle="Solid" BorderWidth="1px" />
                            </asp:Panel>
                            <asp:Panel ID="pnlMoneyValue" runat="server">
                                从&nbsp;
                                <asp:TextBox ID="txtMoneyValueStart" runat="server" Width="80" Style="text-align: right" />
                                <asp:FilteredTextBoxExtender ID="txtMoneyValueStart_FilteredTextBoxExtender" runat="server"
                                    Enabled="True" TargetControlID="txtMoneyValueStart" FilterType="Custom,Numbers"
                                    InvalidChars=".">
                                </asp:FilteredTextBoxExtender>
                                &nbsp;到&nbsp;
                                <asp:TextBox ID="txtMoneyValueEnd" runat="server" Width="80" Style="text-align: right" />
                                <asp:FilteredTextBoxExtender ID="txtMoneyValueEnd_FilteredTextBoxExtender" runat="server"
                                    Enabled="True" TargetControlID="txtMoneyValueEnd" FilterType="Custom,Numbers"
                                    InvalidChars=".">
                                </asp:FilteredTextBoxExtender>
                                &nbsp;
                                <asp:Button ID="btnMoneyValue" runat="server" Text="添加" OnClick="btnMoneyValue_Click"
                                    BorderStyle="Solid" BorderWidth="1px" />
                            </asp:Panel>
                            <asp:Panel ID="pnlDateValue" runat="server">
                                从&nbsp;
                                <asp:TextBox ID="txtDateValueStart" runat="server" Width="80" />
                                <asp:FilteredTextBoxExtender ID="txtDateValueStart_FilteredTextBoxExtender" runat="server"
                                    Enabled="True" TargetControlID="txtDateValueStart" FilterType="Custom,Numbers"
                                    InvalidChars="-">
                                </asp:FilteredTextBoxExtender>
                                <asp:CalendarExtender ID="txtDateValueStart_CalendarExtender" runat="server" Enabled="True"
                                    TargetControlID="txtDateValueStart" Format="yyyy-MM-dd">
                                </asp:CalendarExtender>
                                &nbsp;到&nbsp;
                                <asp:TextBox ID="txtDateValueEnd" runat="server" Width="80" />
                                <asp:FilteredTextBoxExtender ID="txtDateValueEnd_FilteredTextBoxExtender" runat="server"
                                    Enabled="True" TargetControlID="txtDateValueEnd" FilterType="Custom,Numbers"
                                    InvalidChars="-">
                                </asp:FilteredTextBoxExtender>
                                <asp:CalendarExtender ID="txtDateValueEnd_CalendarExtender" runat="server" Enabled="True"
                                    TargetControlID="txtDateValueEnd" Format="yyyy-MM-dd">
                                </asp:CalendarExtender>
                                &nbsp;
                                <asp:Button ID="btnDateValue" runat="server" Text="添加" OnClick="btnDateValue_Click"
                                    BorderStyle="Solid" BorderWidth="1px" />
                            </asp:Panel>
                            <asp:Panel ID="pnlNumberValue" runat="server">
                                从&nbsp;
                                <asp:TextBox ID="txtNumberValueStart" runat="server" Width="80" />
                                <asp:FilteredTextBoxExtender ID="txtNumberValueStart_FilteredTextBoxExtender" runat="server"
                                    Enabled="True" TargetControlID="txtNumberValueStart" FilterType="Numbers">
                                </asp:FilteredTextBoxExtender>
                                &nbsp;到&nbsp;
                                <asp:TextBox ID="txtNumberValueEnd" runat="server" Width="80" />
                                <asp:FilteredTextBoxExtender ID="txtNumberValueEnd_FilteredTextBoxExtender" runat="server"
                                    Enabled="True" TargetControlID="txtDateValueEnd" FilterType="Numbers">
                                </asp:FilteredTextBoxExtender>
                                &nbsp;
                                <asp:Button ID="btnNumberValue" runat="server" Text="添加" OnClick="btnNumberValue_Click"
                                    BorderStyle="Solid" BorderWidth="1px" />
                            </asp:Panel>
                            <asp:Panel ID="pnlAliasValue" runat="server">
                                <asp:DropDownList ID="drpAliasValue" runat="server" Width="210" />
                                &nbsp;
                                <asp:Button ID="btnAliasValue" runat="server" Text="添加" OnClick="btnAliasValue_Click"
                                    BorderStyle="Solid" BorderWidth="1px" />
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <th rowspan="2">
                            逻辑预览
                        </th>
                        <td colspan="4">
                            <asp:TextBox ID="txtResult" runat="server" Width="97%" Height="100%" ReadOnly="True"
                                TextMode="MultiLine" Rows="12" />
                            <asp:HiddenField ID="hidResult" runat="server" />
                            <asp:HiddenField ID="hidSqlWhereClause" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="4">
                            <asp:Button ID="btnReset" runat="server" Text="重置" OnClick="btnReset_Click" BorderStyle="Solid"
                                BorderWidth="1px" />
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div style="width: 100%; text-align: center; background-color: #B5C7DE;">
            <asp:Button ID="btnOk" runat="server" Text="查询" OnClick="btnOk_Click" BorderStyle="Solid"
                BorderWidth="1px" />
            &nbsp;
            <asp:Button ID="btnCancel" runat="server" Text="取消" OnClick="btnCancel_Click" BorderStyle="Solid"
                BorderWidth="1px" />
        </div>
    </div>
</asp:Panel>
<asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" PopupControlID="pnlQueryHelper"
    TargetControlID="btnQuery" BackgroundCssClass="modalBackground" DropShadow="True">
</asp:ModalPopupExtender>
<div style="font-size: 10pt">
    <asp:Panel ID="pnlIqmDataList" runat="server">
        查询内容分类：
        <asp:DropDownList ID="drpIqmDataList" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpIqmDataList_SelectedIndexChanged" />
        <br />
        <br />
    </asp:Panel>
    每页显示行数：
    <asp:DropDownList ID="drpPage" runat="server" AutoPostBack="True">
        <asp:ListItem>10</asp:ListItem>
        <asp:ListItem Selected="True">20</asp:ListItem>
        <asp:ListItem>50</asp:ListItem>
        <asp:ListItem>100</asp:ListItem>
    </asp:DropDownList>
    <asp:HiddenField ID="hidSqlGroupByClause" runat="server" />
    &nbsp;
    <asp:Button ID="btnResetGrid" runat="server" Text="重置查询结果" OnClick="btnResetGrid_Click"
        BorderStyle="Solid" BorderWidth="1px" />
    &nbsp;
    <asp:Button ID="btnQuery" runat="server" Text="组合条件查询" BorderStyle="Solid" BorderWidth="1px" />
    &nbsp;
    <asp:Button ID="btnGroupBy" runat="server" Text="汇总查询结果" BorderStyle="Solid" BorderWidth="1px"
        OnClick="btnGroupBy_Click" />
    &nbsp;
    <asp:Button ID="btnGroupByClose" runat="server" Text="关闭汇总结果" BorderStyle="Solid"
        BorderWidth="1px" OnClick="btnGroupByClose_Click" />
        &nbsp;
    <asp:Button ID="btnExportToExcel" runat="server" Text="将结果导出为Excel" BorderStyle="Solid"
        BorderWidth="1px" onclick="btnExportToExcel_Click" />
    <br />
    <br />
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlGroupBy" runat="server">
                <asp:CheckBoxList ID="cblGroupBy" runat="server" RepeatDirection="Horizontal" RepeatColumns="5" />
                <br />
                <asp:Button ID="btnGroupByOk" runat="server" Text="确认" BorderStyle="Solid" BorderWidth="1px"
                    OnClick="btnGroupByOk_Click" />
                &nbsp;
                <asp:Button ID="btnGroupByReset" runat="server" Text="重置" BorderStyle="Solid" BorderWidth="1px"
                    OnClick="btnGroupByReset_Click" />
                &nbsp;
                <asp:Button ID="btnGroupByCancel" runat="server" Text="取消" BorderStyle="Solid" BorderWidth="1px"
                    OnClick="btnGroupByCancel_Click" />
                <br />
                <br />
            </asp:Panel>
            <asp:GridView ID="grdView" runat="server" AllowPaging="True" DataSourceID="objDataSource"
                EnableModelValidation="True" BackColor="White" BorderColor="#E7E7FF" BorderWidth="1px"
                CellPadding="3" EmptyDataText="没有查询到符合条件的数据！" ShowFooter="True" Caption="标题"
                OnRowDataBound="grdView_RowDataBound" OnDataBound="grdView_DataBound">
                <AlternatingRowStyle BackColor="#F7F7F7" Wrap="False" />
                <EditRowStyle Wrap="False" />
                <EmptyDataRowStyle Wrap="False" BackColor="#E7E7FF" ForeColor="#4A3C8C" />
                <FooterStyle BackColor="#B5C7DE" ForeColor="#4A3C8C" Wrap="False" />
                <HeaderStyle BackColor="#4A3C8C" Font-Bold="True" ForeColor="#F7F7F7" Wrap="False" />
                <PagerSettings Position="TopAndBottom" />
                <PagerStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" HorizontalAlign="Left" Wrap="False" />
                <RowStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" Wrap="False" />
                <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="#F7F7F7" Wrap="False" />
            </asp:GridView>
            <asp:ObjectDataSource ID="objDataSource" runat="server"></asp:ObjectDataSource>
            <br />
            <asp:Label ID="lblSumResult" runat="server" Text="统计信息"></asp:Label>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="drpPage" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="btnGroupBy" EventName="Click" />
            <%--<asp:AsyncPostBackTrigger ControlID="btnGroupByClose" EventName="Click" />--%>
            <asp:PostBackTrigger ControlID="btnGroupByOk" />
        </Triggers>
    </asp:UpdatePanel>
</div>
