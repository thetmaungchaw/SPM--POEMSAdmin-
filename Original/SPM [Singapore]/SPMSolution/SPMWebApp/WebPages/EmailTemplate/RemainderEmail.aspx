<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RemainderEmail.aspx.cs" Inherits="SPMWebApp.RemainderEmail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div>Sending Remainder email by Calls left</div>
        <div id="divMessage1" runat="server"></div><br />
        <div>Sending Remainder email by Follow-up date</div>
        <div id="divMessage2" runat="server"></div>
    </div>
    
    <div>        
        <asp:Button ID="Button1" runat="server" Text="Method One With Base Figure" 
            onclick="Button1_Click" width="200px" Visible="false"/>        
        <br />
        <asp:Button ID="Button2" runat="server" Text="Method Two With FollowUpDate" 
            onclick="Button2_Click" Width="200px" Visible="false"/>
    </div>
    </form>
</body>
</html>
