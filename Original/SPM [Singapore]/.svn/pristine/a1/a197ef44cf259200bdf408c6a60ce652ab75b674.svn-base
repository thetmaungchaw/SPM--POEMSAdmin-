<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AssignedClientInfo.aspx.cs" Inherits="SPMWebApp.WebPages.AssignmentManagement.AssignedClientInfo" %>

<%@ Register Src="~/PagingControl.ascx" TagName="PagingControl" TagPrefix="ucc" %>

<%@ Register Src="~/CoolCalendar.ascx" TagName="CoolCalendar" TagPrefix="ucc" %>
<%@ Register
    Assembly="AjaxControlToolkit"
    Namespace="AjaxControlToolkit"
    TagPrefix="ajaxToolkit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>SPM - Assigned Client Info</title>
    <link rel="stylesheet" href="<%=ConfigurationManager.AppSettings["SPMCssFile"]%>" type="text/css" />
    <style type="text/css">
        .style1
        {
            height: 68px;
        }
        .style2
        {
            width: 117px;
        }
    </style>
</head>
<body>
    <div id="Container">
    <form id="frmAssignedClientInfo" runat="server">
    
        <div class="title">Assigned Client Info</div>
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
        
        <asp:UpdatePanel ID="uplAssignedClientInfo" runat="server">
            <Triggers>           
                <asp:PostBackTrigger ControlID="btnExcel" />        
            </Triggers>
            <ContentTemplate>
                <fieldset style="width:90%;border:1px solid gray;">
                    <legend style="font-family: Arial,Helvetica,sans-serif; font-size:11px; font-weight:bold;padding: 0 0 10px;">Retrieve Assignments</legend>
                    <table style="margin-left:10px">
                        <tr class="normal">
                            <td class="style2">
                            <asp:RadioButton ID="rdoDate" runat="server" GroupName="RadiobutGroup" CssClass="normal" oncheckedchanged="rdo_CheckedChanged"  AutoPostBack="true" />
                            Assign From Date :</td>
                             <asp:Panel runat="server" ID="PnFromDate">
                            <td> 
                                <ucc:CoolCalendar ID="calAssignFrom" TimeRequired="false" DateTimeFormat="dd/MM/yyyy" DateTextRequired="false"
                                    DateTextFromValue="" runat="server" />
                            </td>
                             </asp:Panel>
                            <td>Assign To Date :</td>
                            <asp:Panel runat="server" ID="PnToDate">
                            <td>
                                  <ucc:CoolCalendar ID="calAssignTo" TimeRequired="false" DateTimeFormat="dd/MM/yyyy" DateTextRequired="false"
                                    DateTextFromValue="" runat="server" />
                            </td>
                             </asp:Panel>
                            <td>
                                <asp:Button ID="btnRetrieveAssignment" Text="Retrieve Assignment" 
                                    CssClass="normal" runat="server" onclick="btnRetrieveAssignment_Click" /> &nbsp;
                                <b>(Please retrieve Assignments first before searching Assigned Client Info.)</b>
                            </td>
                        </tr>
                    </table>
                       <table style="margin-left:10px;font-family: Arial,Helvetica,sans-serif; font-size:11px; padding: 0 0 10px;"> 
                          <tr>
                        <td class="style1">
                            <asp:RadioButton ID="rdoProj" runat="server" GroupName="RadiobutGroup"  AutoPostBack="true"
                                CssClass="normal" oncheckedchanged="rdo_CheckedChanged" />
                        </td>
                         
                         <td class="style1">Project Name :</td>
                         <td>&nbsp;</td>                
                        <td class="style1">
                            <asp:TextBox ID="txtProjName" runat="server" Width="260px" CssClass="normal" 
                                Height="23px"></asp:TextBox>
                        </td>
                    </tr>                                
                  </table>
                </fieldset>
                <br />
            
                <fieldset style="width:90%;border:1px solid gray;">
                    <legend style="font-family: Arial,Helvetica,sans-serif; font-size:11px; font-weight:bold;padding: 0 0 10px;">Search Assigned Client Info</legend>
                    <table>
                        <tr class="normal">     <!-- total cells: 14 -->
                            <td>AssignDate: </td>
                            <td>                            
                                <asp:DropDownList ID="ddlAssignDate" CssClass="normal" runat="server" />
                            </td>
                            <td>Project Name:</td>
                            <td>                                
                                <asp:DropDownList ID="ddlProjName" CssClass="normal" runat="server" />
                            </td>  
                            <!--<td>Dealer:</td>
                            <td>
                                <asp:DropDownList ID="ddlDealer" CssClass="normal" runat="server" />
                            </td>
                            <td>&nbsp;</td>
                            <td>A/C:</td>
                            <td>
                                <asp:TextBox ID="txtAccountNo" Width="60" CssClass="normal" runat="server" />
                            </td>                                                                    
                            <td>&nbsp;</td>                        
                            <td>Team: </td>
                            <td>
                                <asp:DropDownList ID="ddlTeamCode" CssClass="normal" runat="server" />
                            </td>-->
                            <td>
                                &nbsp;
                                <asp:Button ID="btnSearch" Text="Search" onclick="btnSearch_Click" CssClass="normal" runat="server" /> &nbsp;
                                <asp:Button ID="btnExcel" Text="Excel" CssClass="normal" onclick="btnExcel_Click" runat="server" />
                            </td>
                        </tr>          
                    </table>
                   
                </fieldset>
                                
                <div id="divMessage" class="normalRed" runat="server"></div> <br />
                
                <asp:GridView ID="gvClientInfo" CssClass="normal" runat="server" 
                    AutoGenerateColumns="false" AllowPaging="true" PageSize="20" PagerSettings-Visible="false"
                        AllowSorting="true" onsorting="gvClientInfo_Sorting">
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
                        <asp:BoundField DataField="Dealer" SortExpression="Dealer" HeaderText="Assign">
                            <HeaderStyle CssClass="grayBorder" />
                            <ItemStyle CssClass="grayBorder" Width="60px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Team" SortExpression="Team" HeaderText="Team">
                            <HeaderStyle CssClass="grayBorder" />
                            <ItemStyle CssClass="grayBorder" Width="50px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="AcctNo" SortExpression="AcctNo" HeaderText="A/C">
                            <HeaderStyle CssClass="grayBorder" />
                            <ItemStyle CssClass="grayBorder" Width="70px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="ClientName" SortExpression="ClientName" HeaderText="Name">
                            <HeaderStyle CssClass="grayBorder" />
                            <ItemStyle CssClass="grayBorder" Width="160px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="LTD" SortExpression="LTD" dataformatstring="{0:dd/MM/yyyy}" HtmlEncode="false" HeaderText="LTD">
                            <HeaderStyle CssClass="grayBorder" />
                            <ItemStyle CssClass="grayBorder" Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Mobile" HeaderText="Mobile">
                            <HeaderStyle CssClass="grayBorder" />
                            <ItemStyle CssClass="grayBorder" Width="70px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Email" HeaderText="Email">
                            <HeaderStyle CssClass="grayBorder" />
                            <ItemStyle CssClass="grayBorder" Width="90px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Address" HeaderText="Address">
                            <HeaderStyle CssClass="grayBorder" />
                            <ItemStyle CssClass="grayBorder" Width="270px" />
                        </asp:BoundField>
                    </Columns>        
                </asp:GridView> <br />
                <div id="divPaging" class="normal" runat="server">
                    <ucc:PagingControl ID="pgcClientInfo" OnPageNoChanged="PagingControl_PageNoChange" OnRowPerPageChanged="PagingControl_RowPerPageChanged"
                        runat="server"/>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        </form>
    </div>
</body>
</html>
