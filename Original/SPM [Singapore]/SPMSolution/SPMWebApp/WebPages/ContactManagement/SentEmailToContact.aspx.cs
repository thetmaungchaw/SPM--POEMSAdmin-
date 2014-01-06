﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SPMWebApp.Services;
using SPMWebApp.BasePage;
using System.Data;

using SPMWebApp.Utilities;
using System.IO;

namespace SPMWebApp.WebPages.ContactManagement
{
    public partial class SentEmailToContact : BasePage.BasePage
    {
        private ClientAssignmentService assignManagement;
        private DataSet ds = null;

        /// <summary>
        /// Fetch User Access Right
        /// Call to ValidateUserAccessRight method with FunctionCode
        /// </summary>        
        private void LoadUserAccessRight()
        {
            List<AccessRight> accessRightList;
            if (AccessRightUtilities.ValidateUserAccessRight((DataTable)Session["UserAccessRights"], "AccessManagement", out accessRightList))
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
            if (!IsPostBack)
            {
                string userid = "";
                string teamCode = "";
                userid = Convert.ToString(Request.QueryString["AccNo"]);
                teamCode = Convert.ToString(Request.QueryString["TeamCode"]);
                BindDataToDropDownList();
                if (!String.IsNullOrEmpty(userid))
                {
                    if (!String.IsNullOrEmpty(teamCode))
                    {
                        Session["ID"] = userid;
                        Session["Team"] = teamCode;
                        if (!String.IsNullOrEmpty(teamCode))
                        {
                            BindUserEmailToDropDownList(userid);
                        }
                    }
                }
            }
            base.divMessage = this.divMessage;
            divMessage.InnerText = "";
        }

        private void BindUserEmailToDropDownList(string userid)
        {
            assignManagement = new ClientAssignmentService(dbConnectionStr);
            DataSet ds = new DataSet();
            bool x = true;
            ds = assignManagement.GetClientInfoByAcctNo(userid);
            if (ds.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString() == "1")
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (!String.IsNullOrEmpty(dr["LEMAIL"].ToString()))
                        {
                            ddlSystemEmails.Items.Add(new ListItem(dr["LEMAIL"].ToString()));
                        }
                        else
                        {
                            x = false;
                        }
                    }
                }
                else
                {
                    x = false;
                }
            }
            else
            {
                x = false;

            }
            if (x == false)
            {
                ddlSystemEmails.Items.Add(new ListItem("--No Record Found--", ""));
            }
        }

        private void BindDataToDropDownList()
        {
            assignManagement = new ClientAssignmentService(dbConnectionStr);
            ds = assignManagement.RetrieveAllProjectInfo();
            if (ds.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString() == "1")
            {
                DataTable dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    ddlProjectName.Items.Add(new ListItem("--select project Name--", ""));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["ProjectType"].ToString().Trim() == "C")
                        {
                            ddlProjectName.Items.Add(new ListItem(dt.Rows[i]["ProjectName"].ToString(), dt.Rows[i]["FilePath"].ToString()));
                        }
                    }
                }
                else
                {
                    ddlProjectName.Items.Add(new ListItem("--select project Name--", ""));
                }
            }
            else
            {
                ddlProjectName.Items.Add(new ListItem("--select project Name--", ""));
            }
        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            if (ValidateResult())
            {
                string teamCode = Session["Team"].ToString();
                string projectName = ddlProjectName.SelectedValue.ToString();

                if (chkSysEmail.Checked)
                {
                    SendEmail(teamCode, ddlSystemEmails.SelectedValue, projectName);
                }

                if (chkCustomEmail.Checked)
                {
                    SendEmail(teamCode, txtCustomEmail.Text, projectName);
                }

                if (chkSysEmail.Checked == false && chkCustomEmail.Checked == false)
                {
                    divMessage.InnerHtml = "Please select email option";
                }
            }
        }

        private bool ValidateResult()
        {
            CommonUtilities common = new CommonUtilities();
            divMessage.InnerHtml = "";
            if (ddlProjectName.SelectedValue == "")
            {
                //divMessage.InnerHtml = "Please select project Name!";
                divMessage.InnerHtml = "Promotion template not found!";
            }
            else if ((chkSysEmail.Checked) && (ddlSystemEmails.SelectedValue == ""))
            {
                divMessage.InnerHtml = "Please select user email!";
            }
            else if ((chkCustomEmail.Checked) && (txtCustomEmail.Text == ""))
            {
                divMessage.InnerHtml = "Please select user email!";
            }
            else if ((chkSysEmail.Checked) && (ddlSystemEmails.SelectedValue != ""))
            {
                if (!common.isEmail(ddlSystemEmails.SelectedValue))
                {
                    divMessage.InnerHtml = "Invalid email format!";
                }
            }
            else if ((chkCustomEmail.Checked) && (txtCustomEmail.Text != ""))
            {
                if (!common.isEmail(txtCustomEmail.Text))
                {
                    divMessage.InnerHtml = "Invalid email format!";
                }
            }
            if (divMessage.InnerHtml == "")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void SendEmail(string teamCode, string toEmail, string projectPath)
        {
            try
            {
                string teamEmail = "";
                string c = "";

                EmailManager emailSender = new EmailManager();
                ClientAssignmentService clientAssignmentService = new ClientAssignmentService(base.dbConnectionStr);

                //get Assignment Announcement Email
                c = emailSender.ReadTextFile(Server.MapPath(projectPath));

                #region Added by Thet Maung Chaw to map the photo and template
                /// <"RemoveUnNecessaryText"
                /// "ReadTextFile"
                /// "ReplacePhotos">

                String readPath = Server.MapPath(projectPath);
                String writePath = Server.MapPath("~/PromotionTemplate/Temp.html");

                emailSender.RemoveUnNecessaryText(readPath, writePath);
                c = emailSender.ReadTextFile(writePath);

                String[] Photos = Directory.GetFiles(Server.MapPath("~/images/"));

                for (int i = 0; i < Photos.Length; i++)
                {
                    Photos[i] = Path.GetFileName(Photos[i].ToString());
                }

                c = emailSender.ReplacePhotos(c, Photos);

                c = emailSender.ReplaceUnNecessaryText(c);

                #endregion

                //Retrieve client name and replace at promotion template accordingly
                DataSet ds = new DataSet();
                ds = clientAssignmentService.GetClientInfoByAcctNo(Session["ID"].ToString());
                if (ds.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString() == "1")
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        string ClientName = dr["LNAME"].ToString();
                        string AcctNo = dr["LACCT"].ToString();
                        c = emailSender.ReplaceSpecialcharacter(c, ClientName, AcctNo);
                    }
                }

                if (!String.IsNullOrEmpty(c))
                {
                    ///send email to user from team email

                    //teamEmail = emailSender.GetTeamEmail(teamCode, base.dbConnectionStr);
                    //CommonUtilities common = new CommonUtilities();

                    //if (!String.IsNullOrEmpty(teamEmail))
                    //{
                    //if (common.isEmail(teamEmail.Trim()))
                    //{
                    //string EmailSubject = "Promotion letter( contact ) from Dealer:" + teamEmail;
                    string EmailSubject = "Exclusive PhillipCapital Promotion";

                    string pLogo = "";
                    pLogo = Server.MapPath("~/images/logo.jpg");

                    emailSender.SendPromotionEmail(teamEmail, toEmail, EmailSubject, c,pLogo);
                    clientAssignmentService.InsertEmailLog(teamEmail, toEmail, EmailSubject, c);
                    divMessage.InnerHtml = "Promotion email was sent successfully.";
                    //}
                    //else
                    //{
                    //    divMessage.InnerHtml = "Invalid team email format!";
                    //}
                    //}
                    //else
                    //{
                    //    divMessage.InnerHtml = "Team email not found!";
                    //}
                }
                else
                {
                    divMessage.InnerHtml = "Promotion template not found!";
                }
            }
            catch (Exception ex)
            {
                //divMessage.InnerHtml = "Error in sending email..!";
                divMessage.InnerHtml = ex.ToString();
            }
        }
    }
}