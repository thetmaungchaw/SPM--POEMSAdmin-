<%@ Page Language="C#" AutoEventWireup="true" validateRequest="false" CodeBehind="AddEditEmailTemplate.aspx.cs" Inherits="SPMWebApp.WebPages.EmailTemplate.AddEditEmailTemplate" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Add/Edit Email Template</title>
    <link rel="stylesheet" href="<%=ConfigurationManager.AppSettings["SPMCssFile"]%>" type="text/css" />    
</head>
<body>
    <div id="Container">
    <form id="frmAddEditEmailTemplate" runat="server">
    <table width="100%">
         <tr>
           <td colspan="3">
              <div class="normalRed" id="divMessage" runat="server"></div>
           </td>
         </tr>
         <tr>
             <td width="150px"><asp:Label ID="lbltemplateName" runat="server" Text="Template Name" CssClass="normal"></asp:Label> </td>  
             <td CssClass="normal">:</td>      
             <td><asp:TextBox ID="txtTemplateName" runat="server" Text="" Width="350px" CssClass="normal"></asp:TextBox>
                 <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                     ErrorMessage="*" ControlToValidate="txtTemplateName" Font-Bold="True" 
                     EnableClientScript="False"></asp:RequiredFieldValidator>
             </td>
         </tr>         
         <tr>
             <td width="150px"> <asp:RadioButton ID="rdoText" runat="server" Text="Text Template" GroupName="TemplateType" CssClass="normal" Checked="true" /> </td>
             <td colspan="2"> <asp:RadioButton ID="rdoHtml" runat="server" Text="HTML Template" GroupName="TemplateType" CssClass="normal" />   </td>
         </tr>         
         <tr>
             <td width="150px"> <asp:Label ID="lblSubject" runat="server" Text="Subject" CssClass="normal"></asp:Label>    </td>
             <td CssClass="style1"> : </td>
             <td> <asp:TextBox ID="txtEmailSubject" runat="server" Width="350px" CssClass="normal"></asp:TextBox> </td>         
         </tr> 
         <tr>
             <td width="150px"> <asp:Label ID="lblContents" runat="server" Text="Contents" CssClass="normal"></asp:Label> </td>
         </tr>
         <tr>
         <td></td>
         </tr>
         </table>
         <table width="100%">
         <tr>
         <td> 
         <asp:TextBox ID="txtEmailContent" runat="server" Height="300px" 
                 TextMode="MultiLine" Width="850px"></asp:TextBox> </td>
         </tr>
         <tr>
         <td width="200px" align="right">                        
             <asp:Button ID="btnUpdate" runat="server" Text="Update" Visible="false"
                 onclick="btnUpdate_Click" /> 
             <asp:Button ID="btnSave" runat="server" Text="Save" onclick="btnSave_Click" />             
             &nbsp;   
             <asp:Button ID="btnCancel" runat="server" Text="Cancel" onclick="btnCancel_Click" />
           </td> 
           <td></td> 
         </tr>  
         <tr>
         <td align="right">
          <asp:Label ID="lblModified" runat="server" Text="Last Modified On :" CssClass="normal"></asp:Label>  
          <asp:Label ID="lblModifiedTime" runat="server" Text="" CssClass="normal"></asp:Label>
         </td>
         </tr>
         </table>
         <asp:Panel ID="Panel1" runat="server" GroupingText="Below columns can be replaced by the system" Width="30%" CssClass="normal">
            ***DealerName***<br />
            ***NumOfClients***<br />
            ***NumOfLeads***<br />
            ***AssignDate***<br />
            ***AcctNo***<br />
            ***ProjectName***<br />
            ***FollowUpDate***
         </asp:Panel>
    </form>
     </div>
</body>
</html>
