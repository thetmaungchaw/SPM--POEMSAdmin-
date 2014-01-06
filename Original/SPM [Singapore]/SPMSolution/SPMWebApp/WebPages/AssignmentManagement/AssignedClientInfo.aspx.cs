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

using SPMWebApp.BasePage;
using SPMWebApp.Services;
using SPMWebApp.Utilities;

namespace SPMWebApp.WebPages.AssignmentManagement
{
    public partial class AssignedClientInfo : BasePage.BasePage
    {
        private ClientAssignmentService clientAssignmentService;

        /// <summary>
        /// Fetch User Access Right
        /// Call to ValidateUserAccessRight method with FunctionCode
        /// </summary>        
        private void LoadUserAccessRight()
        {
            List<AccessRight> accessRightList;
            if (AccessRightUtilities.ValidateUserAccessRight((DataTable)Session["UserAccessRights"], "AssignInfo", out accessRightList))
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
                btnRetrieveAssignment.Enabled = (bool)ViewState["ViewAccessRight"];
                btnExcel.Enabled = (bool)ViewState["ViewAccessRight"];
            }
            catch (Exception e) { }
        }
        /**************Update by TSM**************/
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadUserAccessRight();
                checkAccessRight();
                PrepareForAssignedClientInfo();

                divPaging.Visible = false;
                InitializeRowPerPageSetting();
                ViewState["RowPerPage"] = 20;

                /***to mark the Date radio button****/
                rdoDate.Checked = true;
                txtProjName.Enabled = false;
                ddlProjName.Enabled = false;

                /// <Added by OC>
                CommonService CommonService = new CommonService(base.dbConnectionStr);
                ViewState["dsDealerDetail"] = CommonService.RetrieveDealerCodeAndNameByUserID(Session["UserId"].ToString());
            }

            base.divMessage = divMessage;
            base.gvList = gvClientInfo;
            base.pgcPagingControl = pgcClientInfo;
        }

        protected override DataTable GetDataTable()
        {
            return ViewState["dtClientInfo"] as DataTable;
        }

        protected override void SetCurrentRowPerPage(int rowPerPage)
        {
            ViewState["RowPerPage"] = rowPerPage;
        }

        private void InitializeRowPerPageSetting()
        {
            //Setting for RowPerPage for Custom Paging Control
            pgcClientInfo.StartRowPerPage = 10;
            pgcClientInfo.RowPerPageIncrement = 10;
            pgcClientInfo.EndRowPerPage = 100;
        }

        //For Paging Control
        protected override void DisplayPaging()
        {
            if (divPaging.Visible)
            {
                int rowPerPage = (int)ViewState["RowPerPage"];

                pgcClientInfo.PageCount = gvClientInfo.PageCount;
                pgcClientInfo.CurrentRowPerPage = rowPerPage.ToString();
                pgcClientInfo.DisplayPaging();
            }
        }

        private void PrepareForAssignedClientInfo()
        {
            clientAssignmentService = new ClientAssignmentService(base.dbConnectionStr);
            DataSet ds = null;

            ds = clientAssignmentService.PrepareForAssignedClientInfo(base.userLoginId);
            if (ds.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString() == "1")
            {
                ddlTeamCode.Items.Add(new ListItem("--- Select Team ---", ""));
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    ddlTeamCode.Items.Add(new ListItem(ds.Tables[0].Rows[i]["TeamName"].ToString(), ds.Tables[0].Rows[i]["TeamCode"].ToString()));
                }

                ddlDealer.Items.Add(new ListItem("--- Select Dealer ---", ""));
                for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                {
                    ddlDealer.Items.Add(new ListItem(ds.Tables[1].Rows[i]["DisplayName"].ToString(), ds.Tables[1].Rows[i]["AECode"].ToString()));
                    if (ds.Tables[1].Rows[i]["UserID"].ToString().Equals(base.userLoginId))
                    {
                        ddlDealer.SelectedValue = ds.Tables[1].Rows[i]["AECode"].ToString();
                    }
                }
            }

            calAssignFrom.DateTextFromValue = DateTime.Now.AddDays(-7.0).ToString("dd/MM/yyyy");
            calAssignTo.DateTextFromValue = DateTime.Now.ToString("dd/MM/yyyy");
        }

        /**************Update by TSM**************/
        protected void btnRetrieveAssignment_Click(object sender, EventArgs e)
        {
            string validateResult = "";
            ClientAssignmentService clientAssignmentService = new ClientAssignmentService(base.dbConnectionStr);
            DataSet ds = null;


            if (rdoDate.Checked)
            {
                ddlProjName.Items.Clear();
                txtProjName.Text = "";
                validateResult = CommonUtilities.ValidateDateRange(calAssignFrom.DateTextFromValue.Trim(), calAssignTo.DateTextFromValue.Trim(),
                                        "Assign From Date", "Assign To Date");
                if (String.IsNullOrEmpty(validateResult))
                {
                    //ds = clientAssignmentService.RetrieveAssignmentDateByDateRange(
                    //    DateTime.ParseExact(calAssignFrom.DateTextFromValue, "dd/MM/yyyy", null).ToString("yyyy-MM-dd"),
                    //    DateTime.ParseExact(calAssignTo.DateTextFromValue, "dd/MM/yyyy", null).ToString("yyyy-MM-dd"));

                    DataSet dsDealerDetail = ViewState["dsDealerDetail"] as DataSet;

                    if (dsDealerDetail != null && dsDealerDetail.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString() == "1")
                    {
                        if (dsDealerDetail.Tables[0].Rows[0]["Supervisor"].ToString().ToUpper().Trim() == "Y")
                        {
                            ds = clientAssignmentService.RetrieveAssignmentDateByDateRange(base.userLoginId,
                                DateTime.ParseExact(calAssignFrom.DateTextFromValue, "dd/MM/yyyy", null).ToString("yyyy-MM-dd"),
                                DateTime.ParseExact(calAssignTo.DateTextFromValue, "dd/MM/yyyy", null).ToString("yyyy-MM-dd"));
                        }
                        else if (dsDealerDetail.Tables[0].Rows[0]["Supervisor"].ToString().ToUpper().Trim() == "S")
                        {
                            ds = clientAssignmentService.RetrieveAssignmentDateByDateRangeByUserOrSupervisor(base.userLoginId,
                                DateTime.ParseExact(calAssignFrom.DateTextFromValue, "dd/MM/yyyy", null).ToString("yyyy-MM-dd"),
                                DateTime.ParseExact(calAssignTo.DateTextFromValue, "dd/MM/yyyy", null).ToString("yyyy-MM-dd"),
                                "AEGroup = '" + dsDealerDetail.Tables[0].Rows[0]["TeamCode"].ToString() + "'");
                        }
                        else
                        {
                            ds = clientAssignmentService.RetrieveAssignmentDateByDateRangeByUserOrSupervisor(base.userLoginId,
                                DateTime.ParseExact(calAssignFrom.DateTextFromValue, "dd/MM/yyyy", null).ToString("yyyy-MM-dd"),
                                DateTime.ParseExact(calAssignTo.DateTextFromValue, "dd/MM/yyyy", null).ToString("yyyy-MM-dd"),
                                "AECode = '" + dsDealerDetail.Tables[0].Rows[0]["AECode"].ToString() + "'");
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
                        ddlAssignDate.Items.Clear();
                        CommonUtilities.BindDataToDropDrownList(ddlAssignDate, ds.Tables[0], "AssignDateStr", "AssignDateStr", "--- Select Assignment ---");
                        divMessage.InnerHtml = "";
                    }
                    else
                    {
                        divMessage.InnerHtml = ds.Tables["ReturnTable"].Rows[0]["ReturnMessage"].ToString();
                    }
                }
                else
                {
                    ddlAssignDate.Items.Clear();
                    divMessage.InnerHtml = validateResult;
                }
            } /**end of the check for the AssignDate**/

            if (rdoProj.Checked)
            {
                /*****to clear the ddl and define the date ***/
                ddlAssignDate.Items.Clear();
                calAssignFrom.DateTextFromValue = DateTime.Now.AddDays(-7.0).ToString("dd/MM/yyyy");
                calAssignTo.DateTextFromValue = DateTime.Now.ToString("dd/MM/yyyy");

                string ProjName = txtProjName.Text.ToString();

                /// <Update by OC>
                //ds = clientAssignmentService.RetrieveProjectByProjectName(ProjName);

                /// <Added by OC>
                DataSet dsDealerDetail = ViewState["dsDealerDetail"] as DataSet;

                if (dsDealerDetail != null && dsDealerDetail.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString() == "1")
                {
                    if (dsDealerDetail.Tables[0].Rows[0]["Supervisor"].ToString().Trim().ToUpper() == "Y")
                    {
                        ds = clientAssignmentService.RetrieveProjectByProjectName(ProjName, base.userLoginId);
                    }
                    else if (dsDealerDetail.Tables[0].Rows[0]["Supervisor"].ToString().Trim().ToUpper() == "S")
                    {
                        ds = clientAssignmentService.RetrieveProjectByProjectNameByUserOrSupervisor("AEGroup = '" + dsDealerDetail.Tables[0].Rows[0]["TeamCode"].ToString() + "' AND ProjectDetail.ProjectName LIKE '%" + ProjName + "%'", base.userLoginId);
                    }
                    else
                    {
                        ds = clientAssignmentService.RetrieveProjectByProjectNameByUserOrSupervisor("AECode = '" + dsDealerDetail.Tables[0].Rows[0]["AECode"].ToString() + "' AND ProjectDetail.ProjectName LIKE '%" + ProjName + "%'", base.userLoginId);
                    }

                    divMessage.InnerHtml = "";
                }
                else
                {
                    divMessage.InnerHtml = ds.Tables["ReturnTable"].Rows[0]["ReturnMessage"].ToString();
                    return;
                }

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
        }

        /**************Update by TSM**************/
        protected override Hashtable RetrieveRecords()
        {
            Hashtable ht = new Hashtable();
            clientAssignmentService = new ClientAssignmentService(base.dbConnectionStr);
            DataSet ds = null;
            bool dateFormatFlag = true;
            string assignDate = "";
            ht.Add("ReturnData", null);
            ht.Add("ReturnCode", "");
            ht.Add("ReturnMessage", "");

            if (rdoDate.Checked)
            {
                if ((ddlAssignDate.Items.Count > 0) && (!String.IsNullOrEmpty(ddlAssignDate.SelectedValue)))
                {
                    //try
                    //{
                    //    assignDate = DateTime.ParseExact(ddlAssignDate.SelectedValue, "dd/MM/yyyy", null).ToString("yyyy-MM-dd");                       
                    //}
                    //catch (Exception e)
                    //{
                    //    dateFormatFlag = false;
                    //    ht["ReturnCode"] = "-1";
                    //    ht["ReturnMessage"] = "Assign Date format must be dd/MM/yyyy";
                    //}

                    assignDate = DateTime.Parse(ddlAssignDate.SelectedValue).ToString("yyyy-MM-dd HH:mm:ss.fff");

                    if (dateFormatFlag)
                    {
                        /// <Updated by OC>
                        //ds = clientAssignmentService.RetrieveAssignedClientInfo(ddlTeamCode.SelectedValue, ddlDealer.SelectedValue,
                        //        txtAccountNo.Text.Trim(), assignDate);

                        /// <Added by OC>
                        DataSet dsDealerDetail = ViewState["dsDealerDetail"] as DataSet;

                        if (dsDealerDetail != null && dsDealerDetail.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString() == "1")
                        {
                            if (dsDealerDetail.Tables[0].Rows[0]["Supervisor"].ToString().Trim().ToUpper() == "Y")
                            {
                                ds = clientAssignmentService.RetrieveAssignedClientInfo(ddlTeamCode.SelectedValue, ddlDealer.SelectedValue,
                                        txtAccountNo.Text.Trim(), assignDate);
                            }
                            else if (dsDealerDetail.Tables[0].Rows[0]["Supervisor"].ToString().Trim().ToUpper() == "S")
                            {
                                ds = clientAssignmentService.RetrieveAssignedClientInfoByUserOrSupervisor(assignDate, "AEGroup = '" + dsDealerDetail.Tables[0].Rows[0]["TeamCode"].ToString() + "' ");
                            }
                            else
                            {
                                ds = clientAssignmentService.RetrieveAssignedClientInfoByUserOrSupervisor(assignDate, "AECode = '" + dsDealerDetail.Tables[0].Rows[0]["AECode"].ToString() + "' ");
                            }

                            divMessage.InnerHtml = "";
                        }
                        else
                        {
                            divMessage.InnerHtml = dsDealerDetail.Tables["ReturnTable"].Rows[0]["ReturnMessage"].ToString();
                            ht["ReturnMessage"] = dsDealerDetail.Tables["ReturnTable"].Rows[0]["ReturnMessage"].ToString();
                            return null;
                        }

                        if (ds.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString() == "1")
                        {
                            ht["ReturnData"] = ds.Tables[0];
                            ViewState["dtClientInfo"] = ds.Tables[0];
                            divPaging.Visible = true;
                        }
                        ht["ReturnCode"] = ds.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString();
                        ht["ReturnMessage"] = ds.Tables["ReturnTable"].Rows[0]["ReturnMessage"].ToString();
                    }
                    else
                    {
                        ht["ReturnMessage"] = "Assignment Date Format must be dd/MM/yyyy";
                    }
                }
                else
                {
                    ht["ReturnMessage"] = "Please select the Assignment Date!";
                }

            }
            // for searching by ProjectName
            else if (rdoProj.Checked)
            {
                if (ddlProjName.Text != "")
                {
                    string ProjID = ddlProjName.SelectedValue;

                    /// <Updated by OC>
                    //ds = clientAssignmentService.RetrieveAssignedClientInfoByProj(ProjID);

                    /// <Added by OC>
                    DataSet dsDealerDetail = ViewState["dsDealerDetail"] as DataSet;

                    if (dsDealerDetail != null && dsDealerDetail.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString() == "1")
                    {
                        if (dsDealerDetail.Tables[0].Rows[0]["Supervisor"].ToString().Trim().ToUpper() == "Y")
                        {
                            ds = clientAssignmentService.RetrieveAssignedClientInfoByProj(ProjID);
                        }
                        else if (dsDealerDetail.Tables[0].Rows[0]["Supervisor"].ToString().Trim().ToUpper() == "S")
                        {
                            ds = clientAssignmentService.RetrieveAssignedClientInfoByProjByUserOrSupervisor(ProjID, "AEGroup = '" + dsDealerDetail.Tables[0].Rows[0]["TeamCode"].ToString() + "' ");
                        }
                        else
                        {
                            ds = clientAssignmentService.RetrieveAssignedClientInfoByProjByUserOrSupervisor(ProjID, "AECode = '" + dsDealerDetail.Tables[0].Rows[0]["AECode"].ToString() + "' ");
                        }

                        divMessage.InnerHtml = "";
                    }
                    else
                    {
                        divMessage.InnerHtml = dsDealerDetail.Tables["ReturnTable"].Rows[0]["ReturnMessage"].ToString();
                        ht["ReturnMessage"] = dsDealerDetail.Tables["ReturnTable"].Rows[0]["ReturnMessage"].ToString();
                        return null;
                    }

                    if (ds.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString() == "1")
                    {
                        ViewState["AssignDate"] = assignDate;
                        ViewState["dtClientInfo"] = ds.Tables[0];
                        ht["ReturnData"] = ds.Tables[0];
                        divPaging.Visible = true;
                    }
                    ht["ReturnCode"] = ds.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString();
                    ht["ReturnMessage"] = ds.Tables["ReturnTable"].Rows[0]["ReturnMessage"].ToString();

                }
                else
                {
                    // ht["ReturnMessage"] = "Please select Project Name!";
                    ht["ReturnMessage"] = "Please retrieve Assignments/Projects first before searching AssignedClientInfo.";
                }

            }
            if (ht["ReturnCode"].ToString() != "1")
            {
                //If no record found, clear previous ViewState.
                ViewState["dtClientInfo"] = null;
                divPaging.Visible = false;
            }


            return ht;
        }
        /**************End by TSM**************/

        protected void gvClientInfo_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataTable dtClientInfo = ViewState["dtClientInfo"] as DataTable;
            DataTable sortedDataTable = null;

            string sortDirection = "", sortString = "";

            sortDirection = CommonUtilities.GetSortDirection(e.SortExpression, ViewState["SortExpression"] as string, ViewState["SortDirection"] as string);

            ViewState["SortDirection"] = sortDirection;
            ViewState["SortExpression"] = e.SortExpression;
            sortString = e.SortExpression + " " + sortDirection;

            //dtClientInfo.DefaultView.Sort = e.SortExpression + " " + sortDirection;
            sortedDataTable = CommonUtilities.SortDataTable(dtClientInfo, sortString);

            ViewState["dtClientInfo"] = sortedDataTable;
            gvClientInfo.PageIndex = 0;
            gvClientInfo.DataSource = sortedDataTable;
            gvClientInfo.DataBind();

            DisplayPaging();
            dtClientInfo.Dispose();
        }

        //Generate Excel Report
        protected void btnExcel_Click(object sender, EventArgs e)
        {
            DataTable dtClientInfo = ViewState["dtClientInfo"] as DataTable;
            string[] excelReportReturn = CommonUtilities.GetExcelReport(dtClientInfo, null, null);

            if (excelReportReturn[0] == "1")
            {
                //First clean up the response.object
                Response.Clear();
                Response.Charset = "";

                //Set the response mime type for excel
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("Content-Disposition", "filename=AssignedClientInfo_" + DateTime.Now.ToString("ddMMyyyy_HHmmss") + ".xls");
                Response.Write(excelReportReturn[1]);
                Response.End();
            }
            else
            {
                divMessage.InnerHtml = excelReportReturn[1];
            }
        }


        /**************Update by TSM**************/
        /***to check the radio button ***/
        protected void rdo_CheckedChanged(object sender, EventArgs e)
        {
            txtProjName.Enabled = ((RadioButton)sender).ID.Equals("rdoProj");
            ddlProjName.Enabled = ((RadioButton)sender).ID.Equals("rdoProj");
            PnFromDate.Enabled = ((RadioButton)sender).ID.Equals("rdoDate");
            PnToDate.Enabled = ((RadioButton)sender).ID.Equals("rdoDate");

            if (((RadioButton)sender).ID.Equals("rdoDate"))
            {
                RadioButton r = (RadioButton)sender;
                if (r.Checked)
                    txtProjName.Text = "";
                ddlProjName.Text = "";


            }
            if (((RadioButton)sender).ID.Equals("rdoProj"))
            {
                RadioButton rp = (RadioButton)sender;
                if (rp.Checked)
                {
                    txtProjName.Enabled = true;
                    calAssignFrom.DateTextFromValue = DateTime.Now.AddDays(-7.0).ToString("dd/MM/yyyy");
                    calAssignTo.DateTextFromValue = DateTime.Now.ToString("dd/MM/yyyy");
                }
            }
        }

        /**************End by TSM**************/
    }
}
