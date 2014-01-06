<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ClientAssignLite.aspx.cs" Inherits="SPMWebApp.WebPages.AssignmentManagement.ClientAssignLite" %>

<%@ Register Src="~/PagingControl.ascx" TagName="PagingControl" TagPrefix="ucc" %>
<%@ Register Src="~/CoolCalendar.ascx" TagName="CoolCalendar" TagPrefix="ucc" %>
<%@ Register
    Assembly="AjaxControlToolkit"
    Namespace="AjaxControlToolkit"
    TagPrefix="ajaxToolkit" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>SPM - Client Assignment - Lite version</title>
    <link rel="stylesheet" href="<%=ConfigurationManager.AppSettings["SPMCssFile"]%>" type="text/css" />              
    <style type="text/css">
        .normal
        {}
    </style>
</head>
<body>
    <div id="Container">
    <form id="frmClientAssign" runat="server">            
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        
        <script type="text/javascript" language="javascript">
            <!--
            var prm = Sys.WebForms.PageRequestManager.getInstance();
     
            prm.add_initializeRequest(InitializeRequest);
            prm.add_endRequest(EndRequest);

            function InitializeRequest(sender, args) 
            {
                //sender._postBackSettings.sourceElement.id.indexOf("gvBtnDelete") < 0
                //if((args._postBackElement.id == 'btnSearch') || (args._postBackElement.id == 'btnSaveAssignment'))
                            
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
                //sender._postBackSettings.sourceElement.id.indexOf("gvBtnDelete")
                //if((sender._postBackSettings.sourceElement.id == 'btnSearch') || (sender._postBackSettings.sourceElement.id == 'btnSaveAssignment'))                                                
                
                // Get a reference to the element that raised the postback which is completing to enable it.
                var postBackControl = $get(sender._postBackSettings.sourceElement.id);
                
                $get('Container').style.cursor = 'auto';                
                if(postBackControl != null)
                {                    
                    postBackControl.disabled = false;      
                }
            }
            
            function SelectAssign(id) {
                //get reference of GridView control
                var grid = document.getElementById("<%= gvClientAssign.ClientID %>");
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
            
            // -->        
        </script>
        
        <%--<asp:UpdatePanel ID="uplClientAssign" runat="server">
            <ContentTemplate>--%>
                <div id="div1" align="center" class="normalRed" runat="server"></div>	
                
                <div id="divTitle" class="title" runat="server">Client Assign</div>
                <div id="divDealers" align="center">
                    <fieldset style="width:100%;border:1px solid gray;">                        
                        <legend style="font-family: Arial,Helvetica,sans-serif; font-size:11px; font-weight:bold;padding: 0 0 10px;">Team</legend>                         
                        <table cellpadding="0" cellspacing="0" width="100%" border="0" class="normalGrey">
                            <tr>
                                <!-- <td width="20%" valign="top" class="title">Client Assign</td> -->
		                        <td align="center">			        
			                        <asp:DropDownList ID="ddlTeamCode" CssClass="normal" runat="server" 
                                        AutoPostBack="True" onselectedindexchanged="ddlTeamCode_SelectedIndexChanged"></asp:DropDownList> &nbsp;&nbsp;
                                    <b>(Please select team first before searching clients.)</b>                                
		                        </td>
	                        </tr>	                
                        </table> <br />  
                        <asp:HiddenField ID="hfTeamCode" runat="server" />             
                            <asp:DataList ID="dlstDealer" runat="server" RepeatColumns="10">                                
                                <ItemTemplate>                                    
                                    <table width="100%" border="1" bordercolor="#CDCDCD" cellspacing="0" class="normal">
                                        <tr bgcolor="#FFFFCC" class="normalGrey">      <!--  align="center"  -->
                                            <td align="left">
                                                <asp:Label ID="dlstlblSelect" Visible="false" Text="" runat="server" />
                                            </td>
                                            <td align="center">
                                                <asp:CheckBox ID="dlstchkAutoSelect" runat="server" />
                                            </td>
                                        </tr>
                                        <tr bgcolor="#FFFFCC" class="normalGrey">
                                            <td align="left">
                                                <asp:Label ID="dlstlblDealerTitle" Visible="false"
                                                    Text="Dealer Code:&nbsp;&nbsp;&nbsp;" runat="server" /> 
                                            </td>
                                            <td align="center">
                                                <asp:Label ID="dlstlblDealerCode"  runat="server" 
                                                    Text='<%#  DataBinder.Eval(Container.DataItem,"AECode")  %>'/>
                                            </td>
                                        </tr>
                                        <tr bgcolor="#EEEEEE" class="normalGrey">
                                            <td align="left">
                                                <asp:Label ID="dlstlblDealerNameTitle" Visible="false"
                                                    Text="Dealer Name:&nbsp;&nbsp;&nbsp;" runat="server" /> 
                                            </td>
                                            <td align="center">
                                                <asp:Label ID="dlstlblDealerName" 
                                                    Text='<%#  DataBinder.Eval(Container.DataItem,"AEName")  %>'  runat="server"/>
                                            </td>
                                        </tr>
                                        <tr bgcolor="#FFFFCC" class="normalGrey">
                                            <td align="left">
                                                <asp:Label ID="dlstlblAssignCount" Visible="false"
                                                    Text="Assigned:&nbsp;&nbsp;&nbsp;" runat="server" /> 
                                            </td>
                                            <td align="center">
                                                <asp:Label ID="dlstlblCurrentAssign"  runat="server"
                                                    Text='<%#  DataBinder.Eval(Container.DataItem,"CurrentAssign")  %>'/>
                                            </td>
                                        </tr>
                                    </table>                         
                                </ItemTemplate>
                            </asp:DataList>
                            <br />               
                    </fieldset>               
                </div>                                                                                                
                
                <div id="divSearchClient" align="center">
                    <fieldset style="width:80%;border:1px solid gray;">   <!-- width:630px; -->
                        <legend style="font-family: Arial,Helvetica,sans-serif; font-size:11px; font-weight:bold;padding: 0 0 10px;">Search Clients</legend>
                        <table cellpadding="0" cellspacing="0" width="100%" border="0" class="normalGrey">                    
			                <tr class="normal" align="left">				        
				                <td>				    				           
                                    A/C Open Date from &nbsp;
                                    <ucc:CoolCalendar ID="calAcctFromDate" TimeRequired="false" DateTimeFormat="dd/MM/yyyy" 
                                        DateTextRequired="false" DateTextFromValue="" runat="server" />
                                    To &nbsp; 
                                     <ucc:CoolCalendar ID="calAcctToDate" TimeRequired="false" DateTimeFormat="dd/MM/yyyy"
                                        DateTextRequired="false" DateTextFromValue="" runat="server" />
                                    Cut off DateTime: &nbsp;                             
                                    <ucc:CoolCalendar ID="calCutOffDate" TimeRequired="true" DateTimeFormat="dd/MM/yyyy HH:mm:ss"
                                        DateTextRequired="false" DateTextFromValue="" runat="server" />                            
				                </td>				        
			                </tr>
			                 <tr class="normal">
			                    <td>
			                     &nbsp;
			                    </td>
			                </tr>	
			                <tr class="normal" align="left">				        
				                <td>	
				                   <asp:CheckBox ID="chkConHistory" Text="No Contact History for at least " 
                                        CssClass="normal" runat="server" Enabled="False" AutoPostBack="True" 
                                        oncheckedchanged="chkConHistory_CheckedChanged" />	
                                   <asp:TextBox ID="txtConHistory" runat="server" Width="25px" Enabled="False" CssClass="normal"></asp:TextBox>	
                                   	    				           
                                    months &nbsp;
                                   <asp:CheckBox ID="chkTrustBal" Text="With Funds in Trust Account $ " 
                                        CssClass="normal" runat="server" Enabled="False" AutoPostBack="True" 
                                        oncheckedchanged="chkTrustBal_CheckedChanged" />	
                                   <asp:TextBox ID="txtTrustBal" runat="server" Width="80px" Enabled="False" CssClass="normal"></asp:TextBox>	
                                    &nbsp; 
                                   <asp:CheckBox ID="chkMMFBal" Text="MMF with at least $ " CssClass="normal" 
                                        runat="server" Enabled="False" AutoPostBack="True" 
                                        oncheckedchanged="chkMMFBal_CheckedChanged" style="display: none" />	
                                   <asp:TextBox ID="txtMMFBal" runat="server" Width="80px" Enabled="False" CssClass="normal" style="display: none"></asp:TextBox>
				                </td>
				                <td rowspan="5">
                                    <asp:ListBox ID="lstAccountTypes" runat="server" AppendDataBoundItems="True" 
                                        AutoPostBack="True" CssClass="normal" Height="135px" 
                                        SelectionMode="Multiple" 
                                        onselectedindexchanged="lstAccountTypes_SelectedIndexChanged">                                    
                                        <asp:ListItem Value="CA">Cash Trading</asp:ListItem>
                                        <asp:ListItem Value="CFD">CFD</asp:ListItem>
                                        <asp:ListItem Value="CU">Custodian</asp:ListItem>
                                        <asp:ListItem Value="KC">Cash Management</asp:ListItem>
                                        <asp:ListItem Value="M">Phillip Margin</asp:ListItem>
                                        <asp:ListItem Value="PFN">Phillip Financial</asp:ListItem>
                                        <asp:ListItem Value="S2">Discretionary Accounts</asp:ListItem>
                                        <asp:ListItem Value="UT">Unit Trust Non-Wrap</asp:ListItem>
                                        <%--<asp:ListItem Value="UTW">Advisory Accounts</asp:ListItem>                                    --%>
                                    </asp:ListBox>                             
				                </td>				        
			                </tr>		
			                 <tr class="normal">
			                    <td>
			                     &nbsp;
			                    </td>
			                </tr>	               
			                  <tr class="normal">				        
				                <td align="left">	
				                   <asp:CheckBox ID="chkTPeriod" Text="No trade for at least " CssClass="normal" 
                                        runat="server" Enabled="False" AutoPostBack="True" 
                                        oncheckedchanged="chkTPeriod_CheckedChanged" /> 
                                   <asp:TextBox ID="txtTPeriod" runat="server" Width="25px" Enabled="False" CssClass="normal"></asp:TextBox>				    				           
                                    months&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;
                                   <asp:CheckBox ID="chkSMarketVal" Text="Stock Market Value At least $ " 
                                        CssClass="normal" runat="server" Enabled="False" AutoPostBack="True" 
                                        oncheckedchanged="chkSMarketVal_CheckedChanged" /> 	
                                   <asp:TextBox ID="txtSMarketVal" runat="server" Width="80px" Enabled="False" CssClass="normal"></asp:TextBox>
				                </td>				                			        
			                </tr>		
			                 <tr class="normal">
			                    <td>
			                     &nbsp;
			                    </td>
			                </tr>	
			                <tr class="normal" align="left">
                                <td>                                
			                        <%--<asp:CheckBox ID="chk2NOnly" Text="2N Client Only" Checked="true" CssClass="normal" runat="server" />--%> &nbsp;
					                <asp:CheckBox ID="chkEmail" Text="Email" CssClass="normal" runat="server" /> &nbsp;
					                <asp:CheckBox ID="chkMobile" Text="Mobile" CssClass="normal" runat="server" /> &nbsp;
					                <asp:CheckBox ID="chkAutoAssign" Text="Auto Assign" CssClass="normal" runat="server" />   &nbsp;&nbsp;&nbsp;
					                <asp:Button ID="btnSearch" Text="Search" CssClass="normal" runat="server" 
		                                OnClick="btnSearch_Click" />		                            
                                </td>
                            </tr>
                             <tr class="normal">
			                    <td>
			                     &nbsp;
			                    </td>
			                </tr>	
		                </table>
		                 <asp:Panel ID="pnlProjectInfo" runat="server" Visible="false">
                           <fieldset style="width:98%;border:1px solid gray;">   <!-- width:630px; -->
                           <legend style="font-family: Arial,Helvetica,sans-serif; font-size:11px; font-weight:bold;padding: 0 0 10px;">Project Info</legend>
                           <table width="100%">
                             <tr>
                                 <td>
                                    <table cellpadding="0" cellspacing="0" border="0" class="normalGrey">
                                        <tr class="normal" align="left">
			                                <td class="style1">&nbsp; Project Name :<asp:TextBox ID="txtProName" runat="server" Width="300px" CssClass="normal"></asp:TextBox>&nbsp;                                                 
                                                <asp:Label ID="lblReqField" runat="server" Text="*" ForeColor="Red" Visible="false" CssClass="normal"></asp:Label>
                                            </td>
			                            </tr>
			                            <tr class="normal">
			                                <td class="style1"> &nbsp; </td>
			                            </tr>
			                            <tr class="normal" align="left">                                
			                                <td class="style1"> &nbsp; Project Objective:<asp:TextBox ID="txtProObj" runat="server" Width="300px" CssClass="normal"></asp:TextBox>&nbsp; </td>
			                            </tr>
			                            <tr class="normal">
			                                 <td class="style1"> &nbsp; </td>
			                            </tr>
			                            <tr class="normal" align="left">
			                                <td class="style1"> &nbsp; Select file to upload(.html): &nbsp;
			                                        <asp:FileUpload ID="fileUpload" runat="server" class="normal" />&nbsp;&nbsp;
                                                    <asp:Label ID="Label1" runat="server" Text="The promotion letter must be HTML format." ForeColor="Red"></asp:Label>&nbsp;
                                                    <br /><br />
                                                    <asp:Button ID="btnUpload" runat="server" Text="Upload EmailTemplate" class="normal" 
                                                    onclick="btnUpload_Click" />
                                                    &nbsp;&nbsp;<asp:Button ID="btnUploadAttachment" runat="server" class="normal" 
                                                    Text="Upload Attachment" onclick="btnUploadAttachment_Click" Enabled="false" />
                                            </td>			                        
			                            </tr>
			                            <tr class="normal" align="left">
			                                <td>
			                                    <fieldset>
			                                        <legend>Upload Image</legend>
			                                        <asp:FileUpload ID="fpImage" runat="server" class="normal"  /><br /><br />
                                                    <asp:Button ID="btnUploadImage" runat="server" Text="Upload Image" onclick="btnUploadImage_Click" class="normal"
                                                        onmouseover="showTooltip(t1, 'Instructions to upload the image')" onmouseout="hideTooltip(t1)" />
                                                        <div id="t1" style="display: none"></div>
                                                    <asp:Label ID="labImageList" runat="server"></asp:Label><br />
                                                    <asp:LinkButton ID="lbtnClearImageList" runat="server" onclick="lbtnClearImageList_Click">Clear Image List</asp:LinkButton>
			                                    </fieldset>
			                                </td>
			                            </tr>	
			                            <tr class="normal">
			                                 <td class="style1"> 
                                                 <asp:Label ID="lblFileName" runat="server" Text="" Visible="false"></asp:Label>&nbsp; </td>
			                            </tr>		                           
			                      </table>
                                  </td>
                              <td>
                                  <asp:Panel ID="pnlFileView" runat="server" Visible="false">
                                      <asp:GridView ID="gvEmailFiles" CssClass="normal" AutoGenerateColumns="False" 
                                        runat="server" OnRowDeleting="gvEmailFiles_RowDeleting"
                                        AllowPaging="True" PagerSettings-Visible="false" 
                                          onrowdatabound="gvEmailFiles_RowDataBound" >
                                        <HeaderStyle BackColor="#CDCDCD" />
                                        <PagerSettings Visible="False" />
                                        <RowStyle CssClass="normal" BackColor="White" BorderColor="#777777" />
                                        <alternatingrowstyle CssClass="normal" BackColor="#EEEEEE" BorderColor="#777777" />
                                        <Columns>
                                            <asp:BoundField DataField="Files" HeaderText="FileName">
                                                <HeaderStyle CssClass="grayBorder" />
                                                <ItemStyle CssClass="grayBorder" />
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="FileType">
                                                <ItemTemplate>
                                                    <asp:Label ID="labFileType" runat="server" Text='<%# Eval("FileType") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="grayBorder" />
                                                <ItemStyle CssClass="grayBorder" Width="30px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField ShowHeader="False">
                                                <ItemTemplate>
                                                    <asp:Button ID="gvbtnDelete" runat="server" CausesValidation="False" CommandName="Delete"
                                                        Text="Remove" />
                                                </ItemTemplate>
                                                <ControlStyle CssClass="normal" />
                                                <HeaderStyle CssClass="grayBorder" />
                                                <ItemStyle CssClass="grayBorder" Width="100px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:Button ID="gvbtnPreview" runat="server" CausesValidation="False" Text="Preview" CssClass="normal"
                                                        OnClick="gvbtnPreview_Click" />
                                                    <asp:HiddenField ID="hdfFiles" runat="server" Value='<%# Eval("Files") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                      </asp:GridView>
                                     </asp:Panel>                                
                              </td>
                             </tr>                           
                           </table>
                           </fieldset>
                           <br />
                           </asp:Panel>
                    </fieldset>                    
		        </div>	      
                <br />
                
		        <!-- Colors for CoreClient, Assigned, 2NClient -->
		        <div align="center">
		            <table border="1" bordercolor="#777777" cellspacing="0" cellpadding="0" class="normalGrey">
			            <tr class=normal align="center">
				            <td bgcolor="#FFCC99" width="150px">Core Client
                            </td>
				            <td bgcolor="#FFFF99" width="150px">Assigned</td>
				            <%--<td bgcolor="#FFCCFF" width="150px">2N Client</td>--%>
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
                                   
                <asp:GridView ID="gvClientAssign" CssClass="normal" AutoGenerateColumns="False" 
                    runat="server" OnRowDeleting="gvClientAssign_RowDeleting"
                    onrowdatabound="gvClientAssign_RowDataBound" AllowPaging="true"
                    PagerSettings-Visible="false" AllowSorting="True" 
                    onsorting="gvClientAssign_Sorting">
                    <HeaderStyle BackColor="#CDCDCD" />
                    <PagerSettings Visible="False" />
                    <RowStyle CssClass="normal" BackColor="White" BorderColor="#777777" />
                    <alternatingrowstyle CssClass="normal" BackColor="#EEEEEE" BorderColor="#777777" />
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:CheckBox ID="gvchkSelectAll" runat="server" AutoPostBack="true"  />
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
                                <br />                              
                            </ItemTemplate>
                            <HeaderStyle BorderColor="#777777" CssClass="grayBorder" Width="30px" />
                            <ItemStyle BorderColor="#777777" CssClass="grayBorder" Width="30px" />
                        </asp:TemplateField> 
                        <asp:BoundField  DataField="Email" Visible="false" />               
                        <asp:BoundField DataField="AcctNo" HeaderText="A/C." SortExpression="AcctNo">
                            <HeaderStyle CssClass="grayBorder" />
                            <ItemStyle CssClass="grayBorder" Width="50px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="ClientName" HeaderText="Name" SortExpression="ClientName">
                            <HeaderStyle CssClass="grayBorder" />
                            <ItemStyle CssClass="grayBorder" Width="110px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="StockMarket" HeaderText="Stock Mart" SortExpression="StockMarket">
                            <HeaderStyle CssClass="grayBorder" />
                            <ItemStyle CssClass="grayBorder" Width="30px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Balance" HeaderText="Balance" SortExpression="Balance">
                            <HeaderStyle CssClass="grayBorder" />
                            <ItemStyle CssClass="grayBorder" Width="50px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Equity" HeaderText="Equity" SortExpression="Equity">
                            <HeaderStyle CssClass="grayBorder" />
                            <ItemStyle CssClass="grayBorder" Width="50px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="AUM" HeaderText="AUM" SortExpression="AUM">
                            <HeaderStyle CssClass="grayBorder" />
                            <ItemStyle CssClass="grayBorder" Width="50px" />
                        </asp:BoundField>
                         <asp:BoundField DataField="MMF" HeaderText="MMF" SortExpression="MMF">
                            <HeaderStyle CssClass="grayBorder" />
                            <ItemStyle CssClass="grayBorder" Width="50px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="ACOpen" HeaderText="A/C Open" SortExpression="ACOpen" dataformatstring="{0:dd/MM/yyyy}" 
                                HtmlEncode="false">
                            <HeaderStyle CssClass="grayBorder" />
                            <ItemStyle CssClass="grayBorder" Width="30px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="LastTradeDate" HeaderText="Last Trans" SortExpression="LastTradeDate" dataformatstring="{0:dd/MM/yyyy}" 
                                HtmlEncode="false">
                            <HeaderStyle CssClass="grayBorder" />
                            <ItemStyle CssClass="grayBorder" Width="30px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Status" HeaderText="S" SortExpression="Status">
                            <HeaderStyle CssClass="grayBorder" />
                            <ItemStyle CssClass="grayBorder" Width="10px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="TeamCode" HeaderText="TR" SortExpression="TeamCode">
                            <HeaderStyle CssClass="grayBorder" />
                            <ItemStyle CssClass="grayBorder" Width="30px" />
                        </asp:BoundField>
                         <asp:BoundField DataField="AltAECode" HeaderText="AE Code" SortExpression="FACode">
                            <HeaderStyle CssClass="grayBorder" />
                            <ItemStyle CssClass="grayBorder" Width="30px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="CoreAECode" HeaderText="Core" SortExpression="CoreAECode">
                            <HeaderStyle CssClass="grayBorder" />
                            <ItemStyle CssClass="grayBorder" Width="30px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="TotalCall" HeaderText="Total Call" SortExpression="TotalCall">
                            <HeaderStyle CssClass="grayBorder" />
                            <ItemStyle CssClass="grayBorder" Width="30px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Rank" HeaderText="Last Rank" SortExpression="Rank">
                            <HeaderStyle CssClass="grayBorder" />
                            <ItemStyle CssClass="grayBorder" Width="30px" />
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
                             <HeaderStyle BorderColor="#777777" CssClass="grayBorder" />
                             <ItemStyle BorderColor="#777777" CssClass="grayBorder" />
                        </asp:TemplateField>                       
                    </Columns>
                </asp:GridView>    <br />
                 
                 <div id="divPaging" class="normalGrey" runat="server">
                    <ucc:PagingControl ID="pgcClientAssign" OnPageNoChanged="PagingControl_PageNoChange" OnRowPerPageChanged="PagingControl_RowPerPageChanged"
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
                
          <%#DataBinder.Eval(Container, "DataItem.AECode")%>
        <br />
    <%--</ContentTemplate>
    </asp:UpdatePanel>--%>
    </form>
    </div>
    
    
</body>
</html>

<script language="javascript" type="text/javascript" >
    function showTooltip(div, title) {
        div.style.display = 'inline';
        div.style.position = 'absolute';
        div.style.width = '600px';
        div.style.backgroundColor = '#EFFCF0';
        div.style.border = 'dashed 1px black';
        div.style.padding = '10px';
        div.innerHTML = '<b>' + title + '</b><div style="padding-left:10; padding-right:5; font-family:Courier New">The uploaded image must be .jpg and .png format.</br>' +
        'Put the image file name in promotion email template (html).</br>' +
        'If the image name is Logo.jpg then it should put ***image_logo_image*** in html file.</br>' +
        'File name must be continuous and cannot include space, underscore, etc.</br>' +
        'Type the file name in html file and do not copy it.</br>' +
        'System will replace uploaded image with the image file name in html file.' + '</div>';
    }

    function hideTooltip(div) {
        div.style.display = 'none';
    }
</script>