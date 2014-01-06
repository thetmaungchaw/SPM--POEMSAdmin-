/* 
 * Purpose:         Access Management Page Controller
 * Created By:      Li Qun
 * Date:            30/03/2010
 * 
 * Change History:
 * ---------------------------------------------------------------------------------
 * Modified By      Date        Purpose
 * ---------------------------------------------------------------------------------
 * Li Qun           08/04/2010  change to auto postback on user dropdown list change
 * Yin Mon Win      20/08/2011  Phase III
 * 
 */

using System;
using System.Collections;
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
using System.Collections.Generic;

namespace SPMWebApp.WebPages.AccessControl
{
    public partial class AccessManagement : BasePage.BasePage
    {
        private AccessControlService accessControlService;
        private String lastCategory = "";

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            accessControlService = new AccessControlService(base.dbConnectionStr);
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            LoadUserAccessRight();
                       

            base.gvList = this.gvList;
            base.divMessage = this.divMessage;

            if (!IsPostBack)
            {
                if (Session["UserId"] == null)
                {
                }

                btnSave.Visible = false;

                DataSet ds = null;
                ds = accessControlService.RetrieveUserIdAndName(base.userLoginId);

                if (ds.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString().Equals("1"))
                {
                   ddlUserCode.Items.Add(new ListItem("---Select User---", ""));
                    foreach(DataRow dr in ds.Tables[0].Rows)
                    {
                        ddlUserCode.Items.Add(new ListItem(dr["UserIdAndName"].ToString(), dr["UserId"].ToString()));
                    }
                }
                else
                {
                    divMessage.InnerHtml = ds.Tables["ReturnTable"].Rows[0]["ReturnMessage"].ToString();
                }

                
                ds = accessControlService.RetrieveUserRoles();

                //SearchAndDisplay();

                if (ds.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString().Equals("1"))
                {
                    ddlRoleName.Items.Add(new ListItem("---Select User Role---", ""));
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        ddlRoleName.Items.Add(new ListItem(dr["RoleName"].ToString(), dr["RoleId"].ToString()));
                    }
                }
                else
                {
                    divMessage.InnerHtml = ds.Tables["ReturnTable"].Rows[0]["ReturnMessage"].ToString();
                }
            }
        }

      

        /// <summary>
        /// Fetch User Access Right
        /// Call to ValidateUserAccessRight method with FunctionCode
        /// </summary>        
        private void LoadUserAccessRight()
        {
            List<AccessRight> accessRightList;
            DataSet dsUserAccessRight = AccessRightUtilities.RetrieveUserAccessRights(base.dbConnectionStr, base.userLoginId);
            if (dsUserAccessRight.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString().Equals("1"))
            {
                Session["UserAccessRights"] = dsUserAccessRight.Tables[0];
            }
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

        protected override void SearchAndDisplay()
        {
            base.SearchAndDisplay();
            SaveHide();          
        }

        #region The Old One
        /*
        protected override Hashtable RetrieveRecords()
        {
            Hashtable ht = new Hashtable();
            DataSet ds = null;
            int returnCode = 1;
            string returnMessage = "";

            ht.Add("ReturnData", null);

            String selectedUserId = ddlUserCode.SelectedValue;

            // Check if a user is selected
            if (String.IsNullOrEmpty(selectedUserId))
            {
                returnCode = -1;
                returnMessage = "Pls select a user!";
                ht.Add("ReturnCode", returnCode);
                ht.Add("ReturnMessage", returnMessage);
                return ht;
            }

            ds = accessControlService.RetrieveUserAccessRights(selectedUserId);
            if (ds == null)
            {
                returnCode = -1;
                returnMessage = "System Error - Connection Problem!";
            }
            else if (ds.Tables.Count == 0)
            {
                returnCode = -1;
                returnMessage = "System Error - Database Problem!";
            }
            else if (ds.Tables[0].Rows.Count > 0)
            {
                //ViewState["dtUserAccessRights"] = ds.Tables[0];
                // Create a column for new access rights
                ds.Tables[0].Columns.Add("NewRight", typeof(String));
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    row["NewRight"] = row["UserRight"].ToString();
                }
                ht["ReturnData"] = ds.Tables[0];
                returnCode = -1;
                returnMessage = "";
            }
            else
            {
                returnCode = -1;
                returnMessage = "No record found!";
            }

            ht.Add("ReturnCode", returnCode);
            ht.Add("ReturnMessage", returnMessage);
            return ht;
        }
         */
        #endregion

        protected void SaveHide()
        {
            if (gvList.Rows.Count > 0)
            {
                btnSave.Visible = true;
            }
            else
            {
                btnSave.Visible = false;
            }
        }

        protected override Hashtable RetrieveRecords()
        {
            Hashtable ht = new Hashtable();
            DataSet ds = null;
            int returnCode = 1;
            string returnMessage = "";

            ht.Add("ReturnData", null);
            String selectedUserId = ddlUserCode.SelectedValue;
            //String selectedRoleId = ddlRoleName.SelectedValue;

            // Check if a user is selected
            if (String.IsNullOrEmpty(selectedUserId))
            {
                returnCode = -1;
                returnMessage = "Pls select a user!";
                ht.Add("ReturnCode", returnCode);
                ht.Add("ReturnMessage", returnMessage);
                return ht;
            }

            ds = accessControlService.RetrieveUserAccessRights(selectedUserId);
            if (ds == null)
            {
                returnCode = -1;
                returnMessage = "System Error - Connection Problem!";
            }
            else if (ds.Tables.Count == 0)
            {
                returnCode = -1;
                returnMessage = "System Error - Database Problem!";
            }
            else if (ds.Tables[0].Rows.Count > 0)
            {
                //ViewState["dtUserAccessRights"] = ds.Tables[0];
                // Create a column for new access rights
                //ds.Tables[0].Columns.Add("NewRight", typeof(String));
                //foreach (DataRow row in ds.Tables[0].Rows)
                //{
                //    row["NewRight"] = row["UserRight"].ToString();
                //}
                ht["ReturnData"] = ds.Tables[0];
                returnCode = 1;
                returnMessage = "";

                ddlRoleName.Enabled= gvList.Enabled = btnSave.Enabled = (bool)ViewState["ModifyAccessRight"];
                if ((bool)ViewState["ModifyAccessRight"] == false)
                {
                    returnMessage = "No permission to edit. Existing Record cannot be modified.";
                }
                
            }
            else //New AccessRight
            {
                //ds = accessControlService.RetrieveUserRoleRights("");

                //if (ds.Tables[0].Rows.Count > 0)
                //{
                //    ht["ReturnData"] = ds.Tables[0];
                //    returnCode = 1;
                //    returnMessage = "";
                //}
                //else {
                //    returnCode = -1;
                //    returnMessage = "No record found!";

                //    ddlRoleName.SelectedIndex = 0;
                //}

                if ((bool)ViewState["CreateAccessRight"])
                {
                    returnMessage = "New Access Right!Please choose Role!";
                }
                else 
                {
                    returnMessage = "No record found. You have no permission to create access right.";
                    ddlRoleName.Enabled = false;
                }
                  
            }

            ht.Add("ReturnCode", returnCode);
            ht.Add("ReturnMessage", returnMessage);
            return ht;
        }

        protected Hashtable RetrieveRoleRecords()
        {
            Hashtable ht = new Hashtable();
            DataSet ds = null;
            int returnCode = 1;
            string returnMessage = "";

            ht.Add("ReturnData", null);

            String selectedRoleId = ddlRoleName.SelectedValue;
          

            ds = accessControlService.RetrieveUserRoleRights(selectedRoleId);
            if (ds == null)
            {
                returnCode = -1;
                returnMessage = "System Error - Connection Problem!";
            }
            else if (ds.Tables.Count == 0)
            {
                returnCode = -1;
                returnMessage = "System Error - Database Problem!";
            }
            else if (ds.Tables[0].Rows.Count > 0)
            {
                //ViewState["dtUserAccessRights"] = ds.Tables[0];
                // Create a column for new access rights
                //ds.Tables[0].Columns.Add("NewRight", typeof(String));
                //foreach (DataRow row in ds.Tables[0].Rows)
                //{
                //    row["NewRight"] = row["UserRight"].ToString();
                //}


                ht["ReturnData"] = ds.Tables[0];
                returnCode = 1;
                returnMessage = "";

                if (ds.Tables[0].Rows[0]["RoleID"].ToString()=="") // New Case
                { 
                    
                }
            }
            else
            {
                returnCode = -1;
                returnMessage = "No record found!";
            }

            ht.Add("ReturnCode", returnCode);
            ht.Add("ReturnMessage", returnMessage);
            return ht;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            
            DataTable dtUpdate = new DataTable();
            dtUpdate.Columns.Add("Function_Code", typeof(string));
            dtUpdate.Columns.Add("Action", typeof(String));
            dtUpdate.Columns.Add("CreateRight", typeof(string));
            dtUpdate.Columns.Add("ViewRight", typeof(string));
            dtUpdate.Columns.Add("ModifyRight", typeof(string));
            dtUpdate.Columns.Add("DeleteRight", typeof(string));
            dtUpdate.Columns.Add("UserRight", typeof(string));

            dtUpdate.Columns.Add("Status", typeof(string));

            foreach (GridViewRow gvRow in gvList.Rows)
            {

                CheckBox chkBxCreateAccess = (CheckBox)gvRow.FindControl("cbxCreateAccess");
                CheckBox chkBxModifyAccess = (CheckBox)gvRow.FindControl("cbxModifyAccess");
                CheckBox chkBxViewAccess = (CheckBox)gvRow.FindControl("cbxViewAccess");
                CheckBox chkBxDeleteAccess = (CheckBox)gvRow.FindControl("cbxDeleteAccess");


                HiddenField hidCreateAccess = (HiddenField)gvRow.FindControl("hidCreateAccess");
                HiddenField hidModifyAccess = (HiddenField)gvRow.FindControl("hidModifyAccess");
                HiddenField hidViewAccess = (HiddenField)gvRow.FindControl("hidViewAccess");
                HiddenField hidDeleteAccess = (HiddenField)gvRow.FindControl("hidDeleteAccess");

                HiddenField hidCreateOld = (HiddenField)gvRow.FindControl("hidCreateOld");
                HiddenField hidModifyOld = (HiddenField)gvRow.FindControl("hidModifyOld");
                HiddenField hidViewOld = (HiddenField)gvRow.FindControl("hidViewOld");
                HiddenField hidDeleteOld = (HiddenField)gvRow.FindControl("hidDeleteOld");

                DataRow drUpdate = dtUpdate.NewRow();
                //drUpdate["Function_Code"] = gvRow.Cells[1].Text;
                drUpdate["Function_Code"] = gvList.DataKeys[gvRow.RowIndex].Values[0].ToString();
                drUpdate["Action"] = "A";

                if (chkBxCreateAccess != null && chkBxCreateAccess.Checked)
                {
                    //drUpdate["CreateRight"] = true;
                    drUpdate["CreateRight"] = "Y";
                }
                else
                {
                    // drUpdate["CreateRight"] = false;
                    if (hidCreateAccess.Value == "D")
                    {
                        drUpdate["CreateRight"] = "D";
                        
                    }
                    else
                    {
                        drUpdate["CreateRight"] = "N";
                    }
                }
                if (chkBxModifyAccess != null && chkBxModifyAccess.Checked)
                {
                    //drUpdate["ModifyRight"] = true;
                    drUpdate["ModifyRight"] = "Y";
                }
                else
                {
                    //drUpdate["ModifyRight"] = false;
                    if (hidModifyAccess.Value == "D")
                    {
                        drUpdate["ModifyRight"] = "D";
                    }
                    else
                    {
                        drUpdate["ModifyRight"] = "N";
                    }
                }
                if (chkBxViewAccess != null && chkBxViewAccess.Checked)
                {
                    //drUpdate["ViewRight"] = true;
                    drUpdate["ViewRight"] = "Y";
                }
                else
                {
                    //drUpdate["ViewRight"] = false;
                    if (hidViewAccess.Value == "D")
                    {
                        drUpdate["ViewRight"] = "D";
                    }
                    else
                    {
                        drUpdate["ViewRight"] = "N";
                    }
                }
                if (chkBxDeleteAccess != null && chkBxDeleteAccess.Checked)
                {
                    //drUpdate["DeleteRight"] = true;
                    drUpdate["DeleteRight"] = "Y";
                }
                else
                {
                    //drUpdate["DeleteRight"] = false;
                    if (hidDeleteAccess.Value == "D")
                    {
                        drUpdate["DeleteRight"] = "D";
                    }
                    else
                    {
                        drUpdate["DeleteRight"] = "N";
                    }
                }

                /// <Updated by OC>
                //if (drUpdate["CreateRight"] == "N" && drUpdate["ModifyRight"] == "N" && drUpdate["ViewRight"] == "N" && drUpdate["DeleteRight"] == "N")
                //{
                //    drUpdate["UserRight"] = "N";
                //}
                if (drUpdate["CreateRight"].ToString() != "Y" && drUpdate["ModifyRight"].ToString() != "Y" && drUpdate["ViewRight"].ToString() != "Y" && drUpdate["DeleteRight"].ToString() != "Y")
                {
                    drUpdate["UserRight"] = "N";
                }
                else  //if (drUpdate["CreateRight"] != "N" || drUpdate["ModifyRight"] != "N" || drUpdate["ViewRight"] == "N" && drUpdate["DeleteRight"] == "N")
                {
                    if (hidCreateAccess.Value == "D" && hidModifyAccess.Value == "D" && hidViewAccess.Value == "D" && hidDeleteAccess.Value != "D")
                    {
                        if (Convert.ToBoolean(hidDeleteOld.Value) == chkBxDeleteAccess.Checked)
                        {
                            drUpdate["Status"] = "UC";
                        }
                        else
                        {
                            drUpdate["Status"] = "C";
                        }
                    }
                    else if (hidCreateAccess.Value == "D" && hidModifyAccess.Value == "D" && hidViewAccess.Value != "D" && hidDeleteAccess.Value == "D")
                    {
                        if (Convert.ToBoolean(hidViewOld.Value) == chkBxViewAccess.Checked)
                        {
                            drUpdate["Status"] = "UC";
                        }
                        else
                        {
                            drUpdate["Status"] = "C";
                        }
                    }
                    else if (hidCreateAccess.Value == "D" && hidModifyAccess.Value == "D" && hidViewAccess.Value != "D" && hidDeleteAccess.Value != "D")
                    {
                        if (Convert.ToBoolean(hidViewOld.Value) == chkBxViewAccess.Checked && Convert.ToBoolean(hidDeleteOld.Value) == chkBxDeleteAccess.Checked)
                        {
                            drUpdate["Status"] = "UC";
                        }
                        else
                        {
                            drUpdate["Status"] = "C";
                        }
                    }
                    else if (hidCreateAccess.Value == "D" && hidModifyAccess.Value != "D" && hidViewAccess.Value == "D" && hidDeleteAccess.Value == "D")
                    {
                        if (Convert.ToBoolean(hidModifyOld.Value) == chkBxModifyAccess.Checked)
                        {
                            drUpdate["Status"] = "UC";
                        }
                        else
                        {
                            drUpdate["Status"] = "C";
                        }
                    }
                    else if (hidCreateAccess.Value == "D" && hidModifyAccess.Value != "D" && hidViewAccess.Value == "D" && hidDeleteAccess.Value != "D")
                    {
                        if (Convert.ToBoolean(hidModifyOld.Value) == chkBxModifyAccess.Checked && Convert.ToBoolean(hidDeleteOld.Value) == chkBxDeleteAccess.Checked)
                        {
                            drUpdate["Status"] = "UC";
                        }
                        else
                        {
                            drUpdate["Status"] = "C";
                        }
                    }

                    else if (hidCreateAccess.Value == "D" && hidModifyAccess.Value != "D" && hidViewAccess.Value != "D" && hidDeleteAccess.Value == "D")
                    {
                        if (Convert.ToBoolean(hidModifyOld.Value) == chkBxModifyAccess.Checked && Convert.ToBoolean(hidViewOld.Value) == chkBxViewAccess.Checked)
                        {
                            drUpdate["Status"] = "UC";
                        }
                        else
                        {
                            drUpdate["Status"] = "C";
                        }
                    }
                    else if (hidCreateAccess.Value == "D" && hidModifyAccess.Value != "D" && hidViewAccess.Value != "D" && hidDeleteAccess.Value != "D")
                    {
                        if (Convert.ToBoolean(hidModifyOld.Value) == chkBxModifyAccess.Checked && Convert.ToBoolean(hidViewOld.Value) == chkBxViewAccess.Checked && Convert.ToBoolean(hidDeleteOld.Value) == chkBxDeleteAccess.Checked)
                        {
                            drUpdate["Status"] = "UC";
                        }
                        else
                        {
                            drUpdate["Status"] = "C";
                        }
                    }
                    else if (hidCreateAccess.Value != "D" && hidModifyAccess.Value == "D" && hidViewAccess.Value == "D" && hidDeleteAccess.Value == "D")
                    {
                        if (Convert.ToBoolean(hidCreateOld.Value) == chkBxCreateAccess.Checked)
                        {
                            drUpdate["Status"] = "UC";
                        }
                        else
                        {
                            drUpdate["Status"] = "C";
                        }
                    }
                    else if (hidCreateAccess.Value != "D" && hidModifyAccess.Value == "D" && hidViewAccess.Value == "D" && hidDeleteAccess.Value != "D")
                    {
                        if (Convert.ToBoolean(hidCreateOld.Value) == chkBxCreateAccess.Checked && Convert.ToBoolean(hidDeleteOld.Value) == chkBxDeleteAccess.Checked)
                        {
                            drUpdate["Status"] = "UC";
                        }
                        else
                        {
                            drUpdate["Status"] = "C";
                        }
                    }
                    else if (hidCreateAccess.Value != "D" && hidModifyAccess.Value == "D" && hidViewAccess.Value != "D" && hidDeleteAccess.Value == "D")
                    {
                        if (Convert.ToBoolean(hidCreateOld.Value) == chkBxCreateAccess.Checked && Convert.ToBoolean(hidViewOld.Value) == chkBxViewAccess.Checked)
                        {
                            drUpdate["Status"] = "UC";
                        }
                        else
                        {
                            drUpdate["Status"] = "C";
                        }
                    }
                    else if (hidCreateAccess.Value != "D" && hidModifyAccess.Value == "D" && hidViewAccess.Value != "D" && hidDeleteAccess.Value != "D")
                    {
                        if (Convert.ToBoolean(hidCreateOld.Value) == chkBxCreateAccess.Checked && Convert.ToBoolean(hidViewOld.Value) == chkBxViewAccess.Checked && Convert.ToBoolean(hidDeleteOld.Value) == chkBxDeleteAccess.Checked)
                        {
                            drUpdate["Status"] = "UC";
                        }
                        else
                        {
                            drUpdate["Status"] = "C";
                        }
                    }
                    else if (hidCreateAccess.Value != "D" && hidModifyAccess.Value != "D" && hidViewAccess.Value == "D" && hidDeleteAccess.Value == "D")
                    {
                        if (Convert.ToBoolean(hidCreateOld.Value) == chkBxCreateAccess.Checked && Convert.ToBoolean(hidModifyOld.Value) == chkBxModifyAccess.Checked)
                        {
                            drUpdate["Status"] = "UC";
                        }
                        else
                        {
                            drUpdate["Status"] = "C";
                        }
                    }
                    else if (hidCreateAccess.Value != "D" && hidModifyAccess.Value != "D" && hidViewAccess.Value == "D" && hidDeleteAccess.Value != "D")
                    {
                        if (Convert.ToBoolean(hidCreateOld.Value) == chkBxCreateAccess.Checked && Convert.ToBoolean(hidModifyOld.Value) == chkBxModifyAccess.Checked && Convert.ToBoolean(hidDeleteOld.Value) == chkBxDeleteAccess.Checked)
                        {
                            drUpdate["Status"] = "UC";
                        }
                        else
                        {
                            drUpdate["Status"] = "C";
                        }
                    }
                    else if (hidCreateAccess.Value != "D" && hidModifyAccess.Value != "D" && hidViewAccess.Value != "D" && hidDeleteAccess.Value == "D")
                    {
                        if (Convert.ToBoolean(hidCreateOld.Value) == chkBxCreateAccess.Checked && Convert.ToBoolean(hidModifyOld.Value) == chkBxModifyAccess.Checked && Convert.ToBoolean(hidViewOld.Value) == chkBxViewAccess.Checked)
                        {
                            drUpdate["Status"] = "UC";
                        }
                        else
                        {
                            drUpdate["Status"] = "C";
                        }
                    }
                    else if (hidCreateAccess.Value != "D" && hidModifyAccess.Value != "D" && hidViewAccess.Value != "D" && hidDeleteAccess.Value != "D")
                    {
                        if (Convert.ToBoolean(hidCreateOld.Value) == chkBxCreateAccess.Checked && Convert.ToBoolean(hidModifyOld.Value) == chkBxModifyAccess.Checked && Convert.ToBoolean(hidViewOld.Value) == chkBxViewAccess.Checked && Convert.ToBoolean(hidDeleteOld.Value) == chkBxDeleteAccess.Checked)
                        {
                            drUpdate["Status"] = "UC";
                        }
                        else
                        {
                            drUpdate["Status"] = "C";
                        }
                    }


                    drUpdate["UserRight"] = "Y";
                }

                dtUpdate.Rows.Add(drUpdate);

            }

            String selectedRoleId = ddlRoleName.SelectedValue;
            String selectedUserId = ddlUserCode.SelectedValue;
                     
            DataSet dsUserAccessRigthUpdate = new DataSet();
            dsUserAccessRigthUpdate.Tables.Add(dtUpdate);
            DataSet dsReturn = accessControlService.UpdateUserAccessRights(selectedUserId, selectedRoleId, dsUserAccessRigthUpdate, base.userLoginId);

            if (dsReturn == null)
            {
                divMessage.InnerHtml = "Error calling webservice!";
            }
            else
            {
                //SearchAndDisplay();
                this.ClearRoleForm();

                divMessage.InnerHtml = dsReturn.Tables[0].Rows[0]["ReturnMessage"].ToString();
            }          



            #region Old
            /*
            DataTable dtUpdate = new DataTable();
            dtUpdate.Columns.Add("Function_Code", typeof(string));
            dtUpdate.Columns.Add("Action", typeof(String));
            int changeCnts = 0;
            foreach (GridViewRow gvRow in gvList.Rows)
            {
                Boolean orgAccess = false;
                Boolean newAccess = false;

                CheckBox chkBxOrgAccess = (CheckBox)gvRow.FindControl("cbxOrgAccess");
                CheckBox chkBxNewAccess = (CheckBox)gvRow.FindControl("cbxNewAccess"); 
 
                if (chkBxOrgAccess != null && chkBxOrgAccess.Checked) 
                { 
                    orgAccess = true;
                } 
                if (chkBxNewAccess != null && chkBxNewAccess.Checked) 
                { 
                    newAccess = true;
                }

                if (orgAccess != newAccess)
                {
                    changeCnts += 1;
                    DataRow drUpdate = dtUpdate.NewRow();
                    //drUpdate["Function_Code"] = gvRow.Cells[1].Text;
                    drUpdate["Function_Code"] = gvList.DataKeys[gvRow.RowIndex].Values[0].ToString();
                    if (newAccess)
                    {
                        drUpdate["Action"] = "A";
                    }
                    else
                    {
                        drUpdate["Action"] = "D";
                    }
                    dtUpdate.Rows.Add(drUpdate);
                }
            }
            if (changeCnts == 0)
            {
                divMessage.InnerHtml = "No Change to save!";
            }
            else
            {
                //divMessage.InnerHtml = "Changes: " + changeCnts;
                //DataSet dsUpdate = new DataSet();
                //dsUpdate.Tables.Add(dtUpdate);
                String selectedUserId = ddlUserCode.SelectedValue;
                DataSet dsUserAccessRightsUpdate = new DataSet();
                dsUserAccessRightsUpdate.Tables.Add(dtUpdate);
                DataSet dsReturn = accessControlService.UpdateUserAccessRights(selectedUserId, dsUserAccessRightsUpdate, base.userLoginId);

                if (dsReturn == null)
                {
                    divMessage.InnerHtml = "Error calling webservice!";
                }
                else
                {
                    if (dsReturn.Tables[0].Rows[0]["ReturnCode"].ToString() == "1")
                    {
                        // Set the same check box
                        foreach (GridViewRow gvRow in gvList.Rows)
                        {
                            CheckBox chkBxOrgAccess = (CheckBox)gvRow.FindControl("cbxOrgAccess");
                            CheckBox chkBxNewAccess = (CheckBox)gvRow.FindControl("cbxNewAccess");
                            if (chkBxOrgAccess != null && chkBxNewAccess != null)
                            {
                                chkBxOrgAccess.Checked = chkBxNewAccess.Checked;
                            }

                        }

                    }
                    divMessage.InnerHtml = dsReturn.Tables[0].Rows[0]["ReturnMessage"].ToString();
                }
            }
             */
            #endregion

            #region Added by OC

            //CommonService CommonService = new CommonService(base.dbConnectionStr);
            //DataSet dsDealerDetail = CommonService.RetrieveDealerCodeAndNameByUserID(Session["UserId"].ToString());

            //String ErrMsg = accessControlService.ChangeUserRight(selectedUserId, dbConnectionStr);

            //if (ErrMsg == String.Empty)
            //{
            //    divMessage.InnerHtml = dsReturn.Tables[0].Rows[0]["ReturnMessage"].ToString();
            //}
            //else
            //{
            //    divMessage.InnerHtml = "Error in calling functions";
            //}

            #endregion
        }

        protected void gvList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[0].Text == lastCategory)
                {
                    e.Row.Cells[0].Text = "";
                }
                else
                {
                    lastCategory = e.Row.Cells[0].Text;
                }

                HiddenField hidCreateAccess = (HiddenField)e.Row.FindControl("hidCreateAccess");
                HiddenField hidModifyAccess = (HiddenField)e.Row.FindControl("hidModifyAccess");
                HiddenField hidViewAccess = (HiddenField)e.Row.FindControl("hidViewAccess");
                HiddenField hidDeleteAccess = (HiddenField)e.Row.FindControl("hidDeleteAccess");

                CheckBox chkBxCreateAccess = (CheckBox)e.Row.FindControl("cbxCreateAccess");
                CheckBox chkBxModifyAccess = (CheckBox)e.Row.FindControl("cbxModifyAccess");
                CheckBox chkBxViewAccess = (CheckBox)e.Row.FindControl("cbxViewAccess");
                CheckBox chkBxDeleteAccess = (CheckBox)e.Row.FindControl("cbxDeleteAccess");

                if (hidCreateAccess.Value == "D")
                {
                    chkBxCreateAccess.Enabled = false;
                }
                if (hidModifyAccess.Value == "D")
                {
                    chkBxModifyAccess.Enabled = false;
                }
                if (hidViewAccess.Value == "D")
                {
                    chkBxViewAccess.Enabled = false;
                }
                if (hidDeleteAccess.Value == "D")
                {
                    chkBxDeleteAccess.Enabled = false;
                }
            }

            
        }

        protected void ddlUserCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dtReturnData = new DataTable();

            SearchAndDisplay();

            Hashtable ht = RetrieveRecords();
            
            if (ht["ReturnData"] != null)
            {
                dtReturnData = (DataTable)ht["ReturnData"];
                ddlRoleName.SelectedValue = dtReturnData.Rows[0]["RoleId"].ToString();
            }

        }
              

        #region SPM III

        protected void ddlRoleName_SelectedIndexChanged(object sender, EventArgs e)
        {
            //SearchAndDisplay();

            Hashtable ht = RetrieveRoleRecords();
            DataTable dtReturnData = new DataTable();

            if (ht["ReturnData"] != null)
            {
                dtReturnData = (DataTable)ht["ReturnData"];
            }

            gvList.DataSource = null;
            gvList.PageIndex = 0;
            gvList.DataSource = dtReturnData;
            gvList.DataBind();

            SaveHide();

            divMessage.InnerHtml = ht["ReturnMessage"].ToString();
        }

        private void ClearRoleForm()
        {            
            ddlRoleName.SelectedIndex = 0;
            ddlUserCode.SelectedIndex = 0;

            gvList.DataSource = null;
            gvList.PageIndex = 0;
            gvList.DataBind();

            SaveHide();

            
       
        }
        
        #endregion

        protected void gvList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


    }
}
