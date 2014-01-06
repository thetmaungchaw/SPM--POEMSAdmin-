<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SupervisorDashboard.aspx.cs" Inherits="SPMWebApp.WebPages.Dashboard.SupervisorDashboard" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="ajaxToolkit" %>
    
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>SPM - Supervisor Dashboard Module</title>
    <link rel="stylesheet" href="StyleSheet/SPMStyle.css" type="text/css" />  
    <style type="text/css">        
        .style1
        {
            width: 350px;
            text-align:left;
            vertical-align:top;
        }
        .style2
        {
            width: 400px;
            text-align:left;
            vertical-align:top;
        }
        .style3
        {
            width: 130px;
            text-align:left;
        }
    </style>
</head>
<body>   
    <div id="Container">
        <form id="frmSupervisorDashboard" runat="server">
            <asp:ScriptManager ID="ScriptManager1" runat="server">                
            </asp:ScriptManager>
            
            <asp:UpdatePanel ID="updatePanel_SupervisorDashboard" runat="server">            
            <ContentTemplate>            
                <div id="divMessage" class="normalRed" runat="server"></div>                
                <br />
                <br />
                <div id="divTitle" class="title" runat="server">Supervisor Dashboard Module</div>                
                <br />
                <div id="divWorkingPanel" runat="server">
                    <table align="left" class="normalGrey" width="100%">
                        <tr>
                            <td class="style1">
                                <!-- Dashboard Panel ->
                                <table align="left" class="normalGrey">
                                    <tr>
                                        <td>Project Name</td>
                                        <td>
                                            <asp:TextBox ID="txtProjectName" runat="server" Width="185px" 
                                                ontextchanged="txtProjectName_TextChanged"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Project Name</td>
                                        <td>
                                            <asp:DropDownList ID="ddlProjectName" runat="server" Width="185px" AutoPostBack="true"
                                                onselectedindexchanged="ddlProjectName_SelectedIndexChanged" >
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>                            
                                <br />
                                <br />
                                <br />
                                <br />
                                <div id="divNoProjects" class="normalRed" runat="server"></div>
                                <br />                                
                                <br />
                                <div id="divProjectDetail" runat="server">
                                    <table align="left" class="normalGrey">
                                        <tr>
                                            <td>Project Name: </td>
                                            <td>
                                                <asp:Label ID="lblProjectName" runat="server" ></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Contact Period: </td>
                                            <td>
                                                <asp:Label ID="lblContactPeriod" runat="server" ></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Total Commission: </td>
                                            <td>
                                                <asp:Label ID="lblTotalCommission" runat="server" ></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                    <br />
                                    <br />
                                    <br />
                                    <br />
                                    <br />
                                    <asp:GridView ID="gvCallsByDealer" runat="server" Width="100%" CssClass="normal" AutoGenerateColumns="false" ShowHeader="true">                                
                                        <AlternatingRowStyle BackColor="#FFFFCC" />
                                        <RowStyle BackColor="White" />
                                        <HeaderStyle BackColor="Silver" />
                                        <Columns>
                                            <asp:BoundField HeaderText="Name" DataField="AeName">
                                                <HeaderStyle CssClass="grayBorder" />                            
                                                <ItemStyle CssClass="grayBorder" />  
                                            </asp:BoundField>
                                            <asp:BoundField HeaderText="Number of Calls Left" DataField="NoOfCallsLeft">
                                                <HeaderStyle CssClass="grayBorder" />                            
                                                <ItemStyle CssClass="grayBorder" />  
                                            </asp:BoundField>
                                            <asp:BoundField HeaderText="Number of Calls to Follow-up" DataField="NoOfCallsFollowUp">
                                                <HeaderStyle CssClass="grayBorder" />                            
                                                <ItemStyle CssClass="grayBorder" />  
                                            </asp:BoundField>
                                        </Columns>
                                    </asp:GridView> 
                                </div>                                                                    
                            </td>
                            <td class="style2">                                
                                <!-- Bar Chart Panel ->                            
                                <asp:chart id="chart_CommissionEarned" runat="server" Width="400px" 
                                    Palette="Grayscale" BorderColor="181, 64, 1" BorderDashStyle="Solid" 
                                    BackGradientStyle="TopBottom" BorderWidth="2" backcolor="White" imagetype="Png" 
                                    ImageLocation="~\TempImages\ChartPic_#SEQ(300,3)" Height="255px">
							        <titles>
								        <asp:title ShadowColor="32, 0, 0, 0" Font="Trebuchet MS, 14.25pt, style=Bold" ShadowOffset="3" Text="Commission Earned For this Project" Alignment="TopLeft" ForeColor="26, 59, 105"></asp:title>
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
                        <tr>
                            <td colspan="2" align="left">
                                <div id="divCommEarned" runat="server">
                                <div id="divCommissionEarned" class="bold" runat="server">Commission Earned For this Project</div>
                                    <asp:GridView ID="gvCommissionEarned" runat="server" CssClass="normal"
                                                    AutoGenerateColumns="false" Width="850px" 
                                                    ShowHeader="true" onrowdatabound="gvCommissionEarned_RowDataBound">
                                        <AlternatingRowStyle BackColor="#FFFFCC" />
                                        <RowStyle BackColor="White" />
                                        <HeaderStyle BackColor="Silver" />
                                        <Columns>
                                            <asp:BoundField HeaderText="" DataField="Title">
                                                <HeaderStyle CssClass="grayBorder" />                            
                                                <ItemStyle CssClass="grayBorder" Width="100px" />  
                                            </asp:BoundField>
                                            <asp:BoundField HeaderText="Cash<br/>Trading<br/>(CA)" DataField="CashTrading" HtmlEncode="false">
                                                <HeaderStyle CssClass="grayBorder" />                            
                                                <ItemStyle CssClass="grayBorder" Width="150px" />  
                                            </asp:BoundField>
                                            <asp:BoundField HeaderText="Cash<br/>Management<br/>(KC)" DataField="CashManagement" HtmlEncode="false">
                                                <HeaderStyle CssClass="grayBorder" />                            
                                                <ItemStyle CssClass="grayBorder" Width="200px" />
                                            </asp:BoundField>
                                            <asp:BoundField HeaderText="Phillip<br/>Margin<br/>(M)" DataField="PhillipMargin" HtmlEncode="false">
                                                <HeaderStyle CssClass="grayBorder" />                            
                                                <ItemStyle CssClass="grayBorder" Width="150px" />  
                                            </asp:BoundField>
                                            <asp:BoundField HeaderText="Phillip<br/>Financial<br/>(PFN)" DataField="PhillipFinancial" HtmlEncode="false">
                                                <HeaderStyle CssClass="grayBorder" />                            
                                                <ItemStyle CssClass="grayBorder" Width="200px" />  
                                            </asp:BoundField>
                                            <asp:BoundField HeaderText="Custodian<br/>(CU)" DataField="Custodian" HtmlEncode="false">
                                                <HeaderStyle CssClass="grayBorder" />                            
                                                <ItemStyle CssClass="grayBorder" Width="150px" />  
                                            </asp:BoundField>
                                            <asp:BoundField HeaderText="CFD<br/>(CFD)" DataField="CFD" HtmlEncode="false">
                                                <HeaderStyle CssClass="grayBorder" />                            
                                                <ItemStyle CssClass="grayBorder" Width="200px" />  
                                            </asp:BoundField>
                                            <asp:BoundField HeaderText="Discretionary<br/>Account<br/>(S2)" DataField="DiscretionaryAccounts" HtmlEncode="false">
                                                <HeaderStyle CssClass="grayBorder" />                            
                                                <ItemStyle CssClass="grayBorder" Width="200px" />  
                                            </asp:BoundField>
                                            <asp:BoundField HeaderText="Unit Trust<br/>Non-Wrap<br/>(UT)" DataField="UnitTrustNonWrap" HtmlEncode="false">
                                                <HeaderStyle CssClass="grayBorder" />                            
                                                <ItemStyle CssClass="grayBorder" Width="200px" />
                                            </asp:BoundField>                                            
                                            <asp:BoundField HeaderText="Advisory<br/>Account<br/>(UTW)" DataField="AdvisoryAccounts" HtmlEncode="false">
                                                <HeaderStyle CssClass="grayBorder" />                            
                                                <ItemStyle CssClass="grayBorder" Width="150px" />  
                                            </asp:BoundField>                                            
                                            <asp:BoundField HeaderText="Total" DataField="">
                                                <HeaderStyle CssClass="grayBorder" />                            
                                                <ItemStyle CssClass="grayBorder" Width="200px" />  
                                            </asp:BoundField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </td> 
                        </tr>
                        <tr>
                            <td colspan="2" align="center">
                                <hr />                                
                                    <asp:DataList ID="dlFunctionUrl" runat="server" Width="100%">
                                        <HeaderTemplate>                                        
                                            <% Session["count"] = 1; %>
	                                        <table 
	                                            width="1500px" border="0" bordercolor="#777777"
	                                            cellspacing="0" cellpadding="0" class="normalGrey">
	                                        <tr>
                                        </HeaderTemplate>
                                        <ItemTemplate>  
                                            
                                        <%  
                                            int count = Convert.ToInt32(Session["count"]);
                                            if (count % 8 != 0)
                                            { %>
                                            <td class="style3">
                                                |<asp:HyperLink ID="rpthlFunction" Text='<%#DataBinder.Eval(Container, "DataItem.Function_Desc")%>' 
                                                   NavigateUrl='<%# DataBinder.Eval(Container, "DataItem.URL")%>' runat="server" CssClass="menuSmall" />&nbsp;&nbsp;
                                            </td>
                                            
                                            <% }
                                            else
                                            { %>                                        
                                                <td class="style3">
                                                    |<asp:HyperLink ID="HyperLink1" Text='<%#DataBinder.Eval(Container, "DataItem.Function_Desc")%>' 
                                                       NavigateUrl='<%# DataBinder.Eval(Container, "DataItem.URL")%>' runat="server" CssClass="menuSmall" />&nbsp;&nbsp;
                                                </td>
                                                </tr><tr>
                                            <%}
                                            Session["count"] = count + 1;
                                            %>
                                            
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            </tr>
                                                </table>
                                            <% Session["count"] = 0; %>
                                        </FooterTemplate>
                                    </asp:DataList>                            
                            </td>
                        </tr>
                    </table>                    
                </div>                                
            </ContentTemplate>
            </asp:UpdatePanel>
        </form>
    </div>
</body>
</html>
