<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DealerManagement.aspx.cs"
    Inherits="SPMWebApp.WebPages.Setup.DealerManagement" %>

<%@ Register Src="~/PagingControl.ascx" TagName="PagingControl" TagPrefix="ucc" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>SPM - Dealer Management</title>
    <link rel="stylesheet" href="<%=ConfigurationManager.AppSettings["SPMCssFile"]%>"
        type="text/css" />
</head>
<body>
    <div id="Container">
        <form id="frmDealerManagement" runat="server">
        <p class="title">
            SPM Dealer Management</p>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>       

         <script type="text/javascript" language="javascript">
            <!--
             var prm = Sys.WebForms.PageRequestManager.getInstance();

             prm.add_initializeRequest(InitializeRequest);
             prm.add_endRequest(EndRequest);

             function InitializeRequest(sender, args) {
                 // Get a reference to the element that raised the postback to disables it.
                 var postBackControl = $get(args._postBackElement.id);

                 $get('Container').style.cursor = 'wait';
                 if (postBackControl != null) {
                     postBackControl.disabled = true;
                 }
             }

             function EndRequest(sender, args) {
                 // Get a reference to the element that raised the postback which is completing to enable it.
                 var postBackControl = $get(sender._postBackSettings.sourceElement.id);

                 $get('Container').style.cursor = 'auto';
                 if (postBackControl != null) {
                     postBackControl.disabled = false;
                 }
             }
             // -->

             //Move to Top when GridView modify button click
             function ScrollToTop() {
                 window.scrollTo(0, 0);
             }


             function CheckAll(checkAllBox, fieldName) {
                 var inputs = document.getElementsByTagName("input");
                 var actVar = checkAllBox.checked;

                 for (i = 0; i < inputs.length; i++) {
                     e = inputs[i];

                     if (e.type == 'checkbox' && e.name.indexOf(fieldName) != -1) {
                         e.checked = actVar && (!e.disabled);
                     }
                 }
             }
            
            
            
        </script>

        <asp:UpdatePanel ID="uplDealer" runat="server">
            <ContentTemplate>
                <div id="divSearch" runat="server">
                    <table>
                        <tr class="normal">
                            <td>
                                Dealer Code:
                            </td>
                            <td>
                                <asp:TextBox ID="txtDealerCode" CssClass="normal" runat="server" />
                            </td>
                            <td>
                                Login ID:
                            </td>
                            <td>
                                <asp:TextBox ID="txtLoginId" CssClass="normal" runat="server" />
                            </td>
                            <td>
                                Email :
                            </td>
                            <td>
                                <asp:TextBox ID="txtEmailID" runat="server" CssClass="normal" />
                            </td>
                            <td>
                               <%-- AECode :--%>
                            </td>
                            <td>
                                <asp:TextBox ID="txtAltAECode" Visible="false" Columns="9" runat="server" CssClass="normal" />
                            </td>
                        </tr>
                        <tr class="normal">
                            <td>
                                Name:
                            </td>
                            <td>
                                <asp:TextBox ID="txtDealerName" Width="200px" CssClass="normal" runat="server" />
                            </td>
                            <td>
                                Team Code:
                            </td>
                            <td colspan="3">
                                <asp:DropDownList ID="ddlTeamCode" CssClass="normal" runat="server" />
                            </td>
                        </tr>
                        <tr class="normal">
                            <td>
                                Assign:
                            </td>
                            <td>
                                <asp:CheckBox ID="chkAssign" Text="" CssClass="normal" runat="server" />
                            </td>
                            <td>
                                Cross Group:
                            </td>
                            <td>
                                <asp:CheckBox ID="chkCrossGroup" Text="" CssClass="normal" runat="server" />
                            </td>
                            <td>
                                <%--Administrator:--%>
                            </td>
                            <td>
                                <%--<asp:CheckBox ID="chkSupervisior" Text="" CssClass="normal" runat="server" />--%>
                                <asp:CheckBox ID="chkUserType" Text="User Type" CssClass="normal" runat="server"
                                    AutoPostBack="true" OnCheckedChanged="chkUserType_CheckedChanged" />
                            </td>
                        </tr>
                        <tr class="normal">
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:Panel ID="Panel1" runat="server" GroupingText="Administrator/Supervisor">
                                    <asp:RadioButton ID="rdbtnAdministrator" runat="server" GroupName="AdministratorOrSupervisor"
                                        Text="Administrator (Y)" />
                                    <asp:RadioButton ID="rdbtnSupervisor" runat="server" GroupName="AdministratorOrSupervisor"
                                        Text="Supervisor (S)" />
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                    <div>
                        <asp:Button ID="btnSearch" Text="Search" OnClick="btnSearch_Click" CssClass="normal"
                            runat="server" />
                        <asp:Button ID="btnCreate" Text="Create" CssClass="normal" runat="server" OnClick="btnCreate_Click" />
                    </div>
                    <div id="divMessage" class="normalRed" runat="server">
                    </div>
                    <table width="100%" border="0" bordercolor="#777777" cellspacing="0" cellpadding="0"
                        class="normalGrey">
                        <tr class="normal">
                            <td bgcolor="#FFCC99" align="center">
                                Supervisor
                            </td>
                        </tr>
                    </table>
                    <br />
                    <div id="divDealers" align="left">
                        <asp:GridView ID="gvDealers" CssClass="normal" AllowSorting="true" BorderColor="#777777"
                            DataKeyNames="RecId" AutoGenerateColumns="False" OnRowEditing="gvList_RowEditing"
                            OnRowDeleting="gvListDelete_RowDeleting" AllowPaging="True" PageSize="20" PagerSettings-Visible="false"
                            runat="server" OnRowDataBound="gvDealers_RowDataBound" OnSorting="gvDealers_Sorting"
                            OnRowCommand="gvDealers_RowCommand">
                            <HeaderStyle BackColor="#CDCDCD" />
                            <PagerSettings Visible="False" />
                            <RowStyle CssClass="normal" BackColor="White" BorderColor="#777777" />
                            <AlternatingRowStyle CssClass="normal" BackColor="#EEEEEE" BorderColor="#777777" />
                            <Columns>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        No.
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# Container.DataItemIndex + 1 %>
                                        <asp:HiddenField ID="gvhdfItemIndex" Value='<%# Container.DataItemIndex %>' runat="server" />
                                        <asp:HiddenField ID="gvhdfDisplayIndex" Value='<%# Container.DisplayIndex %>' runat="server" />
                                        <asp:HiddenField ID="gvhdfRecId" Value='<%# Eval("RecId") %>' runat="server" />
                                        <asp:HiddenField ID="gvhdfUserId" Value='<%# Eval("UserID") %>' runat="server" />
                                        <asp:HiddenField ID="gvhdfAEGroup" Value='<%# Eval("AEGroup") %>' runat="server" />
                                    </ItemTemplate>
                                    <HeaderStyle BorderColor="#777777" />
                                    <ItemStyle BorderColor="#777777" CssClass="grayBorder" Width="50px" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="AECode" SortExpression="AECode" HeaderText="Dealer Code">
                                    <HeaderStyle CssClass="grayBorder" />
                                    <ItemStyle CssClass="grayBorder" Width="90px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="UserID" SortExpression="UserID" HeaderText="User ID">
                                    <HeaderStyle CssClass="grayBorder" />
                                    <ItemStyle CssClass="grayBorder" Width="80px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="AEName" SortExpression="AEName" HeaderText="Name">
                                    <HeaderStyle CssClass="grayBorder" />
                                    <ItemStyle CssClass="grayBorder" Width="160px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Email" HeaderText="Email">
                                    <HeaderStyle CssClass="grayBorder" />
                                    <ItemStyle CssClass="grayBorder" Width="160px" />
                                </asp:BoundField>
                                <%--<asp:TemplateField HeaderText="Team Mngt" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>                                
                                <asp:Literal ID="litView" runat="server" Text='<%# GetJSString(Eval("UserID").ToString(),Eval("AEName").ToString(),"VIEW") %>'></asp:Literal>
                                <asp:Literal ID="litSetup" runat="server" Text='<%# GetJSString(Eval("UserID").ToString(),Eval("AEName").ToString(),"SETUP") %>'></asp:Literal>
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                                <asp:BoundField DataField="AEGroup" SortExpression="AEGroup" HeaderText="Team">
                                    <HeaderStyle CssClass="grayBorder" />
                                    <ItemStyle CssClass="grayBorder" Width="60px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="AltAECode" HeaderText="AE Code">
                                    <HeaderStyle CssClass="grayBorder" />
                                    <ItemStyle CssClass="grayBorder" Width="160px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Enable" SortExpression="Enable" HeaderText="Assign">
                                    <HeaderStyle CssClass="grayBorder" />
                                    <ItemStyle CssClass="grayBorder" Width="60px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="CrossGroup" SortExpression="CrossGroup" HeaderText="Cross Group">
                                    <HeaderStyle CssClass="grayBorder" />
                                    <ItemStyle CssClass="grayBorder" Width="80px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Supervisor" SortExpression="Supervisor" HeaderText="Administrator/Supervisor">
                                    <HeaderStyle CssClass="grayBorder" />
                                    <ItemStyle CssClass="grayBorder" Width="60px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="modifiedUser" HeaderText="Modified User">
                                    <HeaderStyle CssClass="grayBorder" />
                                    <ItemStyle CssClass="grayBorder" Width="80px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="modifiedDate" HeaderText="Modified Date" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}"
                                    HtmlEncode="false">
                                    <HeaderStyle CssClass="grayBorder" />
                                    <ItemStyle CssClass="grayBorder" Width="120px" />
                                </asp:BoundField>
                                <asp:TemplateField ShowHeader="False">
                                    <EditItemTemplate>
                                        <asp:Button ID="Button1" runat="server" CausesValidation="True" CommandName="Update"
                                            Text="Update" />
                                        &nbsp;<asp:Button ID="Button2" runat="server" CausesValidation="False" CommandName="Cancel"
                                            Text="Cancel" />
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Button ID="gvbtnModify" runat="server" CausesValidation="False" CommandName="edit"
                                            Text="Modify" OnClientClick="ScrollToTop()" Visible="false" />
                                        <asp:Button ID="Button3" runat="server" CausesValidation="False" CommandName="edit"
                                            Text="Modify" OnClientClick="ScrollToTop()" />
                                        &nbsp;<asp:Button ID="Button2" runat="server" CausesValidation="False" CommandName="Delete"
                                            Text="Delete" />
                                    </ItemTemplate>
                                    <ControlStyle CssClass="normal" />
                                    <HeaderStyle CssClass="grayBorder" />
                                    <ItemStyle CssClass="grayBorder" Width="150px" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                    <br />
                    <div id="divPaging" class="normal" runat="server">
                        <ucc:PagingControl ID="pgcDealer" OnPageNoChanged="PagingControl_PageNoChange" OnRowPerPageChanged="PagingControl_RowPerPageChanged"
                            runat="server" />
                    </div>
                </div>
                <div id="divInput" runat="server" visible="false">
                    <table>
                        <tr class="normal">
                            <td>
                                Login ID:
                            </td>
                            <td>
                                <asp:TextBox ID="txtInpLoginID" CssClass="normal" runat="server" />
                                <asp:HiddenField ID="hdfDisplayIndex" runat="server" />
                                <asp:HiddenField ID="hdfModifyIndex" runat="server" />
                                <asp:HiddenField ID="hdfRecId" runat="server" />
                            </td>
                            <td>
                                &nbsp;&nbsp;&nbsp; Email :
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txtInpEmail" runat="server" Columns="50" CssClass="normal" />
                            </td>
                        </tr>
                        <tr class="normal">
                            <td>
                                Name:
                            </td>
                            <td>
                                <asp:TextBox ID="txtInpName" Width="200px" CssClass="normal" runat="server" />
                            </td>
                            <td>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr class="normal">
                            <td>
                                Assign:
                            </td>
                            <td>
                                <asp:CheckBox ID="chkInpAssign" Text="" CssClass="normal" runat="server" />
                            </td>
                            <td>
                                Cross Group:
                            </td>
                            <td>
                                <asp:CheckBox ID="chkInpCrossGroup" Text="" CssClass="normal" runat="server" />
                            </td>
                            <td colspan="2">
                            </td>
                        </tr>
                        <tr class="normal">
                            <td>
                            </td>
                            <td>
                                <asp:CheckBox ID="chkInpUserType" Text="User Type" CssClass="normal" runat="server"
                                    AutoPostBack="true" OnCheckedChanged="chkInpUserType_CheckedChanged" />
                                <asp:Panel ID="Panel2" runat="server" GroupingText="Administrator/Supervisor" Enabled="false">
                                    <asp:RadioButton ID="rdbtnInpAdministrator" runat="server" GroupName="AdministratorOrSupervisor"
                                        Text="Administrator (Y)" />
                                    <asp:RadioButton ID="rdbtnInpSupervisor" runat="server" GroupName="AdministratorOrSupervisor"
                                        Text="Supervisor (S)" />
                                </asp:Panel>
                            </td>
                            <td>
                            </td>                            
                            <td colspan="3">
                                <div id="divInpMessage" class="normalRed" runat="server">
                                </div>
                            </td>
                        </tr>
                    </table>
                    <table width="45%">
                        <tr>
                            <td >
                                <fieldset style="width: 95%" class="normal">
                                    <legend style="color: Red; font-weight: bold;">Team/Group</legend>
                                    <asp:TextBox ID="txtSearchAEGroup" Width="50px" runat="server"></asp:TextBox>
                                    <asp:Button ID="btnSearchAEGroup" runat="server" Text="Search" OnClick="btnSearchAEGroup_Click" />
                                    <asp:RadioButton ID="rdbtnAEGroupStartWith" runat="server" Text="Start With" GroupName="AEGroup" />
                                    <asp:RadioButton ID="rdbtnAEGroupEndWith" runat="server" Text="End With" GroupName="AEGroup" />
                                    <asp:RadioButton ID="rdbtnAEGroupContain" runat="server" Text="Contain" GroupName="AEGroup" />&nbsp;&nbsp;
                                    <asp:Label ID="labAEGroupSelectAll" runat="server" Text="Select All"></asp:Label>
                                    <asp:CheckBox ID="chkAEGroupSelectAll" runat="server" onclick="CheckAll(this,'chkAEGroup');" />
                                    <div style="overflow-y: scroll; height: 400px; width: 100%">
                                        <asp:DataList ID="dlAEGroup" runat="server" OnPreRender="dlAEGroup_PreRender" OnItemCommand="dlAEGroup_ItemCommand"
                                            OnItemDataBound="dlAEGroup_ItemDataBound">
                                            <ItemTemplate>
                                                <table>
                                                    <tr>
                                                        <td style="width: 25%">
                                                            <asp:CheckBox ID="chkAEGroup" runat="server" Text='<%# Bind("AEGroup") %>' />
                                                            <%--<asp:Label ID="labAEGroup" runat="server" Text='<%# Bind("AEGroup") %>'></asp:Label>--%>
                                                        </td>
                                                        <td style="width: 25%">
                                                            &nbsp;&nbsp;&nbsp;Dealer Code<asp:TextBox ID="txtAECode" Width="70px" runat="server"></asp:TextBox>
                                                        </td>
                                                        <td style="width: 25%">
                                                            &nbsp;&nbsp;&nbsp;AE Code<asp:TextBox ID="txtAltAECode" Width="70px" runat="server"></asp:TextBox>
                                                        </td>
                                                        <td style="width: 25%">
                                                            <asp:LinkButton ID="lnkRefAE" runat="server" Text="Ref ?" CommandArgument='<%# Bind("AEGroup") %>'
                                                                CommandName="indicate"></asp:LinkButton>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                        </asp:DataList>
                                    </div>
                                </fieldset>
                                <div>
                                    <asp:Button ID="btnAddDealer" Text="Save" CssClass="normal" runat="server" OnClick="btnAddDealer_Click" />
                                    &nbsp;
                                    <asp:Button ID="btnUpdate" Text="Update" CssClass="normal" runat="server" OnClick="btnUpdate_Click" />
                                    &nbsp;
                                    <asp:Button ID="btnCancel" Text="Cancel" CssClass="normal" runat="server" OnClick="btnCancel_Click" />
                                    &nbsp;
                                    <asp:Button ID="btnDeleteByUser" Visible="false" Text="Delete User" CssClass="normal"
                                        runat="server" OnClick="btnDeleteByUser_Click" />
                                </div>
                            </td>
                            <td>
                            </td>
                            <td valign="top" style="width: 30%" class="normal">
                                <fieldset>
                                    Existing Dealer Code
                                    <%--<asp:Label ID="lbDealerCodeMsg" ForeColor="Red" runat="server"></asp:Label>--%>
                                    <asp:ListBox ID="lstDealerCode" Width="200px" Height="300px" runat="server"></asp:ListBox>
                                </fieldset>
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="dlAEGroup" />
            </Triggers>
        </asp:UpdatePanel>
        </form>
    </div>
</body>
</html>
