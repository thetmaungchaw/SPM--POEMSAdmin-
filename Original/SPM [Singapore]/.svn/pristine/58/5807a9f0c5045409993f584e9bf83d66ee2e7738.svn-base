/* 
 * Purpose:         Role Management Page Controller
 * Created By:      Yin Mon Win
 * Date:            05/08/2011
 * 
 * Change History:
 * ---------------------------------------------------------------------------------
 * Modified By      Date            Purpose
 * 
 * ---------------------------------------------------------------------------------
 * 
 * 
 */



using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using SPMWebApp.Services;
using SPMWebApp.BasePage;
using SPMWebApp.Utilities;
using System.Collections.Generic;

namespace SPMWebApp.WebPages.AccessControl
{
    public partial class RoleManagement : BasePage.BasePage
    {
        private AccessControlService accessControlService;

        private String lastCategory = ""; private string flag = "";

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            accessControlService = new AccessControlService(base.dbConnectionStr);
        }

        protected void checkAccessRight()
        {
            try
            {
                gvList.Enabled = (bool)ViewState["CreateAccessRight"];
                chkCreateHeader.Enabled = (bool)ViewState["CreateAccessRight"];
                chkCreateHeader.Checked = (bool)ViewState["CreateAccessRight"];
                fsCreate.Disabled = !(bool)ViewState["CreateAccessRight"];
                txtRoleName.Enabled = (bool)ViewState["CreateAccessRight"];
                txtRoleDes.Enabled = (bool)ViewState["CreateAccessRight"];
            }
            catch (Exception e) { }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
                     
            base.gvList = this.gvList;
            base.divMessage = this.divMessage;
            
            if (!IsPostBack)
            {
                LoadUserAccessRight();

                checkAccessRight();

                if (Session["UserId"] == null)
                {
                }

                // btnSave.Visible = false;
                // btnSave.Enabled = false;

                if (chkModifyHeader.Checked)
                {
                    flag = "modify";
                }
                else
                {
                    flag = "create";
                }
                
                DataSet ds = null;
                ds = accessControlService.RetrieveUserRoles();

                SearchAndDisplay();

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
            if (AccessRightUtilities.ValidateUserAccessRight((DataTable)Session["UserAccessRights"], "RoleManagement", out accessRightList))
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

        protected void btnSave_Click(object sender, EventArgs e)
        {
            
            DataTable dtUpdate = new DataTable();
            dtUpdate.Columns.Add("Function_Code", typeof(string));
            dtUpdate.Columns.Add("Action", typeof(String));
            dtUpdate.Columns.Add("CreateRight", typeof(string));
            dtUpdate.Columns.Add("ViewRight", typeof(string));
            dtUpdate.Columns.Add("ModifyRight", typeof(string));
            dtUpdate.Columns.Add("DeleteRight", typeof(string));
            int changeCnts = 0;
            
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
               
                dtUpdate.Rows.Add(drUpdate);
               
            }
         
            String selectedRoleId = "";//ddlUserCode.SelectedValue;
            String RoleName = ""; String RoleDes = ""; string num = "0"; //string validateResult = "ok";
            DataSet  ds = new DataSet ();

            //Validate Contact Entry Form
            string validationResult = ValidateContactEntryForm();

            if (validationResult == "ok")
            {

                ds = accessControlService.RetrieveMaxUserRoleID();

                if (ds.Tables[0].Rows.Count > 0)
                {
                    num = ds.Tables[0].Rows[0]["MaxRoleID"].ToString();
                }
                selectedRoleId = getNewID(num);
                RoleName = txtRoleName.Text;
                RoleDes = txtRoleDes.Text;

                DataSet dsUserRoleUpdate = new DataSet();
                dsUserRoleUpdate.Tables.Add(dtUpdate);
                DataSet dsReturn = accessControlService.UpdateUserRole(selectedRoleId, RoleName, RoleDes, dsUserRoleUpdate, base.userLoginId);

                if (dsReturn == null)
                {
                    divMessage.InnerHtml = "Error calling webservice!";
                }
                else
                {
                    //Rebind RoleName DropDown List
                    ReBindRoleNameddl();

                    SearchAndDisplay();
                    this.ClearRoleForm();

                    divMessage.InnerHtml = dsReturn.Tables[0].Rows[0]["ReturnMessage"].ToString();
                }
            }
            else 
            {
                divMessage.InnerHtml = validationResult;
            }
        }

        private string ValidateContactEntryForm()
        {
            string result = "ok";

            DataSet ds = accessControlService.RetrieveRoleNames(txtRoleName.Text.Trim());

            if (ds.Tables[0].Rows.Count > 0)
            {
                result = "Role Name cannot be duplicated.";
            }
                       
                       
            return result;
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

                CheckBox chkBxCreateAccess = (CheckBox)e.Row .FindControl("cbxCreateAccess");
                CheckBox chkBxModifyAccess = (CheckBox)e.Row .FindControl("cbxModifyAccess");
                CheckBox chkBxViewAccess = (CheckBox)e.Row .FindControl("cbxViewAccess");
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

        protected void ddlRoleName_SelectedIndexChanged(object sender, EventArgs e)
        {
            SearchAndDisplay();

        }

        protected override void SearchAndDisplay()
        {
            base.SearchAndDisplay();
            if (gvList.Rows.Count > 0)
            {
                if (chkModifyHeader.Checked)
                {
                    btnSave.Enabled = false;
                }
                else
                {
                    btnSave.Enabled = true;
                }
            }
            else
            {
                btnSave.Enabled= false;
            }
        }

        protected string getNewID(string prevID)
        {
            int lenNumbers = 4;
            string prefix = "Role";
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

        protected override Hashtable RetrieveRecords()
        {
            Hashtable ht = new Hashtable();
            DataSet ds = null;
            int returnCode = 1;
            string returnMessage = "";

            ht.Add("ReturnData", null);

            String selectedRoleId = ddlRoleName.SelectedValue;

            //if (flag == "modify")
            //{
            //    // Check if a user is selected
            //    if (String.IsNullOrEmpty(selectedRoleId))
            //    {
            //        returnCode = -1;
            //        returnMessage = "Pls select a user role!";
            //        ht.Add("ReturnCode", returnCode);
            //        ht.Add("ReturnMessage", returnMessage);
            //        return ht;
            //    }
            //}
            if (selectedRoleId != "")
            {
                ds = accessControlService.RetrieveUserRoles(selectedRoleId);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    // ht["ReturnData"] = ds.Tables[0];

                    txtRoleDes2.Text = ds.Tables[0].Rows[0]["RoleDesc"].ToString();
                }
            }
           
            ds = null;

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
                //ds.Tables[0].Columns.Add("hidCreate", typeof(String));
                //ds.Tables[0].Columns.Add("hidModify", typeof(String));
                //ds.Tables[0].Columns.Add("hidView", typeof(String));
                //ds.Tables[0].Columns.Add("hidDelete", typeof(String));
                //foreach (DataRow row in ds.Tables[0].Rows)
                //{
                //    if (row["CreateRight"].ToString() == "D" )
                //    {
                //        row["hidCreate"] = "Disable";
                //    }
                //    if (row["ModifyRight"].ToString() == "D")
                //    {
                //        row["hidModify"] = "Disable";
                //    }
                //    if (row["ViewRight"].ToString() == "D")
                //    {
                //        row["hidView"] = "Disable";
                //    }
                //    if (row["DeleteRight"].ToString() == "D")
                //    {
                //        row["hidDelete"] = "Disable";
                //    }
                //}
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

        protected void chkModifyHeader_CheckedChanged(object sender, EventArgs e)
        {
            divMessage.InnerHtml = "";
            if (chkModifyHeader.Checked)
            {
                ddlRoleName.Enabled = true;
                txtRoleDes2.Enabled = true;
                btnUpdate.Enabled = true;
                btnDelete.Enabled = true;

                gvList.Enabled = true;
                chkCreateHeader.Checked = false;
                txtRoleName.Enabled = false;
                txtRoleDes.Enabled = false;
                btnSave.Enabled = false;
                //fsCreate.Disabled = true;

                btnUpdate.Enabled = (bool)ViewState["ModifyAccessRight"];
                btnDelete.Enabled = (bool)ViewState["DeleteAccessRight"];
            }
            else {
                ddlRoleName.Enabled = false;
                txtRoleDes2.Enabled = false;
                btnUpdate.Enabled = false;
                btnDelete.Enabled = false;

                //chkCreateHeader.Checked = true;

                gvList.Enabled = false;
            }
        }

        protected void chkCreateHeader_CheckedChanged(object sender, EventArgs e)
        {
            divMessage.InnerHtml = "";
            if (chkCreateHeader.Checked)
            {
                txtRoleDes.Enabled = true;
                txtRoleName.Enabled = true;
                btnSave.Enabled = true;

                gvList.Enabled = true;

                chkModifyHeader.Checked = false;
                txtRoleDes2.Enabled = false;
                ddlRoleName.Enabled = false;
                btnUpdate.Enabled = false;
                btnDelete.Enabled = false;
            }
            else
            {
                txtRoleName.Enabled = false;
                txtRoleDes.Enabled = false;
                btnSave.Enabled = false;

                //chkModifyHeader.Checked = true;

                gvList.Enabled = false;
            }
        }
        
        private string ValidateRoleForm()
        {
            string validateResult = "";

            if (String.IsNullOrEmpty(txtRoleName.Text.Trim()))
            {
                validateResult = "Role Name cannot be blank!";
            }            
            else if (String.IsNullOrEmpty(txtRoleDes.Text.Trim()))
            {
                validateResult = "Role Description cannot be blank!";
            }
            
            return validateResult;
        }

        private void ClearRoleForm()
        {
            txtRoleName.Text = txtRoleDes.Text = txtRoleDes2.Text = "";
            ddlRoleName.SelectedIndex = 0;
            //chkAssign.Checked = chkCrossGroup.Checked = chkSupervisior.Checked = false;
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            DataTable dtUpdate = new DataTable();
            dtUpdate.Columns.Add("Function_Code", typeof(string));
            dtUpdate.Columns.Add("Action", typeof(String));
            dtUpdate.Columns.Add("CreateRight", typeof(string));
            dtUpdate.Columns.Add("ViewRight", typeof(string));
            dtUpdate.Columns.Add("ModifyRight", typeof(string));
            dtUpdate.Columns.Add("DeleteRight", typeof(string));
           // int changeCnts = 0;

            foreach (GridViewRow gvRow in gvList.Rows)
            {
                
                CheckBox chkBxCreateAccess = (CheckBox)gvRow.FindControl("cbxCreateAccess");
                CheckBox chkBxModifyAccess = (CheckBox)gvRow.FindControl("cbxModifyAccess");
                CheckBox chkBxViewAccess = (CheckBox)gvRow.FindControl("cbxViewAccess");
                CheckBox chkBxDeleteAccess = (CheckBox)gvRow.FindControl("cbxDeleteAccess");

                /// <Added by OC>
                HiddenField hidCreateAccess = (HiddenField)gvRow.FindControl("hidCreateAccess");
                HiddenField hidModifyAccess = (HiddenField)gvRow.FindControl("hidModifyAccess");
                HiddenField hidViewAccess = (HiddenField)gvRow.FindControl("hidViewAccess");
                HiddenField hidDeleteAccess = (HiddenField)gvRow.FindControl("hidDeleteAccess");
               
                DataRow drUpdate = dtUpdate.NewRow();
                
                drUpdate["Function_Code"] = gvList.DataKeys[gvRow.RowIndex].Values[0].ToString();
                drUpdate["Action"] = "U";

                if (chkBxCreateAccess != null && chkBxCreateAccess.Checked)
                {
                    //drUpdate["CreateRight"] = true;
                    drUpdate["CreateRight"] = "Y";
                }
                else
                {
                    // drUpdate["CreateRight"] = false;
                    drUpdate["CreateRight"] = "N";

                    if (hidCreateAccess.Value == "D")
                    {
                        drUpdate["CreateRight"] = "D";
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
                    drUpdate["ModifyRight"] = "N";

                    if (hidModifyAccess.Value == "D")
                    {
                        drUpdate["ModifyRight"] = "D";
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
                    drUpdate["ViewRight"] = "N";

                    if (hidViewAccess.Value == "D")
                    {
                        drUpdate["ViewRight"] = "D";
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
                    drUpdate["DeleteRight"] = "N";

                    if (hidDeleteAccess.Value == "D")
                    {
                        drUpdate["DeleteRight"] = "D";
                    }
                }

                dtUpdate.Rows.Add(drUpdate);
            }

            
            String selectedRoleId = ddlRoleName.SelectedValue;
            //String RoleName = ""; 
            String RoleDes = "";
            DataSet ds = new DataSet();
          
            //RoleName = txtRoleName.Text;
            RoleDes = txtRoleDes2.Text;

            DataSet dsUserRoleUpdate = new DataSet();
            dsUserRoleUpdate.Tables.Add(dtUpdate);
            DataSet dsReturn = accessControlService.UpdateUserRole(selectedRoleId, "", RoleDes, dsUserRoleUpdate, base.userLoginId);

            if (dsReturn == null)
            {
                divMessage.InnerHtml = "Error calling webservice!";
            }
            else
            {
                //Rebind Role Name DropDownList
                ReBindRoleNameddl();

                SearchAndDisplay();

                this.ClearRoleForm();

                divMessage.InnerHtml = dsReturn.Tables[0].Rows[0]["ReturnMessage"].ToString();
            }
        }        

        protected void btnDelete_Click(object sender, EventArgs e)
        {          
            String selectedRoleId = ddlRoleName.SelectedValue;

            DataSet dsAssignedRole = new DataSet();
            //dsAssignedRole.Tables.Add(dtUpdate);
            dsAssignedRole  = accessControlService.RetrieveAssignedRole(selectedRoleId);

            if (dsAssignedRole.Tables[0].Rows.Count > 0)
            {
                ReBindRoleNameddl();
                SearchAndDisplay();
                this.ClearRoleForm();

                divMessage.InnerHtml = "The selected role cannot be deleted and it has been assigned.";
            }
            else
            {

                DataTable dtUpdate = new DataTable();
                dtUpdate.Columns.Add("Function_Code", typeof(string));
                dtUpdate.Columns.Add("Action", typeof(String));
                dtUpdate.Columns.Add("CreateRight", typeof(string));
                dtUpdate.Columns.Add("ViewRight", typeof(string));
                dtUpdate.Columns.Add("ModifyRight", typeof(string));
                dtUpdate.Columns.Add("DeleteRight", typeof(string));

                foreach (GridViewRow gvRow in gvList.Rows)
                {
                    CheckBox chkBxCreateAccess = (CheckBox)gvRow.FindControl("cbxCreateAccess");
                    CheckBox chkBxModifyAccess = (CheckBox)gvRow.FindControl("cbxModifyAccess");
                    CheckBox chkBxViewAccess = (CheckBox)gvRow.FindControl("cbxViewAccess");
                    CheckBox chkBxDeleteAccess = (CheckBox)gvRow.FindControl("cbxDeleteAccess");

                    DataRow drUpdate = dtUpdate.NewRow();

                    drUpdate["Function_Code"] = gvList.DataKeys[gvRow.RowIndex].Values[0].ToString();
                    drUpdate["Action"] = "D";

                    if (chkBxCreateAccess != null && chkBxCreateAccess.Checked)
                    {
                        //drUpdate["CreateRight"] = true;
                        drUpdate["CreateRight"] = "Y";
                    }
                    else
                    {
                        // drUpdate["CreateRight"] = false;
                        drUpdate["CreateRight"] = "N";
                    }
                    if (chkBxModifyAccess != null && chkBxModifyAccess.Checked)
                    {
                        //drUpdate["ModifyRight"] = true;
                        drUpdate["ModifyRight"] = "Y";
                    }
                    else
                    {
                        //drUpdate["ModifyRight"] = false;
                        drUpdate["ModifyRight"] = "N";
                    }
                    if (chkBxViewAccess != null && chkBxViewAccess.Checked)
                    {
                        //drUpdate["ViewRight"] = true;
                        drUpdate["ViewRight"] = "Y";
                    }
                    else
                    {
                        //drUpdate["ViewRight"] = false;
                        drUpdate["ViewRight"] = "N";
                    }
                    if (chkBxDeleteAccess != null && chkBxDeleteAccess.Checked)
                    {
                        //drUpdate["DeleteRight"] = true;
                        drUpdate["DeleteRight"] = "Y";
                    }
                    else
                    {
                        //drUpdate["DeleteRight"] = false;
                        drUpdate["DeleteRight"] = "N";
                    }

                    dtUpdate.Rows.Add(drUpdate);

                }

                DataSet dsUserRoleUpdate = new DataSet();
                dsUserRoleUpdate.Tables.Add(dtUpdate);
                DataSet dsReturn = accessControlService.UpdateUserRole(selectedRoleId, "", "", dsUserRoleUpdate, base.userLoginId);

                if (dsReturn == null)
                {
                    divMessage.InnerHtml = "Error calling webservice!";
                }
                else
                {
                    //Rebind Role Name DropDownList
                    ReBindRoleNameddl();
                    SearchAndDisplay();
                    this.ClearRoleForm();

                    divMessage.InnerHtml = dsReturn.Tables[0].Rows[0]["ReturnMessage"].ToString();
                }
            }
        }

        protected void ReBindRoleNameddl()
        {
            //Rebind Role Name DropDownList
            DataSet ds = new DataSet();
            ds = accessControlService.RetrieveUserRoles();

            ddlRoleName.Items.Clear();

            if (ds.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString().Equals("1"))
            {
                ddlRoleName.Items.Add(new ListItem("---Select User Role---", ""));
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    ddlRoleName.Items.Add(new ListItem(dr["RoleName"].ToString(), dr["RoleId"].ToString()));
                }
            }

        }
        
    }
}
