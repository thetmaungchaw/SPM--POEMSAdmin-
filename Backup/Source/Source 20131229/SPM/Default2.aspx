<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default2.aspx.cs" Inherits="SPM.Default2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    <div>
        <ul class="menu">
        
        </ul>
    </div>
    
    </div>
    </form>
</body>
</html>
<script language="javascript" type="text/javascript">
    $(document).ready(function() {

        // First we connect to the JSON file
        $.getJSON('menu.json', function(data) {
            // Setup the items array
            var items = [];
            // For each line of data
            $.each(data, function(key, val) {
                items.push('<li class="' + liClass + '"><a href="' + href + '"><span class="' + icon + '">' + text + '</span></a></li>');
            });

            $('<ul/>', {
                html: items.join('')
            }).appendTo('#buildHere');

        });

        // END json

    });
</script>