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
using System.Collections.Generic;
using SPMWebApp.Services;
using SPMWebApp.BasePage;
using SPMWebApp.Utilities;

namespace SPMWebApp.WebPages.ContactManagement
{
    public partial class ContactHistory : BasePage.BasePage
    {
        private ClientContactService clientContactService;
        private string gridViewType = "";
        Boolean flag = false;

        /// <summary>
        /// Fetch User Access Right
        /// Call to ValidateUserAccessRight method with FunctionCode
        /// </summary>        
        private void LoadUserAccessRight()
        {
            List<AccessRight> accessRightList;
            if (AccessRightUtilities.ValidateUserAccessRight((DataTable)Session["UserAccessRights"], "ContactHist", out accessRightList))
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
            catch (Exception e)
            {
            }


        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                /// <Added by OC>
                CommonService CommonService = new CommonService(base.dbConnectionStr);
                ViewState["dsDealerDetail"] = CommonService.RetrieveDealerCodeAndNameByUserID(Session["UserId"].ToString());

                checkAccessRight();

                //divCHPaging.Visible = false;
                PrepareForContactHistory();

                divPaging.Visible = false;
                InitializeRowPerPageSetting();
                ViewState["RowPerPage"] = 20;

                //to mark the Date radio button
                rdoContactDate.Checked = true;
                txtProjName.Enabled = false;
                ddlProjName.Enabled = false;

                //Fill Rank DropdownList
                CommonService commonService = new CommonService();
                string[,] clientRanks = commonService.RetrieveClientRanks();
                CommonUtilities.BindDataToDropDrownList(ddlRank, clientRanks, 0, 1, "--- Select Rank ---");
            }
            //txtProjName.Attributes.Add("OnKeyPress", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnSearch + "').Onclick();return false;}} else {return true}; ");
            base.gvList = gvContactHistory;
            base.pgcPagingControl = pgcContactHistory;
        }

        protected override DataTable GetDataTable()
        {
            return ViewState["dtContactHistory"] as DataTable;
        }

        protected override void SetCurrentRowPerPage(int rowPerPage)
        {
            ViewState["RowPerPage"] = rowPerPage;
        }

        private void InitializeRowPerPageSetting()
        {
            //Setting for RowPerPage for Custom Paging Control
            pgcContactHistory.StartRowPerPage = 10;
            pgcContactHistory.RowPerPageIncrement = 10;
            pgcContactHistory.EndRowPerPage = 100;
        }

        //For Paging Control
        private void DisplayPaging()
        {
            if (divPaging.Visible)
            {
                int rowPerPage = (int)ViewState["RowPerPage"];

                pgcContactHistory.PageCount = gvContactHistory.PageCount;
                pgcContactHistory.CurrentRowPerPage = rowPerPage.ToString();
                pgcContactHistory.DisplayPaging();
            }
        }

        private void PrepareForContactHistory()
        {
            clientContactService = new ClientContactService(base.dbConnectionStr);
            DataSet ds = null;

            /// <Updated by OC>
            //ds = clientContactService.PrepareForContactHistory();

            /// <Added by OC>
            DataSet dsDealerDetail = ViewState["dsDealerDetail"] as DataSet;

            if (dsDealerDetail != null && dsDealerDetail.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString() == "1")
            {
                if (dsDealerDetail.Tables[0].Rows[0]["Supervisor"].ToString().Trim().ToUpper() == "Y")
                {
                    //ds = clientContactService.PrepareForContactHistory();
                    ds = clientContactService.PrepareForContactHistoryByUserOrSupervisor("UserType LIKE 'FAR%' ", base.userLoginId);
                }
                else if (dsDealerDetail.Tables[0].Rows[0]["Supervisor"].ToString().Trim().ToUpper() == "S")
                {
                    ds = clientContactService.PrepareForContactHistoryByUserOrSupervisor("AEGroup = '" + dsDealerDetail.Tables[0].Rows[0]["TeamCode"].ToString() + "' ", base.userLoginId);
                }
                else
                {
                    ds = clientContactService.PrepareForContactHistoryByUserOrSupervisor("AECode = '" + dsDealerDetail.Tables[0].Rows[0]["AECode"].ToString() + "' ", base.userLoginId);
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
                //ddlDealer.DataSource = ds.Tables[0];
                //ddlDealer.DataTextField = "DisplayName";
                //ddlDealer.DataValueField = "AECode";
                //ddlDealer.DataBind();

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


                //ddlPreference.DataSource = ds.Tables[1];
                //ddlPreference.DataTextField = "OptionDisplay";
                //ddlPreference.DataValueField = "OptionNo";
                //ddlPreference.DataBind();

                ddlPreference.Items.Add(new ListItem("--- Select Preference ---", ""));
                for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                {
                    ddlPreference.Items.Add(new ListItem(ds.Tables[1].Rows[i]["OptionDisplay"].ToString(), ds.Tables[1].Rows[i]["OptionNo"].ToString()));
                }

                ddlTeam.Items.Add(new ListItem("--- Select Team ---", ""));
                for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                {
                    ddlTeam.Items.Add(new ListItem(ds.Tables[2].Rows[i]["TeamName"].ToString(), ds.Tables[2].Rows[i]["TeamCode"].ToString()));
                }

                //Set OptionNo as Primary Key to use in Client Preferences searching.
                DataColumn[] dcPk = new DataColumn[1];
                dcPk[0] = ds.Tables[1].Columns["OptionNo"];
                ds.Tables[1].PrimaryKey = dcPk;
                ViewState["dtPreferList"] = ds.Tables[1];

                calContactFromDate.DateTextFromValue = DateTime.Now.AddDays(-7.0).ToString("dd/MM/yyyy");
                calContactToDate.DateTextFromValue = DateTime.Now.ToString("dd/MM/yyyy");
            }
        }

        public void gvContactHistory_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string cutOffDtStr = "", clientPrefer = "";
                DataTable dtPreferList = ViewState["dtPreferList"] as DataTable;
                DataRow drPreference = null;


                //Check for Extra Call
                if (String.IsNullOrEmpty(DataBinder.Eval(e.Row.DataItem, "AssignDate").ToString()))
                {
                    System.Drawing.Color c = System.Drawing.ColorTranslator.FromHtml("#FFCC99");
                    e.Row.BackColor = c;
                }

                if (DataBinder.Eval(e.Row.DataItem, "PreferA") != null)
                {
                    drPreference = dtPreferList.Rows.Find(DataBinder.Eval(e.Row.DataItem, "PreferA").ToString().Trim());
                    if (drPreference != null)
                    {
                        clientPrefer = drPreference["OptionContent"].ToString();
                    }
                }

                if (DataBinder.Eval(e.Row.DataItem, "PreferB") != null)
                {
                    if (!String.IsNullOrEmpty(DataBinder.Eval(e.Row.DataItem, "PreferB").ToString().Trim()))
                    {
                        if (!String.IsNullOrEmpty(clientPrefer))
                        {
                            clientPrefer = clientPrefer + "<br />";
                        }

                        drPreference = dtPreferList.Rows.Find(DataBinder.Eval(e.Row.DataItem, "PreferB").ToString().Trim());
                        if (drPreference != null)
                        {
                            clientPrefer = clientPrefer + drPreference["OptionContent"].ToString();

                            //Client Preference List
                            if (gridViewType == "excel")
                            {
                                e.Row.Cells[8].Text = clientPrefer;
                            }
                            else
                            {
                                //e.Row.Cells[9].Text = clientPrefer;
                                ((Label)e.Row.FindControl("gvlblPreferences")).Text = clientPrefer;
                            }
                        }
                    }
                }

                //Rank Text will generate from SQL

                if (DataBinder.Eval(e.Row.DataItem, "Rank") != null)
                {
                    if (gridViewType == "excel")
                        e.Row.Cells[9].Text = CommonUtilities.GetClientRank(DataBinder.Eval(e.Row.DataItem, "Rank").ToString());
                    //else
                    //    e.Row.Cells[10].Text = CommonUtilities.GetClientRank(DataBinder.Eval(e.Row.DataItem, "Rank").ToString());
                }

                /// <Added by Thet Maung Chaw >
                /// <To prevent "0" in the Cells.>
                String Cell8 = e.Row.Cells[8].Text;
                e.Row.Cells[8].Text = (Cell8 == "0" ? "" : Cell8);
            }

            if (gridViewType == "excel")
            {
                e.Row.Cells[10].Visible = false;        //Hide PreferA and PreferB
                e.Row.Cells[12].Visible = false;        //Hide AssignDate
                e.Row.Cells[14].Visible = false;        //Hide RankText
            }

            //Adding Header Text for Excel File
            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (gridViewType == "excel")
                {
                    e.Row.Cells[0].Text = "Contact Date";
                    e.Row.Cells[1].Text = "Team";
                    e.Row.Cells[2].Text = "A/C";
                    e.Row.Cells[3].Text = "Name";
                    e.Row.Cells[4].Text = "Gender";
                    e.Row.Cells[5].Text = "Contact No";

                    e.Row.Cells[8].Text = "Preferences";
                    e.Row.Cells[9].Text = "Rank";
                }
            }
        }

        protected void gvContactHistory_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataTable dtClientContact = ViewState["dtContactHistory"] as DataTable;
            DataTable sortedDataTable = null;

            string sortDirection = "", sortString = "";

            sortDirection = CommonUtilities.GetSortDirection(e.SortExpression, ViewState["SortExpression"] as string, ViewState["SortDirection"] as string);

            ViewState["SortDirection"] = sortDirection;
            ViewState["SortExpression"] = e.SortExpression;
            sortString = e.SortExpression + " " + sortDirection;
            sortedDataTable = CommonUtilities.SortDataTable(dtClientContact, sortString);

            ViewState["dtContactHistory"] = sortedDataTable;
            gvContactHistory.PageIndex = 0;
            gvContactHistory.DataSource = sortedDataTable;
            gvContactHistory.DataBind();
            DisplayPaging();

            dtClientContact.Dispose();
        }

        protected void btnContactHistorySearch_Click(object sender, EventArgs e)
        {
            clientContactService = new ClientContactService(base.dbConnectionStr);
            DataSet ds = null;
            Hashtable ht = new Hashtable();
            string fromDate = "", toDate = "", validateResult = "";

            if (rdoContactDate.Checked)
            {
                validateResult = CommonUtilities.ValidateDateRange(calContactFromDate.DateTextFromValue.Trim(), calContactToDate.DateTextFromValue.Trim(),
                                    "Contact From Date", "Contact To Date");
                if (String.IsNullOrEmpty(validateResult))
                {
                    fromDate = DateTime.ParseExact(calContactFromDate.DateTextFromValue, "dd/MM/yyyy", null).ToString("yyyy-MM-dd");
                    toDate = DateTime.ParseExact(calContactToDate.DateTextFromValue, "dd/MM/yyyy", null).ToString("yyyy-MM-dd") + " 23:59:59";

                    ds = clientContactService.RetrieveContactHistoryByCriteria(txtAccountNo.Text.Trim(), ddlDealer.SelectedValue,
                            fromDate, toDate, ddlRank.SelectedValue, ddlPreference.SelectedValue, txtContent.Text.Trim(), ddlTeam.SelectedValue, base.userLoginId);

                    if (ds.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString() == "1")
                    {
                        ViewState["dtContactHistory"] = ds.Tables[0];
                        gvContactHistory.PageIndex = 0;
                        gvContactHistory.DataSource = ds.Tables[0];
                        gvContactHistory.DataBind();
                        divPaging.Visible = true;
                        DisplayPaging();
                    }
                    else
                    {
                        validateResult = "-1";
                    }

                    divMessage.InnerHtml = ds.Tables["ReturnTable"].Rows[0]["ReturnMessage"].ToString();
                }
                else
                {
                    divMessage.InnerHtml = validateResult;
                }

            }
            else if (rdoProj.Checked && flag == false)
            {

                if (txtProjName.Text == "" && ddlProjName.Text == "")
                {
                    Binding_toProjDDL();
                }
                else if (txtProjName.Text != "" && ddlProjName.Text == "")
                {
                    //divMessage.InnerHtml = "Please select Project Name!";
                    divMessage.InnerHtml = "Please retrieve Assignments/Projects first before searching ContactHistory.";

                }
                else if (ddlProjName.Text != "")
                {

                    string projname = ddlProjName.SelectedValue;
                    ds = clientContactService.RetrieveContactHistoryByProjName(txtAccountNo.Text.Trim(), ddlDealer.SelectedValue,
                              projname, ddlRank.SelectedValue, ddlPreference.SelectedValue, txtContent.Text.Trim(), ddlTeam.SelectedValue, base.userLoginId);

                    if (ds.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString() == "1")
                    {
                        ViewState["dtContactHistory"] = ds.Tables[0];
                        gvContactHistory.PageIndex = 0;
                        gvContactHistory.DataSource = ds.Tables[0];
                        gvContactHistory.DataBind();
                        divPaging.Visible = true;
                        DisplayPaging();
                    }
                    else
                    {
                        validateResult = "-1";
                    }

                    divMessage.InnerHtml = ds.Tables["ReturnTable"].Rows[0]["ReturnMessage"].ToString();
                }
            }

            if (!String.IsNullOrEmpty(validateResult))
            {
                ViewState["dtContactHistory"] = null;
                gvContactHistory.DataSource = null;
                gvContactHistory.DataBind();
                divPaging.Visible = false;
            }
        }

        //Generate Excel Report
        protected void btnExcel_Click(object sender, EventArgs e)
        {

            DataTable dtContactHistory = ViewState["dtContactHistory"] as DataTable;

            gridViewType = "excel";
            string[] excelReportReturn = null;
            List<string> columnNames = new List<string>();

            /*
            if ((dtContactHistory != null) && (dtContactHistory.Rows.Count > 0))
            {
                for (int i = 1; i < gvContactHistory.HeaderRow.Cells.Count; i++)
                {
                    columnNames.Add(gvContactHistory.HeaderRow.Cells[i].Text.Trim());
                }
            }   
            */

            excelReportReturn = CommonUtilities.GetExcelReport(dtContactHistory, this, null);

            if (excelReportReturn[0] == "1")
            {
                //First clean up the response.object
                Response.Clear();
                Response.Charset = "";
                //Set the response mime type for excel
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("Content-Disposition", "filename=ContactHistory_" + DateTime.Now.ToString("ddMMyyyy_HHmmss") + ".xls");
                Response.Write(excelReportReturn[1]);
                Response.End();
            }
            else
            {
                divMessage.InnerHtml = excelReportReturn[1];
            }


            //Export with Current Grid
            /*
            string excelReportData = "";
            if ((dtContactHistory != null) && (dtContactHistory.Rows.Count > 0))
            {
                excelReportData = this.ExportToExcelWithCurrentGridView();
                //First clean up the response.object
                Response.Clear();
                Response.Charset = "";

                //Set the response mime type for excel
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("Content-Disposition", "filename=ContactHistory_" + DateTime.Now.ToString("ddMMyyyy_HHmmss") + ".xls");
                Response.Write(excelReportData);
                Response.End();
            }
            */
        }

        /*
        //Export with Current Grid
        private string ExportToExcelWithCurrentGridView()
        {
            StringWriter stringWriter = new StringWriter();
            HtmlTextWriter htmlWriter = new HtmlTextWriter(stringWriter);

            PrepareGridViewForExport(gvContactHistory);
            gvContactHistory.RenderControl(htmlWriter);
            return stringWriter.ToString();
        }

        public override void VerifyRenderingInServerForm(Control control)
        {

        }

        private void PrepareGridViewForExport(Control gv)
        {
            LinkButton lb = new LinkButton();
            Literal l = new Literal();
            string name = String.Empty;

            for (int i = 0; i < gv.Controls.Count; i++)
            {
                if (gv.Controls[i].GetType() == typeof(LinkButton))
                {
                    l.Text = (gv.Controls[i] as LinkButton).Text;
                    gv.Controls.Remove(gv.Controls[i]);
                    gv.Controls.AddAt(i, l);
                }
                else if (gv.Controls[i].GetType() == typeof(DropDownList))
                {
                    l.Text = (gv.Controls[i] as DropDownList).SelectedItem.Text;
                    gv.Controls.Remove(gv.Controls[i]);
                    gv.Controls.AddAt(i, l);
                }
                else if (gv.Controls[i].GetType() == typeof(CheckBox))
                {
                    l.Text = (gv.Controls[i] as CheckBox).Checked ? "True" : "False";
                    gv.Controls.Remove(gv.Controls[i]);
                    gv.Controls.AddAt(i, l);
                }
                if (gv.Controls[i].HasControls())
                {
                    PrepareGridViewForExport(gv.Controls[i]);
                }
            }
        }
        */


        /* old paging code *\
        /*
        private void DisplayPaging()
        {
            int pageCount = gvContactHistory.PageCount;

            if (pageCount > 0)
            {
                ddlCHPageNo.Items.Clear();
                for (int i = 0; i < pageCount; i++)
                {
                    ddlCHPageNo.Items.Add(new ListItem((i + 1) + "", i + ""));
                }
                ddlCHPageNo.SelectedValue = base.GetGridViewCurrentPage() + "";
                lblCHTotalPage.Text = "of " + pageCount;
            }

            if (pageCount == 1)
            {
                lbtnCHFirst.Visible = false;
                lbtnCHNext.Visible = false;
                lbtnCHPrev.Visible = false;
                lbtnCHLast.Visible = false;
            }
            else if (pageCount == 2)
            {
                lbtnCHFirst.Visible = false;
                lbtnCHNext.Visible = true;
                lbtnCHPrev.Visible = false;
                lbtnCHLast.Visible = false;
            }
            else
            {
                lbtnCHFirst.Visible = false;
                lbtnCHNext.Visible = true;
                lbtnCHPrev.Visible = false;
                lbtnCHLast.Visible = true;
            }
        }
        */

        /*
        protected void lbtnCHFirst_Click(object sender, EventArgs e)
        {
            HandleContactHistoryPageIndexChange(0);
        }

        protected void lbtnCHPrev_Click(object sender, EventArgs e)
        {
            HandleContactHistoryPageIndexChange(gvContactHistory.PageIndex - 1);
        }

        protected void lbtnCHNext_Click(object sender, EventArgs e)
        {
            HandleContactHistoryPageIndexChange(gvContactHistory.PageIndex + 1);
        }

        protected void lbtnCHLast_Click(object sender, EventArgs e)
        {
            HandleContactHistoryPageIndexChange(gvContactHistory.PageCount - 1);
        }

        protected void ddlCHPageNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            HandleContactHistoryPageIndexChange(int.Parse(ddlCHPageNo.SelectedValue));
        }

        private void HandleContactHistoryPageIndexChange(int index)
        {
            int maxIndex = gvContactHistory.PageCount - 1;
            int indexDiff = maxIndex - index;

            lbtnCHFirst.Visible = true;
            lbtnCHNext.Visible = true;
            lbtnCHPrev.Visible = true;
            lbtnCHLast.Visible = true;

            if (index == 0)
            {
                //First Page
                lbtnCHPrev.Visible = false;
                lbtnCHFirst.Visible = false;
                if (gvContactHistory.PageCount < 3)
                    lbtnCHLast.Visible = false;
            }
            else if (index == maxIndex)
            {
                //Last Page
                lbtnCHNext.Visible = false;
                lbtnCHLast.Visible = false;
                if (gvContactHistory.PageCount < 3)
                    lbtnCHFirst.Visible = false;
            }

            gvContactHistory.PageIndex = index;
            DataTable dtContactHistory = (DataTable)ViewState["dtContactHistory"];
            gvContactHistory.DataSource = dtContactHistory;
            gvContactHistory.DataBind();

            ddlCHPageNo.SelectedValue = index + "";
        }
        */

        /*
         <!-- Paging For Contact History -->
                <!--
                <div id="divCHPaging" class="normalGrey" runat="server">
                    <asp:LinkButton ID="lbtnCHFirst" Text="First Page" 
                         runat="server" onclick="lbtnCHFirst_Click" /> &nbsp;
                    <asp:LinkButton ID="lbtnCHPrev" Text="Prev Page" 
                         runat="server" onclick="lbtnCHPrev_Click" /> &nbsp; 
                    <asp:LinkButton ID="lbtnCHNext" Text="Next Page"  runat="server" 
                            onclick="lbtnCHNext_Click" /> &nbsp; 
                    <asp:LinkButton ID="lbtnCHLast" Text="Last Page" runat="server" onclick="lbtnCHLast_Click" 
                          /> &nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Label ID="lblCHPage" Text="Page" runat="server" /> &nbsp;
                    <asp:DropDownList ID="ddlCHPageNo" CssClass="normal" runat="server" 
                          
                         AutoPostBack="True" 
                            onselectedindexchanged="ddlCHPageNo_SelectedIndexChanged" /> &nbsp;
                    <asp:Label ID="lblCHTotalPage" Text="of " runat="server" />
                </div>
                -->
        */

        protected void rdo_CheckedChanged(object sender, EventArgs e)
        {

            txtProjName.Enabled = ((RadioButton)sender).ID.Equals("rdoProj");
            ddlProjName.Enabled = ((RadioButton)sender).ID.Equals("rdoProj");
            pn1.Enabled = ((RadioButton)sender).ID.Equals(" rdoContactDate");
            pn2.Enabled = ((RadioButton)sender).ID.Equals(" rdoContactDate");


            txtProjName.Enabled = ((RadioButton)sender).ID.Equals("");
            ddlProjName.Enabled = ((RadioButton)sender).ID.Equals("rdoProj");
            pn1.Enabled = ((RadioButton)sender).ID.Equals("rdoContactDate");
            pn2.Enabled = ((RadioButton)sender).ID.Equals("rdoContactDate");

            if (((RadioButton)sender).ID.Equals("rdoContactDate"))
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
                    calContactFromDate.DateTextFromValue = DateTime.Now.ToString("dd/MM/yyyy");
                }
            }

        }
        /**************Update by TSM**************/
        protected void txtProjName_TextChanged(object sender, EventArgs e)
        {
            Binding_toProjDDL();
            flag = true;
        }
        protected void Binding_toProjDDL()
        {
            //Clear the Date Dropdownlist 
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
                    //ds = commonService.RetrieveAllProjectByProjectName(ProjName);
                    ds = commonService.RetrieveAllProjectByProjectNameByUserOrSupervisor("UserType NOT LIKE 'FAR%' AND ProjectDetail.ProjectName LIKE '%" + ProjName + "%'", base.userLoginId);
                }
                else if (dsDealerDetail.Tables[0].Rows[0]["Supervisor"].ToString().Trim().ToUpper() == "S")
                {
                    ds = commonService.RetrieveAllProjectByProjectNameByUserOrSupervisor("AEGroup = '" + dsDealerDetail.Tables[0].Rows[0]["TeamCode"].ToString() + "' AND ProjectDetail.ProjectName LIKE '%" + ProjName + "%'", base.userLoginId);
                }
                else
                {
                    ds = commonService.RetrieveAllProjectByProjectNameByUserOrSupervisor("AECode = '" + dsDealerDetail.Tables[0].Rows[0]["AECode"].ToString() + "' AND ProjectDetail.ProjectName LIKE '%" + ProjName + "%'", base.userLoginId);
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
                ddlProjName.Items.Clear();
                CommonUtilities.BindDataToDropDrownList(ddlProjName, ds.Tables[0], "ProjectName", "ProjectID", "--- Select Project Name ---");
                divMessage.InnerHtml = "";
            }
            else
            {
                divMessage.InnerHtml = ds.Tables["ReturnTable"].Rows[0]["ReturnMessage"].ToString();
            }
        }


        //protected void txtProjName_TextChanged(object sender, EventArgs e)
        // {
        //     //Clear the Date Dropdownlist 
        //     ddlProjName.Items.Clear();
        //     string ProjName = txtProjName.Text.ToString();
        //     CommonService commonService = new CommonService(base.dbConnectionStr);


        //     DataSet ds = commonService.RetrieveAllProjectByProjectName(ProjName);

        //     if (ds.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString() == "1")
        //     {
        //         ddlProjName.Items.Clear();
        //         CommonUtilities.BindDataToDropDrownList(ddlProjName, ds.Tables[0], "ProjectName", "ProjectID", "--- Select Project Name ---");
        //         divMessage.InnerHtml = "";
        //     }
        //     else
        //     {
        //         divMessage.InnerHtml = ds.Tables["ReturnTable"].Rows[0]["ReturnMessage"].ToString();
        //     }  
        // }

    }
}
