<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="SPMWebApp.WebPages.Dashboard.Dashboard" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>SPM - Dashboard Module</title>
    <link rel="stylesheet" href="StyleSheet/SPMStyle.css" type="text/css" />  
    <style type="text/css">        
        .style3
        {
            width: 130px;
            text-align:left;
        }
        .style4
        {
            width: 541px;
        }
    </style>    
</head>
<body>
    <div id="Container">
        <form id="frmSupervisorDashboard" runat="server">
            <asp:ScriptManager ID="ScriptManager1" runat="server">                
            </asp:ScriptManager>
            
            <asp:UpdatePanel ID="updatePanel_Dashboard" runat="server">            
            <ContentTemplate>            
                <div id="divMessage" class="normalRed" runat="server"></div>
                <br />
                <br />
                <div id="divTitle" class="title" runat="server">Dashboard Module</div>                
                <br />
                <div id="divWorkingPanel" runat="server">
                    <table align="left" style="vertical-align:top" class="normalGrey">
                        <table width="1200px">
                            <tr>
                                <td class="style4">
                                    <div id="divAssignedClient" class="normalRed" runat="server"></div>
                                    <asp:GridView Width="600px" ID="gvAssignedClient" runat="server" CssClass="normal" 
                                        Caption="<b>Assigned Clients</b>" AutoGenerateColumns="false" 
                                        onrowdatabound="gvAssignedClient_RowDataBound" >
                                        <AlternatingRowStyle BackColor="#FFFFCC" />
                                        <RowStyle BackColor="White" />
                                        <HeaderStyle BackColor="Silver" />
                                        <Columns>
                                            <asp:BoundField HeaderText="Project Name" DataField="ProjectName">
                                                <HeaderStyle CssClass="grayBorder" />                            
                                                <ItemStyle CssClass="grayBorder" Width="100px" />  
                                            </asp:BoundField>
                                            <asp:BoundField HeaderText="Contact Period">
                                                <HeaderStyle CssClass="grayBorder" />                            
                                                <ItemStyle CssClass="grayBorder" Width="150px" />  
                                            </asp:BoundField>
                                            <asp:TemplateField>
                                                <HeaderStyle CssClass="grayBorder" />
                                                <ItemStyle CssClass="grayBorder" Width="80px" />                                                
                                                <HeaderTemplate> 
                                                    <asp:HyperLink ID="hyperlink_CallsLeft" runat="server" Text="Number of Calls Left"
                                                        NavigateUrl="~/WebPages/ContactManagement/ContactEntry.aspx"></asp:HyperLink>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <%# Eval("CallsLeft")%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%--<asp:BoundField HeaderText="Number of Calls Left" DataField="CallsLeft">                                                
                                                <HeaderStyle CssClass="grayBorder" />                            
                                                <ItemStyle CssClass="grayBorder" Width="200px" />  
                                            </asp:BoundField>--%>
                                            <asp:BoundField HeaderText="Number of Calls to Follow-up" DataField="CallsToFollowUp">
                                                <HeaderStyle CssClass="grayBorder" />                            
                                                <ItemStyle CssClass="grayBorder" Width="200px" />  
                                            </asp:BoundField>
                                            <asp:BoundField HeaderText="Follow-up Date" dataformatstring="{0:dd/MM/yyyy HH:mm:ss}" DataField="FollowUpDate">
                                                <HeaderStyle CssClass="grayBorder" />                            
                                                <ItemStyle CssClass="grayBorder" Width="200px" />  
                                            </asp:BoundField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                                <td>
                                    <div id="divLeads" class="normalRed" runat="server"></div>
                                    <asp:GridView Width="600px" ID="gvLeads" runat="server" CssClass="normal" 
                                        Caption="<b>Leads</b>" AutoGenerateColumns = "false" 
                                        onrowdatabound="gvLeads_RowDataBound" >
                                        <AlternatingRowStyle BackColor="#FFFFCC" />
                                        <RowStyle BackColor="White" />
                                        <HeaderStyle BackColor="Silver" />
                                         <Columns>
                                            <asp:BoundField HeaderText="Project Name" DataField="ProjectName">
                                                <HeaderStyle CssClass="grayBorder" />                            
                                                <ItemStyle CssClass="grayBorder" Width="100px" />  
                                            </asp:BoundField>
                                            <asp:BoundField HeaderText="Contact Period">
                                                <HeaderStyle CssClass="grayBorder" />                            
                                                <ItemStyle CssClass="grayBorder" Width="150px" />  
                                            </asp:BoundField>
                                            <asp:TemplateField>
                                                <HeaderStyle CssClass="grayBorder" />
                                                <ItemStyle CssClass="grayBorder" Width="80px" />                                                
                                                <HeaderTemplate>                                                    
                                                    <asp:HyperLink ID="hyperlink_CallsLeft" runat="server" Text="Number of Calls Left"
                                                        NavigateUrl="~/WebPages/LeadManagement/LeadContactEntry.aspx"></asp:HyperLink>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <%# Eval("CallsLeft")%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%--<asp:BoundField HeaderText="Number of Calls Left" DataField="CallsLeft">
                                                <HeaderStyle CssClass="grayBorder" />                            
                                                <ItemStyle CssClass="grayBorder" Width="200px" />  
                                            </asp:BoundField>--%>
                                            <asp:BoundField HeaderText="Number of Calls to Follow-up" DataField="CallsToFollowUp">
                                                <HeaderStyle CssClass="grayBorder" />                            
                                                <ItemStyle CssClass="grayBorder" Width="200px" />  
                                            </asp:BoundField>
                                            <asp:BoundField HeaderText="Follow-up Date" dataformatstring="{0:dd/MM/yyyy HH:mm:ss}" DataField="FollowUpDate">
                                                <HeaderStyle CssClass="grayBorder" />                            
                                                <ItemStyle CssClass="grayBorder" Width="200px" />  
                                            </asp:BoundField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td class="style4">
                                    <div id="divCommission" class="normalRed" runat="server"></div>
                                    <asp:GridView Width="600px" ID="gvCommission" runat="server" CssClass="normal" 
                                        Caption="<b>Commission</b>" AutoGenerateColumns="false" ShowFooter="true" 
                                        onrowdatabound="gvCommission_RowDataBound">
                                        <AlternatingRowStyle BackColor="#FFFFCC" />
                                        <RowStyle BackColor="White" />
                                        <HeaderStyle BackColor="Silver" />
                                        <Columns>
                                            <asp:BoundField HeaderText="Project Name" DataField="ProjectName" FooterText="Total">
                                                <HeaderStyle CssClass="grayBorder" />                
                                                <ItemStyle CssClass="grayBorder" Width="200px" />
                                                <FooterStyle CssClass="grayBorder" Font-Bold="true" BackColor="#CDCDCD" />
                                            </asp:BoundField>
                                            <asp:BoundField HeaderText="Individual" DataField="TotalIndividualComm" >
                                                <HeaderStyle CssClass="grayBorder" />                
                                                <ItemStyle CssClass="grayBorder" Width="200px" />
                                                <FooterStyle CssClass="grayBorder" Font-Bold="true" BackColor="#CDCDCD" />
                                            </asp:BoundField>
                                            <asp:BoundField HeaderText="Team" DataField="TotalTeamComm" >
                                                <HeaderStyle CssClass="grayBorder" />                
                                                <ItemStyle CssClass="grayBorder" Width="200px" />
                                                <FooterStyle CssClass="grayBorder" Font-Bold="true" BackColor="#CDCDCD" />
                                            </asp:BoundField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                                <td>
                                    
                                </td>
                            </tr>
                        </table>
                        <tr>
                            <td colspan="2" align="center">
                                <hr />
                                <asp:DataList ID="dlFunctionUrl" runat="server">
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
