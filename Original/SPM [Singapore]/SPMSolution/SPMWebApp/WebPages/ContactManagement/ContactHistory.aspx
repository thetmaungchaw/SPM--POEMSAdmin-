<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ContactHistory.aspx.cs" Inherits="SPMWebApp.WebPages.ContactManagement.ContactHistory" %>
<%@ Register Src="~/PagingControl.ascx" TagName="PagingControl" TagPrefix="ucc" %>
<%@ Register Src="~/CoolCalendar.ascx" TagName="CoolCalendar" TagPrefix="ucc" %>
<%@ Register
    Assembly="AjaxControlToolkit"
    Namespace="AjaxControlToolkit"
    TagPrefix="ajaxToolkit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Contact History</title>
    <link rel="stylesheet" href="<%=ConfigurationManager.AppSettings["SPMCssFile"]%>" type="text/css" />
    <style type="text/css">
        .style1
        {
            height: 55px;
        }
        .style3
        {
            width: 94px;
        }
        .style5
        {
            width: 95px;
        }
        .style6
        {
            width: 263px;
        }
        </style>
</head>
<body>
    <div id="Container">
    <form id="frmContactHistory" runat="server">    
        <div class="title">SPM - Contact History</div>
        
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
        
        <%--<script language="javascript" type="text/javascript">
            function clickButton(e, buttonid) {                
                var evt = e ? e : window.event;
                var bt = document.getElementById(buttonid);
                if (bt) {
                    if (evt.keyCode == 13) {
                        bt.click();
                        return false;
                    }
                }
            }
    </script>--%>
        
       
     
<asp:UpdatePanel ID="uplContactHistory" runat="server">
         <Triggers>           
                <asp:PostBackTrigger ControlID="btnExcel" />        
            </Triggers>
          <ContentTemplate>
                <table>
                     <tr class="normal">                                                     
                           <td class="normal">                         
                            <asp:RadioButton ID="rdoContactDate" runat="server"  GroupName="RadiobutGroup" 
                                AutoPostBack="true"  oncheckedchanged="rdo_CheckedChanged" CssClass="normal" />
                            </td>
                            <td class="normal" > 
                            <asp:Panel runat="server" ID="pn1" >                          
                            Contact From: <ucc:CoolCalendar ID="calContactFromDate" DateTextRequired="false" TimeRequired="false" DateTimeFormat="dd/MM/yyyy" 
                                                DateTextFromValue="" runat="server" />
                            </asp:Panel> 
                            </td>                                                  
                             
                            <td class="normal">
                            <asp:Panel runat="server" ID="pn2">
                                Contact To: <ucc:CoolCalendar ID="calContactToDate" TimeRequired="false" DateTextRequired="false" DateTimeFormat="dd/MM/yyyy" 
                                DateTextFromValue="" AutoPostBack="true" runat="server" />
                            </asp:Panel>
                            </td> 
                            
                            <td class="style5">
                            <asp:RadioButton ID="rdoProj" runat="server" GroupName="RadiobutGroup" CssClass="normal" AutoPostBack="true" OnCheckedChanged="rdo_CheckedChanged"/>                            
                             Project Name :
                             </td>                                   
                                                      
                            <td class="normal" style="margin-left: 40px">
                            <asp:Panel runat="server">
                             <asp:TextBox ID="txtProjName" runat="server" Width="300px"  CssClass="normal" 
                                   AutoPostBack="false" ontextchanged="txtProjName_TextChanged" ></asp:TextBox>
                            </td>
                            </asp:Panel>
                      </tr>
                     <caption>                         
                         <tr class="normal">
                             <td>
                             </td>
                             <td class="style6">
                                 &nbsp;</td>
                             <td>
                             </td>
                             <td class="style5">
                                 &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Project Name:</td>
                             <td class="normal">
                                 <asp:DropDownList ID="ddlProjName" runat="server" CssClass="normal" />
                             </td>
                         </tr>
                     </caption>
                    </table>
                    <table>
                    <tr class="normal">
                        <td class="normal">
                            A/C: &nbsp; <asp:TextBox ID="txtAccountNo" Width="70" CssClass="normal" runat="server" />
                        </td>
                        <td class="style1">
                            Team: &nbsp; <asp:DropDownList ID="ddlTeam" CssClass="normal" runat="server" />
                        </td>
                        <td class="style1">
                            Dealer: &nbsp; <asp:DropDownList ID="ddlDealer" CssClass="normal" runat="server" />
                        </td>
                                              
                    </tr>
                    <tr class="normal">
                        <td class="style3">                                
                            Rank: &nbsp;
                            <asp:DropDownList ID="ddlRank" CssClass="normal" runat="server" >                                
                            </asp:DropDownList>
                        </td>                        
                        <td>
                            Preference: &nbsp; 
                            <asp:DropDownList ID="ddlPreference" CssClass="normal" runat="server" />
                        </td>
                        <td>
                            Content: &nbsp; <asp:TextBox ID="txtContent" CssClass="normal" runat="server" />
                        </td>                        
                        <td>
                            <asp:Button ID="btnSearch" Text="Search" CssClass="normal" runat="server" 
                                onclick="btnContactHistorySearch_Click"  /> &nbsp;
                                
                                
                            <asp:Button ID="btnExcel" Text="Excel" CssClass="normal" onclick="btnExcel_Click" runat="server" />
                        </td>                        
                    </tr>
                </table> <br />
                
                <div align="center">
                    <table>
                        <tr>
                            <td class="normal" bgcolor="#FFCC99" width="500px">Extra Call</td>
                        </tr>
                    </table>
                </div>
                
                <div id="divMessage" class="normalRed" runat="server"></div> <br />
                
                <asp:GridView ID="gvContactHistory" CssClass="normal" AllowSorting="true" AutoGenerateColumns="False"
                         PagerSettings-Visible="false" AllowPaging="True" PageSize="20" runat="server" 
                         onrowdatabound="gvContactHistory_RowDataBound" OnSorting="gvContactHistory_Sorting">
                    <HeaderStyle BackColor="#CDCDCD" />
                    <PagerSettings Visible="False" />
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
                            <HeaderStyle BorderColor="#777777" CssClass="grayBorder" Width="15px" />
                            <ItemStyle BorderColor="#777777" CssClass="grayBorder" Width="15px" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="ContactDate" SortExpression="ContactDate" HeaderText="Contact Date"
                                dataformatstring="{0:dd/MM/yyyy HH:mm:ss}" HtmlEncode="false">
                            <HeaderStyle CssClass="grayBorder" />
                            <ItemStyle CssClass="grayBorder" Width="110px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Team" SortExpression="Team" HeaderText="Team">
                            <HeaderStyle CssClass="grayBorder" />
                            <ItemStyle CssClass="grayBorder" Width="60px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="AcctNo" SortExpression="AcctNo" HeaderText="A/C">
                            <HeaderStyle CssClass="grayBorder" />
                            <ItemStyle CssClass="grayBorder" Width="50px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Name" SortExpression="Name" HeaderText="Name">
                            <HeaderStyle CssClass="grayBorder" />
                            <ItemStyle CssClass="grayBorder" Width="120px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Sex" SortExpression="Sex" HeaderText="Gender">
                            <HeaderStyle CssClass="grayBorder" />
                            <ItemStyle CssClass="grayBorder" Width="25px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="ContactNo" SortExpression="ContactNo" HeaderText="Contact No">
                            <HeaderStyle CssClass="grayBorder" />
                            <ItemStyle CssClass="grayBorder" Width="80px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Content" HeaderText="Content">
                            <HeaderStyle CssClass="grayBorder" />
                            <ItemStyle CssClass="grayBorder" Width="160px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Remarks" HeaderText="Remark">
                            <HeaderStyle CssClass="grayBorder" />
                            <ItemStyle CssClass="grayBorder" Width="160px" />
                        </asp:BoundField>                            
                        <asp:TemplateField HeaderText="Preferences">                           
                            <ItemTemplate>                                                
                                <asp:Label ID="gvlblPreferences" CssClass="normal" runat="server" />
                            </ItemTemplate>
                            <HeaderStyle CssClass="grayBorder" />
                            <ItemStyle CssClass="grayBorder" Width="120px" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="RankText" SortExpression="RankText" HeaderText="Rank">
                            <HeaderStyle CssClass="grayBorder" />
                            <ItemStyle CssClass="grayBorder" Width="125px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Dealer" SortExpression="Dealer" HeaderText="Dealer">
                            <HeaderStyle CssClass="grayBorder" />
                            <ItemStyle CssClass="grayBorder" Width="60px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="AdminId" SortExpression="AdminId" HeaderText="AdminId">
                            <HeaderStyle CssClass="grayBorder" />
                            <ItemStyle CssClass="grayBorder" Width="60px" />
                        </asp:BoundField>
                    </Columns>
                </asp:GridView> <br />
                
                <div id="divPaging" class="normal" runat="server">
                    <ucc:PagingControl ID="pgcContactHistory" OnPageNoChanged="PagingControl_PageNoChange" OnRowPerPageChanged="PagingControl_RowPerPageChanged"
                        runat="server"/>
                </div>                                
           </ContentTemplate>
    </asp:UpdatePanel>
    </form>
    </div>
</body>
</html>
