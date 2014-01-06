<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CoolCalendar.ascx.cs" Inherits="SPMWebApp.CoolCalendar" %>

<asp:TextBox ID="DateTextFrom" Text="" runat="server" CssClass="normal" autocomplete="off" />
    <asp:RequiredFieldValidator ID="DateTextFromRequired" Enabled=true runat="server"
        ControlToValidate="DateTextFrom" ForeColor="Red" Font-Bold="true" ErrorMessage="*"></asp:RequiredFieldValidator>
       <asp:Panel ID="Panel1" Visible="true" runat="server" CssClass="popupControl" style="display:none;">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                   <center>
        <table width="140px">
            <tr>
                <td align="left" bgcolor="#cccccc">                    
                    <asp:DropDownList id="drpCalMonth" Runat="Server" AutoPostBack="True" OnSelectedIndexChanged="Set_Calendar" 
                        cssClass="normal"></asp:DropDownList>
                </td>
                <td align="left" bgcolor="#cccccc">
                    <asp:DropDownList id="drpCalYear" Runat="Server" AutoPostBack="True" OnSelectedIndexChanged="Set_Calendar" 
                        cssClass="normal"></asp:DropDownList>
                </td>
            </tr>
       	    <tr>
                <td colspan="2">
                        <asp:Calendar ID="Calendar1" runat="server" Width="150px" DayNameFormat="Shortest" CssClass="normal"
                            BackColor="White" BorderColor="#999999" CellPadding="1" Font-Names="Verdana" NextMonthText="" PrevMonthText=""
                            Font-Size="8pt" ForeColor="Black" OnSelectionChanged="Calendar1_SelectionChanged">
                                <SelectedDayStyle BackColor="#666666" Font-Bold="True" ForeColor="White" />
                                <TodayDayStyle BackColor="#CCCCCC" ForeColor="Black" />
                                <SelectorStyle BackColor="#CCCCCC" />
                                <WeekendDayStyle BackColor="#FFFFCC" />
                                <OtherMonthDayStyle ForeColor="#808080" />
                                <NextPrevStyle VerticalAlign="Bottom" />
                                <DayHeaderStyle BackColor="#CCCCCC" Font-Bold="True" Font-Size="7pt" />
                                <TitleStyle BackColor="#999999" Font-Size="7pt" BorderColor="Black" Font-Bold="True" />
                        </asp:Calendar>
            	</td>
            </tr>
        </table>
                    </center>
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:Panel>
        <ajaxToolkit:PopupControlExtender ID="PopupControlExtender1" runat="server"
            TargetControlID="DateTextFrom"
            PopupControlID="Panel1"
            Position="Bottom" />