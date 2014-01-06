<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ClientShortKey.aspx.cs" Inherits="SPMWebApp.WebPages.Setup.ClientShortKey" %>

<%@ Register Src="~/PagingControl.ascx" TagName="PagingControl" TagPrefix="ucc" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>SPM - Client Short Key Setup</title>
    <link rel="stylesheet" href="<%=ConfigurationManager.AppSettings["SPMCssFile"]%>" type="text/css" />
</head>
<body>
    <div id="Container">
    <form id="frmShortKeySetup" runat="server">
    
        <div class="title">Client Short Key Setup</div>
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
        
        
        <br />        
        <asp:UpdatePanel ID="upnlClientShortKey" runat="server">
            <ContentTemplate>
                <table border="0" cellspacing="0" cellpadding="0" class="normalGrey">
			        <tr bgcolor="#ffcccc" class="normal" align="center">
				        <td>&nbsp;</td>
				        <td>A/C:</td>
				        <td>				    
				            <asp:TextBox ID="txtAccountNo" CssClass="normal" runat="server"/>
				        </td>
				        <td>&nbsp;</td>
				        <td>Short Key:</td>
				        <td>
				            <asp:TextBox ID="txtShortKey" CssClass="normal" MaxLength="20" runat="server"></asp:TextBox>
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
                <div id="divMessage" class="normalRed" runat="server"></div>
                <br />
                
                <asp:GridView ID="gvClientShortKey" CssClass="normalGrey" 
                    AutoGenerateColumns="False" BorderColor="#777777" 
                        DataKeyNames="AcctNo" AllowPaging="True" PageSize="20" 
                    OnRowDeleting="gvListDelete_RowDeleting" runat="server" 
                    onrowdatabound="gvClientShortKey_RowDataBound">
                    <HeaderStyle BackColor="#CDCDCD" />
                    <RowStyle CssClass="normal" BackColor="White" BorderColor="#777777" />
                    <alternatingrowstyle CssClass="normal" BackColor="#EEEEEE" BorderColor="#777777" />
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                No.
                            </HeaderTemplate>
                            <ItemTemplate>                                
                                <asp:Label ID="gvlblRowNo" Text='<%# Container.DataItemIndex + 1 %>' CssClass="normal" runat="server" />
                            </ItemTemplate>
                            <HeaderStyle BorderColor="#777777" />
                            <ItemStyle BorderColor="#777777" CssClass="grayBorder" Width="50px" />
                        </asp:TemplateField>                
                        <asp:BoundField DataField="AcctNo" HeaderText="A/C">
                            <HeaderStyle CssClass="grayBorder" />
                            <ItemStyle CssClass="grayBorder" Width="90px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="LNAME" HeaderText="Name">
                            <HeaderStyle CssClass="grayBorder" />
                            <ItemStyle CssClass="grayBorder" Width="180px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="ShortKey" HeaderText="Short Key">
                            <HeaderStyle CssClass="grayBorder" />
                            <ItemStyle CssClass="grayBorder" Width="100px" />
                        </asp:BoundField>
                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                <asp:Button ID="Button1" runat="server" CausesValidation="False" 
                                    CommandName="Delete" Text="Delete" />
                            </ItemTemplate>
                            <ControlStyle CssClass="normal" />
                            <HeaderStyle BorderColor="#777777" />
                            <ItemStyle BorderColor="#777777" HorizontalAlign="Center" Width="90px" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView> <br />
                <div id="divPaging" class="normal" runat="server">
                    <ucc:PagingControl ID="pgcShortKey" OnPageNoChanged="PagingControl_PageNoChange" OnRowPerPageChanged="PagingControl_RowPerPageChanged"
                        runat="server"/>
                   
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>    
    </form>
    </div>
</body>
</html>
