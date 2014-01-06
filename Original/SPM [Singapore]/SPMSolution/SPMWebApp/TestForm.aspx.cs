using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Diagnostics;

namespace SPMWebApp
{
    public partial class TestForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            
            Dictionary<String,Int32> x;
            x = new Dictionary<string, int>();
            x.Add("M1", 1);
            x.Add("M2", 2);

            Debug.WriteLine(x["M1"]);
            if (x["M1"] == 1)
            {
                x["M1"] = 5;
            }
            Debug.WriteLine(x["M1"]);
        }
    }
}
