<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CallReport.aspx.cs" Inherits="SPMWebApp.WebPages.ContactManagement.CallReport" %>

<%@ Register Src="~/PagingControl.ascx" TagName="PagingControl" TagPrefix="ucc" %>
<%@ Register Src="~/CoolCalendar.ascx" TagName="CoolCalendar" TagPrefix="ucc" %>
<%@ Register
    Assembly="AjaxControlToolkit"
    Namespace="AjaxControlToolkit"
    TagPrefix="ajaxToolkit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>SPM - Call Report</title>
    <link rel="stylesheet" href="<%=ConfigurationManager.AppSettings["SPMCssFile"]%>" type="text/css" />
    <style type="text/css">
        .style1
        {
            width: 57px;
        }
        .style2
        {
            height: 86px;
        }
        .style6
        {
            width: 20px;
            height: 86px;
        }
        .normal
        {
            height: 26px;
        }
        .style7
        {
            height: 49px;
        }
    </style>
</head>
<body>
    <div id="Container">
    <form id="frmCallReport" runat="server">    
        <div class="title">Call Report</div>        
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
        
        <asp:UpdatePanel ID="uplCallReport" runat="server">
            <Triggers>           
                <asp:PostBackTrigger ControlID="btnExcel" />        
            </Triggers>
            <ContentTemplate>
                <br />
                <fieldset style="width:80%;border:1px solid gray;">
                    <legend style="font-family: Arial,Helvetica,sans-serif; font-size:11px; font-weight:bold;padding: 0 0 10px;">Retrieve Assignments</legend>
                    <table style="margin-left:10px">
                        <tr class="normal">
                            <td >
                                <asp:RadioButton ID="rdoDate" runat="server"  GroupName="RadiobutGroup"  CssClass="normal"
                                   AutoPostBack="true" oncheckedchanged="rdo_CheckedChanged" />
                            </td>
                            <td >Assign From Date :</td>
                            <asp:Panel runat="server" ID="PnFromDate">
                            <td > 
                                <ucc:CoolCalendar ID="calAssignFrom" TimeRequired="false" DateTimeFormat="dd/MM/yyyy" DateTextRequired="false"
                                    DateTextFromValue="" runat="server" />
                            </td>
                            </asp:Panel>
                            <td >Assign To Date :</td>
                             <asp:Panel runat="server" ID="PnToDate"  >
                            <td >
                                <ucc:CoolCalendar ID="calAssignTo" TimeRequired="false" DateTimeFormat="dd/MM/yyyy" DateTextRequired="false"  
                                    DateTextFromValue="" runat="server" />
                            </td>
                            </asp:Panel>
                            <td >
                                <asp:Button ID="btnRetrieveAssignment" Text="Retrieve Assignment" 
                                    CssClass="normal" runat="server" onclick="btnRetrieveAssignment_Click" /><br/>
                                <b>(Please retrieve Assignments first before searching Call Report.)</b>
                            </td>
                        </tr>
                    </table>
                  <table style="margin-left:10px;font-family: Arial,Helvetica,sans-serif; font-size:11px; padding: 0 0 10px;"> 
                    <tr>
                        <td class="normal">
                            <asp:RadioButton ID="rdoProj" runat="server" GroupName="RadiobutGroup" CssClass="normal"  AutoPostBack="true" oncheckedchanged="rdo_CheckedChanged" />
                        </td>
                         <td class="normal">Project Name :</td>  
                         <td>&nbsp;&nbsp; &nbsp;</td>                     
                        <td class="normal">                          
                           <asp:TextBox ID="txtProjName" runat="server" Width="300px" CssClass="normal" 
                                Height="18px" >
                            </asp:TextBox>
                        </td>
                    </tr>
                  </table>
                     
                </fieldset>
                <br />
            
                <fieldset style="width:80%;border:1px solid gray;">
                    <legend style="font-family: Arial,Helvetica,sans-serif; font-size:11px; font-weight:bold;padding: 0 0 10px;">Search Call Report</legend>
                    <table style="margin-left:10px">                 
                        <tr class="normal">
                            <td>Assign Date:</td>
                            <td>                                
                                <asp:DropDownList ID="ddlAssignDate" CssClass="normal" runat="server" />
                            </td> 
                            <td></td>
                            <td></td>
                            <td>Project Name:</td>
                            <td>                                
                                <asp:DropDownList ID="ddlProjName" CssClass="normal" runat="server" 
                                    Width="158px" />
                            </td>                        
                        </tr>
                        <tr class="normal">
                            <td colspan="2">
                                <asp:Button ID="btnSearch" Text="Search" onclick="btnSearch_Click" CssClass="normal" runat="server" /> &nbsp;
                                <asp:Button ID="btnExcel" Text="Excel" CssClass="normal" runat="server" 
                                    onclick="btnExcel_Click" />
                            </td>
                        </tr>
                    </table>
                </fieldset>                                
                
                <div id="divMessage" class="normalRed" runat="server"></div> <br />
                
                <table>
                    <tr>
                        <td valign="top">
                            <div>
                                <asp:GridView ID="gvCallReport" CssClass="normal" AutoGenerateColumns="false" ShowFooter="true"
                                    AllowSorting="true" AllowPaging="true" PageSize="20" PagerSettings-Visible="false" runat="server" 
                                    onrowcommand="gvCallReport_RowCommand" onrowdatabound="gvCallReport_RowDataBound"
                                    OnSorting="gvCallReport_Sorting">
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
                                        <asp:BoundField DataField="Team" SortExpression="Team" HeaderText="Team">
                                            <HeaderStyle CssClass="grayBorder" />
                                            <ItemStyle CssClass="grayBorder" Width="80px" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Dealer" SortExpression="Dealer">                                                                           
                                            <ItemTemplate>                                    
                                                <asp:LinkButton ID="gvlbtnDealer" Text='<%# Bind("Dealer") %>' CommandName="Dealer" CommandArgument="<%# Container.DataItemIndex %>"
                                                    ForeColor="Blue" CssClass="normal" runat="server" />
                                            </ItemTemplate>
                                            <HeaderStyle BorderColor="#777777" CssClass="grayBorder"  />
                                            <ItemStyle BorderColor="#777777" CssClass="grayBorder" Width="80px" />
                                        </asp:TemplateField>                        
                                        <asp:BoundField DataField="AEName" SortExpression="AEName" HeaderText="Dealer Name" FooterText="Total">
                                            <HeaderStyle CssClass="grayBorder" />
                                            <ItemStyle CssClass="grayBorder" Width="120px" />
                                            <FooterStyle CssClass="grayBorder" Font-Bold="true" BackColor="#CDCDCD" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="TotalAssign" SortExpression="TotalAssign" HeaderText="Total Assign"
                                                FooterText="">
                                            <HeaderStyle CssClass="grayBorder" />
                                            <ItemStyle CssClass="grayBorder" Width="80px" />
                                            <FooterStyle CssClass="grayBorder" Font-Bold="true" BackColor="#CDCDCD" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Miss" SortExpression="Miss" HeaderText="Miss">
                                            <HeaderStyle CssClass="grayBorder" />
                                            <ItemStyle CssClass="grayBorder" Width="50px" />
                                            <FooterStyle CssClass="grayBorder" Font-Bold="true" BackColor="#CDCDCD" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Extra" SortExpression="Extra" HeaderText="Extra" Visible="false">
                                            <HeaderStyle CssClass="grayBorder" />
                                            <ItemStyle CssClass="grayBorder" Width="50px" />
                                            <FooterStyle CssClass="grayBorder" Font-Bold="true" BackColor="#CDCDCD" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ReTradeA" SortExpression="ReTradeA" HeaderText="ReTrade(Assign)">
                                            <HeaderStyle CssClass="grayBorder" />
                                            <ItemStyle CssClass="grayBorder" Width="90px" />
                                            <FooterStyle CssClass="grayBorder" Font-Bold="true" BackColor="#CDCDCD" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ReTradeE" SortExpression="ReTradeE" HeaderText="ReTrade(Extra)" Visible="false">
                                            <HeaderStyle CssClass="grayBorder" />
                                            <ItemStyle CssClass="grayBorder" Width="90px" />
                                            <FooterStyle CssClass="grayBorder" Font-Bold="true" BackColor="#CDCDCD" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Score" SortExpression="Score" HeaderText="Score" Visible="false">
                                            <HeaderStyle CssClass="grayBorder" />
                                            <ItemStyle CssClass="grayBorder" Width="50px" />
                                        </asp:BoundField>
                                    </Columns>
                                </asp:GridView>
                            </div>                            
                            <br />
                            <div id="divPaging" align="left" class="normal" runat="server">
                                <ucc:PagingControl ID="pgcCallReport" OnPageNoChanged="PagingControl_PageNoChange" OnRowPerPageChanged="PagingControl_RowPerPageChanged"
                                    runat="server"/>
                            </div>
                        </td>
                        <td valign="top" style="padding-left:10px;">
                            <div id="divContactInfo" style="vertical-align:top;" runat="server">
                                <asp:Label ID="lblDealer" Text="" CssClass="normal" runat="server" /><br />
                                <table>
                                    <tr class="normal">
                                        <td bgcolor="#C00000" class="style1">Miss Call</td>
                                        <td bgcolor="#FFCC99" class="style1">Extra Call</td>
                                    </tr>
                                </table>
                                <div id="divClientContact" runat="server">
                                <asp:GridView ID="gvContactInfo" CssClass="normal" AutoGenerateColumns="false" AllowPaging="true" PageSize="20"
                                    PagerSettings-Visible="false" runat="server" AllowSorting="true" Caption="Client Contact Info"
                                    onrowdatabound="gvContactInfo_RowDataBound" OnSorting="gvContactInfo_Sorting">
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
                                        <asp:BoundField DataField="AcctNo" SortExpression="AcctNo" HeaderText="A/C No">
                                            <HeaderStyle CssClass="grayBorder" />
                                            <ItemStyle CssClass="grayBorder" Width="80px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ContactDate" SortExpression="ContactDate" HeaderText="Contact Date"
                                                dataformatstring="{0:dd/MM/yyyy HH:mm:ss}" HtmlEncode="false">
                                            <HeaderStyle CssClass="grayBorder" />
                                            <ItemStyle CssClass="grayBorder" Width="100px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="CutOffDate" SortExpression="CutOffDate" HeaderText="CutOffDate"
                                                dataformatstring="{0:dd/MM/yyyy HH:mm:ss}" HtmlEncode="false">
                                            <HeaderStyle CssClass="grayBorder" />
                                            <ItemStyle CssClass="grayBorder" Width="100px" />
                                        </asp:BoundField>
                                    </Columns>   
                                </asp:GridView>
                                <div id="divContactPaging" class="normal" runat="server">
                                <ucc:PagingControl ID="pgcContactInfo" OnPageNoChanged="ContactInfo_PageNoChange" 
                                    OnRowPerPageChanged="ContactInfo_RowPerPageChanged" runat="server"/>
                                </div>  
                                </div>
                                <div id="divLeadContact" runat="server">
                                <asp:GridView ID="gvLeadContactInfo" CssClass="normal" AutoGenerateColumns="false" AllowPaging="true" PageSize="20"
                                    PagerSettings-Visible="false" runat="server" AllowSorting="true" Caption="Lead Contact Info"
                                    onrowdatabound="gvLeadContactInfo_RowDataBound" OnSorting="gvLeadContactInfo_Sorting">
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
                                        <asp:BoundField DataField="Name" SortExpression="Name" HeaderText="Name">
                                            <HeaderStyle CssClass="grayBorder" />
                                            <ItemStyle CssClass="grayBorder" Width="80px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ContactDate" SortExpression="ContactDate" HeaderText="Contact Date"
                                                dataformatstring="{0:dd/MM/yyyy HH:mm:ss}" HtmlEncode="false">
                                            <HeaderStyle CssClass="grayBorder" />
                                            <ItemStyle CssClass="grayBorder" Width="100px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="CutOffDate" SortExpression="CutOffDate" HeaderText="CutOffDate"
                                                dataformatstring="{0:dd/MM/yyyy HH:mm:ss}" HtmlEncode="false">
                                            <HeaderStyle CssClass="grayBorder" />
                                            <ItemStyle CssClass="grayBorder" Width="100px" />
                                        </asp:BoundField>
                                    </Columns>   
                                </asp:GridView>
                                <div id="divLeadContactPaging" class="normal" runat="server">
                                <ucc:PagingControl ID="pgcLeadContactInfo" OnPageNoChanged="LeadContactInfo_PageNoChange" 
                                    OnRowPerPageChanged="LeadContactInfo_RowPerPageChanged" runat="server"/>
                                </div>  
                                </div>
                            </div> 
                            <br />                                                                     
                        </td>
                    </tr>
                </table>               
                
            </ContentTemplate>
        </asp:UpdatePanel>    
    </form>
    </div>
</body>
</html>
