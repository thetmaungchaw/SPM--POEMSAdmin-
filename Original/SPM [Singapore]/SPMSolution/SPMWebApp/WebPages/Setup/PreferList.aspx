<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PreferList.aspx.cs" Inherits="SPMWebApp.WebPages.Setup.PreferList" %>

<%@ Register Src="~/PagingControl.ascx" TagName="PagingControl" TagPrefix="ucc" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>SPM - Preference List Setup</title>
    <link rel="stylesheet" href="<%=ConfigurationManager.AppSettings["SPMCssFile"]%>" type="text/css" />
</head>
<body>
    <div id="Container">
    <form id="frmPreferenceListSetup" runat="server">    
        <div class="title">Preference List Setup</div>
        
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
            
            //Move to Top when GridView modify button click
            function ScrollToTop()
            {
                window.scrollTo(0, 0);
            }
            
        </script>
        
        <br />
        
        <asp:UpdatePanel ID="upnlPreferList" runat="server">
            <ContentTemplate>
                <table width="50%" border="0" cellspacing="0" cellpadding="0" class="normalGrey">
			        <tr bgcolor="#ffcccc" class="normal" align="center">
				        <td width="10">&nbsp;</td>
				        <td width="20">Index</td>
				        <td width="80">				    
				            <asp:TextBox ID="txtIndex" CssClass="normal" MaxLength="2" runat="server" Width="30px" />
				        </td>
				        <td width="10">&nbsp;</td>
				        <td width="20">Content</td>
				        <td width="80">
				            <asp:TextBox ID="txtContent" CssClass="normal" MaxLength="20" runat="server"></asp:TextBox>
				        </td>
				        <td width="50">
				            <asp:Button ID="btnUpdate" CssClass="normal" Text="Update" runat="server" OnClick="btnUpdate_Click"/>
				            <asp:HiddenField ID="hdfModifyIndex" runat="server" />
				        </td>
				        <td width="50">
				            <asp:Button ID="btnCancel" CssClass="normal" Text="Cancel" runat="server" OnClick="btnCancel_Click"/>
				        </td>
				        <td width="50">
				            <asp:Button ID="btnAdd" CssClass="normal" Text="Add" runat="server" OnClick="btnAdd_Click"/>
				        </td>			
				        <td width="50">
				            <asp:Button ID="btnSearch" CssClass="normal" Text="Search" runat="server" 
				                OnClick="btnSearch_Click"/>				            
				        </td>			
			        </tr>
		        </table>
                <br />
                <div id="divMessage" class="normalRed" runat="server"></div>
                <br />
                <asp:GridView ID="gvPreferList" CssClass="normalGrey" 
                    AutoGenerateColumns="False" BorderColor="#777777" PagerSettings-Visible="false"
                       DataKeyNames="RecId" OnRowEditing="gvList_RowEditing" 
                    OnRowDeleting="gvListDelete_RowDeleting" runat="server" AllowPaging="True" 
                    OnPageIndexChanging="gvPreferList_PageIndexChanging" PageSize="20" 
                    onrowdatabound="gvPreferList_RowDataBound">
                    <HeaderStyle BackColor="#CDCDCD" />
                    <PagerSettings FirstPageText="First Page" LastPageText="Last Page" 
                        Mode="NextPreviousFirstLast" NextPageText="Next Page" 
                        PreviousPageText="Prev Page" />
                    <RowStyle CssClass="normal" BackColor="White" BorderColor="#777777" />
                    <alternatingrowstyle CssClass="normal" BackColor="#EEEEEE" BorderColor="#777777" />
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                No.
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                            <HeaderStyle BorderColor="#777777" />
                            <ItemStyle BorderColor="#777777" CssClass="grayBorder" Width="50px" />
                        </asp:TemplateField>                
                        <asp:BoundField DataField="OptionNo" HeaderText="Index">
                            <HeaderStyle CssClass="grayBorder" />
                            <ItemStyle CssClass="grayBorder" Width="80px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="OptionContent" HeaderText="Content">
                            <HeaderStyle CssClass="grayBorder" />
                            <ItemStyle CssClass="grayBorder" Width="200px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="ModifiedUser" HeaderText="Modified User">
                            <HeaderStyle CssClass="grayBorder" />
                            <ItemStyle CssClass="grayBorder" Width="120px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="ModifiedDate" HeaderText="Modified Date" dataformatstring="{0:dd/MM/yyyy HH:mm:ss}" HtmlEncode="false">
                            <HeaderStyle CssClass="grayBorder" />
                            <ItemStyle CssClass="grayBorder" Width="120px" />
                        </asp:BoundField>
                        <asp:TemplateField ShowHeader="False">
                            <EditItemTemplate>
                                <asp:Button ID="Button1" runat="server" CausesValidation="True" 
                                    CommandName="Update" Text="Update" />
                                &nbsp;<asp:Button ID="Button2" runat="server" CausesValidation="False" 
                                    CommandName="Cancel" Text="Cancel" />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Button ID="Button1" runat="server" CausesValidation="False" 
                                    CommandName="Edit" Text="Modify" OnClientClick="ScrollToTop()" />
                                &nbsp;<asp:Button ID="Button2" runat="server" CausesValidation="False" 
                                    CommandName="Delete" Text="Delete" />
                            </ItemTemplate>
                            <ControlStyle CssClass="normal" />
                            <HeaderStyle CssClass="grayBorder" />
                            <ItemStyle CssClass="grayBorder" Width="150px" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView> <br />
                <div id="divPaging" class="normal" runat="server">
                    <ucc:PagingControl ID="pgcPreferList" OnPageNoChanged="PagingControl_PageNoChange" OnRowPerPageChanged="PagingControl_RowPerPageChanged"
                        runat="server"/>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <br />    
    </form>
    </div>
</body>
</html>
