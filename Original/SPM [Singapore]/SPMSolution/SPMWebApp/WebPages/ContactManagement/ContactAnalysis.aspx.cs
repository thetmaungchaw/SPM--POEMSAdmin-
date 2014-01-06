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
using System.IO;
using System.Collections.Generic;

using SPMWebApp.BasePage;
using SPMWebApp.Services;
using SPMWebApp.Utilities;



namespace SPMWebApp.WebPages.ContactManagement
{

    public partial class ContactAnalysis : BasePage.BasePage
    {
        private double retradeTotal = 0.0;
        private string gridViewType = "";
        Boolean flag = false;

        /// <summary>
        /// Fetch User Access Right
        /// Call to ValidateUserAccessRight method with FunctionCode
        /// </summary>        
        private void LoadUserAccessRight()
        {
            List<AccessRight> accessRightList;
            if (AccessRightUtilities.ValidateUserAccessRight((DataTable)Session["UserAccessRights"], "ContactAnalysis", out accessRightList))
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
                //btnRetrieveAssignment.Enabled = (bool)ViewState["ViewAccessRight"];
                btnExcel.Enabled = (bool)ViewState["ViewAccessRight"];
                btnSearch.Enabled = (bool)ViewState["ViewAccessRight"];
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
                PrepareForContactAnalysis();
                divPaging.Visible = false;
                InitializeRowPerPageSetting();
                ViewState["RowPerPage"] = 20;

                //to mark the Date radio button
                RdoContactDate.Checked = true;
                txtProjName.Enabled = false;
                ddlProjName.Enabled = false;
            }
            base.gvList = gvClientContact;
            base.divMessage = divMessage;
            base.pgcPagingControl = pgcClientContact;
            // TextBox1.Attributes.Add("OnKeyPress", "GetKeyPress()");
            txtProjName.Attributes.Add("OnKeyPress", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + txtProjName.ID + "').OnTextChanged();return false;}} else {return true}; ");
        }

        protected override DataTable GetDataTable()
        {
            return ViewState["dtClientContact"] as DataTable;
        }

        protected override void SetCurrentRowPerPage(int rowPerPage)
        {
            ViewState["RowPerPage"] = rowPerPage;
        }

        private void InitializeRowPerPageSetting()
        {
            //Setting for RowPerPage for Custom Paging Control
            pgcClientContact.StartRowPerPage = 10;
            pgcClientContact.RowPerPageIncrement = 10;
            pgcClientContact.EndRowPerPage = 100;
        }

        //For Paging Control
        protected override void DisplayPaging()
        {
            if (divPaging.Visible)
            {
                int rowPerPage = (int)ViewState["RowPerPage"];

                pgcClientContact.PageCount = gvClientContact.PageCount;
                pgcClientContact.CurrentRowPerPage = rowPerPage.ToString();
                pgcClientContact.DisplayPaging();
            }
        }

        private void PrepareForContactAnalysis()
        {
            CommonService commonService = new CommonService(base.dbConnectionStr);
            DataSet ds = null;

            /// <Added by OC>
            DataSet dsDealerDetail = ViewState["dsDealerDetail"] as DataSet;

            if (dsDealerDetail != null && dsDealerDetail.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString() == "1")
            {
                if (dsDealerDetail.Tables[0].Rows[0]["Supervisor"].ToString().Trim().ToUpper() == "Y")
                {
                    ds = commonService.RetrieveAllDealer(base.userLoginId);
                }
                else if (dsDealerDetail.Tables[0].Rows[0]["Supervisor"].ToString().Trim().ToUpper() == "S")
                {
                    ds = commonService.RetrieveAllDealerByUserOrSupervisor("AEGroup = '" + dsDealerDetail.Tables[0].Rows[0]["TeamCode"].ToString() + "'", base.userLoginId);
                }
                else
                {
                    ds = commonService.RetrieveAllDealerByUserOrSupervisor("AECode = '" + dsDealerDetail.Tables[0].Rows[0]["AECode"].ToString() + "'", base.userLoginId);
                }
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
            calContactFromDate.DateTextFromValue = DateTime.Now.AddDays(-7.0).ToString("dd/MM/yyyy");
            calContactToDate.DateTextFromValue = DateTime.Now.ToString("dd/MM/yyyy");
        }




        protected void gvClientContact_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataTable dtClientContact = ViewState["dtClientContact"] as DataTable;
            DataTable sortedDataTable = null;

            string sortDirection = "", sortString = "";

            sortDirection = CommonUtilities.GetSortDirection(e.SortExpression, ViewState["SortExpression"] as string, ViewState["SortDirection"] as string);

            ViewState["SortDirection"] = sortDirection;
            ViewState["SortExpression"] = e.SortExpression;
            sortString = e.SortExpression + " " + sortDirection;

            //dtClientContact.DefaultView.Sort = e.SortExpression + " " + sortDirection;

            sortedDataTable = CommonUtilities.SortDataTable(dtClientContact, sortString);

            ViewState["dtClientContact"] = sortedDataTable;
            gvClientContact.PageIndex = 0;
            gvClientContact.DataSource = sortedDataTable;
            gvClientContact.DataBind();

            DisplayPaging();
            dtClientContact.Dispose();
        }

        public void gvClientContact_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            string currentLTD = "", contactLTD = "";

            if (DataBinder.Eval(e.Row.DataItem, "LTradeDate") != null)
            {
                contactLTD = DataBinder.Eval(e.Row.DataItem, "LTradeDate").ToString();
            }

            if (DataBinder.Eval(e.Row.DataItem, "LastTradeDate") != null)
            {
                currentLTD = DataBinder.Eval(e.Row.DataItem, "LastTradeDate").ToString();
            }

            if ((!String.IsNullOrEmpty(currentLTD)) && (!String.IsNullOrEmpty(contactLTD)))
            {
                if (Convert.ToDateTime(DataBinder.Eval(e.Row.DataItem, "LastTradeDate")).CompareTo(Convert.ToDateTime(DataBinder.Eval(e.Row.DataItem, "LTradeDate"))) > 0)
                {
                    System.Drawing.Color c = System.Drawing.ColorTranslator.FromHtml("#FFCC99");
                    e.Row.BackColor = c;
                }
            }

            //Calculate for ReTrade total
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                //double rowTotal = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "TotalComm"));
                //retradeTotal = retradeTotal + rowTotal;

                double rowTotal = 0.00;
                if (double.TryParse(DataBinder.Eval(e.Row.DataItem, "TotalComm").ToString(), out rowTotal))
                {
                    retradeTotal = retradeTotal + rowTotal;
                }
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                //For Original Grid
                //e.Row.Cells[8].Text = String.Format("{0:0.00}", retradeTotal);

                if (gridViewType == "excel")
                {
                    e.Row.Cells[6].Text = "Total";
                    e.Row.Cells[7].Text = String.Format("{0:0.00}", retradeTotal);
                }
                else
                {
                    e.Row.Cells[8].Text = String.Format("{0:0.00}", retradeTotal);
                }
            }
            else if ((e.Row.RowType == DataControlRowType.Header) &&
                            (gridViewType == "excel"))                      // Adding Header Text for Excel File
            {
                e.Row.Cells[0].Text = "Contact Date";
                e.Row.Cells[1].Text = "Dealer";
                e.Row.Cells[2].Text = "Team";
                e.Row.Cells[3].Text = "A/C";
                e.Row.Cells[4].Text = "Name";
                e.Row.Cells[5].Text = "LTD[Before Contact Date]";
                e.Row.Cells[6].Text = "LTD[Current Date]";
                e.Row.Cells[7].Text = "Total Comm";
            }
        }

        //Generate Excel Report
        protected void btnExcel_Click(object sender, EventArgs e)
        {
            DataTable dtClientContact = ViewState["dtClientContact"] as DataTable;
            gridViewType = "excel";
            string[] excelReportReturn = CommonUtilities.GetExcelReport(dtClientContact, this, null);

            if (excelReportReturn[0] == "1")
            {
                //First clean up the response.object
                Response.Clear();
                Response.Charset = "";

                //Set the response mime type for excel
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("Content-Disposition", "filename=ContactAnalysis_" + DateTime.Now.ToString("ddMMyyyy_HHmmss") + ".xls");
                Response.Write(excelReportReturn[1]);
                Response.End();
            }
            else
            {
                divMessage.InnerHtml = excelReportReturn[1];
            }
        }
        /**************Update by TSM**************/
        //to type the search string in the textbox
        protected void txtProjName_TextChanged(object sender, EventArgs e)
        {
            Binding_toProjDDL();
            flag = true;
        }

        // to check the radio button and text box enable or disable
        protected void rdo_CheckedChanged(object sender, EventArgs e)
        {
            txtProjName.Enabled = ((RadioButton)sender).ID.Equals("rdoProj");
            ddlProjName.Enabled = ((RadioButton)sender).ID.Equals("rdoProj");
            PnFromDate.Enabled = ((RadioButton)sender).ID.Equals("RdoContactDate");
            PnToDate.Enabled = ((RadioButton)sender).ID.Equals("RdoContactDate");

            if (((RadioButton)sender).ID.Equals("RdoContactDate"))
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
                    calContactFromDate.DateTextFromValue = DateTime.Now.AddDays(-7.0).ToString("dd/MM/yyyy");
                    calContactToDate.DateTextFromValue = DateTime.Now.ToString("dd/MM/yyyy");
                }
            }
        }

        //To bind the dropdwon list 
        protected void Binding_toProjDDL()
        {
            ddlProjName.Items.Clear();
            string ProjName = txtProjName.Text.ToString();
            CommonService commonService = new CommonService(base.dbConnectionStr);

            /// <Updated by OC>
            //DataSet ds = commonService.RetrieveAllProjectByProjectName(ProjName);

            /// <Added by OC>
            DataSet ds = null;
            DataSet dsDealerDetail = ViewState["dsDealerDetail"] as DataSet;

            if (dsDealerDetail != null && dsDealerDetail.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString() == "1")
            {
                if (dsDealerDetail.Tables[0].Rows[0]["Supervisor"].ToString().Trim().ToUpper() == "Y")
                {
                    ds = commonService.RetrieveAllProjectByProjectName(ProjName);
                }
                else if (dsDealerDetail.Tables[0].Rows[0]["Supervisor"].ToString().Trim().ToUpper() == "S")
                {
                    ds = commonService.RetrieveAllProjectByProjectNameByUserOrSupervisor("AEGroup = '" + dsDealerDetail.Tables[0].Rows[0]["TeamCode"].ToString() + "' AND ProjectDetail.ProjectName LIKE '%" + ProjName + "%'", base.userLoginId);
                }
                else
                {
                    ds = commonService.RetrieveAllProjectByProjectNameByUserOrSupervisor("AECode = '" + dsDealerDetail.Tables[0].Rows[0]["AECode"].ToString() + "' AND ProjectDetail.ProjectName LIKE '%" + ProjName + "%'", base.userLoginId);
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

        //to show the data records when the searching is successful
        protected override Hashtable RetrieveRecords()
        {
            Hashtable ht = new Hashtable();
            ClientContactService clientContactService = new ClientContactService(base.dbConnectionStr);
            DataSet ds = null;
            string fromDate = "", toDate = "", validateResult = "";

            ht.Add("ReturnData", null);
            ht.Add("ReturnCode", "-1");
            ht.Add("ReturnMessage", "");

            if (RdoContactDate.Checked)
            {
                validateResult = CommonUtilities.ValidateDateRange(calContactFromDate.DateTextFromValue.Trim(), calContactToDate.DateTextFromValue.Trim(),
                                        "Contact From Date", "Contact To Date");
                if (String.IsNullOrEmpty(validateResult))
                {
                    fromDate = DateTime.ParseExact(calContactFromDate.DateTextFromValue.Trim(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd");
                    toDate = DateTime.ParseExact(calContactToDate.DateTextFromValue.Trim(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd");

                    ds = clientContactService.RetrieveContactAnalysis(ddlDealer.SelectedValue, txtAccountNo.Text.Trim(), fromDate, toDate);

                    if (ds.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString() == "1")
                    {
                        ViewState["dtClientContact"] = ds.Tables[0];
                        ht["ReturnData"] = ds.Tables[0];

                        divPaging.Visible = true;
                    }

                    ht["ReturnCode"] = ds.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString();
                    ht["ReturnMessage"] = ds.Tables["ReturnTable"].Rows[0]["ReturnMessage"].ToString();
                }
                else
                {
                    ht["ReturnMessage"] = validateResult;
                }

            }
            else if (rdoProj.Checked && flag == false)
            {
                string temp = ddlProjName.Text;
                if (txtProjName.Text == "" && ddlProjName.Text == "")
                {
                    Binding_toProjDDL();
                }
                else if (txtProjName.Text != "" && ddlProjName.Text == "")
                {
                    // ht["ReturnMessage"] = "Please select Project Name!";
                    ht["ReturnMessage"] = "Please retrieve Assignments/Projects first before searching ContactAnalysis.";


                }
                else if (ddlProjName.Text != "")
                {
                    string projname = ddlProjName.SelectedValue;
                    ds = clientContactService.RetrieveContactAnalysisByProjectName(ddlDealer.SelectedValue, txtAccountNo.Text.Trim(), projname);
                    if (ds.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString() == "1")
                    {
                        // ViewState["AssignDate"] = assignDate;
                        ViewState["dtClientContact"] = ds.Tables[0];
                        ht["ReturnData"] = ds.Tables[0];
                        divPaging.Visible = true;
                    }
                    ht["ReturnCode"] = ds.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString();
                    ht["ReturnMessage"] = ds.Tables["ReturnTable"].Rows[0]["ReturnMessage"].ToString();
                }

            }

            if (ht["ReturnCode"].ToString() != "1")
            {
                ViewState["dtClientContact"] = null;
                divPaging.Visible = false;
            }
            return ht;
        }


        /**************End by TSM**************/








    }
}
