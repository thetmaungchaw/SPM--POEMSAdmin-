<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PagingControl.ascx.cs" Inherits="SPMWebApp.PagingControl" %>


<asp:Label ID="lblRowPerPage" Text="Row Per Page:" runat="server" /> &nbsp;

<asp:DropDownList ID="ddlRowPerPage" AutoPostBack="true" runat="server" CssClass="normal"
    onselectedindexchanged="ddlRowPerPage_SelectedIndexChanged" /> &nbsp;&nbsp;

<asp:LinkButton ID="lbtnFirst" Text="First Page" 
    runat="server" onclick="lbtnFirst_Click" /> &nbsp;
                    
<asp:LinkButton ID="lbtnPrev" Text="Prev Page"  runat="server" 
    onclick="lbtnPrev_Click" /> &nbsp; 
                    
<asp:LinkButton ID="lbtnNext" Text="Next Page"  runat="server" 
    onclick="lbtnNext_Click" /> &nbsp; 
                    
<asp:LinkButton ID="lbtnLast" Text="Last Page"  runat="server" 
    onclick="lbtnLast_Click" /> &nbsp;&nbsp;&nbsp;&nbsp;
                    
<asp:Label ID="lblPage" Text="Page"  runat="server" /> &nbsp;
                    
<asp:DropDownList ID="ddlPageNo"  AutoPostBack="true" CssClass="normal"
    runat="server" onselectedindexchanged="ddlPageNo_SelectedIndexChanged" /> &nbsp;
                    
<asp:Label ID="lblTotalPage" Text="of "  runat="server" /> 

<asp:HiddenField ID="hdfPageCount" runat="server" />

