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
using System.Collections.Generic;

using SPMWebApp.BasePage;
using SPMWebApp.Services;
using SPMWebApp.Utilities;
using System.Text;

namespace SPMWebApp.WebPages.Setup
{
    public partial class DealerManagement : BasePage.BasePage
    {
        private DealerService dealerService;
        SuperAdminService superAdminService;
        //private CommonUtilities comUti;
        //private string[] accessRight = new string[4];

        /// <summary>
        /// Fetch User Access Right
        /// Call to ValidateUserAccessRight method with FunctionCode
        /// </summary>        
        private void LoadUserAccessRight()
        {
            List<AccessRight> accessRightList;
            if (AccessRightUtilities.ValidateUserAccessRight((DataTable)Session["UserAccessRights"], "Management", out accessRightList))
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

        protected void Page_Load(object sender, EventArgs e)
        {
            //comUti = new CommonUtilities();
            //accessRight = comUti.getAccessRight("Management", base.userLoginId, this.dbConnectionStr);

            if (!IsPostBack)
            {
                LoadUserAccessRight();

                PrepareForDealerManagement();
                btnUpdate.Visible = false;
                //btnCancel.Visible = false;
                divPaging.Visible = false;

                InitializeRowPerPageSetting();
                ViewState["RowPerPage"] = 20;

                checkAccessRight();

                chkUserType.Checked = false;
                Panel1.Enabled = false;

                dl_DataBind();
            }

            base.gvList = gvDealers;
            base.divMessage = divMessage;
            base.hdfModifyIndex = hdfModifyIndex;
            base.pgcPagingControl = pgcDealer;
        }

        private void checkAccessRight()
        {
            try
            {
                btnAddDealer.Enabled = (bool)ViewState["CreateAccessRight"];
                btnUpdate.Enabled = (bool)ViewState["ModifyAccessRight"];
            }
            catch (Exception e) { }

            //if (accessRight[0] == "N")
            //{
            //    btnAddDealer.Enabled = false;
            //}
            //else
            //{
            //    btnAddDealer.Enabled = true;               
            //}

            //if (accessRight[2] == "N")
            //{
            //    btnUpdate.Enabled = false;
            //}
            //else
            //{
            //    btnUpdate.Enabled = true;
            //}


        }

        protected override DataTable GetDataTable()
        {
            return ViewState["dtDealers"] as DataTable;
        }

        protected override void SetCurrentRowPerPage(int rowPerPage)
        {
            ViewState["RowPerPage"] = rowPerPage;
        }




        private void InitializeRowPerPageSetting()
        {
            //Setting for RowPerPage for Custom Paging Control
            pgcDealer.StartRowPerPage = 10;
            pgcDealer.RowPerPageIncrement = 10;
            pgcDealer.EndRowPerPage = 100;
        }

        //For Paging Control
        protected override void DisplayPaging()
        {
            if (divPaging.Visible)
            {
                int rowPerPage = (int)ViewState["RowPerPage"];

                pgcDealer.PageCount = gvDealers.PageCount;
                pgcDealer.CurrentRowPerPage = rowPerPage.ToString();
                pgcDealer.DisplayPaging();
            }
        }


        private void PrepareForDealerManagement()
        {
            DataSet ds = null;

            CommonService commonService = new CommonService(base.dbConnectionStr);

            //ds = commonService.RetrieveTeamCodeAndName();
            //ds = commonService.RetrieveAllTeamCodeAndName(base.userLoginId);
            // remove login id filter 
            ds = commonService.RetrieveAllTeamCodeAndName("");

            if (ds.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString() == "1")
            {
                ddlTeamCode.Items.Add(new ListItem("--- Select Team ---", ""));
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    ddlTeamCode.Items.Add(new ListItem(ds.Tables[0].Rows[i]["TeamName"].ToString(), ds.Tables[0].Rows[i]["TeamCode"].ToString()));
                }
            }

            //gvDealers.DataSource = ds.Tables[0];
            //gvDealers.DataBind();
        }

        protected override Hashtable RetrieveRecords()
        {
            dealerService = new DealerService(base.dbConnectionStr);
            DataSet ds = null;
            Hashtable ht = new Hashtable();
            int enable = 1;
            //string crossGroup = "N", supervisior = "N";
            string crossGroup = "", supervisior = "";

            ht.Add("ReturnData", null);
            ht.Add("ReturnCode", "-1");
            ht.Add("ReturnMessage", "");

            if (!chkAssign.Checked)
            {
                enable = 0;
            }

            if (chkCrossGroup.Checked)
            {
                crossGroup = "Y";
            }

            //if (chkSupervisior.Checked)
            //{
            //    supervisior = "Y";
            //}

            if (chkUserType.Checked == true)
            {
                if (rdbtnAdministrator.Checked == true)
                {
                    supervisior = "Y";
                }
                else
                {
                    supervisior = "S";
                }
            }

            //ds = dealerService.RetrieveAllDealer();

            //to uncomment again
            ds = dealerService.RetrieveDealerByCriteria(txtLoginId.Text.Trim(), txtDealerCode.Text.Trim(), txtDealerName.Text.Trim(),
                    ddlTeamCode.SelectedValue, "", enable, crossGroup, supervisior, txtEmailID.Text.Trim(), base.userLoginId);

            if (ds.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString() == "1")
            {
                //Add RecId as primary key.
                ds.Tables[0].PrimaryKey = new DataColumn[] { ds.Tables[0].Columns["RecId"] };

                ht["ReturnData"] = ds.Tables[0];
                ViewState["dtDealers"] = ds.Tables[0];

                ht["ReturnCode"] = ds.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString();

                divPaging.Visible = true;
                //InitializeRowPerPageSetting();
            }
            else
            {
                ViewState["dtDealers"] = null;
                divPaging.Visible = false;
            }

            ht["ReturnMessage"] = ds.Tables["ReturnTable"].Rows[0]["ReturnMessage"].ToString();

            return ht;
        }

        protected override void DisplayDetails(int modifyIndex)
        {
            GridViewRow gvrModify = gvDealers.Rows[modifyIndex];
            HiddenField gvhdfRecId = (HiddenField)gvrModify.FindControl("gvhdfRecId");
            HiddenField gvhdfDisplayIndex = (HiddenField)gvrModify.FindControl("gvhdfDisplayIndex");
            HiddenField gvhdfItemIndex = (HiddenField)gvrModify.FindControl("gvhdfItemIndex");

            HiddenField gvhdfUserId = (HiddenField)gvDealers.Rows[modifyIndex].FindControl("gvhdfUserId");
            HiddenField gvhdfAEGroup = (HiddenField)gvDealers.Rows[modifyIndex].FindControl("gvhdfAEGroup");

            hdfRecId.Value = gvhdfRecId.Value;
            txtInpLoginID.Text = gvrModify.Cells[2].Text.Trim();
            txtInpName.Text = gvrModify.Cells[3].Text.Trim();

            if ((gvrModify.Cells[4].Text.Trim().Equals("&nbsp;")))
            {
                txtInpEmail.Text = "";
            }
            else
            {
                txtInpEmail.Text = gvrModify.Cells[4].Text.Trim();
            }

            chkInpAssign.Checked = true;
            chkInpCrossGroup.Checked = false;
            chkInpUserType.Checked = false;
            if (gvrModify.Cells[7].Text.Trim().Equals("N"))
            {
                chkInpAssign.Checked = false;
            }

            if (gvrModify.Cells[8].Text.Trim().Equals("Y"))
            {
                chkInpCrossGroup.Checked = true;
            }

            if (gvrModify.Cells[9].Text.Trim().Equals("Y"))
            {
                /// <Updated by OC>
                //chkSupervisior.Checked = true;
                chkInpUserType.Checked = true;
                Panel2.Enabled = true;
                rdbtnInpAdministrator.Checked = true;
                rdbtnInpSupervisor.Checked = false;
            }
            else if (gvrModify.Cells[9].Text.Trim().Equals("S"))
            {
                chkInpUserType.Checked = true;
                Panel2.Enabled = true;
                rdbtnInpSupervisor.Checked = true;
                rdbtnInpAdministrator.Checked = false;
            }
            else
            {
                chkInpUserType.Checked = false;
                Panel2.Enabled = false;
            }

            this.hdfModifyIndex.Value = gvhdfItemIndex.Value;
            this.hdfDisplayIndex.Value = gvhdfDisplayIndex.Value;

            populateAEGroup();

            SetUI("edit");
        }

        protected override Hashtable DeleteRecord(int deleteIndex)
        {
            long deleteRecId = long.Parse(gvDealers.DataKeys[deleteIndex].Value.ToString());
            HiddenField gvhdfItemIndex = (HiddenField)gvDealers.Rows[deleteIndex].FindControl("gvhdfItemIndex");
            HiddenField gvhdfUserId = (HiddenField)gvDealers.Rows[deleteIndex].FindControl("gvhdfUserId");
            HiddenField gvhdfAEGroup = (HiddenField)gvDealers.Rows[deleteIndex].FindControl("gvhdfAEGroup");


            dealerService = new DealerService(base.dbConnectionStr);
            Hashtable ht = new Hashtable();
            DataTable dtDealers = null;
            string[] wsReturn = null;

            ht.Add("ReturnData", null);

            wsReturn = dealerService.DeleteDealer(deleteRecId);

            if (wsReturn[0] == "1")
            {
                //Delete SuperAdmin's records by User Id
                deleteSuperAdmin(gvhdfUserId.Value, gvhdfAEGroup.Value);

                dtDealers = ViewState["dtDealers"] as DataTable;
                dtDealers.Rows.RemoveAt(int.Parse(gvhdfItemIndex.Value));
                ViewState["dtDealers"] = dtDealers;
                ht["ReturnData"] = dtDealers;
            }

            ht.Add("ReturnCode", wsReturn[0]);
            ht.Add("ReturnMessage", wsReturn[1]);

            this.ClearDealerForm();
            btnAddDealer.Visible = true;
            btnSearch.Visible = true;

            btnUpdate.Visible = false;
            //btnCancel.Visible = false;

            return ht;
        }

        private Boolean deleteSuperAdmin(string userid, string aegroup)
        {
            SuperAdminService superAdminService;
            DataSet ds;

            superAdminService = new SuperAdminService(base.dbConnectionStr);
            ds = superAdminService.SuperAdmin_Delete(userid, aegroup);

            if (ds.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString().Equals("1"))
            {
                return true;
            }

            return false;
        }

        private void dl_DataBind()
        {
            DataSet ds = new DataSet();

            superAdminService = new SuperAdminService(base.dbConnectionStr);

            ds = superAdminService.SuperAdmin_Getddl("", "", "", "");

            if (ds.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString().Equals("1"))
            {
                dlAEGroup.DataSource = ds.Tables["dtAEGroup"];
                dlAEGroup.DataBind();
            }
        }

        protected void dlAEGroup_PreRender(object sender, EventArgs e)
        {
            ClientScriptManager cs = Page.ClientScript;

            foreach (DataListItem item in dlAEGroup.Items)
            {
                CheckBox chkAEGroup = item.FindControl("chkAEGroup") as CheckBox;
                cs.RegisterArrayDeclaration("chkAEGroup", String.Concat("'", chkAEGroup.ClientID, "'"));
            }
        }

        protected void btnSearchAEGroup_Click(object sender, EventArgs e)
        {

            // Save Selected items
            SaveCheckedValues();

            DataSet ds = new DataSet();

            superAdminService = new SuperAdminService(base.dbConnectionStr);

            String rdbtnAEGroupOption = String.Empty;

            if (rdbtnAEGroupStartWith.Checked)
            {
                rdbtnAEGroupOption = "S";
            }
            else if (rdbtnAEGroupEndWith.Checked)
            {
                rdbtnAEGroupOption = "E";
            }
            else
            {
                rdbtnAEGroupOption = "C";
            }

            ds = superAdminService.SuperAdmin_Getddl("", txtSearchAEGroup.Text.Trim(), "", rdbtnAEGroupOption);

            if (ds.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString().Equals("1"))
            {
                dlAEGroup.DataSource = ds.Tables["dtAEGroup"];
                dlAEGroup.DataBind();
            }
            else
            {

            }

            if (ds.Tables["dtAEGroup"].Rows.Count == 0)
            {
                labAEGroupSelectAll.Enabled = false;
                chkAEGroupSelectAll.Enabled = false;
            }
            else
            {
                labAEGroupSelectAll.Enabled = true;
                chkAEGroupSelectAll.Enabled = true;
            }
        }



        protected void btnCreate_Click(object sender, EventArgs e)
        {
            SetUI("create");
            this.ClearDealerForm();
            ClearPrevSelectedAEGroup();
        }

        private void SetUI(string type)
        {
            if (type == "create")
            {
                divSearch.Visible = false;
                divInput.Visible = true;
                divInpMessage.InnerHtml = "";
                txtInpLoginID.Enabled = true;
                btnUpdate.Visible = false;
                btnAddDealer.Visible = true;
                //btnDeleteByUser.Visible = false;
            }
            else if (type == "edit")
            {
                divSearch.Visible = false;
                divInput.Visible = true;
                divInpMessage.InnerHtml = "";
                txtInpLoginID.Enabled = false;
                btnAddDealer.Visible = false;
                btnUpdate.Visible = true;
                //btnDeleteByUser.Visible = true;
            }
            else
            {
                divSearch.Visible = true;
                divInput.Visible = false;
                btnSearch.Visible = true;
            }
        }

        protected void btnAddDealer_Click(object sender, EventArgs e)
        {
            string[] wsReturn = null;
            dealerService = new DealerService(base.dbConnectionStr);
            wsReturn = dealerService.CheckDealerExists(txtInpLoginID.Text.Trim());

            if (wsReturn[0] == "1")
            {
                if (SaveDealer("Ins"))
                {
                    this.ClearDealerForm();
                    ClearPrevSelectedAEGroup();
                }
            }
            else
            {
                divInpMessage.InnerHtml = wsReturn[1];
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            SaveDealer("Upd");
            resetSelectedAEGroup();
        }

        private bool SaveDealer(string type)
        {
            Boolean success = false;
            string _chkAEGroup;
            string _txtAECode;
            string _txtAltAECode;
            DataSet ds;
            StringBuilder sb = new StringBuilder();

            dealerService = new DealerService(base.dbConnectionStr);
            superAdminService = new SuperAdminService(base.dbConnectionStr);
            string[] wsReturn = null;
            int enable = 1;
            string crossGroup = "N", supervisior = "N", assign = "Y", validateResult = "";

            if (!chkInpAssign.Checked)
            {
                enable = 0;
                assign = "N";
            }

            if (chkInpCrossGroup.Checked)
            {
                crossGroup = "Y";
            }

            supervisior = CheckSupervisor();

            validateResult = ValidateDealerForm();

            if (String.IsNullOrEmpty(validateResult))
            {

                // Clear Existing Records from DealerDetail and SuperAdmin
                if (ClearExistingDealerInfo(txtInpLoginID.Text.Trim()))
                {


                    // Save Selected AE Groups
                    SaveCheckedValues();

                    List<KeyValuePair<string, string>> listOfAEGroups = SelectedAEGroup;
                    foreach (KeyValuePair<string, string> pair in listOfAEGroups)
                    {
                        _chkAEGroup = pair.Key;
                        _txtAECode = pair.Value.Substring(0, pair.Value.IndexOf(","));
                        _txtAltAECode = pair.Value.Substring(pair.Value.IndexOf(",") + 1);

                        if (_chkAEGroup != "")
                        {
                            // Save @ Dealer Detail
                            wsReturn = dealerService.InsertDealer(txtInpLoginID.Text.Trim(), _txtAECode, txtInpName.Text.Trim(), _chkAEGroup,
                                "", enable, crossGroup, supervisior, base.userLoginId, txtInpEmail.Text.Trim(), _txtAltAECode);

                            //divMessage.InnerHtml = (type == "Ins" ? wsReturn[1] : wsReturn[1].Replace("inserted", "updated"));
                            divInpMessage.InnerHtml = (type == "Ins" ? wsReturn[1] : wsReturn[1].Replace("inserted", "updated"));

                            // Save @ SuperAdmin 
                            String UserID = txtInpLoginID.Text.Trim();

                            ds = superAdminService.SuperAdmin_Insert(UserID, _chkAEGroup);

                            if (ds.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString() != "1")
                            {
                                sb.Append(ds.Tables["ReturnTable"].Rows[0]["ReturnMessage"].ToString() + Environment.NewLine);
                            }
                        }
                    }
                    success = true;
                }               
            }
            else
            {
                divInpMessage.InnerHtml = validateResult;
            }
            return success;
        }

        protected override Hashtable UpdateRecord(int modifyIndex)
        {
            dealerService = new DealerService(base.dbConnectionStr);
            string[] wsReturn = null;
            int enable = 1, displayIndex = 0;
            string crossGroup = "N", supervisior = "N", assign = "Y", validateResult = "";
            Hashtable ht = new Hashtable();
            DataTable dtDealers = null;
            DataRow drUpdate = null;

            ht.Add("ReturnData", null);


            dtDealers = ViewState["dtDealers"] as DataTable;
            drUpdate = dtDealers.Rows.Find(hdfRecId.Value);

            if (!chkAssign.Checked)
            {
                enable = 0;
                assign = "N";
            }

            if (chkCrossGroup.Checked)
            {
                crossGroup = "Y";
            }

            //if (chkSupervisior.Checked)
            //{
            //    supervisior = "Y";
            //}

            supervisior = CheckSupervisor();

            validateResult = ValidateDealerForm();

            if (String.IsNullOrEmpty(validateResult))
            {
                //to uncomment again
                wsReturn = dealerService.UpdateDealer(hdfRecId.Value, txtLoginId.Text.Trim(), txtDealerCode.Text.Trim(), txtDealerName.Text.Trim(),
                           ddlTeamCode.SelectedValue, "", enable, crossGroup, supervisior, base.userLoginId,
                           drUpdate["OriginalAECode"].ToString(), drUpdate["OriginalUserID"].ToString(), txtEmailID.Text.Trim(), txtAltAECode.Text.Trim());

                if (wsReturn[0] == "1")
                {
                    btnAddDealer.Visible = true;
                    btnSearch.Visible = true;

                    btnUpdate.Visible = false;
                    //btnCancel.Visible = false;

                    //Update in DataTable

                    /*
                    //Update Datatable with Item Index, it will not correct if GridView is sortable.
                    dtDealers.Rows[modifyIndex].BeginEdit();
                    dtDealers.Rows[modifyIndex]["UserID"] = txtLoginId.Text.Trim();
                    dtDealers.Rows[modifyIndex]["AECode"] = txtDealerCode.Text.Trim();
                    dtDealers.Rows[modifyIndex]["AEName"] = txtDealerName.Text.Trim();
                    dtDealers.Rows[modifyIndex]["AEGroup"] = ddlTeamCode.SelectedValue;
                    //dtDealers.Rows[modifyIndex]["ATSLogin"] = txtAtsLogin.Text.Trim();
                    dtDealers.Rows[modifyIndex]["Enable"] = assign;
                    dtDealers.Rows[modifyIndex]["CrossGroup"] = crossGroup;
                    dtDealers.Rows[modifyIndex]["Supervisor"] = supervisior;
                    dtDealers.Rows[modifyIndex]["ModifiedDate"] = DateTime.Now;
                    dtDealers.Rows[modifyIndex].EndEdit();
                    dtDealers.AcceptChanges();
                    */

                    //Update Datatable with RecId which is unique and primary key

                    if (drUpdate != null)
                    {
                        drUpdate["UserID"] = txtLoginId.Text.Trim();
                        drUpdate["AECode"] = txtDealerCode.Text.Trim();
                        drUpdate["AEName"] = txtDealerName.Text.Trim();
                        drUpdate["AEGroup"] = ddlTeamCode.SelectedValue;
                        drUpdate["Enable"] = assign;
                        drUpdate["CrossGroup"] = crossGroup;
                        drUpdate["Supervisor"] = supervisior;
                        drUpdate["ModifiedDate"] = DateTime.Now;

                        drUpdate["OriginalAECode"] = txtDealerCode.Text.Trim();
                        drUpdate["OriginalUserID"] = txtLoginId.Text.Trim();
                        drUpdate["Email"] = txtEmailID.Text.Trim();
                        drUpdate["AltAECode"] = txtAltAECode.Text.Trim();
                    }

                    ViewState["dtDealers"] = dtDealers;
                    ht["ReturnData"] = dtDealers;

                    gvDealers.DataSource = dtDealers;
                    gvDealers.DataBind();

                    this.ClearDealerForm();
                }

                validateResult = wsReturn[1];
                ht.Add("ReturnCode", wsReturn[0]);
            }
            else
            {
                ht.Add("ReturnCode", "-1");
            }


            ht.Add("ReturnMessage", validateResult);
            return ht;
        }

        protected void gvDealers_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                System.Drawing.Color color = System.Drawing.ColorTranslator.FromHtml("#CCFFCC");

                if ((DataBinder.Eval(e.Row.DataItem, "Supervisor") != null) &&
                        ((DataBinder.Eval(e.Row.DataItem, "Supervisor").ToString() == "Y")))
                {
                    e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFCC99");
                    e.Row.Cells[7].BackColor = color;
                }

                this.FillColor(e.Row, 5, DataBinder.Eval(e.Row.DataItem, "Enable"), "Y", color);
                this.FillColor(e.Row, 6, DataBinder.Eval(e.Row.DataItem, "CrossGroup"), "Y", color);


                //Access Right

                System.Web.UI.WebControls.Button btnModify = (System.Web.UI.WebControls.Button)e.Row.Cells[10].FindControl("gvbtnModify");
                System.Web.UI.WebControls.Button btnDelete = (System.Web.UI.WebControls.Button)e.Row.Cells[10].FindControl("Button2");

                if (btnModify != null)
                {
                    ((System.Web.UI.WebControls.Button)e.Row.Cells[10].FindControl("gvbtnModify")).Enabled = (bool)ViewState["ModifyAccessRight"];
                    //if (accessRight[2] == "N")
                    //{
                    //    ((System.Web.UI.WebControls.Button)e.Row.Cells[10].FindControl("gvbtnModify")).Enabled = false;
                    //}
                    //else
                    //{
                    //    ((System.Web.UI.WebControls.Button)e.Row.Cells[10].FindControl("gvbtnModify")).Enabled = true;
                    //}
                }
                if (btnDelete != null)
                {
                    ((System.Web.UI.WebControls.Button)e.Row.Cells[10].FindControl("Button2")).Enabled = (bool)ViewState["DeleteAccessRight"];
                    //if (accessRight[3] == "N")
                    //{
                    //    ((System.Web.UI.WebControls.Button)e.Row.Cells[10].FindControl("Button2")).Enabled = false;
                    //}
                    //else
                    //{
                    //    ((System.Web.UI.WebControls.Button)e.Row.Cells[10].FindControl("Button2")).Enabled = true;
                    //}
                }
            }
        }

        protected void gvDealers_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataTable dtDealers = ViewState["dtDealers"] as DataTable;
            DataTable sortedDataTable = null;

            string sortDirection = "", sortString = "";

            sortDirection = CommonUtilities.GetSortDirection(e.SortExpression, ViewState["SortExpression"] as string, ViewState["SortDirection"] as string);

            ViewState["SortDirection"] = sortDirection;
            ViewState["SortExpression"] = e.SortExpression;

            sortString = e.SortExpression + " " + sortDirection;
            sortedDataTable = CommonUtilities.SortDataTable(dtDealers, sortString);

            ViewState["dtDealers"] = sortedDataTable;

            gvDealers.PageIndex = 0;
            gvDealers.DataSource = sortedDataTable;
            gvDealers.DataBind();

            DisplayPaging();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            SetUI("search");
            divMessage.InnerHtml = "";
            ClearDealerForm();
            SearchAndDisplay();
        }


        //Helper Methods
        private void FillColor(GridViewRow gvr, int cellIndex, object bindValue, string checkValue, System.Drawing.Color color)
        {
            if ((bindValue != null) && (bindValue.ToString().Equals(checkValue)))
            {
                gvr.Cells[cellIndex].BackColor = color;
            }
        }

        private string ValidateDealerForm()
        {
            string validateResult = "";
            CommonUtilities common = new CommonUtilities();

            if (String.IsNullOrEmpty(txtInpLoginID.Text.Trim()))
            {
                validateResult = "Login Id cannot be blank!";
            }
            else if (String.IsNullOrEmpty(txtInpName.Text.Trim()))
            {
                validateResult = "Name cannot be blank!";
            }
            else if (String.IsNullOrEmpty(txtInpEmail.Text.Trim()))
            {
                validateResult = "Email cannot be blank!";
            }

            foreach (DataListItem itemAEGroup in dlAEGroup.Items)
            {
                CheckBox chkAEGroup = itemAEGroup.FindControl("chkAEGroup") as CheckBox;
                if (chkAEGroup.Checked)
                {
                    TextBox txtAECode = itemAEGroup.FindControl("txtAECode") as TextBox;
                    if (txtAECode.Text.Trim() == "")
                    {
                        txtAECode.Focus();
                        validateResult = "Dealer Code cannot be blank!";
                        return validateResult;
                    }
                }
            }

            return validateResult;
        }

        private void ClearDealerForm()
        {
            txtInpLoginID.Text = txtInpName.Text = txtInpEmail.Text = "";

            /// <Updated by OC>
            //chkAssign.Checked = chkCrossGroup.Checked = chkSupervisior.Checked = false;
            chkInpAssign.Checked = chkInpCrossGroup.Checked = chkInpUserType.Checked = Panel2.Enabled = false;
            //lbDealerCodeMsg.Text = "";
            lstDealerCode.Items.Clear();
        }

        private String CheckSupervisor()
        {
            if (chkInpUserType.Checked == true)
            {
                if (rdbtnInpAdministrator.Checked == true)
                {
                    return "Y";
                }
                else if (rdbtnInpSupervisor.Checked == true)
                {
                    return "S";
                }
                else
                {
                    return "N";
                }
            }
            else
            {
                return "N";
            }
        }

        protected void chkUserType_CheckedChanged(object sender, EventArgs e)
        {
            if (chkUserType.Checked == true)
            {
                Panel1.Enabled = true;
                rdbtnAdministrator.Checked = true;
            }
            else
            {
                Panel1.Enabled = false;
            }
        }

        protected void chkInpUserType_CheckedChanged(object sender, EventArgs e)
        {
            if (chkInpUserType.Checked == true)
            {
                Panel2.Enabled = true;
                rdbtnInpAdministrator.Checked = true;
            }
            else
            {
                Panel2.Enabled = false;
            }
        }

        public string GetJSString(string userId, string name, string type)
        {
            string txt = string.Empty;
            if (type.ToUpper().Trim() == "VIEW")
            {
                txt = "<a style=\"font-size:12px;\" href=\"javascript: void(0)\" onclick=\"window.open('../AccessControl/SuperAdmin.aspx?uid=" + userId + "&nm=" + name + "&tb=0', '', 'width=800, height=780, scrollbars=yes'); return false;\">";
                txt += "<img src='../../images/fm.png'>";
            }
            else if (type.ToUpper().Trim() == "SETUP")
            {
                txt = "<a style=\"font-size:12px;\" href=\"javascript: void(0)\" onclick=\"window.open('../AccessControl/SuperAdmin.aspx?uid=" + userId + "&nm=" + name + "&tb=1', '', 'width=800, height=780, scrollbars=yes'); return false;\">";
                txt += "<img src='../../images/Add.png'>";
            }
            return txt + "</a>";
        }

        protected void gvDealers_RowCommand(Object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "modify")
            {
                SetUI("create");
                this.ClearDealerForm();
                ClearPrevSelectedAEGroup();
            }
        }


        protected void dlAEGroup_ItemCommand(Object source, DataListCommandEventArgs e)
        {
            //lbDealerCodeMsg.Text = "";
            lstDealerCode.Items.Clear();
            if (e.CommandName == "indicate")
            {
                try
                {
                    string aecode = e.CommandArgument.ToString();
                    ddlTeamCode.SelectedValue = aecode;

                    DataTable dtReturnData = null;
                    Hashtable ht = RetrieveRecords();

                    divMessage.InnerHtml = ht["ReturnMessage"].ToString();

                    if (ht["ReturnData"] != null)
                    {
                        dtReturnData = (DataTable)ht["ReturnData"];
                    }
                    lstDealerCode.DataTextField = "AECode";
                    lstDealerCode.DataValueField = "AECode";
                    lstDealerCode.DataSource = dtReturnData;
                    lstDealerCode.DataBind();

                    if (ht["ReturnCode"] == "-1")
                    {
                        lstDealerCode.Items.Add(new ListItem("There is no ref. dealer code."));
                    }

                    

                    //gvDealerCode.PageIndex = 0;
                    //gvDealerCode.DataSource = dtReturnData;
                    //gvDealerCode.DataBind();

                    //divMessage.InnerHtml = divMessage.InnerHtml + "<br /> Total Page : " + gvList.PageCount + ", Current Page : " + gvList.PageIndex;
                    //DisplayPaging();
                }
                catch (Exception)
                {
                    //lbDealerCodeMsg.Text = "";
                    lstDealerCode.Items.Add(new ListItem("There is no ref. dealer code."));
                    //lstDealerCode.DataSource = "";
                    //lstDealerCode.DataBind();
                }
            }
        }

        protected void dlAEGroup_ItemDataBound(Object source, DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item ||
             e.Item.ItemType == ListItemType.AlternatingItem)
            {
                CheckBox chkAEGroup = (CheckBox)e.Item.FindControl("chkAEGroup");
                if (chkAEGroup != null)
                {
                    string value = GetPrevSelectedVal(chkAEGroup.Text);
                    if (value != "")
                    {
                        chkAEGroup.Checked = true;
                        TextBox txtAECode = (TextBox)e.Item.FindControl("txtAECode");
                        TextBox txtAltAECode = (TextBox)e.Item.FindControl("txtAltAECode");
                        txtAECode.Text = value.Substring(0, value.IndexOf(","));
                        txtAltAECode.Text = value.Substring(value.IndexOf(",") + 1);
                    }
                }
            }
        }


        protected bool ClearExistingDealerInfo(string userid)
        {
            Boolean result = true;
            dealerService = new DealerService(base.dbConnectionStr);
            string[] wsReturn = null;

            wsReturn = dealerService.DeleteDealerByUserId(txtInpLoginID.Text.Trim());

            if (!deleteSuperAdmin(txtInpLoginID.Text.Trim(), string.Empty))
            {
                result = false;
            }

            return result;
        }

        protected List<KeyValuePair<string, string>> SelectedAEGroup
        {
            get
            {
                return (List<KeyValuePair<string, string>>)ViewState["SelectedAEGroup"];
            }
            set
            {
                ViewState["SelectedAEGroup"] = value;
            }
        }

        private void SaveCheckedValues()
        {
            List<KeyValuePair<string, string>> listOfAEGroups = new List<KeyValuePair<string, string>>();
            if (SelectedAEGroup != null)
                listOfAEGroups = SelectedAEGroup;

            resetSelectedAEGroup();

            foreach (DataListItem itemAEGroup in dlAEGroup.Items)
            {
                CheckBox chkAEGroup = itemAEGroup.FindControl("chkAEGroup") as CheckBox;
                RemoveIfExists(chkAEGroup.Text, ref listOfAEGroups);
                if (chkAEGroup.Checked)
                {
                    string aegroup = chkAEGroup.Text;
                    string aecode = (itemAEGroup.FindControl("txtAECode") as TextBox).Text;
                    string altaecode = (itemAEGroup.FindControl("txtAltAECode") as TextBox).Text;

                    KeyValuePair<string, string> KVAEGroup = new KeyValuePair<string, string>(aegroup.ToString(), aecode + "," + altaecode);
                    listOfAEGroups.Add(KVAEGroup);
                }
            }
            SelectedAEGroup = listOfAEGroups;
        }

        private void RemoveIfExists(string aegroup, ref List<KeyValuePair<string, string>> listOfAEGroups)
        {
            foreach (KeyValuePair<string, string> pair in listOfAEGroups)
            {
                if (pair.Key == aegroup)
                {
                    listOfAEGroups.Remove(pair);
                    return;
                }
            }
        }

        private string GetPrevSelectedVal(string aegroup)
        {
            string val = string.Empty;
            if (SelectedAEGroup != null)
            {
                List<KeyValuePair<string, string>> listOfAEGroups = SelectedAEGroup;
                foreach (KeyValuePair<string, string> pair in listOfAEGroups)
                {
                    if (pair.Key == aegroup)
                        val = pair.Value;
                }
            }
            return val;
        }

        private void resetSelectedAEGroup()
        {
            List<KeyValuePair<string, string>> listOfAEGroups = new List<KeyValuePair<string, string>>();
            SelectedAEGroup = listOfAEGroups;
        }

        protected void btnDeleteByUser_Click(object sender, EventArgs e)
        {
            dealerService = new DealerService(base.dbConnectionStr);
            string[] wsReturn = null;
            wsReturn = dealerService.DeleteDealerByUserId(txtInpLoginID.Text.Trim());

            if (wsReturn[0] != "-1")
            {
                //Delete SuperAdmin's records by User Id
                deleteSuperAdmin(txtInpLoginID.Text.Trim(), string.Empty);

                this.ClearDealerForm();
                ClearPrevSelectedAEGroup();

                SetUI("search");
                ViewState["dtDealers"] = null;
            }
            SearchAndDisplay();
            divMessage.InnerHtml = wsReturn[1];
            divInpMessage.InnerHtml = wsReturn[1];

        }

        protected void populateAEGroup()
        {
            dealerService = new DealerService(base.dbConnectionStr);
            DataSet ds = null;
            int enable = 1;
            string crossGroup = "", supervisior = "";

            if (!chkInpAssign.Checked)
            {
                enable = 0;
            }

            if (chkInpCrossGroup.Checked)
            {
                crossGroup = "Y";
            }

            if (chkInpUserType.Checked == true)
            {
                if (rdbtnInpAdministrator.Checked == true)
                {
                    supervisior = "Y";
                }
                else
                {
                    supervisior = "S";
                }
            }

            ds = dealerService.RetrieveDealerByCriteria(txtInpLoginID.Text.Trim(), "", "",
                    "", "", enable, crossGroup, supervisior, "", base.userLoginId);

            if (ds.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString() == "1")
            {
                List<KeyValuePair<string, string>> listOfAEGroups = new List<KeyValuePair<string, string>>();
                if (SelectedAEGroup != null)
                    listOfAEGroups = SelectedAEGroup;

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string aegroup = dr["AEGroup"].ToString();
                    string aecode = dr["AECode"].ToString();
                    string altaecode = dr["AltAECode"].ToString();

                    foreach (DataListItem itemAEGroup in dlAEGroup.Items)
                    {
                        CheckBox chkAEGroup = itemAEGroup.FindControl("chkAEGroup") as CheckBox;
                        if (chkAEGroup.Text == aegroup)
                        {
                            chkAEGroup.Checked = true;
                            (itemAEGroup.FindControl("txtAECode") as TextBox).Text = aecode;
                            (itemAEGroup.FindControl("txtAltAECode") as TextBox).Text = altaecode;

                            KeyValuePair<string, string> KVAEGroup = new KeyValuePair<string, string>(aegroup.ToString(), aecode + "," + altaecode);
                            listOfAEGroups.Add(KVAEGroup);
                            SelectedAEGroup = listOfAEGroups;
                        }
                    }
                }
            }
        }

        private void ClearPrevSelectedAEGroup()
        {
            txtSearchAEGroup.Text = "";
            rdbtnAEGroupStartWith.Checked = false;
            rdbtnAEGroupEndWith.Checked = false;
            rdbtnAEGroupContain.Checked = false;
            chkAEGroupSelectAll.Checked = false;

            foreach (DataListItem itemAEGroup in dlAEGroup.Items)
            {
                CheckBox chkAEGroup = itemAEGroup.FindControl("chkAEGroup") as CheckBox;
                TextBox txtAECode = itemAEGroup.FindControl("txtAECode") as TextBox;
                TextBox txtAltAECode = itemAEGroup.FindControl("txtAltAECode") as TextBox;
                if (chkAEGroup != null)
                {
                    chkAEGroup.Checked = false;
                    txtAECode.Text = "";
                    txtAltAECode.Text = "";
                    resetSelectedAEGroup();
                }
            }
            dl_DataBind();
        }


    }


}
