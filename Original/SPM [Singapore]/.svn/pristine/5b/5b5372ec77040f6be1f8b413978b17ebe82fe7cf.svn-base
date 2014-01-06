<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CrossTeamSetup.aspx.cs" Inherits="SPMWebApp.WebPages.Setup.CrossTeamSetup" %>

<%@ Register Src="~/PagingControl.ascx" TagName="PagingControl" TagPrefix="ucc" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>SPM - Cross Team Setup</title>
    <link rel="stylesheet" href="<%=ConfigurationManager.AppSettings["SPMCssFile"]%>" type="text/css" />
</head>
<body>
    <div id="Container">
    <form id="frmCrossTeamSetup" runat="server">    
        <div class="title">Cross Team Setup</div>
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
        
        
        <asp:UpdatePanel ID="uplCrossTeamSetup" runat="server">
            <ContentTemplate>
                <table>
                    <tr class="normal">     <!-- total cells: 14 -->
                        <td>Dealer:</td>
                        <td>
                            <asp:DropDownList ID="ddlDealer" CssClass="normal" runat="server" />
                        </td>                                                
                        <td>&nbsp;</td>
                        <td>Original Team: </td>
                        <td>
                            <asp:DropDownList ID="ddlTeamCode" CssClass="normal" runat="server" />
                        </td>
                        <td>&nbsp;</td>
                        <td>Match Team: </td>
                        <td>
                            <asp:DropDownList ID="ddlCrossTeamCode" CssClass="normal" runat="server" />
                        </td>
                        <td>
                            &nbsp;
                            <asp:Button ID="btnSearch" Text="Search" onclick="btnSearch_Click" CssClass="normal" runat="server" />                            
                        </td>
                    </tr>          
                </table>
                <div id="divMessage" class="normalRed" runat="server"></div> <br />
                <asp:GridView ID="gvCommonDealer" CssClass="normal" AllowPaging="True" 
                    PageSize="20" PagerSettings-Visible="false" BorderColor="#777777" runat="server"
                        DataKeyNames="AECode" AutoGenerateColumns="False" 
                    onrowdatabound="gvCommonDealer_RowDataBound" 
                    onrowediting="gvCommonDealer_RowEditing"
                    >
                    <HeaderStyle BackColor="#CDCDCD" />
                    <PagerSettings Visible="False" />
                    <RowStyle CssClass="normal" BackColor="White" BorderColor="#777777" />
                    <alternatingrowstyle CssClass="normal" BackColor="#EEEEEE" BorderColor="#777777" />
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                No.
                            </HeaderTemplate>
                            <ItemTemplate>                  
                                <%# Container.DataItemIndex + 1 %>
                                <asp:HiddenField ID="gvhdfItemIndex" Value='<%# Container.DataItemIndex %>' runat="server" />
                            </ItemTemplate>
                            <HeaderStyle BorderColor="#777777" />
                            <ItemStyle BorderColor="#777777" CssClass="grayBorder" Width="50px" />
                        </asp:TemplateField>                
                        <asp:BoundField DataField="AECode" HeaderText="Dealer Code">
                            <HeaderStyle CssClass="grayBorder" />
                            <ItemStyle CssClass="grayBorder" Width="90px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="AEName" HeaderText="Name">
                            <HeaderStyle CssClass="grayBorder" />
                            <ItemStyle CssClass="grayBorder" Width="160px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Original" HeaderText="Original Team">
                            <HeaderStyle CssClass="grayBorder" />
                            <ItemStyle CssClass="grayBorder" Width="100px" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Match Team">                           
                            <ItemTemplate>                  
                                <asp:DropDownList ID="gvddlMatch" CssClass="normal" runat="server" />
                            </ItemTemplate>
                            <HeaderStyle BorderColor="#777777" />
                            <ItemStyle BorderColor="#777777" CssClass="grayBorder" Width="100px" />
                        </asp:TemplateField>
                        <asp:TemplateField ShowHeader="False">
                            <EditItemTemplate>
                                <asp:Button ID="Button1" runat="server" CausesValidation="True" 
                                    CommandName="Update" Text="Update" />
                                &nbsp;<asp:Button ID="Button2" runat="server" CausesValidation="False" 
                                    CommandName="Cancel" Text="Cancel" />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Button ID="Button1" runat="server" CausesValidation="False" 
                                    CommandName="Edit" Text="Change" />
                            </ItemTemplate>
                            <ControlStyle CssClass="normal" />
                            <HeaderStyle CssClass="grayBorder" />
                            <ItemStyle CssClass="grayBorder" Width="80px" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="modifiedUser" HeaderText="Modified User">
                            <HeaderStyle CssClass="grayBorder" />
                            <ItemStyle CssClass="grayBorder" Width="80px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="modifiedDate" HeaderText="Modified Date" dataformatstring="{0:dd/MM/yyyy HH:mm:ss}" 
                                HtmlEncode="false">
                            <HeaderStyle CssClass="grayBorder" />
                            <ItemStyle CssClass="grayBorder" Width="120px" />
                        </asp:BoundField>
                    </Columns>
                </asp:GridView> <br />
                <div id="divPaging" class="normal" runat="server">
                    <ucc:PagingControl ID="pgcCommonDealer" OnPageNoChanged="PagingControl_PageNoChange" OnRowPerPageChanged="PagingControl_RowPerPageChanged"
                        runat="server"/>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>    
    </form>
    </div>
</body>
</html>
