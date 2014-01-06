﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ContactAnalysis.aspx.cs"
    Inherits="SPMWebApp.WebPages.ContactManagement.ContactAnalysis" %>

<%@ Register Src="~/PagingControl.ascx" TagName="PagingControl" TagPrefix="ucc" %>
<%@ Register Src="~/CoolCalendar.ascx" TagName="CoolCalendar" TagPrefix="ucc" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>SPM - Contact Analysis</title>
    <link rel="stylesheet" href="<%=ConfigurationManager.AppSettings["SPMCssFile"]%>"
        type="text/css" />
</head>
<body>
    <div id="Container">
        <form id="frmContactAnalysis" runat="server">
        <div class="title">
            Contact Analysis</div>
        <br />
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

            function GetKeyPress(sender, args) {
                alert("you Pressed enter");
            }    
        </script>

        <%-- <script language="javascript" type="text/javascript">
            function clickButton(e, buttonid) {
               // alert('Hello');
               // var evt = e ? e : window.event;
               // alert(evt);
                var bt = document.getElementById(buttonid);
                alert(bt);
                if (bt) {
                    if (evt.keyCode == 13) {
                        bt.click();
                        return false;
                    }
                }
            }
   
    </script> --%>
        <asp:UpdatePanel ID="uplContactAnalysis" runat="server">
            <Triggers>
                <asp:PostBackTrigger ControlID="btnExcel" />
            </Triggers>
            <ContentTemplate>
                <table class="normal">
                    <tr class="normal">
                        <!-- total cells: 14 -->
                        <td class="normal">
                            <asp:RadioButton ID="RdoContactDate" runat="server" GroupName="RadiobutGroup" CssClass="normal"
                                AutoPostBack="true" OnCheckedChanged="rdo_CheckedChanged" />
                            Contact From:
                        </td>
                        <asp:Panel runat="server" ID="PnFromDate">
                            <td class="normal">
                                <ucc:CoolCalendar ID="calContactFromDate" TimeRequired="false" DateTimeFormat="dd/MM/yyyy"
                                    DateTextRequired="false" DateTextFromValue="" runat="server" />
                            </td>
                        </asp:Panel>
                        <td>
                            Contact To:
                        </td>
                        <asp:Panel runat="server" ID="PnToDate">
                            <td class="normal">
                                <ucc:CoolCalendar ID="calContactToDate" TimeRequired="false" DateTimeFormat="dd/MM/yyyy"
                                    DateTextRequired="false" DateTextFromValue="" runat="server" />
                            </td>
                        </asp:Panel>
                        <td>
                            Dealer
                        </td>
                        <td class="normal">
                            <asp:DropDownList ID="ddlDealer" CssClass="normal" runat="server" />
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td class="normal">
                            <asp:RadioButton ID="rdoProj" runat="server" GroupName="RadiobutGroup" CssClass="normal"
                                AutoPostBack="true" OnCheckedChanged="rdo_CheckedChanged" />
                            Project Name :
                        </td>
                        <td class="normal">
                            <asp:TextBox ID="txtProjName" runat="server" Width="300px" CssClass="normal" OnTextChanged="txtProjName_TextChanged"
                                AutoPostBack="true"></asp:TextBox>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td class="normal">
                            A/C:
                        </td>
                        <td>
                            <asp:TextBox ID="txtAccountNo" Width="154px" CssClass="normal" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="normal">
                            &nbsp;&nbsp;&nbsp;Project Name:
                        </td>
                        <td class="normal">
                            <asp:DropDownList ID="ddlProjName" CssClass="normal" runat="server" Height="23px"
                                Width="185px" />
                        </td>
                    </tr>
                </table>
                <div>
                    <asp:Button ID="btnSearch" Text="Search" CssClass="normal" OnClick="btnSearch_Click"
                        runat="server" />
                    &nbsp;&nbsp;
                    <asp:Button ID="btnExcel" Text="Excel" CssClass="normal" OnClick="btnExcel_Click"
                        runat="server" />
                </div>
                <!-- Colors for CoreClient, Assigned, 2NClient -->
                <div align="center">
                    <table border="1" bordercolor="#777777" cellspacing="0" cellpadding="0" class="normalGrey">
                        <tr class="normal" align="center">
                            <td bgcolor="#FFCC99" width="300px">
                                Recently ReTraded Client
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="divMessage" class="normalRed" runat="server">
                </div>
                <br />
                <div id="divClientContact" align="left">
                    <asp:GridView ID="gvClientContact" CssClass="normal" AutoGenerateColumns="false"
                        AllowSorting="true" AllowPaging="true" PageSize="20" ShowFooter="true" PagerSettings-Visible="false"
                        runat="server" OnSorting="gvClientContact_Sorting" OnRowDataBound="gvClientContact_RowDataBound">
                        <HeaderStyle BackColor="#CDCDCD" />
                        <RowStyle CssClass="normal" BackColor="White" />
                        <AlternatingRowStyle CssClass="normal" BackColor="#EEEEEE" />
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    No.</HeaderTemplate>
                                <ItemTemplate>
                                    <%# Container.DataItemIndex + 1 %></ItemTemplate>
                                <HeaderStyle BorderColor="#777777" CssClass="grayBorder" />
                                <ItemStyle BorderColor="#777777" CssClass="grayBorder" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="ContactDate" DataFormatString="{0:dd/MM/yyyy}" HtmlEncode="false"
                                SortExpression="ContactDate" HeaderText="Contact Date">
                                <HeaderStyle CssClass="grayBorder" />
                                <ItemStyle CssClass="grayBorder" Width="120px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="DealerCode" SortExpression="DealerCode" HeaderText="Dealer">
                                <HeaderStyle CssClass="grayBorder" />
                                <ItemStyle CssClass="grayBorder" Width="90px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="TeamCode" SortExpression="TeamCode" HeaderText="Team">
                                <HeaderStyle CssClass="grayBorder" />
                                <ItemStyle CssClass="grayBorder" Width="80px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="AcctNo" SortExpression="AcctNo" HeaderText="A/C">
                                <HeaderStyle CssClass="grayBorder" />
                                <ItemStyle CssClass="grayBorder" Width="80px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ClientName" SortExpression="ClientName" HeaderText="Name">
                                <HeaderStyle CssClass="grayBorder" />
                                <ItemStyle CssClass="grayBorder" Width="160px" />
                            </asp:BoundField>
                            <%--<asp:BoundField DataField="LTradeDate" dataformatstring="{0:dd/MM/yyyy}" HtmlEncode="false"
                                    SortExpression="LTradeDate" HeaderText="LTD [Before Contact Date]">
                            <HeaderStyle CssClass="grayBorder" />
                            <ItemStyle CssClass="grayBorder" Width="160px" />
                        </asp:BoundField>--%>
                            <asp:TemplateField HeaderText="LTD [Before Contact Date]" SortExpression="LTradeDate">
                                <HeaderStyle CssClass="grayBorder" />
                                <ItemTemplate>
                                    <asp:Label ID="Label1" runat="server" Text='<%# Eval("LTradeDate") %>'></asp:Label>
                                    <%--Text='<%# Eval("LTradeDate") == "" ? "" : Convert.ToDateTime(Eval("LTradeDate")).ToString("dd/MM/yyyy") %>' Visible='<%# !Eval("LTradeDate").ToString().Equals("01/01/1900 12:00:00 AM") %>'--%>
                                </ItemTemplate>
                                <ItemStyle CssClass="grayBorder" Width="160px" />
                            </asp:TemplateField>
                            <%--<asp:BoundField DataField="LastTradeDate" dataformatstring="{0:dd/MM/yyyy}" HtmlEncode="false" FooterText="Total"
                                SortExpression="LastTradeDate" HeaderText="LTD [Current Date]">
                            <HeaderStyle CssClass="grayBorder" />
                            <ItemStyle CssClass="grayBorder" Width="120px" />
                            <FooterStyle CssClass="grayBorder" Font-Bold="true" BackColor="#CDCDCD" />
                        </asp:BoundField>--%>
                            <asp:TemplateField HeaderText="LTD [Current Date]" SortExpression="LastTradeDate">
                                <HeaderStyle CssClass="grayBorder" />
                                <ItemTemplate>
                                    <asp:Label ID="Label1" runat="server" Text='<%# Eval("LastTradeDate") %>'></asp:Label>
                                    <%--Text='<%# Eval("LastTradeDate") == "" ? "" : Convert.ToDateTime(Eval("LastTradeDate")).ToString("dd/MM/yyyy") %>' Visible='<%# !Eval("LastTradeDate").ToString().Equals("01/01/1900 12:00:00 AM") %>'--%>
                                </ItemTemplate>
                                <ItemStyle CssClass="grayBorder" Width="160px" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="TotalComm" SortExpression="TotalComm" FooterText="" HeaderText="Total Comm">
                                <HeaderStyle CssClass="grayBorder" />
                                <ItemStyle CssClass="grayBorder" Width="100px" />
                                <FooterStyle CssClass="grayBorder" Font-Bold="true" BackColor="#CDCDCD" />
                            </asp:BoundField>
                        </Columns>
                    </asp:GridView>
                </div>
                <br />
                <div id="divPaging" class="normal" runat="server">
                    <ucc:PagingControl ID="pgcClientContact" OnPageNoChanged="PagingControl_PageNoChange"
                        OnRowPerPageChanged="PagingControl_RowPerPageChanged" runat="server" />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        </form>
    </div>
</body>
</html>