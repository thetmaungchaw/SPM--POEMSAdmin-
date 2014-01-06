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


namespace SPMWebApp.WebPages.Setup
{
    public partial class PreferList : BasePage.BasePage
    {
        private PreferListService preferListService;

        /// <summary>
        /// Fetch User Access Right
        /// Call to ValidateUserAccessRight method with FunctionCode
        /// </summary>        
        private void LoadUserAccessRight()
        {
            List<AccessRight> accessRightList;
            if (AccessRightUtilities.ValidateUserAccessRight((DataTable)Session["UserAccessRights"], "PreferList", out accessRightList))
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
                btnAdd.Enabled = (bool)ViewState["CreateAccessRight"];
                btnSearch.Enabled = (bool)ViewState["ViewAccessRight"];                
            }
            catch (Exception e) { }             
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            base.gvList = this.gvPreferList;
            base.divMessage = this.divMessage;
            base.hdfModifyIndex = this.hdfModifyIndex;
            base.pgcPagingControl = this.pgcPreferList;
            
            
            if (!IsPostBack)
            {
                LoadUserAccessRight();
                checkAccessRight();
                //Setting for RowPerPage for Custom Paging Control
                InitializeRowPerPageSetting();
                ViewState["RowPerPage"] = 20;
                divPaging.Visible = false;

                btnUpdate.Visible = false;
                btnCancel.Visible = false;
            }
        }

        protected override DataTable GetDataTable()
        {
            return ViewState["dtPreferList"] as DataTable;
        }

        protected override void SetCurrentRowPerPage(int rowPerPage)
        {
            ViewState["RowPerPage"] = rowPerPage;
        }

        private void InitializeRowPerPageSetting()
        {
            //Setting for RowPerPage for Custom Paging Control
            pgcPreferList.StartRowPerPage = 10;
            pgcPreferList.RowPerPageIncrement = 10;
            pgcPreferList.EndRowPerPage = 100;
        }

        protected override Hashtable UpdateRecord(int modifyIndex)
        {
            long recId = 1;
            Hashtable ht = new Hashtable();
            DataRow[] drArrModify = null;
            DataTable dtPreferList = null;
            string[] wsReturn = null;
            string returnMessage = "";
            preferListService = new PreferListService(base.dbConnectionStr);
            
            btnUpdate.Visible = true;
            btnCancel.Visible = true;

            ht.Add("ReturnData", null);
            ht.Add("ReturnCode", "");
            ht.Add("ReturnMessage", "");

            //Validate Values
            returnMessage = ValidateForm();
            if (returnMessage == "ok")
            {
                recId = long.Parse(gvPreferList.DataKeys[modifyIndex].Value.ToString());
                wsReturn = preferListService.UpdatePreferList(recId, txtIndex.Text.Trim(), txtContent.Text.Trim());

                if (wsReturn[0] == "1")
                {
                    if (ViewState["dtPreferList"] != null)
                    {
                        dtPreferList = (DataTable)ViewState["dtPreferList"];
                        recId = long.Parse(gvPreferList.DataKeys[modifyIndex].Value.ToString());
                        drArrModify = dtPreferList.Select("RecId = " + recId);

                        drArrModify[0].BeginEdit();
                        drArrModify[0]["OptionNo"] = txtIndex.Text.Trim();
                        drArrModify[0]["OptionContent"] = txtContent.Text.Trim();
                        drArrModify[0].EndEdit();

                        gvPreferList.Rows[modifyIndex].Cells[1].Text = txtIndex.Text;
                        gvPreferList.Rows[modifyIndex].Cells[2].Text = txtContent.Text;

                        ViewState["dtPreferList"] = dtPreferList;
                        txtContent.Text = "";
                        txtIndex.Text = "";

                        btnAdd.Visible = true;
                        btnSearch.Visible = true;

                        btnUpdate.Visible = false;
                        btnCancel.Visible = false;
                    }
                }

                ht["ReturnCode"] = wsReturn[0];
                ht["ReturnMessage"] = wsReturn[1];
            }
            else
            {
                ht["ReturnCode"] = "-1";
                ht["ReturnMessage"] = returnMessage;
            }


            if (ht["ReturnCode"].ToString() != "1")
            {
                divPaging.Visible = false;
            }
            else
            {
                divPaging.Visible = true;
            }
            
            return ht;
        }

        protected override void DisplayDetails(int modifyIndex)
        {
            divMessage.InnerHtml = "";
            if (ViewState["dtPreferList"] != null)
            {
                GridViewRow gvrModify = gvPreferList.Rows[modifyIndex];
                txtIndex.Text = gvrModify.Cells[1].Text;
                txtContent.Text = gvrModify.Cells[2].Text;

                this.hdfModifyIndex.Value = modifyIndex + "";
                btnUpdate.Visible = true;
                btnCancel.Visible = true;
                btnAdd.Visible = false;
                btnSearch.Visible = false;                
            }
        }

        protected override Hashtable RetrieveRecords()
        {
            Hashtable ht = new Hashtable();
            DataSet ds = null;
            int returnCode = 1;
            string returnMessage = "";
            preferListService = new PreferListService(base.dbConnectionStr);

            ht.Add("ReturnData", null);
            ds = preferListService.RetrievePerferList(txtIndex.Text.Trim(), txtContent.Text.Trim());
            if (ds.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString() == "1")
            {
                ViewState["dtPreferList"] = ds.Tables[0];
                ht["ReturnData"] = ds.Tables[0];

                //Show Paging
                divPaging.Visible = true;                
            }
            else
            {
                divPaging.Visible = false;
            }

            ht.Add("ReturnCode", ds.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString());
            ht.Add("ReturnMessage", ds.Tables["ReturnTable"].Rows[0]["ReturnMessage"].ToString());
            return ht;
        }

        protected override void DisplayPaging()
        {
            if (divPaging.Visible)
            {
                int rowPerPage = (int)ViewState["RowPerPage"];

                pgcPreferList.PageCount = gvPreferList.PageCount;
                pgcPreferList.CurrentRowPerPage = rowPerPage.ToString();
                pgcPreferList.DisplayPaging();
            }            
        }

        protected override Hashtable AddRecords()
        {
            Hashtable ht = new Hashtable();
            DataSet ds = null;
            string returnMessage = "";
            DataTable dtPreferList = ViewState["dtPreferList"] as DataTable;
            DataRow drNew = null;
            preferListService = new PreferListService(base.dbConnectionStr);


            ht.Add("ReturnData", null);
            ht.Add("ReturnCode", "");
            ht.Add("ReturnMessage", "");

            //Validate Values
            returnMessage = ValidateForm();
            if (returnMessage == "ok")
            {
                ds = preferListService.AddPreference(Session["UserId"].ToString(), txtIndex.Text.Trim(), txtContent.Text.Trim());
                if (ds.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString() == "1")
                {
                    //ViewState["dtPreferList"] = ds.Tables[0];
                    //ht["ReturnData"] = ds.Tables[0];

                    if (dtPreferList == null)
                    {
                        dtPreferList = new DataTable();
                        dtPreferList.Columns.Add("RecId", String.Empty.GetType());
                        dtPreferList.Columns.Add("OptionNo", String.Empty.GetType());
                        dtPreferList.Columns.Add("OptionContent", String.Empty.GetType());
                        dtPreferList.Columns.Add("ModifiedUser", String.Empty.GetType());
                        dtPreferList.Columns.Add("ModifiedDate", String.Empty.GetType());
                    }


                    drNew = dtPreferList.NewRow();
                    for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
                    {
                        drNew[i] = ds.Tables[0].Rows[0][i].ToString();
                    }
                    dtPreferList.Rows.Add(drNew);

                    ViewState["dtPreferList"] = dtPreferList;
                    ht["ReturnData"] = dtPreferList;
                }

                ht["ReturnCode"] = ds.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString();
                ht["ReturnMessage"] = ds.Tables["ReturnTable"].Rows[0]["ReturnMessage"].ToString();
            }
            else
            {
                ht["ReturnCode"] = "-1";
                ht["ReturnMessage"] = returnMessage;
            }


            if (ht["ReturnCode"].ToString() != "1")
            {
                divPaging.Visible = false;
            }
            else
            {
                divPaging.Visible = true;
            }
            
            return ht;
        }

        private string ValidateForm()
        {
            string result = "ok";
            if (String.IsNullOrEmpty(txtIndex.Text.Trim()))
            {
                result = "Index cannot be blank!";
            }
            else if (String.IsNullOrEmpty(txtContent.Text.Trim()))
            {
                result = "Content cannot be blank!";
            }
            return result;
        }

        protected override Hashtable DeleteRecord(int deleteIndex)
        {
            long deleteRecId = long.Parse(gvPreferList.DataKeys[deleteIndex].Value.ToString());
            Hashtable ht = new Hashtable();
            DataTable dtPreferList = null;
            DataRow[] drDelete = null;
            string[] wsReturn = null;
            preferListService = new PreferListService(base.dbConnectionStr);

            ht.Add("ReturnData", null);
            wsReturn = preferListService.DeletePreferList(deleteRecId);

            if (wsReturn[0] == "1")
            {
                if (ViewState["dtPreferList"] != null)
                {
                    dtPreferList = (DataTable)ViewState["dtPreferList"];
                    drDelete = dtPreferList.Select("RecId = " + deleteRecId);
                    if (drDelete != null)
                    {
                        dtPreferList.Rows.Remove(drDelete[0]);
                    }

                    if (dtPreferList.Rows.Count == 0)
                        divPaging.Visible = false;

                    ViewState["dtPreferList"] = dtPreferList;
                    ht["ReturnData"] = dtPreferList;
                    drDelete = null;
                }
            }
            else
            {
                divPaging.Visible = false;
            }

            ht.Add("ReturnCode", wsReturn[0]);
            ht.Add("ReturnMessage", wsReturn[1]);

            return ht;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            txtIndex.Text = "";
            txtContent.Text = "";

            btnUpdate.Visible = false;
            btnCancel.Visible = false;
            btnAdd.Visible = true;
            btnSearch.Visible = true;

            this.divMessage.InnerHtml = "";
        }

        protected void gvPreferList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvPreferList.PageIndex = e.NewPageIndex;
            if (ViewState["dtPreferList"] != null)
            {
                DataTable dtPreferList = (DataTable)ViewState["dtPreferList"];
                gvPreferList.DataSource = dtPreferList;
                gvPreferList.DataBind();
            }
        }

        protected void gvPreferList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                System.Web.UI.WebControls.Button btnModify = (System.Web.UI.WebControls.Button)e.Row.Cells[5].FindControl("Button1");
                System.Web.UI.WebControls.Button btnDelete = (System.Web.UI.WebControls.Button)e.Row.Cells[5].FindControl("Button2");

                if (btnModify != null)
                {
                    ((System.Web.UI.WebControls.Button)e.Row.Cells[5].FindControl("Button1")).Enabled = (bool)ViewState["ModifyAccessRight"];
                }

                if (btnDelete != null)
                {
                    ((System.Web.UI.WebControls.Button)e.Row.Cells[5].FindControl("Button2")).Enabled = (bool)ViewState["DeleteAccessRight"];
                }
            }
        }

        //Custom Paging Control Event Handlers
        /*
        protected void PagingControl_PageNoChange(object sender, EventArgs<Int32> e)
        {
            gvPreferList.PageIndex = e.Value - 1;
            if (ViewState["dtPreferList"] != null)
            {
                DataTable dtPreferList = (DataTable)ViewState["dtPreferList"];
                gvPreferList.DataSource = dtPreferList;
                gvPreferList.DataBind();
            }
        }

        protected void PagingControl_RowPerPageChanged(object sender, EventArgs<Int32> e)
        {            
            DataTable dtPreferList = ViewState["dtPreferList"] as DataTable;

            if (dtPreferList != null)
            {
                gvPreferList.PageSize = e.Value;
                gvPreferList.PageIndex = 0;
                gvPreferList.DataSource = dtPreferList;
                gvPreferList.DataBind();

                pgcPreferList.PageCount = gvPreferList.PageCount;
                pgcPreferList.DisplayPaging();
            }
        }
        */
    }
}
