using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

using SPMWebApp.Services;
using SPMWebApp.BasePage;
using SPMWebApp.Utilities;

namespace SPMWebApp.WebPages.LeadManagement
{
    public partial class SyncForm : BasePage.BasePage
    {
        private LeadsService leadsService;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (rdoAccNo.Checked)
            {
                txtSyncNRIC.Enabled = false;
                txtSyncAccNo.Enabled = true;
            }
            else {
                txtSyncNRIC.Enabled = true ;
                txtSyncAccNo.Enabled = false;
            }
        }

        protected void btnSyncSubmit_Click(object sender, EventArgs e)
        {
            if (rdoNRIC.Checked)
            {
                SyncByNRIC();
            }
            else if (rdoAccNo.Checked)
            {
                SyncByAccNo();
            }
        }

       
        protected void rdo_CheckedChanged(object sender, EventArgs e)
        {
            
            txtSyncNRIC.Enabled = ((RadioButton)sender).ID.Equals("rdoNRIC");
            txtSyncAccNo.Enabled = ((RadioButton)sender).ID.Equals("rdoAccNo");
        
        }

        private void SyncByNRIC()
        {
           
            string[] wsReturn = null;
            //DataSet ds = null;
           
            leadsService = new LeadsService(base.dbConnectionStr);

            wsReturn = leadsService.LeadsDataSync("NRIC",txtSyncNRIC.Text.Trim());

            if (wsReturn[0] == "1")
            {
                divMessage.InnerHtml = "Leads Synchronisation is successfully done!"; return;
            }
            else
            {
                divMessage.InnerHtml = "Error in Leads Synchronisation!"; return;
            }
        }

        private void SyncByAccNo()
        {
            string[] wsReturn = null;
            //DataSet ds = null;

            leadsService = new LeadsService(base.dbConnectionStr);

            wsReturn = leadsService.LeadsDataSync("AccNo", txtSyncAccNo.Text.Trim());

            if (wsReturn[0] == "1")
            {
                divMessage.InnerHtml = "Leads Synchronisation is successfully done!"; return;
            }
            else
            {
                divMessage.InnerHtml = "Error in Leads Synchronisation!"; return;
            }
        }

        protected void btnSyncCancel_Click(object sender, EventArgs e)
        {
            txtSyncAccNo.Text = "";
            txtSyncNRIC.Text = "";
        }
    }
}
