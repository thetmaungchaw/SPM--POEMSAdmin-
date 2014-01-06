﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SPMWebApp.Services;
using System.Data;
using SPMWebApp.Utilities;
using System.Configuration;

namespace SPMWebApp
{
    public partial class SPMMainPage : BasePage.BasePage
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
            
            //Development
            //Session["UserId"] = "ittest";
            //Session["UserId"] = "eiei"; // Ei Ei Thu
            //Session["UserId"] = "tserandyfoo"; // Andy Foo Chin Yong
            //Session["UserId"] = "tncolin"; // Colin Thura Maung
            //Session["UserId"] = "pstrfhv"; // Ying Yi
            Session["UserId"] = "icchiahl"; // Hui Lin Chia
            //Session["UserId"] = "tsergavinl"; // Gavin
            //Session["UserId"] = "tmgavin"; // Gavin
            //Session["UserId"] = "tsertayv"; // Vincent
            //Session["UserId"] = "tserpatrickccg"; //
            //Session["UserId"] = "pwtracy"; // Tracy
            //Session["UserId"] = "pwgarycst"; // Gary
            //Session["UserId"] = "pwrachel"; // Rachel
            //Session["UserId"] = "TF_PIC2"; // OC
            //Session["UserId"] = "oc"; // OC
            //Session["UserId"] = Request.QueryString["UserID"].ToString();
            //Session["DbConnStr"] = "Provider=sqloledb;Data Source=10.22.1.28;Network Library=dbmssocn;Initial Catalog=SPM; Integrated Security=SSPI;";
            Session["DbConnStr"] = "Provider=sqloledb;Data Source=10.30.5.8;Network Library=dbmssocn;Initial Catalog=SPM; Integrated Security=SSPI;";
            //Session["DbConnStr"] = "Provider=sqloledb;Data Source=10.30.6.8;Network Library=dbmssocn;Initial Catalog=SPM; Integrated Security=SSPI;";

            //String userId = System.Configuration.ConfigurationSettings.AppSettings["UserId"].ToString();
            //String dbConn = System.Configuration.ConfigurationSettings.AppSettings["DBConnection"].ToString();
            //Session["DbConnStr"] = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString.ToString();
            //Session["UserId"] = userId;
            //Session["DbConnStr"] = dbConn;

            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            String redirectPage;            
            if (LoadUserAccessRight(out redirectPage))
            {
                LoadUserMenuTable();
                Response.Redirect(redirectPage);
            }            
        }

        #region Methods
        private void LoadUserMenuTable()
        {
            DataTable userMenuTable = AccessRightUtilities.GetUserMenuTable(base.dbConnectionStr, base.userLoginId);
            Session["UserMenuTable"] = userMenuTable;
        }

        private bool LoadUserAccessRight(out String redirectPage)
        {
            DataSet dsUserAccessRight = AccessRightUtilities.RetrieveUserAccessRights(base.dbConnectionStr, base.userLoginId);
            if (dsUserAccessRight.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString().Equals("1"))
            {
                Session["UserAccessRights"] = dsUserAccessRight.Tables[0];
                GetClientAssignLiteUserRight(dsUserAccessRight.Tables[0]);
                var results = from tempAccessRight in dsUserAccessRight.Tables[0].AsEnumerable()
                              where (tempAccessRight.Field<String>("Function_Code").Equals("SupervisorDashboard") || tempAccessRight.Field<String>("Function_Code").Equals("IndividualDashboard"))                               
                              select tempAccessRight;
                EnumerableRowCollection<DataRow> drResult = results as EnumerableRowCollection<DataRow>;

                int userRightCount = VerifySupervisorUserRight(drResult);

                //if (userRightCount == 2)
                //{
                //    Session["IsSupervisor"] = true;
                //    redirectPage = "SupervisorDashboard.aspx";
                //}
                //else
                //{
                //    Session["IsSupervisor"] = false;
                //    redirectPage = "Dashboard.aspx";
                //}
                
                //return true;

                if (userRightCount == 1)
                {
                    Session["IsSupervisor"] = true;
                    redirectPage = "SupervisorDashboard.aspx";
                    return true;
                }
                else if (userRightCount == 0)
                {
                    Session["IsSupervisor"] = false;
                    redirectPage = "Dashboard.aspx";
                    return true;
                }
                else
                {
                    divMessage.InnerHtml = "User has no access right";
                    redirectPage = string.Empty;
                    return false;
                }
            }
            else
            {
                divMessage.InnerHtml = dsUserAccessRight.Tables["ReturnTable"].Rows[0]["ReturnMessage"].ToString();
                redirectPage = string.Empty;
                return false;
            }
        }

        private void GetClientAssignLiteUserRight(DataTable dataTable)
        {
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                if (dataTable.Rows[i]["Function_Code"].ToString().Trim() == "ClientAssignLite")
                {
                    Session["ClientAssignLiteUserRight"] = dataTable.Rows[i]["UserRight"].ToString();
                }
            }
        }

        private static int VerifySupervisorUserRight(EnumerableRowCollection<DataRow> drResult)
        {
            /// <Updated by OC>
            //int userRightCount = 0;
            //if (drResult.Count() > 0)
            //{
            //    foreach (DataRow dr in drResult)
            //    {
            //        if (dr.Field<String>("UserRight").ToUpper().Equals("Y"))
            //        {
            //            userRightCount += 1;
            //        }
            //    }
            //}
            //return userRightCount;

            /// <Added by OC>
            int userRightCount = -1;
            if (drResult.Count() > 0)
            {
                foreach (DataRow dr in drResult)
                {
                    if (dr.Field<String>("UserRight").ToUpper().Equals("Y") && dr.Field<String>("Function_Code").Equals("SupervisorDashboard"))
                    {
                        userRightCount = 1;
                    }
                    if (dr.Field<String>("UserRight").ToUpper().Equals("Y") && dr.Field<String>("Function_Code").Equals("IndividualDashboard"))
                    {
                        userRightCount = 0;
                    }

                }
            }
            return userRightCount;

        }
        #endregion
    }
}