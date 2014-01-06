﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.IO;

using SPMWebApp.BasePage;
using SPMWebApp.Services;
using SPMWebApp.Utilities;

namespace SPMWebApp.WebPages.ContactManagement
{
    public partial class CallReport : BasePage.BasePage
    {
        private ClientContactService clientContactService;
        private string gridViewType = "";
        private int totalAssign = 0, totalMiss = 0, totalExtra = 0, totalReTradeA = 0, totalReTradeE = 0;

        /// <summary>
        /// Fetch User Access Right
        /// Call to ValidateUserAccessRight method with FunctionCode
        /// </summary>        
        private void LoadUserAccessRight()
        {
            List<AccessRight> accessRightList;
            if (AccessRightUtilities.ValidateUserAccessRight((DataTable)Session["UserAccessRights"], "CallRpt", out accessRightList))
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
                btnSearch.Enabled = (bool)ViewState["ViewAccessRight"];
            }
            catch (Exception e) { }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            base.gvList = gvCallReport;
            base.divMessage = divMessage;

            if (!IsPostBack)
            {
                /// <Added by Thet Maung Chaw>
                CommonService CommonService = new CommonService(base.dbConnectionStr);
                ViewState["dsDealerDetail"] = CommonService.RetrieveDealerCodeAndNameByUserID(Session["UserId"].ToString());

                LoadUserAccessRight();
                checkAccessRight();
                divContactInfo.Visible = false;

                divContactPaging.Visible = false;
                divLeadContactPaging.Visible = false;
                divPaging.Visible = false;
                InitializeRowPerPageSetting();
                InitializeContactRowPerPageSetting();
                ViewState["RowPerPage"] = 20;               //For CallReport GridView

                ViewState["ContactInfoRowPerPage"] = 20;    //For ContactInfo GridView
                ViewState["LeadContactInfoRowPerPage"] = 20;

                calAssignFrom.DateTextFromValue = DateTime.Now.AddDays(-7.0).ToString("dd/MM/yyyy");
                calAssignTo.DateTextFromValue = DateTime.Now.ToString("dd/MM/yyyy");

                //to mark the Date radio button
                rdoDate.Checked = true;
                txtProjName.Enabled = false;
                ddlProjName.Enabled = false;
            }

            base.pgcPagingControl = pgcCallReport;
        }

        protected override DataTable GetDataTable()
        {
            return ViewState["dtCallReport"] as DataTable;
        }

        protected override void SetCurrentRowPerPage(int rowPerPage)
        {
            ViewState["RowPerPage"] = rowPerPage;
        }

        private void InitializeRowPerPageSetting()
        {
            //Setting for RowPerPage for Custom Paging Control
            pgcCallReport.StartRowPerPage = 10;
            pgcCallReport.RowPerPageIncrement = 10;
            pgcCallReport.EndRowPerPage = 100;
        }

        private void InitializeContactRowPerPageSetting()
        {
            //Setting for RowPerPage for Custom Paging Control
            pgcContactInfo.StartRowPerPage = 10;
            pgcContactInfo.RowPerPageIncrement = 10;
            pgcContactInfo.EndRowPerPage = 100;

            pgcLeadContactInfo.StartRowPerPage = 10;
            pgcLeadContactInfo.RowPerPageIncrement = 10;
            pgcLeadContactInfo.EndRowPerPage = 100;
        }

        //For Paging Control
        protected override void DisplayPaging()
        {
            if (divPaging.Visible)
            {
                int rowPerPage = (int)ViewState["RowPerPage"];

                pgcCallReport.PageCount = gvCallReport.PageCount;
                pgcCallReport.CurrentRowPerPage = rowPerPage.ToString();
                pgcCallReport.DisplayPaging();
            }


        }



        /**************Update by TSM**************/
        protected override Hashtable RetrieveRecords()
        {
            Hashtable ht = new Hashtable();
            clientContactService = new ClientContactService(base.dbConnectionStr);
            DataSet ds = null;
            string assignDate = "";

            //Hide Contact Detail GridView
            divContactPaging.Visible = false;
            gvContactInfo.DataSource = null;
            gvContactInfo.DataBind();

            divLeadContactPaging.Visible = false;
            gvLeadContactInfo.DataSource = null;
            gvLeadContactInfo.DataBind();

            ht.Add("ReturnData", null);
            ht.Add("ReturnCode", "-1");
            ht.Add("ReturnMessage", "");

            divContactInfo.Visible = false;
            if (rdoDate.Checked)
            {
                if ((ddlAssignDate.Items.Count > 0) && (!String.IsNullOrEmpty(ddlAssignDate.SelectedValue)))
                {
                    /// <Updated by Thet Maung Chaw>
                    //try
                    //{
                    //assignDate = DateTime.ParseExact(ddlAssignDate.SelectedValue, "dd/MM/yyyy", null).ToString("yyyy-MM-dd");
                    //}
                    //catch (Exception e)
                    //{
                    //    assignDate = "error";
                    //}

                    /// <Added by Thet Maung Chaw>
                    assignDate = Convert.ToDateTime(ddlAssignDate.SelectedValue).ToString("yyyy-MM-dd HH:mm:ss.fff");

                    if (assignDate != "error")
                    {
                        /// <Added by Thet Maung Chaw>
                        DataSet dsDealerDetail = ViewState["dsDealerDetail"] as DataSet;

                        if (dsDealerDetail != null && dsDealerDetail.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString() == "1")
                        {
                            if (dsDealerDetail.Tables[0].Rows[0]["Supervisor"].ToString().Trim().ToUpper() == "Y")
                            {
                                ds = clientContactService.RetrieveCallReport(assignDate, base.userLoginId);
                            }
                            else if (dsDealerDetail.Tables[0].Rows[0]["Supervisor"].ToString().Trim().ToUpper() == "S")
                            {
                                ds = clientContactService.RetrieveCallReportByUserOrSupervisor(assignDate, "Team = '" + dsDealerDetail.Tables[0].Rows[0]["TeamCode"].ToString() + "' ", base.userLoginId);
                            }
                            else
                            {
                                ds = clientContactService.RetrieveCallReportByUserOrSupervisor(assignDate, "Dealer = '" + dsDealerDetail.Tables[0].Rows[0]["AECode"].ToString() + "' ", base.userLoginId);
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
                            ViewState["dtCallReport"] = ds.Tables[0];
                            ht["ReturnData"] = ds.Tables[0];
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
            else if (rdoProj.Checked)
            {
                if (ddlProjName.Text != "")
                {
                    string projname = ddlProjName.SelectedValue;

                    /// <Updated by Thet Maung Chaw>
                    //ds = clientContactService.RetrieveCallReportByProjectName(projname);

                    /// <Added by Thet Maung Chaw>
                    DataSet dsDealerDetail = ViewState["dsDealerDetail"] as DataSet;

                    if (dsDealerDetail != null && dsDealerDetail.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString() == "1")
                    {
                        if (dsDealerDetail.Tables[0].Rows[0]["Supervisor"].ToString().Trim().ToUpper() == "Y")
                        {
                            ds = clientContactService.RetrieveCallReportByProjectName(projname, base.userLoginId);
                        }
                        else if (dsDealerDetail.Tables[0].Rows[0]["Supervisor"].ToString().Trim().ToUpper() == "S")
                        {
                            //ds = clientContactService.RetrieveCallReportByProjectNameByUserOrSupervisor(projname, " AND DD.AEGroup = '" + dsDealerDetail.Tables[0].Rows[0]["TeamCode"].ToString() + "' ", base.userLoginId);
                            ds = clientContactService.RetrieveCallReportByProjectNameByUserOrSupervisor(projname, "DD.AEGroup IN (SELECT AEGroup FROM SuperAdmin WHERE UserID='" + base.userLoginId + "') ");
                        }
                        else
                        {
                            //ds = clientContactService.RetrieveCallReportByProjectNameByUserOrSupervisor(projname, " AND DD.AECode = '" + dsDealerDetail.Tables[0].Rows[0]["AECode"].ToString() + "' ", base.userLoginId);
                            ds = clientContactService.RetrieveCallReportByProjectNameByUserOrSupervisor(projname, "DD.AECode IN (SELECT AECode FROM DealerDetail WHERE UserID IN (SELECT UserID FROM SuperAdmin WHERE UserID='" +  base.userLoginId + "'))");
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
                        ViewState["ProjectName"] = projname;
                        ViewState["dtCallReport"] = ds.Tables[0];
                        ht["ReturnData"] = ds.Tables[0];
                        divPaging.Visible = true;
                    }
                    ht["ReturnCode"] = ds.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString();
                    ht["ReturnMessage"] = ds.Tables["ReturnTable"].Rows[0]["ReturnMessage"].ToString();
                }
                else
                {
                    //ht["ReturnMessage"] = "Please select Project Name!";
                    ht["ReturnMessage"] = "Please retrieve Assignments/Projects first before searching Call Report.";
                }
            }
            if (ht["ReturnCode"].ToString() != "1")
            {
                ViewState["dtCallReport"] = null;
                divPaging.Visible = false;
            }

            return ht;
        }

        /**************End TSM**************/
        protected void gvCallReport_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataTable dtCallReport = ViewState["dtCallReport"] as DataTable;
            DataTable sortedDataTable = null;

            string sortDirection = "", sortString = "";

            sortDirection = CommonUtilities.GetSortDirection(e.SortExpression, ViewState["SortExpression"] as string, ViewState["SortDirection"] as string);

            ViewState["SortDirection"] = sortDirection;
            ViewState["SortExpression"] = e.SortExpression;

            sortString = e.SortExpression + " " + sortDirection;
            sortedDataTable = CommonUtilities.SortDataTable(dtCallReport, sortString);

            ViewState["dtCallReport"] = sortedDataTable;
            gvCallReport.PageIndex = 0;

            //dtAssignments.DefaultView.Sort = e.SortExpression + " " + sortDirection;
            //gvAssignments.DataSource = dtAssignments.DefaultView;

            gvCallReport.DataSource = sortedDataTable;
            gvCallReport.DataBind();
            DisplayPaging();

            dtCallReport.Dispose();
        }

        protected void gvCallReport_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Dealer")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                DataTable dtCallReport = (DataTable)ViewState["dtCallReport"];
                clientContactService = new ClientContactService(base.dbConnectionStr);
                DataSet ds = null;

                if (ViewState["AssignDate"] != null)
                {
                    ds = clientContactService.RetrieveCallReportDetail(ViewState["AssignDate"] as string,
                          dtCallReport.Rows[index]["Dealer"].ToString());
                }
                else
                {
                    ds = clientContactService.RetrieveCallReportProjectDetail(ViewState["ProjectName"] as string,
                          dtCallReport.Rows[index]["Dealer"].ToString());
                }



                divContactInfo.Visible = false;

                if (ds.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString() == "1")
                {
                    divContactInfo.Visible = true;
                    divContactPaging.Visible = true;
                    divLeadContactPaging.Visible = true;

                    object objContactInfo = from contactInfo in ds.Tables[0].AsEnumerable()
                                            where contactInfo.Field<String>("projectType").ToUpper().Equals("C")
                                            select contactInfo;

                    object objLeadContactInfo = from leadContact in ds.Tables[0].AsEnumerable()
                                                where leadContact.Field<String>("projectType").ToUpper().Equals("L")
                                                select leadContact;

                    EnumerableRowCollection<DataRow> drContactList = (EnumerableRowCollection<DataRow>)objContactInfo;
                    EnumerableRowCollection<DataRow> drLeadContactList = (EnumerableRowCollection<DataRow>)objLeadContactInfo;

                    DataView dvContactInfo = drContactList.AsDataView();
                    DataView dvLeadContactInfo = drLeadContactList.AsDataView();

                    divClientContact.Visible = false;
                    divLeadContact.Visible = false;

                    if (dvContactInfo.Count > 0)
                    {
                        divClientContact.Visible = false;
                        //Need to check with Row Per Page
                        ViewState["dtContactInfo"] = dvContactInfo.ToTable();// ds.Tables[0];
                        gvContactInfo.DataSource = dvContactInfo.ToTable();// ds.Tables[0];
                        gvContactInfo.DataBind();
                    }
                    if (gvContactInfo.DataSource != null && gvContactInfo.Rows.Count > 0)
                    {
                        divClientContact.Visible = true;
                        DisplayContactInfoPaging();
                    }

                    if (dvLeadContactInfo.Count > 0)
                    {
                        divLeadContact.Visible = false;
                        //Need to check with Row Per Page
                        ViewState["dtLeadContactInfo"] = dvLeadContactInfo.ToTable();// ds.Tables[0];
                        gvLeadContactInfo.DataSource = dvLeadContactInfo.ToTable();// ds.Tables[0];
                        gvLeadContactInfo.DataBind();
                    }
                    if (gvLeadContactInfo.DataSource != null && gvLeadContactInfo.Rows.Count > 0)
                    {
                        divLeadContact.Visible = true;
                        DisplayLeadContactInfoPaging();
                    }
                }
                else
                {
                    divContactPaging.Visible = false;
                    divLeadContactPaging.Visible = false;
                    gvLeadContactInfo.DataSource = null;
                    gvLeadContactInfo.DataBind();
                    gvContactInfo.DataSource = null;
                    gvContactInfo.DataBind();
                }
            }
        }

        protected void gvContactInfo_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataTable dtContactInfo = ViewState["dtContactInfo"] as DataTable;
            DataTable sortedDataTable = null;

            string sortDirection = "", sortString = "";

            sortDirection = CommonUtilities.GetSortDirection(e.SortExpression, ViewState["ContactSortExpression"] as string, ViewState["ContactSortDirection"] as string);

            ViewState["ContactSortDirection"] = sortDirection;
            ViewState["ContactSortExpression"] = e.SortExpression;

            sortString = e.SortExpression + " " + sortDirection;
            sortedDataTable = CommonUtilities.SortDataTable(dtContactInfo, sortString);

            ViewState["dtContactInfo"] = sortedDataTable;
            gvCallReport.PageIndex = 0;

            //dtAssignments.DefaultView.Sort = e.SortExpression + " " + sortDirection;
            //gvAssignments.DataSource = dtAssignments.DefaultView;

            gvContactInfo.DataSource = sortedDataTable;
            gvContactInfo.DataBind();
            DisplayContactInfoPaging();

            dtContactInfo.Dispose();
        }

        protected void gvContactInfo_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string assignDtStr = DataBinder.Eval(e.Row.DataItem, "AssignDate").ToString(),
                    cutOffDtStr = DataBinder.Eval(e.Row.DataItem, "CutOffDate").ToString();

                if (String.IsNullOrEmpty(assignDtStr))
                {
                    e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFCC99");
                }
                else if (!String.IsNullOrEmpty(cutOffDtStr))
                {
                    string contactDtStr = DataBinder.Eval(e.Row.DataItem, "ContactDate").ToString();
                    DateTime contactDt, cutOffDt;

                    if (String.IsNullOrEmpty(contactDtStr))
                    {
                        contactDt = DateTime.Now;
                    }
                    else
                    {
                        contactDt = DateTime.Parse(contactDtStr);
                    }

                    cutOffDt = DateTime.Parse(cutOffDtStr);

                    if (contactDt.CompareTo(cutOffDt) > 0)
                    {
                        e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#C00000");           //Miss Call
                    }
                }
            }
        }

        //Paging Event for Contact Info
        protected void ContactInfo_RowPerPageChanged(object sender, EventArgs<Int32> e)
        {
            DataTable dtContactInfo = ViewState["dtContactInfo"] as DataTable;
            if (dtContactInfo != null)
            {
                gvContactInfo.PageSize = e.Value;
                gvContactInfo.PageIndex = 0;
                gvContactInfo.DataSource = dtContactInfo;
                gvContactInfo.DataBind();


                ViewState["ContactInfoRowPerPage"] = e.Value;
                DisplayContactInfoPaging();

                //pgcContactInfo.PageCount = gvContactInfo.PageCount;
                //pgcContactInfo.DisplayPaging();
            }
        }

        protected void ContactInfo_PageNoChange(object sender, EventArgs<Int32> e)
        {
            gvContactInfo.PageIndex = e.Value - 1;
            DataTable dtContactInfo = ViewState["dtContactInfo"] as DataTable;

            if (dtContactInfo != null)
            {
                gvContactInfo.DataSource = dtContactInfo;
                gvContactInfo.DataBind();
            }
        }

        //For Paging Control
        private void DisplayContactInfoPaging()
        {
            if (divContactPaging.Visible)
            {
                int rowPerPage = (int)ViewState["ContactInfoRowPerPage"];

                pgcContactInfo.PageCount = gvContactInfo.PageCount;
                pgcContactInfo.CurrentRowPerPage = rowPerPage.ToString();
                pgcContactInfo.DisplayPaging();
            }
        }

        //Paging Event for Contact Info
        protected void LeadContactInfo_RowPerPageChanged(object sender, EventArgs<Int32> e)
        {
            DataTable dtContactInfo = ViewState["dtLeadContactInfo"] as DataTable;
            if (dtContactInfo != null)
            {
                gvLeadContactInfo.PageSize = e.Value;
                gvLeadContactInfo.PageIndex = 0;
                gvLeadContactInfo.DataSource = dtContactInfo;
                gvLeadContactInfo.DataBind();


                ViewState["LeadContactInfoRowPerPage"] = e.Value;
                DisplayLeadContactInfoPaging();

                //pgcContactInfo.PageCount = gvContactInfo.PageCount;
                //pgcContactInfo.DisplayPaging();
            }
        }

        protected void LeadContactInfo_PageNoChange(object sender, EventArgs<Int32> e)
        {
            gvLeadContactInfo.PageIndex = e.Value - 1;
            DataTable dtContactInfo = ViewState["dtLeadContactInfo"] as DataTable;

            if (dtContactInfo != null)
            {
                gvLeadContactInfo.DataSource = dtContactInfo;
                gvLeadContactInfo.DataBind();
            }
        }

        //For Lead Contact Paging Control
        private void DisplayLeadContactInfoPaging()
        {
            if (divLeadContactPaging.Visible)
            {
                int rowPerPage = (int)ViewState["LeadContactInfoRowPerPage"];

                pgcLeadContactInfo.PageCount = gvLeadContactInfo.PageCount;
                pgcLeadContactInfo.CurrentRowPerPage = rowPerPage.ToString();
                pgcLeadContactInfo.DisplayPaging();
            }
        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            gridViewType = "excel";
            DataTable dtCallReport = ViewState["dtCallReport"] as DataTable;

            string[] excelReportReturn = null;
            List<string> columnNames = new List<string>();

            //if ((dtCallReport != null) && (dtCallReport.Rows.Count > 0))
            //{
            //    for (int i = 1; i < gvCallReport.HeaderRow.Cells.Count; i++)
            //    {
            //        columnNames.Add(gvCallReport.HeaderRow.Cells[i].Text.Trim());
            //    }
            //}

            excelReportReturn = CommonUtilities.GetExcelReport(dtCallReport, this, null);
            if (excelReportReturn[0] == "1")
            {
                //First clean up the response.object
                Response.Clear();
                Response.Charset = "";

                //Set the response mime type for excel
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("Content-Disposition", "filename=CallReport_" + DateTime.Now.ToString("ddMMyyyy_HHmmss") + ".xls");
                Response.Write(excelReportReturn[1]);
                Response.End();
            }
            else
            {
                divMessage.InnerHtml = excelReportReturn[1];
            }
        }


        public void gvCallReport_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Calculate for Total Value
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                totalAssign += Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "TotalAssign"));
                totalMiss += Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "Miss"));
                //totalExtra += Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "Extra"));
                if (DataBinder.Eval(e.Row.DataItem, "ReTradeA") == "" || DataBinder.Eval(e.Row.DataItem, "ReTradeA") == DBNull.Value)
                {
                    totalReTradeA += 0;
                }
                else
                {
                    totalReTradeA += Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "ReTradeA"));
                }
                //if (DataBinder.Eval(e.Row.DataItem, "ReTradeE") == "" || DataBinder.Eval(e.Row.DataItem, "ReTradeE") == DBNull.Value)
                //{
                //    totalReTradeE += 0;
                //}
                //else
                //{
                //totalReTradeE += Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "ReTradeE"));
                //}
            }

            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                if (gridViewType == "excel")
                {
                    e.Row.Cells[2].Text = "Total";
                    e.Row.Cells[3].Text = totalAssign.ToString();
                    e.Row.Cells[4].Text = totalMiss.ToString();
                    //e.Row.Cells[5].Text = totalExtra.ToString();
                    e.Row.Cells[5].Text = totalReTradeA.ToString(); //6
                    //e.Row.Cells[6].Text = totalReTradeE.ToString(); //7
                }
                else
                {
                    e.Row.Cells[4].Text = totalAssign.ToString();
                    e.Row.Cells[5].Text = totalMiss.ToString();
                    //e.Row.Cells[6].Text = totalExtra.ToString(); //6
                    e.Row.Cells[6].Text = totalReTradeA.ToString(); //7
                    //e.Row.Cells[8].Text = totalReTradeE.ToString();
                }
            }
            else if ((e.Row.RowType == DataControlRowType.Header) &&
                            (gridViewType == "excel"))                      // Adding Header Text for Excel File
            {
                e.Row.Cells[0].Text = "Team";
                e.Row.Cells[1].Text = "Dealer";
                e.Row.Cells[2].Text = "Dealer Name";
                e.Row.Cells[3].Text = "Total Assign";
                e.Row.Cells[4].Text = "Miss";
                //e.Row.Cells[5].Text = "Extra";
                e.Row.Cells[5].Text = "ReTrade(Assign)"; //6
                //e.Row.Cells[6].Text = "ReTrade(Extra)"; //7
                //e.Row.Cells[8].Text = "Score";
            }
        }

        /**************Update by TSM**************/
        // To bind data to the Assigndate and Project Name dropdownlists
        protected void btnRetrieveAssignment_Click(object sender, EventArgs e)
        {
            string validateResult = "";

            ClientAssignmentService clientAssignmentService = new ClientAssignmentService(base.dbConnectionStr);
            DataSet ds = null;

            // check the radiio button
            if (rdoDate.Checked)
            {
                ddlProjName.Items.Clear();
                txtProjName.Text = "";
                validateResult = CommonUtilities.ValidateDateRange(calAssignFrom.DateTextFromValue.Trim(), calAssignTo.DateTextFromValue.Trim(),
                                         "Assign From Date", "Assign To Date");
                if (String.IsNullOrEmpty(validateResult))
                {
                    /// <Updated by Thet Maung Chaw>
                    /// <If the user is Administrator, everything will show.
                    /// If the user is Supervisor, only his team will show.
                    /// If the user is just the user, then only his records will show.
                    /// "Param" will be "AECode = 'ITTEST'" or "AEGroup = 'T10'".>

                    /// <Original>
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
            }

            if (rdoProj.Checked)
            {
                //Clear the Date Dropdownlist 
                ddlAssignDate.Items.Clear();
                calAssignFrom.DateTextFromValue = DateTime.Now.AddDays(-7.0).ToString("dd/MM/yyyy");
                calAssignTo.DateTextFromValue = DateTime.Now.ToString("dd/MM/yyyy");
                string ProjName = txtProjName.Text.ToString();

                /// <Original>
                //ds = clientAssignmentService.RetrieveProjectByProjectName(ProjName);

                /// <Added by Thet Maung Chaw>
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
        /**************End TSM**************/

        /**************Update by TSM**************/
        // to check the radio button 
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

        protected void gvLeadContactInfo_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataTable dtContactInfo = ViewState["dtLeadContactInfo"] as DataTable;
            DataTable sortedDataTable = null;

            string sortDirection = "", sortString = "";

            sortDirection = CommonUtilities.GetSortDirection(e.SortExpression, ViewState["ContactLeadSortExpression"] as string, ViewState["ContactLeadSortDirection"] as string);

            ViewState["ContactLeadSortDirection"] = sortDirection;
            ViewState["ContactLeadSortExpression"] = e.SortExpression;

            sortString = e.SortExpression + " " + sortDirection;
            sortedDataTable = CommonUtilities.SortDataTable(dtContactInfo, sortString);

            ViewState["dtLeadContactInfo"] = sortedDataTable;
            gvCallReport.PageIndex = 0;

            gvLeadContactInfo.DataSource = sortedDataTable;
            gvLeadContactInfo.DataBind();
            DisplayLeadContactInfoPaging();

            dtContactInfo.Dispose();
        }

        protected void gvLeadContactInfo_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string assignDtStr = DataBinder.Eval(e.Row.DataItem, "AssignDate").ToString(),
                    cutOffDtStr = DataBinder.Eval(e.Row.DataItem, "CutOffDate").ToString();

                if (String.IsNullOrEmpty(assignDtStr))
                {
                    e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFCC99");
                }
                else if (!String.IsNullOrEmpty(cutOffDtStr))
                {
                    string contactDtStr = DataBinder.Eval(e.Row.DataItem, "ContactDate").ToString();
                    DateTime contactDt, cutOffDt;

                    if (String.IsNullOrEmpty(contactDtStr))
                    {
                        contactDt = DateTime.Now;
                    }
                    else
                    {
                        contactDt = DateTime.Parse(contactDtStr);
                    }

                    cutOffDt = DateTime.Parse(cutOffDtStr);

                    if (contactDt.CompareTo(cutOffDt) > 0)
                    {
                        e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#C00000");           //Miss Call
                    }
                }
            }
        }
        /**************End by TSM**************/
    }
}