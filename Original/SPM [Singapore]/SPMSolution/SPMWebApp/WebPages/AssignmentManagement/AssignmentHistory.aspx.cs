﻿using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Collections.Generic;

using SPMWebApp.Services;
using SPMWebApp.BasePage;
using SPMWebApp.Utilities;

//Import for Excel Report
using System.IO;

namespace SPMWebApp.WebPages.AssignmentManagement
{
    public partial class AssignmentHistory : BasePage.BasePage
    {
        private ClientAssignmentService clientAssignmentService;
        private double retradeTotal = 0.0;
        private string gridViewType = "";

        /// <summary>
        /// Fetch User Access Right
        /// Call to ValidateUserAccessRight method with FunctionCode
        /// </summary>        
        private void LoadUserAccessRight()
        {
            List<AccessRight> accessRightList;
            if (AccessRightUtilities.ValidateUserAccessRight((DataTable)Session["UserAccessRights"], "AssignHist", out accessRightList))
            {
                foreach (AccessRight accessRight in accessRightList)
                {
                    switch (accessRight.accessRightType)
                    {
                        case AccessRightType.Create:
                            {
                                ViewState["CreateAccessRight"] = accessRight.hasAccessRight;
                                break;
                            }
                        case AccessRightType.Modify:
                            {
                                ViewState["ModifyAccessRight"] = accessRight.hasAccessRight;
                                break;
                            }
                        case AccessRightType.View:
                            {
                                ViewState["ViewAccessRight"] = accessRight.hasAccessRight;
                                break;
                            }
                        case AccessRightType.Delete:
                            {
                                ViewState["DeleteAccessRight"] = accessRight.hasAccessRight;
                                break;
                            }
                        default:
                            {
                                break;
                            }
                    }
                }
            }
        }

        protected void checkAccessRight()
        {
            try
            {
                btnSearch.Enabled = (bool)ViewState["ViewAccessRight"];
                btnExcel.Enabled = (bool)ViewState["ViewAccessRight"];
            }
            catch (Exception e) { }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                /// <Added by OC>
                CommonService CommonService = new CommonService(base.dbConnectionStr);
                ViewState["dsDealerDetail"] = CommonService.RetrieveDealerCodeAndNameByUserID(Session["UserId"].ToString());

                LoadUserAccessRight();
                checkAccessRight();
                PrepareForAssignmentHistory();

                divPaging.Visible = false;
                InitializeRowPerPageSetting();
                ViewState["RowPerPage"] = 20;

                //to mark the Date radio button
                rdoDate.Checked = true;
                // txtProjName.Enabled = false;
                ddlProjName.Enabled = false;
            }

            base.gvList = gvAssignments;
            base.divMessage = divMessage;
            base.pgcPagingControl = pgcAssignment;
        }

        protected override DataTable GetDataTable()
        {
            return ViewState["dtAssignments"] as DataTable;
        }

        protected override void SetCurrentRowPerPage(int rowPerPage)
        {
            ViewState["RowPerPage"] = rowPerPage;
        }

        private void InitializeRowPerPageSetting()
        {
            //Setting for RowPerPage for Custom Paging Control
            pgcAssignment.StartRowPerPage = 10;
            pgcAssignment.RowPerPageIncrement = 10;
            pgcAssignment.EndRowPerPage = 100;
        }

        //For Paging Control
        protected override void DisplayPaging()
        {
            if (divPaging.Visible)
            {
                int rowPerPage = (int)ViewState["RowPerPage"];

                pgcAssignment.PageCount = gvAssignments.PageCount;
                pgcAssignment.CurrentRowPerPage = rowPerPage.ToString();
                pgcAssignment.DisplayPaging();
            }
        }

        private void PrepareForAssignmentHistory()
        {
            clientAssignmentService = new ClientAssignmentService(base.dbConnectionStr);

            /// <Updated by OC>
            //DataSet ds = clientAssignmentService.RetrieveAllDealer();

            /// <Added by OC>
            DataSet ds = null;
            DataSet dsDealerDetail = ViewState["dsDealerDetail"] as DataSet;

            if (dsDealerDetail != null && dsDealerDetail.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString() == "1")
            {
                if (dsDealerDetail.Tables[0].Rows[0]["Supervisor"].ToString().Trim().ToUpper() == "Y")
                {
                    ds = clientAssignmentService.RetrieveAllDealer(base.userLoginId);
                }
                else if (dsDealerDetail.Tables[0].Rows[0]["Supervisor"].ToString().Trim().ToUpper() == "S")
                {
                    ds = clientAssignmentService.RetrieveAllDealerByUserOrSupervisor("AEGroup = '" + dsDealerDetail.Tables[0].Rows[0]["TeamCode"].ToString() + "' ", base.userLoginId);
                }
                else
                {
                    ds = clientAssignmentService.RetrieveAllDealerByUserOrSupervisor("AECode = '" + dsDealerDetail.Tables[0].Rows[0]["AECode"].ToString() + "' ", base.userLoginId);
                }

                divMessage.InnerHtml = "";
            }
            else
            {
                divMessage.InnerHtml = dsDealerDetail.Tables["ReturnTable"].Rows[0]["ReturnMessage"].ToString();
                return;
            }

            if (ds.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString() == "1")
            {
                /// <Updated by OC>
                //ddlDealer.Items.Add(new ListItem("--- Select Dealer ---", ""));
                if (dsDealerDetail.Tables[0].Rows[0]["Supervisor"].ToString().Trim().ToUpper() == "Y")
                {
                    ddlDealer.Items.Add(new ListItem("--- Select Dealer ---", ""));
                }

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    ddlDealer.Items.Add(new ListItem(ds.Tables[0].Rows[i]["DisplayName"].ToString(), ds.Tables[0].Rows[i]["AECode"].ToString()));
                    if (ds.Tables[0].Rows[i]["UserID"].ToString().Equals(base.userLoginId))
                    {
                        ddlDealer.SelectedValue = ds.Tables[0].Rows[i]["AECode"].ToString();
                    }
                }
            }

            calAssignFromDate.DateTextFromValue = DateTime.Now.AddDays(-7.0).ToString("dd/MM/yyyy");
            calAssignToDate.DateTextFromValue = DateTime.Now.ToString("dd/MM/yyyy");

            ddlProjName.Items.Clear();
            string ProjName = "";
            CommonService commonService = new CommonService(base.dbConnectionStr);

            /// <Updated by OC>
            //DataSet dsProj = commonService.RetrieveAllProjectByProjectName(ProjName);

            /// <Added by OC>
            DataSet dsProj = null;
            dsDealerDetail = ViewState["dsDealerDetail"] as DataSet;

            if (dsDealerDetail != null && dsDealerDetail.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString() == "1")
            {
                if (dsDealerDetail.Tables[0].Rows[0]["Supervisor"].ToString().Trim().ToUpper() == "Y")
                {
                    /// <Modified by Thet Maung Chaw not to include FAR records.>
                    //dsProj = commonService.RetrieveAllProjectByProjectName(ProjName);
                    dsProj = commonService.RetrieveAllProjectByProjectNameByUserOrSupervisor("UserType NOT LIKE 'FAR%' ", base.userLoginId);
                }
                else if (dsDealerDetail.Tables[0].Rows[0]["Supervisor"].ToString().Trim().ToUpper() == "S")
                {
                    dsProj = commonService.RetrieveAllProjectByProjectNameByUserOrSupervisor("AEGroup = '" + dsDealerDetail.Tables[0].Rows[0]["TeamCode"].ToString() + "' ", base.userLoginId);
                }
                else
                {
                    dsProj = commonService.RetrieveAllProjectByProjectNameByUserOrSupervisor("AECode = '" + dsDealerDetail.Tables[0].Rows[0]["AECode"].ToString() + "' ", base.userLoginId);
                }

                divMessage.InnerHtml = "";
            }
            else
            {
                divMessage.InnerHtml = dsDealerDetail.Tables["ReturnTable"].Rows[0]["ReturnMessage"].ToString();
                return;
            }

            if (dsProj.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString() == "1")
            {
                ddlProjName.Items.Clear();
                CommonUtilities.BindDataToDropDrownList(ddlProjName, dsProj.Tables[0], "ProjectName", "ProjectID", "--- Select Project Name ---");
                divMessage.InnerHtml = "";
            }
            else
            {
                divMessage.InnerHtml = dsProj.Tables["ReturnTable"].Rows[0]["ReturnMessage"].ToString();
            }
        }

        /**************Update by TSM**************/
        protected override Hashtable RetrieveRecords()
        {
            Hashtable ht = new Hashtable();
            clientAssignmentService = new ClientAssignmentService(base.dbConnectionStr);
            DataSet ds = null;
            DataSet dsCommissionEarnedHistory = null;

            string fromDate = "", toDate = "", validateResult = "";
            ht.Add("ReturnData", null);
            ht.Add("ReturnCode", "-1");
            ht.Add("ReturnMessage", "");

            DataSet dsDealerDetail = ViewState["dsDealerDetail"] as DataSet;

            if (rdoDate.Checked)
            {
                validateResult = CommonUtilities.ValidateDateRange(calAssignFromDate.DateTextFromValue.Trim(), calAssignToDate.DateTextFromValue.Trim(),
                                        "Assign From Date", "Assign To Date");
                if (String.IsNullOrEmpty(validateResult))
                {
                    fromDate = DateTime.ParseExact(calAssignFromDate.DateTextFromValue.Trim(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd");
                    toDate = DateTime.ParseExact(calAssignToDate.DateTextFromValue.Trim(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd");

                    if (dsDealerDetail.Tables[0].Rows[0]["Supervisor"].ToString().ToUpper().Trim() == "Y" && ddlDealer.SelectedIndex == 0)
                    {
                        ds = clientAssignmentService.RetrieveAssignmentHistoryByCriteria(ddlDealer.SelectedValue, txtAccountNo.Text.Trim(), txtNric.Text.Trim(),
                                fromDate, toDate, chkRetrade.Checked, "AEGroup = '" + dsDealerDetail.Tables[0].Rows[0]["TeamCode"].ToString() + "' ", base.userLoginId);
                    }
                    else
                    {
                        ds = clientAssignmentService.RetrieveAssignmentHistoryByCriteria(ddlDealer.SelectedValue, txtAccountNo.Text.Trim(), txtNric.Text.Trim(),
                                fromDate, toDate, chkRetrade.Checked, String.Empty, base.userLoginId);
                    }

                    if (ds.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString() == "1")
                    {
                        ViewState["dtAssignments"] = ds.Tables[0];

                        ht["ReturnData"] = ds.Tables[0];
                        divPaging.Visible = true;
                    }

                    ht["ReturnCode"] = ds.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString();
                    ht["ReturnMessage"] = ds.Tables["ReturnTable"].Rows[0]["ReturnMessage"].ToString();

                    //Retrieve Commission Earned History
                    dsCommissionEarnedHistory = clientAssignmentService.RetrieveCommissionEarnedHistoryByCriteria(ddlDealer.SelectedValue, txtAccountNo.Text.Trim(), txtNric.Text.Trim(),
                            fromDate, toDate, chkRetrade.Checked);
                }
                else
                {
                    ht["ReturnMessage"] = validateResult;
                }
            }// end of the check for the AssignDate
            else if (rdoProj.Checked)
            {
                /* if (txtProjName.Text == "")
                 {
                     ht["ReturnMessage"] = "Please choose Project Name!";
                 }*/
                // else
                //{
                string ProjID = ddlProjName.SelectedValue;
                ds = clientAssignmentService.RetrieveAssignmentHistoryByProjectName(ddlDealer.SelectedValue, txtAccountNo.Text.Trim(), txtNric.Text.Trim(),
                            ProjID, chkRetrade.Checked);
                if (ds.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString() == "1")
                {
                    // ViewState["AssignDate"] = assignDate;
                    ViewState["dtAssignments"] = ds.Tables[0];
                    ht["ReturnData"] = ds.Tables[0];
                    divPaging.Visible = true;
                }
                ht["ReturnCode"] = ds.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString();
                ht["ReturnMessage"] = ds.Tables["ReturnTable"].Rows[0]["ReturnMessage"].ToString();

                //Retrieve Commission Earned History
                dsCommissionEarnedHistory = clientAssignmentService.RetrieveCommissionEarnedHistoryByProjectName(ddlDealer.SelectedValue, txtAccountNo.Text.Trim(), txtNric.Text.Trim(),
                            ProjID, chkRetrade.Checked);
            }
            //}

            if (ht["ReturnCode"].ToString() != "1")
            {
                //If no record found, clear previous ViewState.
                ViewState["dtAssignments"] = null;
                divPaging.Visible = false;
            }

            if (dsCommissionEarnedHistory != null)
            {
                Session["CommissionEarnedHistory"] = dsCommissionEarnedHistory;
            }

            return ht;
        }

        public void gvAssignments_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            string formatDate = "", currentLTD = "", assignLTD = "";
            DateTime? contactDt = null;
            DateTime? cutOffDt = null;

            //Displapy color for CurrentLTD > AssignLTD
            if (DataBinder.Eval(e.Row.DataItem, "CurrentLTD") != null)
            {
                currentLTD = DataBinder.Eval(e.Row.DataItem, "CurrentLTD").ToString();
            }

            if (DataBinder.Eval(e.Row.DataItem, "AssignLTD") != null)
            {
                assignLTD = DataBinder.Eval(e.Row.DataItem, "AssignLTD").ToString();
            }

            //Check for Retrade Client
            if ((!String.IsNullOrEmpty(currentLTD)) && (!String.IsNullOrEmpty(assignLTD)))
            {
                //if (DateTime.ParseExact(currentLTD, "dd/MM/yyyy", null).CompareTo(DateTime.ParseExact(assignLTD, "dd/MM/yyyy", null)) > 0)

                if (Convert.ToDateTime(currentLTD).CompareTo(Convert.ToDateTime(assignLTD)) > 0)
                {
                    System.Drawing.Color c = System.Drawing.ColorTranslator.FromHtml("#FFCC99");
                    e.Row.BackColor = c;
                }
            }
            else if (!String.IsNullOrEmpty(currentLTD))
            {
                System.Drawing.Color c = System.Drawing.ColorTranslator.FromHtml("#FFCC99");
                e.Row.BackColor = c;
            }


            //Calculate for ReTrade total
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                double rowTotal = 0.00;
                if (double.TryParse(DataBinder.Eval(e.Row.DataItem, "TotalComm").ToString(), out rowTotal))
                {
                    retradeTotal = retradeTotal + rowTotal;
                }
                //= Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "TotalComm"));
            }

            if (DataBinder.Eval(e.Row.DataItem, "CurrentLTD") != null)
            {
                currentLTD = DataBinder.Eval(e.Row.DataItem, "CurrentLTD").ToString();
            }

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                if (gridViewType == "excel")
                {
                    e.Row.Cells[11].Text = "ReTrade Total";
                    e.Row.Cells[12].Text = String.Format("{0:0.00}", retradeTotal);
                }
                else
                {
                    e.Row.Cells[13].Text = String.Format("{0:0.00}", retradeTotal);
                }
            }

            //Calculate for Miss Called Format            
            if ((DataBinder.Eval(e.Row.DataItem, "AssignDate") != null) && (DataBinder.Eval(e.Row.DataItem, "CutOffDate") != null))
            {
                cutOffDt = Convert.ToDateTime(DataBinder.Eval(e.Row.DataItem, "CutOffDate").ToString());
                if (String.IsNullOrEmpty((DataBinder.Eval(e.Row.DataItem, "LastCallDate").ToString())))
                {
                    contactDt = DateTime.Now;
                }
                else
                {
                    contactDt = Convert.ToDateTime(DataBinder.Eval(e.Row.DataItem, "LastCallDate").ToString());
                }

                if (contactDt.Value.CompareTo(cutOffDt) > 0)
                {
                    System.Drawing.Color c = System.Drawing.ColorTranslator.FromHtml("#C00000");
                    e.Row.BackColor = c;
                }
            }
        }

        protected void gvAssignments_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataTable dtAssignments = ViewState["dtAssignments"] as DataTable;
            DataTable sortedDataTable = null;

            string sortDirection = "", sortString = "";

            sortDirection = CommonUtilities.GetSortDirection(e.SortExpression, ViewState["SortExpression"] as string, ViewState["SortDirection"] as string);

            ViewState["SortDirection"] = sortDirection;
            ViewState["SortExpression"] = e.SortExpression;

            sortString = e.SortExpression + " " + sortDirection;
            sortedDataTable = CommonUtilities.SortDataTable(dtAssignments, sortString);

            ViewState["dtAssignments"] = sortedDataTable;
            gvAssignments.PageIndex = 0;

            //dtAssignments.DefaultView.Sort = e.SortExpression + " " + sortDirection;
            //gvAssignments.DataSource = dtAssignments.DefaultView;

            gvAssignments.DataSource = sortedDataTable;
            gvAssignments.DataBind();
            DisplayPaging();

            dtAssignments.Dispose();
        }

        //Generate Excel Report
        protected void btnExcel_Click(object sender, EventArgs e)
        {
            DataTable dtAssignments = ViewState["dtAssignments"] as DataTable;
            gridViewType = "excel";

            string[] excelReportReturn = CommonUtilities.GetExcelReport(dtAssignments, this, null);

            if (excelReportReturn[0] == "1")
            {
                //First clean up the response.object
                Response.Clear();
                Response.Charset = "";

                //Set the response mime type for excel
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("Content-Disposition", "filename=AssignmentHistory_" + DateTime.Now.ToString("ddMMyyyy_HHmmss") + ".xls");
                Response.Write(excelReportReturn[1]);
                Response.End();
            }
            else
            {
                divMessage.InnerHtml = excelReportReturn[1];
            }
        }

        /**************Update by TSM**************/
        // to check the radio button 
        protected void rdo_CheckedChanged(object sender, EventArgs e)
        {
            // txtProjName.Enabled = ((RadioButton)sender).ID.Equals("rdoProj");
            ddlProjName.Enabled = ((RadioButton)sender).ID.Equals("rdoProj");
            PnFromDate.Enabled = ((RadioButton)sender).ID.Equals("rdoDate");
            PnToDate.Enabled = ((RadioButton)sender).ID.Equals("rdoDate");

            if (((RadioButton)sender).ID.Equals("rdoDate"))
            {
                RadioButton r = (RadioButton)sender;

                if (r.Checked)
                    ddlProjName.Text = "";

                // if(r.Checked)
                //txtProjName.Text = "";               
            }
            if (((RadioButton)sender).ID.Equals("rdoProj"))
            {
                RadioButton rp = (RadioButton)sender;
                if (rp.Checked)
                {
                    //txtProjName.Enabled = true;
                    calAssignFromDate.DateTextFromValue = DateTime.Now.AddDays(-7.0).ToString("dd/MM/yyyy");
                    calAssignToDate.DateTextFromValue = DateTime.Now.ToString("dd/MM/yyyy");
                }
            }
        }

        protected void txtProjName_TextChanged(object sender, EventArgs e)
        {
            //Clear the Date Dropdownlist 
            ddlProjName.Items.Clear();
            string ProjName = "";
            CommonService commonService = new CommonService(base.dbConnectionStr);

            DataSet ds = commonService.RetrieveAllProjectByProjectName(ProjName);

            if (ds.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString() == "1")
            {
                ddlProjName.Items.Clear();
                CommonUtilities.BindDataToDropDrownList(ddlProjName, ds.Tables[0], "ProjectName", "ProjectID", "--- Select Project Name ---");
                divMessage.InnerHtml = "";
            }
            else
            {
                divMessage.InnerHtml = ds.Tables["ReturnTable"].Rows[0]["ReturnMessage"].ToString();
            }

        }

        /**************End by TSM**************/
    }
}