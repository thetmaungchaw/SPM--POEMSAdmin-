<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EmailTemplate.aspx.cs" Inherits="SPMWebApp.EmailTemplate" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Email Template</title>
    <link rel="stylesheet" href="<%=ConfigurationManager.AppSettings["SPMCssFile"]%>" type="text/css" />
       <script type="text/javascript">
        function ConfirmSave() {
        
           confirm('Are you sure you want to delete this record')
        }
    </script>

</head>
<body>
    <div id="Container">
    <form id="frmEmailTemplate" runat="server">    
        <br />
        <asp:LinkButton ID="addTemplate" runat="server" class="emaillinkBlue" 
            onclick="addTemplate_Click">Add a new template</asp:LinkButton>
        <br />
       <br />
        <div id="div2" class="normalGrey">Current Email Templates</div> <br />
        <div id="divMessage" class="normalRed" runat="server"></div><br />
        <asp:GridView ID="grvItemTemplate" runat="server" AutoGenerateColumns="False" CellPadding="4"
            DataKeyNames="TemplateID" ForeColor="#333333" GridLines="None" CssClass="normal"
            OnRowDeleting="grvItemTemplate_RowDeleting" 
            OnRowEditing="grvItemTemplate_RowEditing" 
            onrowdatabound="grvItemTemplate_RowDataBound">         
            <PagerSettings Visible="False" />
           <%-- <RowStyle BackColor="#FFFBD6" ForeColor="#333333" />--%>
            <RowStyle CssClass="normal" BackColor="White" BorderColor="#777777" />
            <Columns>  
                <asp:TemplateField>
                 <ItemTemplate>
                  <asp:Label ID="templateID" runat="server" Visible="false"></asp:Label> 
                 </ItemTemplate>
                </asp:TemplateField>              
                <asp:BoundField DataField="TemplateName" HeaderText="Template Name" />
                <asp:CommandField HeaderText="Edit" ShowEditButton="True" ButtonType="Link" />
               <%-- <asp:CommandField HeaderText="Delete" ShowDeleteButton="True" ButtonType="Link"  />  --%>
                <asp:TemplateField>
                <ItemTemplate>
                            <asp:LinkButton ID="LinkButton1" runat="server" OnClientClick="return confirm('Are you sure you want to delete this record?');"
                                CommandName="Delete">Delete</asp:LinkButton>
                </ItemTemplate>
                </asp:TemplateField>              
            </Columns>                       
            <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#FFCC66" ForeColor="#333333" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
            <HeaderStyle BackColor="#CDCDCD" />
            <alternatingrowstyle CssClass="normal" BackColor="#EEEEEE" BorderColor="#777777" />
           <%-- <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />--%>
            <%--<AlternatingRowStyle BackColor="White" />--%>
        </asp:GridView>
    </form>
     </div>
</body>
</html>
