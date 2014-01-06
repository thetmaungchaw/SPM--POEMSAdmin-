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



namespace SPMWebApp.WebPages.LeadManagement
{
    public partial class LeadContactEntry : BasePage.BasePage
    {
        //private CommonUtilities comUti;
        //private string[] accessRight = new string[4];
        //private leadsContactService leadsContactService;
        private LeadsContactService leadsContactService;
        private ClientAssignmentService clientAssignmentService;
        private LeadsService leadsService;
        private string contactType = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            //comUti = new CommonUtilities();
            //accessRight = comUti.getAccessRight("LeadsMod", base.userLoginId, this.dbConnectionStr);
            if (hdfStatus.Value == "F")
            {
                //txtContent.Text = "It is followup";
            }



            if (!IsPostBack)
            {
                //Add JavaScript Event for Short Key Textbox blur event
                //this.txtKey.Attributes.Add("onblur", "javascript:handleShortKeyBlur()");

                //divPopUp.Visible = false;

                LoadUserAccessRight();

                panelSync.Visible = false;

                //No Need to check Admin or User in Leads Contact Entry
                divAdminEntry.Visible = false;
                ViewState["UserRole"] = "user";

                //For Contact Entry Admin
                //if (String.IsNullOrEmpty(Request.Params["type"]))
                //{
                //    divAdminEntry.Visible = false;
                //    ViewState["UserRole"] = "user";

                //}
                //else
                //{
                //    divTitle.InnerHtml = "Contact Entry Admin";
                //    divAdminEntry.Visible = true;
                //    ViewState["UserRole"] = "admin";
                //}


                //Fill Rank DropdownList
                // CommonService commonService = new CommonService();
                //// string[,] clientRanks = commonService.RetrieveClientRanks();
                //// CommonUtilities.BindDataToDropDrownList(ddlRank, clientRanks, 0, 1, null);

                // DataSet ds = commonService.RetrieveDealerCodeAndNameByUserID(base.userLoginId);

                // if (ds.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString() == "1")
                // {
                //     for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                //     {
                //         ddlActualDealer.Items.Add(new ListItem(ds.Tables[0].Rows[i]["TeamName"].ToString(), ds.Tables[0].Rows[i]["AECode"].ToString()));
                //     }
                // }


                divCHPaging.Visible = false;
                divAssignPaging.Visible = false;
                ViewState["RowPerPage"] = 10;                   //For Assignment GridView
                ViewState["ContactHistoryRowPerPage"] = 10;     //For Contact History GridView
                InitializeRowPerPageSetting();
                InitializeContactHistoryRowPerPageSetting();

                contactType = "Entry";
                ViewState["ContactType"] = "Entry";
                tdEntryHistory.BgColor = "#6E6A6B";
                PrepareForContactEntry();

                //Update in SPM III
                //This is button are set to true in DisplayDetails Method.
                btnUpdateContact.Visible = false;
                btnCancel.Visible = false;
                btnAddContact.Visible = true;
                btnSync.Visible = true;
            }

            base.hdfModifyIndex = this.hdfModifyIndex;
            base.gvList = gvContactHistory;
            base.divMessage = this.divMessage;
            divMessageTwo.InnerText = "";
            divMessage.InnerHtml = "";

            checkAccessRight();

            ////This is button are set to true in DisplayDetails Method.
            //btnUpdateContact.Visible = false;
            //btnCancel.Visible = false;
            //btnAddContact.Visible = true;
        }

        /// <summary>
        /// Fetch User Access Right
        /// Call to ValidateUserAccessRight method with FunctionCode
        /// </summary>        
        private void LoadUserAccessRight()
        {
            List<AccessRight> accessRightList;
            if (AccessRightUtilities.ValidateUserAccessRight((DataTable)Session["UserAccessRights"], "LeadsMod", out accessRightList))
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

        private void checkAccessRight()
        {
            try
            {
                btnAddContact.Enabled = (bool)ViewState["CreateAccessRight"];
                btnSync.Enabled = (bool)ViewState["CreateAccessRight"];
            }
            catch (Exception e) { }
        }

        private void InitializeRowPerPageSetting()
        {
            //Setting for RowPerPage for Custom Paging Control
            pgcAssignment.StartRowPerPage = 10;
            pgcAssignment.RowPerPageIncrement = 10;
            pgcAssignment.EndRowPerPage = 100;

            pgcFollowUp.StartRowPerPage = 5;
            pgcFollowUp.RowPerPageIncrement = 5;
            pgcFollowUp.EndRowPerPage = 100;
        }

        private void DisplayAssignmentsPaging()
        {
            if (divAssignPaging.Visible)
            {
                int rowPerPage = (int)ViewState["RowPerPage"];

                pgcAssignment.PageCount = gvAssignments.PageCount;
                pgcAssignment.CurrentRowPerPage = rowPerPage.ToString();
                pgcAssignment.DisplayPaging();
            }
        }

        protected void AssignmentPaging_RowPerPageChanged(object sender, EventArgs<Int32> e)
        {
            DataTable dtAssignment = ViewState["dtAssignment"] as DataTable;
            if (dtAssignment != null)
            {
                gvAssignments.PageSize = e.Value;
                gvAssignments.PageIndex = 0;
                gvAssignments.DataSource = dtAssignment;
                gvAssignments.DataBind();

                //Need to rebind Contact History
                /*
                DataTable dtContactHistory = ViewState["dtContactHistory"] as DataTable;
                if (dtContactHistory != null)
                {
                    gvContactHistory.DataSource = dtContactHistory;
                    gvContactHistory.DataBind();
                }
                */


                ViewState["RowPerPage"] = e.Value;
                DisplayAssignmentsPaging();
            }
        }

        protected void AssignmentPaging_PageNoChange(object sender, EventArgs<Int32> e)
        {
            gvAssignments.PageIndex = e.Value - 1;

            DataTable dtAssignment = ViewState["dtAssignment"] as DataTable;
            if (dtAssignment != null)
            {
                gvAssignments.DataSource = dtAssignment;
                gvAssignments.DataBind();
            }
        }

        // ***** FollowUp Paging
        private void DisplayFollowUpPaging()
        {
            if (divFollowUpPaging.Visible)
            {
                int rowPerPage = (int)ViewState["RowPerPage"];

                pgcFollowUp.PageCount = gvFollowUp.PageCount;
                pgcFollowUp.CurrentRowPerPage = rowPerPage.ToString();
                pgcFollowUp.DisplayPaging();
            }
        }

        protected void FollowUpPaging_RowPerPageChanged(object sender, EventArgs<Int32> e)
        {
            DataTable dtFollowUp = ViewState["dtFollowUp"] as DataTable;
            if (dtFollowUp != null)
            {
                gvFollowUp.PageSize = e.Value;
                gvFollowUp.PageIndex = 0;
                gvFollowUp.DataSource = dtFollowUp;
                gvFollowUp.DataBind();

                //Need to rebind Contact History
                /*
                DataTable dtContactHistory = ViewState["dtContactHistory"] as DataTable;
                if (dtContactHistory != null)
                {
                    gvContactHistory.DataSource = dtContactHistory;
                    gvContactHistory.DataBind();
                }
                */


                ViewState["RowPerPage"] = e.Value;
                DisplayFollowUpPaging();
            }
        }

        protected void FollowUpPaging_PageNoChange(object sender, EventArgs<Int32> e)
        {
            gvFollowUp.PageIndex = e.Value - 1;

            DataTable dtFollowUp = ViewState["dtFollowUp"] as DataTable;
            if (dtFollowUp != null)
            {
                gvFollowUp.DataSource = dtFollowUp;
                gvFollowUp.DataBind();
            }
        }

        // ***** Contact History Paging        
        private void InitializeContactHistoryRowPerPageSetting()
        {
            //Setting for RowPerPage for Custom Paging Control
            pgcContactHistory.StartRowPerPage = 10;
            pgcContactHistory.RowPerPageIncrement = 10;
            pgcContactHistory.EndRowPerPage = 100;
        }

        private void DisplayContactHistoryPaging()
        {
            if (divCHPaging.Visible)
            {
                int rowPerPage = (int)ViewState["ContactHistoryRowPerPage"];

                pgcContactHistory.PageCount = gvContactHistory.PageCount;
                pgcContactHistory.CurrentRowPerPage = rowPerPage.ToString();
                pgcContactHistory.DisplayPaging();
            }
        }

        protected void ContactHistory_RowPerPageChanged(object sender, EventArgs<Int32> e)
        {
            //DataTable dtContactHistory = ViewState["dtContactHistory"] as DataTable;
            DataTable dtLeadsContact = GetLeadsContact();

            if (dtLeadsContact != null)
            {
                gvContactHistory.PageSize = e.Value;
                gvContactHistory.PageIndex = 0;
                gvContactHistory.DataSource = dtLeadsContact;
                gvContactHistory.DataBind();

                if (dtLeadsContact.Rows.Count < 1)
                {
                    gvFollowUp.Visible = false;
                }

                ViewState["ContactHistoryRowPerPage"] = e.Value;
                DisplayContactHistoryPaging();
            }
        }

        protected void ContactHistory_PageNoChange(object sender, EventArgs<Int32> e)
        {
            gvContactHistory.PageIndex = e.Value - 1;
            //DataTable dtContactHistory = ViewState["dtContactHistory"] as DataTable;

            DataTable dtClientContact = GetLeadsContact();

            if (dtClientContact != null)
            {
                gvContactHistory.DataSource = dtClientContact;
                gvContactHistory.DataBind();


            }
            else
            {
                gvFollowUp.Visible = false;
            }
        }

        private DataTable GetLeadsContact()
        {
            if (ViewState["ContactType"].ToString() == "Entry")
            {
                return ViewState["dtEntryHistory"] as DataTable;
            }
            else
            {
                return ViewState["dtContactHistory"] as DataTable;
            }
        }

        private void PrepareForContactEntry()
        {
            int returnCode = 1;
            string userRole = ViewState["UserRole"] as string;

            leadsContactService = new LeadsContactService(base.dbConnectionStr);
            DataSet ds = leadsContactService.PrepareForContactEntry(userRole, base.userLoginId);


            returnCode = int.Parse(ds.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString());
            if (returnCode == 1)
            {
                //no need to check for User or Admin in Leads Contact Entry
                //if (userRole == "user")
                //{
                hdfDealerCode.Value = ds.Tables[0].Rows[0]["AECode"].ToString();
                ViewState["dtAssignment"] = ds.Tables[2];       //Table Index 1 is Uncontacted Assignments
                gvAssignments.DataSource = ds.Tables[2];
                gvAssignments.DataBind();

                //ddlTeam.DataSource = ds.Tables[1];
                //ddlTeam.DataBind();

                if (ds.Tables[1].Rows.Count > 0)
                {
                    CommonUtilities.BindDataToDropDrownList(ddlTeam, ds.Tables[1], "TeamName", "AECode", "----- Select Dealer -----");
                }

                if (ds.Tables[2].Rows.Count > 0)
                {
                    divAssignPaging.Visible = true;
                    DisplayAssignmentsPaging();
                }
                else
                {
                    divAssignPaging.Visible = false;
                }

                //For Entry History
                if (ds.Tables["dtEntryHistory"].Rows.Count > 0)
                {
                    ViewState["dtEntryHistory"] = ds.Tables["dtEntryHistory"];
                    gvContactHistory.DataSource = ds.Tables["dtEntryHistory"];
                    gvContactHistory.DataBind();

                    if (ds.Tables["dtEntryHistory"].Rows.Count < 1)
                    {
                        gvFollowUp.Visible = false;
                    }

                    divCHPaging.Visible = true;
                    DisplayContactHistoryPaging();
                }

                //For Follow Up
                if (ds.Tables[4].Rows.Count > 0)
                {
                    ViewState["dtFollowUp"] = ds.Tables[4];
                    gvFollowUp.DataSource = ds.Tables[4];
                    gvFollowUp.DataBind();

                    divFollowUpPaging.Visible = true;
                    DisplayFollowUpPaging();
                }
                else
                {
                    ViewState["dtFollowUp"] = ds.Tables[4];
                    gvFollowUp.DataSource = ds.Tables[4];
                    gvFollowUp.DataBind();

                    divFollowUpPaging.Visible = false;
                    pgcFollowUp.StartRowPerPage = 1;
                }
                //}
                //else
                //{
                //    divAssignPaging.Visible = false;
                //    divCHPaging.Visible = false;
                //    CommonUtilities.BindDataToDropDrownListByFilter(ddlActualDealer, base.userLoginId, ds.Tables[0], "DisplayName","UserID", "----- Select Dealer -----");
                //}
            }
        }

        protected void btnAddContact_Click(object sender, EventArgs e)
        {
            string dealerCode = "", userId = "", adminId = "", userRole = "user", sex = "", validationResult = "",// needFollowUp = "Y",
                        lastleadId = "", lastLeadName = "", currentLeadid = "", teamCode = "", FollowupStatus = "N";
            string[] wsReturn = null; int result = 1;
            DataSet ds = null;
            DataTable dtContactHistory = ViewState["dtContactHistory"] as DataTable;
            DataRow drNewContact = null;
            leadsContactService = new LeadsContactService(base.dbConnectionStr);

            string followUpDealer = "";
            string followUpDate = "";

            //Validate Contact Entry Form
            validationResult = ValidateContactEntryForm();

            if (validationResult == "ok")
            {
                if (rbtnFemale.Checked)
                    sex = "F";
                else if (rbtnMale.Checked)
                    sex = "M";


                //if (chkFollowUp.Checked)
                //{
                //   // needFollowUp = "Y";                    
                //}
                //else
                //{
                //   // needFollowUp = "N";
                //}

                if (ViewState["UserRole"] != null)
                {
                    userRole = ViewState["UserRole"] as string;
                }

                if (userRole == "admin")
                {
                    dealerCode = ddlActualDealer.SelectedItem.Text.Split('-')[1].Trim();
                    userId = ddlActualDealer.SelectedValue;
                    adminId = base.userLoginId;
                }
                else
                {
                    dealerCode = hdfDealerCode.Value;
                    userId = base.userLoginId;
                    adminId = "";
                }

                //if ((dtContactHistory != null) && (dtContactHistory.Rows[0]["LeadId"].ToString().Equals(txtAccountNo.Text.Trim())))
                //{
                //    lastAcctNo = txtAccountNo.Text.Trim();
                //    clientName = lblAccountName.Text;
                //}


                if ((dtContactHistory != null) && (dtContactHistory.Rows[0]["LeadId"].ToString().Equals(lblLeadId.Text.Trim())))
                {
                    lastleadId = dtContactHistory.Rows[0]["LeadId"].ToString().Trim();
                    lastLeadName = txtName.Text.ToString();
                }

                if (chkFollowUp.Checked)
                {
                    followUpDealer = hdfDealerCode.Value;
                    followUpDate = calFollowupDate.DateTextFromValue;
                }

                //if (chkFollowUp.Checked)
                //{
                //    if (userRole == "admin")
                //    {
                //        followUpDealer = ddlActualDealer.SelectedItem.Text.Split('-')[1].Trim();
                //        //userId = ddlActualDealer.SelectedValue;
                //        //adminId = base.userLoginId;
                //    }
                //    else
                //    {
                //        followUpDealer = hdfDealerCode.Value;
                //        //userId = base.userLoginId;
                //        //adminId = "";
                //    }

                //    followUpDate = calFollowupDate.DateTextFromValue;
                //}

                if (hdfStatus.Value != "F")
                {
                    if (String.IsNullOrEmpty(lblLeadId.Text))//check for Walk In Leads
                    {
                        LeadsService leadsService = new LeadsService(base.dbConnectionStr);
                        DataSet dsMax = new DataSet(); string num = "0";
                        dsMax = leadsService.RetrieveMaxLeadsID();

                        if (dsMax.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString() == "1")
                        {
                            if (dsMax.Tables[0].Rows.Count > 0)
                            {
                                num = dsMax.Tables[0].Rows[0]["MaxLeadID"].ToString();

                                /// <OC Updated>
                                if (num == String.Empty)
                                {
                                    num = "0";
                                }
                            }

                            currentLeadid = getNewID(num);

                            int indexOfHyphen = ddlTeam.SelectedItem.Text.IndexOf("-");

                            teamCode = ddlTeam.SelectedItem.Text.Substring(0, indexOfHyphen - 1);

                            wsReturn = leadsService.InsertLeads(currentLeadid, txtName.Text.Trim(), txtNRIC.Text.Trim(), txtMobileNo.Text.Trim(),
                                txtHomeNo.Text.Trim(), sex, txtEmail.Text.Trim(), txtEvent.Text.Trim(), teamCode, dealerCode, "C");

                            if (wsReturn[0] == "1")
                            {

                            }
                            else
                            {
                                divMessage.InnerHtml = "Error in Leads Walk In Insertion!"; return;
                            }
                        }
                    }
                    else
                    {
                        currentLeadid = lblLeadId.Text.Trim();
                    }
                    //ds = leadsContactService.InsertLeadsContact(dealerCode, userId, currentLeadid, sex, txtMobileNo.Text.Trim(), txtHomeNo.Text.Trim(), txtContent.Text.Trim(),
                    //      needFollowUp, followUpDate, ddlPreferredMode.SelectedItem.Text.Trim(), lblProjectID.Text.Trim(), lastleadId, lastLeadName);


                    ds = leadsContactService.InsertLeadsContact(dealerCode, userId, currentLeadid, sex, txtMobileNo.Text.Trim(), txtHomeNo.Text.Trim(), txtContent.Text.Trim(),
                         FollowupStatus, followUpDate, ddlPreferredMode.SelectedItem.Text.Trim(), lblProjectID.Text.Trim(), lastleadId, lastLeadName);
                }
                else
                {//from gvFollowUp grid
                    ds = leadsContactService.InsertLeadsContact(dealerCode, userId, lblLeadId.Text.Trim(), sex, txtMobileNo.Text.Trim(), txtHomeNo.Text.Trim(), txtContent.Text.Trim(),
                        "F", null, ddlPreferredMode.SelectedItem.Text.Trim(), lblProjectID.Text.Trim(), lastleadId, lastLeadName);
                    result = leadsContactService.UpdateLeadsContactFollowup(dealerCode, userId, lblLeadId.Text.Trim(),
                      sex, txtMobileNo.Text.Trim(), txtHomeNo.Text.Trim(), txtContent.Text.Trim(), "Y",
                      ddlPreferredMode.SelectedItem.Text, lblProjectID.Text.Trim(), lastleadId, lastLeadName, hdfRecId.Value);

                    ViewState["dtContactHistory"] = leadsContactService.RetrieveContactEntryForToday(dealerCode, base.userLoginId).Tables[0];

                    dtContactHistory = (DataTable)ViewState["dtContactHistory"];
                }

                validationResult = ds.Tables["ReturnTable"].Rows[0]["ReturnMessage"].ToString();

                if (ds.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString() == "1")
                {
                    divMessage.InnerHtml = ds.Tables["ReturnTable"].Rows[0]["ReturnMessage"].ToString();

                    //Refresh on Uncontacted Assignments
                    DataTable dtAssignment = (DataTable)ViewState["dtAssignment"];
                    if (dtAssignment != null)
                    {
                        DataRow[] drDelete = dtAssignment.Select(" LeadId = '" + lblLeadId.Text.Trim() + "'");

                        if (drDelete.Length > 0)
                        {
                            //divMessage.InnerHtml = divMessage.InnerHtml + "<br /> To Delete Account No : " + drDelete[0]["AcctNo"].ToString();
                            dtAssignment.Rows.Remove(drDelete[0]);

                            if (dtAssignment.Rows.Count < 1)
                            {
                                divAssignPaging.Visible = false;
                            }

                            ViewState["dtAssignment"] = dtAssignment;

                            gvAssignments.DataSource = dtAssignment;
                            gvAssignments.DataBind();
                        }
                    }


                    //Refresh on Follow Up 
                    DataTable dtFollowUp = (DataTable)ViewState["dtFollowUp"];
                    if (dtFollowUp != null)
                    {
                        DataRow[] drDelete = dtFollowUp.Select(" LeadId = '" + lblLeadId.Text.Trim() + "'");

                        if (drDelete.Length > 0)
                        {
                            //divMessage.InnerHtml = divMessage.InnerHtml + "<br /> To Delete Account No : " + drDelete[0]["AcctNo"].ToString();
                            dtFollowUp.Rows.Remove(drDelete[0]);

                            if (dtFollowUp.Rows.Count < 1)
                            {
                                divFollowUpPaging.Visible = false;
                            }

                            ViewState["dtFollowUp"] = dtFollowUp;

                            gvFollowUp.DataSource = dtFollowUp;
                            gvFollowUp.DataBind();
                        }
                    }

                    if (!String.IsNullOrEmpty(lastleadId))
                    {
                        //Because of Contact Entry is inserted by one record, no need to check row count.                        
                        drNewContact = dtContactHistory.NewRow();

                        //drNewContact["AssignDate"] = ds.Tables["dtContactHistory"].Rows[0]["AssignDate"].ToString();

                        //Becz of adding on existing record, assign date should be old record.
                        if (String.IsNullOrEmpty(dtContactHistory.Rows[0]["AssignDate"].ToString()))
                            drNewContact["AssignDate"] = DBNull.Value;
                        else
                            drNewContact["AssignDate"] = dtContactHistory.Rows[0]["AssignDate"].ToString();

                        drNewContact["RecId"] = ds.Tables["dtContactHistory"].Rows[0]["RecId"].ToString();
                        drNewContact["ContactDate"] = ds.Tables["dtContactHistory"].Rows[0]["ContactDate"].ToString();
                        drNewContact["AEGroup"] = ds.Tables["dtContactHistory"].Rows[0]["AEGroup"].ToString();
                        drNewContact["LeadId"] = ds.Tables["dtContactHistory"].Rows[0]["LeadId"].ToString();
                        drNewContact["LeadName"] = ds.Tables["dtContactHistory"].Rows[0]["LeadName"].ToString();
                        drNewContact["Sex"] = ds.Tables["dtContactHistory"].Rows[0]["Sex"].ToString();

                        /// <Updated by OC>
                        /* Original is
                         * drNewContact["MobileNo"] = ds.Tables["dtContactHistory"].Rows[0]["LeadMoblie"].ToString();
                         * drNewContact["HomeNo"] = ds.Tables["dtContactHistory"].Rows[0]["LeadHome"].ToString(); */
                        drNewContact["LeadMobile"] = ds.Tables["dtContactHistory"].Rows[0]["MobileNo"].ToString();
                        drNewContact["LeadHome"] = ds.Tables["dtContactHistory"].Rows[0]["HomeNo"].ToString();

                        drNewContact["AECode"] = ds.Tables["dtContactHistory"].Rows[0]["AECode"].ToString();
                        drNewContact["Content"] = ds.Tables["dtContactHistory"].Rows[0]["Content"].ToString();
                        //drNewContact["Remarks"] = ds.Tables["dtContactHistory"].Rows[0]["Remarks"].ToString();
                        //drNewContact["PreferA"] = ds.Tables["dtContactHistory"].Rows[0]["PreferA"].ToString();

                        //if (hdfStatus.Value == "F")
                        //{
                        //    drNewContact["NeedFollowUp"] = "Y";
                        //    //drNewContact["FollowUpDate"] = null;// ds.Tables["dtContactHistory"].Rows[0]["FollowUpDate"].ToString();
                        //}
                        //else
                        //{
                        //    drNewContact["NeedFollowUp"] = ds.Tables["dtContactHistory"].Rows[0]["NeedFollowUp"].ToString();
                        //    drNewContact["FollowUpDate"] = ds.Tables["dtContactHistory"].Rows[0]["FollowUpDate"].ToString();
                        //}
                        drNewContact["NeedFollowUp"] = ds.Tables["dtContactHistory"].Rows[0]["NeedFollowUp"].ToString();

                        /// <Updated by OC>
                        //drNewContact["FollowUpDate"] = ds.Tables["dtContactHistory"].Rows[0]["FollowUpDate"].ToString();
                        if (ds.Tables["dtContactHistory"].Rows[0]["FollowUpDate"].ToString() == String.Empty)
                        {
                            drNewContact["NeedFollowUp"] = DateTime.MinValue;
                        }
                        else
                        {
                            DateTime.Parse(ds.Tables["dtContactHistory"].Rows[0]["FollowUpDate"].ToString());
                        }


                        drNewContact["PreferMode"] = ds.Tables["dtContactHistory"].Rows[0]["PreferMode"].ToString();
                        drNewContact["ModifiedUser"] = ds.Tables["dtContactHistory"].Rows[0]["ModifiedUser"].ToString();
                        // drNewContact["RankText"] = ds.Tables["dtContactHistory"].Rows[0]["RankText"].ToString();
                        dtContactHistory.Rows.InsertAt(drNewContact, 0);
                    }
                    else
                    {
                        if (hdfStatus.Value != "F")
                        {
                            dtContactHistory = null;
                            dtContactHistory = ds.Tables["dtContactHistory"];
                        }
                    }

                    InsertEntryHistory(dtContactHistory.Rows[0]);

                    tdContactHistory.BgColor = "#6E6A6B";
                    tdEntryHistory.BgColor = "";

                    ViewState["ContactType"] = "History";
                    ViewState["dtContactHistory"] = dtContactHistory;
                    gvContactHistory.DataSource = dtContactHistory;
                    gvContactHistory.DataBind();

                    if (dtContactHistory.Rows.Count < 1)
                    {
                        gvFollowUp.Visible = false;
                    }
                    divCHPaging.Visible = true;
                    DisplayContactHistoryPaging();

                    clearContactEntryForm();
                }

                #region Added by Thet Maung Chaw [To refresh gvFollowUP]

                //String ErrMsg = String.Empty;

                //DataTable dt = leadsContactService.RetrieveFollowUpLead(dealerCode, ref ErrMsg);

                //if (dt == null && ErrMsg != String.Empty)
                //{
                //    divMessage.InnerHtml = ErrMsg;
                //    return;
                //}
                //else
                //{
                //    gvFollowUp.DataSource = dt;
                //    gvFollowUp.DataBind();
                //}

                #endregion

                /// <Added by Thet Maung Chaw>
                if (chkFollowUp.Checked)
                {
                    SendFollowUpEmailToDealer(followUpDealer, txtName.Text.Trim());
                }

                /// <Added by Thet Maung Chaw>
                Response.Redirect("~/WebPages/LeadManagement/LeadContactEntry.aspx");
            }
            else
            {
                divMessage.InnerHtml = validationResult;
                divMessageTwo.InnerHtml = "";
            }
        }

        private void SendFollowUpEmailToDealer(string dealerCode, string accNo)
        {
            string EmailContent = string.Empty;
            string EmailContentsInfo = "";
            string dealerEmail = "";
            DataSet ds = new DataSet();
            EmailManager emailSender = new EmailManager();

            //get Email Template
            EmailContent = emailSender.getAssignmentAnnouncementEmail(EmailManager.FOLLOWUP_TEMPLATEID, base.dbConnectionStr);

            clientAssignmentService = new ClientAssignmentService(dbConnectionStr);
            ds = clientAssignmentService.RetrieveDealerEmailByDealerCode(dealerCode);
            if (ds.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString() == "1")
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    dealerEmail = dr["Email"].ToString();

                    string dealerName = dr["AEName"].ToString();
                    //EmailContentsInfo = emailSender.ReplaceSpecialcharacter(EmailContent, dealerName, "");
                    //EmailContentsInfo = emailSender.ReplaceAcctNo(EmailContentsInfo, accNo);
                    //EmailContentsInfo = emailSender.ReplaceSpecialcharacter(EmailContentsInfo);
                    EmailContentsInfo = emailSender.ReplaceSpecialcharacterForFollowUp(EmailContent, dealerName, accNo);
                }
            }

            if (!String.IsNullOrEmpty(dealerEmail))
            {
                CommonUtilities common = new CommonUtilities();
                if (common.isEmail(dealerEmail.Trim()))
                {
                    string EmailSubject = "Assistance Required by Client - << " + accNo + " >>";
                    emailSender.SendEmail("spm@phillip.com.sg", dealerEmail, EmailSubject, EmailContentsInfo);
                    clientAssignmentService.InsertEmailLog("spm@phillip.com.sg", dealerEmail, EmailSubject, EmailContentsInfo);
                }
            }
        }

        protected void btnUpdateContact_Click(object sender, EventArgs e)
        {
            string dealerCode = "", userId = "", adminId = "", userRole = "user", sex = "", validationResult = "", FollowupStatus = "N",//needFollowUp = "Y", 
                        lastLeadId = "", leadName = "";
            string followUpDealer = "";
            string followUpDate = null;

            string[] wsReturn = null;
            DataTable dtEntryHistory = ViewState["dtEntryHistory"] as DataTable;
            DataRow drNewContact = null;
            leadsContactService = new LeadsContactService(base.dbConnectionStr);

            //Validate Contact Entry Form
            validationResult = ValidateContactEntryForm();

            if (validationResult == "ok")
            {
                if (rbtnFemale.Checked)
                    sex = "F";
                else if (rbtnMale.Checked)
                    sex = "M";

                //if (chkFollowUp.Checked)
                //{
                //    needFollowUp = "Y";
                //}
                //else
                //{
                //    needFollowUp = "N";
                //}


                if (ViewState["UserRole"] != null)
                {
                    userRole = ViewState["UserRole"] as string;
                }

                //if (chkFollowUp.Checked)
                //{
                //    // followUpDealer = ddlActualDealer.SelectedValue;
                //    followupDate = calFollowupDate.DateTextFromValue;
                //}


                if (chkFollowUp.Checked)
                {
                    followUpDealer = hdfDealerCode.Value;
                    followUpDate = calFollowupDate.DateTextFromValue;
                }

                if (userRole == "admin")
                {
                    dealerCode = ddlActualDealer.SelectedItem.Text.Split('-')[1].Trim();
                    userId = ddlActualDealer.SelectedValue;
                    adminId = base.userLoginId;
                }
                else
                {
                    dealerCode = hdfDealerCode.Value;
                    userId = base.userLoginId;
                    adminId = "";
                }

                wsReturn = leadsContactService.UpdateLeadsContact(dealerCode, userId, lblLeadId.Text.Trim(),
                    sex, txtMobileNo.Text.Trim(), txtHomeNo.Text.Trim(), txtContent.Text.Trim(), FollowupStatus, followUpDate,
                    ddlPreferredMode.SelectedItem.Text, lblProjectID.Text.Trim(), hdfRecId.Value);

                if (int.Parse(wsReturn[0]) > 0)
                {
                    drNewContact = dtEntryHistory.Rows[int.Parse(hdfModifyIndex.Value)];


                    //drNewContact["RecId"] = ds.Tables["dtContactHistory"].Rows[0]["RecId"].ToString();
                    drNewContact["ContactDate"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");//ds.Tables["dtContactHistory"].Rows[0]["ContactDate"].ToString();
                    drNewContact["AEGroup"] = ddlTeam.SelectedValue.ToString();//ddlActualDealer.SelectedValue.ToString();//ds.Tables["dtContactHistory"].Rows[0]["AEGroup"].ToString();
                    drNewContact["LeadId"] = lblLeadId.Text.Trim();// ds.Tables["dtContactHistory"].Rows[0]["LeadId"].ToString();
                    drNewContact["LeadName"] = txtName.Text;//ds.Tables["dtContactHistory"].Rows[0]["LeadName"].ToString();
                    drNewContact["Sex"] = sex;//ds.Tables["dtContactHistory"].Rows[0]["Sex"].ToString();
                    drNewContact["LeadMobile"] = txtMobileNo.Text;// ds.Tables["dtContactHistory"].Rows[0]["MoblieNo"].ToString();

                    /// <Updated by OC>
                    drNewContact["LeadHome"] = txtHomeNo.Text;//ds.Tables["dtContactHistory"].Rows[0]["HomeNo"].ToString();

                    drNewContact["Content"] = txtContent.Text;//ds.Tables["dtContactHistory"].Rows[0]["Content"].ToString();
                    drNewContact["Event"] = txtEvent.Text;//ds.Tables["dtContactHistory"].Rows[0]["Remarks"].ToString();
                    drNewContact["LeadEmail"] = txtEmail.Text;//ds.Tables["dtContactHistory"].Rows[0]["PreferA"].ToString();
                    // drNewContact["SeminarName"] = "";//ds.Tables["dtContactHistory"].Rows[0]["PreferB"].ToString();
                    drNewContact["NeedFollowUp"] = FollowupStatus; //ds.Tables["dtContactHistory"].Rows[0]["NeedFollowUp"].ToString();
                    if (chkFollowUp.Checked)
                    {
                        //drNewContact["FollowUpDate"] = calFollowupDate.DateTextFromValue;//Convert.ToDateTime(calFollowupDate.DateTextFromValue);//ds.Tables["dtContactHistory"].Rows[0]["FollowUpDate"].ToString();
                        if (!string.IsNullOrEmpty(calFollowupDate.DateTextFromValue))
                        {
                            IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                            String datetime = calFollowupDate.DateTextFromValue;
                            drNewContact["FollowUpDate"] = DateTime.Parse(datetime, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                        }
                    }
                    else
                    {
                        drNewContact["FollowUpDate"] = DBNull.Value;
                    }

                    drNewContact["PreferMode"] = ddlPreferredMode.SelectedValue.ToString();//ds.Tables["dtContactHistory"].Rows[0]["PreferMode"].ToString();
                    drNewContact["ModifiedUser"] = dealerCode;//ds.Tables["dtContactHistory"].Rows[0]["ModifiedUser"].ToString();

                    ViewState["ContactType"] = "Entry";
                    ViewState["dtEntryHistory"] = dtEntryHistory;
                    gvContactHistory.PageIndex = int.Parse(pgcContactHistory.CurrentPageNo) - 1;
                    gvContactHistory.DataSource = dtEntryHistory;
                    gvContactHistory.DataBind();

                    if (dtEntryHistory.Rows.Count < 1)
                    {
                        gvFollowUp.Visible = false;
                    }

                    clearContactEntryForm();
                }

                validationResult = wsReturn[1];

                btnUpdateContact.Visible = false;
                btnCancel.Visible = false;
                btnAddContact.Visible = true;
                btnSync.Visible = true;
            }

            divMessage.InnerHtml = validationResult;

            /// <Added by Thet Maung Chaw to refresh the Follow Up GridView.>
            Response.Redirect("~/WebPages/LeadManagement/LeadContactEntry.aspx");
        }

        protected void gvAssignments_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if ((e.CommandName == "LeadId") || (e.CommandName == "LeadName"))
            {
                hdfStatus.Value = "A";

                int index = Convert.ToInt32(e.CommandArgument);
                DataTable dtAssignment = (DataTable)ViewState["dtAssignment"];

                if (e.CommandName == "LeadId")
                {
                    string sex = "";

                    if (String.IsNullOrEmpty(dtAssignment.Rows[index]["AECode"].ToString()))
                        ddlTeam.SelectedIndex = 0;
                    else if (ddlTeam.Items.FindByValue(dtAssignment.Rows[index]["AECode"].ToString().Trim()) != null)
                        ddlTeam.SelectedValue = dtAssignment.Rows[index]["AECode"].ToString().Trim();

                    lblLeadId.Text = dtAssignment.Rows[index]["LeadId"].ToString();
                    txtName.Text = dtAssignment.Rows[index]["LeadName"].ToString();
                    txtMobileNo.Text = dtAssignment.Rows[index]["LeadMobile"].ToString();
                    txtHomeNo.Text = dtAssignment.Rows[index]["LeadHomeNo"].ToString();
                    txtEvent.Text = dtAssignment.Rows[index]["Event"].ToString();
                    txtEmail.Text = dtAssignment.Rows[index]["LeadEmail"].ToString();

                    sex = dtAssignment.Rows[index]["Sex"].ToString();

                    rbtnMale.Checked = rbtnFemale.Checked = false;
                    if (sex == "F")
                        rbtnFemale.Checked = true;
                    else if (sex == "M")
                        rbtnMale.Checked = true;

                    if (String.IsNullOrEmpty(dtAssignment.Rows[index]["PreferMode"].ToString()))
                        ddlPreferredMode.SelectedIndex = 0;
                    else if (ddlPreferredMode.Items.FindByValue(dtAssignment.Rows[index]["PreferMode"].ToString().Trim()) != null)
                        ddlPreferredMode.SelectedValue = dtAssignment.Rows[index]["PreferMode"].ToString().Trim();

                    lblProjectID.Text = dtAssignment.Rows[index]["ProjectID"].ToString();
                    //if (String.IsNullOrEmpty(dtAssignment.Rows[index]["PreferA"].ToString()))
                    //    ddlPreferenceOne.SelectedIndex = 0;
                    //else if (ddlPreferenceOne.Items.FindByValue(dtAssignment.Rows[index]["PreferA"].ToString().Trim()) != null)
                    //    ddlPreferenceOne.SelectedValue = dtAssignment.Rows[index]["PreferA"].ToString().Trim();


                }

                //Retrieve contact history.(it will retrieve when user click LeadId (or) Leads name)
                if (!String.IsNullOrEmpty(dtAssignment.Rows[index]["LeadId"].ToString()))
                {
                    contactType = "History";
                    ViewState["ContactType"] = "History";
                    lbtnContactHistory.Enabled = true;
                    this.retrieveContactHistory(dtAssignment.Rows[index]["LeadId"].ToString());
                }
            }
        }

        protected void gvAssignments_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string cutOffDtStr = "";
                string contactDtStr = "";

                /*
                if (DataBinder.Eval(e.Row.DataItem, "CutOffDate") != null)
                {
                    cutOffDtStr = DataBinder.Eval(e.Row.DataItem, "CutOffDate").ToString();
                    if (!String.IsNullOrEmpty(cutOffDtStr))
                    {
                        e.Row.Cells[12].Text = DateTime.ParseExact(cutOffDtStr, "yyyy-MM-dd HH:mm:ss", null).ToString("dd/MM/yyyy HH:mm:ss");                      
                    }
                }
                */

                if (DataBinder.Eval(e.Row.DataItem, "ContactDate") != null)
                {
                    contactDtStr = DataBinder.Eval(e.Row.DataItem, "ContactDate").ToString();
                    if (!String.IsNullOrEmpty(contactDtStr))
                    {
                        e.Row.Cells[10].Text = DateTime.ParseExact(contactDtStr, "yyyy-MM-dd HH:mm:ss", null).ToString("dd/MM/yyyy HH:mm:ss");
                    }
                }
            }
        }

        protected void gvAssignments_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataTable dtAssignments = ViewState["dtAssignment"] as DataTable;
            DataTable sortedDataTable = null;

            string sortDirection = "", sortString = "";

            sortDirection = CommonUtilities.GetSortDirection(e.SortExpression, ViewState["SortExpression"] as string, ViewState["SortDirection"] as string);

            ViewState["SortDirection"] = sortDirection;
            ViewState["SortExpression"] = e.SortExpression;
            sortString = e.SortExpression + " " + sortDirection;
            sortedDataTable = CommonUtilities.SortDataTable(dtAssignments, sortString);

            ViewState["dtAssignment"] = sortedDataTable;
            gvAssignments.PageIndex = 0;
            gvAssignments.DataSource = sortedDataTable;
            gvAssignments.DataBind();
            DisplayAssignmentsPaging();

            dtAssignments.Dispose();
        }

        protected void gvContactHistory_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                System.Web.UI.WebControls.Button btnModify = (System.Web.UI.WebControls.Button)e.Row.Cells[16].FindControl("gvbtnEdit");
                System.Web.UI.WebControls.Button btnDelete = (System.Web.UI.WebControls.Button)e.Row.Cells[16].FindControl("gvbtnDelete");

                if (btnModify != null)
                {
                    ((System.Web.UI.WebControls.Button)e.Row.Cells[16].FindControl("gvbtnEdit")).Enabled = (bool)ViewState["ModifyAccessRight"];

                    //if (accessRight[2] == "N")
                    //{
                    //    ((System.Web.UI.WebControls.Button)e.Row.Cells[16].FindControl("gvbtnEdit")).Enabled = false;
                    //}
                    //else
                    //{
                    //    ((System.Web.UI.WebControls.Button)e.Row.Cells[16].FindControl("gvbtnEdit")).Enabled = true;
                    //}
                }
                if (btnDelete != null)
                {
                    ((System.Web.UI.WebControls.Button)e.Row.Cells[16].FindControl("gvbtnDelete")).Enabled = (bool)ViewState["DeleteAccessRight"];

                    //if (accessRight[3] == "N")
                    //{
                    //    ((System.Web.UI.WebControls.Button)e.Row.Cells[16].FindControl("gvbtnDelete")).Enabled = false;
                    //}
                    //else
                    //{
                    //    ((System.Web.UI.WebControls.Button)e.Row.Cells[16].FindControl("gvbtnDelete")).Enabled = true;
                    //}
                }

                //string cutOffDtStr = "";
                //DataTable dtPreferList = ViewState["dtPreferList"] as DataTable;                
                //DataRow drPreference = null;


                //if (DataBinder.Eval(e.Row.DataItem, "ContactDate") != null)
                //{
                //    cutOffDtStr = DataBinder.Eval(e.Row.DataItem, "ContactDate").ToString();
                //    if (!String.IsNullOrEmpty(cutOffDtStr))
                //    {
                //        e.Row.Cells[1].Text = DateTime.ParseExact(cutOffDtStr, "yyyy-MM-dd HH:mm:ss", null).ToString("dd/MM/yyyy HH:mm:ss");
                //    }
                //}


                if (e.Row.Cells[13].Text == "Y")
                {
                    e.Row.Cells[13].Text = "Yes";//DateTime.ParseExact(cutOffDtStr, "yyyy-MM-dd HH:mm:ss", null).ToString("dd/MM/yyyy HH:mm:ss");
                }
                else
                {
                    e.Row.Cells[13].Text = "No";
                }
                if (ViewState["ContactType"].ToString() == "History")
                {
                    //Check for Extra Call
                    //if (String.IsNullOrEmpty(DataBinder.Eval(e.Row.DataItem, "AssignDate").ToString()))
                    //{
                    //    System.Drawing.Color c = System.Drawing.ColorTranslator.FromHtml("#FFCC99");

                    //    e.Row.BackColor = c;
                    //}

                    ((LinkButton)e.Row.FindControl("gvlbtnLeadId")).Visible = false;
                    ((LinkButton)e.Row.FindControl("gvlbtnLeadName")).Visible = false;
                }
                else
                {
                    ((Label)e.Row.FindControl("gvlblLeadId")).Visible = false;
                    ((Label)e.Row.FindControl("gvlblLeadName")).Visible = false;
                }
            }

            //Modify and Delete button is off for Contact History
            if (ViewState["ContactType"].ToString() == "History")
            {
                e.Row.Cells[16].Visible = false;
            }
        }

        protected void gvContactHistory_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if ((e.CommandName == "AcctNo") || (e.CommandName == "ClientName"))
            {
                int index = Convert.ToInt32(e.CommandArgument);
                DataTable dtEntryHistory = ViewState["dtEntryHistory"] as DataTable;

                if (dtEntryHistory != null)
                {
                    DataRow drModify = dtEntryHistory.Rows[index];
                    if (e.CommandName == "AcctNo")
                    {
                        DisplayContactDetail(drModify["LeadId"].ToString(), drModify["LeadName"].ToString(), drModify["LeadNRIC"].ToString(),
                            drModify["Sex"].ToString(), drModify["LeadMobile"].ToString(), dtEntryHistory.Columns.Contains("LeadHomeNo") ? drModify["LeadHomeNo"].ToString() : drModify["LeadHome"].ToString(),
                            drModify["LeadEmail"].ToString(), drModify["PreferMode"].ToString(), drModify["Content"].ToString(),
                            drModify["Event"].ToString(), "", drModify["NeedFollowUp"].ToString(),
                            drModify["FollowUpDate"].ToString(), drModify["AECode"].ToString(), drModify["ProjectID"].ToString());
                    }

                    ViewState["ContactType"] = "History";
                    lbtnContactHistory.Enabled = true;
                    this.retrieveContactHistory(drModify["LeadId"].ToString());
                }
            }
        }

        protected void gvContactHistory_Sorting(object sender, GridViewSortEventArgs e)
        {
            string tableName = "dtEntryHistory";

            if (ViewState["ContactType"].ToString() == "History")
            {
                tableName = "dtContactHistory";
            }

            DataTable dtClientContact = ViewState[tableName] as DataTable;
            DataTable sortedDataTable = null;

            string sortDirection = "", sortString = "";

            sortDirection = CommonUtilities.GetSortDirection(e.SortExpression, ViewState["ContactSortExpression"] as string, ViewState["ContactSortDirection"] as string);

            ViewState["ContactSortDirection"] = sortDirection;
            ViewState["ContactSortExpression"] = e.SortExpression;
            sortString = e.SortExpression + " " + sortDirection;
            sortedDataTable = CommonUtilities.SortDataTable(dtClientContact, sortString);

            ViewState[tableName] = sortedDataTable;
            gvContactHistory.PageIndex = 0;
            gvContactHistory.DataSource = sortedDataTable;
            gvContactHistory.DataBind();
            DisplayContactHistoryPaging();

            dtClientContact.Dispose();
        }

        //To retrieve Client Info by Short Key
        //protected void btnShortKey_Click(object sender, EventArgs e)
        //{
        //    divMessage.InnerHtml = "";

        //    if (!String.IsNullOrEmpty(txtKey.Text.Trim()))
        //    {
        //        txtKey.Text = txtKey.Text.Trim().ToUpper();

        //        leadsContactService = new leadsContactService(base.dbConnectionStr);
        //        DataSet ds = leadsContactService.RetrieveClientInfoByShortKey(txtKey.Text);

        //        if (ds.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString() == "1")
        //        {

        //            DisplayContactDetail(ds.Tables[0].Rows[0]["AcctNo"].ToString(), ds.Tables[0].Rows[0]["LNAME"].ToString(),
        //                txtKey.Text.Trim(), ds.Tables[0].Rows[0]["Phone"].ToString(), ds.Tables[0].Rows[0]["Sex"].ToString(),
        //                ds.Tables[0].Rows[0]["PreferA"].ToString(), ds.Tables[0].Rows[0]["PreferB"].ToString(),
        //                "", "", "0");

        //            /*
        //            lblAccountName.Text = ds.Tables[0].Rows[0]["LNAME"].ToString();
        //            txtAccountNo.Text = ds.Tables[0].Rows[0]["AcctNo"].ToString();
        //            txtPhone.Text = ds.Tables[0].Rows[0]["Phone"].ToString();

        //            rbtnUnknown.Checked = rbtnMale.Checked = rbtnFemale.Checked = false;

        //            if (ds.Tables[0].Rows[0]["Sex"].ToString().Trim() == "F")
        //                rbtnFemale.Checked = true;
        //            else if (ds.Tables[0].Rows[0]["Sex"].ToString().Trim() == "M")
        //                rbtnMale.Checked = true;
        //            else
        //                rbtnUnknown.Checked = true;


        //            if (ddlPreferenceOne.Items.FindByValue(ds.Tables[0].Rows[0]["PreferA"].ToString().Trim()) != null)
        //                ddlPreferenceOne.SelectedValue = ds.Tables[0].Rows[0]["PreferA"].ToString().Trim();


        //            if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["PreferB"].ToString().Trim()))
        //            {
        //                if (ddlPreferenceTwo.Items.FindByValue(ds.Tables[0].Rows[0]["PreferB"].ToString().Trim()) != null)
        //                    ddlPreferenceTwo.SelectedValue = ds.Tables[0].Rows[0]["PreferB"].ToString();
        //            }
        //            */
        //        }
        //        else
        //        {
        //            divMessage.InnerHtml = "Client Short Key not found!";
        //            /*
        //            txtAccountNo.Text = "";
        //            txtPhone.Text = "";

        //            rbtnFemale.Checked = false;
        //            rbtnMale.Checked = false;

        //            ddlPreferenceOne.SelectedIndex = 0;
        //            ddlPreferenceTwo.SelectedIndex = 1;
        //            */
        //        }
        //    }
        //}

        protected void ddlActualDealer_SelectedIndexChanged(object sender, EventArgs e)
        {
            leadsContactService = new LeadsContactService(base.dbConnectionStr);
            DataSet ds = null;
            string dealerCode = "";

            divAssignPaging.Visible = false;

            //Clear Previous Dealer Contact History
            ViewState["dtContactHistory"] = null;

            if (!String.IsNullOrEmpty(ddlActualDealer.SelectedValue))
            {
                dealerCode = ddlActualDealer.SelectedItem.Text.Split('-')[1].Trim();
                ds = leadsContactService.RetrieveUnContactedAssignment(dealerCode, base.userLoginId);
                if (ds.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString() == "1")
                {
                    if ((ds.Tables["dtUnContactedAssignment"] != null) && (ds.Tables["dtUnContactedAssignment"].Rows.Count > 0))
                    {
                        ViewState["dtAssignment"] = ds.Tables["dtUnContactedAssignment"];
                        gvAssignments.DataSource = ds.Tables["dtUnContactedAssignment"];
                        gvAssignments.DataBind();

                        divAssignPaging.Visible = true;
                        DisplayAssignmentsPaging();
                    }
                    else
                    {
                        ViewState["dtAssignment"] = null;
                        gvAssignments.DataSource = null;
                        gvAssignments.DataBind();

                        divAssignPaging.Visible = false;
                    }

                    //Retrieve History Entry for Contact Entry Admin
                    if ((ds.Tables["dtEntryHistory"] != null) && (ds.Tables["dtEntryHistory"].Rows.Count > 0))
                    {
                        ViewState["ContactType"] = "Entry";
                        ViewState["dtEntryHistory"] = ds.Tables["dtEntryHistory"];

                        gvContactHistory.PageIndex = 0;
                        gvContactHistory.DataSource = ds.Tables["dtEntryHistory"];
                        gvContactHistory.DataBind();

                        if (ds.Tables["dtEntryHistory"].Rows.Count < 1)
                        {
                            gvFollowUp.Visible = false;
                        }

                        divCHPaging.Visible = true;
                        DisplayContactHistoryPaging();

                        tdEntryHistory.BgColor = "#6E6A6B";
                        tdContactHistory.BgColor = "";
                    }
                    else
                    {
                        ViewState["dtEntryHistory"] = null;
                        gvContactHistory.DataSource = null;
                        gvContactHistory.DataBind();

                        gvFollowUp.Visible = false;

                        divCHPaging.Visible = false;
                    }
                }
                else
                {
                    ViewState["dtAssignment"] = null;
                    gvAssignments.DataSource = null;
                    gvAssignments.DataBind();

                    ViewState["dtEntryHistory"] = null;
                    gvContactHistory.DataSource = null;
                    gvContactHistory.DataBind();

                    gvFollowUp.Visible = false;

                    divCHPaging.Visible = false;
                    divAssignPaging.Visible = false;
                }
            }
            else
            {
                ViewState["dtEntryHistory"] = null;
                ViewState["dtAssignment"] = null;
                gvAssignments.DataSource = null;
                gvAssignments.DataBind();

                gvContactHistory.DataSource = null;
                gvContactHistory.DataBind();

                gvFollowUp.Visible = false;

                divCHPaging.Visible = false;
                divAssignPaging.Visible = false;
            }

            clearContactEntryForm();
            divMessage.InnerHtml = "";
            divMessageTwo.InnerHtml = "";
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            clearContactEntryForm();
            btnUpdateContact.Visible = false;
            btnCancel.Visible = false;
            btnAddContact.Visible = true;
            btnSync.Visible = true;
        }

        protected void lbtnEntryHistory_Click(object sender, EventArgs e)
        {
            ViewState["ContactType"] = "Entry";

            tdEntryHistory.BgColor = "#6E6A6B";     //#595454,
            tdContactHistory.BgColor = "";

            RetrieveEntryHistory();
        }

        private void RetrieveEntryHistory()
        {
            DataTable dtClientContact = null;
            if (ViewState["dtEntryHistory"] == null)
            {
                leadsContactService = new LeadsContactService(base.dbConnectionStr);
                DataSet ds = null;
                string userRole = "", dealerCode = "";

                if (ViewState["UserRole"] != null)
                {
                    userRole = ViewState["UserRole"] as string;
                }

                if (userRole == "admin")
                {
                    dealerCode = ddlActualDealer.SelectedItem.Text.Split('-')[1].Trim();
                }
                else
                {
                    dealerCode = hdfDealerCode.Value;
                }

                ds = leadsContactService.RetrieveContactEntryForToday(dealerCode, base.userLoginId);
                if (ds.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString() == "1")
                {
                    dtClientContact = ds.Tables["dtEntryHistory"];
                    ViewState["dtEntryHistory"] = ds.Tables["dtEntryHistory"];
                }
                else
                {
                    //divMessageTwo.InnerHtml = "No Record found!";
                }

                //divMessage.InnerHtml = ds.Tables["ReturnTable"].Rows[0]["ReturnMessage"].ToString();
            }
            else
            {
                dtClientContact = ViewState["dtEntryHistory"] as DataTable;
            }


            if ((dtClientContact != null) && (dtClientContact.Rows.Count > 0))
            {
                DisplayContactHistoryPaging();
                divCHPaging.Visible = true;
                lbtnContactHistory.Enabled = false;
            }
            else
            {
                divCHPaging.Visible = false;
            }

            /// <Added by Thet Maung Chaw>
            if (dtClientContact != null && dtClientContact.Columns.Contains("LeadHomeNo"))
            {
                dtClientContact.Columns["LeadHomeNo"].ColumnName = "LeadHome";
            }

            gvContactHistory.DataSource = dtClientContact;
            gvContactHistory.DataBind();

            if (dtClientContact != null && dtClientContact.Rows.Count < 1)
            {
                gvFollowUp.Visible = false;
            }
        }

        protected override void DisplayDetails(int modifyIndex)
        {
            btnUpdateContact.Visible = true;
            btnCancel.Visible = true;
            btnAddContact.Visible = false;
            btnSync.Visible = false;
            this.hdfModifyIndex.Value = modifyIndex.ToString();

            int dataItemIndex = 1;
            DataTable dtEntryHistory = ViewState["dtEntryHistory"] as DataTable;

            HiddenField gvhdfRecordId = (HiddenField)gvContactHistory.Rows[modifyIndex].FindControl("gvhdfRecordId");

            dataItemIndex = int.Parse(gvhdfRecordId.Value);
            if (dtEntryHistory != null)
            {
                DataRow drModify = dtEntryHistory.Rows[dataItemIndex];

                hdfRecId.Value = drModify["RecId"].ToString();
                DisplayContactDetail(drModify["LeadId"].ToString(), drModify["LeadName"].ToString(), drModify["LeadNRIC"].ToString(),
                   drModify["Sex"].ToString(), drModify["LeadMobile"].ToString(), drModify["LeadHome"].ToString(), drModify["LeadEmail"].ToString(),
                   drModify["PreferMode"].ToString(), drModify["Content"].ToString(), drModify["Event"].ToString(), "",
                   drModify["NeedFollowUp"].ToString(), drModify["FollowUpDate"].ToString(), drModify["AECode"].ToString(), drModify["ProjectID"].ToString());
            }
        }

        protected override Hashtable DeleteRecord(int deleteIndex)
        {
            long deleteRecId = long.Parse(gvContactHistory.DataKeys[deleteIndex].Value.ToString());

            HiddenField gvhdfRecordId = (HiddenField)gvContactHistory.Rows[deleteIndex].FindControl("gvhdfRecordId");
            leadsContactService = new LeadsContactService(base.dbConnectionStr);
            Hashtable ht = new Hashtable();
            DataTable dtEntryHistory = ViewState["dtEntryHistory"] as DataTable;
            DataSet ds = null;
            string[] wsReturn = null;
            string dealerCode = "", userId = "", userRole = "";


            ht.Add("ReturnData", null);
            ht.Add("ReturnCode", "-1");
            ht.Add("ReturnMessage", "");


            if (ViewState["UserRole"] != null)
            {
                userRole = ViewState["UserRole"] as string;
            }

            if (userRole == "admin")
            {
                dealerCode = ddlActualDealer.SelectedItem.Text.Split('-')[1].Trim();
                userId = ddlActualDealer.SelectedValue;
            }
            else
            {
                dealerCode = hdfDealerCode.Value;
                userId = base.userLoginId;
            }

            ds = leadsContactService.DeleteLeadsContact(deleteRecId.ToString(), dealerCode, base.userLoginId);
            if (ds.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString() == "1")
            {
                dtEntryHistory.Rows.RemoveAt(int.Parse(gvhdfRecordId.Value));

                if (dtEntryHistory.Rows.Count < 1)
                {
                    divCHPaging.Visible = false;
                }

                ViewState["dtEntryHistory"] = dtEntryHistory;

                ht["ReturnData"] = dtEntryHistory;
                ht["ReturnCode"] = ds.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString();
                //ht["ReturnMessage"] = wsReturn[1];

                divMessageTwo.InnerText = ds.Tables["ReturnTable"].Rows[0]["ReturnMessage"].ToString();

                if ((ds.Tables["dtUnContactedAssignment"] != null) &&
                        (ds.Tables["dtUnContactedAssignment"].Rows.Count > 0))
                {
                    ViewState["dtAssignment"] = ds.Tables["dtUnContactedAssignment"];
                    gvAssignments.PageIndex = 0;
                    gvAssignments.DataSource = ds.Tables["dtUnContactedAssignment"];
                    gvAssignments.DataBind();

                    divAssignPaging.Visible = true;
                    DisplayAssignmentsPaging();
                }
                else
                {
                    ViewState["dtAssignment"] = null;
                    gvAssignments.DataSource = null;
                    gvAssignments.DataBind();

                    divAssignPaging.Visible = false;
                }

                PrepareForContactEntry();
            }

            return ht;
        }

        //Helper Methods

        private void InsertEntryHistory(DataRow dr)
        {
            DataTable dtEntryHistory = ViewState["dtEntryHistory"] as DataTable;
            DataRow drNewContact = null;

            if (dtEntryHistory == null)
            {
                dtEntryHistory = new DataTable("dtEntryHistory");
                dtEntryHistory.Columns.Add("RecId", String.Empty.GetType());
                dtEntryHistory.Columns.Add("ContactDate", String.Empty.GetType());
                dtEntryHistory.Columns.Add("AEGroup", String.Empty.GetType());
                dtEntryHistory.Columns.Add("LeadId", String.Empty.GetType());
                dtEntryHistory.Columns.Add("LeadName", String.Empty.GetType());
                dtEntryHistory.Columns.Add("LeadNRIC", String.Empty.GetType());
                dtEntryHistory.Columns.Add("Sex", String.Empty.GetType());
                dtEntryHistory.Columns.Add("LeadMobile", String.Empty.GetType());
                dtEntryHistory.Columns.Add("LeadHomeNo", String.Empty.GetType());
                dtEntryHistory.Columns.Add("LeadEmail", String.Empty.GetType());
                dtEntryHistory.Columns.Add("PreferMode", String.Empty.GetType());
                dtEntryHistory.Columns.Add("Content", String.Empty.GetType());
                dtEntryHistory.Columns.Add("Event", String.Empty.GetType());
                dtEntryHistory.Columns.Add("SeminarName", String.Empty.GetType());
                dtEntryHistory.Columns.Add("AECode", string.Empty.GetType());
                dtEntryHistory.Columns.Add("NeedFollowUp", String.Empty.GetType());
                dtEntryHistory.Columns.Add("FollowupDate", String.Empty.GetType());
                dtEntryHistory.Columns.Add("ModifiedUser", String.Empty.GetType());
                dtEntryHistory.Columns.Add("AssignDate", String.Empty.GetType());
                dtEntryHistory.Columns.Add("ProjectID", String.Empty.GetType());
                // dtEntryHistory.Columns.Add("ShortKey", String.Empty.GetType());     //It will get from txtKey
                //dtEntryHistory.Columns.Add("RankText", String.Empty.GetType());
            }

            drNewContact = dtEntryHistory.NewRow();
            drNewContact["RecId"] = dr["RecId"].ToString();
            drNewContact["ContactDate"] = dr["ContactDate"].ToString();
            drNewContact["AEGroup"] = dr["AEGroup"].ToString();
            drNewContact["LeadId"] = dr["LeadId"].ToString();
            drNewContact["LeadName"] = dr["LeadName"].ToString();
            drNewContact["LeadNRIC"] = dr["LeadNRIC"].ToString();
            drNewContact["Sex"] = dr["Sex"].ToString();
            drNewContact["LeadMobile"] = dr["LeadMobile"].ToString();

            /// <Updated by OC>
            if (dtEntryHistory.Columns.Contains("LeadHome"))
            {
                drNewContact["LeadHome"] = dr["LeadHome"].ToString();
            }
            else if (dtEntryHistory.Columns.Contains("LeadHomeNo"))
            {
                drNewContact["LeadHomeNo"] = dr["LeadHome"].ToString();
            }

            drNewContact["LeadEmail"] = dr["LeadEmail"].ToString();
            drNewContact["PreferMode"] = dr["PreferMode"].ToString();
            drNewContact["Content"] = dr["Content"].ToString();
            drNewContact["Event"] = dr["Event"].ToString();

            /// <Updated by Thet Maung Chaw>
            if (dr.Table.Columns.Contains("SeminarName"))
            {
                drNewContact["SeminarName"] = dr["SeminarName"].ToString();
            }

            drNewContact["AECode"] = dr["AECode"].ToString();

            drNewContact["NeedFollowUp"] = dr["NeedFollowUp"].ToString();
            //drNewContact["FollowupDate"] = dr["FollowupDate"].ToString();

            if (chkFollowUp.Checked)
            {
                drNewContact["FollowupDate"] = dr["FollowupDate"].ToString();
            }
            else
            {
                drNewContact["FollowupDate"] = DBNull.Value;
            }
            drNewContact["ModifiedUser"] = dr["ModifiedUser"].ToString();
            drNewContact["AssignDate"] = DBNull.Value;
            //drNewContact["ShortKey"] = txtKey.Text.Trim();
            //drNewContact["RankText"] = dr["RankText"].ToString();

            dtEntryHistory.Rows.InsertAt(drNewContact, 0);
            ViewState["dtEntryHistory"] = dtEntryHistory;
        }

        /*
        private void UpdateHistoryEntry(string accountNo, string sex, string content, string remark)
        {

        }
        */

        private void DisplayContactDetail(string leadId, string leadName, string leadNRIC, string sex, string mobileNo, string homeno,
                       string email, string prefermode, string content, string levent, string seminars, string needfollowup, string followupdate,
            string dealercode, string projid)
        {
            lblLeadId.Text = leadId;
            txtName.Text = leadName;
            txtNRIC.Text = leadNRIC;
            txtMobileNo.Text = mobileNo;
            txtHomeNo.Text = homeno;
            txtContent.Text = content;
            txtEvent.Text = levent;
            lblProjectID.Text = projid;

            if (ddlTeam.Items.FindByValue(dealercode) != null)
                ddlTeam.SelectedValue = dealercode;

            if (sex.Trim().StartsWith("F"))
                rbtnFemale.Checked = true;
            else
                rbtnMale.Checked = true;

            txtEvent.Text = levent;

            if (String.IsNullOrEmpty(prefermode))
                ddlPreferredMode.SelectedIndex = 0;
            else if (ddlPreferredMode.Items.FindByValue(prefermode) != null)
                ddlPreferredMode.SelectedValue = prefermode;

            txtEmail.Text = email;
            if (needfollowup.Equals("Y"))
            {
                chkFollowUp.Checked = true;
                panelFollowupPanel.Enabled = true;
            }
            else
            {
                chkFollowUp.Checked = false;
                panelFollowupPanel.Enabled = false;
            }

            calFollowupDate.DateTextFromValue = followupdate;

        }

        private void retrieveContactHistory(string leadId)
        {
            tdContactHistory.BgColor = "#6E6A6B";

            tdEntryHistory.BgColor = "";

            leadsContactService = new LeadsContactService(base.dbConnectionStr);
            DataSet ds = leadsContactService.getContactHistoryByLeadId(leadId);

            if (ds.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString().Equals("1"))
            {
                ViewState["ContactType"] = "History";

                ViewState["dtContactHistory"] = ds.Tables[0];
                gvContactHistory.DataSource = ds.Tables[0];
                gvContactHistory.DataBind();

                if (ds.Tables[0].Rows.Count < 1)
                {
                    gvFollowUp.Visible = false;
                }

                divCHPaging.Visible = true;
                DisplayContactHistoryPaging();
            }
            else
            {
                divCHPaging.Visible = false;

                gvContactHistory.DataSource = null;
                gvContactHistory.DataBind();

                gvFollowUp.Visible = false;

            }

            //divMessageTwo.InnerHtml = ds.Tables["ReturnTable"].Rows[0]["ReturnMessage"].ToString();
        }

        private void RefreshContactHistory()
        {
            DataTable dtContactHistory = ViewState["dtContactHistory"] as DataTable;
            DataRow drNewContact = null;
            string sex = "M";

            if (rbtnFemale.Checked)
                sex = "F";

            if ((dtContactHistory != null) && (dtContactHistory.Rows[0]["LeadId"].ToString().Equals(lblLeadId.Text.Trim())))
            {
                drNewContact = dtContactHistory.NewRow();
                drNewContact["ContactDate"] = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");

                //Need To return from WebService after successfully inserted into ClientContact table. And it also need to return from WebService
                //whether inserted Contact is Extra called or not.
                drNewContact["AEGroup"] = dtContactHistory.Rows[0]["AEGroup"].ToString();
                drNewContact["LeadId"] = lblLeadId.Text.Trim();
                drNewContact["LeadName"] = txtName.Text;
                drNewContact["Sex"] = sex;
                drNewContact["MobileNo"] = txtMobileNo.Text.Trim();
                drNewContact["HomeNo"] = txtHomeNo.Text.Trim();
                drNewContact["Content"] = txtContent.Text.Trim();
                drNewContact["PreferMode"] = ddlPreferredMode.SelectedItem.Text.Trim();
                drNewContact["NeedFollowUp"] = chkFollowUp.Checked;//ddlPreferredMode.SelectedItem.Text.Trim();
                if (chkFollowUp.Checked)
                {
                    drNewContact["FollowUpDate"] = calFollowupDate.DateTextFromValue;//Convert.ToDateTime(calFollowupDate.DateTextFromValue);//ddlPreferredMode.SelectedItem.Text.Trim();
                }
                else
                {
                    drNewContact["FollowUpDate"] = null;
                }
                //drNewContact["PreferA"] = ddlPreferenceOne.SelectedValue.Trim();
                //drNewContact["PreferB"] = ddlPreferenceTwo.SelectedValue.Trim();
                //drNewContact["Rank"] = ddlRank.SelectedValue;
                drNewContact["ModifiedUser"] = hdfDealerCode.Value;
                dtContactHistory.Rows.Add(drNewContact);
            }
            else if (dtContactHistory == null)
            {
                dtContactHistory = new DataTable();

                dtContactHistory.Columns.Add("ContactDate", String.Empty.GetType());
                dtContactHistory.Columns.Add("AEGroup", String.Empty.GetType());
                dtContactHistory.Columns.Add("LeadId", String.Empty.GetType());
                dtContactHistory.Columns.Add("LeadName", String.Empty.GetType());
                dtContactHistory.Columns.Add("Sex", String.Empty.GetType());
                dtContactHistory.Columns.Add("MoblieNo", String.Empty.GetType());
                dtContactHistory.Columns.Add("HomeNo", String.Empty.GetType());
                dtContactHistory.Columns.Add("Content", String.Empty.GetType());
                dtContactHistory.Columns.Add("PreferMode", String.Empty.GetType());
                dtContactHistory.Columns.Add("NeedFollowUp", String.Empty.GetType());
                dtContactHistory.Columns.Add("FollowupDate", String.Empty.GetType());
                //dtContactHistory.Columns.Add("Rank", String.Empty.GetType());
                dtContactHistory.Columns.Add("ModifiedUser", String.Empty.GetType());
            }

            ViewState["dtContactHistory"] = dtContactHistory;
            gvContactHistory.DataSource = dtContactHistory;
            gvContactHistory.DataBind();

            if (dtContactHistory.Rows.Count < 1)
            {
                gvFollowUp.Visible = false;
            }

            divCHPaging.Visible = true;
            DisplayContactHistoryPaging();
        }

        private string ValidateContactEntryForm()
        {
            string result = "ok", userRole = "user";

            if (String.IsNullOrEmpty(txtName.Text.Trim()))
            {
                result = "Leads Name cannot be blank!";
            }
            else if (String.IsNullOrEmpty(txtMobileNo.Text.Trim()))
            {
                result = "Mobile Number cannot be blank!";
            }
            else if (String.IsNullOrEmpty(txtContent.Text.Trim()))
            {
                result = "Content cannot be blank!";
            }

            /*
            else if ((!rbtnMale.Checked) && (!rbtnFemale.Checked))
            {
                result = "Please select Client's Sex!";
            }
            */
            else if (ViewState["UserRole"] != null)
            {
                userRole = ViewState["UserRole"] as string;
                if ((userRole == "admin") && (ddlActualDealer.SelectedValue == ""))
                {
                    result = "Please select Dealer!";
                }
            }

            if (chkFollowUp.Checked)
            {
                if (calFollowupDate.DateTextFromValue == "")
                {
                    result = "Please select Follow Up Date!";
                }
            }

            return result;
        }

        private void clearContactEntryForm()
        {
            lblLeadId.Text = "";

            txtName.Text = "";
            txtNRIC.Text = "";
            txtMobileNo.Text = "";
            txtHomeNo.Text = "";
            txtContent.Text = "";
            txtEvent.Text = "";
            txtEmail.Text = "";

            ddlPreferredMode.SelectedIndex = 0;
            ddlTeam.SelectedIndex = 0;

            rbtnFemale.Checked = false;
            rbtnMale.Checked = false;

        }

        protected void chkFollowUp_CheckedChanged(object sender, EventArgs e)
        {
            if (chkFollowUp.Checked)
            {
                panelFollowupPanel.Enabled = true;
            }
            else
            {
                panelFollowupPanel.Enabled = false;
                calFollowupDate.DateTextFromValue = null;
            }
        }

        protected string getNewID(string prevID)
        {
            int lenNumbers = 4;
            string prefix = "L";
            string num = "";

            //prevID = cmc.GetMaxID(0);
            if (prevID == "0")
            {
                num = "0";
            }
            else
            {
                num = prevID.Substring(prevID.Length - lenNumbers);
            }

            num = padZeros(Convert.ToString(Convert.ToInt32(num) + 1), lenNumbers);
            string newID = prefix + num;

            return newID;
        }

        //filling with zero
        public static string padZeros(string stringToPad, Int32 desiredLength)
        {
            string pZ = stringToPad;
            if (stringToPad.Length < desiredLength)
            {
                for (int i = stringToPad.Length + 1; i <= desiredLength; i++)
                    pZ = "0" + pZ;
            }
            return pZ;
        }

        protected void btnSync_Click(object sender, EventArgs e)
        {
            //panelSync.Visible = true;
            //Response.Redirect("SyncForm.aspx");
            //divPopUp.Visible = true;           
            //divPopUp.Style.Add("POSITION", "absolute");
            //divPopUp.Style.Add("LEFT", "10px");
            //divPopUp.Style.Add("TOP", "10px");
            //divPopUp.Style.Add("COLOR", "black");
            //divPopUp.Style.Add("background-color", "#FFCC66");

            panelSync.Visible = true;

            panelSync.Style.Add("POSITION", "absolute");
            panelSync.Style.Add("LEFT", "20px");
            panelSync.Style.Add("TOP", "50px");
            panelSync.Style.Add("COLOR", "black");
            panelSync.Style.Add("background-color", "#FFCC66");

            if (rdoAccNo.Checked)
            {
                txtSyncAccNo.Enabled = true;
                txtSyncNRIC.Enabled = false;
            }
            else if (rdoNRIC.Checked)
            {
                txtSyncAccNo.Enabled = false;
                txtSyncNRIC.Enabled = true;
            }
        }

        //======= for POP UP ============

        protected void btnSyncSubmit_Click(object sender, EventArgs e)
        {
            //Validate Sync Entry Form
            string validationResult = ValidateSyncForm();

            if (validationResult == "ok")
            {
                if (rdoNRIC.Checked)
                {
                    MoveToLeadArchiveByNRIC();
                    SyncByNRIC();
                }
                else if (rdoAccNo.Checked)
                {
                    MoveToLeadArchiveByAccNo();
                    SyncByAccNo();
                }
            }
            else
            {
                divSyncMessage.InnerHtml = validationResult;

            }
        }

        protected void rdo_CheckedChanged(object sender, EventArgs e)
        {

            txtSyncNRIC.Enabled = ((RadioButton)sender).ID.Equals("rdoNRIC");
            txtSyncAccNo.Enabled = ((RadioButton)sender).ID.Equals("rdoAccNo");

        }

        private void MoveToLeadArchiveByAccNo()
        {
            string[] wsReturn = null;

            leadsService = new LeadsService(base.dbConnectionStr);

            wsReturn = leadsService.MoveToLeadArchive("AccNo", txtSyncAccNo.Text.Trim());

            if (wsReturn[0] == "1")
            {
                divSyncMessage.InnerHtml = "oving to Lead Archive is successfully done!";
            }
            else if (wsReturn[0] == "0")
            {
                divSyncMessage.InnerHtml = wsReturn[1];
            }
            else
            {
                divSyncMessage.InnerHtml = "Error in Moving to Lead Archive!";
                return;
            }
        }

        private void MoveToLeadArchiveByNRIC()
        {
            string[] wsReturn = null;

            leadsService = new LeadsService(base.dbConnectionStr);

            wsReturn = leadsService.MoveToLeadArchive("NRIC", txtSyncNRIC.Text.Trim());

            if (wsReturn[0] == "1")
            {
                divSyncMessage.InnerHtml = "Moving to Lead Archive is successfully done!";
            }
            else if (wsReturn[0] == "0")
            {
                divSyncMessage.InnerHtml = wsReturn[1];
            }
            else
            {
                divSyncMessage.InnerHtml = "Error in Moving to Lead Archive!";
                return;
            }
        }

        private void SyncByNRIC()
        {
            string[] wsReturn = null;
            //DataSet ds = null;

            leadsService = new LeadsService(base.dbConnectionStr);

            wsReturn = leadsService.LeadsDataSync("NRIC", txtSyncNRIC.Text.Trim());

            if (wsReturn[0] == "1")
            {
                divSyncMessage.InnerHtml = "Leads Synchronisation is successfully done!";
            }
            else if (wsReturn[0] == "0")
            {
                divSyncMessage.InnerHtml = wsReturn[1];
            }
            else
            {
                divSyncMessage.InnerHtml = "Error in Leads Synchronisation!"; return;
            }
        }

        private void SyncByAccNo()
        {
            string[] wsReturn = null;
            //DataSet ds = null;

            leadsService = new LeadsService(base.dbConnectionStr);

            wsReturn = leadsService.LeadsDataSync("AccNo", txtSyncAccNo.Text.Trim());

            if (wsReturn[0] == "1")
            {
                divMessage.InnerHtml = "Leads Synchronisation is successfully done!"; return;
            }
            else if (wsReturn[0] == "0")
            {
                divMessage.InnerHtml = "No Match Data For Leads Synchronisation! Please try again."; return;
            }
            else
            {
                divMessage.InnerHtml = "Error in Leads Synchronisation!"; return;
            }
        }

        protected void btnSyncCancel_Click(object sender, EventArgs e)
        {
            txtSyncAccNo.Text = "";
            txtSyncNRIC.Text = "";

            panelSync.Visible = false;
        }

        private string ValidateSyncForm()
        {
            string result = "ok";

            if (rdoAccNo.Checked)
            {
                if (String.IsNullOrEmpty(txtSyncAccNo.Text.Trim()))
                {
                    result = "Account No cannot be blank!";
                }
            }
            else if (rdoNRIC.Checked)
            {
                if (String.IsNullOrEmpty(txtSyncNRIC.Text.Trim()))
                {
                    result = "NRIC/Passport Number cannot be blank!";
                }
            }

            return result;
        }

        protected void gvFollowUp_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if ((e.CommandName == "Show"))// || (e.CommandName == "ClientName"))
            {
                hdfStatus.Value = "F";
                chkFollowUp.Checked = false;
                chkFollowUp.Enabled = false;
                panelFollowupPanel.Enabled = false;

                int index = Convert.ToInt32(e.CommandArgument);
                DataTable dtFollowUp = (DataTable)ViewState["dtFollowUp"];

                hdfRecId.Value = dtFollowUp.Rows[index]["RecId"].ToString();

                //if (e.CommandName == "Show")
                //{
                string sex = "";

                /// <Updated by OC [From "DealerCode" to "AECode" And "LeadHomeNo" to "LeadHome"]>
                if (String.IsNullOrEmpty(dtFollowUp.Rows[index]["AECode"].ToString()))
                    ddlTeam.SelectedIndex = 0;
                else if (ddlTeam.Items.FindByValue(dtFollowUp.Rows[index]["AECode"].ToString().Trim()) != null)
                    ddlTeam.SelectedValue = dtFollowUp.Rows[index]["AECode"].ToString().Trim();

                lblLeadId.Text = dtFollowUp.Rows[index]["LeadId"].ToString();
                txtName.Text = dtFollowUp.Rows[index]["LeadName"].ToString();
                txtMobileNo.Text = dtFollowUp.Rows[index]["LeadMobile"].ToString();
                txtHomeNo.Text = dtFollowUp.Rows[index]["LeadHome"].ToString();
                txtEvent.Text = dtFollowUp.Rows[index]["Event"].ToString();
                txtEmail.Text = dtFollowUp.Rows[index]["LeadEmail"].ToString();
                txtContent.Text = "It is Follow Up";

                sex = dtFollowUp.Rows[index]["Sex"].ToString();

                rbtnMale.Checked = rbtnFemale.Checked = false;
                if (sex == "F")
                    rbtnFemale.Checked = true;
                else if (sex == "M")
                    rbtnMale.Checked = true;

                if (String.IsNullOrEmpty(dtFollowUp.Rows[index]["PreferMode"].ToString()))
                    ddlPreferredMode.SelectedIndex = 0;
                else if (ddlPreferredMode.Items.FindByValue(dtFollowUp.Rows[index]["PreferMode"].ToString().Trim()) != null)
                    ddlPreferredMode.SelectedValue = dtFollowUp.Rows[index]["PreferMode"].ToString().Trim();

                lblProjectID.Text = dtFollowUp.Rows[index]["ProjectID"].ToString();


                //}

                //Retrieve contact history.(it will retrieve when user click LeadId (or) Lead name)
                //if (!String.IsNullOrEmpty(dtFollowUp.Rows[index]["LeadId"].ToString()))
                //{
                //    contactType = "History";
                //    ViewState["ContactType"] = "History";
                //    lbtnContactHistory.Enabled = true;
                //    this.retrieveContactHistory(dtFollowUp.Rows[index]["LeadId"].ToString());
                //}
            }
        }

        protected void gvFollowUp_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataTable dtFollowUp = ViewState["dtFollowUp"] as DataTable;
            DataTable sortedDataTable = null;

            string sortDirection = "", sortString = "";

            sortDirection = CommonUtilities.GetSortDirection(e.SortExpression, ViewState["SortExpression"] as string, ViewState["SortDirection"] as string);

            ViewState["SortDirection"] = sortDirection;
            ViewState["SortExpression"] = e.SortExpression;
            sortString = e.SortExpression + " " + sortDirection;
            sortedDataTable = CommonUtilities.SortDataTable(dtFollowUp, sortString);

            ViewState["dtFollowUp"] = sortedDataTable;
            gvFollowUp.PageIndex = 0;
            gvFollowUp.DataSource = sortedDataTable;
            gvFollowUp.DataBind();
            DisplayAssignmentsPaging();

            dtFollowUp.Dispose();
        }

        protected void gvFollowUp_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string cutOffDtStr = "";
                string contactDtStr = "";

                /*
                if (DataBinder.Eval(e.Row.DataItem, "CutOffDate") != null)
                {
                    cutOffDtStr = DataBinder.Eval(e.Row.DataItem, "CutOffDate").ToString();
                    if (!String.IsNullOrEmpty(cutOffDtStr))
                    {
                        e.Row.Cells[12].Text = DateTime.ParseExact(cutOffDtStr, "yyyy-MM-dd HH:mm:ss", null).ToString("dd/MM/yyyy HH:mm:ss");                      
                    }
                }
                */

                //if (DataBinder.Eval(e.Row.DataItem, "FollowUpDate") != null)
                //{
                //    contactDtStr = DataBinder.Eval(e.Row.DataItem, "FollowUpDate").ToString();
                //    if (!String.IsNullOrEmpty(contactDtStr))
                //    {
                //        e.Row.Cells[10].Text = DateTime.ParseExact(contactDtStr, "yyyy-MM-dd HH:mm:ss", null).ToString("dd/MM/yyyy HH:mm:ss");
                //    }
                //}
            }
        }

        //===============================
        //protected void gvContactHistory_RowDeleting(object sender, GridViewDeleteEventArgs e)
        //{
        //    Hashtable ht = DeleteRecord(e.RowIndex);
        //    DataTable dtLeads = null;

        //    if (int.Parse(ht["ReturnCode"].ToString()) > 0)
        //    {
        //        dtLeads = (DataTable)ht["ReturnData"];
        //    }

        //    gvContactHistory.DataSource = dtLeads;
        //    gvContactHistory.DataBind();
        //    divMessage.InnerHtml = ht["ReturnMessage"].ToString();
        //}

        protected DataTable gvFollowUp_DataBind(String AECode)
        {
            leadsContactService = new LeadsContactService(base.dbConnectionStr);
            return leadsContactService.RetrieveFollowUpLead(AECode);
        }
    }
}