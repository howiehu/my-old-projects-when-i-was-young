/// <reference path="jquery.min.js" />

var jsonColumns;
var jsonDataTable;

/* ==================== 页面加载 ==================== */
$(document).ready(function () {
    $("#div_iqm_ui").load("../IQMPages/IQMHtmlCode.htm #div_iqm_ui", function () {
        CreateDialogs();
        CreateTabs();
        CreateSearchToolbar();
        CreatePageNumberToolbar();
        CreateSearchContentButtons();
    });
});

/* ==================== 创建对话框功能 ==================== */
function CreateDialogs() {
    $("#div_dialog_exportexcel").dialog({
        autoOpen: false,
        width: 400,
        height: 260,
        resizable: false,
        modal: true,
        open: function (event, ui) {
            $("#button_getexcel").button({ disabled: false });
            $("#link_excel").hide();
        }
    });

    $("#div_dialog_noxml").dialog({
        autoOpen: false,
        height: 150,
        resizable: false,
        modal: true
    });

    $("#div_dialog_nocolumns").dialog({
        autoOpen: false,
        height: 150,
        resizable: false,
        modal: true
    });

    $("#div_dialog_nosearchcontent").dialog({
        autoOpen: false,
        height: 120,
        resizable: false,
        modal: true
    });

    $("#div_dialog_nogroupbycontent").dialog({
        autoOpen: false,
        height: 120,
        resizable: false,
        modal: true
    });

    $("#div_dialog_pageoutrange").dialog({
        autoOpen: false,
        height: 120,
        resizable: false,
        modal: true
    });

    $("#div_form_search").dialog({
        autoOpen: false,
        height: 480,
        width: 640,
        resizable: false,
        modal: true,
        buttons: {
            "查询": function () {
                if ($("#div_search_preview_text").text() == "") {
                    ShowNoSearchContentDialog();
                }
                else {
                    $("#hidden_whereClause").val($("#hidden_whereClauseTemp").val());
                    $("#hidden_startRowIndex").val("0");
                    $("#hidden_pageIndex").val("1");
                    CreateDataTable();
                    $(this).dialog("close");
                }
            },
            "重置": function () {
                ResetSearchForm();
            },
            "取消": function () {
                $(this).dialog("close");
            }
        }
    });

    $("#div_form_groupby").dialog({
        autoOpen: false,
        height: 480,
        width: 320,
        resizable: false,
        modal: true,
        buttons: {
            "汇总": function () {
                var $items = $("#div_groupby_items :checked");
                if ($items.length > 0) {
                    var groupColumns = "";
                    $items.each(function () {
                        groupColumns += $(this).val() + ",";
                    });
                    groupColumns = groupColumns.substring(0, groupColumns.length - 1);
                    $("#hidden_groupColumns").val(groupColumns);

                    $("#hidden_startRowIndex").val("0");
                    $("#hidden_pageIndex").val("1");
                    CreateDataTable();
                    $("#button_summary_clean").button({ disabled: false });
                    $(this).dialog("close");
                }
                else {
                    ShowNoGroupByContentDialog();
                }
            },
            "重置": function () {
                $("#div_groupby_items :checked").removeAttr("checked");
            },
            "取消": function () {
                $("#div_groupby_items :checked").removeAttr("checked");
                var groupColumns = $("#hidden_groupColumns").val();
                if (groupColumns != "") {
                    var columnArray = groupColumns.split(",");
                    for (var i = 0; i < columnArray.length; i++) {
                        var selector = "#div_groupby_items input[value='" + columnArray[i] + "']";
                        if ($(selector).length > 0) {
                            $(selector).attr("checked", "checked");
                        }
                    }
                }
                $(this).dialog("close");
            }
        }
    });
}

/* ==================== 显示Xml文档读取错误对话框 ==================== */
function ShowExportExcelDialog() {
    $("#div_dialog_exportexcel").dialog("open");
}

/* ==================== 显示Xml文档读取错误对话框 ==================== */
function ShowNoXmlDialog() {
    $("#div_dialog_noxml").dialog("open");
}

/* ==================== 显示列数据读取错误对话框 ==================== */
function ShowNoColumnsDialog() {
    $("#div_dialog_nocolumns").dialog("open");
}

/* ==================== 显示无查询条件错误对话框 ==================== */
function ShowNoSearchContentDialog() {
    $("#div_dialog_nosearchcontent").dialog("open");
}

/* ==================== 显示无汇总项目错误对话框 ==================== */
function ShowNoGroupByContentDialog() {
    $("#div_dialog_nogroupbycontent").dialog("open");
}

/* ==================== 显示分页跳转错误对话框 ==================== */
function ShowPageOutRangeDialog() {
    $("#div_dialog_pageoutrange").dialog("open");
}

/* ==================== 显示组合条件查询窗体 ==================== */
function ShowFormSearch() {
    $("#div_form_search").dialog("open");
}

/* ==================== 显示汇总查询结果窗体 ==================== */
function ShowFormGroupBy() {
    $("#div_form_groupby").dialog("open");
}

/* ==================== 创建标签及相关功能 ==================== */
function CreateTabs() {
    $.ajax({
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        url: "../IQMServices/IQMWebService.asmx/GetTableAlias",
        data: xmlDocNames,
        success: function (data) {
            if (data["d"].length > 0) {
                /* --------- 发送$xmlDocNames中的JSON对象至WebService获得数据表别名并填充标签项 --------- */
                for (var i = 0; i < data["d"].length; i++) {
                    var obj = data["d"][i];
                    var tab = "<li><a href=\"#div_panel_main\">" + obj.TableAlias + "</a><input type=\"hidden\" value=\"" + obj.XmlDocName + "\"></li>";
                    $("#ul_tabs").append(tab);
                }

                /* --------- 将div_tabs转化为JQuery Tabs插件并绑定标签选择事件 --------- */
                $("#div_tabs").tabs({
                    create: function (event, ui) {
                        //从Cookie中获取标签选择状态（保证刷新时标签选择不发生变化）
                        var cookie = $.cookie("IQM_XMLDOCNAME");
                        if (cookie) {
                            for (var i = 0; i < data["d"].length; i++) {
                                var obj = data["d"][i];
                                if (obj.XmlDocName == cookie) {
                                    $("#div_tabs").tabs("select", i);
                                }
                            }
                        }
                    },
                    select: function (event, ui) {
                        var valueold = $("#hidden_xmlDocName").val(); //保存旧的Xml文件名

                        //将被选中标签中的Xml文档名存入相应的隐藏区域和Cookie
                        var value = $(this).find(".ui-state-hover").children("input").val(); //在Chrome浏览器中不支持ui-state-focus样式
                        $("#hidden_xmlDocName").val(value);
                        var cookie = $.cookie("IQM_XMLDOCNAME");
                        if (cookie != value) {
                            $.cookie("IQM_XMLDOCNAME", value);
                        }

                        CreateQueryParameter(value);

                        //切换标签时重构界面
                        if (value != null && value != valueold) {
                            ResetDataTable();
                            CreateSearchFunction();
                            CreateDataTable();
                        }
                    }
                });

                ///* --------- 初始化时加粗标签字体 --------- */
                //$("#ul_tabs").tabs().find(".ui-state-active").children("a").addClass("boldfont");

                /* --------- 初始化时获取Cookie中的每页行数并保存到相关隐藏区域 --------- */
                var cookie = $.cookie("IQM_MAXIMUMROWS");
                if (cookie) {
                    $("#hidden_maximumRows").val(cookie);
                }

                /* --------- 将当前被选择标签中的Xml文档名存入相应隐藏区域 --------- */
                var value = $("#ul_tabs").tabs().find(".ui-state-active").children("input").val();
                $("#hidden_xmlDocName").val(value);

                CreateQueryParameter(value);

                /* --------- 页面初次载入构造界面 --------- */
                if (value != null) {
                    CreateSearchFunction();
                    CreateDataTable();
                }
            }
            else {
                ShowNoXmlDialog();
            }
        },
        error: function (object, text, thrown) {
            ShowNoXmlDialog();
        }
    });
}

function CreateQueryParameter(value) {
    var parameters = $.parseJSON(queryParameters);
    if (parameters.length > 0) {
        var paraString = "";
        for (var p = 0; p < parameters.length; p++) {
            if (parameters[p].XmlDocName == value) {
                if (parameters[p].QueryParameters.length > 0) {
                    for (var pp = 0; pp < parameters[p].QueryParameters.length; pp++) {
                        paraString = paraString + "'" + parameters[p].QueryParameters[pp] + "',";
                    }
                }
            }
        }
        $("#hidden_queryParameter").val(paraString);
    }
}

/* ==================== 创建搜索工具栏及相关功能 ==================== */
function CreateSearchToolbar() {
    /* --------- 定义重置查询结果按钮 --------- */
    $("#button_reset").button({ icons: { primary: "ui-icon-arrowreturnthick-1-w"} })
                              .click(function () {
                                  ResetDataTable();
                                  CreateDataTable();
                                  return false
                              });

    /* --------- 定义清除汇总结果按钮 --------- */
    $("#button_summary_clean").button({ icons: { primary: "ui-icon-trash" }, disabled: true })
                              .click(function () {
                                  $("#hidden_startRowIndex").val("0");
                                  $("#hidden_pageIndex").val("1");
                                  $("#hidden_groupColumns,#hidden_havingClause").val("");
                                  CreateDataTable();
                                  $("#button_summary_clean").button({ disabled: true });
                                  return false;
                              })

    /* --------- 定义组合条件查询按钮 --------- */
    $("#button_search").button({ icons: { primary: "ui-icon-search"} })
                              .click(function () {
                                  ResetSearchForm(); //打开查询窗口时初始化
                                  ShowFormSearch();
                                  return false;
                              });

    /* --------- 定义汇总查询结果按钮 --------- */
    $("#button_summary").button({ icons: { primary: "ui-icon-script"} })
                              .click(function () {
                                  ShowFormGroupBy();
                                  return false;
                              });

    /* --------- 定义导出查询结果按钮 --------- */
    $("#button_export").button({ icons: { primary: "ui-icon-document"} })
                              .click(function () {
                                  ShowExportExcelDialog();
                                  return false;
                              });

    /* --------- 定义导出查询结果按钮 --------- */
    $("#button_getexcel").button({ icons: { primary: "ui-icon-disk"} });
    $("#button_getexcel").click(function () {
        var send = '{"xmlDocName":"' + $("#hidden_xmlDocName").val() + '","selectColumns":"' + $("#hidden_selectColumns").val() + '","whereClause":"' + $("#hidden_whereClause").val() + '","groupColumns":"' + $("#hidden_groupColumns").val() + '","orderClause":"' + $("#hidden_orderClause").val() + '","havingClause":"' + $("#hidden_havingClause").val() + '","queryParameter":"' + $("#hidden_queryParameter").val() + '"}';
        $.ajax({
            type: "POST",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            url: "../IQMServices/IQMWebService.asmx/GetExcelName",
            data: send,
            success: function (data) {
                $("#button_getexcel").button({ disabled: true });
                $("#link_excel").attr("href", "../IQMTemp/" + data["d"]).show();
            },
            error: function (object, text, thrown) {
                $("#link_excel").text("文件导出失败，请联系网络管理员！");
                $("#link_excel").show();
            }
        });

    });
}

/* ==================== 创建每页行数工具栏及相关功能 ==================== */
function CreatePageNumberToolbar() {
    /* --------- 根据Cookie中的内容对每页行数工具栏中的按钮进行选定（保证页面刷型时按钮选择不发生变化） --------- */
    var cookie = $.cookie("IQM_MAXIMUMROWS");
    if (cookie) {
        var selector = "#span_toolbar_pagenum :radio[value=" + cookie + "]";
        $(selector).click();
    }

    /* --------- 按钮切换时在Cookie中保存每页行数并读取数据 --------- */
    var valueold; //用于保存旧数据以便在重复点击已选定的按钮时不重复获取数据
    $("#span_toolbar_pagenum :radio")
            .focus(function () {
                valueold = $("#span_toolbar_pagenum :checked").val();
            })
            .click(function () {
                //按钮选定时将值存入相应隐藏区域和Cookie
                var value = $("#span_toolbar_pagenum :checked").val();
                $("#hidden_maximumRows").val(value);
                if (cookie != value) {
                    $.cookie("IQM_MAXIMUMROWS", value);
                }

                //按钮切换时读取数据
                if (value != null && value != valueold) {
                    $("#hidden_startRowIndex").val("0");
                    $("#hidden_pageIndex").val("1");
                    CreateDataTable();
                }
            });

    /* --------- 转化成每页行数工具栏 --------- */
    $("#span_toolbar_pagenum").buttonset();
}

/* ==================== 创建各自定义模块及相关功能 ==================== */
function CreateSearchFunction() {
    var send = '{"xmlDocName":"' + $("#hidden_xmlDocName").val() + '"}';
    $.ajax({
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        url: "../IQMServices/IQMWebService.asmx/GetColumns",
        data: send,
        success: function (data) {
            if (data["d"].length > 0) {
                jsonColumns = data["d"]; //将数据列JSON对象存入全局变量

                /* ---------- 填充列名选择框和汇总项目列表 ---------- */
                $("#select_columns").children().remove();
                $("#div_groupby_items").children().remove();
                for (var i = 1; i < jsonColumns.length; i++) { //从1开始遍历，忽略序号列
                    var obj = jsonColumns[i];
                    //列名选择框
                    var value = "<option value=\"" + obj.Name + "\">" + obj.Alias + "</option>";
                    $("#select_columns").append(value);
                    //汇总项目列表
                    if (!obj.IsSumColumn) {
                        var groupValue = "<input id=\"checkbox_groupby_" + obj.Name + "\" type=\"checkbox\" value=\"" + obj.Name + "\" /><label for=\"checkbox_groupby_" + obj.Name + "\">" + obj.Alias + "</label><br />";
                        $("#div_groupby_items").append(groupValue);
                    }
                }
                //绑定列名选择框onchange事件
                $("#select_columns").change(function () {
                    SelectColumnsOption_OnClick();
                });

                /* ---------- 根据浏览器类型及版本调整列名选择列表的长度以保证显示效果一致 ---------- */
                var $selectColumns = $("#select_columns");
                if ($.browser.msie) {
                    if ($.browser.version == "8.0") {
                        $selectColumns.attr("size", "26");
                    } else if ($.browser.version == "9.0") {
                        $selectColumns.attr("size", "24");
                    }
                } else if ($.browser.mozilla) {
                    $selectColumns.attr("size", "21");
                } else if ($.browser.webkit) {
                    $selectColumns.attr("size", "25");
                } else if ($.browser.opera) {
                    $selectColumns.attr("size", "21");
                }

                /* ---------- 设置查询精度按钮点击事件 ---------- */
                $("#radio_likeall").click(function () {
                    $("#div_form_search .divequal").hide();
                    $("#div_form_search .divrange").show();
                });
                $("#radio_equal").click(function () {
                    $("#div_form_search .divequal").show();
                    $("#div_form_search .divrange").hide();
                });

                /* ---------- 文本型查询内容的自动完成功能 ---------- */
                $("#text_search_text").focus(function () {
                    var send = '{"xmlDocName":"' + $("#hidden_xmlDocName").val() + '","columnName":"' + $("#select_columns :selected").val() + '"}';
                    $.ajax({
                        type: "POST",
                        contentType: "application/json;charset=utf-8",
                        dataType: "json",
                        url: "../IQMServices/IQMWebService.asmx/GetAutocompleteValue",
                        data: send,
                        success: function (data) {
                            $("#text_search_text").autocomplete({
                                source: data["d"],
                                minLength: 0,
                                delay: 0
                            });
                        }
                    });
                });

                /* ---------- 为不同类型列的查询内容区域进行样式设置 ---------- */
                var divs = "#div_search_type_text,#div_search_type_money,#div_search_type_date,#div_search_type_year,#div_search_type_month,#div_search_type_day,#div_search_type_number,#div_search_type_alias";
                $(divs).children("h3").remove();
                $(divs).addClass("div_search_type ui-widget-content").prepend("<h3 class=\"ui-widget-header\">查询内容</h3>");

            }
            else {
                ShowNoColumnsDialog();
            }
        },
        error: function (object, text, thrown) {
            ShowNoColumnsDialog();
        }
    });
}

/* ==================== 创建数据表格及相关功能 ==================== */
function CreateDataTable() {
    var send = '{"xmlDocName":"' + $("#hidden_xmlDocName").val() + '","startRowIndex":"' + $("#hidden_startRowIndex").val() + '","maximumRows":"' + $("#hidden_maximumRows").val() + '","selectColumns":"' + $("#hidden_selectColumns").val() + '","whereClause":"' + $("#hidden_whereClause").val() + '","groupColumns":"' + $("#hidden_groupColumns").val() + '","orderClause":"' + $("#hidden_orderClause").val() + '","havingClause":"' + $("#hidden_havingClause").val() + '","queryParameter":"' + $("#hidden_queryParameter").val() + '"}';
    $.ajax({
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        url: "../IQMServices/IQMWebService.asmx/GetData",
        data: send,
        success: function (data) {
            jsonDataTable = data["d"];
            if (jsonDataTable.DataTotal == "0") {
                $("#button_summary,#button_export").button({ disabled: true });
                $("#div_table_data").hide();
                $("#div_table_nodata").show();
            }
            else {
                ShowDataTable();
                ShowPageButton();
                $("#button_summary,#button_export").button({ disabled: false });
                $("#div_table_data").show();
                $("#div_table_nodata").hide();
            }
            ShowSumResults();
        }
    });
}

/* ==================== 显示数据表 ==================== */
function ShowDataTable() {

    $("#div_table_body").children().remove();
    $("#div_table_body").append("<table id=\"table_main\"></table>");

    $("#table_main").append("<thead><tr id=\"tr_columns\"></tr></thead><tbody></tbody>");

    for (var i = 0; i < jsonDataTable.ColumnNames.length; i++) {
        var obj = jsonDataTable.ColumnNames[i];
        for (var x = 0; x < jsonColumns.length; x++) {
            var column = jsonColumns[x];
            if (column.Name == obj) {
                var th = "<th>" + column.Alias + "</th>";
                $("#tr_columns").append(th);
                break;
            }
        }
    }

    for (var i = 0; i < jsonDataTable.Rows.length; i++) {
        var obj = jsonDataTable.Rows[i];
        var tr = "<tr id=\"tr_" + i + "\"></tr>";
        $("#table_main tbody").append(tr);

        for (var x = 0; x < obj.Cells.length; x++) {
            var cell = obj.Cells[x];

            //提取文本居中方式信息
            var align;
            var column = jsonDataTable.ColumnNames[x];
            for (var z = 0; z < jsonColumns.length; z++) {
                if (jsonColumns[z].Name == column) {
                    align = jsonColumns[z].TextAlign;
                    break;
                }
            }

            var td;
            if (cell.ValueUrl == null) {
                td = "<td align=\"" + align + "\">" + cell.Value + "</td>";
            }
            else {
                td = "<td align=\"" + align + "\"><a href=\"" + cell.ValueUrl + "\">" + cell.Value + "</a></td>";
            }

            var selector = "#tr_" + i;
            $(selector).append(td);
        }
    }
}

/* ==================== 显示汇总结果 ==================== */
function ShowSumResults() {
    $("#div_result div").remove(); //移除全部已有结果
    //添加数据总数
    var dataTotal = "<div class=\"ui-state-highlight ui-corner-all sumresult\"><strong>符合条件的记录总计：</strong>" + jsonDataTable.DataTotal + "</div>";
    $("#div_result").append(dataTotal);
    //添加合计项汇总结果
    if (jsonDataTable.SumResults.length > 0) {
        for (var i = 0; i < jsonDataTable.SumResults.length; i++) {
            var obj = jsonDataTable.SumResults[i];
            var sumResult = "<div class=\"ui-state-highlight ui-corner-all sumresult\"><strong>" + obj.Name + "总计：</strong>" + obj.Result + "</div>";
            $("#div_result").append(sumResult);
        }
    }
}

/* ==================== 显示分页按钮 ==================== */
function ShowPageButton() {
    var rowCount = parseInt(jsonDataTable.DataTotal);
    var pageSize = parseInt($("#hidden_maximumRows").val());
    var pageCount = parseInt((rowCount + pageSize - 1) / pageSize);

    $("#span_page_count").text(pageCount);
    //$("#hidden_pageEndIndex").val(pageCount);
    if (rowCount == 0) {
        $("#span_page_index").text("0");
    }
    else {
        $("#span_page_index").text($("#hidden_pageIndex").val());
    }

    $("#button_page_first").button({ icons: { primary: "ui-icon-arrowthickstop-1-w"} });
    $("#button_page_last").button({ icons: { secondary: "ui-icon-arrowthickstop-1-e"} });
    $("#button_page_prev").button({ icons: { primary: "ui-icon-arrowthick-1-w"} });
    $("#button_page_next").button({ icons: { secondary: "ui-icon-arrowthick-1-e"} });
    $("#button_page_jump").button({ icons: { primary: "ui-icon-search"} });

    //根据当前页号决定各分页按钮按钮是否可用
    $("#button_page_first,#button_page_last,#button_page_prev,#button_page_next,#button_page_jump").button({ disabled: false });

    if (rowCount != 0) {
        var pageIndex = $("#hidden_pageIndex").val();
        if (pageIndex == 1 && pageIndex < pageCount) {
            $("#button_page_first,#button_page_prev").button({ disabled: true });
        }
        else if (pageIndex == 1 && pageIndex == pageCount) {
            $("#button_page_first,#button_page_last,#button_page_prev,#button_page_next,#button_page_jump").button({ disabled: true });
        }
        else if (pageIndex > 1 && pageIndex < pageCount) {

        }
        else if (pageIndex > 1 && pageIndex == pageCount) {
            $("#button_page_last,#button_page_next").button({ disabled: true });
        }
    }
    else {
        $("#button_page_first,#button_page_last,#button_page_prev,#button_page_next,#button_page_jump").button({ disabled: true });
    }

    //计算下一页起始行号
    var nextIndex;
    var prevIndex;
    var lastIndex = (pageCount - 1) * pageSize;
    if ($("#hidden_startRowIndex").val() == "0") {
        nextIndex = pageSize;
        prevIndex = -1;
    }
    else {
        nextIndex = pageIndex * pageSize;
        prevIndex = (pageIndex - 2) * pageSize;
    }

    //绑定下一页按钮事件
    $("#button_page_next").unbind("click");
    $("#button_page_next").click(function () {
        $("#hidden_startRowIndex").val(nextIndex);
        $("#hidden_pageIndex").val(parseInt(pageIndex) + 1);
        CreateDataTable();
        return false;
    });

    //绑定上一页按钮事件
    $("#button_page_prev").unbind("click");
    $("#button_page_prev").click(function () {
        $("#hidden_startRowIndex").val(prevIndex);
        $("#hidden_pageIndex").val(parseInt(pageIndex) - 1);
        CreateDataTable();
        return false;
    });

    //绑定首页按钮事件
    $("#button_page_first").unbind("click");
    $("#button_page_first").click(function () {
        $("#hidden_startRowIndex").val("0");
        $("#hidden_pageIndex").val("1");
        CreateDataTable();
        return false;
    });

    //绑定末页按钮事件
    $("#button_page_last").unbind("click");
    $("#button_page_last").click(function () {
        $("#hidden_startRowIndex").val(lastIndex);
        $("#hidden_pageIndex").val(pageCount);
        CreateDataTable();
        return false;
    });

    //清空跳转输入框并限制只能输入数字
    $("#text_jump_to").val("");
    $("#text_jump_to").keyup(function () {  //keyup事件处理 
        $(this).val($(this).val().replace(/[^0-9]/g, ''));
    }).bind("paste", function () {  //CTR+V事件处理 
        $(this).val($(this).val().replace(/[^0-9]/g, ''));
    }).css("ime-mode", "disabled");  //CSS设置输入法不可用

    //绑定跳转按钮事件
    $("#button_page_jump").unbind("click");
    $("#button_page_jump").click(function () {
        var jumpPage = $("#text_jump_to").val();
        if (jumpPage >= 1 && jumpPage <= pageCount) {
            $("#hidden_startRowIndex").val((parseInt(jumpPage) - 1) * pageSize);
            $("#hidden_pageIndex").val(jumpPage);
            CreateDataTable();
        }
        else {
            ShowPageOutRangeDialog();
            $("#text_jump_to").val("");
        }

        return false;
    });
}

/* ==================== 列名选择列表点击事件 ==================== */
function SelectColumnsOption_OnClick() {
    var value = $("#select_columns :selected").val();
    for (var i = 1; i < jsonColumns.length; i++) {
        var obj = jsonColumns[i];
        if (obj.Name == value) {

            $("#radio_and,#radio_likeall").click(); //切换项目时重置单选按钮选择状态

            //切换项目时根据是否有预览文字决定或、或非按钮的显示
            var result = $("#div_search_preview_text").text();
            if (result == "") {
                $("#span_or,#span_ornot").hide();
            }
            else {
                $("#span_or,#span_ornot").show();
            }

            $("#radio_likeall+label").text("范围"); //默认查询精度的Like按钮文字为范围

            $("#table_search_range").show(); //默认显示全部查询精度单选按钮
            $("#span_likefront,#span_likebehind").hide(); //默认隐藏前匹配和后匹配单选按钮

            HideAllSearchTypeDiv(); //默认隐藏查询内容类型区域
            switch (obj.ColumnType) {
                case "text":
                    $select = $("#text_search_text");
                    $select.val("");

                    $("#radio_likeall+label").text("模糊");

                    $("#span_likefront,#span_likebehind").show();
                    $("#div_search_type_text").show();
                    break;
                case "date":
                    $select = $("#text_search_date_start,#text_search_date_end,#text_search_date");
                    $select.val("");
                    $select.keyup(function () {  //keyup事件处理 
                        $(this).val($(this).val().replace(/[^0-9-]/g, ''));
                    }).bind("paste", function () {  //CTR+V事件处理 
                        $(this).val($(this).val().replace(/[^0-9-]/g, ''));
                    }).css("ime-mode", "disabled");  //CSS设置输入法不可用
                    $select.datepicker({
                        changeYear: true,
                        changeMonth: true,
                        showButtonPanel: true,
                        yearRange: '2000:2030'
                    });

                    $("#div_search_type_date").show();
                    break;
                case "money":
                    $select = $("#text_search_money_start,#text_search_money_end,#text_search_money");
                    $select.val("");
                    $select.keyup(function () {  //keyup事件处理 
                        $(this).val($(this).val().replace(/[^0-9.]/g, ''));
                    }).bind("paste", function () {  //CTR+V事件处理 
                        $(this).val($(this).val().replace(/[^0-9.]/g, ''));
                    }).css("ime-mode", "disabled");  //CSS设置输入法不可用

                    $("#div_search_type_money").show();
                    break;
                case "year":
                    $select = $("#text_search_year_start,#text_search_year_end,#text_search_year");
                    $select.val("");
                    $select.keyup(function () {  //keyup事件处理 
                        $(this).val($(this).val().replace(/[^0-9]|^0/g, ''));
                    }).bind("paste", function () {  //CTR+V事件处理 
                        $(this).val($(this).val().replace(/[^0-9]|^0/g, ''));
                    }).css("ime-mode", "disabled");  //CSS设置输入法不可用

                    $("#div_search_type_year").show();
                    break;
                case "month":
                    $select = $("#select_search_month_start,#select_search_month_end,#select_search_month");
                    $select.children().remove()
                    $select.append("<option value=\"\">-</option>");
                    for (var x = 1; x <= 12; x++) {
                        $select.append("<option value=\"" + x + "\">" + x + "</option>");
                    }

                    $("#div_search_type_month").show();
                    break;
                case "day":
                    $select = $("#select_search_day_start,#select_search_day_end,#select_search_day");
                    $select.children().remove()
                    $select.append("<option value=\"\">-</option>");
                    for (var y = 1; y <= 31; y++) {
                        $select.append("<option value=\"" + y + "\">" + y + "</option>");
                    }

                    $("#div_search_type_day").show();
                    break;
                case "number":
                    $select = $("#text_search_number_start,#text_search_number_end,#text_search_number");
                    $select.val("");
                    $select.keyup(function () {  //keyup事件处理 
                        $(this).val($(this).val().replace(/[^0-9.]/g, ''));
                    }).bind("paste", function () {  //CTR+V事件处理 
                        $(this).val($(this).val().replace(/[^0-9.]/g, ''));
                    }).css("ime-mode", "disabled");  //CSS设置输入法不可用

                    $("#div_search_type_number").show();
                    break;
                case "alias":
                    $select = $("#select_search_alias");
                    $select.children().remove()
                    $select.append("<option value=\"\">-</option>");
                    for (var z = 0; z < obj.ValueAlias.length; z++) {
                        var valueAlias = obj.ValueAlias[z];
                        $select.append("<option value=\"" + valueAlias.Value + "\">" + valueAlias.Alias + "</option>");
                    }

                    $("#table_search_range").hide();
                    $("#div_search_type_alias").show();
                    break;
            }
            break;
        }
    }
}

/* ==================== 隐藏全部的查询内容类型区域 ==================== */
function HideAllSearchTypeDiv() {
    var divs = "#div_search_type_text,#div_search_type_money,#div_search_type_date,#div_search_type_year,#div_search_type_month,#div_search_type_day,#div_search_type_number,#div_search_type_alias";
    $(divs).addClass("div_search_type ui-widget-content").hide();
}

/* ==================== 重置查询界面 ==================== */
function ResetSearchForm() {
    $("#div_search_preview_text").text("");
    $("#hidden_whereClauseTemp").val("");
    $("#select_columns :first").attr("selected", "selected");
    SelectColumnsOption_OnClick();
}

function ResetDataTable() {
    $("#hidden_startRowIndex").val("0");
    $("#hidden_pageIndex").val("1");
    $("#hidden_selectColumns,#hidden_whereClause,#hidden_groupColumns,#hidden_orderClause,#hidden_havingClause").val("");
    $("#button_summary_clean").button({ disabled: true });
}

/* ==================== 建立查询内容添加按钮功能 ==================== */
function CreateSearchContentButtons() {
    /* ---------- 文本类型添加按钮 ---------- */
    $("#div_form_search .text-type-button")
            .button({ icons: { primary: "ui-icon-plus"} })
            .click(function () {
                var $Item = $("#select_columns :selected");
                var $Preview = $("#div_search_preview_text");
                var $SqlTemp = $("#hidden_whereClauseTemp");
                var $Content = $(this).prev();

                var valLogical = $("#table_search_logical :checked").val();
                var logical = GetLogical(valLogical);
                var logicalSql = GetLogicalSql(valLogical);

                var valRange = $("#table_search_range :checked").val();
                var range = GetRange(valRange);
                var rangeSql = GetRangeSql(valRange, $Content.val());

                if ($Preview.text() == "") { //当预览文本为空时的语句
                    if ($Content.val() == "") { //当查询内容为空时的语句
                        if (valLogical == "and") { //条件是语句
                            var txt = "<" + $Item.text() + "> 是 '空值'";
                            $Preview.text(txt);
                            var sql = "WHERE (" + $Item.val() + " IS NULL) OR (RTRIM(" + $Item.val() + ") = '')";
                            $SqlTemp.val(sql);
                        }
                        else if (valLogical == "andnot") { //条件非语句
                            var txt = "<" + $Item.text() + "> 非 '空值'";
                            $Preview.text(txt);
                            var sql = "WHERE (NOT(" + $Item.val() + " IS NULL) OR (RTRIM(" + $Item.val() + ") = ''))";
                            $SqlTemp.val(sql);
                        }
                    }
                    else { //当查询内容不为空时的语句
                        if (valLogical == "and") { //条件是语句
                            var txt = "<" + $Item.text() + "> " + range + " '" + $Content.val() + "'";
                            $Preview.text(txt);
                            var sql = "WHERE (" + $Item.val() + " " + rangeSql + ")";
                            $SqlTemp.val(sql);
                        }
                        else if (valLogical == "andnot") { //条件非语句
                            var txt = "<" + $Item.text() + "> 非 " + range + " '" + $Content.val() + "'";
                            $Preview.text(txt);
                            var sql = "WHERE (NOT(" + $Item.val() + " " + rangeSql + "))";
                            $SqlTemp.val(sql);
                        }
                    }

                    //显示条件或、条件或非选择按钮
                    $("#span_or,#span_ornot").show();
                }
                else { //当预览文本不为空时的语句
                    if ($Content.val() == "") { //当查询内容为空时的语句
                        var txt = $Preview.text();
                        txt += " | " + logical + " <" + $Item.text() + "> 为 '空值'";
                        $Preview.text(txt);
                        var sql = $SqlTemp.val();
                        sql += " " + logicalSql + " (" + $Item.val() + " IS NULL) OR (RTRIM(" + $Item.val() + ") = '')";
                        $SqlTemp.val(sql);
                    }
                    else { //当查询内容不为空时的语句
                        var txt = $Preview.text();
                        txt += " | " + logical + " <" + $Item.text() + "> " + range + " '" + $Content.val() + "'";
                        $Preview.text(txt);
                        var sql = $SqlTemp.val();
                        sql += " " + logicalSql + " (" + $Item.val() + " " + rangeSql + ")";
                        $SqlTemp.val(sql);
                    }
                }

                return false;
            });

    /* ---------- 精确查询添加按钮 ---------- */
    $("#div_form_search .equal-type-button")
            .button({ icons: { primary: "ui-icon-plus"} })
            .click(function () {
                var $Item = $("#select_columns :selected");
                var $Preview = $("#div_search_preview_text");
                var $SqlTemp = $("#hidden_whereClauseTemp");
                var $Content;
                if ($(this).prev(":selected") == null) {
                    $Content = $(this).prev(":selected");
                }
                else {
                    $Content = $(this).prev();
                }

                var valLogical = $("#table_search_logical :checked").val();
                var logical = GetLogical(valLogical);
                var logicalSql = GetLogicalSql(valLogical);

                if ($Preview.text() == "") { //当预览文本为空时的语句
                    if ($Content.val() == "") { //当查询内容为空时的语句
                        if (valLogical == "and") { //条件是语句
                            var txt = "<" + $Item.text() + "> 是 '空值'";
                            $Preview.text(txt);
                            var sql = "WHERE (" + $Item.val() + " IS NULL) OR (RTRIM(" + $Item.val() + ") = '')";
                            $SqlTemp.val(sql);
                        }
                        else if (valLogical == "andnot") { //条件非语句
                            var txt = "<" + $Item.text() + "> 非 '空值'";
                            $Preview.text(txt);
                            var sql = "WHERE (NOT(" + $Item.val() + " IS NULL) OR (RTRIM(" + $Item.val() + ") = ''))";
                            $SqlTemp.val(sql);
                        }
                    }
                    else { //当查询内容不为空时的语句
                        if (valLogical == "and") { //条件是语句
                            var txt = "<" + $Item.text() + "> 等于 '" + $Content.val() + "'";
                            $Preview.text(txt);
                            var sql = "WHERE (" + $Item.val() + " = '" + $Content.val() + "')";
                            $SqlTemp.val(sql);
                        }
                        else if (valLogical == "andnot") { //条件非语句
                            var txt = "<" + $Item.text() + "> 非 '" + $Content.val() + "'";
                            $Preview.text(txt);
                            var sql = "WHERE (NOT(" + $Item.val() + " = '" + $Content.val() + "'))";
                            $SqlTemp.val(sql);
                        }
                    }

                    //显示条件或、条件或非选择按钮
                    $("#span_or,#span_ornot").show();
                }
                else { //当预览文本不为空时的语句
                    if ($Content.val() == "") { //当查询内容为空时的语句
                        var txt = $Preview.text();
                        txt += " | " + logical + " <" + $Item.text() + "> 为 '空值'";
                        $Preview.text(txt);
                        var sql = $SqlTemp.val();
                        sql += " " + logicalSql + " (" + $Item.val() + " IS NULL) OR (RTRIM(" + $Item.val() + ") = '')";
                        $SqlTemp.val(sql);
                    }
                    else { //当查询内容不为空时的语句
                        var txt = $Preview.text();
                        txt += " | " + logical + " <" + $Item.text() + "> 等于 '" + $Content.val() + "'";
                        $Preview.text(txt);
                        var sql = $SqlTemp.val();
                        sql += " " + logicalSql + " (" + $Item.val() + " = '" + $Content.val() + "')";
                        $SqlTemp.val(sql);
                    }
                }

                return false;
            });

    /* ---------- 范围查询添加按钮 ---------- */
    $("#div_form_search .range-type-button")
            .button({ icons: { primary: "ui-icon-plus"} })
            .click(function () {
                var $Item = $("#select_columns :selected");
                var $Preview = $("#div_search_preview_text");
                var $SqlTemp = $("#hidden_whereClauseTemp");

                var $Start;
                if ($(this).prev().prev(":selected") == null) {
                    $Start = $(this).prev().prev(":selected");
                }
                else {
                    $Start = $(this).prev().prev();
                }

                var $End;
                if ($(this).prev(":selected") == null) {
                    $End = $(this).prev(":selected");
                }
                else {
                    $End = $(this).prev();
                }

                var valLogical = $("#table_search_logical :checked").val();
                var logical = GetLogical(valLogical);
                var logicalSql = GetLogicalSql(valLogical);

                if ($Preview.text() == "") { //当预览文本为空时的语句
                    if (valLogical == "and") { //条件是语句
                        if ($Start.val() == "" && $End.val() == "") {
                            var txt = "<" + $Item.text() + "> 是 '空值'";
                            $Preview.text(txt);
                            var sql = "WHERE (" + $Item.val() + " IS NULL) OR (RTRIM(" + $Item.val() + ") = '')";
                            $SqlTemp.val(sql);
                        }
                        else if ($Start.val() == "" && $End.val() != "") {
                            var txt = "<" + $Item.text() + "> 小于等于 '" + $End.val() + "'";
                            $Preview.text(txt);
                            var sql = "WHERE (" + $Item.val() + " >= '" + $End.val() + "')";
                            $SqlTemp.val(sql);
                        }
                        else if ($Start.val() != "" && $End.val() == "") {
                            var txt = "<" + $Item.text() + "> 大于等于 '" + $Start.val() + "'";
                            $Preview.text(txt);
                            var sql = "WHERE (" + $Item.val() + " <= '" + $Start.val() + "')";
                            $SqlTemp.val(sql);
                        }
                        else {
                            if ($Start.val() == $End.val()) {
                                var txt = "<" + $Item.text() + "> 等于 '" + $Start.val() + "'";
                                $Preview.text(txt);
                                var sql = "WHERE (" + $Item.val() + " = '" + $Start.val() + "')";
                                $SqlTemp.val(sql);
                            }
                            else {
                                var txt = "<" + $Item.text() + "> 介于 '" + $Start.val() + "' 与 '" + $End.val() + "' 之间";
                                $Preview.text(txt);
                                var sql = "WHERE (" + $Item.val() + " BETWEEN '" + $Start.val() + "' AND '" + $End.val() + "' OR " + $Item.val() + " BETWEEN '" + $End.val() + "' AND '" + $Start.val() + "')";
                                $SqlTemp.val(sql);
                            }
                        }
                    }
                    else if (valLogical == "andnot") { //条件非语句
                        if ($Start.val() == "" && $End.val() == "") {
                            var txt = "<" + $Item.text() + "> 非 '空值'";
                            $Preview.text(txt);
                            var sql = "WHERE (NOT(" + $Item.val() + " IS NULL) OR (RTRIM(" + $Item.val() + ") = ''))";
                            $SqlTemp.val(sql);
                        }
                        else if ($Start.val() == "" && $End.val() != "") {
                            var txt = "<" + $Item.text() + "> 非 小于等于 '" + $End.val() + "'";
                            $Preview.text(txt);
                            var sql = "WHERE (NOT(" + $Item.val() + " >= '" + $End.val() + "'))";
                            $SqlTemp.val(sql);
                        }
                        else if ($Start.val() != "" && $End.val() == "") {
                            var txt = "<" + $Item.text() + "> 非 大于等于 '" + $Start.val() + "'";
                            $Preview.text(txt);
                            var sql = "WHERE (NOT(" + $Item.val() + " <= '" + $Start.val() + "'))";
                            $SqlTemp.val(sql);
                        }
                        else {
                            if ($Start.val() == $End.val()) {
                                var txt = "<" + $Item.text() + "> 非 '" + $Start.val() + "'";
                                $Preview.text(txt);
                                var sql = "WHERE (NOT(" + $Item.val() + " = '" + $Start.val() + "'))";
                                $SqlTemp.val(sql);
                            }
                            else {
                                var txt = "<" + $Item.text() + "> 非 介于 '" + $Start.val() + "' 与 '" + $End.val() + "' 之间";
                                $Preview.text(txt);
                                var sql = "WHERE (NOT(" + $Item.val() + " BETWEEN '" + $Start.val() + "' AND '" + $End.val() + "' OR " + $Item.val() + " BETWEEN '" + $End.val() + "' AND '" + $Start.val() + "'))";
                                $SqlTemp.val(sql);
                            }
                        }
                    }

                    //显示条件或、条件或非选择按钮
                    $("#span_or,#span_ornot").show();
                }
                else { //当预览文本不为空时的语句
                    if ($Start.val() == "" && $End.val() == "") {
                        var txt = $Preview.text();
                        txt += " | " + logical + " <" + $Item.text() + "> 为 '空值'";
                        $Preview.text(txt);
                        var sql = $SqlTemp.val();
                        sql += " " + logicalSql + " (" + $Item.val() + " IS NULL) OR (RTRIM(" + $Item.val() + ") = '')";
                        $SqlTemp.val(sql);
                    }
                    else if ($Start.val() == "" && $End.val() != "") {
                        var txt = $Preview.text();
                        txt += " | " + logical + " <" + $Item.text() + "> 小于等于 '" + $End.val() + "'";
                        $Preview.text(txt);
                        var sql = $SqlTemp.val();
                        sql += " " + logicalSql + " (" + $Item.val() + " >= '" + $End.val() + "')";
                        $SqlTemp.val(sql);
                    }
                    else if ($Start.val() != "" && $End.val() == "") {
                        var txt = $Preview.text();
                        txt += " | " + logical + " <" + $Item.text() + "> 大于等于 '" + $Start.val() + "'";
                        $Preview.text(txt);
                        var sql = $SqlTemp.val();
                        sql += " " + logicalSql + " (" + $Item.val() + " <= '" + $Start.val() + "')";
                        $SqlTemp.val(sql);
                    }
                    else {
                        if ($Start.val() == $End.val()) {
                            var txt = $Preview.text();
                            txt += " | " + logical + " <" + $Item.text() + "> 等于 '" + $Start.val() + "'";
                            $Preview.text(txt);
                            var sql = $SqlTemp.val();
                            sql += " " + logicalSql + " (" + $Item.val() + " = '" + $Start.val() + "')";
                            $SqlTemp.val(sql);
                        }
                        else {
                            var txt = $Preview.text();
                            txt += " | " + logical + " <" + $Item.text() + "> 介于 '" + $Start.val() + "' 与 '" + $End.val() + "' 之间";
                            $Preview.text(txt);
                            var sql = $SqlTemp.val();
                            sql += " " + logicalSql + " (" + $Item.val() + " BETWEEN '" + $Start.val() + "' AND '" + $End.val() + "' OR " + $Item.val() + " BETWEEN '" + $End.val() + "' AND '" + $Start.val() + "')";
                            $SqlTemp.val(sql);
                        }
                    }
                }

                return false;
            });
}

function GetLogical(value) {
    var txt = "";
    switch (value) {
        case "and":
            txt = "并且"
            break;
        case "andnot":
            txt = "并且非"
            break;
        case "or":
            txt = "或者"
            break;
        case "ornot":
            txt = "或者非"
            break;
    }
    return txt;
}

function GetLogicalSql(value) {
    var txt = "";
    switch (value) {
        case "and":
            txt = "AND"
            break;
        case "andnot":
            txt = "AND NOT"
            break;
        case "or":
            txt = "OR"
            break;
        case "ornot":
            txt = "OR NOT"
            break;
    }
    return txt;
}

function GetRange(value) {
    var txt = "";
    switch (value) {
        case "likeall":
            txt = "包含"
            break;
        case "equal":
            txt = "等于"
            break;
        case "likefront":
            txt = "前匹配"
            break;
        case "likebehind":
            txt = "后匹配"
            break;
    }
    return txt;
}

function GetRangeSql(value, content) {
    var txt = "";
    switch (value) {
        case "likeall":
            txt = "LIKE '%" + content + "%'";
            break;
        case "equal":
            txt = "= '" + content + "'";
            break;
        case "likefront":
            txt = "LIKE '" + content + "%'";
            break;
        case "likebehind":
            txt = "LIKE '%" + content + "'";
            break;
    }
    return txt;
}