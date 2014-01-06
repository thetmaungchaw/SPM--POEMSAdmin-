<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SyncForm.aspx.cs" Inherits="SPMWebApp.WebPages.LeadManagement.SyncForm" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Synchronisation</title>
     <link rel="stylesheet" href="../../StyleSheet/SPMStyle.css" type="text/css" />
    <style type="text/css">
        .style1
        {
            height: 44px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div style="height: 162px">
    <asp:Panel ID="panelSync" runat="server" Height="156px" Width="341px" >
    <fieldset>
        <legend>Synchronisation</legend>
        <div id="divMessage" runat="server" class="normalRed"></div>
        <table class="normal" cellpadding="0" cellspacing="0">
        <tr>
            <td><asp:RadioButton ID="rdoNRIC" runat="server" Text="NRIC / Passport No." 
                    GroupName="radSync" CssClass="normal" 
                    oncheckedchanged="rdo_CheckedChanged" AutoPostBack="True" Checked="True"/></td>
            <td><asp:TextBox ID="txtSyncNRIC" runat="server" CssClass="normal" Width="180px" 
                    Height="35px"></asp:TextBox></td>
        </tr>
        <tr>
            <td ><asp:RadioButton ID="rdoAccNo" runat="server" Text="Account No." 
                    GroupName="radSync" CssClass="normal" OnCheckedChanged="rdo_CheckedChanged" 
                    AutoPostBack="True"/></td>
            <td class="style1"><asp:TextBox ID="txtSyncAccNo" runat="server" CssClass="normal" 
                    Width="180px" Height="35px"></asp:TextBox></td>
        </tr>
        <tr >
            <td colspan="2" align="center" valign="middle">
            <asp:Button ID="btnSyncSubmit" runat="server" Text="Submit" CssClass="normal" 
                    onclick="btnSyncSubmit_Click"/>&nbsp;&nbsp;
            <asp:Button ID="btnSyncCancel" runat="server" Text="Cancel" CssClass="normal" 
                    onclick="btnSyncCancel_Click" />
            </td>
        </tr>
        </table>
        
        
    </fieldset>
    </asp:Panel>
    </div>
    </form>
</body>
</html>
