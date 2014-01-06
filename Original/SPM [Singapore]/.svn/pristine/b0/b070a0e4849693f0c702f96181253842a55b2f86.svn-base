<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LeadAssign.aspx.cs" Inherits="SPMWebApp.WebPages.LeadManagement.LeadAssign" %>

<%@ Register Src="~/PagingControl.ascx" TagName="PagingControl" TagPrefix="ucc" %>
<%@ Register Src="~/CoolCalendar.ascx" TagName="CoolCalendar" TagPrefix="ucc" %>
<%@ Register
    Assembly="AjaxControlToolkit"
    Namespace="AjaxControlToolkit"
    TagPrefix="ajaxToolkit" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>SPM - Leads Assignment - Lite version</title>
    <!--<link rel="stylesheet" href="<%=ConfigurationManager.AppSettings["SPMCssFile"]%>" type="text/css" /> -->    
    <link rel="Stylesheet" href="../../StyleSheet/SPMStyle.css" />        
    <style type="text/css">
        .style1
        {
            width: 326px;
            height: 59px;
        }
        </style>
</head>
<body>
    <div id="Container">
    <form id="frmClientAssign" runat="server">            
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <script>
        function SelectAll(id)
            {
                //get reference of GridView control
                var grid = document.getElementById("<%= gvDealer.ClientID %>");
                //variable to contain the cell of the grid
                var cell;
                
                if (grid.rows.length > 0)
                {
                    //loop starts from 1. rows[0] points to the header.
                    for (i=1; i<grid.rows.length; i++)
                    {
                        //get the reference of first column
                        cell = grid.rows[i].cells[0];
                        
                        //loop according to the number of childNodes in the cell
                        for (j=0; j<cell.childNodes.length; j++)
                        {           
                            //if childNode type is CheckBox                 
                            if (cell.childNodes[j].type =="checkbox")
                            {
                            //assign the status of the Select All checkbox to the cell checkbox within the grid
                                cell.childNodes[j].checked = document.getElementById(id).checked;
                            }
                        }
                    }
                }
            }
            
            function SelectAll2(id) {
                //get reference of GridView control
                var grid = document.getElementById("<%= gvLeadAssign.ClientID %>");
                //variable to contain the cell of the grid
                var cell;

                if (grid.rows.length > 0) {
                    //loop starts from 1. rows[0] points to the header.
                    for (i = 1; i < grid.rows.length; i++) {
                        //get the reference of first column
                        cell = grid.rows[i].cells[0];

                        //loop according to the number of childNodes in the cell
                        for (j = 0; j < cell.childNodes.length; j++) {
                            //if childNode type is CheckBox                 
                            if (cell.childNodes[j].type == "checkbox") {
                                //assign the status of the Select All checkbox to the cell checkbox within the grid
                                cell.childNodes[j].checked = document.getElementById(id).checked;
                            }
                        }
                    }
                }
            }
        </script>
                
        <asp:UpdatePanel ID="uplLeadsAssign" runat="server">
            <ContentTemplate>
               <div id="div1" align="center" class="normalRed" runat="server"></div>	
                
                <div id="divTitle" class="title" runat="server">Leads Assign</div>
                <div id="divDealers" align="center">
                    <fieldset style="width:100%;border:1px solid gray;">                        
                        <legend style="font-family: Arial,Helvetica,sans-serif; font-size:11px; font-weight:bold;padding: 0 0 10px;">Team</legend>                         
                        <table cellpadding="0" cellspacing="0" width="100%" border="0" class="normalGrey">
                            <tr>
                                <!-- <td width="20%" valign="top" class="title">Client Assign</td> -->
		                        <td align="center">			        
			                      
                                    <b>(Please select team first before searching leads.)</b> 
                                    <br /> 
                                     <asp:ListBox ID="lstTeamCode" runat="server" AppendDataBoundItems="True" 
                                        AutoPostBack="True" CssClass="normal" Height="135px" 
                                        SelectionMode="Multiple" 
                                        OnSelectedIndexChanged="lstTeamCode_SelectedIndexChanged" Width="177px">                                    
                                                                       
                                    </asp:ListBox>                              
		                            <br />
                                    <b><i>For multiple selection, click on Ctrl and Select</i></b></td>
	                        </tr>	                
                        </table> <br />                        
                            
                            
                            <asp:GridView ID="gvDealer" runat="server" CssClass="normal" 
                            AutoGenerateColumns="false" onrowdatabound="gvDealer_RowDataBound">
                                <AlternatingRowStyle BackColor="#FFFFCC" />
                                <RowStyle BackColor="White" />
                                <HeaderStyle BackColor="Silver" />
                                <Columns>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                        <asp:CheckBox ID="gvchkSelectAll" runat="server" AutoPostBack="true"  />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                        <asp:CheckBox ID="gvchkAutoSelect" runat="server" />
                                        </ItemTemplate>                                      
                                        <HeaderStyle CssClass="grayBorder" />                            
                                        <ItemStyle CssClass="grayBorder"/>  
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="AECode" SortExpression="AECode" HeaderText="Dealer Code">  
                                        <HeaderStyle CssClass="grayBorder" />                            
                                        <ItemStyle CssClass="grayBorder" Width="100px" />  
                                    </asp:BoundField>
                                    <asp:BoundField DataField="AEName" SortExpression="AECode" HeaderText="Dealer Name">  
                                        <HeaderStyle CssClass="grayBorder" />                            
                                        <ItemStyle CssClass="grayBorder" Width="200px" />  
                                    </asp:BoundField>
                                    <asp:BoundField DataField="CurrentAssign" SortExpression="AECode" HeaderText="Assigned">  
                                        <HeaderStyle CssClass="grayBorder" />                            
                                        <ItemStyle CssClass="grayBorder" Width="60px" />  
                                    </asp:BoundField>
                                </Columns>
                            </asp:GridView>
                            
                            <asp:HiddenField ID="hidSelectedTeamCode" runat="server" Visible="False" />
                            
                            <table style="height: 61px">
                            <tr valign="middle">
                            <td><asp:Label ID="lblProjectName" runat="server" CssClass="normal" Visible="False">Project Name: &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</asp:Label></td>
                            <td><asp:TextBox ID="txtProjectName" runat="server" Width="300px" CssClass="normal" 
                                    Visible="False"></asp:TextBox>
                                </td>
                            <td class="normal">
                            <asp:Label ID="lblCutOff" runat="server" CssClass="normal" Visible="false"> Cut off DateTime: &nbsp;</asp:Label>                             
                                    <ucc:CoolCalendar ID="calCutOffDate" TimeRequired="true" DateTimeFormat="dd/MM/yyyy HH:mm:ss"
                                        DateTextRequired="false" DateTextFromValue="" runat="server" 
                                    Visible="False" />  
                            </td>
                            <td><asp:CheckBox ID="chkAutoAssign" Text="Auto Assign" CssClass="normal" runat="server" />   &nbsp;&nbsp;&nbsp;
					                <asp:Button ID="btnLeadSearch" Text="Search" CssClass="normal" runat="server" 
                                        onclick="btnLeadSearch_Click" /></td>
                            </tr>
                            </table>
                            
                            <br />               
                    </fieldset>               
                </div>                                                                                                
                
                	      
                <br />
                
		        <!-- Colors for  Assigned Leads-->
		        <div align="center">
		            <table border="1" bordercolor="#777777" cellspacing="0" cellpadding="0" class="normalGrey">
			            <tr class=normal align="center">
				          <!--
				            <td bgcolor="#FFCC99" width="150px">Core Client</td> -->
				            <td bgcolor="#FFFF99" width="150px">Assigned</td>
				          <!--
				            <td bgcolor="#FFCCFF" width="150px">2N Client</td> -->
				            <td bgcolor="#C00000" width="150px">Miss Call</td>
				            <td style="padding-right:20px;width:162px;" align="right">
				                <asp:Button ID ="btnSaveAssignment" Text="Save Assignment" CssClass="normal" runat="server" 
                                        onclick="btnSaveAssignment_Click" />
				            </td>		
				            <td style="padding-right:30px;width:120px;" align="right">
				             <asp:Button ID="btnAssignmentDelete" runat="server" Text="Delete" CssClass="normal"
                                    onclick="btnAssignmentDelete_Click" Width="74px" />
				            </td>		            
			            </tr>
		            </table>
		        </div> <br />
		        <div id="divMessage" align="center" class="normalRed" runat="server"></div>
               
                <!-- DIV for adding Scorllbar in Assignment GridView
                <div id="divClientAssign" style="width:100%;height:300px;overflow:auto"> </div>
                -->
                                   
                <asp:GridView ID="gvLeadAssign" CssClass="normal" AutoGenerateColumns="False" 
                    runat="server" OnRowDeleting="gvLeadAssign_RowDeleting"
                    onrowdatabound="gvLeadAssign_RowDataBound" AllowPaging="True" 
                    PagerSettings-Visible="false" AllowSorting="True" 
                    onsorting="gvLeadAssign_Sorting">
                    <HeaderStyle BackColor="#CDCDCD" />
                    <PagerSettings Visible="False" />
                    <RowStyle CssClass="normal" BackColor="White" BorderColor="#777777" />
                    <alternatingrowstyle CssClass="normal" BackColor="#EEEEEE" BorderColor="#777777" />
                    <Columns>
                     <asp:TemplateField>
                           <HeaderTemplate>
                                <asp:CheckBox ID="gvchkSelectAll2" runat="server" AutoPostBack="true"  />
                           </HeaderTemplate>
                           <ItemTemplate>
                                 <asp:CheckBox ID="chkRecID" runat="server" />                                  
                            </ItemTemplate>
                             <HeaderStyle BorderColor="#777777" CssClass="grayBorder" Width="30px" />
                             <ItemStyle BorderColor="#777777" CssClass="grayBorder" Width="30px" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                No.
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1%>
                                <asp:HiddenField ID="gvhdfAssignStatus" runat="server" />
                                <asp:HiddenField ID="gvhdfRecordId" Value='<%# Container.DataItemIndex %>' runat="server" />
                            </ItemTemplate>
                            <HeaderStyle BorderColor="#777777" CssClass="grayBorder" Width="30px" />
                            <ItemStyle BorderColor="#777777" CssClass="grayBorder" Width="30px" />
                        </asp:TemplateField>                
                        <asp:BoundField DataField="LeadId" HeaderText="A/C." SortExpression="LeadId" Visible="false">
                            <HeaderStyle CssClass="grayBorder" />
                            <ItemStyle CssClass="grayBorder" Width="50px" />
                        </asp:BoundField>                        
                        <asp:BoundField DataField="LeadName" HeaderText="Name" SortExpression="Name">
                            <HeaderStyle CssClass="grayBorder" />
                            <ItemStyle CssClass="grayBorder" Width="110px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="LeadNRIC" HeaderText="NRIC / Passport No" SortExpression="LeadNRIC">
                            <HeaderStyle CssClass="grayBorder" />
                            <ItemStyle CssClass="grayBorder" Width="80px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="LeadMobile" HeaderText="Moblie No" SortExpression="LeadMobile">
                            <HeaderStyle CssClass="grayBorder" />
                            <ItemStyle CssClass="grayBorder" Width="50px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="LeadHomeNo" HeaderText="Home No" SortExpression="LeadHomeNo">
                            <HeaderStyle CssClass="grayBorder" />
                            <ItemStyle CssClass="grayBorder" Width="50px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="LeadGender" HeaderText="Gender" SortExpression="LeadGender">
                            <HeaderStyle CssClass="grayBorder" />
                            <ItemStyle CssClass="grayBorder" Width="50px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Event" HeaderText="Event" SortExpression="Evnet">
                            <HeaderStyle CssClass="grayBorder" />
                            <ItemStyle CssClass="grayBorder" Width="50px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="PreferMode" HeaderText="Prefer Mode of Contact" SortExpression="PreferMode">
                            <HeaderStyle CssClass="grayBorder" />
                            <ItemStyle CssClass="grayBorder" Width="50px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="LeadEmail" HeaderText="Email" SortExpression="LeadEmail">
                            <HeaderStyle CssClass="grayBorder" />
                            <ItemStyle CssClass="grayBorder" Width="80px" />
                        </asp:BoundField>                                          
                                           
                        <asp:BoundField DataField="LastCallDealer" HeaderText="Last Call Dealer" SortExpression="LastCallDealer">
                            <HeaderStyle CssClass="grayBorder" />
                            <ItemStyle CssClass="grayBorder" Width="40px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="LastCallDate" HeaderText="Last Call Date" SortExpression="LastCallDate"
                                dataformatstring="{0:dd/MM/yyyy}" HtmlEncode="false">
                            <HeaderStyle CssClass="grayBorder" />
                            <ItemStyle CssClass="grayBorder" Width="60px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="LastAssignDealer" HeaderText="Last Assign Dealer" SortExpression="LastAssignDealer">
                            <HeaderStyle CssClass="grayBorder" />
                            <ItemStyle CssClass="grayBorder" Width="50px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="AssignDate" HeaderText="Last Assign Date" SortExpression="AssignDate" 
                                dataformatstring="{0:dd/MM/yyyy}" HtmlEncode="false">
                            <HeaderStyle CssClass="grayBorder" />
                            <ItemStyle CssClass="grayBorder" Width="60px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="LastCutOffDate" HeaderText="Last CutOff Date" SortExpression="LastCutOffDate"
                                dataformatstring="{0:dd/MM/yyyy}" HtmlEncode="false">
                            <HeaderStyle CssClass="grayBorder" />
                            <ItemStyle CssClass="grayBorder" Width="70px" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Assign To">
                             <ItemTemplate>
                                 <asp:DropDownList ID="gvcboDealer" runat="server">
                                 </asp:DropDownList>
                                 <!-- Bind("AssignDealer") -->
                                 <asp:HiddenField ID="gvhdfLastAssignDealer" Value='<%# Bind("AssignDealer") %>' runat="server" />
                             </ItemTemplate>
                             <HeaderStyle BorderColor="#777777" CssClass="grayBorder"/>
                             <ItemStyle BorderColor="#777777" CssClass="grayBorder" HorizontalAlign="Left" />
                             <ControlStyle CssClass="normal" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Cut Off">                           
                            <ItemTemplate>
                                <asp:TextBox ID="gvtxtCutOffDate" runat="server" CssClass="normal" 
                                    Text='<%# Bind("CutOffDate") %>' />
                                
                            </ItemTemplate>
                            <HeaderStyle CssClass="grayBorder" />
                            <ItemStyle CssClass="grayBorder" Width="30px" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Button ID="gvBtnDelete" runat="server" CommandName="Delete" 
                                    CssClass="normal" Text="Delete" />
                            </ItemTemplate>
                        </asp:TemplateField>                       
                    </Columns>
                </asp:GridView>    <br />
                 
                 <div id="divPaging" class="normalGrey" runat="server">
                    <ucc:PagingControl ID="pgcLeadAssign" OnPageNoChanged="PagingControl_PageNoChange" OnRowPerPageChanged="PagingControl_RowPerPageChanged"
                        runat="server"/>
                 </div>                 
                <br />
                
                   <asp:Repeater id="rptAutoAssign" runat="server">
                    <HeaderTemplate>
                        <table width="50%" border="1" bordercolor="#777777" cellspacing="0" cellpadding="0" class="normalGrey">
			                <tr class="normal" align="center" bgcolor="#CCFF99">
				                <td colspan="2"><b>Auto Selected</b></td>
			                </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr class=normal align="center" bgcolor="#CCFF99">
				            <td><%#DataBinder.Eval(Container, "DataItem.AECode")%></td>
				            <td><%#DataBinder.Eval(Container, "DataItem.AssignedCount")%></td>
			            </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>
               
                <br />
                
            </ContentTemplate>
        </asp:UpdatePanel>    
        
        <br />
    </form>
    </div>
</body>
</html>
