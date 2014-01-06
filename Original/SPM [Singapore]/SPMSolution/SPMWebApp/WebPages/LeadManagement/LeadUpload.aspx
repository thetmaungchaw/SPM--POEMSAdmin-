<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LeadUpload.aspx.cs" Inherits="SPMWebApp.WebPages.LeadManagement.LeadUpload" %>

<%@ Register Src="~/PagingControl.ascx" TagName="PagingControl" TagPrefix="ucc" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>SPM - Lead Management</title>
    <!--<link rel="stylesheet" href="<%#ConfigurationManager.AppSettings["SPMCssFile"]%>" />-->
    <link rel="Stylesheet" href="../../StyleSheet/SPMStyle.css" />
</head>
<body>
    <div id="Container">
        <form id="frmLeadUpload" runat="server">
        <p class="title">
            SPM Lead Management</p>
        <p class="title">
        </p>
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
            //-->

            //Move to Top when GridView modify button click
            function ScrollToTop() {
                window.scrollTo(0, 0);
            }
        </script>

        <cc1:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0" OnActiveTabChanged="TabContainer1_ActiveTabChanged"
            AutoPostBack="True">
            <cc1:TabPanel ID="TabPanel1" runat="server" HeaderText="Upload">
                <HeaderTemplate>
                    Upload</HeaderTemplate>
                <ContentTemplate>
                    <div id="divMessage" class="normalRed" runat="server">
                    </div>
                    <asp:HiddenField ID="hdfExcelPath" runat="server" />
                    <table style="width: 470px">
                        <tr>
                            <td class="normal">
                                Team :
                                <asp:DropDownList ID="ddlTeamCodeUpload" runat="server" CssClass="normal" Width="191px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="normal">
                                Select file to upload:
                                <asp:FileUpload ID="fileUploadLeads" runat="server" CssClass="normal" Width="43%">
                                </asp:FileUpload>
                                <asp:Button ID="btnUpload" runat="server" OnClick="btnUpload_Click" Text="Upload"
                                    CssClass="normal"></asp:Button>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <fieldset id="fsDup" runat="server" visible="False">
                        <legend class="normal">Duplicate Records</legend>
                        <div id="divDuplicate" runat="server" class="normal">
                        </div>
                        <table id="tblDuplicate" runat="server" visible="False">
                            <tr id="Tr1" runat="server">
                                <td id="Td1" runat="server">
                                    <asp:GridView ID="gvDuplicate" CssClass="normal" AllowPaging="True" PageSize="20"
                                        AutoGenerateColumns="False" AllowSorting="True" OnSorting="gvDuplicate_Sorting"
                                        OnRowCommand="gvDuplicate_RowCommand" runat="server">
                                        <Columns>
                                            <asp:TemplateField Visible="False">
                                                <HeaderStyle BorderColor="#777777" CssClass="grayBorder" />
                                                <ItemStyle BorderColor="#777777" CssClass="grayBorder" />
                                                <HeaderTemplate>
                                                    No.
                                                </HeaderTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Upload Records" SortExpression="Upload Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblUploadName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Upload Name") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="grayBorder" />
                                                <ItemStyle CssClass="grayBorder" Width="200px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="Existing Name" HeaderText="Existing Records">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblExistingName" runat="server" Text='<%#  DataBinder.Eval(Container.DataItem,"Existing Name")  %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="grayBorder" />
                                                <ItemStyle CssClass="grayBorder" Width="200px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="Action" HeaderText="Action">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAction" runat="server" Text='<%#  DataBinder.Eval(Container.DataItem,"Action")  %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="grayBorder" />
                                                <ItemStyle CssClass="grayBorder" Width="100px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="Action" HeaderText="Reason">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblReason" runat="server" Text='<%#  DataBinder.Eval(Container.DataItem,"Reason")  %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="grayBorder" />
                                                <ItemStyle CssClass="grayBorder" Width="100px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField Visible="False">
                                                <ItemTemplate>
                                                    <asp:HiddenField ID="hidLeadId" runat="server" Value='<%#  DataBinder.Eval(Container.DataItem,"LeadId")  %>'>
                                                    </asp:HiddenField>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="grayBorder" />
                                                <ItemStyle CssClass="grayBorder" Width="100px" />
                                            </asp:TemplateField>
                                            <asp:ButtonField ButtonType="Button" Text="Details" CommandName="Details">
                                                <ControlStyle CssClass="normal" />
                                                <HeaderStyle CssClass="grayBorder" />
                                                <ItemStyle CssClass="grayBorder" Width="100px" />
                                            </asp:ButtonField>
                                        </Columns>
                                        <AlternatingRowStyle CssClass="normal" BackColor="#EEEEEE" />
                                        <HeaderStyle BackColor="#CDCDCD" />
                                        <PagerSettings Visible="False" />
                                        <RowStyle CssClass="normal" BackColor="White" />
                                    </asp:GridView>
                                    </br\>
                                </td>
                            </tr>
                            <tr id="Tr2" runat="server">
                                <td id="Td2" align="center" runat="server">
                                    <asp:Button ID="btnSubmit" runat="server" Enabled="False" Text="Submit" OnClick="btnSubmit_Click"
                                        CssClass="normal" />
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                    <div id="divPaging" class="normal" runat="server">
                        <ucc:PagingControl ID="pgcDealer" OnPageNoChanged="PagingControl_PageNoChange" OnRowPerPageChanged="PagingControl_RowPerPageChanged"
                            runat="server" />
                    </div>
                    <div id="div1" class="normalRed" runat="server">
                    </div>
                    <table border="0" cellspacing="0" cellpadding="10" runat="server" id="tblUploadCompare"
                        visible="False">
                        <tr id="Tr3" runat="server">
                            <td id="Td3" valign="top" runat="server">
                                <table border="1" cellpadding="3" cellspacing="0" width="100%">
                                    <tr bgcolor="Silver">
                                        <td align="center" colspan="2" class="normal">
                                            Uploaded Record
                                        </td>
                                    </tr>
                                    <tr bgcolor="#EEEEEE">
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr bgcolor="White">
                                        <td>
                                            <font class="normal">Name:</font>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtUploadName" runat="server" CssClass="normal" Width="150px" Enabled="False"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr bgcolor="#EEEEEE">
                                        <td>
                                            <font class="normal">NRIC/Passport No:</font>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtUploadNRIC" runat="server" CssClass="normal" Width="150px" Enabled="False"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr bgcolor="White">
                                        <td>
                                            <font class="normal">Mobile No:</font>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtUploadMobileNo" runat="server" Width="150px" CssClass="normal"
                                                Enabled="False"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr bgcolor="#EEEEEE">
                                        <td>
                                            <font class="normal">Home No:</font>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtUploadHome" runat="server" CssClass="normal" Width="150px" Enabled="False"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr bgcolor="White">
                                        <td>
                                            <font class="normal">Gender:</font>
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="radUploadFemale" runat="server" CssClass="normal" Enabled="False"
                                                GroupName="Gender" Text="Female"></asp:RadioButton>
                                            <asp:RadioButton ID="radUploadMale" runat="server" CssClass="normal" Enabled="False"
                                                GroupName="Gender" Text="Male"></asp:RadioButton>
                                        </td>
                                    </tr>
                                    <tr bgcolor="#EEEEEE">
                                        <td>
                                            <font class="normal">Event:</font>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtUploadEvent" runat="server" CssClass="normal" Width="150px" Enabled="False"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr bgcolor="White">
                                        <td>
                                            <font class="normal">Email :</font>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtUploadEmail" runat="server" CssClass="normal" Width="150px" Enabled="False"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr bgcolor="#EEEEEE">
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr bgcolor="White">
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr bgcolor="#EEEEEE">
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr bgcolor="White">
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td id="Td4" runat="server" valign="top">
                                <asp:DetailsView ID="dvExistingRecord" runat="server" AutoGenerateRows="False" HeaderText="Existing Record"
                                    Width="100%" OnDataBound="dvExistingRecord_DataBound">
                                    <AlternatingRowStyle CssClass="normal" BackColor="#EEEEEE" />
                                    <Fields>
                                        <asp:TemplateField Visible="False">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtLeadId" runat="server" Enabled="false" CssClass="normal" Width="150px"
                                                    Text='<%#  DataBinder.Eval(Container.DataItem,"LeadId")  %>' Visible="false"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Team">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtTeam" runat="server" Enabled="false" CssClass="normal" Width="150px"
                                                    Text='<%#  DataBinder.Eval(Container.DataItem,"TeamName")  %>'></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Name">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtNameTesting" runat="server" Enabled="false" CssClass="normal"
                                                    Width="150px" Text='<%#  DataBinder.Eval(Container.DataItem,"LeadName")  %>'></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="NRIC/Passport No.">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtNRIC" runat="server" Enabled="false" CssClass="normal" Width="150px"
                                                    Text='<%#  DataBinder.Eval(Container.DataItem,"LeadNRIC")  %>'></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Mobile No.">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtMobile" runat="server" Enabled="false" CssClass="normal" Width="150px"
                                                    Text='<%#  DataBinder.Eval(Container.DataItem,"LeadMobile")  %>'></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Home No.">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtHomeNo" runat="server" Enabled="false" CssClass="normal" Width="150px"
                                                    Text='<%#  DataBinder.Eval(Container.DataItem,"LeadHomeNo")  %>'></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Gender">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hidGender" runat="server" Value='<%#  DataBinder.Eval(Container.DataItem,"LeadGender")  %>'>
                                                </asp:HiddenField>
                                                <asp:RadioButton ID="radExistingFemale" runat="server" GroupName="Gender" Enabled="false"
                                                    Text="Female" CssClass="normal" />
                                                <asp:RadioButton ID="radExistingMale" runat="server" GroupName="Gender" Enabled="false"
                                                    Text="Male" CssClass="normal" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Event">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtEvent" runat="server" Enabled="false" CssClass="normal" Width="150px"
                                                    Text='<%#  DataBinder.Eval(Container.DataItem,"Event")  %>'></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Email">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtEmail" runat="server" Enabled="false" CssClass="normal" Width="150px"
                                                    Text='<%#  DataBinder.Eval(Container.DataItem,"LeadEmail")  %>'></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Last Call Dealer">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtLastCallDealer" runat="server" Enabled="false" Text='<%# Eval("LastCallDealer") %>' CssClass="normal"
                                                    Width="150px"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Last Call Date">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtCallDate" runat="server" Enabled="false" Text='<%# Eval("LastCallDate") %>' CssClass="normal"
                                                    Width="150px"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Last Assign Dealer">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtLastAssignDealer" runat="server" Enabled="false" Text='<%# Eval("LastAssignDealer") %>' CssClass="normal"
                                                    Width="150px"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Last Assign Date">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtLastCallDate" runat="server" Enabled="false" Text='<%# Eval("LastCallDate") %>' CssClass="normal"
                                                    Width="150px"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Uploaded By">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtUploadedBy" runat="server" Enabled="false" Text='<%# Eval("UploadedBy") %>' CssClass="normal"
                                                    Width="150px"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Uploaded Date">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtUploadedDate" runat="server" Enabled="false" Text='<%# Eval("UploadedDate") %>' CssClass="normal"
                                                    Width="150px"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Fields>
                                    <HeaderStyle CssClass="normal" BackColor="Silver" HorizontalAlign="Center" />
                                    <RowStyle CssClass="normal" BackColor="White" />
                                </asp:DetailsView>
                            </td>
                        </tr>
                        <tr id="Tr4" runat="server" align="center">
                            <td id="Td5" runat="server" colspan="2">
                                <asp:Button ID="btnUpdateUpload" runat="server" OnClick="btnUpdateUpload_Click" Text="Retain"
                                    CssClass="normal" />
                                <asp:Button ID="btnSkipUpload" runat="server" OnClick="btnSkipUpload_Click" Text="Remove duplicate(s)"
                                    CssClass="normal" />
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </cc1:TabPanel>
            <cc1:TabPanel ID="TabPanel2" runat="server" HeaderText="Key In">
                <ContentTemplate>
                    <div id="divMessage2" class="normalRed" runat="server">
                    </div>
                    <table border="0" cellspacing="0" cellpadding="10">
                        <tr valign="top">
                            <td valign="top">
                                <table border="1" cellpadding="0" cellspacing="0">
                                    <tr bgcolor="Silver">
                                        <td align="center" colspan="2" class="normal">
                                            Key In Record
                                        </td>
                                    </tr>
                                    <tr bgcolor="#EEEEEE">
                                        <td>
                                            <font class="normal">Team: </font><font class="normalRed">*</font>
                                        </td>
                                        <td>
                                            <asp:HiddenField ID="hidLeadsId" runat="server" />
                                            <asp:DropDownList ID="ddlTeamCodeKeyIn" Width="250px" runat="server" CssClass="normal">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr bgcolor="white">
                                        <td>
                                            <font class="normal">Name:</font> <font class="normalRed">*</font>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtName" runat="server" CssClass="normal" Width="250px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr bgcolor="#EEEEEE">
                                        <td>
                                            <font class="normal">NRIC/Passport No:</font>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtNRIC" runat="server" CssClass="normal" Width="250px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr bgcolor="white">
                                        <td>
                                            <font class="normal">Mobile No:</font><font class="normalRed">*</font>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtMobileNo" runat="server" CssClass="normal" Width="250px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr bgcolor="#EEEEEE">
                                        <td>
                                            <font class="normal">Home No:</font>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtHomeNo" Width="250px" runat="server" CssClass="normal"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr bgcolor="white">
                                        <td>
                                            <font class="normal">Gender:</font><font class="normalRed">*</font>
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="radFemale" runat="server" Checked="true" GroupName="Gender"
                                                Text="Female" CssClass="normal" />
                                            <asp:RadioButton ID="radMale" runat="server" GroupName="Gender" Text="Male" CssClass="normal" />
                                        </td>
                                    </tr>
                                    <tr bgcolor="#EEEEEE">
                                        <td>
                                            <font class="normal">Event:</font>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtEvent" runat="server" CssClass="normal" Width="250px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr bgcolor="white">
                                        <td>
                                            <font class="normal">Email :</font>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtEmail" runat="server" CssClass="normal" Width="250px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr bgcolor="#EEEEEE">
                                        <td>
                                            <font class="normal">Key In By</font>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtKeyInBy" runat="server" Enabled="false" Visible="true" CssClass="normal"
                                                Width="250px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" align="center">
                                            <asp:Button ID="btnAddRecord" runat="server" OnClick="btnAddRecord_Click" CssClass="normal"
                                                Text="Add New Record" />
                                            <asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click" CssClass="normal"
                                                Text="Cancel" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td>
                                <asp:DetailsView ID="dtvExisting" runat="server" AutoGenerateRows="false" EnableViewState="true"
                                    HeaderStyle-BackColor="Silver" OnDataBound="dtvExisting_DataBound" HeaderText="Existing Record"
                                    CssClass="normal" HeaderStyle-HorizontalAlign="Center">
                                    <Fields>
                                        <asp:TemplateField Visible="false">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtLeadId" runat="server" Enabled="false" CssClass="normal" Text='<%#  DataBinder.Eval(Container.DataItem,"LeadId") %>'
                                                    Visible="false" Width="250px">
                                                </asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Team">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtTeam" runat="server" Enabled="false" CssClass="normal" Text='<%#  DataBinder.Eval(Container.DataItem,"TeamName")  %>'
                                                    Width="250px">
                                                </asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Dealer">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtDealer" runat="server" Enabled="false" CssClass="normal" Text='<%#  DataBinder.Eval(Container.DataItem,"AEName")  %>'
                                                    Width="250px">
                                                </asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Name">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtNameTesting" runat="server" Enabled="false" CssClass="normal"
                                                    Text='<%#  DataBinder.Eval(Container.DataItem,"LeadName")  %>' Width="250px">
                                                </asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="NRIC/Passport No.">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtNRIC" runat="server" Enabled="false" CssClass="normal" Text='<%#  DataBinder.Eval(Container.DataItem,"LeadNRIC")  %>'
                                                    Width="250px">
                                                </asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Account No.">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtAccount" runat="server" Enabled="false" Text="NA" CssClass="normal"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Mobile No.">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtMobile" runat="server" Enabled="false" CssClass="normal" Text='<%#  DataBinder.Eval(Container.DataItem,"LeadMobile")  %>'
                                                    Width="250px">
                                                </asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Home No.">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtHomeNo" runat="server" Enabled="false" CssClass="normal" Text='<%#  DataBinder.Eval(Container.DataItem,"LeadHomeNo")  %>'
                                                    Width="250px">
                                                </asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Gender">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hidGender" runat="server" Value='<%#  DataBinder.Eval(Container.DataItem,"LeadGender")  %>'>
                                                </asp:HiddenField>
                                                <asp:RadioButton ID="radFemale" runat="server" GroupName="Gender" Enabled="false"
                                                    Text="Female" CssClass="normal" />
                                                <asp:RadioButton ID="radMale" runat="server" GroupName="Gender" Enabled="false" Text="Male"
                                                    CssClass="normal" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Event">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtEvent" runat="server" Enabled="false" CssClass="normal" Text='<%#  DataBinder.Eval(Container.DataItem,"Event")  %>'
                                                    Width="250px">
                                                </asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Email">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtEmail" runat="server" Enabled="false" CssClass="normal" Text='<%#  DataBinder.Eval(Container.DataItem,"LeadEmail")  %>'
                                                    Width="250px">
                                                </asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Last Call Dealer">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtLastCallDealer" runat="server" Enabled="false" CssClass="normal"
                                                    Text="" Width="250px">
                                                </asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Last Call Date">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtCallDate" runat="server" Enabled="false" Text="" CssClass="normal"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Last Assign Dealer">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtLastAssignDealer" runat="server" Enabled="false" CssClass="normal"
                                                    Text="" Width="250px">
                                                </asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Last Call Date">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtLastCallDate" runat="server" Enabled="false" CssClass="normal"
                                                    Text="" Width="250px">
                                                </asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Fields>
                                </asp:DetailsView>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="center">
                                <asp:Button ID="btnUpdate" runat="server" OnClick="btnUpdate_Click" Text="Retain"
                                    Visible="false" CssClass="normal" />
                                <asp:Button ID="btnSkip" runat="server" OnClick="btnSkip_Click" Text="Remove duplicate(s)"
                                    Visible="false" CssClass="normal" />
                            </td>
                        </tr>
                    </table>
                    <div id="LeadsHistory" cssclass="normal" runat="server">
                        <fieldset>
                            <legend runat="server" id="lgEntry" class="normal">Entry History</legend>
                            <asp:GridView ID="gvLeadsHistory" CssClass="normal" AllowSorting="true" PersistedSelection="true"
                                AllowPaging="true" PageSize="10" OnRowEditing="gvList_RowEditing" OnRowDeleting="gvLeadsHistory_RowDeleting"
                                AutoGenerateColumns="false" runat="server" OnPageIndexChanged="gvLeadsHistory_PageIndexChanged"
                                OnPageIndexChanging="gvLeadsHistory_PageIndexChanging" DataKeyNames="LeadId"
                                OnRowDataBound="gvLeadsHistory_RowDataBound" OnSorting="gvLeadsHistory_Sorting">
                                <HeaderStyle BackColor="#CDCDCD" />
                                <RowStyle CssClass="normal" BackColor="White" />
                                <AlternatingRowStyle CssClass="normal" BackColor="#EEEEEE" />
                                <Columns>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            No.</HeaderTemplate>
                                        <ItemTemplate>
                                            <%# Container.DataItemIndex + 1 %>
                                            <asp:HiddenField ID="gvhdfRecordId" Value='<%# Container.DataItemIndex %>' runat="server" />
                                        </ItemTemplate>
                                        <HeaderStyle BorderColor="#777777" CssClass="grayBorder" Width="15px" />
                                        <ItemStyle BorderColor="#777777" CssClass="grayBorder" Width="15px" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="AEGroup" SortExpression="AEGroup" HeaderText="Team">
                                        <HeaderStyle CssClass="grayBorder" />
                                        <ItemStyle CssClass="grayBorder" Width="60px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="LeadName" SortExpression="LeadName" HeaderText="Name">
                                        <HeaderStyle CssClass="grayBorder" />
                                        <ItemStyle CssClass="grayBorder" Width="150px" HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="LeadNRIC" SortExpression="LeadNRIC" HeaderText="NRIC / Passport No">
                                        <HeaderStyle CssClass="grayBorder" />
                                        <ItemStyle CssClass="grayBorder" Width="80px" HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="LeadGender" HeaderText="Gender" SortExpression="LeadGender">
                                        <HeaderStyle CssClass="grayBorder" />
                                        <ItemStyle CssClass="grayBorder" Width="80px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="LeadMobile" HeaderText="Mobile No" SortExpression="LeadMobile">
                                        <HeaderStyle CssClass="grayBorder" />
                                        <ItemStyle CssClass="grayBorder" Width="100px" HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="LeadHomeNo" HeaderText="Home No" SortExpression="LeadHomeNo">
                                        <HeaderStyle CssClass="grayBorder" />
                                        <ItemStyle CssClass="grayBorder" Width="100px" HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="LeadEmail" HeaderText="Email" SortExpression="LeadEmail">
                                        <HeaderStyle CssClass="grayBorder" />
                                        <ItemStyle CssClass="grayBorder" Width="160px" HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Event" HeaderText="Event" SortExpression="Event">
                                        <HeaderStyle CssClass="grayBorder" Width="150px" />
                                        <ItemStyle CssClass="grayBorder" Width="100px" HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="AECode" HeaderText="Dealer Code" SortExpression="AECode">
                                        <HeaderStyle CssClass="grayBorder" />
                                        <ItemStyle CssClass="grayBorder" Width="60px" />
                                    </asp:BoundField>
                                    <asp:TemplateField ShowHeader="False">
                                        <ItemTemplate>
                                            <asp:Button ID="gvbtnEdit" runat="server" CausesValidation="False" CommandName="Edit"
                                                Text="Modify" OnClientClick="ScrollToTop()" />
                                            <asp:Button ID="gvbtnDelete" runat="server" CausesValidation="False" OnClientClick="return confirm('Are you sure you want to delete?');"
                                                CommandName="Delete" Text="Delete" />
                                        </ItemTemplate>
                                        <ControlStyle CssClass="normal" />
                                        <HeaderStyle CssClass="grayBorder" />
                                        <ItemStyle CssClass="grayBorder" Width="120px" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <br />
                            <%--<div id="divLeadHistoryPaging" class="normal" runat="server">
                                            <ucc:PagingControl ID="pgcLeadHistory" OnPageNoChanged="PagingControl_LeadHistory_PageNoChange" OnRowPerPageChanged="PagingControl_LeadHistory_RowPerPageChanged"
                                                runat="server"/>
                                        </div>--%>
                        </fieldset>
                    </div>
                </ContentTemplate>
            </cc1:TabPanel>
        </cc1:TabContainer>
        <br />
        
        <asp:Panel ID="Panel1" runat="server" GroupingText=" " Width="70%" CssClass="normal">
            . The file must be a .xls format file.<br />
            . Note that the first line of the file is a header line and will be ignored.<br />
            . The file must include S/N, Name, Gender, NRIC/Passport, Mobile No., Home No, Email and Event Name columns.
        </asp:Panel>
        
        </form>
    </div>
</body>
</html>
