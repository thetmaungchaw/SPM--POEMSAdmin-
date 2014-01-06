﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ContactEntry.aspx.cs" Inherits="SPMWebApp.WebPages.ContactManagement.ContactEntry" %>

<%@ Register Src="~/PagingControl.ascx" TagName="PagingControl" TagPrefix="ucc" %>

<%@ Register Src="~/CoolCalendar.ascx" TagName="CoolCalendar" TagPrefix="ucc" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <!-- Mimic Internet Explorer 7 -->
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7">
    <title>SPM - Contact Entry</title>
    <link rel="stylesheet" href="<%=ConfigurationManager.AppSettings["SPMCssFile"]%>"
        type="text/css" />

    <script type="text/javascript" language="Javascript">

        function openwindow() {
            var value = document.getElementById('<%=txtAccountNo.ClientID%>').value;
            if (value != "") {
                var teamCode = document.getElementById('<%=hdfTeamCode.ClientID%>').value;
                if (teamCode != "") {
                    window.open("SentEmailToContact.aspx?AccNo=" + value + "&TeamCode=" + teamCode, 'window', 'width=630,height=300,background=silver,menubar=no, resizable=no');
                }
                else {
                    alert("Please select dealer");
                }
            }
            else {
                alert("Please add client information");
            }
        }

        function handleShortKeyBlur() {
            __doPostBack('btnShortKey', '');
        }

        //Move to Top when GridView modify button click
        //top:expression(this.offsetParent.scrollTop);
        function ScrollToTop() {
            window.scrollTo(0, 0);
        }

        function MouseHover() {
            newwindow = window.open('AvailableFundsBalance.aspx', 'AvailableFundsBalance', 'height=400,width=800,scrollbars=yes');
            if (window.focus) { newwindow.focus() }
            return false;
        }  
    </script>

    <style type="text/css">
        .tab_none
        {
            background-color: Gray;
        }
        .LabelUnderline
        {
            text-decoration: none;
            border-bottom: 2px dashed;
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
    </style>
</head>
<body>
    <form id="frmContactEntry" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <%--<asp:UpdatePanel ID="updatePanel1" runat="server">
        <ContentTemplate>--%>
            <div id="Container">

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

                <div id="divTitle" class="title" runat="server">
                    Contact Entry</div>
                <table style="background-color: #CDCDCD;" cellspacing="1px">
                    <tr>
                        <td id="td1" width="110px" runat="server" style="background-color: #6E6A6B">
                            <asp:LinkButton ID="btnEntry" Text="Entry" CssClass="normalGrey" runat="server" OnClick="btnEntry_Click" />
                        </td>
                        <td id="td2" width="150px" runat="server">
                            <asp:LinkButton ID="btnClientOverView" Text="Overview of Client" CssClass="normalGrey"
                                runat="server" OnClick="btnClientOverView_Click" />
                        </td>
                    </tr>
                </table>
                <div id="divMessage" class="normalRed" runat="server">
                </div>
                <div id="divClientOverViewForm" runat="server" visible="false">
                    <table cssclass="normal" border="0" style="text-align: left; vertical-align: top;
                        font-family: Arial,Helvetica,sans-serif; font-size: 10px; padding: 0 0 10px;
                        font-style: normal; text-decoration: none; line-height: 18px;" width="100%">
                        <tr>
                            <td align="center">
                                <h5>
                                    T-series</h5>
                            </td>
                        </tr>
                        <tr>
                            <td cssclass="normal">
                                <asp:CheckBox ID="chkAdvisoryAccount" runat="server" Width="23px" CssClass="normal" />Advisory
                                Account
                            </td>
                            <td cssclass="normal">
                                <asp:Label ID="txtAdvisoryAccount" runat="server" CssClass="normal" Font-Bold="true"></asp:Label>
                            </td>
                            <td>
                                &nbsp; &nbsp; &nbsp; &nbsp;
                            </td>
                            <td cssclass="normal">
                                <asp:CheckBox ID="chkCashManagement" runat="server" Width="23px" CssClass="normal" />Cash
                                Management
                            </td>
                            <td cssclass="normal">
                                <asp:Label ID="txtCashManagement" runat="server" CssClass="normal" Font-Bold="true"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td cssclass="normal">
                                <asp:CheckBox ID="chkCFD" runat="server" Width="23px" CssClass="normal" />CFD
                            </td>
                            <td cssclass="normal">
                                <asp:Label ID="txtCFD" runat="server" CssClass="normal" Font-Bold="true"></asp:Label>
                                <td>
                                    &nbsp; &nbsp;
                                </td>
                                <td cssclass="normal">
                                    <asp:CheckBox ID="chkCashTrading" runat="server" Width="23px" CssClass="normal" />Cash
                                    Trading
                                </td>
                                <td cssclass="normal">
                                    <asp:Label ID="txtCashTrading" runat="server" CssClass="normal" Font-Bold="true"></asp:Label>
                                </td>
                        </tr>
                        <tr>
                            <td cssclass="normal">
                                <asp:CheckBox ID="chkUTNW" runat="server" Width="23px" CssClass="normal" />Unit
                                Trust Non-Wrap
                            </td>
                            <td cssclass="normal">
                                <asp:Label ID="txtUTNW" runat="server" CssClass="normal" Font-Bold="true"></asp:Label>
                            </td>
                            <td>
                                &nbsp; &nbsp;
                            </td>
                            <td cssclass="normal">
                                <asp:CheckBox ID="chkCustodian" runat="server" Width="23px" CssClass="normal" />Custodian
                            </td>
                            <td cssclass="normal">
                                <asp:Label ID="txtCustodian" runat="server" CssClass="normal" Font-Bold="true"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td cssclass="normal">
                                <asp:CheckBox ID="chkDiscretionaryAcct" runat="server" Width="23px" CssClass="normal" />Discretionary
                                Account
                            </td>
                            <td cssclass="normal">
                                <asp:Label ID="txtDiscretionaryAcct" runat="server" CssClass="normal" Font-Bold="true"></asp:Label>
                            </td>
                            <td>
                                &nbsp; &nbsp;
                            </td>
                            <td cssclass="normal">
                                <asp:CheckBox ID="chkPhillipMargin" runat="server" Width="23px" CssClass="normal" />Phillip
                                Margin
                            </td>
                            <td cssclass="normal">
                                <asp:Label ID="txtPhillipMargin" runat="server" CssClass="normal" Font-Bold="true"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td cssclass="normal">
                                <asp:CheckBox ID="chkPhillipFinancial" runat="server" Width="23px" CssClass="normal" />Phillip
                                Financial
                            </td>
                            <td cssclass="normal">
                                <asp:Label ID="txtPhillipFinancial" runat="server" CssClass="normal" Font-Bold="true"></asp:Label>
                            </td>
                            <td>
                                &nbsp; &nbsp;
                            </td>
                            <td cssclass="normal">
                                <%--<asp:CheckBox ID="chkPhillipFinancial" runat="server" Width="23px" CssClass="normal" />--%>
                            </td>
                            <td cssclass="normal">
                                <%--<asp:Label ID="txtPhillipFinancial" runat="server" CssClass="normal" Font-Bold="true"></asp:Label>--%>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <h5>
                                    Non T-series</h5>
                            </td>
                        </tr>
                        <tr>
                            <td cssclass="normal">
                                <asp:CheckBox ID="chkNonTAdvisoryAcct" runat="server" Width="23px" CssClass="normal" />Advisory
                                Account
                            </td>
                            <td cssclass="normal">
                                <asp:Label ID="txtNonTAdvisoryAcct" runat="server" CssClass="normal" Font-Bold="true"></asp:Label>
                            </td>
                            <td>
                                &nbsp; &nbsp;
                            </td>
                            <td cssclass="normal">
                                <asp:CheckBox ID="chkNonTCashAcct" runat="server" Width="23px" CssClass="normal" />Cash
                                Management
                            </td>
                            <td cssclass="normal">
                                <asp:Label ID="txtNonTCashAcct" runat="server" CssClass="normal" Font-Bold="true"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td cssclass="normal">
                                <asp:CheckBox ID="chkNonTCFD" runat="server" Width="23px" CssClass="normal" />CFD
                            </td>
                            <td cssclass="normal">
                                <asp:Label ID="txtNonTCFD" runat="server" CssClass="normal" Font-Bold="true"></asp:Label>
                            </td>
                            <td>
                                &nbsp; &nbsp;
                            </td>
                            <td cssclass="normal">
                                <asp:CheckBox ID="chkNonTCashTrading" runat="server" Width="23px" CssClass="normal" />Cash
                                Trading
                            </td>
                            <td cssclass="normal">
                                <asp:Label ID="txtNonTCashTrading" runat="server" CssClass="normal" Font-Bold="true"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td cssclass="normal">
                                <asp:CheckBox ID="chkNonTUTNW" runat="server" Width="23px" CssClass="normal" />Unit
                                Trust Non-Wrap
                            </td>
                            <td cssclass="normal">
                                <asp:Label ID="txtNonTUTNW" runat="server" CssClass="normal" Font-Bold="true"></asp:Label>
                            </td>
                            <td>
                                &nbsp; &nbsp;
                            </td>
                            <td cssclass="normal">
                                <asp:CheckBox ID="chkNonTCustodian" runat="server" Width="23px" CssClass="normal" />Custodian
                            </td>
                            <td cssclass="normal">
                                <asp:Label ID="txtNonTCustodian" runat="server" CssClass="normal" Font-Bold="true"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td cssclass="normal">
                                <asp:CheckBox ID="chkNonTDiscretionaryAcct" runat="server" Width="23px" CssClass="normal" />Discretionary
                                Account
                            </td>
                            <td cssclass="normal">
                                <asp:Label ID="txtNonTDiscretionaryAcct" runat="server" CssClass="normal" Font-Bold="true"></asp:Label>
                            </td>
                            <td>
                                &nbsp; &nbsp;
                            </td>
                            <td cssclass="normal">
                                <asp:CheckBox ID="chkNonTPhillpMargin" runat="server" Width="23px" CssClass="normal" />Phillip
                                Margin
                            </td>
                            <td cssclass="normal">
                                <asp:Label ID="txtNonTPhillipMargin" runat="server" CssClass="normal" Font-Bold="true"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td cssclass="normal">
                                <asp:CheckBox ID="chkNonTPhillipFinancial" runat="server" Width="23px" CssClass="normal" />Phillip
                                Financial
                            </td>
                            <td cssclass="normal">
                                <asp:Label ID="txtNonTPhillipFinancial" runat="server" CssClass="normal" Font-Bold="true"></asp:Label>
                            </td>
                            <td>
                                &nbsp; &nbsp;
                            </td>
                            <td cssclass="normal">
                                <%--<asp:CheckBox ID="chkNonTPhillipFinancial" runat="server" Width="23px" CssClass="normal" />--%>
                            </td>
                            <td cssclass="normal">
                                <%--<asp:Label ID="txtNonTPhillipFinancial" runat="server" CssClass="normal" Font-Bold="true"></asp:Label>--%>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <asp:FormView ID="ClientInfoView" runat="server" AllowPaging="False" EnableViewState="False"
                        CssClass="normal">
                        <ItemTemplate>
                            <table border="0" width="800px">
                                <tr>
                                    <td cssclass="normal">
                                        Name:
                                    </td>
                                    <td cssclass="normal">
                                        <%# Eval("Name")%>
                                    </td>
                                </tr>
                                <tr>
                                    <td cssclass="normal">
                                        Address:
                                    </td>
                                    <td cssclass="normal">
                                        <%# Eval("Address")%>
                                    </td>
                                    <td>
                                        &nbsp; &nbsp; &nbsp; &nbsp;
                                    </td>
                                    <td cssclass="normal">
                                        Email:
                                    </td>
                                    <td cssclass="normal">
                                        <%# Eval("EMAIL")%>
                                    </td>
                                    <td>
                                        &nbsp; &nbsp; &nbsp; &nbsp;
                                    </td>
                                    <td cssclass="normal">
                                        Office No:
                                    </td>
                                    <td cssclass="normal">
                                        <%# Eval("OFFICENO")%>
                                    </td>
                                </tr>
                                <tr>
                                    <td cssclass="normal">
                                        Postal Code:
                                    </td>
                                    <td cssclass="normal">
                                        <%# Eval("Address")%>
                                    </td>
                                    <td>
                                        &nbsp; &nbsp; &nbsp; &nbsp;
                                    </td>
                                    <td cssclass="normal">
                                        Home No:
                                    </td>
                                    <td cssclass="normal">
                                        <%# Eval("HOMENO")%>
                                    </td>
                                    <td>
                                        &nbsp; &nbsp; &nbsp; &nbsp;
                                    </td>
                                    <td cssclass="normal">
                                        Mobile No:
                                    </td>
                                    <td cssclass="normal">
                                        <%# Eval("MOBILENO")%>
                                    </td>
                                </tr>
                                <tr>
                                    <td cssclass="normal">
                                        CDP Inv A/C:
                                    </td>
                                    <td cssclass="normal">
                                        <%# Eval("CPFInvAC")%>
                                    </td>
                                    <td>
                                        &nbsp; &nbsp; &nbsp; &nbsp;
                                    </td>
                                    <td cssclass="normal">
                                        GSA:
                                    </td>
                                    <td cssclass="normal">
                                        <%# Eval("GSA")%>
                                    </td>
                                    <td>
                                        &nbsp; &nbsp; &nbsp; &nbsp;
                                    </td>
                                    <td cssclass="normal">
                                        Occupation:
                                    </td>
                                    <td cssclass="normal">
                                        <%# Eval("Occupation")%>
                                    </td>
                                </tr>
                                <tr>
                                    <td cssclass="normal">
                                        SRS A/C:
                                    </td>
                                    <td cssclass="normal">
                                        <%# Eval("SRSAC")%>
                                    </td>
                                    <td>
                                        &nbsp; &nbsp; &nbsp; &nbsp;
                                    </td>
                                    <td cssclass="normal">
                                        Employer:
                                    </td>
                                    <td cssclass="normal">
                                        <%# Eval("Employer")%>
                                    </td>
                                    <td>
                                        &nbsp; &nbsp; &nbsp; &nbsp;
                                    </td>
                                    <td cssclass="normal">
                                        Birth Date:
                                    </td>
                                    <td cssclass="normal">
                                        <%# Eval("BirthDate")%>
                                    </td>
                                </tr>
                                <tr>
                                    <td cssclass="normal">
                                        EPS Bank A/C:
                                    </td>
                                    <td cssclass="normal">
                                        <%# Eval("EPSBankAC")%>
                                    </td>
                                    <td>
                                        &nbsp; &nbsp; &nbsp; &nbsp;
                                    </td>
                                    <td cssclass="normal">
                                        NRIC:
                                    </td>
                                    <td cssclass="normal">
                                        <%# Eval("NRIC")%>
                                    </td>
                                    <td>
                                        &nbsp; &nbsp; &nbsp; &nbsp;
                                    </td>
                                    <td cssclass="normal">
                                        PR Status:
                                    </td>
                                    <td cssclass="normal">
                                        <%# Eval("PRStatus")%>
                                    </td>
                                </tr>
                                <tr>
                                    <td cssclass="normal">
                                        GIRO Bank A/C:
                                    </td>
                                    <td cssclass="normal">
                                        <%# Eval("GIROBankAC")%>
                                    </td>
                                    <td>
                                        &nbsp; &nbsp; &nbsp; &nbsp;
                                    </td>
                                    <td cssclass="normal">
                                        Nationality:
                                    </td>
                                    <td cssclass="normal">
                                        <%# Eval("Nationality")%>
                                    </td>
                                    <td>
                                        &nbsp; &nbsp; &nbsp; &nbsp;
                                    </td>
                                    <td cssclass="normal">
                                        Msia A\C:
                                    </td>
                                    <td cssclass="normal">
                                        <%# Eval("MsiaAC")%>
                                    </td>
                                </tr>
                                <tr>
                                    <td cssclass="normal">
                                        Ref Bank A/C:
                                    </td>
                                    <td cssclass="normal">
                                        <%# Eval("RefBankAC")%>
                                    </td>
                                    <td>
                                        &nbsp; &nbsp; &nbsp; &nbsp;
                                    </td>
                                    <td cssclass="normal">
                                        POEMS A\C:
                                    </td>
                                    <td cssclass="normal">
                                        <%# Eval("POEMSAC")%>
                                    </td>
                                    <td>
                                        &nbsp; &nbsp; &nbsp; &nbsp;
                                    </td>
                                    <td cssclass="normal">
                                        US Online:
                                    </td>
                                    <td cssclass="normal">
                                        <%# Eval("USOnline")%>
                                    </td>
                                </tr>
                                <tr>
                                    <td cssclass="normal">
                                        Ref Bank:
                                    </td>
                                    <td cssclass="normal">
                                        <%# Eval("RefBank")%>
                                    </td>
                                    <td>
                                        &nbsp; &nbsp; &nbsp; &nbsp;
                                    </td>
                                    <td cssclass="normal">
                                        W8 Form:
                                    </td>
                                    <td cssclass="normal">
                                        <%# Eval("W8Form")%>
                                    </td>
                                    <td>
                                        &nbsp; &nbsp; &nbsp; &nbsp;
                                    </td>
                                    <td cssclass="normal">
                                        OTC:
                                    </td>
                                    <td cssclass="normal">
                                        <%# Eval("OTC")%>
                                    </td>
                                </tr>
                                <tr>
                                    <td cssclass="normal">
                                        CPF Bank:
                                    </td>
                                    <td cssclass="normal">
                                        <%# Eval("CPFBank")%>
                                    </td>
                                    <td>
                                        &nbsp; &nbsp; &nbsp; &nbsp;
                                    </td>
                                    <td cssclass="normal">
                                        GSA Link:
                                    </td>
                                    <td cssclass="normal">
                                        <%# Eval("GSALink")%>
                                    </td>
                                    <td>
                                        &nbsp; &nbsp; &nbsp; &nbsp;
                                    </td>
                                    <td cssclass="normal">
                                        Future CFD:
                                    </td>
                                    <td cssclass="normal">
                                        <%# Eval("FuturesCFD")%>
                                    </td>
                                </tr>
                                <tr>
                                    <td cssclass="normal">
                                        SRS Bank:
                                    </td>
                                    <td cssclass="normal">
                                        <%# Eval("SRSBank")%>
                                    </td>
                                    <td>
                                        &nbsp; &nbsp; &nbsp; &nbsp;
                                    </td>
                                    <td cssclass="normal">
                                        EPS Link:
                                    </td>
                                    <td cssclass="normal">
                                        <%# Eval("EPSLink")%>
                                    </td>
                                    <td>
                                        &nbsp; &nbsp; &nbsp; &nbsp;
                                    </td>
                                    <td cssclass="normal">
                                        PPC[Start]:
                                    </td>
                                    <td cssclass="normal">
                                        <%# Eval("PPC_Start")%>
                                    </td>
                                </tr>
                                <tr>
                                    <td cssclass="normal">
                                        Start Date:
                                    </td>
                                    <td cssclass="normal">
                                        <%# Eval("StartDate")%>
                                    </td>
                                    <td>
                                        &nbsp; &nbsp; &nbsp; &nbsp;
                                    </td>
                                    <td cssclass="normal">
                                        GIRO Link:
                                    </td>
                                    <td cssclass="normal">
                                        <%# Eval("GIROLink")%>
                                    </td>
                                    <td>
                                        &nbsp; &nbsp; &nbsp; &nbsp;
                                    </td>
                                    <td cssclass="normal">
                                        PPC[End]:
                                    </td>
                                    <td cssclass="normal">
                                        <%# Eval("GIROLink")%>
                                    </td>
                                </tr>
                                <tr>
                                    <td cssclass="normal">
                                        Last Transaction:
                                    </td>
                                    <td cssclass="normal">
                                        <%# Eval("LastTransaction")%>
                                    </td>
                                    <td>
                                        &nbsp; &nbsp; &nbsp; &nbsp;
                                    </td>
                                    <td cssclass="normal">
                                        Kinetics:
                                    </td>
                                    <td cssclass="normal">
                                        <%# Eval("Kinetics")%>
                                    </td>
                                    <td>
                                        &nbsp; &nbsp; &nbsp; &nbsp;
                                    </td>
                                    <td cssclass="normal">
                                        RDS:
                                    </td>
                                    <td cssclass="normal">
                                        <%# Eval("RDS")%>
                                    </td>
                                </tr>
                                <tr>
                                    <td cssclass="normal">
                                        Auth.3rd Party Name:
                                    </td>
                                    <td cssclass="normal">
                                        <%# Eval("AuthParty")%>
                                    </td>
                                    <td>
                                        &nbsp; &nbsp; &nbsp; &nbsp;
                                    </td>
                                    <td cssclass="normal">
                                        E-Consent:
                                    </td>
                                    <td cssclass="normal">
                                        <%# Eval("EConsent") %>
                                    </td>
                                    <td>
                                        &nbsp; &nbsp; &nbsp; &nbsp;
                                    </td>
                                    <td cssclass="normal">
                                        <%--<asp:Label CssClass="LabelUnderline" ID="lblMMF" runat="server" 
                                            Text="MMF" ToolTip="Available Funds Balance" onmouseover="MouseHover();">
                                        </asp:Label>--%>
                                        <asp:Label ID="Label2" runat="server" Text="MMF"></asp:Label>
                                    </td>
                                    <td cssclass="normal">
                                        <asp:Label ID="labMMF" runat="server" Text='<%# Eval("MMF") %>'></asp:Label>
                                        <asp:Label ID="labMMFValue" runat="server" Text='<%# Eval("MMFValue") %>'></asp:Label>
                                    </td>
                                    
                                </tr>
                                <tr>
                                    <td cssclass="normal">
                                        3rd Party IC No:
                                    </td>
                                    <td cssclass="normal">
                                        <%# Eval("AuthPartyIC")%>
                                    </td>
                                    <td>
                                        &nbsp; &nbsp; &nbsp; &nbsp;
                                    </td>
                                    <td cssclass="normal">
                                        Income Level:
                                    </td>
                                    <td cssclass="normal">
                                        <%# Eval("IncomeLevel")%>
                                    </td>
                                    <td>
                                        &nbsp; &nbsp; &nbsp; &nbsp;
                                    </td>
                                    <td cssclass="normal">
                                        Net Worth:
                                    </td>
                                    <td cssclass="normal">
                                        <%# Eval("networth")%>
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </asp:FormView>
                    <br />
                    <asp:Label ID="lblSeminarRegistration" runat="server" Text="Seminisr(s) Registered:"
                        Font-Bold="true" Font-Size="10" Visible="false"></asp:Label>
                    <asp:GridView ID="gvSeminar" CssClass="normal" AllowSorting="True" AutoGenerateColumns="false"
                        PagerSettings-Visible="false" AllowPaging="True" runat="server" Width="800px">
                        <HeaderStyle BackColor="#CDCDCD" />
                        <PagerSettings Visible="False"></PagerSettings>
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
                                <HeaderStyle BorderColor="#777777" CssClass="grayBorder" Width="50px" />
                                <ItemStyle BorderColor="#777777" CssClass="grayBorder" Width="50px" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="SeminarName" HeaderText="Seminar" HtmlEncode="false">
                                <HeaderStyle CssClass="grayBorder" />
                                <ItemStyle CssClass="grayBorder" Width="110px" />
                            </asp:BoundField>
                        </Columns>
                    </asp:GridView>
                    <br />
                    <br />
                    <asp:GridView ID="gvCommissionEarned" runat="server" CssClass="normal" AutoGenerateColumns="false"
                        OnPreRender="gvCommissionEarned_PreRender" OnRowDataBound="gvCommissionEarned_RowDataBound"
                        Width="800px">
                        <HeaderStyle BackColor="#CDCDCD" />
                        <PagerSettings Visible="False"></PagerSettings>
                        <RowStyle CssClass="normal" BackColor="White" />
                        <AlternatingRowStyle CssClass="normal" BackColor="#EEEEEE" />
                        <Columns>
                            <asp:BoundField HeaderText="" DataField="MonthYear">
                                <HeaderStyle CssClass="grayBorder" />
                                <ItemStyle CssClass="grayBorder" Width="100px" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="" DataField="RepeatGroupColumn">
                                <HeaderStyle CssClass="grayBorder" />
                                <ItemStyle CssClass="grayBorder" Width="100px" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Cash Trading" DataField="CA">
                                <HeaderStyle CssClass="grayBorder" />
                                <ItemStyle CssClass="grayBorder" Width="100px" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Cash Management" DataField="KC">
                                <HeaderStyle CssClass="grayBorder" />
                                <ItemStyle CssClass="grayBorder" Width="100px" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Phillip Margin" DataField="M">
                                <HeaderStyle CssClass="grayBorder" />
                                <ItemStyle CssClass="grayBorder" Width="100px" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Phillip Financial" DataField="PFN">
                                <HeaderStyle CssClass="grayBorder" />
                                <ItemStyle CssClass="grayBorder" Width="100px" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Custodian" DataField="CU">
                                <HeaderStyle CssClass="grayBorder" />
                                <ItemStyle CssClass="grayBorder" Width="100px" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="CFD" DataField="CFD">
                                <HeaderStyle CssClass="grayBorder" />
                                <ItemStyle CssClass="grayBorder" Width="100px" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Discretionary Account" DataField="S2">
                                <HeaderStyle CssClass="grayBorder" />
                                <ItemStyle CssClass="grayBorder" Width="100px" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Unit Trust Non-Wrap" DataField="UT">
                                <HeaderStyle CssClass="grayBorder" />
                                <ItemStyle CssClass="grayBorder" Width="100px" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Advisory Account" DataField="UTW">
                                <HeaderStyle CssClass="grayBorder" />
                                <ItemStyle CssClass="grayBorder" Width="100px" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Total" DataField="">
                                <HeaderStyle CssClass="grayBorder" />
                                <ItemStyle CssClass="grayBorder" Width="200px" />
                            </asp:BoundField>
                        </Columns>
                    </asp:GridView>
                    <br />
                </div>
                <div id="divContentEntryForm" runat="server">
                    <asp:HiddenField ID="hdfDealerCode" runat="server" />
                    <asp:HiddenField ID="hdfNRIC" runat="server" />
                    <asp:HiddenField ID="hdfAeCode" runat="server" />
                    <asp:HiddenField ID="hdfTeamCode" runat="server" />
                    <!--  div for the whole page to display scroll bar  -->
                    <!-- <div id="divContactEntry" style="height:260px;overflow:auto;">  -->
                    <table width="100%" style="vertical-align: text-top; text-align: left;">
                        <!--  style="height: 260px"  width="100%"-->
                        <tr>
                            <td valign="top">
                                <!-- style="width:20%" -->
                                <table border="0" bordercolor="#777777" cellspacing="1" cellpadding="1" class="smallGrey">
                                    <tr>
                                        <td>
                                            <asp:HiddenField ID="lblProjectID" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <b>Name</b>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblAccountName" runat="server" />
                                            
                                        </td>
                                        <td>
                                            &nbsp;&nbsp;
                                        </td>
                                        <td>
                                            <b>AccountType</b>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtAccountType" CssClass="normal" runat="server" Enabled="false"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <b>A/C No.<font color="#FF0000">*</font></b>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtAccountNo" CssClass="normal" runat="server" />
                                            <asp:Label ID="labError" runat="server" />
                                            <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click"
                                                Style="display: none" />
                                        </td>
                                        <td>
                                            &nbsp;&nbsp;
                                        </td>
                                        <td>
                                            <b>Key</b>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtKey" CssClass="normal" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <b>Phone<font color="#FF0000">*</font></b>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPhone" CssClass="normal" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;&nbsp;
                                        </td>
                                        <td>
                                            <b>Keep</b>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkKeep" Text="" Checked="true" Enabled="false" CssClass="normal"
                                                runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
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
                                        <td colspan="3">
                                            <asp:RadioButton ID="rbtnUnknown" Text="Unknown" GroupName="Sex" Checked="true" CssClass="normal"
                                                runat="server" />
                                            &nbsp;&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="100px">
                                            <asp:CheckBox ID="chkDealer" runat="server" Checked="false" AutoPostBack="true" /><b>Follow
                                                up by</b>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlFollowUpDealer" CssClass="normal" runat="server" Enabled="false"
                                                AutoPostBack="true" Height="22px" Width="125px" />
                                        </td>
                                        <td>
                                            &nbsp;&nbsp;
                                        </td>
                                        <td>
                                            <asp:Label ID="lblDate" runat="server" Text="Date" Font-Bold="true" Visible="false"></asp:Label>
                                            <%--<b>Date</b>--%>
                                        </td>
                                        <td>
                                            <ucc:CoolCalendar ID="calFollowUpDate" TimeRequired="false" DateTimeFormat="dd/MM/yyyy"
                                                DateTextRequired="false" DateTextFromValue="" runat="server" Visible="false" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <b>Content<font color="#FF0000">*</font></b>
                                            <br />
                                            <b>(max: 500)</b>
                                        </td>
                                        <td colspan="4">
                                            <asp:TextBox ID="txtContact" CssClass="normal" TextMode="MultiLine" MaxLength="500"
                                                Rows="2" Columns="50" runat="server" Height="80px" />
                                        </td>
                                    </tr>
                                    <%--<tr>
                                        <td>
                                            <b>Course/</b>  <br />
                                            <b>Remark</b> <br />
                                            <b>(max: 255)</b>
                                        </td>
                                        <td colspan="4">
                                            <asp:TextBox ID="txtCourse" CssClass="normal" TextMode="MultiLine" Rows="2" MaxLength="255" Columns="50" runat="server"/>                                        
                                        </td>                            
                                    </tr>--%>
                                    <tr style="height: 22px; display:none">
                                        <td>
                                            <b>Seminar</b>
                                        </td>
                                        <td colspan="4">
                                            <a href="http://www.poems.com.sg/index.php?option=com_content&view=article&id=203&Itemid=282&lang=en"
                                                target="_blank">Seminar Registrations</a>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <b>Preference</b>
                                        </td>
                                        <td colspan="4">
                                            <asp:DropDownList ID="ddlPreferenceOne" CssClass="normal" runat="server" />
                                            <asp:DropDownList ID="ddlPreferenceTwo" CssClass="normal" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <b>Rank</b>
                                        </td>
                                        <td colspan="4">
                                            <asp:DropDownList ID="ddlRank" CssClass="normal" runat="server">
                                            </asp:DropDownList>
                                            &nbsp;&nbsp;
                                            <asp:Button ID="btnEmail" Text="Resend" CssClass="normal" runat="server" OnClientClick="openwindow()"
                                                Width="70px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="5">
                                            <%= DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") %>
                                            &nbsp;&nbsp;
                                            <asp:Button ID="btnAddContact" Text="Add Record" CssClass="normal" runat="server"
                                                OnClick="btnAddContact_Click" />
                                            &nbsp;
                                            <asp:Button ID="btnUpdateContact" Text="Update" CssClass="normal" runat="server"
                                                OnClick="btnUpdateContact_Click" />
                                            &nbsp;
                                            <asp:Button ID="btnCancel" Text="Cancel" CssClass="normal" runat="server" OnClick="btnCancel_Click" />
                                            <asp:HiddenField ID="hdfModifyIndex" runat="server" />
                                            <asp:HiddenField ID="hdfRecId" runat="server" />
                                            <asp:HiddenField ID="hdfFollowUpRecordId" runat="server" />
                                            <div style="display: none">
                                                <asp:Button ID="btnShortKey" CssClass="normal" Visible="true" runat="server" Text="ShortKey"
                                                    OnClick="btnShortKey_Click" />
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td>
                                <!-- style="width:80%" -->
                                <div id="div2" class="normalGrey">
                                    Current Assignments</div>
                                <!-- For Contact Entry Admin -->
                                <div id="divAdminEntry" runat="server">
                                    <asp:RadioButton ID="rdobtnDealer" runat="server" Text="Dealer" CssClass="normal"
                                        GroupName="Group1" AutoPostBack="true" OnCheckedChanged="rdobtnDealer_CheckedChanged" />
                                    <asp:RadioButton ID="rdobtnProject" runat="server" Text="Project" CssClass="normal"
                                        GroupName="Group1" AutoPostBack="true" Checked="True" OnCheckedChanged="rdobtnProject_CheckedChanged" />
                                    <br />
                                    <br />
                                    <asp:Label ID="lblDealer" Text="Dealer :" CssClass="normal" runat="server" />
                                    &nbsp;
                                    <asp:DropDownList ID="ddlActualDealer" CssClass="normal" runat="server" AutoPostBack="true"
                                        OnSelectedIndexChanged="ddlActualDealer_SelectedIndexChanged" />
                                </div>
                                <br />
                                <br />
                                <div id="divAdminEntry2" runat="server">
                                    <asp:Label ID="Label1" Text="Project :" CssClass="normal" runat="server" />
                                    &nbsp;
                                    <asp:DropDownList ID="ddlProjectList" CssClass="normal" runat="server" AutoPostBack="true"
                                        Width="300px" Height="22px" OnSelectedIndexChanged="ddlProjectList_SelectedIndexChanged" />
                                    <br />
                                    <asp:Label ID="txtProjectObj" Text="Project Objective :" CssClass="normal" runat="server" />
                                    <asp:Label ID="lblObjective" CssClass="normal" runat="server" />
                                </div>
                                <div style="height: 200px; overflow: auto;">
                                    <!--  style="height:260px;overflow:auto;"  -->
                                    <asp:GridView ID="gvAssignments" runat="server" AllowPaging="true" AllowSorting="true"
                                        AutoGenerateColumns="false" CssClass="normal" OnRowCommand="gvAssignments_RowCommand"
                                        OnRowDataBound="gvAssignments_RowDataBound" OnSorting="gvAssignments_Sorting"
                                        PagerSettings-Visible="false" PageSize="10">
                                        <HeaderStyle BackColor="#CDCDCD" CssClass="Freezing" />
                                        <RowStyle BackColor="#FFCCFF" CssClass="grayBorder" />
                                        <Columns>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    No.
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <%# Container.DataItemIndex + 1 %>
                                                    <asp:HiddenField ID="gvhdfRecordId0" runat="server" Value="<%# Container.DataItemIndex %>" />
                                                </ItemTemplate>
                                                <ItemStyle CssClass="grayBorder" Width="15px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="A/C" SortExpression="AcctNo">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="gvlbtnAccountNo0" runat="server" CommandArgument="<%# Container.DataItemIndex %>"
                                                        CommandName="AcctNo" CssClass="normal" Font-Underline="True" ForeColor="Blue"
                                                        Text='<%# Bind("AcctNo") %>' ToolTip="Select Client" />
                                                    <asp:Label ID="labAECode" runat="server" Text='<%# Eval("AECode") %>' style="display: none"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle CssClass="grayBorder" Width="30px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Name" SortExpression="ClientName">
                                                <ItemTemplate>
                                                    <%# DataBinder.Eval(Container.DataItem, "ClientName")%>
                                                    <%-- <asp:LinkButton ID="gvlbtnClientName0" runat="server" 
                                                        CommandArgument="<%# Container.DataItemIndex %>" CommandName="ClientName" 
                                                        CssClass="normal" Font-Underline="True" ForeColor="Blue" 
                                                        Text='<%# Bind("ClientName") %>' ToolTip="View History" />--%>
                                                </ItemTemplate>
                                                <ItemStyle CssClass="grayBorder" Width="100px" />
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="Sex" HeaderText="Gender" SortExpression="Sex">
                                                <ItemStyle CssClass="grayBorder" Width="23px" />
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="Contact No.">
                                                <ItemTemplate>
                                                    [P]<%# DataBinder.Eval(Container.DataItem, "Phone") %>
                                                    &nbsp;&nbsp; [T]<%# DataBinder.Eval(Container.DataItem, "LTEL")%>
                                                    <br />
                                                    [M]<%# DataBinder.Eval(Container.DataItem, "LMOBILE")%>
                                                    &nbsp;&nbsp; [O]<%# DataBinder.Eval(Container.DataItem, "LOFFTEL")%>
                                                </ItemTemplate>
                                                <ItemStyle CssClass="grayBorder" Width="160px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Rank" SortExpression="Rank">
                                                <ItemTemplate>
                                                    <%# DataBinder.Eval(Container.DataItem, "Rank")%>
                                                </ItemTemplate>
                                                <ItemStyle CssClass="grayBorder" Width="23px" />
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="OverallLTD" HeaderText="Trade" SortExpression="OverallLTD">
                                                <ItemStyle CssClass="grayBorder" Width="55px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Market_vl" HeaderText="Stock Mkt" SortExpression="Market_vl">
                                                <ItemStyle CssClass="grayBorder" Width="20px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="out_Bal" HeaderText="Bal" SortExpression="out_Bal">
                                                <ItemStyle CssClass="grayBorder" Width="15px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="TotalCall" HeaderText="Total Call" SortExpression="TotalCall">
                                                <ItemStyle CssClass="grayBorder" Width="60px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="ContactDate" HeaderText="Contact">
                                                <ItemStyle CssClass="grayBorder" Width="80px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="AssignDate" DataFormatString="{0:dd/MM/yyyy}" HeaderText="Assign"
                                                HtmlEncode="false" SortExpression="AssignDate">
                                                <ItemStyle CssClass="grayBorder" Width="25px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="CutOffDate" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}"
                                                HeaderText="Cutoff Date" HtmlEncode="false" SortExpression="CutOffDate">
                                                <HeaderStyle CssClass="grayBorder" />
                                                <ItemStyle CssClass="grayBorder" Width="100px" />
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="TR/Core">
                                                <ItemTemplate>
                                                    <%# DataBinder.Eval(Container.DataItem, "DealerTeam")%>
                                                    <br />
                                                    <%# DataBinder.Eval(Container.DataItem, "CoreDealer")%>
                                                </ItemTemplate>
                                                <ItemStyle CssClass="grayBorder" Width="25px" />
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="ClientStatus" HeaderText="S">
                                                <ItemStyle CssClass="grayBorder" Width="20px" />
                                            </asp:BoundField>
                                            <%--<asp:BoundField DataField="AccServiceType" HeaderText="AccountType">
                                                <ItemStyle CssClass="grayBorder" Width="20px" />
                                            </asp:BoundField>--%>
                                            <asp:BoundField DataField="ProjectID" HeaderText="ProjectID" Visible="false">
                                                <ItemStyle CssClass="grayBorder" Width="20px" />
                                            </asp:BoundField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                                <br />
                                <div id="divAssignPaging" class="normal" runat="server">
                                    <ucc:PagingControl ID="pgcAssignment" OnPageNoChanged="AssignmentPaging_PageNoChange"
                                        OnRowPerPageChanged="AssignmentPaging_RowPerPageChanged" runat="server" />
                                </div>
                                <br />
                                <div id="divCurAssignmentText" class="normalGrey" runat="server">
                                    Current Follow-up</div>
                                <br />
                                <div style="height: 200px; overflow: auto;">
                                    <!--  style="height:260px;overflow:auto;"  -->
                                    <asp:GridView ID="gvContactFollowUp" runat="server" AllowPaging="true" AllowSorting="true"
                                        AutoGenerateColumns="false" CssClass="normal" OnRowCommand="gvContactFollowUp_RowCommand"
                                        OnRowDataBound="gvContactFollowUp_RowDataBound" OnSorting="gvContactFollowUp_Sorting"
                                        PagerSettings-Visible="false" PageSize="10">
                                        <HeaderStyle BackColor="#CDCDCD" CssClass="Freezing" />
                                        <RowStyle BackColor="#FFCCFF" CssClass="grayBorder" />
                                        <Columns>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    No.
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <%# Container.DataItemIndex + 1 %>
                                                    <asp:HiddenField ID="gvhdfRecordId0" runat="server" Value="<%# Container.DataItemIndex %>" />
                                                </ItemTemplate>
                                                <ItemStyle CssClass="grayBorder" Width="15px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="A/C" SortExpression="AcctNo">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="gvlbtnAccountNo0" runat="server" CommandArgument="<%# Container.DataItemIndex %>"
                                                        CommandName="AcctNo" CssClass="normal" Font-Underline="True" ForeColor="Blue"
                                                        Text='<%# Bind("AcctNo") %>' ToolTip="Select Client" />
                                                </ItemTemplate>
                                                <ItemStyle CssClass="grayBorder" Width="30px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Name" SortExpression="ClientName">
                                                <ItemTemplate>
                                                    <asp:Label ID="gvlabName" Text='<%# Bind("ClientName") %>' runat="server"></asp:Label>
                                                    <%--<asp:LinkButton ID="gvlbtnClientName0" runat="server" CommandArgument="<%# Container.DataItemIndex %>"
                                                        CommandName="ClientName" CssClass="normal" Font-Underline="True" ForeColor="Blue"
                                                        Text='<%# Bind("ClientName") %>' ToolTip="View History" />--%>
                                                </ItemTemplate>
                                                <ItemStyle CssClass="grayBorder" Width="100px" />
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="Sex" HeaderText="Gender" SortExpression="Sex">
                                                <ItemStyle CssClass="grayBorder" Width="23px" />
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="Contact No.">
                                                <ItemTemplate>
                                                    [P]<%# DataBinder.Eval(Container.DataItem, "Phone") %>
                                                    &nbsp;&nbsp; [T]<%# DataBinder.Eval(Container.DataItem, "LTEL")%>
                                                    <br />
                                                    [M]<%# DataBinder.Eval(Container.DataItem, "LMOBILE")%>
                                                    &nbsp;&nbsp; [O]<%# DataBinder.Eval(Container.DataItem, "LOFFTEL")%>
                                                </ItemTemplate>
                                                <ItemStyle CssClass="grayBorder" Width="160px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Rank" SortExpression="Rank">
                                                <ItemTemplate>
                                                    <%# DataBinder.Eval(Container.DataItem, "Rank")%>
                                                </ItemTemplate>
                                                <ItemStyle CssClass="grayBorder" Width="23px" />
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="OverallLTD" HeaderText="Trade" SortExpression="OverallLTD">
                                                <ItemStyle CssClass="grayBorder" Width="55px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Market_vl" HeaderText="Stock Mkt" SortExpression="Market_vl">
                                                <ItemStyle CssClass="grayBorder" Width="20px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="out_Bal" HeaderText="Bal" SortExpression="out_Bal">
                                                <ItemStyle CssClass="grayBorder" Width="15px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="TotalCall" HeaderText="Total Call" SortExpression="TotalCall">
                                                <ItemStyle CssClass="grayBorder" Width="60px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="ContactDate" HeaderText="Contact">
                                                <ItemStyle CssClass="grayBorder" Width="80px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="AssignDate" DataFormatString="{0:dd/MM/yyyy}" HeaderText="Assign"
                                                HtmlEncode="false" SortExpression="AssignDate">
                                                <ItemStyle CssClass="grayBorder" Width="25px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="CutOffDate" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}"
                                                HeaderText="Cutoff Date" HtmlEncode="false" SortExpression="CutOffDate">
                                                <HeaderStyle CssClass="grayBorder" />
                                                <ItemStyle CssClass="grayBorder" Width="100px" />
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="TR/Core">
                                                <ItemTemplate>
                                                    <%# DataBinder.Eval(Container.DataItem, "DealerTeam")%>
                                                    <br />
                                                    <%# DataBinder.Eval(Container.DataItem, "CoreDealer")%>
                                                </ItemTemplate>
                                                <ItemStyle CssClass="grayBorder" Width="25px" />
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="ClientStatus" HeaderText="S">
                                                <ItemStyle CssClass="grayBorder" Width="20px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="AccServiceType" HeaderText="AccountType">
                                                <ItemStyle CssClass="grayBorder" Width="20px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="ProjectID" HeaderText="ProjectID" Visible="false">
                                                <ItemStyle CssClass="grayBorder" Width="20px" />
                                            </asp:BoundField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                                <br />
                                <div id="divFollowUpPaging" class="normal" runat="server">
                                    <ucc:PagingControl ID="pgcContactFollowUp" OnPageNoChanged="pgcContactFollowUp_PageNoChange"
                                        OnRowPerPageChanged="pgcContactFollowUp_RowPerPageChanged" runat="server" />
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
                                <%--<asp:BoundField DataField="ContactDate" SortExpression="ContactDate" HeaderText="Contact Date" 
                                        dataformatstring="{0:dd/MM/yyyy HH:mm:ss}" HtmlEncode="false">
                                    <HeaderStyle CssClass="grayBorder" />
                                    <ItemStyle CssClass="grayBorder" Width="110px" />
                                </asp:BoundField>--%>
                                <%-- <asp:BoundField DataField="AEGroup" SortExpression="AEGroup" HeaderText="TR Code">
                                    <HeaderStyle CssClass="grayBorder" />
                                    <ItemStyle CssClass="grayBorder" Width="60px" />
                                </asp:BoundField>--%>
                                <asp:TemplateField SortExpression="CName" HeaderText="Name">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="gvlbtnClientName" CssClass="normal" Text='<%# Bind("CName") %>'
                                            CommandName="ClientName" CommandArgument="<%# Container.DataItemIndex %>" Font-Underline="True"
                                            ForeColor="Blue" runat="server" ToolTip="View History" />
                                        <asp:Label ID="gvlblClientName" CssClass="normal" Text='<%# Bind("CName") %>' runat="server" />
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="grayBorder" />
                                    <ItemStyle CssClass="grayBorder" Width="120px" />
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="AcctNo" HeaderText="Account Number">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="gvlbtnAccountNo" CssClass="normal" Text='<%# Bind("AcctNo") %>'
                                            CommandName="AcctNo" CommandArgument="<%# Container.DataItemIndex %>" runat="server"
                                            Font-Underline="True" ForeColor="Blue" ToolTip="Select Client" />
                                        <asp:Label ID="gvlblAccountNo" CssClass="normal" Text='<%# Bind("AcctNo") %>' runat="server" />
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="grayBorder" />
                                    <ItemStyle CssClass="grayBorder" Width="60px" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="AccServiceType" HeaderText="Account Type" Visible="false">
                                    <HeaderStyle CssClass="grayBorder" />
                                    <ItemStyle CssClass="grayBorder" Width="50px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Sex" SortExpression="Sex" HeaderText="Gender">
                                    <HeaderStyle CssClass="grayBorder" />
                                    <ItemStyle CssClass="grayBorder" Width="25px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Phone" HeaderText="Phone">
                                    <HeaderStyle CssClass="grayBorder" />
                                    <ItemStyle CssClass="grayBorder" Width="80px" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Preferences">
                                    <ItemTemplate>
                                        <asp:Label ID="gvlblPreferences" CssClass="normal" runat="server" />
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="grayBorder" />
                                    <ItemStyle CssClass="grayBorder" Width="110px" />
                                </asp:TemplateField>
                                <asp:BoundField SortExpression="RankText" DataField="RankText" HeaderText="Rank">
                                    <HeaderStyle CssClass="grayBorder" />
                                    <ItemStyle CssClass="grayBorder" Width="60px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Content" HeaderText="Content">
                                    <HeaderStyle CssClass="grayBorder" />
                                    <ItemStyle CssClass="grayBorder" Width="160px" />
                                </asp:BoundField>
                                <%--<asp:BoundField DataField="Remarks" HeaderText="Course/Remark">
                                    <HeaderStyle CssClass="grayBorder" />
                                    <ItemStyle CssClass="grayBorder" Width="160px" />
                                </asp:BoundField>--%>
                                <asp:BoundField HeaderText="Seminars" Visible="false">
                                    <HeaderStyle CssClass="grayBorder" />
                                    <ItemStyle CssClass="grayBorder" Width="160px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="FollowUpStatus" HeaderText="Follow-up">
                                    <HeaderStyle CssClass="grayBorder" />
                                    <ItemStyle CssClass="grayBorder" Width="60px" />
                                </asp:BoundField>
                                <%--<asp:TemplateField HeaderText="Follow-up">
                                    <ItemTemplate>
                                        <asp:Label ID="labFollowUp" runat="server" Text='<%# Eval("FollowUpStatus")="Y": %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle CssClass="grayBorder" Width="60px" />
                                </asp:TemplateField>--%>
                                <asp:BoundField DataField="FollowUpDate" HeaderText="Follow-up Date">
                                    <HeaderStyle CssClass="grayBorder" />
                                    <ItemStyle CssClass="grayBorder" Width="80px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="ModifiedUser" HeaderText="Dealer Code">
                                    <HeaderStyle CssClass="grayBorder" />
                                    <ItemStyle CssClass="grayBorder" Width="60px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="AECode" HeaderText="AE">
                                    <HeaderStyle CssClass="grayBorder" />
                                    <ItemStyle CssClass="grayBorder" Width="60px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="AltAECode" HeaderText="Alt AE">
                                    <HeaderStyle CssClass="grayBorder" />
                                    <ItemStyle CssClass="grayBorder" Width="60px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="ProjectID" HeaderText="ProjectID" Visible="false">
                                    <HeaderStyle CssClass="grayBorder" />
                                    <ItemStyle CssClass="grayBorder" Width="60px" />
                                </asp:BoundField>
                                <asp:TemplateField ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:Button ID="gvbtnEdit" runat="server" CausesValidation="False" CommandName="Edit"
                                            Text="Modify" OnClientClick="ScrollToTop()" />
                                        &nbsp;<asp:Button ID="gvbtnDelete" runat="server" CausesValidation="False" CommandName="Delete"
                                            Text="Delete" />
                                    </ItemTemplate>
                                    <ControlStyle CssClass="normal" />
                                    <HeaderStyle CssClass="grayBorder" />
                                    <ItemStyle CssClass="grayBorder" Width="120px" />
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
                </div>
            </div>
        <%--</ContentTemplate>
    </asp:UpdatePanel>--%>
    </form>
</body>
</html>

<script type="text/javascript">
    function doClick(buttonName, e) {//the purpose of this function is to allow the enter key to point to the correct button to click.
        var key;

        if (window.event)
            key = window.event.keyCode;     //IE
        else
            key = e.which;     //firefox

        if (key == 13) {
            //Get the button the user wants to have clicked
            var btn = document.getElementById(buttonName);
            if (btn != null) { //If we find the button click it
                btn.click();
                event.keyCode = 0
            }
        }
    }
</script>

