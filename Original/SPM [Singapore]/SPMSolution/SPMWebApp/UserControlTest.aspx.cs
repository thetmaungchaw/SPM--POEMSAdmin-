using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace SPMWebApp
{
    public partial class UserControlTest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //Setting for Row Per Page
                pgc.StartRowPerPage = 10;
                pgc.RowPerPageIncrement = 10;
                pgc.EndRowPerPage = 100;

                //Setting for PageNo Dropdown List
                pgc.PageCount = 5;
                pgc.DisplayPaging();
            }
        }

        protected void PagingControl_PageNoChange(object sender, EventArgs<Int32> e)
        {
            
            lblInfo.Text = "Custom Control PagingNoChanged Event Occured. Page No : " + e.Value;
        }

        protected void PagingControl_RowPerPageChanged(object sender, EventArgs<Int32> e)
        {
            int rowsCount = 100;
            lblInfo.Text = "Custom Control RowPerPageChanged Event Occured. Row Per Page : " + e.Value;
            pgc.PageCount = (rowsCount / e.Value);
            pgc.DisplayPaging();
        }
    }
}
