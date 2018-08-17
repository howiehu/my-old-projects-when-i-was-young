<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Template.aspx.cs" Inherits="Template" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>演示页面</title>
    <%--这是必须添加的JSON数据对象，值内容为Xml配置文件名--%>
    <script type="text/javascript">
        var xmlDocNames = '{"xmlDocNames":"IQMZbPlanYear,IQMZbPlanMonth,IQMZbPlanTemp,IQMzb,IQMzbHthq,IQMctQuarter"}';
    </script>
    <%--这些是必须添加的样式表文件--%>
    <link href="../IQMThemes/IQMCSS.css" rel="stylesheet" type="text/css" />
    <link href="../IQMThemes/jquery-ui.css" rel="stylesheet" type="text/css" />
    <%--这些是必须添加的Javascript脚本--%>
    <script src="../IQMScripts/jquery.min.js" type="text/javascript"></script>
    <script src="../IQMScripts/jquery.cookie.js" type="text/javascript"></script>
    <script src="../IQMScripts/jquery-ui.min.js" type="text/javascript"></script>
    <script src="../IQMScripts/jquery-ui-i18n.js" type="text/javascript"></script>
    <script src="../IQMScripts/IQMCore.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="div_iqm_ui">
        <%--此为必须添加的Div元素用于接收Html模板--%>
    </div>
    </form>
</body>
</html>
