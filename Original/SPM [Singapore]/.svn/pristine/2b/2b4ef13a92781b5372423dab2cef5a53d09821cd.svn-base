<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SentEmailToContact.aspx.cs" Inherits="SPMWebApp.WebPages.ContactManagement.SentEmailToContact" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
      <link rel="stylesheet" href="<%=ConfigurationManager.AppSettings["SPMCssFile"]%>" type="text/css" />
       <script type="text/javascript" language="Javascript">
           function closewindow() {
               window.close();
           }
       </script>    
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <fieldset  style="width:80%;border:1px solid gray;">
        <legend style="font-family: Arial,Helvetica,sans-serif; font-size:11px; font-weight:bold;padding: 0 0 10px;">
        Sent Promotion Email        
        </legend>
        <table border="0" bordercolor="#777777" cellspacing="1" cellpadding="1" class="smallGrey">
        <tr>
            <td CssClass="normal">
               <b>Project Name:</b>
            </td>
            <td>
                <asp:DropDownList ID="ddlProjectName" runat="server" CssClass="normal" 
                    Width="180px"></asp:DropDownList>
            </td>
        </tr>
         <tr>
            <td>
             &nbsp;
            </td>
        </tr>
         <tr>
            <td CssClass="normal">
            <asp:CheckBox ID="chkSysEmail" runat="server" CssClass="normal" />
            <b>Email from the system:</b>
            </td>
            <td>
              <asp:DropDownList ID="ddlSystemEmails" runat="server" CssClass="normal" 
                    Height="22px" Width="180px"></asp:DropDownList>
            </td>
        </tr>
         <tr>
            <td>
             &nbsp;
            </td>
        </tr>
         <tr>
            <td CssClass="normal">
             <asp:CheckBox ID="chkCustomEmail" runat="server" CssClass="normal" />
             <b>Email as specified by Client:</b>
            </td>
            <td>
                <asp:TextBox ID="txtCustomEmail" runat="server" CssClass="normal" Width="180px"></asp:TextBox>
            </td>
        </tr>
         <tr>
            <td>
             &nbsp;
            </td>
        </tr>
         <tr>
        <td align="right">
            <asp:Button ID="btnSend" runat="server" Text="Send" Width="70px" 
                CssClass="normal" onclick="btnSend_Click" />
            &nbsp; &nbsp;
        </td>
        <td>
           &nbsp;&nbsp;
            <asp:Button ID="btnCancel" runat="server" Text="Cancel" Width="70px" 
                CssClass="normal"  onclientclick="closewindow()" />
        </td>
        </tr>
        <tr>
            <td>
             &nbsp;
                <div id="divMessage" runat="server" class="normalRed">
                </div>
            </td>
        </tr>
        </table> 
        </fieldset>  
    </div>
    </form>
</body>
</html>
