<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AccessManagement.aspx.cs"
    Inherits="SPMWebApp.WebPages.AccessControl.AccessManagement" %>
<%@ Register
    Assembly="AjaxControlToolkit"
    Namespace="AjaxControlToolkit"
    TagPrefix="ajaxToolkit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>User Access Management</title>
    <link rel="stylesheet" href="<%=ConfigurationManager.AppSettings["SPMCssFile"]%>" type="text/css" />
</head>
<body>
    <div id="Container">
    <form id="frmAccessManagement" runat="server">
    <div>
        <div class="title">Access Management<br/></div>
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
                <table width="660px" border="0" cellspacing="0" cellpadding="0" class="normalGrey">
                    <tr bgcolor="#ffcccc" class="normal" align="center">
                        <td width="200px">
                            User
                        </td>
                        <td width="200px" align="center">
                            <asp:DropDownList ID="ddlUserCode" CssClass="normal" runat="server" AutoPostBack="True"
                                OnSelectedIndexChanged="ddlUserCode_SelectedIndexChanged" Width="135px">
                            </asp:DropDownList>
                        </td>
                        <td width="180px">
                            <asp:DropDownList ID="ddlRoleName" CssClass="normal" runat="server" AutoPostBack="True"
                                OnSelectedIndexChanged="ddlRoleName_SelectedIndexChanged" Width="131px">
                            </asp:DropDownList>
                            &nbsp;
                        </td>
                        <td width="80px">
                            <asp:Button ID="btnSave" CssClass="normal" Text="Save" runat="server" OnClick="btnSave_Click" Width="54px" />
                        </td>
                    </tr>
                </table>
                <br />
                <div id="divMessage" class="normalRed" runat="server">
                </div>
                <asp:GridView ID="gvList" runat="server" AutoGenerateColumns="False" 
                    onrowdatabound="gvList_RowDataBound" DataKeyNames="Function_Code" 
                    GridLines="None" onselectedindexchanged="gvList_SelectedIndexChanged">
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
                            <asp:HiddenField ID="hidCreateOld" runat="server" Value='<%# Convert.ToBoolean(Eval("CreateRight")) %>' />
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
                            <asp:HiddenField ID="hidModifyAccess" runat="server" Value='<%# Convert.ToString(Eval("hidModify")) %>' />
                            <asp:HiddenField ID="hidModifyOld" runat="server" Value='<%# Convert.ToBoolean(Eval("ModifyRight")) %>' />
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
                            <asp:HiddenField ID="hidViewAccess" runat="server" Value='<%# Convert.ToString(Eval("hidView")) %>' />
                            <asp:HiddenField ID="hidViewOld" runat="server" Value='<%# Convert.ToBoolean(Eval("ViewRight")) %>' />
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
                            <asp:HiddenField ID="hidDeleteAccess" runat="server" Value='<%# Convert.ToString(Eval("hidDelete")) %>' />
                            <asp:HiddenField ID="hidDeleteOld" runat="server" Value='<%# Convert.ToBoolean(Eval("DeleteRight")) %>' />
                                <asp:CheckBox ID="cbxDeleteAccess" runat="server" Checked='<%# Convert.ToBoolean(Eval("DeleteRight")) %>' 
                                    Enabled="true" />
                            </ItemTemplate>
                            <HeaderStyle CssClass="title" />
                            <ItemStyle HorizontalAlign="Center" Width="200px" />
                        </asp:TemplateField>
                       
                    </Columns>
                </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel>
              
        
        
    </div>
    </form>
    </div>
</body>
</html>
