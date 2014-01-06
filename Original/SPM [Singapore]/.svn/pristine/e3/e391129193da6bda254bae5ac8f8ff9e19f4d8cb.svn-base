<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LeadContactEntry.aspx.cs"
    Inherits="SPMWebApp.WebPages.LeadManagement.LeadContactEntry" %>

<%@ Register Src="~/PagingControl.ascx" TagName="PagingControl" TagPrefix="ucc" %>
<%@ Register Src="~/CoolCalendar.ascx" TagName="CoolCalendar" TagPrefix="ucc" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <!-- Mimic Internet Explorer 7 -->
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7">
    <title>SPM - Contact Entry</title>
    <link rel="stylesheet" href="../../StyleSheet/SPMStyle.css" type="text/css" />

    <script type="text/javascript" language="Javascript">
        //        function handleShortKeyBlur()
        //        {            
        //            __doPostBack('btnShortKey','');
        //        }
        //        
        //Move to Top when GridView modify button click
        //top:expression(this.offsetParent.scrollTop);
        function ScrollToTop() {
            window.scrollTo(0, 0);
        }
       
    </script>

    <style type="text/css">
        .tab_none
        {
            background-color: Gray;
        }
        .Freezing
        {
            background-color: #CDCDCD;
            vertical-align: middle;
            text-align: center;
            margin-right: auto;
            margin-left: auto;
            position: relative;
            z-index: 10;
            font-size: 10px;
        }
        .style1
        {
            width: 593px;
        }
        .style2
        {
            width: 90px;
        }
        .style3
        {
            width: 289px;
        }
    </style>
</head>
<body>
    <form id="frmContactEntry" runat="server">
    <div id="Container">
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
        </script>

        <asp:UpdatePanel ID="uplContactEntry" runat="server">
            <ContentTemplate>
                <div id="divTitle" class="title" runat="server">
                    Leads Contact Entry</div>
                <div id="divMessage" class="normalRed" runat="server">
                </div>
                <asp:HiddenField ID="hdfDealerCode" runat="server" />
                <asp:HiddenField ID="hdfStatus" runat="server" />
                <!--  div for the whole page to display scroll bar  -->
                <!-- <div id="divContactEntry" style="height:260px;overflow:auto;">  -->
                <table width="100%">
                    <!--  style="height: 260px"  width="100%"-->
                    <tr>
                        <td>
                            <!-- style="width:20%" -->
                            <!-- For Contact Entry Admin -->
                            <div id="divAdminEntry" runat="server">
                                <asp:Label ID="lblDealer" Text="Dealer :" CssClass="normal" runat="server" />
                                &nbsp;
                                <asp:DropDownList ID="ddlActualDealer" CssClass="normal" runat="server" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlActualDealer_SelectedIndexChanged" />
                            </div>
                            <table border="0" bordercolor="#777777" cellspacing="1" cellpadding="1" class="smallGrey">
                                <tr>
                                    <td>
                                        <b>Team: <font color="#FF0000">*</font></b>
                                    </td>
                                    <td colspan="4">
                                        <asp:DropDownList ID="ddlTeam" runat="server" CssClass="normal" Height="29px" Width="200px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <b>Name: <font color="#FF0000">*</font></b>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtName" runat="server" CssClass="normal"></asp:TextBox>
                                        <asp:Label ID="lblLeadId" runat="server" /><asp:Label ID="lblProjectID" runat="server" />
                                    </td>
                                    <td>
                                        &nbsp;&nbsp;
                                    </td>
                                    <td>
                                        <b>Gender<font color="#FF0000">*</font></b>
                                    </td>
                                    <td>
                                        <asp:RadioButton ID="rbtnFemale" Text="Female" GroupName="Sex" Checked="false" CssClass="normal"
                                            runat="server" />
                                        &nbsp;&nbsp;
                                        <asp:RadioButton ID="rbtnMale" Text="Male" GroupName="Sex" Checked="false" CssClass="normal"
                                            runat="server" />
                                        &nbsp;&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <b>NRIC / Passport No:</b>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtNRIC" CssClass="normal" runat="server" Width="200px" />
                                    </td>
                                    <td>
                                        &nbsp;&nbsp;
                                    </td>
                                    <td>
                                        <b>Event: </b>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtEvent" CssClass="normal" runat="server" Width="200px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <b>Mobile No: <font color="#FF0000">*</font></b>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtMobileNo" CssClass="normal" runat="server" Width="200px" />
                                    </td>
                                    <td>
                                        &nbsp;&nbsp;
                                    </td>
                                    <td>
                                        <b>Preferred Mode of Contact: </b>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlPreferredMode" CssClass="normal" runat="server" Width="200px">
                                            <asp:ListItem Text="Mobile No" Value="Mobile No"></asp:ListItem>
                                            <asp:ListItem Text="Home No" Value="Home No"></asp:ListItem>
                                            <asp:ListItem Text="Email" Value="Email"></asp:ListItem>
                                            <asp:ListItem Text="Mail" Value="Mail"></asp:ListItem>
                                            <asp:ListItem Text="SMS" Value="SMS"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <b>Home No:</b>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtHomeNo" CssClass="normal" runat="server" Width="200px" />
                                    </td>
                                    <td>
                                        &nbsp;&nbsp;
                                    </td>
                                    <td>
                                        <b>Email:</b>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtEmail" CssClass="normal" runat="server" Width="200px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <b>Content<font color="#FF0000">*</font></b>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtContent" CssClass="normal" TextMode="MultiLine" MaxLength="255"
                                            Rows="2" Columns="50" runat="server" Height="120px" Width="200px" />
                                    </td>
                                    <td>
                                        &nbsp;&nbsp;
                                    </td>
                                    <td valign="top">
                                        <b>Seminar:</b><br />
                                        <br />
                                        <br />
                                        <asp:CheckBox ID="chkFollowUp" Text="Follow Up" Width="90px" Checked="false" CssClass="normal"
                                            runat="server" OnCheckedChanged="chkFollowUp_CheckedChanged" AutoPostBack="True"
                                            Font-Bold="True" /></b>
                                    </td>
                                    <td valign="top">
                                        <a href="http://www.poems.com.sg/index.php?option=com_content&view=article&id=203&Itemid=282&lang=en"
                                            target="_blank">Seminar Registrations</a>
                                        <br />
                                        <br />
                                        <br />
                                        <asp:Panel ID="panelFollowupPanel" runat="server" Width="242px" Enabled="false">
                                            Date: &nbsp;
                                            <ucc:CoolCalendar ID="calFollowupDate" TimeRequired="true" DateTimeFormat="dd/MM/yyyy HH:mm:ss"
                                                DateTextRequired="false" DateTextFromValue="" runat="server" />
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="5">
                                        <%= DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") %>
                                        &nbsp;&nbsp;
                                        <asp:Button ID="btnAddContact" Text="Add Record" CssClass="normal" runat="server"
                                            OnClick="btnAddContact_Click" />
                                        &nbsp;
                                        <asp:Button ID="btnSync" Text="Sync" CssClass="normal" runat="server" OnClick="btnSync_Click" />
                                        &nbsp;
                                        <asp:Button ID="btnUpdateContact" Text="Update" CssClass="normal" runat="server"
                                            OnClick="btnUpdateContact_Click" />
                                        &nbsp;
                                        <asp:Button ID="btnCancel" Text="Cancel" CssClass="normal" runat="server" OnClick="btnCancel_Click" />
                                        <asp:HiddenField ID="hdfModifyIndex" runat="server" />
                                        <asp:HiddenField ID="hdfRecId" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td>
                            <!-- style="width:80%" -->
                            <div id="div2" class="normalGrey">
                                Current Assignments</div>
                            <br />
                            <div style="height: 260px; overflow: auto;">
                                <!--  style="height:260px;overflow:auto;"  -->
                                <asp:GridView ID="gvAssignments" CssClass="normal" AllowSorting="true" AutoGenerateColumns="false"
                                    PagerSettings-Visible="false" AllowPaging="true" PageSize="10" runat="server"
                                    OnRowCommand="gvAssignments_RowCommand" OnSorting="gvAssignments_Sorting" OnRowDataBound="gvAssignments_RowDataBound">
                                    <HeaderStyle BackColor="#CDCDCD" CssClass="Freezing" />
                                    <RowStyle BackColor="#FFCCFF" CssClass="grayBorder" />
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                No.
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>
                                                <asp:HiddenField ID="gvhdfRecordId" Value='<%# Container.DataItemIndex %>' runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle CssClass="grayBorder" Width="15px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Lead ID" SortExpression="LeadId">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="gvlbtnAccountNo" CssClass="normal" Text='<%# Bind("LeadId") %>'
                                                    CommandName="LeadId" CommandArgument="<%# Container.DataItemIndex %>" runat="server"
                                                    Font-Underline="True" ForeColor="Blue" ToolTip="Select Leads" />
                                            </ItemTemplate>
                                            <ItemStyle CssClass="grayBorder" Width="30px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Name" SortExpression="LeadName">
                                            <ItemTemplate>
                                                <%# Eval("LeadName") %>
                                                <%--<asp:LinkButton ID="gvlbtnClientName" CssClass="normal" Text='<%# Bind("LeadName") %>' 
                                                   CommandName="LeadName" CommandArgument="<%# Container.DataItemIndex %>"
                                                   Font-Underline="True" ForeColor="Blue" runat="server" ToolTip="View History"/>--%>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="grayBorder" Width="100px" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="LeadNRIC" SortExpression="LeadNRIC" HeaderText="NRIC/Passport No">
                                            <ItemStyle CssClass="grayBorder" Width="55px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Sex" HeaderText="Gender" SortExpression="Sex">
                                            <ItemStyle CssClass="grayBorder" Width="10px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="LeadMobile" SortExpression="LeadMobile" HeaderText="Moblie No.">
                                            <ItemStyle CssClass="grayBorder" Width="160px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="LeadHomeNo" SortExpression="LeadHomeNo" HeaderText="Home No.">
                                            <ItemStyle CssClass="grayBorder" Width="160px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="LeadEmail" SortExpression="Email" HeaderText="Email">
                                            <ItemStyle CssClass="grayBorder" Width="60px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Event" SortExpression="Event" HeaderText="Event">
                                            <ItemStyle CssClass="grayBorder" Width="55px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ContactDate" HeaderText="Contact" SortExpression="Contact">
                                            <ItemStyle CssClass="grayBorder" Width="80px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="AssignDate" SortExpression="AssignDate" HeaderText="Assign"
                                            DataFormatString="{0:dd/MM/yyyy}" HtmlEncode="false">
                                            <ItemStyle CssClass="grayBorder" Width="25px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="CutOffDate" SortExpression="CutOffDate" HeaderText="Cutoff Date"
                                            DataFormatString="{0:dd/MM/yyyy HH:mm:ss}" HtmlEncode="false">
                                            <HeaderStyle CssClass="grayBorder" />
                                            <ItemStyle CssClass="grayBorder" Width="100px" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Team" SortExpression="Team">
                                            <ItemTemplate>
                                                <%# DataBinder.Eval(Container.DataItem, "DealerTeam")%>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="grayBorder" Width="25px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Project" SortExpression="ProjectName">
                                            <ItemTemplate>
                                                <%# DataBinder.Eval(Container.DataItem, "ProjectName")%>
                                                <asp:HiddenField ID="gvhdfProjectID" Value='<%# DataBinder.Eval(Container.DataItem, "ProjectID")%> '
                                                    runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle CssClass="grayBorder" Width="25px" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <br />
                            <div id="divAssignPaging" class="normal" runat="server">
                                <ucc:PagingControl ID="pgcAssignment" OnPageNoChanged="AssignmentPaging_PageNoChange"
                                    OnRowPerPageChanged="AssignmentPaging_RowPerPageChanged" runat="server" />
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <!-- style="width:80%" -->
                            <div id="div1" class="normalGrey">
                                Current Followup</div>
                            <br />
                            <div style="height: 116px; overflow: auto;">
                                <!--  style="height:260px;overflow:auto;"  -->
                                <asp:GridView ID="gvFollowUp" CssClass="normal" AllowSorting="true" AutoGenerateColumns="false"
                                    PagerSettings-Visible="false" AllowPaging="true" PageSize="5" runat="server"
                                    DataKeyNames="RecId" OnRowCommand="gvFollowUp_RowCommand" OnSorting="gvFollowUp_Sorting"
                                    OnRowDataBound="gvFollowUp_RowDataBound" Width="542px">
                                    <HeaderStyle BackColor="#CDCDCD" CssClass="Freezing" />
                                    <PagerSettings Visible="False" />
                                    <RowStyle BackColor="#FFCCFF" CssClass="grayBorder" />
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                No.
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>
                                                <asp:HiddenField ID="gvhdfRecordId" Value='<%# Container.DataItemIndex %>' runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle CssClass="grayBorder" Width="15px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Lead ID" SortExpression="LeadId">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="gvlbtnAccountNo" CssClass="normal" Text='<%# Bind("LeadId") %>'
                                                    CommandName="Show" CommandArgument="<%# Container.DataItemIndex %>" runat="server"
                                                    Font-Underline="True" ForeColor="Blue" ToolTip="Select Leads" />
                                            </ItemTemplate>
                                            <ItemStyle CssClass="grayBorder" Width="30px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Name" SortExpression="LeadName">
                                            <ItemTemplate>
                                                <%# Eval("LeadName") %>
                                                <%--<asp:LinkButton ID="gvlbtnClientName" CssClass="normal" Text='<%# Bind("LeadName") %>' 
                                                   CommandName="Show" CommandArgument="<%# Container.DataItemIndex %>"
                                                   Font-Underline="True" ForeColor="Blue" runat="server" ToolTip="View History"/>--%>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="grayBorder" Width="100px" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="AECode" SortExpression="AECode" HeaderText="FollowUp Dealer Code">
                                            <ItemStyle CssClass="grayBorder" Width="100px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="FollowUpDate" HeaderText="FollowUp Date" SortExpression="FollowUpDate"
                                            DataFormatString="{0:dd/MM/yyyy}" HtmlEncode="false">
                                            <ItemStyle CssClass="grayBorder" Width="100px" />
                                        </asp:BoundField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <br />
                            <div id="divFollowUpPaging" class="normal" runat="server">
                                <ucc:PagingControl ID="pgcFollowUp" OnPageNoChanged="FollowUpPaging_PageNoChange"
                                    OnRowPerPageChanged="FollowUpPaging_RowPerPageChanged" runat="server" />
                            </div>
                        </td>
                    </tr>
                </table>
                <br />
                <div id="divMessageTwo" class="normalRed" runat="server">
                </div>
                <table style="background-color: #CDCDCD;" cellspacing="1px">
                    <tr>
                        <td id="tdEntryHistory" width="110px" runat="server">
                            <asp:LinkButton ID="lbtnEntryHistory" Text="Entry History" CssClass="normalGrey"
                                runat="server" OnClick="lbtnEntryHistory_Click" />
                        </td>
                        <td id="tdContactHistory" width="110px" runat="server">
                            <asp:LinkButton ID="lbtnContactHistory" Text="Contact History" Enabled="false" CssClass="normalGrey"
                                runat="server" />
                        </td>
                    </tr>
                </table>
                <div id="divContactHistory" style="width: 100%">
                    <asp:GridView ID="gvContactHistory" CssClass="normal" AllowSorting="true" OnRowEditing="gvList_RowEditing"
                        AutoGenerateColumns="false" PagerSettings-Visible="false" OnRowDeleting="gvListDelete_RowDeleting"
                        AllowPaging="true" PageSize="10" runat="server" DataKeyNames="RecId" OnRowCommand="gvContactHistory_RowCommand"
                        OnRowDataBound="gvContactHistory_RowDataBound" OnSorting="gvContactHistory_Sorting">
                        <HeaderStyle BackColor="#CDCDCD" />
                        <RowStyle CssClass="normal" BackColor="White" />
                        <AlternatingRowStyle CssClass="normal" BackColor="#EEEEEE" />
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    No.
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%# Container.DataItemIndex + 1 %>
                                    <asp:HiddenField ID="gvhdfRecordId" Value='<%# Container.DataItemIndex %>' runat="server" />
                                </ItemTemplate>
                                <HeaderStyle BorderColor="#777777" CssClass="grayBorder" Width="15px" />
                                <ItemStyle BorderColor="#777777" CssClass="grayBorder" Width="15px" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="ContactDate" SortExpression="ContactDate" HeaderText="Contact Date"
                                DataFormatString="{0:dd/MM/yyyy HH:mm:ss}" HtmlEncode="false">
                                <HeaderStyle CssClass="grayBorder" />
                                <ItemStyle CssClass="grayBorder" Width="110px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="AEGroup" SortExpression="AEGroup" HeaderText="Team">
                                <HeaderStyle CssClass="grayBorder" />
                                <ItemStyle CssClass="grayBorder" Width="60px" />
                            </asp:BoundField>
                            <asp:TemplateField SortExpression="LeadId" HeaderText="Lead ID">
                                <ItemTemplate>
                                    <asp:LinkButton ID="gvlbtnLeadId" CssClass="normal" Text='<%# Bind("LeadId") %>'
                                        CommandName="AcctNo" CommandArgument="<%# Container.DataItemIndex %>" runat="server"
                                        Font-Underline="True" ForeColor="Blue" ToolTip="Select Leads" />
                                    <asp:Label ID="gvlblLeadId" CssClass="normal" Text='<%# Bind("LeadId") %>' runat="server" />
                                </ItemTemplate>
                                <HeaderStyle CssClass="grayBorder" />
                                <ItemStyle CssClass="grayBorder" Width="40px" />
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="LeadName" HeaderText="Name">
                                <ItemTemplate>
                                    <asp:LinkButton ID="gvlbtnLeadName" CssClass="normal" Text='<%# Bind("LeadName") %>'
                                        CommandName="ClientName" CommandArgument="<%# Container.DataItemIndex %>" Font-Underline="True"
                                        ForeColor="Blue" runat="server" ToolTip="View History" />
                                    <asp:Label ID="gvlblLeadName" CssClass="normal" Text='<%# Bind("LeadName") %>' runat="server" />
                                </ItemTemplate>
                                <HeaderStyle CssClass="grayBorder" />
                                <ItemStyle CssClass="grayBorder" Width="120px" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="Sex" SortExpression="Sex" HeaderText="Gender">
                                <HeaderStyle CssClass="grayBorder" />
                                <ItemStyle CssClass="grayBorder" Width="25px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="LeadMobile" HeaderText="Mobile No" SortExpression="LeadMobile">
                                <HeaderStyle CssClass="grayBorder" />
                                <ItemStyle CssClass="grayBorder" Width="80px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="LeadHome" HeaderText="Home No" SortExpression="LeadHome">
                                <HeaderStyle CssClass="grayBorder" />
                                <ItemStyle CssClass="grayBorder" Width="80px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="LeadEmail" HeaderText="Email" SortExpression="LeadEmail">
                                <HeaderStyle CssClass="grayBorder" />
                                <ItemStyle CssClass="grayBorder" Width="80px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="PreferMode" HeaderText="Preferred Mode of Contact" SortExpression="PreferMode">
                                <HeaderStyle CssClass="grayBorder" />
                                <ItemStyle CssClass="grayBorder" Width="80px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Content" HeaderText="Content">
                                <HeaderStyle CssClass="grayBorder" />
                                <ItemStyle CssClass="grayBorder" Width="160px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Event" HeaderText="Event" SortExpression="Event">
                                <HeaderStyle CssClass="grayBorder" />
                                <ItemStyle CssClass="grayBorder" Width="160px" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Seminars">
                                <HeaderStyle CssClass="grayBorder" />
                                <ItemStyle CssClass="grayBorder" Width="160px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="NeedFollowUp" HeaderText="Follow-up (Y/N)">
                                <HeaderStyle CssClass="grayBorder" />
                                <ItemStyle CssClass="grayBorder" Width="120px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="FollowUpDate" HeaderText="Follow-up Date">
                                <HeaderStyle CssClass="grayBorder" />
                                <ItemStyle CssClass="grayBorder" Width="60px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="AECode" HeaderText="Dealer Code">
                                <HeaderStyle CssClass="grayBorder" />
                                <ItemStyle CssClass="grayBorder" Width="60px" />
                            </asp:BoundField>
                            <asp:TemplateField ShowHeader="False">
                                <ItemTemplate>
                                    <asp:Button ID="gvbtnEdit" runat="server" CausesValidation="False" CommandName="Edit"
                                        Text="Modify" OnClientClick="ScrollToTop()" />
                                    &nbsp;<asp:Button ID="gvbtnDelete" runat="server" CausesValidation="False" OnClientClick="return confirm('Are you sure you want to delete?');"
                                        CommandName="Delete" Text="Delete" />
                                </ItemTemplate>
                                <ControlStyle CssClass="normal" />
                                <HeaderStyle CssClass="grayBorder" />
                                <ItemStyle CssClass="grayBorder" Width="220px" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <br />
                    <!-- Paging For Contact History -->
                    <div id="divCHPaging" class="normal" runat="server">
                        <ucc:PagingControl ID="pgcContactHistory" OnPageNoChanged="ContactHistory_PageNoChange"
                            OnRowPerPageChanged="ContactHistory_RowPerPageChanged" runat="server" />
                    </div>
                </div>
                <asp:Panel ID="panelSync" runat="server" Height="156px" Width="341px" CssClass="normal"
                    Visible="false">
                    <fieldset>
                        <legend class="normal">Synchronisation</legend>
                        <div id="divSyncMessage" runat="server" class="normalRed">
                        </div>
                        <table class="normal" cellpadding="0" cellspacing="0">
                            <tr>
                                <td>
                                    <asp:RadioButton ID="rdoNRIC" runat="server" Text="NRIC / Passport No." GroupName="radSync"
                                        CssClass="normal" OnCheckedChanged="rdo_CheckedChanged" AutoPostBack="True" Checked="True" />
                                    <font color="red"><b>*</b></font>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtSyncNRIC" runat="server" CssClass="normal" Width="180px" Height="25px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:RadioButton ID="rdoAccNo" runat="server" Text="Account No." GroupName="radSync"
                                        CssClass="normal" OnCheckedChanged="rdo_CheckedChanged" AutoPostBack="True" /><font
                                            color="red"><b>*</b></font>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtSyncAccNo" runat="server" CssClass="normal" Width="180px" Height="25px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="center" valign="middle">
                                    &nbsp;
                                    <asp:Button ID="btnSyncSubmit" runat="server" Text="Submit" CssClass="normal" OnClick="btnSyncSubmit_Click" />&nbsp;&nbsp;
                                    <asp:Button ID="btnSyncCancel" runat="server" Text="Cancel" CssClass="normal" OnClick="btnSyncCancel_Click" />
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
