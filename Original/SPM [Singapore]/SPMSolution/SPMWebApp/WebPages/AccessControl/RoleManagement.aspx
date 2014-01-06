<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RoleManagement.aspx.cs" Inherits="SPMWebApp.WebPages.AccessControl.RoleManagement" %>

<%@ Register
    Assembly="AjaxControlToolkit"
    Namespace="AjaxControlToolkit"
    TagPrefix="ajaxToolkit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>User Role Management</title>    
 <link rel="stylesheet" href="<%=ConfigurationManager.AppSettings["SPMCssFile"]%>" type="text/css" />
 
</head>
<body>
    <form id="form1" runat="server">
    <div id="Container">
    <div>
        <div class="title">Role Management<br/></div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        
        <script type="text/javascript" language="javascript">
            <!--
            var prm = Sys.WebForms.PageRequestManager.getInstance();
     
            prm.add_initializeRequest(InitializeRequest);
            prm.add_endRequest(EndRequest);

            function InitializeRequest(sender, args) 
            {
                // Get a reference to the element that raised the postback to disables it.
                var postBackControl = $get(args._postBackElement.id);                            
                
                $get('Container').style.cursor = 'wait';                
                if(postBackControl != null)
                {                    
                    postBackControl.disabled = true;      
                }                                                             
            }

            function EndRequest(sender, args) 
            {                                
                // Get a reference to the element that raised the postback which is completing to enable it.
                var postBackControl = $get(sender._postBackSettings.sourceElement.id);
                
                $get('Container').style.cursor = 'auto';                
                if(postBackControl != null)
                {                    
                    postBackControl.disabled = false;      
                }
            }
            // -->        
        </script>
        
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
            <table border="0" cellspacing="0" cellpadding="10"  width="70%" align="left">
                <tr >
                    <td width="40%" align="left">
                        <fieldset ID="fsCreate" runat="server" title="Create Role" >
                            <legend><asp:CheckBox ID="chkCreateHeader" runat="server" Text="Create Role"  
                                   AutoPostBack="True" Checked="True" oncheckedchanged="chkCreateHeader_CheckedChanged" CssClass="normalGrey" /></legend>
                            <table align="left" width="100%" class="normal" >
                                <tr >
                                    <td  width="30%" > <label id="lblRName">Role Name</label></td>
                                    <td width="80%"><asp:TextBox ID="txtRoleName" runat="server" Width="185px"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td width="30%"><label id="lblRDes">Role Description</label></td>
                                    <td width="80%"><asp:TextBox ID="txtRoleDes" runat="server" Width="250px" ></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                <td width="30%">&nbsp;</td>
                                <td width="80%"><asp:Button ID="btnSave" runat="server" Text="Save" Width="70px" CssClass="normal"
                                            onclick="btnSave_Click"/></td>
                                </tr>
                            </table>
                        </fieldset>       
                    </td>
                    <td width="40%" align="left" > 
                        <fieldset ID="fsModify" runat="server" title="Modify/Delete Role">
                            <legend><asp:CheckBox ID="chkModifyHeader" runat="server" Text="Modify/Delete Role" CssClass="normalGrey"
                                    AutoPostBack="True" oncheckedchanged="chkModifyHeader_CheckedChanged" /></legend>
                            <table  align="left" width="100%" class="normal">
                                <tr >
                                    <td width="30%">  <label id="Label1">Role Name</label></td>
                                    <td width="80%">
                                         <asp:DropDownList ID="ddlRoleName" runat="server" CssClass="normal"
                                             AutoPostBack="True" onselectedindexchanged="ddlRoleName_SelectedIndexChanged" 
                                             Enabled="False" Width="185px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="30%"><label id="Label2">Role Description</label></td>
                                    <td width="80%"><asp:TextBox ID="txtRoleDes2" runat="server" Enabled="False" Width="250px" CssClass="normal"></asp:TextBox></td>
                                </tr>
                                <tr><td width="30%">&nbsp;</td>
                                    <td width="80%"><asp:Button ID="btnUpdate" runat="server" Text="Update" Enabled="False" 
                                            onclick="btnUpdate_Click" Width="70px" CssClass="normal" />&nbsp; 
                                        <asp:Button ID="btnDelete" runat="server" Text="Delete" Enabled="False" CssClass="normal"
                                            onclick="btnDelete_Click" Width="70px" /></td>
                                </tr>
                            </table>
                        </fieldset>   
                    </td>                   
                </tr>
                <tr>
                  <td colspan="2"> <br /><div id="divMessage" class="normalRed" runat="server"/>  </td>
                </tr>
                <tr>
                <td colspan="2"> 
                    <asp:GridView ID="gvList" runat="server" AutoGenerateColumns="False" 
                    onrowdatabound="gvList_RowDataBound" DataKeyNames="Function_Code" 
                    GridLines="None">
                    <Columns>
                        <asp:BoundField DataField="Category" ItemStyle-Font-Bold="True" 
                            ItemStyle-CssClass="title" ItemStyle-Width="200px" >
                            <ItemStyle CssClass="title" Font-Bold="True" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Function_Code" Visible="false" />
                        <asp:BoundField DataField="Function_Desc" HeaderText="Function"
                            ItemStyle-Width="200px" HeaderStyle-CssClass="title" 
                            ItemStyle-CssClass="menuBig" >
                            <HeaderStyle CssClass="title" HorizontalAlign="Left" />
                            <ItemStyle CssClass="menuBig" Width="200px" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Create" HeaderStyle-CssClass="title">
                            <EditItemTemplate>
                                <asp:CheckBox ID="cbxCreateAccess0" runat="server" Checked='<%# Convert.ToBoolean(Eval("CreateRight")) %>' />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:HiddenField ID="hidCreateAccess" runat="server" Value='<%# Convert.ToString(Eval("hidCreate")) %>' />
                                <asp:CheckBox ID="cbxCreateAccess" runat="server" Checked='<%# Convert.ToBoolean(Eval("CreateRight")) %>'
                                Enabled="true" />
                            </ItemTemplate>
                            <HeaderStyle CssClass="title" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Modify" HeaderStyle-CssClass="title">
                            <EditItemTemplate>
                                <asp:CheckBox ID="cbxModifyAccess0" runat="server" Checked='<%# Convert.ToBoolean(Eval("ModifyRight"))  %>' />
                            </EditItemTemplate>
                            <ItemTemplate>
                            <asp:HiddenField ID="hidModifyAccess" runat="server"  Value='<%# Convert.ToString(Eval("hidModify")) %>'/>
                                <asp:CheckBox ID="cbxModifyAccess" runat="server" Checked='<%# Convert.ToBoolean(Eval("ModifyRight")) %>' 
                                    Enabled="true" />
                            </ItemTemplate>
                            <HeaderStyle CssClass="title" />
                            <ItemStyle HorizontalAlign="Center" Width="200px" />
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderText="View" HeaderStyle-CssClass="title">
                            <EditItemTemplate>
                                <asp:CheckBox ID="cbxViewAccess0" runat="server" Checked='<%# Convert.ToBoolean(Eval("ViewRight")) %>' />
                            </EditItemTemplate>
                            <ItemTemplate>
                            <asp:HiddenField ID="hidViewAccess" runat="server" Value='<%# Convert.ToString(Eval("hidView")) %>'/>
                                <asp:CheckBox ID="cbxViewAccess" runat="server" Checked='<%# Convert.ToBoolean(Eval("ViewRight")) %>' 
                                    Enabled="true" />
                            </ItemTemplate>
                            <HeaderStyle CssClass="title" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Delete" HeaderStyle-CssClass="title">
                            <EditItemTemplate>
                                <asp:CheckBox ID="cbxDeleteAccess0" runat="server" Checked='<%# Convert.ToBoolean(Eval("DeleteRight"))  %>' />
                            </EditItemTemplate>
                            <ItemTemplate>
                            <asp:HiddenField ID="hidDeleteAccess" runat="server" Value='<%# Convert.ToString(Eval("hidDelete")) %>'/>
                                <asp:CheckBox ID="cbxDeleteAccess" runat="server" Checked='<%# Convert.ToBoolean(Eval("DeleteRight")) %>' 
                                    Enabled="true" />
                            </ItemTemplate>
                            <HeaderStyle CssClass="title" />
                            <ItemStyle HorizontalAlign="Center" Width="200px" />
                        </asp:TemplateField>
                       
                    </Columns>
                </asp:GridView>
                </td>
                </tr>
                
            </table>
                       
               
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </div>
    </form>
</body>
</html>
