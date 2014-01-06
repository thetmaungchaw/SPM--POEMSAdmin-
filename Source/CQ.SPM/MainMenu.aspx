<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MainMenu.aspx.cs" Inherits="MainMenu" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <title></title>
    <script src="Content/JavaScript/jquery.min.js" type="text/javascript"></script>
    <script src="Content/JavaScript/jquery.quicksand.js" type="text/javascript"></script>
    <script src="Content/JavaScript/jquery.easing.1.3.js" type="text/javascript"></script>
    <script src="Content/JavaScript/main.js" type="text/javascript"></script>


    <link rel="stylesheet" type="text/css" href="Content/CSS/all.css" media="screen" />
</head>
<body>
    <form id="form1" runat="server">
        <div id="container">
            <ul id="filterOptions">
                <li class="active"><a href="#" class="all">All</a></li>
                <li><a href="#" class="prem">Access</a></li>
                <li><a href="#" class="champ">Setup</a></li>
                <li><a href="#" class="league1">Assignment</a></li>
                <li><a href="#" class="league2">Contact</a></li>
                <li><a href="#" class="prem">Report</a></li>
                <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Button" />
                <asp:Button ID="Button2" runat="server" OnClick="Button2_Click" Text="Button" />
            </ul>

            <ul class="ourHolder">
                <li class="item" data-id="id-1" data-type="league2">
                    <img src="images/accrington-stanley.jpg" alt="Accrington Stanley" />
                    <h3>Accrington Stanley</h3>
                </li>
                <li class="item" data-id="id-2" data-type="prem">
                    <img src="images/arsenal.jpg" alt="Arsenal" />
                    <h3>Arsenal</h3>
                </li>
                <li class="item" data-id="id-3" data-type="league2">
                    <img src="images/burton-albion.jpg" alt="Burton Albion" />
                    <h3>Burton Albion</h3>
                </li>
                <li class="item" data-id="id-4" data-type="champ">
                    <img src="images/cardiff-city.jpg" alt="Cardiff City" />
                    <h3>Cardiff City</h3>
                </li>
                <li class="item" data-id="id-5" data-type="champ">
                    <img src="images/derby-county.jpg" alt="Derby County" />
                    <h3>Derby County</h3>
                </li>
                <li class="item" data-id="id-6" data-type="prem">
                    <img src="images/everton.jpg" alt="Everton" />
                    <h3>Everton</h3>
                </li>
                <li class="item" data-id="id-7" data-type="league2">
                    <img src="images/morecambe.jpg" alt="Morecambe" />
                    <h3>Morecambe</h3>
                </li>
                <li class="item" data-id="id-8" data-type="champ">
                    <img src="images/norwich-city.jpg" alt="Norwich City" />
                    <h3>Norwich City</h3>
                </li>
                <li class="item" data-id="id-9" data-type="league1">
                    <img src="images/notts-county.jpg" alt="Notts County" />
                    <h3>Notts County</h3>
                </li>
                <li class="item" data-id="id-10" data-type="champ">
                    <img src="images/reading.jpg" alt="Reading" />
                    <h3>Reading</h3>
                </li>
                <li class="item" data-id="id-11" data-type="league1">
                    <img src="images/sheffield-wednesday.jpg" alt="Sheffield Wednesday" />
                    <h3>Sheffield Wednesday</h3>
                </li>
                <li class="item" data-id="id-12" data-type="prem">
                    <img src="images/sunderland.jpg" alt="Sunderland" />
                    <h3>Sunderland</h3>
                </li>
                <li class="item" data-id="id-13" data-type="prem">
                    <img src="images/tottenham-hotspur.jpg" alt="Tottenham Hotspur" />
                    <h3>Tottenham Hotspur</h3>
                </li>
                <li class="item" data-id="id-14" data-type="champ">
                    <img src="images/watford.jpg" alt="Watford" />
                    <h3>Watford</h3>
                </li>
            </ul>
        </div>
    </form>
</body>
</html>
<script type="text/javascript">
    $(document).ready(function () {
        // get the action filter option item on page load
        var $filterType = $('#filterOptions li.active a').attr('class');

        // get and assign the ourHolder element to the
        // $holder varible for use later
        var $holder = $('ul.ourHolder');

        // clone all items within the pre-assigned $holder element
        var $data = $holder.clone();

        // attempt to call Quicksand when a filter option
        // item is clicked
        $('#filterOptions li a').click(function (e) {
            // reset the active class on all the buttons
            $('#filterOptions li').removeClass('active');

            // assign the class of the clicked filter option
            // element to our $filterType variable
            var $filterType = $(this).attr('class');
            $(this).parent().addClass('active');
            if ($filterType == 'all') {
                // assign all li items to the $filteredData var when
                // the 'All' filter option is clicked
                var $filteredData = $data.find('li');
            }
            else {
                // find all li elements that have our required $filterType
                // values for the data-type element
                var $filteredData = $data.find('li[data-type=' + $filterType + ']');
            }

            // call quicksand and assign transition parameters
            $holder.quicksand($filteredData, {
                duration: 800,
                easing: 'easeInOutQuad'
            });
            return false;
        });
    });
</script>
