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
    public partial class CoolCalendar : System.Web.UI.UserControl
    {
        private Boolean timeRequired;
        private string dateTimeFormat;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Populate_MonthList();
                Populate_YearList();

                DateTime dt;
                if (DateTextFrom.Text != "")
                {
                    //Than Dev
                   // dt = DateTime.Parse(DateTextFrom.Text);

                    
                    string strYear = "";
                    IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                    String datetime = DateTextFrom.Text;
                    DateTime dtFormat = DateTime.Parse(datetime, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                   
                    Calendar1.TodaysDate = dtFormat;
                }
            }            
        }


        //Properties
        public string DateTextFromValue
        {
            get { return DateTextFrom.Text; }
            set { DateTextFrom.Text = value; }
        }

        public Boolean DateTextRequired
        {
            get { return DateTextFromRequired.Enabled; }
            set { DateTextFromRequired.Enabled = value; }
        }

        public string DateTextRequiredText
        {
            get { return DateTextFromRequired.ErrorMessage; }
            set { DateTextFromRequired.ErrorMessage = value; }
        }

        public string DateTimeFormat
        {
            get { return dateTimeFormat; }
            set { dateTimeFormat = value; }
        }

        public Boolean TimeRequired
        {
            get { return timeRequired; }
            set { timeRequired = value; }
        }

        public int ControlWidth
        {
            set
            {
                DateTextFrom.Width = Unit.Pixel(value);
            }
        }

        // SET THE FIRST CALENDAR (FROM DATE) 
        public void Set_Calendar(object Sender, EventArgs E)
        {          
            string theDate = drpCalMonth.SelectedItem.Value + " 1, " + drpCalYear.SelectedItem.Value;
            DateTime dtFoo;
            dtFoo = System.Convert.ToDateTime(theDate);
            Calendar1.TodaysDate = dtFoo;
        }

        protected void Calendar1_SelectionChanged(object sender, EventArgs e)
        {
            // Popup result is the selected date
            drpCalMonth.SelectedIndex = Calendar1.SelectedDate.Month - 1;
            drpCalYear.SelectedItem.Selected = false;
            drpCalYear.Items.FindByValue(Calendar1.SelectedDate.Year.ToString()).Selected = true;
                       

            // Popup result is the selected date
            if(timeRequired)
                PopupControlExtender1.Commit(Calendar1.SelectedDate.ToString("dd/MM/yyyy") + " 23:59:59");
            else
                PopupControlExtender1.Commit(Calendar1.SelectedDate.ToString("dd/MM/yyyy"));
        }

        // INNOVATIVE CALENDARS VIA AJAX THAT ALLOW USER TO PICK MONTH AND DATE VIA DROPDOWN
        void Populate_MonthList()
        {
            drpCalMonth.Items.Add("January");
            drpCalMonth.Items.Add("February");
            drpCalMonth.Items.Add("March");
            drpCalMonth.Items.Add("April");
            drpCalMonth.Items.Add("May");
            drpCalMonth.Items.Add("June");
            drpCalMonth.Items.Add("July");
            drpCalMonth.Items.Add("August");
            drpCalMonth.Items.Add("September");
            drpCalMonth.Items.Add("October");
            drpCalMonth.Items.Add("November");
            drpCalMonth.Items.Add("December");
            string strMonth;

            if (DateTextFrom.Text == "")
            {
                strMonth = DateTime.Now.ToString("MMMM");
            }
            else
            {
                //strMonth = Convert.ToDateTime(DateTextFrom.Text).ToString("MMMM");
                strMonth = DateTime.ParseExact(DateTextFrom.Text.Trim(), dateTimeFormat, null).ToString("MMMM");
            }             

            drpCalMonth.Items.FindByValue(strMonth).Selected = true;
        }

        // POPULATE THE YEARLIST FROM 20 YEARS AGO TO ONE YEAR HENCE 
        void Populate_YearList()
        {
            int intYear;


            // Year list can be changed by changing the lower and upper 
            // limits of the For statement
            //for (intYear = DateTime.Now.Year - 20; intYear <= DateTime.Now.Year + 1; intYear++)
            for (intYear = 1990; intYear <= DateTime.Now.Year + 1; intYear++)
            {
                drpCalYear.Items.Add(intYear.ToString());
            }

            if (DateTextFrom.Text == "")
                drpCalYear.Items.FindByValue(DateTime.Now.Year.ToString()).Selected = true;
            else
            {
                //Than Dev
                //string strYear = Convert.ToDateTime(DateTextFrom.Text).Year.ToString();
                //drpCalYear.Items.FindByValue(strYear).Selected = true;
                //End Than Dev

                //Thiri Dev
                string strYear = "";
                IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                String datetime = DateTextFrom.Text;
                DateTime dt = DateTime.Parse(datetime, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                strYear = dt.Year.ToString();
                drpCalYear.Items.FindByValue(strYear).Selected = true;

            }
        }
    }
}