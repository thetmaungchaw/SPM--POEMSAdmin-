<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SPMMenu.aspx.cs" Inherits="SPMWebApp.SPMMenu" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>SPM Main Menu</title>    
    <link rel="stylesheet" href="<%=ConfigurationManager.AppSettings["SPMCssFile"]%>" type="text/css" />
</head>
<body>
    <form id="frmSPMMenu" runat="server">
    <div>
  	    <p class="title">Sales Performance Management System</p>
        <div id="divMessage" class="normalRed" runat="server"></div>
       <asp:Repeater ID="rptFunctions" runat="server">
            <HeaderTemplate>
	        <table width="600" border="0" bordercolor="#777777" cellspacing="0" cellpadding="0" class="normalGrey">
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td>
                        <asp:HyperLink ID="rpthlFunction" Text='<%#DataBinder.Eval(Container, "DataItem.Function_Desc")%>' 
                           NavigateUrl='<%# DataBinder.Eval(Container, "DataItem.URL")%>' runat="server" CssClass="menuSmall" />
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
        </asp:Repeater>
        <br />
    </div>    
    </form>
</body>
</html>
