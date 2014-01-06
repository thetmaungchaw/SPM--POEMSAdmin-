using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SPMWebApp.Services;
using System.Data;


using SPMWebApp.Utilities;

namespace SPMWebApp.WebPages.EmailTemplate
{
    public partial class AddEditEmailTemplate : BasePage.BasePage
    {
        private EmailTemplateService emailTemplateService;
        DataSet ds = null;        
        string templateID = "";

        /// <summary>
        /// Fetch User Access Right
        /// Call to ValidateUserAccessRight method with FunctionCode
        /// </summary>        
        private void LoadUserAccessRight()
        {
            List<AccessRight> accessRightList;
            if (AccessRightUtilities.ValidateUserAccessRight((DataTable)Session["UserAccessRights"], "EmailTemplate", out accessRightList))
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
            if(!IsPostBack)
            {
                LoadUserAccessRight();
                if (!String.IsNullOrEmpty(Request.QueryString["Id"]))
                {
                    Session["templateID"] = Request.QueryString["Id"];
                    templateID = Session["templateID"].ToString();          // templateID = Request.QueryString["Id"];                    
                    emailTemplateService = new EmailTemplateService(base.dbConnectionStr);
                    ds = emailTemplateService.RetrieveEmailTemplateByID(templateID);
                    if (ds.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString() == "1")
                    {
                        DataTable dtEmailTemplate = ds.Tables[0];
                        if (dtEmailTemplate.Rows.Count == 1)
                        {
                            foreach (DataRow row in dtEmailTemplate.Rows)
                            {
                                txtTemplateName.Text = row["TemplateName"].ToString();
                                if (row["TemplateType"].ToString() == "H")
                                {
                                    rdoText.Checked = false;
                                    rdoHtml.Checked = true;
                                }
                                else if (row["TemplateType"].ToString() == "T")
                                {
                                    rdoHtml.Checked = false;
                                    rdoText.Checked = true;
                                }
                                txtEmailSubject.Text = row["Subject"].ToString();
                                txtEmailContent.Text = row["Contents"].ToString();
                                if(!String.IsNullOrEmpty(row["ModifiedDate"].ToString()))
                                {
                                    lblModifiedTime.Text = String.Format("{0:g}",Convert.ToDateTime(row["ModifiedDate"].ToString()));
                                }
                            }
                            btnSave.Visible = false;
                            btnUpdate.Visible = true;
                            txtTemplateName.Enabled = false;
                        }
                    }
                    else
                    {
                        divMessage.InnerHtml = ds.Tables["ReturnTable"].Rows[0]["ReturnMessage"].ToString();
                    }
                }
            }           
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string[] wsReturn = null;     
                emailTemplateService = new EmailTemplateService(dbConnectionStr);
                string templateName = "";
                string templateType = "";
                string subject = "";
                string content = "";
                templateName = txtTemplateName.Text;
                if (!String.IsNullOrEmpty(templateName))
                {
                    if (rdoHtml.Checked == true)
                    {
                        templateType = "H";
                    }
                    else if (rdoText.Checked == true)
                    {
                        templateType = "T";
                    }
                    if (ValidateResult())
                    {
                        subject = txtEmailSubject.Text;
                        content = txtEmailContent.Text;

                        wsReturn = emailTemplateService.InsertEmailTemplate(templateName, templateType, subject, content, "new");
                        if (Convert.ToInt32(wsReturn[0]) > 1)
                        {
                            Response.Redirect("~/WebPages/EmailTemplate/EmailTemplate.aspx?x=1&msg=" + wsReturn[1].ToString());
                        }
                        else
                        {
                            divMessage.InnerHtml = wsReturn[1].ToString();
                        }
                    }
                }
            }
            catch
            {
            }
            finally
            {
                //divMessage.InnerHtml = wsReturn[1].ToString();
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/EmailTemplate/EmailTemplate.aspx");
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            string[] wsReturn = null;     
            emailTemplateService = new EmailTemplateService(dbConnectionStr);
            templateID = Session["templateID"].ToString();            
            string templateType = "";
            string subject = "";
            string content = "";
            if (!String.IsNullOrEmpty(templateID))
            {
                if (rdoHtml.Checked == true)
                {
                    templateType = "H";
                }
                else if (rdoText.Checked == true)
                {
                    templateType = "T";
                }
                if (ValidateResult())
                {
                    subject = txtEmailSubject.Text;
                    content = txtEmailContent.Text;

                    wsReturn = emailTemplateService.UpdateEmailTemplate(templateID, templateType, subject, content);
                    if (Convert.ToInt32(wsReturn[0]) == 1)
                    {
                        Response.Redirect("~/WebPages/EmailTemplate/EmailTemplate.aspx?x=1&msg=" + wsReturn[1].ToString());
                    }
                    else
                    {
                        divMessage.InnerHtml = wsReturn[1].ToString();
                    }
                }
            }           
        }

        private bool ValidateResult()
        {
            bool flag = false;
            divMessage.InnerHtml = "";
            if (String.IsNullOrEmpty(txtEmailSubject.Text))
            {
                divMessage.InnerHtml = "Subject cannot be blank!";
            }
            else if (String.IsNullOrEmpty(txtEmailContent.Text))
            {
                divMessage.InnerHtml = "Content cannot be blank!";
            }
            if (divMessage.InnerHtml == "")
            {
                flag = true;
            }
            return flag;
        }      
     
    }
}
