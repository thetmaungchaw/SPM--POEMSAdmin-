<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SPMMainPage.aspx.cs" Inherits="SPMWebApp.SPMMainPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <link rel="stylesheet" href="<%=ConfigurationManager.AppSettings["SPMCssFile"]%>" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <div id="divMessage" class="normalRed" runat="server"></div>
    </div>
    </form>
</body>
</html>
