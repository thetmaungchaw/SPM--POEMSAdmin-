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
using SPMWebApp.BasePage;
using SPMWebApp.Services;
using SPMWebApp.Utilities;

namespace SPMWebApp.WebPages.ContactManagement
{
    public partial class ContactEntry : BasePage.BasePage
    {
        private ClientContactService clientContactService;
        private ClientAssignmentService clientAssignmentService;
        private CommonUtilities commonUtil = new CommonUtilities();
        private string contactType = "";
        private string lastDeleteAccNoTrack = "";

        /// <summary>
        /// Fetch User Access Right
        /// Call to ValidateUserAccessRight method with FunctionCode
        /// </summary>        
        private void LoadUserAccessRight(string PageURL)
        {
            List<AccessRight> accessRightList;
            if (AccessRightUtilities.ValidateUserAccessRight((DataTable)Session["UserAccessRights"], PageURL, out accessRightList))
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
                // btnAddContact.Enabled = (bool)ViewState["CreateAccessRight"];
                //btnSync.Enabled = (bool)ViewState["CreateAccessRight"];
            }
            catch (Exception e) { }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                /// <Added by OC>
                ddlActualDealer.Enabled = false;
                ddlProjectList.Enabled = true;

                //ddlActualDealer.Enabled = true;
                //ddlProjectList.Enabled = false;
                CommonService CommonService = new CommonService(base.dbConnectionStr);
                ViewState["dsDealerDetail"] = CommonService.RetrieveDealerCodeAndNameByUserID(Session["UserId"].ToString());
                txtAccountNo.Attributes.Add("onKeyPress", "doClick('" + btnSearch.ClientID + "',event)");


                //Add JavaScript Event for Short Key Textbox blur event
                this.txtKey.Attributes.Add("onblur", "javascript:handleShortKeyBlur()");
                lblSeminarRegistration.Visible = false;

                //For Contact Entry Admin
                if (String.IsNullOrEmpty(Request.Params["type"]))
                {
                    LoadUserAccessRight("ContactEntry");
                    divAdminEntry.Visible = false;
                    divAdminEntry2.Visible = false;
                    ViewState["UserRole"] = "user";
                }
                else
                {
                    LoadUserAccessRight("ContactEntryAdmin");
                    divTitle.InnerHtml = "Contact Entry Admin";
                    divAdminEntry.Visible = true;
                    divAdminEntry2.Visible = true;
                    ViewState["UserRole"] = "admin";
                }

                checkAccessRight();

                //Fill Rank DropdownList
                CommonService commonService = new CommonService();
                string[,] clientRanks = commonService.RetrieveClientRanks();
                CommonUtilities.BindDataToDropDrownList(ddlRank, clientRanks, 0, 1, null);
                PrepareDealerList();
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
                btnUpdateContact.Visible = false;
                btnAddContact.Visible = true;
                btnCancel.Visible = false;
            }
            if (chkDealer.Checked)
            {
                ddlFollowUpDealer.Enabled = true;
                lblDate.Visible = true;
                calFollowUpDate.Visible = true;
            }
            else if (chkDealer.Checked == false)
            {
                ddlFollowUpDealer.Enabled = false;
                lblDate.Visible = false;
                calFollowUpDate.Visible = false;
            }
            base.hdfModifyIndex = this.hdfModifyIndex;
            base.gvList = gvContactHistory;
            base.divMessage = this.divMessage;
            divMessageTwo.InnerText = "";
            divMessage.InnerHtml = "";
            //This is button are set to true in DisplayDetails Method.

        }

        private void InitializeRowPerPageSetting()
        {
            //Setting for RowPerPage for Custom Paging Control
            pgcAssignment.StartRowPerPage = 10;
            pgcAssignment.RowPerPageIncrement = 10;
            pgcAssignment.EndRowPerPage = 100;

            pgcContactFollowUp.StartRowPerPage = 10;
            pgcContactFollowUp.RowPerPageIncrement = 10;
            pgcContactFollowUp.EndRowPerPage = 100;
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
            DataTable dtClientContact = GetClientContact();

            if (dtClientContact != null)
            {
                gvContactHistory.PageSize = e.Value;
                gvContactHistory.PageIndex = 0;
                gvContactHistory.DataSource = dtClientContact;
                gvContactHistory.DataBind();

                ViewState["ContactHistoryRowPerPage"] = e.Value;
                DisplayContactHistoryPaging();
            }
        }

        protected void ContactHistory_PageNoChange(object sender, EventArgs<Int32> e)
        {
            gvContactHistory.PageIndex = e.Value - 1;
            //DataTable dtContactHistory = ViewState["dtContactHistory"] as DataTable;

            DataTable dtClientContact = GetClientContact();

            if (dtClientContact != null)
            {
                gvContactHistory.DataSource = dtClientContact;
                gvContactHistory.DataBind();
            }
        }

        private DataTable GetClientContact()
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

            clientContactService = new ClientContactService(base.dbConnectionStr);
            ViewState["dtProjectInfo"] = null;
            DataSet ds = clientContactService.PrepareForContactEntry(userRole, base.userLoginId);
            returnCode = int.Parse(ds.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString());
            if (returnCode == 1)
            {

                // dtProjectInfo Fill Project Information List
                if (userRole == "admin")
                {
                    ddlProjectList.DataSource = ds.Tables["dtProjectInfo"];
                    ddlProjectList.DataTextField = "ProjectName";
                    ddlProjectList.DataValueField = "ProjectID";
                    ddlProjectList.DataBind();
                    // ddlProjectList.SelectedIndex = 0;
                    // ddlProjectList.SelectedValue = "--Please Select Project--";
                    if (ds.Tables["dtProjectInfo"].Rows.Count > 0)
                    {
                        ViewState["dtProjectInfo"] = ds.Tables["dtProjectInfo"];
                        lblObjective.Text = ds.Tables["dtProjectInfo"].Rows[0]["ProjectObjective"].ToString();
                        divAssignPaging.Visible = false;

                        #region Added by Thet Maung Chaw (Copied from below)

                        //Set OptionNo as Primary Key to use in Client Preferences searching.
                        DataColumn[] dPk = new DataColumn[1];
                        dPk[0] = ds.Tables["dtPreference"].Columns["OptionNo"];
                        ds.Tables["dtPreference"].PrimaryKey = dPk;
                        ViewState["dtPreferList"] = ds.Tables["dtPreference"];
                        if (ds.Tables[1].Rows.Count > 1)
                            ddlPreferenceTwo.SelectedIndex = 1;
                        else
                            ddlPreferenceTwo.SelectedIndex = 0;

                        #endregion

                        //Clear Previous Dealer Contact History
                        ViewState["dtContactHistory"] = null;
                        RetrieveUnContactedAssignmentForByProjectID(ds.Tables["dtProjectInfo"].Rows[0]["ProjectID"].ToString());
                    }
                }
                /* Fill User Preference List */
                ddlPreferenceOne.DataSource = ds.Tables["dtPreference"];             //Table 1 is PreferenceList
                ddlPreferenceOne.DataTextField = "OptionDisplay";
                ddlPreferenceOne.DataValueField = "OptionNo";
                ddlPreferenceOne.DataBind();
                ddlPreferenceOne.SelectedIndex = 0;

                ddlPreferenceTwo.DataSource = ds.Tables["dtPreference"];
                ddlPreferenceTwo.DataTextField = "OptionDisplay";
                ddlPreferenceTwo.DataValueField = "OptionNo";
                ddlPreferenceTwo.DataBind();

                //Set OptionNo as Primary Key to use in Client Preferences searching.
                DataColumn[] dcPk = new DataColumn[1];
                dcPk[0] = ds.Tables["dtPreference"].Columns["OptionNo"];
                ds.Tables["dtPreference"].PrimaryKey = dcPk;
                ViewState["dtPreferList"] = ds.Tables["dtPreference"];
                if (ds.Tables[1].Rows.Count > 1)
                    ddlPreferenceTwo.SelectedIndex = 1;
                else
                    ddlPreferenceTwo.SelectedIndex = 0;

                if (userRole == "user")
                {
                    hdfDealerCode.Value = ds.Tables[0].Rows[0]["AECode"].ToString();
                    hdfTeamCode.Value = ds.Tables[0].Rows[0]["AEGroup"].ToString();


                    ViewState["dtAssignment"] = ds.Tables[2];       //Table Index 2 is Uncontacted Assignments
                    gvAssignments.DataSource = ds.Tables[2];
                    gvAssignments.DataBind();

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
                        //tdEntryHistory.BgColor = "";
                        ViewState["dtEntryHistory"] = ds.Tables["dtEntryHistory"];
                        gvContactHistory.DataSource = ds.Tables["dtEntryHistory"];
                        gvContactHistory.DataBind();

                        divCHPaging.Visible = true;
                        DisplayContactHistoryPaging();
                        // CommonUtilities.BindDataToDropDrownListByFilter(ddlFollowUpByDealer, base.userLoginId, ds.Tables[0], "DisplayName", "UserID", "----- Select Dealer -----");

                    }
                }
                else
                {
                    divAssignPaging.Visible = false;
                    divCHPaging.Visible = false;

                    ViewState["hdfDealerTable"] = null;
                    ViewState["hdfDealerTable"] = ds.Tables[0];
                    CommonUtilities.BindDataToDropDrownListByFilter(ddlActualDealer, base.userLoginId, ds.Tables[0], "DisplayName", "AECode", "----- Select Dealer -----");
                }
            }

            if (userRole == "user")
            {
                divCurAssignmentText.Visible = true;
                DataSet dsContactToFollowUp = clientContactService.PrepareForContactToFollowUp(base.userLoginId, "User");
                if (dsContactToFollowUp.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString().Equals("1"))
                {
                    ViewState["dtContactToFollowUp"] = dsContactToFollowUp.Tables[0];
                    gvContactFollowUp.DataSource = dsContactToFollowUp.Tables[0];
                    gvContactFollowUp.DataBind();
                    if (dsContactToFollowUp.Tables[0].Rows.Count > 0)
                    {
                        divFollowUpPaging.Visible = true;
                        DisplayFollowUpPaging();
                    }
                    else
                    {
                        divFollowUpPaging.Visible = false;
                    }
                }
                else if (dsContactToFollowUp.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString().Equals("0"))
                {
                    /// <Added by OC>
                    gvContactFollowUp.DataSource = null;
                    gvContactFollowUp.DataBind();

                    divFollowUpPaging.Visible = false;
                }
                else
                {
                    divMessage.InnerHtml = dsContactToFollowUp.Tables["ReturnTable"].Rows[0]["ReturnMessage"].ToString();
                }
            }
            else
            {
                divCurAssignmentText.Visible = false;
                divFollowUpPaging.Visible = false;
            }
        }

        protected void btnAddContact_Click(object sender, EventArgs e)
        {
            String followUpRecId = hdfFollowUpRecordId.Value;

            string dealerCode = "", userId = "", adminId = "", userRole = "user", sex = "", keep = "Y", validationResult = "",
                            lastAcctNo = "", clientName = "";
            string followUpDealer = "";
            string followUpDate = "";
            DataSet ds = null;
            DataTable dtContactHistory = ViewState["dtContactHistory"] as DataTable;
            DataRow drNewContact = null;
            clientContactService = new ClientContactService(base.dbConnectionStr);

            //Validate Contact Entry Form
            validationResult = ValidateContactEntryForm();

            if (validationResult == "ok")
            {
                if (rbtnFemale.Checked)
                    sex = "F";
                else if (rbtnMale.Checked)
                    sex = "M";


                if (ViewState["UserRole"] != null)
                {
                    userRole = ViewState["UserRole"] as string;
                }

                if (userRole == "admin")
                {
                    /// <Original.  Commented and updated by Thet Maung Chaw>
                    //dealerCode = ddlActualDealer.SelectedItem.Text.Split('-')[1].Trim();
                    dealerCode = hdfDealerCode.Value;


                    userId = ddlActualDealer.SelectedValue;
                    adminId = base.userLoginId;
                }
                else
                {
                    dealerCode = hdfDealerCode.Value;
                    userId = base.userLoginId;
                    adminId = "";
                }

                if ((dtContactHistory != null) && (dtContactHistory.Rows[0]["AcctNo"].ToString().Equals(txtAccountNo.Text.Trim())))
                {
                    lastAcctNo = txtAccountNo.Text.Trim();
                    clientName = lblAccountName.Text;
                }

                //if (!String.IsNullOrEmpty(lastDeleteAccNoTrack))
                //{
                //    if (lastDeleteAccNoTrack == txtAccountNo.Text.Trim())
                //    {
                //        lastAcctNo = "";
                //    }
                //}

                if (chkDealer.Checked)
                {
                    followUpDealer = ddlFollowUpDealer.SelectedValue;
                    followUpDate = calFollowUpDate.DateTextFromValue;
                }

                int followUpStatus = chkDealer.Checked ? 1 : 0;

                //comment txtCourse.Text.Trim() and replace with "" 
                if (String.IsNullOrEmpty(followUpRecId))
                {
                    ds = clientContactService.InsertClientContact(dealerCode, userId, txtAccountNo.Text.Trim(), txtKey.Text.Trim(),
                        sex, txtPhone.Text.Trim(), txtContact.Text.Trim(), ddlPreferenceOne.SelectedValue.Trim(), ddlPreferenceTwo.SelectedValue.Trim(),
                        "", ddlRank.SelectedValue, keep, adminId, lastAcctNo, clientName, followUpStatus, followUpDealer, followUpDate, lblProjectID.Value);
                }
                else
                {

                    followUpStatus = 2;
                    //Project ID is null for Follow Up Contact
                    ds = clientContactService.InsertClientContact(dealerCode, userId, txtAccountNo.Text.Trim(), txtKey.Text.Trim(),
                        sex, txtPhone.Text.Trim(), txtContact.Text.Trim(), ddlPreferenceOne.SelectedValue.Trim(), ddlPreferenceTwo.SelectedValue.Trim(),
                        "", ddlRank.SelectedValue, keep, adminId, lastAcctNo, clientName, followUpStatus, ddlFollowUpDealer.SelectedValue, followUpDate, lblProjectID.Value);

                    //Update Previous Record's Follow-Up Status to 'Y'
                    /// <Commented by Thet Maung Chaw.  Only "clientContactService.UpdateClientContact" method>
                    String[] result = clientContactService.UpdateClientContact(dealerCode, userId, txtAccountNo.Text.Trim(), txtKey.Text.Trim(),
                                                sex, txtPhone.Text.Trim(), txtContact.Text.Trim(), ddlPreferenceOne.SelectedValue.Trim(), ddlPreferenceTwo.SelectedValue.Trim(),
                                                "", ddlRank.SelectedValue, keep, adminId, hdfFollowUpRecordId.Value, followUpStatus, ddlFollowUpDealer.SelectedValue, followUpDate, lblProjectID.Value);
                    
                    //String[] result = clientContactService.UpdateClientContactFollowUpStatus(followUpRecId, const_FollowUp);
                }


                validationResult = ds.Tables["ReturnTable"].Rows[0]["ReturnMessage"].ToString();

                if (ds.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString() == "1")
                {
                    //Refresh on Uncontacted Assignments
                    DataTable dtAssignment = (DataTable)ViewState["dtAssignment"];
                    if (dtAssignment != null)
                    {
                        DataRow[] drDelete = dtAssignment.Select(" AcctNo = '" + txtAccountNo.Text.Trim() + "'");

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


                    if (!String.IsNullOrEmpty(lastAcctNo))
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
                        drNewContact["AcctNo"] = ds.Tables["dtContactHistory"].Rows[0]["AcctNo"].ToString();
                        drNewContact["CName"] = ds.Tables["dtContactHistory"].Rows[0]["CName"].ToString();
                        drNewContact["Sex"] = ds.Tables["dtContactHistory"].Rows[0]["Sex"].ToString();
                        drNewContact["Phone"] = ds.Tables["dtContactHistory"].Rows[0]["Phone"].ToString();
                        drNewContact["Content"] = ds.Tables["dtContactHistory"].Rows[0]["Content"].ToString();

                        //drNewContact["SeminarName"] = ds.Tables["dtContactHistory"].Rows[0]["SeminarName"].ToString();
                        drNewContact["FollowUpBy"] = ds.Tables["dtContactHistory"].Rows[0]["FollowUpBy"].ToString();
                        drNewContact["FollowUpStatus"] = ds.Tables["dtContactHistory"].Rows[0]["FollowUpStatus"].ToString();
                        drNewContact["FollowUpDate"] = ds.Tables["dtContactHistory"].Rows[0]["FollowUpDate"].ToString();

                        drNewContact["Remarks"] = ds.Tables["dtContactHistory"].Rows[0]["Remarks"].ToString();
                        drNewContact["PreferA"] = ds.Tables["dtContactHistory"].Rows[0]["PreferA"].ToString();
                        drNewContact["PreferB"] = ds.Tables["dtContactHistory"].Rows[0]["PreferB"].ToString();
                        drNewContact["Rank"] = ds.Tables["dtContactHistory"].Rows[0]["Rank"].ToString();
                        drNewContact["ModifiedUser"] = ds.Tables["dtContactHistory"].Rows[0]["ModifiedUser"].ToString();
                        drNewContact["RankText"] = ds.Tables["dtContactHistory"].Rows[0]["RankText"].ToString();
                        dtContactHistory.Rows.InsertAt(drNewContact, 0);
                    }
                    else
                    {
                        dtContactHistory = null;
                        dtContactHistory = ds.Tables["dtContactHistory"];
                    }

                    InsertEntryHistory(dtContactHistory.Rows[0]);

                    //tdContactHistory.BgColor = "#6E6A6B";
                    //tdEntryHistory.BgColor = "";

                    ViewState["ContactType"] = "History";
                    ViewState["dtContactHistory"] = dtContactHistory;
                    gvContactHistory.DataSource = dtContactHistory;
                    gvContactHistory.DataBind();

                    divCHPaging.Visible = true;
                    DisplayContactHistoryPaging();

                    if (chkDealer.Checked)
                    {
                        SentFollowUpEmailToDealer(followUpDealer, txtAccountNo.Text.Trim());
                    }
                    clearContactEntryForm();

                }

                #region Added by Thet Maung Chaw (Refresh Follow Up Grid)

                if (userRole == "user")
                {
                    divCurAssignmentText.Visible = true;
                    DataSet dsContactToFollowUp = clientContactService.PrepareForContactToFollowUp(base.userLoginId, "User");
                    if (dsContactToFollowUp.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString().Equals("1"))
                    {
                        ViewState["dtContactToFollowUp"] = dsContactToFollowUp.Tables[0];
                        gvContactFollowUp.DataSource = dsContactToFollowUp.Tables[0];
                        gvContactFollowUp.DataBind();
                        if (dsContactToFollowUp.Tables[0].Rows.Count > 0)
                        {
                            divFollowUpPaging.Visible = true;
                            DisplayFollowUpPaging();
                        }
                        else
                        {
                            divFollowUpPaging.Visible = false;
                        }
                    }
                    else if (dsContactToFollowUp.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString().Equals("0"))
                    {
                        /// <Added by OC>
                        gvContactFollowUp.DataSource = null;
                        gvContactFollowUp.DataBind();

                        divFollowUpPaging.Visible = false;
                    }
                    else
                    {
                        divMessage.InnerHtml = dsContactToFollowUp.Tables["ReturnTable"].Rows[0]["ReturnMessage"].ToString();
                    }
                }
                else
                {
                    divCurAssignmentText.Visible = false;
                    divFollowUpPaging.Visible = false;
                }

                #endregion
            }

            divMessage.InnerHtml = validationResult;
            divMessageTwo.InnerHtml = "";
        }

        private void SentFollowUpEmailToDealer(string dealerCode, string accNo)
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

        protected void gvAssignments_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if ((e.CommandName == "AcctNo") || (e.CommandName == "ClientName"))
            {
                int index = Convert.ToInt32(e.CommandArgument);
                DataTable dtAssignment = (DataTable)ViewState["dtAssignment"];

                if (e.CommandName == "AcctNo")
                {
                    clearContactEntryForm();
                    string sex = "";

                    hdfDealerCode.Value = dtAssignment.Rows[index]["AECode"].ToString();

                    lblAccountName.Text = dtAssignment.Rows[index]["ClientName"].ToString();
                    txtAccountNo.Text = dtAssignment.Rows[index]["AcctNo"].ToString();
                    txtKey.Text = dtAssignment.Rows[index]["ShortKey"].ToString();
                    txtPhone.Text = dtAssignment.Rows[index]["Phone"].ToString();
                    sex = dtAssignment.Rows[index]["Sex"].ToString();
                    txtAccountType.Text = dtAssignment.Rows[index]["AccServiceType"].ToString();
                    lblProjectID.Value = dtAssignment.Rows[index]["ProjectID"].ToString();

                    hdfFollowUpRecordId.Value = String.Empty;
                    chkDealer.Enabled = true;

                    rbtnUnknown.Checked = rbtnMale.Checked = rbtnFemale.Checked = false;
                    if (sex == "F")
                        rbtnFemale.Checked = true;
                    else if (sex == "M")
                        rbtnMale.Checked = true;
                    else
                        rbtnUnknown.Checked = true;

                    if (String.IsNullOrEmpty(dtAssignment.Rows[index]["PreferA"].ToString()))
                        ddlPreferenceOne.SelectedIndex = 0;
                    else if (ddlPreferenceOne.Items.FindByValue(dtAssignment.Rows[index]["PreferA"].ToString().Trim()) != null)
                        ddlPreferenceOne.SelectedValue = dtAssignment.Rows[index]["PreferA"].ToString().Trim();

                    if (String.IsNullOrEmpty(dtAssignment.Rows[index]["PreferB"].ToString()))
                    {
                        if (ddlPreferenceTwo.Items.Count > 1)
                            ddlPreferenceTwo.SelectedIndex = 1;
                        else
                            ddlPreferenceTwo.SelectedIndex = 0;
                    }
                    else if (ddlPreferenceTwo.Items.FindByValue(dtAssignment.Rows[index]["PreferB"].ToString().Trim()) != null)
                    {
                        ddlPreferenceTwo.SelectedValue = dtAssignment.Rows[index]["PreferB"].ToString().Trim();
                    }


                    if (String.IsNullOrEmpty(dtAssignment.Rows[index]["Rank"].ToString()))
                        ddlRank.SelectedValue = "0";
                    else
                        ddlRank.SelectedValue = dtAssignment.Rows[index]["Rank"].ToString();

                    /// <Added by Thet Maung Chaw>
                    if (ddlActualDealer.Items.Count > 1)
                    {
                        ddlActualDealer.SelectedIndex = 1;
                        hdfTeamCode.Value = ddlActualDealer.SelectedValue;
                    }
                }

                //Retrieve contact history.(it will retrieve when user click account no (or) client name)
                if (!String.IsNullOrEmpty(dtAssignment.Rows[index]["AcctNo"].ToString()))
                {
                    contactType = "History";
                    ViewState["ContactType"] = "History";
                    lbtnContactHistory.Enabled = true;
                    this.retrieveContactHistory(dtAssignment.Rows[index]["AcctNo"].ToString());
                }

                btnUpdateContact.Visible = false;
                btnAddContact.Visible = true;
                btnCancel.Visible = false;
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
                string cutOffDtStr = "", clientPrefer = "", clientRank = "";
                DataTable dtPreferList = ViewState["dtPreferList"] as DataTable;
                DataRow drPreference = null;

                System.Web.UI.WebControls.Button btnModify = (System.Web.UI.WebControls.Button)e.Row.Cells[16].FindControl("gvbtnEdit");
                System.Web.UI.WebControls.Button btnDelete = (System.Web.UI.WebControls.Button)e.Row.Cells[16].FindControl("gvbtnDelete");

                if (btnModify != null)
                {
                    ((System.Web.UI.WebControls.Button)e.Row.Cells[16].FindControl("gvbtnEdit")).Enabled = (bool)ViewState["ModifyAccessRight"];
                }

                if (btnDelete != null)
                {
                    ((System.Web.UI.WebControls.Button)e.Row.Cells[16].FindControl("gvbtnDelete")).Enabled = (bool)ViewState["DeleteAccessRight"];

                }


                /*
                if (DataBinder.Eval(e.Row.DataItem, "ContactDate") != null)
                {
                    cutOffDtStr = DataBinder.Eval(e.Row.DataItem, "ContactDate").ToString();
                    if (!String.IsNullOrEmpty(cutOffDtStr))
                    {
                        e.Row.Cells[1].Text = DateTime.ParseExact(cutOffDtStr, "yyyy-MM-dd HH:mm:ss", null).ToString("dd/MM/yyyy HH:mm:ss");
                    }
                }
                */

                if (DataBinder.Eval(e.Row.DataItem, "PreferA") != null)
                {
                    string preFerA = DataBinder.Eval(e.Row.DataItem, "PreferA").ToString().Trim();
                    if (!String.IsNullOrEmpty(preFerA))
                    {
                        drPreference = dtPreferList.Rows.Find(preFerA);
                        // drPreference = dtPreferList.Rows.Find(DataBinder.Eval(e.Row.DataItem, "PreferA").ToString().Trim());
                        if (drPreference != null)
                        {
                            clientPrefer = drPreference["OptionContent"].ToString();
                        }
                    }
                }

                if (DataBinder.Eval(e.Row.DataItem, "PreferB") != null)
                {
                    if (!String.IsNullOrEmpty(clientPrefer))
                    {
                        clientPrefer = clientPrefer + "<br />";
                    }

                    drPreference = dtPreferList.Rows.Find(DataBinder.Eval(e.Row.DataItem, "PreferB").ToString().Trim());
                    if (drPreference != null)
                    {
                        clientPrefer = clientPrefer + drPreference["OptionContent"].ToString();
                    }

                    //Client Preference List
                    ((Label)e.Row.FindControl("gvlblPreferences")).Text = clientPrefer;
                }

                /*
                if (DataBinder.Eval(e.Row.DataItem, "Rank") != null)
                {
                    e.Row.Cells[10].Text = CommonUtilities.GetClientRank(DataBinder.Eval(e.Row.DataItem, "Rank").ToString());                                        
                }
                */


                if (ViewState["ContactType"].ToString() == "History")
                {
                    //Check for Extra Call
                    if (String.IsNullOrEmpty(DataBinder.Eval(e.Row.DataItem, "AssignDate").ToString()))
                    {
                        System.Drawing.Color c = System.Drawing.ColorTranslator.FromHtml("#FFCC99");
                        e.Row.BackColor = c;
                    }

                    ((LinkButton)e.Row.FindControl("gvlbtnAccountNo")).Visible = false;
                    ((LinkButton)e.Row.FindControl("gvlbtnClientName")).Visible = false;
                }
                else
                {
                    ((Label)e.Row.FindControl("gvlblAccountNo")).Visible = false;
                    ((Label)e.Row.FindControl("gvlblClientName")).Visible = false;
                }
            }

            //Modify and Delete button is off for Contact History
            if (ViewState["ContactType"] == "History")
            {
                e.Row.Cells[10].Visible = false;
                //e.Row.Cells[12].Visible = false;
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
                    //if (e.CommandName == "AcctNo")
                    //{
                    //    DisplayContactDetail(drModify["AcctNo"].ToString(), drModify["CName"].ToString(), drModify["ShortKey"].ToString(),
                    //        drModify["Phone"].ToString(), drModify["Sex"].ToString(), drModify["PreferA"].ToString(), drModify["PreferB"].ToString(),
                    //        drModify["Content"].ToString(), drModify["Remarks"].ToString(), drModify["Rank"].ToString(), drModify["AccServiceType"].ToString(),drModify["ProjectID"].ToString());
                    //}

                    ViewState["ContactType"] = "History";
                    lbtnContactHistory.Enabled = true;
                    this.retrieveContactHistory(drModify["AcctNo"].ToString());
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
        protected void btnShortKey_Click(object sender, EventArgs e)
        {
            divMessage.InnerHtml = "";

            if (!String.IsNullOrEmpty(txtKey.Text.Trim()))
            {
                txtKey.Text = txtKey.Text.Trim().ToUpper();

                clientContactService = new ClientContactService(base.dbConnectionStr);
                DataSet ds = clientContactService.RetrieveClientInfoByShortKey(txtKey.Text);

                if (ds.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString() == "1")
                {

                    DisplayContactDetail(ds.Tables[0].Rows[0]["AcctNo"].ToString(), ds.Tables[0].Rows[0]["LNAME"].ToString(),
                        txtKey.Text.Trim(), ds.Tables[0].Rows[0]["Phone"].ToString(), ds.Tables[0].Rows[0]["Sex"].ToString(),
                        ds.Tables[0].Rows[0]["PreferA"].ToString(), ds.Tables[0].Rows[0]["PreferB"].ToString(),
                        "", "", "0", ds.Tables[0].Rows[0]["AccServiceType"].ToString(), ds.Tables[0].Rows[0]["ProjectID"].ToString(), "");

                    /*
                    lblAccountName.Text = ds.Tables[0].Rows[0]["LNAME"].ToString();
                    txtAccountNo.Text = ds.Tables[0].Rows[0]["AcctNo"].ToString();
                    txtPhone.Text = ds.Tables[0].Rows[0]["Phone"].ToString();

                    rbtnUnknown.Checked = rbtnMale.Checked = rbtnFemale.Checked = false;

                    if (ds.Tables[0].Rows[0]["Sex"].ToString().Trim() == "F")
                        rbtnFemale.Checked = true;
                    else if (ds.Tables[0].Rows[0]["Sex"].ToString().Trim() == "M")
                        rbtnMale.Checked = true;
                    else
                        rbtnUnknown.Checked = true;
                            
                                
                    if (ddlPreferenceOne.Items.FindByValue(ds.Tables[0].Rows[0]["PreferA"].ToString().Trim()) != null)
                        ddlPreferenceOne.SelectedValue = ds.Tables[0].Rows[0]["PreferA"].ToString().Trim();
                    

                    if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["PreferB"].ToString().Trim()))
                    {
                        if (ddlPreferenceTwo.Items.FindByValue(ds.Tables[0].Rows[0]["PreferB"].ToString().Trim()) != null)
                            ddlPreferenceTwo.SelectedValue = ds.Tables[0].Rows[0]["PreferB"].ToString();
                    }
                    */
                }
                else
                {
                    divMessage.InnerHtml = "Client Short Key not found!";
                    /*
                    txtAccountNo.Text = "";
                    txtPhone.Text = "";

                    rbtnFemale.Checked = false;
                    rbtnMale.Checked = false;

                    ddlPreferenceOne.SelectedIndex = 0;
                    ddlPreferenceTwo.SelectedIndex = 1;
                    */
                }
            }
        }

        protected void ddlActualDealer_SelectedIndexChanged(object sender, EventArgs e)
        {

            clientContactService = new ClientContactService(base.dbConnectionStr);
            DataSet ds = null;
            string dealerCode = "";

            divAssignPaging.Visible = false;

            //Clear Previous Dealer Contact History
            ViewState["dtContactHistory"] = null;

            if (!String.IsNullOrEmpty(ddlActualDealer.SelectedValue))
            {
                dealerCode = ddlActualDealer.SelectedItem.Text.Split('-')[1].Trim();

                ds = clientContactService.RetrieveUnContactedAssignment(dealerCode, base.userLoginId);
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

                    divCHPaging.Visible = false;
                    divAssignPaging.Visible = false;
                }

                /// <Added by Thet Maung Chaw>
                DataSet dsContactToFollowUp = clientContactService.PrepareForContactToFollowUp(ddlActualDealer.SelectedValue, "Admin");
                if (dsContactToFollowUp.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString().Equals("1"))
                {
                    ViewState["dtContactToFollowUp"] = dsContactToFollowUp.Tables[0];
                    gvContactFollowUp.DataSource = dsContactToFollowUp.Tables[0];
                    gvContactFollowUp.DataBind();
                    divFollowUpPaging.Visible = true;
                }
                else if (dsContactToFollowUp.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString().Equals("-1"))
                {
                    ViewState["dtContactToFollowUp"] = null;
                    gvContactFollowUp.DataSource = null;
                    gvContactFollowUp.DataBind();
                    divFollowUpPaging.Visible = false;
                    divMessage.InnerHtml = dsContactToFollowUp.Tables["ReturnTable"].Rows[0]["ReturnMessage"].ToString();
                }
                else
                {
                    ViewState["dtContactToFollowUp"] = null;
                    gvContactFollowUp.DataSource = null;
                    gvContactFollowUp.DataBind();
                    divFollowUpPaging.Visible = false;
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

                divCHPaging.Visible = false;
                divAssignPaging.Visible = false;
            }

            //clearContactEntryForm();
            divMessage.InnerHtml = "";
            divMessageTwo.InnerHtml = "";

            // Retrieve team code to resend email to client

            DataTable hdActualDealer = ViewState["hdfDealerTable"] as DataTable;
            for (int i = 0; i < hdActualDealer.Rows.Count; i++)
            {
                if (ddlActualDealer.SelectedItem.Text.Split('-')[1].Trim() == hdActualDealer.Rows[i]["AECode"].ToString())
                {
                    hdfTeamCode.Value = hdActualDealer.Rows[0]["AEGroup"].ToString();
                }
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            clearContactEntryForm();

            if (String.IsNullOrEmpty(Request.Params["type"]))
            {
                Response.Redirect("~/WebPages/ContactManagement/ContactEntry.aspx");
            }
            else
            {
                Response.Redirect("~/WebPages/ContactManagement/ContactEntry.aspx?type=admin");
            }
        }

        protected void btnUpdateContact_Click(object sender, EventArgs e)
        {
            string dealerCode = "", userId = "", adminId = "", userRole = "user", sex = "", keep = "Y", validationResult = "",
                        lastAcctNo = "", clientName = "";

            string followUpDealer = "";
            string followUpDate = "";

            string[] wsReturn = null;
            DataTable dtEntryHistory = ViewState["dtEntryHistory"] as DataTable;
            DataRow drNewContact = null;
            clientContactService = new ClientContactService(base.dbConnectionStr);

            //Validate Contact Entry Form
            validationResult = ValidateContactEntryForm();

            if (validationResult == "ok")
            {
                if (rbtnFemale.Checked)
                    sex = "F";
                else if (rbtnMale.Checked)
                    sex = "M";


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

                if (chkDealer.Checked)
                {
                    followUpDealer = ddlFollowUpDealer.SelectedValue;
                    followUpDate = calFollowUpDate.DateTextFromValue;
                }

                int followUpStatus = chkDealer.Checked ? 1 : 0;
                String followUpRecId = hdfFollowUpRecordId.Value;
                if (!String.IsNullOrEmpty(followUpRecId))
                {
                    followUpStatus = 2;
                }

                wsReturn = clientContactService.UpdateClientContact(dealerCode, userId, txtAccountNo.Text.Trim(), txtKey.Text.Trim(),
                    sex, txtPhone.Text.Trim(), txtContact.Text.Trim(), ddlPreferenceOne.SelectedValue.Trim(), ddlPreferenceTwo.SelectedValue.Trim(),
                    "", ddlRank.SelectedValue, keep, adminId, hdfRecId.Value, followUpStatus, followUpDealer, followUpDate, lblProjectID.Value);

                if (int.Parse(wsReturn[0]) > 0)
                {
                    drNewContact = dtEntryHistory.Rows[int.Parse(hdfModifyIndex.Value)];

                    drNewContact["ContactDate"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    drNewContact["AcctNo"] = txtAccountNo.Text.Trim();
                    //drNewContact["CName"] = lblAccountName.Text;
                    drNewContact["Sex"] = sex;
                    drNewContact["Phone"] = txtPhone.Text.Trim();
                    drNewContact["Content"] = txtContact.Text.Trim();
                    //drNewContact["Remarks"] = txtCourse.Text.Trim();
                    drNewContact["Remarks"] = "";
                    drNewContact["PreferA"] = ddlPreferenceOne.SelectedValue.Trim();
                    drNewContact["PreferB"] = ddlPreferenceTwo.SelectedValue.Trim();
                    drNewContact["Rank"] = ddlRank.SelectedValue;
                    drNewContact["RankText"] = CommonUtilities.GetClientRank(ddlRank.SelectedValue);
                    drNewContact["ModifiedUser"] = dealerCode;
                    //drNewContact["AssignDate"] = assignDate;

                    ViewState["ContactType"] = "Entry";
                    ViewState["dtEntryHistory"] = dtEntryHistory;
                    gvContactHistory.PageIndex = int.Parse(pgcContactHistory.CurrentPageNo) - 1;
                    gvContactHistory.DataSource = dtEntryHistory;
                    gvContactHistory.DataBind();

                    if (chkDealer.Checked)
                    {
                        SentFollowUpEmailToDealer(followUpDealer, txtAccountNo.Text.Trim());
                    }

                    clearContactEntryForm();
                }

                validationResult = wsReturn[1];
            }

            divMessage.InnerHtml = validationResult;
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
                clientContactService = new ClientContactService(base.dbConnectionStr);
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

                ds = clientContactService.RetrieveContactEntryForToday(dealerCode, base.userLoginId);

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

            gvContactHistory.DataSource = dtClientContact;
            gvContactHistory.DataBind();
        }

        //protected override void DisplayDetails(int modifyIndex)
        //{
        //    btnUpdateContact.Visible = true;
        //    btnCancel.Visible = true;
        //    btnAddContact.Visible = false;
        //    this.hdfModifyIndex.Value = modifyIndex.ToString();

        //    int dataItemIndex = 1;
        //    DataTable dtEntryHistory = ViewState["dtEntryHistory"] as DataTable;

        //    HiddenField gvhdfRecordId = (HiddenField)gvContactHistory.Rows[modifyIndex].FindControl("gvhdfRecordId");

        //    dataItemIndex = int.Parse(gvhdfRecordId.Value);
        //    if (dtEntryHistory != null)
        //    {
        //        DataRow drModify = dtEntryHistory.Rows[dataItemIndex];

        //        hdfRecId.Value = drModify["RecId"].ToString();

        //        if (dtEntryHistory.Columns.Contains("FollowUpBy"))
        //        {
        //            DisplayContactDetail(drModify["AcctNo"].ToString(), drModify["CName"].ToString(), drModify["ShortKey"].ToString(),
        //                drModify["Phone"].ToString(), drModify["Sex"].ToString(), drModify["PreferA"].ToString(), drModify["PreferB"].ToString(),
        //                drModify["Content"].ToString(), drModify["Remarks"].ToString(), drModify["Rank"].ToString(), drModify["AccServiceType"].ToString(), drModify["ProjectID"].ToString(), drModify["FollowUpBy"].ToString());
        //        }
        //        else
        //        {
        //            DisplayContactDetail(drModify["AcctNo"].ToString(), drModify["CName"].ToString(), drModify["ShortKey"].ToString(),
        //                drModify["Phone"].ToString(), drModify["Sex"].ToString(), drModify["PreferA"].ToString(), drModify["PreferB"].ToString(),
        //                drModify["Content"].ToString(), drModify["Remarks"].ToString(), drModify["Rank"].ToString(), drModify["AccServiceType"].ToString(), drModify["ProjectID"].ToString(), String.Empty);
        //        }
        //    }
        //}

        protected override void DisplayDetails(int modifyIndex)
        {
            btnUpdateContact.Visible = true;
            btnCancel.Visible = true;
            btnAddContact.Visible = false;
            this.hdfModifyIndex.Value = modifyIndex.ToString();

            int dataItemIndex = 1;
            DataTable dtContactHistory = ViewState["dtContactHistory"] as DataTable;

            HiddenField gvhdfRecordId = (HiddenField)gvContactHistory.Rows[modifyIndex].FindControl("gvhdfRecordId");

            dataItemIndex = int.Parse(gvhdfRecordId.Value);
            if (dtContactHistory != null)
            {
                DataRow drModify = dtContactHistory.Rows[dataItemIndex];

                hdfRecId.Value = drModify["RecId"].ToString();

                txtAccountNo.Text = drModify["AcctNo"].ToString();
                lblAccountName.Text = drModify["CName"].ToString();
                txtKey.Text = "";
                txtPhone.Text =  drModify["Phone"].ToString();
                txtContact.Text = drModify["Content"].ToString();
                //txtCourse.Text = remark;          
                txtAccountType.Text =  drModify["AccServiceType"].ToString();
                lblProjectID.Value = drModify["ProjectID"].ToString();
                rbtnUnknown.Checked = rbtnMale.Checked = rbtnFemale.Checked = false;

                if (drModify["Sex"].ToString() == "F")
                    rbtnFemale.Checked = true;
                else if (drModify["Sex"].ToString() == "M")
                    rbtnMale.Checked = true;
                else
                    rbtnUnknown.Checked = true;

                if (ddlPreferenceOne.Items.FindByValue(drModify["PreferA"].ToString()) != null)
                {
                    ddlPreferenceOne.SelectedValue = drModify["PreferA"].ToString();
                }
                else
                {
                    ddlPreferenceOne.SelectedIndex = 0;
                }

                if (ddlPreferenceTwo.Items.FindByValue(drModify["PreferB"].ToString()) != null)
                {
                    ddlPreferenceTwo.SelectedValue = drModify["PreferB"].ToString();
                }
                else if (ddlPreferenceTwo.Items.Count > 1)
                {
                    ddlPreferenceTwo.SelectedIndex = 1;
                }
                else
                {
                    ddlPreferenceTwo.SelectedIndex = 0;
                }

                if (String.IsNullOrEmpty(drModify["Rank"].ToString()))
                    ddlRank.SelectedValue = "0";
                else
                    ddlRank.SelectedValue = drModify["Rank"].ToString();

                if (drModify["FollowUpBy"].ToString() != String.Empty)
                {
                    chkDealer.Checked = true;
                    ddlFollowUpDealer.Enabled = true;
                    ddlFollowUpDealer.SelectedValue = drModify["FollowUpBy"].ToString();
                    lblDate.Visible = true;
                    calFollowUpDate.Visible = true;
                }
            }
        }

        protected override Hashtable DeleteRecord(int deleteIndex)
        {
            long deleteRecId = long.Parse(gvContactHistory.DataKeys[deleteIndex].Value.ToString());

            lastDeleteAccNoTrack = gvContactHistory.Rows[deleteIndex].Cells[2].Text;
            HiddenField gvhdfRecordId = (HiddenField)gvContactHistory.Rows[deleteIndex].FindControl("gvhdfRecordId");
            clientContactService = new ClientContactService(base.dbConnectionStr);
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

            ds = clientContactService.DeleteClientContact(deleteRecId.ToString(), dealerCode, base.userLoginId);
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

                /// <Added by Thet Maung Chaw>
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
                dtEntryHistory.Columns.Add("AcctNo", String.Empty.GetType());
                dtEntryHistory.Columns.Add("CName", String.Empty.GetType());
                dtEntryHistory.Columns.Add("Sex", String.Empty.GetType());
                dtEntryHistory.Columns.Add("Phone", String.Empty.GetType());
                dtEntryHistory.Columns.Add("Content", String.Empty.GetType());

                dtEntryHistory.Columns.Add("SeminarName", String.Empty.GetType());
                dtEntryHistory.Columns.Add("FollowUpStatus", String.Empty.GetType());
                dtEntryHistory.Columns.Add("FollowUpDate", String.Empty.GetType());

                dtEntryHistory.Columns.Add("Remarks", String.Empty.GetType());
                dtEntryHistory.Columns.Add("PreferA", String.Empty.GetType());
                dtEntryHistory.Columns.Add("PreferB", String.Empty.GetType());
                dtEntryHistory.Columns.Add("Rank", String.Empty.GetType());
                dtEntryHistory.Columns.Add("ModifiedUser", String.Empty.GetType());
                dtEntryHistory.Columns.Add("AssignDate", String.Empty.GetType());
                dtEntryHistory.Columns.Add("ShortKey", String.Empty.GetType());     //It will get from txtKey
                dtEntryHistory.Columns.Add("RankText", String.Empty.GetType());
                //Add 3 Records

                //20110927 
                dtEntryHistory.Columns.Add("AECode", String.Empty.GetType());
                dtEntryHistory.Columns.Add("AltAECode", String.Empty.GetType());

                dtEntryHistory.Columns.Add("AccServiceType", String.Empty.GetType());
                dtEntryHistory.Columns.Add("ProjectID", String.Empty.GetType());
                // dtEntryHistory.Columns.Add("AccServiceType", String.Empty.GetType());  
            }

            drNewContact = dtEntryHistory.NewRow();
            drNewContact["RecId"] = dr["RecId"].ToString();
            drNewContact["AssignDate"] = DBNull.Value;
            drNewContact["ContactDate"] = dr["ContactDate"].ToString();
            drNewContact["AEGroup"] = dr["AEGroup"].ToString();
            drNewContact["AcctNo"] = dr["AcctNo"].ToString();
            drNewContact["CName"] = dr["CName"].ToString();
            drNewContact["Sex"] = dr["Sex"].ToString();
            drNewContact["Phone"] = dr["Phone"].ToString();
            drNewContact["Content"] = dr["Content"].ToString();

            //drNewContact["SeminarName"] = dr["SeminarName"].ToString();
            drNewContact["FollowUpStatus"] = dr["FollowUpStatus"].ToString();
            drNewContact["FollowUpDate"] = dr["FollowUpDate"].ToString();

            drNewContact["Remarks"] = dr["Remarks"].ToString();
            drNewContact["PreferA"] = dr["PreferA"].ToString();
            drNewContact["PreferB"] = dr["PreferB"].ToString();
            drNewContact["Rank"] = dr["Rank"].ToString();
            drNewContact["ModifiedUser"] = dr["ModifiedUser"].ToString();
            drNewContact["ShortKey"] = txtKey.Text.Trim();
            drNewContact["RankText"] = dr["RankText"].ToString();

            drNewContact["AECode"] = dr["AECode"].ToString();
            drNewContact["AltAECode"] = dr["AltAECode"].ToString();

            drNewContact["AccServiceType"] = dr["AccServiceType"].ToString();
            drNewContact["ProjectID"] = dr["ProjectID"].ToString();

            dtEntryHistory.Rows.InsertAt(drNewContact, 0);
            ViewState["dtEntryHistory"] = dtEntryHistory;
        }

        /*
        private void UpdateHistoryEntry(string accountNo, string sex, string content, string remark)
        {

        }
        */

        private void DisplayContactDetail(string accountNo, string clientName, string shortKey, string contactNo,
                        string sex, string preferA, string preferB, string content, string remark, string rank, string AcctType, string projectID, String FollowUpBy)
        {
            txtAccountNo.Text = accountNo;
            lblAccountName.Text = clientName;
            txtKey.Text = shortKey;
            txtPhone.Text = contactNo;
            txtContact.Text = content;
            //txtCourse.Text = remark;          
            txtAccountType.Text = AcctType;
            lblProjectID.Value = projectID;
            rbtnUnknown.Checked = rbtnMale.Checked = rbtnFemale.Checked = false;
            if (sex == "F")
                rbtnFemale.Checked = true;
            else if (sex == "M")
                rbtnMale.Checked = true;
            else
                rbtnUnknown.Checked = true;

            if (ddlPreferenceOne.Items.FindByValue(preferA) != null)
            {
                ddlPreferenceOne.SelectedValue = preferA;
            }
            else
            {
                ddlPreferenceOne.SelectedIndex = 0;
            }

            if (ddlPreferenceTwo.Items.FindByValue(preferB) != null)
            {
                ddlPreferenceTwo.SelectedValue = preferB;
            }
            else if (ddlPreferenceTwo.Items.Count > 1)
            {
                ddlPreferenceTwo.SelectedIndex = 1;
            }
            else
            {
                ddlPreferenceTwo.SelectedIndex = 0;
            }

            if (String.IsNullOrEmpty(rank))
                ddlRank.SelectedValue = "0";
            else
                ddlRank.SelectedValue = rank;

            if (FollowUpBy != String.Empty)
            {
                chkDealer.Checked = true;
                ddlFollowUpDealer.Enabled = true;
                ddlFollowUpDealer.SelectedValue = FollowUpBy;
                lblDate.Visible = true;
                calFollowUpDate.Visible = true;
            }
        }

        private void retrieveContactHistory(string accountNo)
        {
            tdContactHistory.BgColor = "#6E6A6B";
            tdEntryHistory.BgColor = "";

            clientContactService = new ClientContactService(base.dbConnectionStr);
            DataSet ds = clientContactService.getContactHistoryByAccountNo(accountNo);

            if (ds.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString().Equals("1"))
            {
                ViewState["ContactType"] = "History";

                ViewState["dtContactHistory"] = ds.Tables[0];
                gvContactHistory.DataSource = ds.Tables[0];
                gvContactHistory.DataBind();

                divCHPaging.Visible = true;
                DisplayContactHistoryPaging();
            }
            else
            {
                divCHPaging.Visible = false;

                gvContactHistory.DataSource = null;
                gvContactHistory.DataBind();
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

            if ((dtContactHistory != null) && (dtContactHistory.Rows[0]["AcctNo"].ToString().Equals(txtAccountNo.Text.Trim())))
            {
                drNewContact = dtContactHistory.NewRow();
                drNewContact["ContactDate"] = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");

                //Need To return from WebService after successfully inserted into ClientContact table. And it also need to return from WebService
                //whether inserted Contact is Extra called or not.
                drNewContact["AEGroup"] = dtContactHistory.Rows[0]["AEGroup"].ToString();
                drNewContact["AcctNo"] = txtAccountNo.Text.Trim();
                drNewContact["CName"] = lblAccountName.Text;
                drNewContact["Sex"] = sex;
                drNewContact["Phone"] = txtPhone.Text.Trim();
                drNewContact["Content"] = txtContact.Text.Trim();
                // drNewContact["Remarks"] = txtCourse.Text.Trim();
                drNewContact["Remarks"] = "";
                drNewContact["PreferA"] = ddlPreferenceOne.SelectedValue.Trim();
                drNewContact["PreferB"] = ddlPreferenceTwo.SelectedValue.Trim();
                drNewContact["Rank"] = ddlRank.SelectedValue;
                drNewContact["ModifiedUser"] = hdfDealerCode.Value;
                dtContactHistory.Rows.Add(drNewContact);
            }
            else if (dtContactHistory == null)
            {
                dtContactHistory = new DataTable();

                dtContactHistory.Columns.Add("ContactDate", String.Empty.GetType());
                dtContactHistory.Columns.Add("AEGroup", String.Empty.GetType());
                dtContactHistory.Columns.Add("AcctNo", String.Empty.GetType());
                dtContactHistory.Columns.Add("CName", String.Empty.GetType());
                dtContactHistory.Columns.Add("Sex", String.Empty.GetType());
                dtContactHistory.Columns.Add("Phone", String.Empty.GetType());
                dtContactHistory.Columns.Add("Content", String.Empty.GetType());
                dtContactHistory.Columns.Add("Remarks", String.Empty.GetType());
                dtContactHistory.Columns.Add("PreferA", String.Empty.GetType());
                dtContactHistory.Columns.Add("PreferB", String.Empty.GetType());
                dtContactHistory.Columns.Add("Rank", String.Empty.GetType());
                dtContactHistory.Columns.Add("ModifiedUser", String.Empty.GetType());
            }

            ViewState["dtContactHistory"] = dtContactHistory;
            gvContactHistory.DataSource = dtContactHistory;
            gvContactHistory.DataBind();

            divCHPaging.Visible = true;
            DisplayContactHistoryPaging();
        }

        private string ValidateContactEntryForm()
        {
            string result = "ok", userRole = "user";

            if (String.IsNullOrEmpty(txtAccountNo.Text.Trim()))
            {
                result = "Account Number cannot be blank!";
            }
            else if (String.IsNullOrEmpty(txtPhone.Text.Trim()))
            {
                result = "Phone Number cannot be blank!";
            }
            else if (!commonUtil.IsNumber(txtPhone.Text.Trim()))
            {
                result = "Phone Number must be numeric!";
            }
            else if (String.IsNullOrEmpty(txtContact.Text.Trim()))
            {
                result = "Content cannot be blank!";
            }
            else if (txtContact.Text.Trim().Length > 500)
            {
                result = "Maximum characters for Content is 500!";
            }
            //else if (txtCourse.Text.Trim().Length > 255)
            //{
            //    result = "Maximum characters for Course/Remark is 255!";
            //}
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

            if (result == "ok" && chkDealer.Checked == true && ddlFollowUpDealer.SelectedValue == "")
            {
                result = "Please select FollowUp Dealer!";
            }
            if (result == "ok" && chkDealer.Checked == true)
            {
                string dateFormat = "dd/mm/yyyy";
                bool dateFlag = String.IsNullOrEmpty(calFollowUpDate.DateTextFromValue);
                DateTime? followUpDate = null;
                if (dateFlag)
                {
                    result = "Follow Up Date cannot be blank!";
                }
                else if ((!dateFlag))
                {
                    try
                    {
                        DateTime.ParseExact(calFollowUpDate.DateTextFromValue, dateFormat, null);
                    }
                    catch
                    {
                        result = "Date format should be" + dateFormat;
                    }
                }

            }
            return result;
        }

        private void clearContactEntryForm()
        {
            lblAccountName.Text = "";
            txtAccountNo.Text = "";
            txtKey.Text = "";
            txtPhone.Text = "";
            txtContact.Text = "";
            //txtCourse.Text = "";
            txtAccountType.Text = "";
            lblProjectID.Value = "";
            rbtnFemale.Checked = false;
            rbtnMale.Checked = false;
            rbtnUnknown.Checked = true;

            ddlPreferenceOne.SelectedIndex = 0;
            ddlPreferenceTwo.SelectedIndex = 1;
            ddlRank.SelectedValue = "0";
        }

        private void PrepareDealerList()
        {
            Boolean flag = false;

            ddlFollowUpDealer.Items.Clear();
            clientAssignmentService = new ClientAssignmentService(base.dbConnectionStr);
            DataSet ds = clientAssignmentService.RetrieveAllDealer(base.userLoginId);

            if (ds.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString() == "1")
            {
                ddlFollowUpDealer.Items.Add(new ListItem("--- Select Dealer ---", ""));
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    ddlFollowUpDealer.Items.Add(new ListItem(ds.Tables[0].Rows[i]["DisplayName"].ToString(), ds.Tables[0].Rows[i]["AECode"].ToString()));

                    if (ds.Tables[0].Rows[i]["UserID"].ToString().Equals(base.userLoginId) && flag == false)
                    {
                        ddlFollowUpDealer.SelectedValue = ds.Tables[0].Rows[i]["AECode"].ToString();
                        flag = true;
                    }
                }
            }
        }

        protected void btnClientOverView_Click(object sender, EventArgs e)
        {
            DataSet dsUserInfo = null;
            DataSet dsSemnior = null;

            /// <Updated by Thet Maung Chaw.  txtAccountType.Text will be blank.>
            //if (String.IsNullOrEmpty(txtAccountNo.Text) || String.IsNullOrEmpty(txtAccountType.Text))
            if (String.IsNullOrEmpty(txtAccountNo.Text))
            {
                divMessage.InnerHtml = "Please Add Client Information";
                gvCommissionEarned.DataSource = null;
                gvCommissionEarned.DataBind();
            }
            else
            {
                divContentEntryForm.Visible = false;
                divClientOverViewForm.Visible = true;
                lblSeminarRegistration.Visible = false;

                clientContactService = new ClientContactService(base.dbConnectionStr);
                DataSet dsClientAcctType = clientContactService.RetrieveClientServiceTypeByClientAcct(txtAccountNo.Text);
                ClearClientServiceType();
                if (dsClientAcctType.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString() == "1")
                {
                    foreach (DataRow dr in dsClientAcctType.Tables[0].Rows)
                    {
                        BindUserAccountInformation(dr["AcctType"].ToString(), dr["AcctNo"].ToString(), dr["AeCode"].ToString());
                    }
                }

                labError.Text = dsClientAcctType.Tables["ReturnTable"].Rows[0]["returnMessage"].ToString();

                clientContactService = new ClientContactService(base.dbConnectionStr);
                dsUserInfo = clientContactService.GetDetailUserInformation(txtAccountNo.Text);
                if (dsUserInfo.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString() == "1")
                {
                    ClientInfoView.DataSource = dsUserInfo;
                    ClientInfoView.DataBind();
                }

                labError.Text = dsUserInfo.Tables["ReturnTable"].Rows[0]["returnMessage"].ToString();

                dsSemnior = clientContactService.GetSeminorRegistrationByAccNo(txtAccountNo.Text);
                if (dsSemnior.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString() == "1")
                {
                    if (dsSemnior.Tables[0].Rows.Count > 0)
                    {
                        lblSeminarRegistration.Visible = true;
                        gvSeminar.DataSource = dsSemnior;
                        gvSeminar.DataBind();
                    }
                }

                labError.Text = dsSemnior.Tables["ReturnTable"].Rows[0]["returnMessage"].ToString();

                //Commission Earned Table
                DataSet dsCommissionEarned = clientContactService.RetrieveCommissionEarnedByClientAcctNo(txtAccountNo.Text);
                if (dsCommissionEarned.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString() == "1")
                {
                    gvCommissionEarned.DataSource = CommonUtilities.GetPivotTable(dsCommissionEarned.Tables[0]);
                    gvCommissionEarned.DataBind();
                }

                labError.Text = dsCommissionEarned.Tables["ReturnTable"].Rows[0]["returnMessage"].ToString();

                //Prepare Datasource for Available Funds Balance
                String mmfString = dsUserInfo.Tables[0].Rows[0]["MMF"].ToString();
                if (!String.IsNullOrEmpty(mmfString))
                {
                    switch (mmfString)
                    {
                        case "Y":
                            DataSet dsCashAndEquivalents = clientContactService.RetrieveCashAndEquivalentsByUserAcctNo(txtAccountNo.Text);
                            Session["CashAndEquivalents"] = dsCashAndEquivalents;
                            Session["AvailableFunds"] = null;
                            labError.Text = dsCashAndEquivalents.Tables["ReturnTable"].Rows[0]["returnMessage"].ToString();
                            break;
                        case "N":
                            DataSet dsAvailableFunds = clientContactService.RetrieveAvailableFundsByUserAcctNo(txtAccountNo.Text);
                            Session["CashAndEquivalents"] = null;
                            Session["AvailableFunds"] = dsAvailableFunds;
                            labError.Text = dsAvailableFunds.Tables["ReturnTable"].Rows[0]["returnMessage"].ToString();
                            break;
                    }
                }

                

            }
        }

        private void ClearClientServiceType()
        {
            chkNonTCashTrading.Checked = false;
            txtNonTCashTrading.Text = String.Empty;
            chkNonTCFD.Checked = false;
            txtNonTCFD.Text = String.Empty;
            chkNonTCustodian.Checked = false;
            txtNonTCustodian.Text = String.Empty;
            chkNonTCashAcct.Checked = false;
            txtNonTCashAcct.Text = String.Empty;
            chkNonTPhillpMargin.Checked = false;
            txtNonTPhillipMargin.Text = String.Empty;
            chkNonTPhillipFinancial.Checked = false;
            txtNonTPhillipFinancial.Text = String.Empty;
            chkNonTDiscretionaryAcct.Checked = false;
            txtNonTDiscretionaryAcct.Text = String.Empty;
            chkNonTUTNW.Checked = false;
            txtNonTUTNW.Text = String.Empty;
            chkNonTAdvisoryAcct.Checked = false;
            txtNonTAdvisoryAcct.Text = String.Empty;

            chkCashTrading.Checked = false;
            txtCashTrading.Text = String.Empty;
            chkCFD.Checked = false;
            txtCFD.Text = String.Empty;
            chkCustodian.Checked = false;
            txtCustodian.Text = String.Empty;
            chkCashManagement.Checked = false;
            txtCashManagement.Text = String.Empty;
            chkPhillipMargin.Checked = false;
            txtPhillipMargin.Text = String.Empty;
            chkPhillipFinancial.Checked = false;
            txtPhillipFinancial.Text = String.Empty;
            chkDiscretionaryAcct.Checked = false;
            txtDiscretionaryAcct.Text = String.Empty;
            chkUTNW.Checked = false;
            txtUTNW.Text = String.Empty;
            chkAdvisoryAccount.Checked = false;
            txtAdvisoryAccount.Text = String.Empty;
        }

        private const String ACCOUNT_NUMBER = "Account Number : ";

        private void BindUserAccountInformation(string AccType, string AccNo, string aeCode)
        {
            bool isTSeries = VerifyAccountType(aeCode);
            if (isTSeries)
            {
                FillAccountInfoForTSeries(AccType, AccNo);
            }
            else
            {
                FillAccountInfoForNonTSeries(AccType, AccNo);
            }
        }

        private void FillAccountInfoForNonTSeries(string AccType, string AccNo)
        {
            switch (AccType)
            {
                case "CA":
                    {
                        chkNonTCashTrading.Checked = true;
                        FormatAccountTypeString(AccNo, txtNonTCashTrading);
                        break;
                    }
                case "CFD":
                    {
                        chkNonTCFD.Checked = true;
                        FormatAccountTypeString(AccNo, txtNonTCFD);
                        break;
                    }
                case "CU":
                    {
                        chkNonTCustodian.Checked = true;
                        FormatAccountTypeString(AccNo, txtNonTCustodian);
                        break;
                    }
                case "KC":
                    {
                        chkNonTCashAcct.Checked = true;
                        FormatAccountTypeString(AccNo, txtNonTCashAcct);
                        break;
                    }
                case "M":
                    {
                        chkNonTPhillpMargin.Checked = true;
                        FormatAccountTypeString(AccNo, txtNonTPhillipMargin);
                        break;
                    }
                case "PFN":
                    {
                        chkNonTPhillipFinancial.Checked = true;
                        FormatAccountTypeString(AccNo, txtNonTPhillipFinancial);
                        break;
                    }
                case "S2":
                    {
                        chkNonTDiscretionaryAcct.Checked = true;
                        FormatAccountTypeString(AccNo, txtNonTDiscretionaryAcct);
                        break;
                    }
                case "UT":
                    {
                        chkNonTUTNW.Checked = true;
                        FormatAccountTypeString(AccNo, txtNonTUTNW);
                        break;
                    }
                case "UTW":
                    {
                        chkNonTAdvisoryAcct.Checked = true;
                        FormatAccountTypeString(AccNo, txtNonTAdvisoryAcct);
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }

        private void FormatAccountTypeString(String accNo, Label textBox)
        {
            if (String.IsNullOrEmpty(textBox.Text))
            {
                textBox.Text = ACCOUNT_NUMBER + accNo;
            }
            else
            {
                textBox.Text = textBox.Text + ", " + accNo;
            }

        }

        private void FillAccountInfoForTSeries(string AccType, string AccNo)
        {
            switch (AccType)
            {
                case "CA":
                    {
                        chkCashTrading.Checked = true;
                        FormatAccountTypeString(AccNo, txtCashTrading);
                        break;
                    }
                case "CFD":
                    {
                        chkCFD.Checked = true;
                        FormatAccountTypeString(AccNo, txtCashTrading);
                        break;
                    }
                case "CU":
                    {
                        chkCustodian.Checked = true;
                        FormatAccountTypeString(AccNo, txtCustodian);
                        break;
                    }
                case "KC":
                    {
                        chkCashManagement.Checked = true;
                        FormatAccountTypeString(AccNo, txtCashManagement);
                        break;
                    }
                case "M":
                    {
                        chkPhillipMargin.Checked = true;
                        FormatAccountTypeString(AccNo, txtPhillipMargin);
                        break;
                    }
                case "PFN":
                    {
                        chkPhillipFinancial.Checked = true;
                        FormatAccountTypeString(AccNo, txtPhillipFinancial);
                        break;
                    }
                case "S2":
                    {
                        chkDiscretionaryAcct.Checked = true;
                        FormatAccountTypeString(AccNo, txtDiscretionaryAcct);
                        break;
                    }
                case "UT":
                    {
                        chkUTNW.Checked = true;
                        FormatAccountTypeString(AccNo, txtUTNW);
                        break;
                    }
                case "UTW":
                    {
                        chkAdvisoryAccount.Checked = true;
                        FormatAccountTypeString(AccNo, txtAdvisoryAccount);
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }

        private bool VerifyAccountType(string aeCode)
        {
            bool isTSeries = false;
            if (!String.IsNullOrEmpty(aeCode))
            {
                if (aeCode[0].ToString().ToLower().Equals("t"))
                {
                    isTSeries = true;
                }
                else
                {
                    if (aeCode.Length > 3)
                    {
                        isTSeries = aeCode.Substring(0, 3).ToLower().Equals("sfr");
                    }
                }
            }
            return isTSeries;
        }

        protected void btnEntry_Click(object sender, EventArgs e)
        {
            divClientOverViewForm.Visible = false;
            divContentEntryForm.Visible = true;
        }

        protected void ddlProjectList_SelectedIndexChanged(object sender, EventArgs e)
        {

            string projectID = "";
            lblObjective.Text = "";
            divAssignPaging.Visible = false;

            //Clear Previous Dealer Contact History
            ViewState["dtContactHistory"] = null;
            if (!String.IsNullOrEmpty(ddlProjectList.SelectedValue))
            {
                projectID = ddlProjectList.SelectedValue;
                RetrieveUnContactedAssignmentForByProjectID(projectID);
                lblObjective.Text = GetProjectObjectiveByID(projectID);
            }
            else
            {
                ViewState["dtEntryHistory"] = null;
                ViewState["dtAssignment"] = null;
                gvAssignments.DataSource = null;
                gvAssignments.DataBind();

                gvContactHistory.DataSource = null;
                gvContactHistory.DataBind();

                divCHPaging.Visible = false;
                divAssignPaging.Visible = false;
            }

            clearContactEntryForm();
            divMessage.InnerHtml = "";
            divMessageTwo.InnerHtml = "";


            /// <Added by Thet Maung Chaw>
            ViewState["dtContactToFollowUp"] = null;
            gvContactFollowUp.DataSource = null;
            gvContactFollowUp.DataBind();
            divFollowUpPaging.Visible = false;
        }

        private string GetProjectObjectiveByID(string projectID)
        {
            string projectObj = "";
            DataTable dtProjectInfo = ViewState["dtProjectInfo"] as DataTable;
            for (int i = 0; i < dtProjectInfo.Rows.Count; i++)
            {
                if (dtProjectInfo.Rows[i]["ProjectID"].ToString().Equals(projectID))
                {
                    projectObj = dtProjectInfo.Rows[i]["ProjectObjective"].ToString();
                    break;
                }
            }
            return projectObj;
        }

        private void RetrieveUnContactedAssignmentForByProjectID(string projectID)
        {
            DataSet ds = null;
            clientContactService = new ClientContactService(base.dbConnectionStr);
            ds = clientContactService.RetrieveUnContactedAssignmentByProjectID(projectID, base.userLoginId);

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

                divCHPaging.Visible = false;
                divAssignPaging.Visible = false;
            }
        }

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
                e.Row.Cells[11].Text = String.Format("{0:00.00}", total);
            }
        }

        protected void gvCommissionEarned_PreRender(object sender, EventArgs e)
        {
            MergeRows(gvCommissionEarned);
        }

        public void MergeRows(GridView gridView)
        {
            for (int rowIndex = gridView.Rows.Count - 2; rowIndex >= 0; rowIndex--)
            {
                GridViewRow row = gridView.Rows[rowIndex];
                GridViewRow previousRow = gridView.Rows[rowIndex + 1];

                for (int i = 0; i < row.Cells.Count; i++)
                {
                    if (gridView.Columns[i].HeaderText.Equals(""))
                    {
                        if (row.Cells[i].Text == previousRow.Cells[i].Text)
                        {
                            row.Cells[i].RowSpan = previousRow.Cells[i].RowSpan < 2 ? 2 :
                                                   previousRow.Cells[i].RowSpan + 1;
                            previousRow.Cells[i].Visible = false;
                        }
                    }
                }
            }
        }

        //Contact to Follow-Up
        protected void gvContactFollowUp_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if ((e.CommandName == "AcctNo") || (e.CommandName == "ClientName"))
            {
                int index = Convert.ToInt32(e.CommandArgument);
                DataTable dtFollowUp = (DataTable)ViewState["dtContactToFollowUp"];

                if (e.CommandName == "AcctNo")
                {
                    clearContactEntryForm();
                    string sex = "";
                    lblAccountName.Text = dtFollowUp.Rows[index]["ClientName"].ToString();
                    txtAccountNo.Text = dtFollowUp.Rows[index]["AcctNo"].ToString();
                    txtKey.Text = dtFollowUp.Rows[index]["ShortKey"].ToString();
                    txtPhone.Text = dtFollowUp.Rows[index]["Phone"].ToString();
                    sex = dtFollowUp.Rows[index]["Sex"].ToString();
                    txtAccountType.Text = dtFollowUp.Rows[index]["AccServiceType"].ToString();
                    lblProjectID.Value = dtFollowUp.Rows[index]["ProjectID"].ToString();

                    //Keep RecId for ClientContact
                    hdfFollowUpRecordId.Value = dtFollowUp.Rows[index]["RecId"].ToString();
                    chkDealer.Checked = false;
                    chkDealer.Enabled = false;

                    rbtnUnknown.Checked = rbtnMale.Checked = rbtnFemale.Checked = false;
                    if (sex == "F")
                        rbtnFemale.Checked = true;
                    else if (sex == "M")
                        rbtnMale.Checked = true;
                    else
                        rbtnUnknown.Checked = true;

                    if (String.IsNullOrEmpty(dtFollowUp.Rows[index]["PreferA"].ToString()))
                        ddlPreferenceOne.SelectedIndex = 0;
                    else if (ddlPreferenceOne.Items.FindByValue(dtFollowUp.Rows[index]["PreferA"].ToString().Trim()) != null)
                        ddlPreferenceOne.SelectedValue = dtFollowUp.Rows[index]["PreferA"].ToString().Trim();

                    if (String.IsNullOrEmpty(dtFollowUp.Rows[index]["PreferB"].ToString()))
                    {
                        if (ddlPreferenceTwo.Items.Count > 1)
                            ddlPreferenceTwo.SelectedIndex = 1;
                        else
                            ddlPreferenceTwo.SelectedIndex = 0;
                    }
                    else if (ddlPreferenceTwo.Items.FindByValue(dtFollowUp.Rows[index]["PreferB"].ToString().Trim()) != null)
                    {
                        ddlPreferenceTwo.SelectedValue = dtFollowUp.Rows[index]["PreferB"].ToString().Trim();
                    }


                    if (String.IsNullOrEmpty(dtFollowUp.Rows[index]["Rank"].ToString()))
                        ddlRank.SelectedValue = "0";
                    else
                        ddlRank.SelectedValue = dtFollowUp.Rows[index]["Rank"].ToString();
                }

                //Retrieve contact history.(it will retrieve when user click account no (or) client name)
                if (!String.IsNullOrEmpty(dtFollowUp.Rows[index]["AcctNo"].ToString()))
                {
                    contactType = "History";
                    ViewState["ContactType"] = "History";
                    lbtnContactHistory.Enabled = true;
                    this.retrieveContactHistory(dtFollowUp.Rows[index]["AcctNo"].ToString());
                }

                btnUpdateContact.Visible = false;
                btnAddContact.Visible = true;
                btnCancel.Visible = false;
            }
        }

        protected void gvContactFollowUp_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string cutOffDtStr = "";
                string contactDtStr = "";

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

        protected void gvContactFollowUp_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataTable dtFollowUp = ViewState["dtContactToFollowUp"] as DataTable;
            DataTable sortedDataTable = null;

            string sortDirection = "", sortString = "";

            sortDirection = CommonUtilities.GetSortDirection(e.SortExpression, ViewState["FollowUpSortExpression"] as string, ViewState["FollowUpSortDirection"] as string);

            ViewState["FollowUpSortDirection"] = sortDirection;
            ViewState["FollowUpSortExpression"] = e.SortExpression;
            sortString = e.SortExpression + " " + sortDirection;
            sortedDataTable = CommonUtilities.SortDataTable(dtFollowUp, sortString);

            ViewState["dtContactToFollowUp"] = sortedDataTable;
            gvContactFollowUp.PageIndex = 0;
            gvContactFollowUp.DataSource = sortedDataTable;
            gvContactFollowUp.DataBind();
            DisplayFollowUpPaging();
            dtFollowUp.Dispose();
        }

        protected void pgcContactFollowUp_RowPerPageChanged(object sender, EventArgs<Int32> e)
        {
            DataTable dtFollowUp = ViewState["dtContactToFollowUp"] as DataTable;
            if (dtFollowUp != null)
            {
                gvContactFollowUp.PageSize = e.Value;
                gvContactFollowUp.PageIndex = 0;
                gvContactFollowUp.DataSource = dtFollowUp;
                gvContactFollowUp.DataBind();

                ViewState["RowPerPage"] = e.Value;
                DisplayFollowUpPaging();
            }
        }

        protected void pgcContactFollowUp_PageNoChange(object sender, EventArgs<Int32> e)
        {
            gvAssignments.PageIndex = e.Value - 1;

            DataTable dtFollowUp = ViewState["dtContactToFollowUp"] as DataTable;
            if (dtFollowUp != null)
            {
                gvContactFollowUp.DataSource = dtFollowUp;
                gvContactFollowUp.DataBind();
            }
        }

        private void DisplayFollowUpPaging()
        {
            if (divFollowUpPaging.Visible)
            {
                int rowPerPage = (int)ViewState["RowPerPage"];

                pgcContactFollowUp.PageCount = gvContactFollowUp.PageCount;
                pgcContactFollowUp.CurrentRowPerPage = rowPerPage.ToString();
                pgcContactFollowUp.DisplayPaging();
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            btnAddContact.Visible = true;
            btnUpdateContact.Visible = false;

            clientContactService = new ClientContactService(base.dbConnectionStr);

            /// <Updated by OC>
            //DataSet ds = clientContactService.RetrieveByAccountNo(txtAccountNo.Text, base.userLoginId);

            /// <Added by OC>
            DataSet ds = null;
            DataSet dsDealerDetail = ViewState["dsDealerDetail"] as DataSet;

            if (dsDealerDetail != null && dsDealerDetail.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString() == "1")
            {
                if (dsDealerDetail.Tables[0].Rows[0]["Supervisor"].ToString().Trim().ToUpper() == "Y")
                {
                    ds = clientContactService.RetrieveByAccountNo(txtAccountNo.Text, "");
                }
                else if (dsDealerDetail.Tables[0].Rows[0]["Supervisor"].ToString().Trim().ToUpper() == "S")
                {
                    ds = clientContactService.RetrieveByAccountNo(txtAccountNo.Text, "DealerDetail.AEGroup = '" + dsDealerDetail.Tables[0].Rows[0]["TeamCode"].ToString() + "' AND ");
                }
                else
                {
                    ds = clientContactService.RetrieveByAccountNo(txtAccountNo.Text, "DealerDetail.AECode = '" + dsDealerDetail.Tables[0].Rows[0]["AECode"].ToString() + "' AND ");
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
                lblAccountName.Text = ds.Tables[0].Rows[0]["LNAME"].ToString();
                txtPhone.Text = ds.Tables[0].Rows[0]["ContactPhone"].ToString();
                txtAccountType.Text = ds.Tables[0].Rows[0]["AccServiceType"].ToString();
                txtKey.Text = ds.Tables[0].Rows[0]["ShortKey"].ToString();

                if (ds.Tables[0].Rows[0]["Sex"].ToString().Trim().ToUpper() == "F")
                {
                    rbtnFemale.Checked = true;
                }
                else if (ds.Tables[0].Rows[0]["Sex"].ToString().Trim().ToUpper() == "M")
                {
                    rbtnMale.Checked = true;
                }
                else
                {
                    rbtnUnknown.Checked = true;
                }

                if (ds.Tables[0].Rows[0]["PreferA"].ToString() != String.Empty)
                {
                    ddlPreferenceOne.SelectedValue = ds.Tables[0].Rows[0]["PreferA"].ToString();
                }

                if (ds.Tables[0].Rows[0]["PreferB"].ToString() != String.Empty)
                {
                    ddlPreferenceTwo.SelectedValue = ds.Tables[0].Rows[0]["PreferB"].ToString();
                }

                if (ds.Tables[0].Rows[0]["Rank"].ToString() != String.Empty)
                {
                    ddlRank.SelectedValue = ds.Tables[0].Rows[0]["Rank"].ToString();
                }

                txtAccountNo.Focus();
            }
            else if (ds.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString() == "0")
            {
                clearContactEntryForm();
                divMessage.InnerHtml = "Account No not found!";
                txtAccountNo.Focus();
            }
            else
            {
                clearContactEntryForm();
                divMessage.InnerHtml = ds.Tables["ReturnTable"].Rows[0]["ReturnMessage"].ToString();
            }
        }

        protected void rdobtnDealer_CheckedChanged(object sender, EventArgs e)
        {
            ddlActualDealer.Enabled = true;
            ddlProjectList.Enabled = false;
        }

        protected void rdobtnProject_CheckedChanged(object sender, EventArgs e)
        {
            ddlActualDealer.Enabled = false;
            ddlProjectList.Enabled = true;
        }
    }
}