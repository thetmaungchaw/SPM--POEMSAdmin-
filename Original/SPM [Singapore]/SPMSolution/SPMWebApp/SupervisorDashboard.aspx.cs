﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SPMWebApp.Services;
using System.Collections;
using SPMWebApp.Utilities;
using System.Drawing;

namespace SPMWebApp.WebPages.Dashboard
{
    public partial class SupervisorDashboard : BasePage.BasePage
    {
        #region Variable Declaration & PageLoad        
        private const String XChartValue = "Category";
        private const String YChartValue = "CommissionEarned";
        private const String CONST_SELECT_PROJECT = "-- Select Project --";
        private const String CONST_SUPERVISOR_DASHBOARD = "SupervisorDashboard";
        private const String CONST_INDIVIDUAL_DASHBOARD = "IndividualDashboard";

        protected void Page_Load(object sender, EventArgs e)
        {            
            //chart_CommissionEarned.ImageStorageMode = System.Web.UI.DataVisualization.Charting.ImageStorageMode.UseImageLocation;
            VerifyUserRole();
            LoadFunctionURLMenu();            
            if (!IsPostBack)
            {
                /// <Added by OC>
                CommonService CommonService = new CommonService(base.dbConnectionStr);
                ViewState["dsDealerDetail"] = CommonService.RetrieveDealerCodeAndNameByUserID(Session["UserId"].ToString());

                FillInProjectName();
                divProjectDetail.Visible = false;
            }
        }
        #endregion
        
        #region Methods
        private void VerifyUserRole()
        {
            if (Session["IsSupervisor"] == null || !(bool)Session["IsSupervisor"])
            {
                Response.Redirect("SPMMainPage.aspx");
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

            IEnumerable<DataRow> query = from tempTbl in userMenuTable.AsEnumerable()
                                            where tempTbl.Field<String>("Function_Code") != CONST_SUPERVISOR_DASHBOARD
                                            select tempTbl;

            dlFunctionUrl.RepeatDirection = RepeatDirection.Horizontal;
            dlFunctionUrl.DataSource = query.CopyToDataTable<DataRow>();
            dlFunctionUrl.DataBind();
        }

        private void LoadProjectDetails()
        {
            if (ddlProjectName.SelectedItem != null && !ddlProjectName.SelectedItem.Text.Equals(CONST_SELECT_PROJECT))
            {                
                divCommEarned.Visible = true;
                divProjectDetail.Visible = true;
                ClientAssignmentService clientAssignmentService = new ClientAssignmentService(base.dbConnectionStr);
                DataSet dsCommissionEarned = clientAssignmentService.RetrieveCommissionEarnedByProjectId(ddlProjectName.SelectedValue);
                LoadProjectInformation();
                LoadClientCalls(ddlProjectName.SelectedValue);

                if (dsCommissionEarned.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString().Equals("1"))
                {
                    LoadCommissionEarnedByProject(dsCommissionEarned.Tables["CommissionEarned"]);
                    LoadChart(dsCommissionEarned.Tables["CommissionEarned"]);
                }
                else
                {
                    this.gvCommissionEarned.DataSource = null;
                    this.gvCommissionEarned.DataBind();
                }
            }
            else
            {
                divCommEarned.Visible = false;
                divProjectDetail.Visible = false;
            }
        }

        private void LoadClientCalls(String projectId)
        {
            ClientAssignmentService clientAssignmentService = new ClientAssignmentService(base.dbConnectionStr);
            DataSet ds = clientAssignmentService.RetrieveClientCallsByProjectId(base.userLoginId, projectId);

            if (ds.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString().Equals("1"))
            {
                gvCallsByDealer.DataSource = ds.Tables[0];
                gvCallsByDealer.DataBind();
            }
            else
            {
                #region Added by OC

                //CommonService CommonService = new CommonService(base.dbConnectionStr);
                //DataSet dsDD = CommonService.RetrieveDealerCodeAndNameByUserID(Session["UserId"].ToString());

                //if (dsDD.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString().Equals("1"))
                //{
                //    DataTable dt = new DataTable();
                //    dt.Columns.Add("AeName");
                //    dt.Columns.Add("NoOfCallsLeft");
                //    dt.Columns.Add("NoOfCallsFollowUp");

                //    DataRow dr = dt.NewRow();
                //    dr["AeName"] = dsDD.Tables[0].Rows[0]["AEName"].ToString();
                //    dr["NoOfCallsLeft"] = 0;
                //    dr["NoOfCallsFollowUp"] = 0;

                //    dt.Rows.Add(dr);

                //    gvCallsByDealer.DataSource = dt;
                //    gvCallsByDealer.DataBind();
                //}
                //else
                //{
                //    divMessage.InnerHtml = dsDD.Tables["ReturnTable"].Rows[0]["ReturnMessage"].ToString();

                //    gvCallsByDealer.DataSource = null;
                //    gvCallsByDealer.DataBind();
                //}

                #endregion

                /// <Updated by OC>
                gvCallsByDealer.DataSource = null;
                gvCallsByDealer.DataBind();
            }
        }

        private void FillInProjectName()
        {
            /// <Updated By OC>
            //DataSet ds = base.RetrieveProjectsByUserId(base.userLoginId, this.txtProjectName.Text);            

            /// <Added by OC>
            DataSet ds = null;
            DataSet dsDealerDetail = ViewState["dsDealerDetail"] as DataSet;
            ClientAssignmentService clientAssignmentService = new ClientAssignmentService(base.dbConnectionStr);

            if (dsDealerDetail != null && dsDealerDetail.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString() == "1")
            {
                if (dsDealerDetail.Tables[0].Rows[0]["Supervisor"].ToString().Trim().ToUpper() == "Y")
                {
                    //ds = clientAssignmentService.RetrieveProjectByProjectName(txtProjectName.Text);
                    ds = clientAssignmentService.RetrieveProjectByProjectNameByUserOrSupervisor("UserType NOT LIKE 'FAR%' AND ProjectDetail.ProjectName LIKE '%" + txtProjectName.Text.Trim() + "%' ", base.userLoginId);
                }
                else if (dsDealerDetail.Tables[0].Rows[0]["Supervisor"].ToString().Trim().ToUpper() == "S")
                {
                    ds = clientAssignmentService.RetrieveProjectByProjectNameByUserOrSupervisor("AEGroup ='" + dsDealerDetail.Tables[0].Rows[0]["TeamCode"].ToString() + "' AND ProjectDetail.ProjectName LIKE '%" + txtProjectName.Text.Trim() + "%' ", base.userLoginId);
                }
                else
                {
                    ds = clientAssignmentService.RetrieveProjectByProjectNameByUserOrSupervisor("AECode ='" + dsDealerDetail.Tables[0].Rows[0]["AECode"].ToString() + "' AND ProjectDetail.ProjectName LIKE '%" + txtProjectName.Text.Trim() + "%' ", base.userLoginId);
                }

                divMessage.InnerHtml = "";
            }
            else
            {
                divMessage.InnerHtml = dsDealerDetail.Tables["ReturnTable"].Rows[0]["ReturnMessage"].ToString();
                return;
            }

            if (ds.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString().Equals("1"))
            {
                Session["ProjectDetailsTable"] = ds.Tables[0];
                this.ddlProjectName.Items.Clear();
                CommonUtilities.BindDataToDropDrownList
                    (ddlProjectName, ds.Tables[0], "ProjectName", "ProjectId", CONST_SELECT_PROJECT);                
                ClearMessages();
                divProjectDetail.Visible = true;
                divCommEarned.Visible = true;
            }
            if (ds.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString().Equals("0"))
            {
                //No project founds!
                ddlProjectName.Items.Clear();
                divNoProjects.InnerHtml = ds.Tables["ReturnTable"].Rows[0]["ReturnMessage"].ToString();
                divProjectDetail.Visible = false;
                divCommEarned.Visible = true;                
            }
            else
            {
                //Error in loading projects
                divMessage.InnerHtml = ds.Tables["ReturnTable"].Rows[0]["ReturnMessage"].ToString();
                divProjectDetail.Visible = false; 
                divCommEarned.Visible = false;
            }
        }

        private void LoadCommissionEarnedByProject(DataTable tblCommissionEarned)
        {
            if (tblCommissionEarned != null && tblCommissionEarned.Rows.Count > 0)
            {
                this.gvCommissionEarned.DataSource = GetFormattedDataTable(tblCommissionEarned);
                this.gvCommissionEarned.DataBind();
            }
        }

        private void LoadProjectInformation()
        {
            DataTable tblProjectDetails = Session["ProjectDetailsTable"] as DataTable;
            String projectId = ddlProjectName.SelectedValue;
            if (tblProjectDetails != null)
            {
                foreach (DataRow dr in tblProjectDetails.Rows)
                {
                    if (String.Equals(dr["ProjectId"].ToString(), projectId))
                    {
                        lblProjectName.Text = dr["ProjectName"].ToString();
                        lblContactPeriod.Text = Convert.ToDateTime(dr["AssignDate"].ToString()).ToShortDateString() + " - "
                                                    + Convert.ToDateTime(dr["CutOffDate"].ToString()).ToShortDateString();

                        Double totalComm = 0.00d;
                        DataSet dsTotalCommForProject = base.RetrieveProjectTotalCommByProjectId(projectId);
                        if (dsTotalCommForProject.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString().Equals("1") &&
                            dsTotalCommForProject.Tables[0].Rows.Count > 0)
                        {
                            totalComm = String.IsNullOrEmpty(dsTotalCommForProject.Tables[0].Rows[0]["TotalCommission"].ToString()) ? 0.00d :
                                            Convert.ToDouble(dsTotalCommForProject.Tables[0].Rows[0]["TotalCommission"].ToString());
                        }                        
                        lblTotalCommission.Text = String.Format("{0:00.00}", totalComm);
                        break;
                    }
                }
            }
        }

        private void LoadChart(DataTable sourceTable)
        {
            chart_CommissionEarned.DataSource = GetChartDataTable(sourceTable);

            // set series members names for the X and Y values 
            chart_CommissionEarned.Series["chartSeries_CommissionEarned"].XValueMember = XChartValue;
            chart_CommissionEarned.Series["chartSeries_CommissionEarned"].YValueMembers = YChartValue;
            chart_CommissionEarned.Series["chartSeries_CommissionEarned"]["PointWidth"] = Convert.ToString(0.6);


            // data bind to the selected data source     
            chart_CommissionEarned.ChartAreas["chartArea_CommissionEarned"].AxisY.Title = "Total";
            chart_CommissionEarned.ChartAreas["chartArea_CommissionEarned"].AxisY.TitleFont = new Font("Times New Roman", 12, FontStyle.Bold);
            chart_CommissionEarned.ChartAreas["chartArea_CommissionEarned"].AxisY.TitleForeColor = Color.Red;

            chart_CommissionEarned.ChartAreas["chartArea_CommissionEarned"].AxisX.ArrowStyle = System.Web.UI.DataVisualization.Charting.AxisArrowStyle.Triangle;
            chart_CommissionEarned.DataBind();            
        }

        public DataTable GetFormattedDataTable(DataTable source)
        {
            DataTable dest = new DataTable(source.TableName);
            dest.Columns.Add("Title");
            foreach (DataColumn c in source.Columns)
            {
                dest.Columns.Add(c.ToString());
            }

            for (int i = 0; i < source.Rows.Count; i++)
            {
                dest.Rows.Add(dest.NewRow());
            }

            for (int r = 0; r < dest.Rows.Count; r++)
            {
                for (int col = 0; col < dest.Columns.Count; col++)
                {
                    if (col == 0)
                    {
                        dest.Rows[r][col] = "Total Earned";
                    }
                    else
                    {
                        String value = !String.IsNullOrEmpty(source.Rows[r][col - 1].ToString()) ? source.Rows[r][col - 1].ToString() : "0.00";
                        dest.Rows[r][col] = String.Format("{0:00.00}", Convert.ToDouble(value));
                    }
                }
            }

            dest.AcceptChanges();
            return dest;
        }

        public DataTable GetChartDataTable(DataTable source)
        {
            DataTable dest = new DataTable(source.TableName);
            dest.Columns.Add(XChartValue);
            dest.Columns.Add(YChartValue);

            for (int i = 0; i < source.Columns.Count; i++)
            {
                dest.Rows.Add(dest.NewRow());
            }

            for (int r = 0; r < dest.Rows.Count; r++)
            {
                for (int c = 0; c < dest.Columns.Count; c++)
                {
                    if (c == 0)
                    {
                        String colName = source.Columns[r].ColumnName;
                        String caption = string.Empty;
                        switch (colName)
                        {
                            case "CashTrading":
                                caption = "CA";
                                break;
                            case "CFD":
                                caption = "CFD";
                                break;
                            case "Custodian":
                                caption = "CU";
                                break;
                            case "CashManagement":
                                caption = "KC";
                                break;
                            case "PhillipMargin":
                                caption = "M";
                                break;
                            case "PhillipFinancial":
                                caption = "PFN";
                                break;
                            case "DiscretionaryAccounts":
                                caption = "S2";
                                break;
                            case "UnitTrustNonWrap":
                                caption = "UT";
                                break;
                            case "AdvisoryAccounts":
                                caption = "UTW";
                                break;
                            default: break;
                        }
                        
                        dest.Rows[r][c] = caption;
                    }
                    else
                    {
                        dest.Rows[r][c] = source.Rows[c - 1][r];
                    }
                }
            }

            dest.AcceptChanges();
            return dest;
        }

        private void ClearMessages()
        {
            divMessage.InnerHtml = String.Empty;
            divNoProjects.InnerHtml = String.Empty;
        }
        #endregion

        #region Events
        protected void gvCommissionEarned_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                double total = 0;
                foreach (Object obj in (e.Row.DataItem as DataRowView).Row.ItemArray)
                {
                    double amt;
                    if (double.TryParse(obj.ToString(), out amt))
                    {
                        total += amt;
                    }
                }
                e.Row.Cells[10].Text = String.Format("{0:00.00}", total);
            }
        }

        protected void txtProjectName_TextChanged(object sender, EventArgs e)
        {
            FillInProjectName();
        }

        protected void ddlProjectName_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadProjectDetails();
        }
        #endregion
    }
}
