<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ClientAnalysis.aspx.cs" Inherits="SPMWebApp.WebPages.ContactManagement.ClientAnalysis" %>

<%@ Register Src="~/CoolCalendar.ascx" TagName="CoolCalendar" TagPrefix="ucc" %>
<%@ Register
    Assembly="AjaxControlToolkit"
    Namespace="AjaxControlToolkit"
    TagPrefix="ajaxToolkit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>SPM - Client Analysis</title>    
    <link rel="stylesheet" href="<%=ConfigurationManager.AppSettings["SPMCssFile"]%>" type="text/css" />
</head>
<body>
    <div id="Container">
    <form id="frmClientAnalysis" runat="server">    
        <div class="title">Client Analysis</div>
        <br />
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        
        <script type="text/javascript" language="javascript">
            <!--
            var prm = Sys.WebForms.PageRequestManager.getInstance();
     
            prm.add_initializeRequest(InitializeRequest);
            prm.add_endRequest(EndRequest);

            function InitializeRequest(sender, args) 
            {                            
                // Get a reference to the element that raised the postback to disables it.
                var postBackControl = $get(args._postBackElement.id);                            
                
                $get('Container').style.cursor = 'wait';                
                if(postBackControl != null)
                {                    
                    postBackControl.disabled = true;      
                }                                                             
            }

            function EndRequest(sender, args) 
            {                
                // Get a reference to the element that raised the postback which is completing to enable it.
                var postBackControl = $get(sender._postBackSettings.sourceElement.id);
                
                $get('Container').style.cursor = 'auto';                
                if(postBackControl != null)
                {                    
                    postBackControl.disabled = false;      
                }
            }
            // -->        
        </script>
        
        
        <asp:UpdatePanel ID="uplClientAnalysis" runat="server">
            <Triggers>           
                <asp:PostBackTrigger ControlID="btnExcel" />        
            </Triggers>
            <ContentTemplate>
                <table>
                    <tr class="normal">                        
                        <td>
                            Team: &nbsp; <asp:DropDownList ID="ddlTeam" CssClass="normal" runat="server" />
                        </td>
                        <td>
                           A/C Open Date: &nbsp; <ucc:CoolCalendar ID="calAcctOpenDate" DateTextRequired="false" TimeRequired="false" DateTimeFormat="dd/MM/yyyy" 
                                                DateTextFromValue="" runat="server" />
                        </td>
                        <td>
                            Last Trade Date: &nbsp; <ucc:CoolCalendar ID="calLastTradeDate" TimeRequired="false" DateTextRequired="false" DateTimeFormat="dd/MM/yyyy" 
                                DateTextFromValue="" runat="server" />
                        </td>
                        <td>
                             <asp:Button ID="btnSearchClient" Text="Search" CssClass="normal" runat="server" 
                                onclick="btnSearchClient_Click" /> &nbsp;
                            <asp:Button ID="btnExcel" Text="Excel" CssClass="normal" onclick="btnExcel_Click" runat="server" />
                        </td>           
                    </tr>
                </table>
                <div id="divMessage" class="normalRed" runat="server"></div> <br />
                <asp:Repeater ID="rptClientAnalysis" runat="server" onitemdatabound="rptClientAnalysis_ItemDataBound">                    
                    <HeaderTemplate>
                        <table width="100%" class="normalGrey" border="1" bordercolor="#777777">
                            <tr bgcolor="#CDCDCD" align="center">
						        <td rowspan="3">No.</td>
						        <td rowspan="3">Team</td>
						        <td rowspan="3"><span onMouseOver="this.style.cursor='pointer';this.style.color='#226666';" onMouseOut="this.style.color='#000000';" onclick="SortAct('Act')">Active Client</span></td>
						        <td colspan="4">Inactive Client</td>						        
						        <td rowspan="3"><span onMouseOver="this.style.cursor='pointer';this.style.color='#226666';" onMouseOut="this.style.color='#000000';" onclick="SortAct('3')">2N Client</span></td>
						        <td rowspan="3"><span onMouseOver="this.style.cursor='pointer';this.style.color='#226666';" onMouseOut="this.style.color='#000000';" onclick="SortAct('Total')">Total Client</span></td>
					        </tr>
					        <tr bgcolor="#CDCDCD" align="center">
						        <td colspan="2">Last Trade After <asp:Label ID="lblLastTradeAfter" CssClass="normalGrey" runat="server"/> </td>
						        <td colspan="2">Last Trade Before <asp:Label ID="lblLastTradeBefore" CssClass="normalGrey" runat="server"/></td>
					        </tr>
					        <tr bgcolor="#CDCDCD" align="center">
						        <td>A/C Open After <asp:Label ID="lblLTAfterACAfter" CssClass="normalGrey" runat="server"/> </td>
						        <td>A/C Open Before <asp:Label ID="lblLTAfterACBefore" CssClass="normalGrey" runat="server"/> </td>
						        <td>A/C Open After <asp:Label ID="lblLTBeforeACAfter" CssClass="normalGrey" runat="server"/> </td>
						        <td>A/C Open Before <asp:Label ID="lblLTBeforeACBefore" CssClass="normalGrey" runat="server"/> </td>
					        </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr align="center">
                            <td><%# Container.ItemIndex + 1 %></td>
					        <td>
					            <%#DataBinder.Eval(Container, "DataItem.AECode")%>
					        </td>
					        <td bgcolor="#FFCCCC">
					            <asp:Label ID="rptlblTotAct" Text='<%#DataBinder.Eval(Container, "DataItem.TotAct")%>' CssClass="normal" 
					                runat="server" />
					        </td>
					        <td bgcolor="#CCFF99">
					            <asp:Label ID="rptlblTotInAfterAOAfter" Text='<%#DataBinder.Eval(Container, "DataItem.TotInAfterAOAfter")%>' 
					                CssClass="normal" runat="server" />
					        </td>
					        <td bgcolor="#99FF99">
					            <asp:Label ID="rptlblTotInAfterAOBefore" Text='<%#DataBinder.Eval(Container, "DataItem.TotInAfterAOBefore")%>' 
					                CssClass="normal" runat="server" />
					        </td>
					        <td bgcolor="#FFFFCC">
					            <asp:Label ID="rptlblTotInBeforeAOAfter" Text='<%#DataBinder.Eval(Container, "DataItem.TotInBeforeAOAfter")%>' 
					                CssClass="normal" runat="server" />
					        </td>
					        <td bgcolor="#FFFF99">
					            <asp:Label ID="rptlblTotInBeforeAOBefore" Text='<%#DataBinder.Eval(Container, "DataItem.TotInBeforeAOBefore")%>' 
					                CssClass="normal" runat="server" />
					        </td>
					        <td>
					            <asp:Label ID="rptlblTot3None" Text='<%#DataBinder.Eval(Container, "DataItem.Tot3None")%>' 
					                CssClass="normal" runat="server" />
					        </td>
					        <td>
					            <asp:Label ID="rptlblTotalClient" Text="" CssClass="normal" runat="server" />
					        </td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>                        
                            <tr bgcolor="#FFCC66" class="normal" align="center">
					            <td colspan="2">Total:</td>
					            <td>
					                <asp:Label ID="rptftlblTotAct" Text="" CssClass="normal" runat="server" />
					            </td>
					            <td>
					                <asp:Label ID="rptftlblTotInAfterAOAfter" Text="" CssClass="normal" runat="server" />    
					            </td>
					            <td>
					                <asp:Label ID="rptftlblTotInAfterAOBefore" Text="" CssClass="normal" runat="server" />
					            </td>
					            <td>
					                <asp:Label ID="rptftlblTotInBeforeAOAfter" Text="" CssClass="normal" runat="server" />
					            </td>
					            <td>
					                <asp:Label ID="rptftlblTotInBeforeAOBefore" Text="" CssClass="normal" runat="server" />
					            </td>
					            <td>
					                <asp:Label ID="rptftlblTot3None" Text="" CssClass="normal" runat="server" />
					            </td>
					            <td>
					                <asp:Label ID="rptftlblTotalClient" Text="" CssClass="normal" runat="server" />
					            </td>
				            </tr>
                        </table>
                    </FooterTemplate>                    
                </asp:Repeater>
                <br />
            </ContentTemplate>
        </asp:UpdatePanel>    
    </form>
    </div>
</body>
</html>
