/* 
 * Purpose:         SPM Main Menu Page controller
 * Created By:      Than Htike Tun
 * Date:            10/02/2010
 * 
 * Change History:
 * ----------------------------------------------------------------------------------
 * Modified By      Date        Purpose
 * ----------------------------------------------------------------------------------
 * Li Qun           09/04/2010  Check session expiry
 * 
 */

using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using SPMWebApp.BasePage;
using SPMWebApp.Services;

namespace SPMWebApp
{
    public partial class SPMMenu : BasePage.BasePage
    {
        private AccessControlService accessControlService;

        protected override void OnInit(EventArgs e)
        {
            //Get user id and db connection string from POEMS Admin
            if (!String.IsNullOrEmpty(Request.Params["UserId"]))
            {
                if (Session["UserId"] == null || String.IsNullOrEmpty(Session["UserId"].ToString()) || !Request.Params["UserId"].Trim().Equals(Session["UserId"].ToString().Trim()))
                {
                    Session["UserId"] = Request.Params["UserId"];
                    Session["DbConnStr"] = Request.Params["DbConnStr"];
                }
            }

            //OLD DEV DB => ITDBDEV02.PSDEVDB.COM
           Session["UserId"] = "cqdemo1";
           Session["DbConnStr"] = "Provider=sqloledb;Data Source=10.30.5.8;Network Library=dbmssocn;Initial Catalog=SPM; Integrated Security=SSPI;";
           
            //Session["DbConnStr"] = "Provider=sqloledb;Data Source=ITDBDEV12.PSDEVDB.COM;Network Library=dbmssocn;Initial Catalog=SPM; Integrated Security=SSPI;";
            //Session["DbConnStr"] = "Provider=sqloledb;Data Source=.;Network Library=dbmssocn;Initial Catalog=SPM; Integrated Security=SSPI;";            
             // Session["DbConnStr"] = "Provider=sqloledb;Data Source=10.22.1.98;Network Library=dbmssocn;Initial Catalog=SPMPhaseIII;uid=sa;pwd=LDsa256;";
           //Session["DbConnStr"] = "Provider=sqloledb;Data Source=localhost;Network Library=dbmssocn;Initial Catalog=SPM; Integrated Security=SSPI;";
           
            base.OnInit(e);
        }
            
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                accessControlService = new AccessControlService(base.dbConnectionStr);
                DataSet ds = accessControlService.RetrieveUserMenuOptions(base.userLoginId);

                if (ds.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString().Equals("1"))
                {
                    rptFunctions.DataSource = ds.Tables[0];
                    rptFunctions.DataBind();
                }
                else
                {
                    divMessage.InnerHtml = ds.Tables["ReturnTable"].Rows[0]["ReturnMessage"].ToString();
                    rptFunctions.DataSource = null;
                    rptFunctions.DataBind();
                }
            }
        }


    }
}
