using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SPMWebApp.Services;
using System.Data;

using SPMWebApp.Utilities;

namespace SPMWebApp
{
    public partial class EmailTemplate : BasePage.BasePage
    {
        private EmailTemplateService emailTemplateService;  
     
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

        protected void checkAccessRight()
        {
            try
            {
                addTemplate.Enabled = (bool)ViewState["CreateAccessRight"];
            }
            catch (Exception e) { }
        }
       
        protected void Page_Load(object sender, EventArgs e)
        {
            divMessage.InnerHtml = "";
            

            if (!IsPostBack)
            {
                LoadUserAccessRight();

                int? x;
                x = Convert.ToInt32(Request.QueryString["x"]);
                if(!String.IsNullOrEmpty(x.ToString()) && (x!=0))
                {
                    divMessage.InnerHtml = Convert.ToString(Request.QueryString["msg"]);
                }                
            }
            try
            {
                BindData();
            }
            catch (Exception ex)
            {
            }
            finally
            {
            }
           
        }

        private void BindData()
        {
            emailTemplateService = new EmailTemplateService(base.dbConnectionStr);
            grvItemTemplate.DataSource = emailTemplateService.RetrieveEmailTemplate();
            grvItemTemplate.DataBind();
        }       

        protected void grvItemTemplate_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
             
            try
            {
                DataSet ds = null;
                string templateID = grvItemTemplate.DataKeys[e.RowIndex].Value.ToString();
                emailTemplateService = new EmailTemplateService(dbConnectionStr);
                ds = emailTemplateService.DeleteTemplate(templateID);
                if (ds.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString() == "1")
                {
                    divMessage.InnerHtml = ds.Tables["ReturnTable"].Rows[0]["ReturnMessage"].ToString();
                }
                else
                {
                    divMessage.InnerHtml = ds.Tables["ReturnTable"].Rows[0]["ReturnMessage"].ToString();
                }
                BindData();
            }
            catch
            {
            }
            finally
            {
            }
        }

        protected void grvItemTemplate_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                int templateID = Convert.ToInt32(grvItemTemplate.DataKeys[e.NewEditIndex].Value);
                Response.Redirect("~/WebPages/EmailTemplate/AddEditEmailTemplate.aspx?ID=" + templateID);
            }
            catch
            {
            }
            finally
            {
            }
        }

        protected void addTemplate_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/EmailTemplate/AddEditEmailTemplate.aspx");
        }

        protected void grvItemTemplate_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[2].Enabled = (bool)ViewState["ModifyAccessRight"];
                e.Row.Cells[3].Enabled = (bool)ViewState["DeleteAccessRight"];

                string templateID = "";
                //templateID = e.Row.Cells[0].Text;
                if(!String.IsNullOrEmpty(DataBinder.Eval(e.Row.DataItem,"TemplateID").ToString()))
                {
                   templateID = DataBinder.Eval(e.Row.DataItem,"TemplateID").ToString();
                   if (templateID == "1" || templateID == "2" || templateID == "3")
                   {
                       e.Row.Cells[3].Enabled = false;
                   }
                } 
            }
            
        }    
    }
}
