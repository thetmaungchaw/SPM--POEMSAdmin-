<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AssignmentHistory.aspx.cs" Inherits="SPMWebApp.WebPages.AssignmentManagement.AssignmentHistory" %>

<%@ Register Src="~/PagingControl.ascx" TagName="PagingControl" TagPrefix="ucc" %>
<%@ Register Src="~/CoolCalendar.ascx" TagName="CoolCalendar" TagPrefix="ucc" %>
<%@ Register
    Assembly="AjaxControlToolkit"
    Namespace="AjaxControlToolkit"
    TagPrefix="ajaxToolkit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>SPM - Assignment History</title>
    <link rel="stylesheet" href="<%=ConfigurationManager.AppSettings["SPMCssFile"]%>" type="text/css" />    
    <script type="text/javascript">
        function MouseHover() {
            newwindow = window.open('CommissionEarnedChart.aspx', 'CommissionEarnedChart', 'height=350,width=800,scrollbars=yes');
            if (window.focus) { newwindow.focus() }
            return false;
        }  
    </script>
</head>
<body>
    <div id="Container">
    <form id="frmAssignmentHistory" runat="server">    
        <div class="title">Assignment History</div>
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
        
        
        <asp:UpdatePanel ID="uplAssignmentHistory" runat="server">
            <Triggers>           
                <asp:PostBackTrigger ControlID="btnExcel" />        
            </Triggers>
            <ContentTemplate>
                <%--<ajaxToolkit:ToolkitScriptManager ID="toolKitScriptManager1" runat="server">
                </ajaxToolkit:ToolkitScriptManager> --%>   

                <table>
                    <tr class="normal">     <!-- total cells: 14 -->                        
                        <td>Dealer:</td>
                        <td>&nbsp;</td>
                        <td>
                            <asp:DropDownList ID="ddlDealer" CssClass="normal" runat="server" />
                        </td>
                        <td>A/C:</td>
                        <td>
                            <asp:TextBox ID="txtAccountNo" Width="60" CssClass="normal" runat="server" />
                        </td>                                                
                        <td>&nbsp;</td>
                        <td>NRIC:</td>
                        <td>
                            <asp:TextBox ID="txtNric" Width="60" CssClass="normal" runat="server" />
                        </td>                                                
                        <td>
                        <asp:RadioButton ID="rdoDate" runat="server" GroupName="RadiobutGroup" CssClass="normal" oncheckedchanged="rdo_CheckedChanged"  AutoPostBack="true" />
                        </td>
                        <td>Assign From: </td>
                        <asp:Panel runat="server" ID="PnFromDate">
                        <td>
                            <ucc:CoolCalendar ID="calAssignFromDate" TimeRequired="false" DateTimeFormat="dd/MM/yyyy" DateTextRequired="false"
                                DateTextFromValue="" runat="server" />
                        </td>
                        </asp:Panel>
                        <td>&nbsp;</td>
                        <td>To: </td>
                        <asp:Panel runat="server" ID="PnToDate">
                        <td>
                            <ucc:CoolCalendar ID="calAssignToDate" TimeRequired="false" DateTimeFormat="dd/MM/yyyy" DateTextRequired="false"
                                DateTextFromValue="" runat="server" />
                        </td>
                        </asp:Panel>
                        <td>&nbsp;</td>   
                        </tr>  
                       <tr class="normal"> 
                        <td>&nbsp;</td>   
                        <td>&nbsp;</td>   
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>   
                        <td>&nbsp;</td>   
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>   
                        <td>&nbsp;</td>                                                             
                        <td class="style1">
                            <asp:RadioButton ID="rdoProj" runat="server" GroupName="RadiobutGroup"  AutoPostBack="true"
                                CssClass="normal" oncheckedchanged="rdo_CheckedChanged" />
                        </td>
                         <td class="style1">Project Name :</td>                       
                        <td class="style1">                          
                            <asp:DropDownList ID="ddlProjName" CssClass="normal" runat="server" />                                 
                        </td> 
                          <td>&nbsp;</td>                                          
                        <td>
                            <asp:CheckBox ID="chkRetrade" Text="Retrade Only" CssClass="normal" runat="server" />
                        </td>
                        </tr> 
                        <tr>
                        <td>&nbsp;</td>   
                        <td>&nbsp;</td>   
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>   
                        <td>&nbsp;</td>   
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>   
                        <td>&nbsp;</td> 
                        <td>&nbsp;</td>   
                        <td>&nbsp;</td> 
                        <td>                       
                                                      
                        </td>  
                        </tr>                   
                </table>
                
                <div>
                </div>
                <div>
                    <asp:Button ID="btnSearch" Text="Search" CssClass="normal" runat="server" 
                        onclick="btnSearch_Click" /> &nbsp;&nbsp;
                    <asp:Button ID="btnExcel" Text="Excel" CssClass="normal" runat="server" 
                        onclick="btnExcel_Click" />
                </div>
                
                <!-- Colors for CoreClient, Assigned, 2NClient -->
		        <div align="center">
		            <table border="1" bordercolor="#777777" cellspacing="0" cellpadding="0" class="normalGrey">
			            <tr class=normal align="center">				            
				            <td bgcolor="#FFCC99" width="210px">Recently ReTraded Client</td>
				            <td bgcolor="#C00000" width="210px">Miss Call</td>				            
			            </tr>
		            </table>
		        </div> <br />
                
                <div id="divMessage" class="normalRed" runat="server"></div>
                <div id="divAssignments" align="left">
                    <asp:GridView ID="gvAssignments" CssClass="normal" AllowPaging="true" ShowFooter="true"
                        PageSize="20" AutoGenerateColumns="false" AllowSorting="true" PagerSettings-Visible="false"
                        runat="server" onrowdatabound="gvAssignments_RowDataBound" 
                        onsorting="gvAssignments_Sorting">
                        <HeaderStyle BackColor="#CDCDCD" />
                        <RowStyle CssClass="normal" BackColor="White"/>
                        <alternatingrowstyle CssClass="normal" BackColor="#EEEEEE"/>
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    No.
                                </HeaderTemplate>                                
                                <ItemTemplate>                                    
                                    <%# Container.DataItemIndex + 1 %>                                    
                                </ItemTemplate>
                                <HeaderStyle BorderColor="#777777" CssClass="grayBorder"  />
                                <ItemStyle BorderColor="#777777" CssClass="grayBorder" />
                            </asp:TemplateField>                            
                             <asp:BoundField DataField="Dealer" SortExpression="Dealer" HeaderText="Dealer">
                                <HeaderStyle CssClass="grayBorder" />
                                <ItemStyle CssClass="grayBorder" Width="60px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="TeamCode" SortExpression="TeamCode" HeaderText="Team">
                                <HeaderStyle CssClass="grayBorder" />
                                <ItemStyle CssClass="grayBorder" Width="50px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="AcctNo" SortExpression="AcctNo" HeaderText="A/C">
                                <HeaderStyle CssClass="grayBorder" />
                                <ItemStyle CssClass="grayBorder" Width="70px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="NRIC" SortExpression="NRIC" HeaderText="NRIC">
                                <HeaderStyle CssClass="grayBorder" />
                                <ItemStyle CssClass="grayBorder" Width="70px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ClientName" SortExpression="ClientName" HeaderText="Name">
                                <HeaderStyle CssClass="grayBorder" />
                                <ItemStyle CssClass="grayBorder" Width="120px" />
                            </asp:BoundField> 
                            <asp:BoundField DataField="AcctCreateDate" dataformatstring="{0:dd/MM/yyyy}" HtmlEncode="false"
                                    SortExpression="AcctCreateDate" HeaderText="A/C Create Date">
                                <HeaderStyle CssClass="grayBorder" />
                                <ItemStyle CssClass="grayBorder" Width="90px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="AssignDate" dataformatstring="{0:dd/MM/yyyy}" HtmlEncode="false"
                                    SortExpression="AssignDate" HeaderText="Assigned Date">
                                <HeaderStyle CssClass="grayBorder" />
                                <ItemStyle CssClass="grayBorder" Width="90px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="AssignLTD" dataformatstring="{0:dd/MM/yyyy}" HtmlEncode="false"
                                    SortExpression="AssignLTD" HeaderText="AssignLTD">
                                <HeaderStyle CssClass="grayBorder" />
                                <ItemStyle CssClass="grayBorder" Width="90px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="CurrentLTD" dataformatstring="{0:dd/MM/yyyy}" HtmlEncode="false"
                                    SortExpression="CurrentLTD" HeaderText="CurrentLTD">
                                <HeaderStyle CssClass="grayBorder" />
                                <ItemStyle CssClass="grayBorder" Width="90px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="LastCallDate" dataformatstring="{0:dd/MM/yyyy HH:mm:ss}" HtmlEncode="false"
                                    SortExpression="LastCallDate" HeaderText="Last Call Date">
                                <HeaderStyle CssClass="grayBorder" />
                                <ItemStyle CssClass="grayBorder" Width="110px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="CutOffDate" dataformatstring="{0:dd/MM/yyyy HH:mm:ss}" HtmlEncode="false"
                                    SortExpression="CutOffDate" HeaderText="Cut Off Date">
                                <HeaderStyle CssClass="grayBorder" />
                                <ItemStyle CssClass="grayBorder" Width="110px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="MonthDiff" SortExpression="MonthDiff" FooterText="ReTrade Total" HeaderText="MonthDiff">
                                <HeaderStyle CssClass="grayBorder" />
                                <ItemStyle CssClass="grayBorder" Width="80px" />
                                <FooterStyle CssClass="grayBorder" Font-Bold="true" BackColor="#CDCDCD" />
                            </asp:BoundField>
                            <asp:TemplateField SortExpression="TotalComm">
                                <HeaderStyle CssClass="grayBorder" />
                                <ItemStyle CssClass="grayBorder" Width="80px" />
                                <FooterStyle CssClass="grayBorder" Font-Bold="true" BackColor="#CDCDCD" />
                                <HeaderTemplate>
                                    <asp:LinkButton id="lnkBtnTotalComm" runat="server" 
                                        Text="Total Comm" CommandName="Sort" CommandArgument="TotalComm"></asp:LinkButton>
                                    <asp:Image ID="imgChart" runat="server" ImageUrl="~/images/chart.PNG" onmouseover="MouseHover();"></asp:Image>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%# Eval("TotalComm")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--<asp:BoundField DataField="TotalComm" SortExpression="TotalComm" FooterText="" HeaderText="Total Comm">                                
                                <HeaderStyle CssClass="grayBorder" />
                                <ItemStyle CssClass="grayBorder" Width="80px" />
                                <FooterStyle CssClass="grayBorder" Font-Bold="true" BackColor="#CDCDCD" />
                            </asp:BoundField>--%>  
                             <asp:BoundField DataField="NoofTrades" SortExpression="NoofTrades" FooterText="" HeaderText="No of Trades">
                                <HeaderStyle CssClass="grayBorder" />
                                <ItemStyle CssClass="grayBorder" Width="80px" />
                                <FooterStyle CssClass="grayBorder" Font-Bold="true" BackColor="#CDCDCD" />
                            </asp:BoundField>
                        </Columns>                        
                    </asp:GridView>
                </div>   <br />
                <div id="divPaging" class="normal" runat="server">
                    <ucc:PagingControl ID="pgcAssignment" OnPageNoChanged="PagingControl_PageNoChange" OnRowPerPageChanged="PagingControl_RowPerPageChanged"
                        runat="server"/>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
    </div>
</body>
</html>
