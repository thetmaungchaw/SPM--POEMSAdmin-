<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SPM._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    
    <link href="Content/CSS/examples-offline.css" rel="Stylesheet" />
    <link href="Content/CSS/kendo.common.min.css" rel="stylesheet" />
    <link href="Content/CSS/kendo.default.min.css" rel="stylesheet" />
    <link href="Content/CSS/kendo.rtl.min.css" rel="stylesheet" />

    <script src="Content/JavaScript/jquery.min.js" type="text/jscript"></script>
    <script src="Content/JavaScript/kendo.web.min.js" type="text/jscript"></script>
    <script src="Content/JavaScript/console.js" type="text/jscript"></script>
    
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:Button ID="Button1" runat="server" onclick="Button1_Click" Text="Click" />
    
    </div>
    
    <div id="Div1" class="k-content">
        <div id="megaStore">
            <ul id="menu">
                <%--<li>Reports
                    <asp:DataList ID="dl" runat="server">
                        <ItemTemplate>
                            <ul>
                                <li>
                                    <asp:Label ID="lab" runat="server" Text='<%# Eval("AECode") %>'></asp:Label>
                                </li>
                            </ul>
                        </ItemTemplate>
                    </asp:DataList>
                </li>--%>
                <li>Furniture
                    <ul>
                        <!-- moving the UL to the next line will cause an IE7 problem -->
                        <li>Tables</li>
                        <li>Sofas</li>
                        <li>Occasional</li>
                        <li>Childerns</li>
                        <li>Beds</li>
                    </ul>
                </li>
            </ul>
        </div>
    </div>
    
    
    <asp:GridView ID="GridView1" runat="server">
    </asp:GridView>
    
    <div id="example" class="k-content">
            <div id="grid"></div>

            <script type="text/javascript">
                $(document).ready(function() {
                    $("#grid").kendoGrid({
                        dataSource: {
                            type: "odata",
                            transport: {
                            //read: "http://demos.kendoui.com/service/Northwind.svc/Orders"
                            //read: "http://localhost:21403/"
                                read: ""
                            },
                            schema: {
                                model: {
                                    fields: {
                                        AECode: { type: "string" },
                                        AEName: { type: "string" }
                                    }
                                }
                            },
                            pageSize: 30,
                            serverPaging: true,
                            serverFiltering: true,
                            serverSorting: true
                        },
                        height: 430,
                        sortable: true,
                        filterable: true,
                        columnMenu: true,
                        pageable: true,
                        columns: [{
                            field: "AECode",
                            title: "Dealer Code",
                            width: 100
                        }, {
                            field: "AEName",
                            title: "Dealer Name",
                            width: 200
                        }
                        ]
                    });
                });
            </script>
        </div>
    
    
    
        
    <asp:Button ID="Button2" runat="server" Text="Click" onclick="Button2_Click" />
    
    <style type="text/css">
    #megaStore
    {
        width: 600px;
        margin: 30px auto;
        padding-top: 120px;
        background: url('../../content/web/menu/header.jpg') no-repeat 0 0;
    }
    #menu h2
    {
        font-size: 1em;
        text-transform: uppercase;
        padding: 5px 10px;
    }
    #template img
    {
        margin: 5px 20px 0 0;
        float: left;
    }
    #template
    {
        width: 380px;
    }
    #template ol
    {
        float: left;
        margin: 0 0 0 30px;
        padding: 10px 10px 0 10px;
    }
    #template:after
    {
        content: ".";
        display: block;
        height: 0;
        clear: both;
        visibility: hidden;
    }
    #template .k-button
    {
        float: left;
        clear: left;
        margin: 5px 0 5px 12px;
    }
</style>
    
    </form>
</body>
</html>

<script type="text/javascript">
    $(document).ready(function() {
        $("#menu").kendoMenu();
    });
</script>