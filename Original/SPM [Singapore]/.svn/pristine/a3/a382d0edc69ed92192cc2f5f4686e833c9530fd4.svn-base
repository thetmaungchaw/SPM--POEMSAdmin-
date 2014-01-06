<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserControlTest.aspx.cs" Inherits="SPMWebApp.UserControlTest" %>

<%@ Register Src="~/PagingControl.ascx" TagName="PagingControl" TagPrefix="ucc" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>User Control Test</title>
    <link rel="stylesheet" href="StyleSheet/style.css" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <br />
                <asp:Label ID="lblInfo" Text="Page Index : " CssClass="normal" runat="server" />
                <br />
                                
                <div id="divPaging" class="normal" runat="server">
                    <ucc:PagingControl ID="pgc" OnPageNoChanged="PagingControl_PageNoChange" OnRowPerPageChanged="PagingControl_RowPerPageChanged" 
                        runat="server"/>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
            
    </div>    
    </form>
</body>
</html>
