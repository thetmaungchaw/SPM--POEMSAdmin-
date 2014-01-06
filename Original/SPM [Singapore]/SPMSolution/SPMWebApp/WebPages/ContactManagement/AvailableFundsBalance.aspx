<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AvailableFundsBalance.aspx.cs" Inherits="SPMWebApp.WebPages.ContactManagement.AvailableFundsBalance" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>SPM - Available Funds Balance</title>
    <link rel="stylesheet" href="StyleSheet/SPMStyle.css" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div id="divMessage" runat="server" class="normalRed">
        </div>
        <div id="divAvailableFundsBalance" runat="server">        
        </div>
        <div id="divCashAndEquivanents" runat="server">
            <table>
                <tr>
                    <td>
                        <asp:Label ID="lblTital" runat="server" Text="CASH AND EQUIVALENTS"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblSingapore" runat="server" Text="Singapore"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:GridView ID="gvCashAndEquivalents" runat="server" ShowFooter="true"
                            AutoGenerateColumns="false" onrowdatabound="gvCashAndEquivalents_RowDataBound">
                            <HeaderStyle BackColor="#CDCDCD" />
                            <PagerSettings Visible="False"></PagerSettings>
                            <RowStyle CssClass="normal" BackColor="White"/>
                            <alternatingrowstyle CssClass="normal" BackColor="#EEEEEE"/>
                            <Columns>
                                <asp:BoundField DataField="name" HeaderText="Name" HtmlEncode="false">
                                    <HeaderStyle CssClass="grayBorder" />
                                    <ItemStyle CssClass="grayBorder" Width="110px" />
                                    <FooterStyle CssClass="grayBorder" Font-Bold="true" BackColor="#CDCDCD" />
                                </asp:BoundField>
                                 <asp:BoundField DataField="mode" HeaderText="Mode" HtmlEncode="false">
                                    <HeaderStyle CssClass="grayBorder" />
                                    <ItemStyle CssClass="grayBorder" Width="110px" />
                                    <FooterStyle CssClass="grayBorder" Font-Bold="true" BackColor="#CDCDCD" />
                                </asp:BoundField>
                                 <asp:BoundField DataField="currency" HeaderText="Currency" HtmlEncode="false">
                                    <HeaderStyle CssClass="grayBorder" />
                                    <ItemStyle CssClass="grayBorder" Width="110px" />
                                    <FooterStyle CssClass="grayBorder" Font-Bold="true" BackColor="#CDCDCD" />
                                </asp:BoundField>
                                 <asp:BoundField DataField="quantity" HeaderText="Qty" HtmlEncode="false">
                                    <HeaderStyle CssClass="grayBorder" />
                                    <ItemStyle CssClass="grayBorder" Width="110px" />
                                    <FooterStyle CssClass="grayBorder" Font-Bold="true" BackColor="#CDCDCD" />
                                </asp:BoundField>
                                 <asp:BoundField DataField="avgprice" HeaderText="Avg Price" HtmlEncode="false">
                                    <HeaderStyle CssClass="grayBorder" />
                                    <ItemStyle CssClass="grayBorder" Width="110px" />
                                    <FooterStyle CssClass="grayBorder" Font-Bold="true" BackColor="#CDCDCD" />
                                </asp:BoundField>
                                 <asp:BoundField DataField="closingprice" HeaderText="Closing Price" HtmlEncode="false" FooterText="Grand Total(SGD):">
                                    <HeaderStyle CssClass="grayBorder" />
                                    <ItemStyle CssClass="grayBorder" Width="110px" />
                                    <FooterStyle CssClass="grayBorder" Font-Bold="true" BackColor="#CDCDCD" />
                                </asp:BoundField>
                                 <asp:BoundField DataField="totalcost" HeaderText="Total Cost<br/>(SGD)" HtmlEncode="false">
                                    <HeaderStyle CssClass="grayBorder" />
                                    <ItemStyle CssClass="grayBorder" Width="110px" />
                                    <FooterStyle CssClass="grayBorder" Font-Bold="true" BackColor="#CDCDCD" />
                                </asp:BoundField>
                                 <asp:BoundField DataField="marketvalue" HeaderText="Market Value<br/>(SGD)" HtmlEncode="false">
                                    <HeaderStyle CssClass="grayBorder" />
                                    <ItemStyle CssClass="grayBorder" Width="110px" />
                                    <FooterStyle CssClass="grayBorder" Font-Bold="true" BackColor="#CDCDCD" />
                                </asp:BoundField>
                                 <asp:BoundField DataField="port" HeaderText="Port(%)" HtmlEncode="false">
                                    <HeaderStyle CssClass="grayBorder" />
                                    <ItemStyle CssClass="grayBorder" Width="110px" />
                                    <FooterStyle CssClass="grayBorder" Font-Bold="true" BackColor="#CDCDCD" />
                                </asp:BoundField>
                                 <asp:BoundField DataField="unrealisedpl" HeaderText="Un realised<br/>P/(L)" HtmlEncode="false">
                                    <HeaderStyle CssClass="grayBorder" />
                                    <ItemStyle CssClass="grayBorder" Width="130px" />
                                    <FooterStyle CssClass="grayBorder" Font-Bold="true" BackColor="#CDCDCD" />
                                </asp:BoundField>
                                <asp:BoundField DataField="profitloss" HeaderText="P/(L)(%)" HtmlEncode="false">
                                    <HeaderStyle CssClass="grayBorder" />
                                    <ItemStyle CssClass="grayBorder" Width="110px" />
                                    <FooterStyle CssClass="grayBorder" Font-Bold="true" BackColor="#CDCDCD" />
                                </asp:BoundField>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    </form>
</body>
</html>
