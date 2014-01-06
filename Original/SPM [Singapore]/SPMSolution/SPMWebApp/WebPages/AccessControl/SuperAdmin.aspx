<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SuperAdmin.aspx.cs" Inherits="SPMWebApp.WebPages.AccessControl.SuperAdmin" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Team Management</title>
    <style type="text/css">
        .border
        {
        }
        .ajax__myTab .ajax__tab_header
        {
            font-family: verdana,tahoma,helvetica;
            font-size: 12px;
            border-bottom: solid 1px #999999;
        }
        .ajax__myTab .ajax__tab_outer
        {
            padding-right: 4px;
            height: 21px;
            background-color: #C0C0C0;
            margin-right: 2px;
            border-right: solid 1px #666666;
            border-top: solid 1px #aaaaaa;
        }
        .ajax__myTab .ajax__tab_inner
        {
            padding-left: 3px;
            background-color: #C0C0C0;
        }
        .ajax__myTab .ajax__tab_tab
        {
            height: 13px;
            padding: 4px;
            margin: 0;
        }
        .ajax__myTab .ajax__tab_hover .ajax__tab_outer
        {
            background-color: #cccccc;
        }
        .ajax__myTab .ajax__tab_hover .ajax__tab_inner
        {
            background-color: #cccccc;
        }
        .ajax__myTab .ajax__tab_hover .ajax__tab_tab
        {
        }
        .ajax__myTab .ajax__tab_active .ajax__tab_outer
        {
            background-color: #fff;
            border-left: solid 1px #999999;
        }
        .ajax__myTab .ajax__tab_active .ajax__tab_inner
        {
            background-color: #fff;
        }
        .ajax__myTab .ajax__tab_active .ajax__tab_tab
        {
        }
        .ajax__myTab .ajax__tab_body
        {
            font-family: verdana,tahoma,helvetica;
            font-size: 10pt;
            border: 1px solid #999999;
            border-top: 0;
            padding: 8px;
            background-color: #ffffff;
        }
    </style>   
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <ajaxToolkit:TabContainer ID="tcMainContainer" runat="server" ActiveTabIndex="0" CssClass="ajax__myTab">
        <ajaxToolkit:TabPanel ID="TabView" runat="server" HeaderText="View List">
            <ContentTemplate>
                <fieldset>
                    <legend style="color: Red; font-weight: bold;">Team Management</legend>
                    <asp:Label ID="labUserID" runat="server" Text="User ID"></asp:Label>
                    <asp:DropDownList ID="ddlUserID" runat="server" DataTextField="AEName" DataValueField="UserID">
                    </asp:DropDownList>
                    <%--<asp:Label ID="labAECode" runat="server" Text="AECode"></asp:Label>
        <asp:DropDownList ID="ddlAECode" runat="server" DataTextField="AECode" DataValueField="AECode">
        </asp:DropDownList>--%>
                    <asp:Label ID="labAEName" runat="server"></asp:Label>
                    <asp:Label ID="labAEGroup" runat="server" Text="AEGroup"></asp:Label>
                    <asp:DropDownList ID="ddlAEGroup" runat="server" DataTextField="AEGroup" DataValueField="AEGroup">
                    </asp:DropDownList>
                    <asp:Button ID="btnFind" runat="server" Text="Search" OnClick="btnFind_Click" CausesValidation="false" />
                    <asp:Button ID="btnRemove" runat="server" Text="Delete" OnClick="btnRemove_Click"
                        CausesValidation="false" />
                    <asp:Button ID="btnDeleteByUserID" runat="server" Text="Delete By UserID" Visible="false" OnClick="btnDeleteByUserID_Click"
                        OnClientClick="return confirm('Are you sure you want to delete ?');" />
                    <div id="divSuperAdmin" style="overflow-y: scroll; height: 700px; width: 100%;">
                        <asp:GridView ID="gvSuperAdmin" runat="server" AutoGenerateColumns="False" OnPreRender="gvSuperAdmin_PreRender">
                            <Columns>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <input id="chkHDelete" name="chkHDelete" onclick="SelectAllSuperAdmin(this)" type="checkbox" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkIDelete" runat="server" />
                                        <itemstyle width="50px" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="RowNo" HeaderText="No" />
                                <asp:TemplateField HeaderText="UserID">
                                    <ItemTemplate>
                                        <asp:Label ID="labUserID" runat="server" Text='<%# Eval("UserID") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="AEName" HeaderText="AEName" />
                                <asp:TemplateField HeaderText="AEGroup">
                                    <ItemTemplate>
                                        <asp:Label ID="labAEGroup" runat="server" Text='<%# Eval("AEGroup") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <div id="divPaging2" runat="server">
                            <table>
                                <tr>
                                    <td>
                                        Row Per Page:
                                        <asp:DropDownList ID="ddlRowPerPage" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlRowPerPage_SelectedIndexChanged">
                                            <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                            <asp:ListItem Text="20" Value="20"></asp:ListItem>
                                            <asp:ListItem Text="50" Value="50" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="100" Value="100"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnFirst" runat="server" Text="<<" OnClick="btnFirst_Click" />
                                    </td>
                                    <td>
                                        <asp:Button ID="btnPrevious" runat="server" Text="<" OnClick="btnPrevious_Click" />
                                    </td>
                                    <td>
                                        <asp:Button ID="btnNext" runat="server" Text=">" OnClick="btnNext_Click" />
                                    </td>
                                    <td>
                                        <asp:Button ID="btnLast" runat="server" Text=">>" OnClick="btnLast_Click" />
                                    </td>
                                    <td>
                                        Page
                                        <asp:DropDownList ID="ddlNoOfPage" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlNoOfPage_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        of
                                        <asp:Label ID="labTotalPage" runat="server"></asp:Label>.&nbsp;&nbsp;Total Record
                                        <asp:Label ID="labTotalRecord" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </fieldset>
            </ContentTemplate>
        </ajaxToolkit:TabPanel>
        <ajaxToolkit:TabPanel ID="TabEntry" runat="server" HeaderText="Setup">
            <ContentTemplate>
                <fieldset>
                    <legend style="color: Red; font-weight: bold;">Name</legend>
                    <asp:TextBox ID="txtUserIDSearch" runat="server"></asp:TextBox>
                    <asp:Button ID="btnUserIDSearch" runat="server" Text="Search" OnClick="btnUserIDSearch_Click" />
                    <asp:RadioButton ID="rdbtnUserIDStartWith" runat="server" Text="Start With" GroupName="UserID" />
                    <asp:RadioButton ID="rdbtnUserIDEndWith" runat="server" Text="End With" GroupName="UserID" />
                    <asp:RadioButton ID="rdbtnUserIDContain" runat="server" Text="Contain" GroupName="UserID" />&nbsp;&nbsp;
                    <asp:Label ID="labUserIDSelectAll" runat="server" Text="Select All"></asp:Label>
                    <asp:CheckBox ID="chkUserIDSelectAll" runat="server" onclick="SelectAllUserID(this)" />
                    <div style="overflow-y: scroll; height: 400px; width: 100%;">
                        <asp:DataList ID="dlUserID" runat="server" RepeatColumns="6" RepeatDirection="Horizontal"
                            OnPreRender="dlUserID_PreRender">
                            <ItemTemplate>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="chkUserID" runat="server" />
                                        </td>
                                        <td>
                                            <asp:Label ID="labAEName" runat="server" Text='<%# Bind("AEName") %>'></asp:Label>
                                            <asp:Label ID="labUserID" Visible="false" runat="server" Text='<%# Bind("UserID") %>'></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                        </asp:DataList>
                    </div>
                </fieldset>
                <fieldset>
                    <legend style="color: Red; font-weight: bold;">Team/Group</legend>
                    <asp:TextBox ID="txtSearchAEGroup" runat="server"></asp:TextBox>
                    <asp:Button ID="btnSearchAEGroup" runat="server" Text="Search" OnClick="btnSearchAEGroup_Click" />
                    <asp:RadioButton ID="rdbtnAEGroupStartWith" runat="server" Text="Start With" GroupName="AEGroup" />
                    <asp:RadioButton ID="rdbtnAEGroupEndWith" runat="server" Text="End With" GroupName="AEGroup" />
                    <asp:RadioButton ID="rdbtnAEGroupContain" runat="server" Text="Contain" GroupName="AEGroup" />&nbsp;&nbsp;
                    <asp:Label ID="labAEGroupSelectAll" runat="server" Text="Select All"></asp:Label>
                    <asp:CheckBox ID="chkAEGroupSelectAll" runat="server" onclick="SelectAllAEGroup(this)" />
                    <div style="overflow-y: scroll; height: 400px; width: 100%">
                        <asp:DataList ID="dlAEGroup" runat="server" RepeatColumns="10" RepeatDirection="Horizontal"
                            OnPreRender="dlAEGroup_PreRender">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkAEGroup" runat="server" />
                                <asp:Label ID="labAEGroup" runat="server" Text='<%# Bind("AEGroup") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:DataList>
                    </div>
                </fieldset>
                <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" />
                <asp:Label ID="labError" runat="server"></asp:Label>
                <asp:HiddenField ID="hdnField1" runat="server" />
                <asp:HiddenField ID="hdnField2" runat="server" />
            </ContentTemplate>
        </ajaxToolkit:TabPanel>
    </ajaxToolkit:TabContainer>
    </form>
</body>
</html>

<script language="javascript" type="text/javascript">

    function SelectAllUserID(objchkH) {
        for (i = 0; i < chkUserID.length; i++) {
            var objchk = document.getElementById(chkUserID[i]);

            if (objchkH.checked == true) {
                objchk.checked = true;
            }
            else {
                objchk.checked = false;
            }
        }
    }

    function SelectAllAEGroup(objchkH) {
        for (i = 0; i < chkAEGroup.length; i++) {
            var objchk = document.getElementById(chkAEGroup[i]);

            if (objchkH.checked == true) {
                objchk.checked = true;
            }
            else {
                objchk.checked = false;
            }
        }
    }

    function SelectAllSuperAdmin(objchkH) {
        for (i = 0; i < chkIDelete.length; i++) {
            var objchk = document.getElementById(chkIDelete[i]);

            if (objchkH.checked == true) {
                objchk.checked = true;
            }
            else {
                objchk.checked = false;
            }
        }
    }

 
</script>

<script language="javascript" type="text/javascript">

    var hdnField1 = document.getElementById('<%=hdnField1.ClientID%>');
    var hdnField2 = document.getElementById('<%=hdnField2.ClientID%>');
    var divSuperAdmin = document.getElementById('divSuperAdmin');

    divSuperAdmin.onscroll = function() {
        hdnField1.value = divSuperAdmin.scrollTop;
        hdnField2.value = divSuperAdmin.scrollLeft;
    }

    window.onload = function() {
        divSuperAdmin.scrollTop = hdnField1.value;
        divSuperAdmin.scrollLeft = hdnField2.value;


        //        // get tab container
        //        var container = document.getElementById("tabContainer");
        //        var tabcon = document.getElementById("tabscontent");
        //        //alert(tabcon.childNodes.item(1));
        //        // set current tab
        //        var navitem = document.getElementById("tabHeader_1");

        //        //store which tab we are on
        //        var ident = navitem.id.split("_")[1];
        //        //alert(ident);
        //        navitem.parentNode.setAttribute("data-current", ident);
        //        //set current tab with class of activetabheader
        //        navitem.setAttribute("class", "tabActiveHeader");

        //        //hide two tab contents we don't need
        //        //        var pages = tabcon.getElementsByTagName("div");                
        //        //        for (var i = 1; i < pages.length; i++) {            
        //        //            pages.item(i).style.display = "none";
        //        //        };
        //        document.getElementById('tabpage_2').style.display = "none";

        //        //this adds click event to tabs
        //        var tabs = container.getElementsByTagName("li");
        //        for (var i = 0; i < tabs.length; i++) {
        //            tabs[i].onclick = displayPage;
        //        }

        //        ResetChks();

    }

    // on click of one of tabs
    function displayPage() {
        var current = this.parentNode.getAttribute("data-current");
        //remove class of activetabheader and hide old contents
        document.getElementById("tabHeader_" + current).removeAttribute("class");
        document.getElementById("tabpage_" + current).style.display = "none";

        var ident = this.id.split("_")[1];
        //add class of activetabheader to new active tab and show contents
        this.setAttribute("class", "tabActiveHeader");
        document.getElementById("tabpage_" + ident).style.display = "block";
        this.parentNode.setAttribute("data-current", ident);
    }

    function ResetChks() {
        for (i = 0; i < chkAEGroup.length; i++) {
            var objchk = document.getElementById(chkAEGroup[i]);
            objchk.checked = false;
        }

        //        for (i = 0; i < chkUserID.length; i++) {
        //            var objchk = document.getElementById(chkUserID[i]);
        //            objchk.checked = false;
        //         }
    }

</script>

