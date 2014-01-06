<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CoreClient.aspx.cs" Inherits="SPMWebApp.WebPages.Setup.CoreClient" %>

<%@ Register Src="~/PagingControl.ascx" TagName="PagingControl" TagPrefix="ucc" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>SPM - Core Client Setup</title>
    <link rel="stylesheet" href="<%=ConfigurationManager.AppSettings["SPMCssFile"]%>" type="text/css" />
</head>
<body>
    <div id="Container">
    <form id="frmCoreClientSetup" runat="server">    
        <div class="title">Core Client Setup</div>
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
        
                               
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
            
            <table width="50%" border="0" cellspacing="0" cellpadding="0" class="normalGrey">
			    <tr bgcolor="#ffcccc" class="normal" align="center">
				    <td>Dealer Code</td>
				    <td align="right">
				        <asp:DropDownList ID="ddlDealer" CssClass="normal" runat="server" />
				    </td>				
				    <td class="normalBlue"></td>
				    <td>A/C</td>
				    <td>
				        <asp:TextBox ID="txtAccNo" CssClass="normal" MaxLength="15" runat="server"></asp:TextBox>
				    </td>
				    <td>
				        <asp:Button ID="btnAdd" CssClass="normal" Text="Add" runat="server" OnClick="btnAdd_Click"/>
				    </td>
				    <td>
				        <asp:Button ID="btnSearch" CssClass="normal" Text="Search" runat="server" 
				            OnClick="btnSearch_Click"/>
				    </td>				
			    </tr>
		    </table>
		    <br />                        
            
            <div id="divMessage" class="normalRed" runat="server">		
		    </div>
            <asp:GridView ID="gvList" CssClass="normalGrey" BorderColor="#777777" AutoGenerateColumns="False"
                CellPadding="0" datakeynames="RecId" runat="server" 
                    OnRowDeleting="gvListDelete_RowDeleting" 
                    onrowdatabound="gvList_RowDataBound">            
                    <HeaderStyle BackColor="#CDCDCD" />            
                    <RowStyle CssClass="normal" BackColor="White" BorderColor="#777777" />
                    <alternatingrowstyle CssClass="normal" BackColor="#EEEEEE" BorderColor="#777777" />
                    <Columns>
                        <asp:TemplateField ItemStyle-BorderColor="#777777" ItemStyle-Width="40px"
                                ItemStyle-HorizontalAlign="Center" HeaderStyle-BorderColor="#777777">
                            <HeaderTemplate>
                                No.
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                            <HeaderStyle BorderColor="#777777" />
                            <ItemStyle BorderColor="#777777" HorizontalAlign="Center" Width="40px" />
                        </asp:TemplateField>                
                        <asp:BoundField DataField="AECode" HeaderText="Dealer Code" ItemStyle-BorderColor="#777777" 
                            HeaderStyle-BorderColor="#777777" ItemStyle-Width="80px" 
                            ItemStyle-HorizontalAlign="Center">
                            <HeaderStyle BorderColor="#777777" />
                            <ItemStyle BorderColor="#777777" HorizontalAlign="Center" Width="80px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="AEName" HeaderText="Dealer Name" ItemStyle-BorderColor="#777777"
                            HeaderStyle-BorderColor="#777777" ItemStyle-Width="120px" 
                            ItemStyle-HorizontalAlign="Center">
                            <HeaderStyle BorderColor="#777777" />
                            <ItemStyle BorderColor="#777777" HorizontalAlign="Center" Width="120px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="AcctNo" HeaderText="A/C" ItemStyle-BorderColor="#777777"
                            HeaderStyle-BorderColor="#777777" ItemStyle-Width="100px" 
                            ItemStyle-HorizontalAlign="Center">
                            <HeaderStyle BorderColor="#777777" />
                            <ItemStyle BorderColor="#777777" HorizontalAlign="Center" Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="ModifiedUser" HeaderText="Modified By" ItemStyle-BorderColor="#777777"
                            HeaderStyle-BorderColor="#777777" ItemStyle-Width="100px" 
                            ItemStyle-HorizontalAlign="Center">
                            <HeaderStyle BorderColor="#777777" />
                            <ItemStyle BorderColor="#777777" HorizontalAlign="Center" Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="ModifiedDate" HeaderText="Modified At" ItemStyle-BorderColor="#777777"
                            HeaderStyle-BorderColor="#777777" ItemStyle-Width="120px" 
                            ItemStyle-HorizontalAlign="Center">
                            <HeaderStyle BorderColor="#777777" />
                            <ItemStyle BorderColor="#777777" HorizontalAlign="Center" Width="120px" />
                        </asp:BoundField>
                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                <asp:Button ID="Button1" runat="server" CausesValidation="False" 
                                    CommandName="Delete" Text="Delete" />
                            </ItemTemplate>
                            <ControlStyle CssClass="normal" />
                            <HeaderStyle BorderColor="#777777" />
                            <ItemStyle BorderColor="#777777" HorizontalAlign="Center" Width="100px" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView> <br />
                <div id="divPaging" class="normal" runat="server">
                    <ucc:PagingControl ID="pgcCoreClient" OnPageNoChanged="PagingControl_PageNoChange" OnRowPerPageChanged="PagingControl_RowPerPageChanged"
                        runat="server"/>
                </div>
            </ContentTemplate>            
        </asp:UpdatePanel>
        <br />    
    </form>
    </div>
</body>
</html>
