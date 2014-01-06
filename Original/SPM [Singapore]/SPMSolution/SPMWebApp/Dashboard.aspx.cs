using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SPMWebApp.Utilities;
using System.Data;
using SPMWebApp.Services;

namespace SPMWebApp.WebPages.Dashboard
{
    public partial class Dashboard : BasePage.BasePage
    {
        #region Variable Declaration & PageLoad  
        private const String CONST_SUPERVISOR_DASHBOARD = "SupervisorDashboard";
        private const String CONST_INDIVIDUAL_DASHBOARD = "IndividualDashboard";
        private double individualTotal = 0.0;
        private double teamTotal = 0.0;

        protected void Page_Load(object sender, EventArgs e)
        {
            VerifyUserRole();
            LoadFunctionURLMenu();

            LoadClientAssignedProjects();
            LoadLeadsAssignedProjects();
            LoadTotalCommission();
        }        
        #endregion

        #region Methods
        private void LoadClientAssignedProjects()
        {
            ClientAssignmentService clientAssignmentService = new ClientAssignmentService(base.dbConnectionStr);
            DataSet ds = clientAssignmentService.RetrieveAssignedProjectsByUserId(base.userLoginId);
            if (ds.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString().Equals("1"))
            {
                this.gvAssignedClient.DataSource = ds.Tables[0];
                this.gvAssignedClient.DataBind();
            }
            else 
            {
                this.divAssignedClient.InnerHtml = ds.Tables["ReturnTable"].Rows[0]["ReturnMessage"].ToString();
            }
        }

        private void LoadLeadsAssignedProjects()
        {
            LeadsAssignmentService leadsAssignmentService = new LeadsAssignmentService(base.dbConnectionStr);
            DataSet ds = leadsAssignmentService.RetrieveAssignedLeadsProjectsByUserId(base.userLoginId);
            if (ds.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString().Equals("1"))
            {
                this.gvLeads.DataSource = ds.Tables[0];
                this.gvLeads.DataBind();
            }
            else
            {
                this.divLeads.InnerHtml = ds.Tables["ReturnTable"].Rows[0]["ReturnMessage"].ToString();
            }
        }

        private void LoadTotalCommission()
        {
            CommonService commonService = new CommonService(base.dbConnectionStr);
            DataSet ds = commonService.RetrieveTotalCommissionForUser(base.userLoginId);
            if (ds.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString().Equals("1"))
            {
                this.gvCommission.DataSource = ds.Tables[0];
                this.gvCommission.DataBind();
            }
            else
            {
                this.divCommission.InnerHtml = ds.Tables["ReturnTable"].Rows[0]["ReturnMessage"].ToString();
            }
        }
        
        private void LoadFunctionURLMenu()
        {
            DataTable userMenuTable = Session["UserMenuTable"] as DataTable;
            if (userMenuTable == null)
            {
                userMenuTable = AccessRightUtilities.GetUserMenuTable(base.dbConnectionStr, base.userLoginId);
                Session["UserMenuTable"] = userMenuTable;
            }

            IEnumerable<DataRow> query;
            if (Session["IsSupervisor"] != null && (bool)Session["IsSupervisor"])
            {
                query = from tempTbl in userMenuTable.AsEnumerable()
                                             where tempTbl.Field<String>("Function_Code") != CONST_INDIVIDUAL_DASHBOARD
                                             select tempTbl;
            }
            else
            {
                query = from tempTbl in userMenuTable.AsEnumerable()
                        where tempTbl.Field<String>("Function_Code") != CONST_INDIVIDUAL_DASHBOARD &&
                                tempTbl.Field<String>("Function_Code") != CONST_SUPERVISOR_DASHBOARD
                        select tempTbl;
            }
            
            dlFunctionUrl.RepeatDirection = RepeatDirection.Horizontal;

            if (query.CopyToDataTable<DataRow>().Rows.Count < 10)
            {
                dlFunctionUrl.CaptionAlign = TableCaptionAlign.Left;
                dlFunctionUrl.HorizontalAlign = HorizontalAlign.Left;
                dlFunctionUrl.ItemStyle.HorizontalAlign = HorizontalAlign.Left;
                dlFunctionUrl.RepeatLayout = RepeatLayout.Flow;
            }

            dlFunctionUrl.DataSource = query.CopyToDataTable<DataRow>();
            dlFunctionUrl.DataBind();
        }

        private void VerifyUserRole()
        {
            if (Session["IsSupervisor"] == null)
            {
                Response.Redirect("SPMMainPage.aspx");
            }
        }
        #endregion

        #region Events
        protected void gvAssignedClient_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                bool hasValidPeriod = true;
                String assignedDateString = String.Empty;
                String cutOffDateString = String.Empty;

                if (DataBinder.Eval(e.Row.DataItem, "AssignDate") != null)
                {
                    assignedDateString = String.Format("{0:dd/MM/yyyy HH:mm:ss}", DataBinder.Eval(e.Row.DataItem, "AssignDate").ToString());
                }
                else
                {
                    hasValidPeriod = false;
                }
                if (DataBinder.Eval(e.Row.DataItem, "CutOffDate") != null)
                {
                    cutOffDateString = String.Format("{0:dd/MM/yyyy HH:mm:ss}", DataBinder.Eval(e.Row.DataItem, "CutOffDate").ToString());
                }
                else
                {
                    hasValidPeriod = false;
                }
                if (hasValidPeriod)
                {
                    e.Row.Cells[1].Text = Convert.ToDateTime(assignedDateString).ToShortDateString() + " - " + Convert.ToDateTime(cutOffDateString).ToShortDateString();
                }
                else
                {
                    e.Row.Cells[1].Text = " - ";
                }

                if (DataBinder.Eval(e.Row.DataItem, "FollowUpDate") != null)
                {
                    String followUpDate = DataBinder.Eval(e.Row.DataItem, "FollowUpDate").ToString();
                    if (!String.IsNullOrEmpty(followUpDate))
                    {
                        e.Row.Cells[4].Text = Convert.ToDateTime(followUpDate).ToShortDateString();
                    }
                }
            }
        }

        protected void gvLeads_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                bool hasValidPeriod = true;
                String assignedDateString = String.Empty;
                String cutOffDateString = String.Empty;

                if (DataBinder.Eval(e.Row.DataItem, "AssignedDate") != null)
                {
                    assignedDateString = String.Format("{0:dd/MM/yyyy HH:mm:ss}", DataBinder.Eval(e.Row.DataItem, "AssignedDate").ToString());
                }
                else
                {
                    hasValidPeriod = false;
                }
                if (DataBinder.Eval(e.Row.DataItem, "CutOffDate") != null)
                {
                    cutOffDateString = String.Format("{0:dd/MM/yyyy HH:mm:ss}", DataBinder.Eval(e.Row.DataItem, "CutOffDate").ToString());
                }
                else
                {
                    hasValidPeriod = false;
                }
                if (hasValidPeriod)
                {
                    e.Row.Cells[1].Text = Convert.ToDateTime(assignedDateString).ToShortDateString() + " - " + Convert.ToDateTime(cutOffDateString).ToShortDateString();
                }
                else
                {
                    e.Row.Cells[1].Text = " - ";
                }

                if (DataBinder.Eval(e.Row.DataItem, "FollowUpDate") != null)
                {
                    String followUpDate = DataBinder.Eval(e.Row.DataItem, "FollowUpDate").ToString();
                    if (!String.IsNullOrEmpty(followUpDate))
                    {
                        e.Row.Cells[4].Text = Convert.ToDateTime(followUpDate).ToShortDateString();
                    }                    
                }
            }
        }

        protected void gvCommission_RowDataBound(object sender, GridViewRowEventArgs e)
        {            
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (DataBinder.Eval(e.Row.DataItem, "TotalIndividualComm") != null )
                {
                    double individualRowTotal = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "TotalIndividualComm"));
                    individualTotal = individualTotal + individualRowTotal;
                }

                if (DataBinder.Eval(e.Row.DataItem, "TotalTeamComm") != null)
                {
                    double teamRowTotal = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "TotalTeamComm"));
                    teamTotal = teamTotal + teamRowTotal;
                }
            }

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[1].Text = String.Format("{0:0.00}", individualTotal);
                e.Row.Cells[2].Text = String.Format("{0:0.00}", teamTotal);  
            }
        }
        #endregion
    }
}
