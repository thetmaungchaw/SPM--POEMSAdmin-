<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CommissionEarnedChart.aspx.cs" Inherits="SPMWebApp.WebPages.AssignmentManagement.CommissionEarnedChart" %>
<%@ Register Assembly="System.Web.DataVisualization, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>SPM - Commission Earned by the Project</title>
</head>
<body>
    <form id="form1" runat="server">
    <div style="text-align:center; vertical-align:middle;">
    <div id="divMessage" class="normalRed" runat="server"></div>
    <table>
        <tr>
            <td>
                <asp:GridView ID="gvCommissionEarned" runat="server" CssClass="normal"
                        AutoGenerateColumns="false" Caption="<b>Commission Earned for the Project</b>"
                        ShowHeader="true" onrowdatabound="gvCommissionEarned_RowDataBound">
                    <AlternatingRowStyle BackColor="#FFFFCC" />
                    <RowStyle BackColor="White" />
                    <HeaderStyle BackColor="Silver" />
                    <Columns>
                        <asp:BoundField HeaderText="" DataField="Title">
                            <HeaderStyle CssClass="grayBorder" />                            
                            <ItemStyle CssClass="grayBorder" Width="100px" />  
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Cash Trading(CA)" DataField="CashTrading">
                            <HeaderStyle CssClass="grayBorder" />                            
                            <ItemStyle CssClass="grayBorder" Width="150px" />  
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Cash Management(KC)" DataField="CashManagement">
                            <HeaderStyle CssClass="grayBorder" />                            
                            <ItemStyle CssClass="grayBorder" Width="200px" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Phillip Margin(M)" DataField="PhillipMargin">
                            <HeaderStyle CssClass="grayBorder" />                            
                            <ItemStyle CssClass="grayBorder" Width="150px" />  
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Phillip Financial(PFN)" DataField="PhillipFinancial">
                            <HeaderStyle CssClass="grayBorder" />                            
                            <ItemStyle CssClass="grayBorder" Width="200px" />  
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Custodian(CU)" DataField="Custodian">
                            <HeaderStyle CssClass="grayBorder" />                            
                            <ItemStyle CssClass="grayBorder" Width="150px" />  
                        </asp:BoundField>
                        <asp:BoundField HeaderText="CFD(CFD)" DataField="CFD">
                            <HeaderStyle CssClass="grayBorder" />                            
                            <ItemStyle CssClass="grayBorder" Width="200px" />  
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Unit Trust Wrap(UTW)" DataField="AdvisoryAccounts">
                            <HeaderStyle CssClass="grayBorder" />                            
                            <ItemStyle CssClass="grayBorder" Width="150px" />  
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Unit Trust Non-Wrap(UT)" DataField="UnitTrustNonWrap">
                            <HeaderStyle CssClass="grayBorder" />                            
                            <ItemStyle CssClass="grayBorder" Width="200px" />  
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Managed Account(S2)" DataField="DiscretionaryAccounts">
                            <HeaderStyle CssClass="grayBorder" />                            
                            <ItemStyle CssClass="grayBorder" Width="200px" />  
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Total" DataField="">
                            <HeaderStyle CssClass="grayBorder" />                            
                            <ItemStyle CssClass="grayBorder" Width="200px" />  
                        </asp:BoundField>
                    </Columns>
                </asp:GridView>
            </td>
            <td>
                <asp:chart id="chart_CommissionEarned" runat="server" Width="400px"
                    Palette="Grayscale" BorderColor="181, 64, 1" BorderDashStyle="Solid" 
                    BackGradientStyle="TopBottom" BorderWidth="2" backcolor="White" imagetype="Png" 
                    ImageLocation="~\TempImages\ChartPic_#SEQ(300,3)" Height="300px">
	                <titles>
		                <asp:title ShadowColor="32, 0, 0, 0" Font="Trebuchet MS, 14.25pt, style=Bold" ShadowOffset="3" Text="Commission Earned by the Project" Alignment="TopLeft" ForeColor="26, 59, 105"></asp:title>
	                </titles>
	                <legends>
		                <asp:legend Enabled="False" IsTextAutoFit="True" Name="Default" BackColor="Transparent" Font="Trebuchet MS, 8.25pt, style=Bold"></asp:legend>
	                </legends>
	                <borderskin skinstyle="Emboss"></borderskin>
	                <series>
		                <asp:series ChartArea="chartArea_CommissionEarned" Name="chartSeries_CommissionEarned" BorderColor="180, 26, 59, 105" Color="224, 64, 10"></asp:series>
	                </series>
	                <chartareas>
		                <asp:chartarea Name="chartArea_CommissionEarned" 
		                            BorderColor="64, 64, 64, 64" BorderDashStyle="Solid" BackSecondaryColor="White" 
		                            BackColor="White" ShadowColor="Transparent" BackGradientStyle="TopBottom">
			                <area3dstyle Rotation="10" perspective="10" Inclination="15" IsRightAngleAxes="False" wallwidth="0" IsClustered="False"></area3dstyle>									    
			                <axisy linecolor="64, 64, 64, 64" IsLabelAutoFit="False" TitleAlignment="Near">
				                <labelstyle font="Trebuchet MS, 8.25pt, style=Bold" />
				                <majorgrid linecolor="64, 64, 64, 64" />
			                </axisy>
			                <axisx linecolor="64, 64, 64, 64" IsLabelAutoFit="False">
				                <labelstyle font="Trebuchet MS, 8.25pt, style=Bold" IsStaggered="True" />
				                <majorgrid linecolor="64, 64, 64, 64" />
			                </axisx>
		                </asp:chartarea>
	                </chartareas>
                </asp:chart>
            </td>
        </tr>
    </table>
        
</div>
    </form>
</body>
</html>
