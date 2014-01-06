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
using System.Collections.Generic;

using SPMWebApp.Utilities;
using SPMWebApp.BasePage;
using SPMWebApp.Services;

namespace SPMWebApp.WebPages.Setup
{
    public partial class ClientShortKey : BasePage.BasePage
    {
        private ClientShortKeyService clientShortKeyService;

        protected void Page_Load(object sender, EventArgs e)
        {
            base.gvList = gvClientShortKey;
            base.divMessage = divMessage;
            base.pgcPagingControl = pgcShortKey;

            if (!IsPostBack)
            {
                LoadUserAccessRight();
                checkAccessRight();
                divPaging.Visible = false;

                InitializeRowPerPageSetting();
                ViewState["RowPerPage"] = 20;
            }
        }

        protected void checkAccessRight()
        {
            try
            {
                btnAdd.Enabled = (bool)ViewState["CreateAccessRight"];
            }
            catch (Exception e) { }            
        }

        protected override DataTable GetDataTable()
        {
            return ViewState["dtClientShortKey"] as DataTable;
        }

        protected override void SetCurrentRowPerPage(int rowPerPage)
        {
            ViewState["RowPerPage"] = rowPerPage;
        }

        private void InitializeRowPerPageSetting()
        {
            //Setting for RowPerPage for Custom Paging Control
            pgcShortKey.StartRowPerPage = 10;
            pgcShortKey.RowPerPageIncrement = 10;
            pgcShortKey.EndRowPerPage = 100;
        }

        //For Paging Control
        protected override void DisplayPaging()
        {
            if (divPaging.Visible)
            {
                int rowPerPage = (int)ViewState["RowPerPage"];

                pgcShortKey.PageCount = gvClientShortKey.PageCount;
                pgcShortKey.CurrentRowPerPage = rowPerPage.ToString();
                pgcShortKey.DisplayPaging();
            }
        }

        protected override Hashtable RetrieveRecords()
        {
            Hashtable ht = new Hashtable();
            DataSet ds = null;            
            clientShortKeyService = new ClientShortKeyService(base.dbConnectionStr);

            ht.Add("ReturnData", null);
            ht.Add("ReturnCode", "-1");
            ht.Add("ReturnMessage", "");

            ds = clientShortKeyService.RetrieveClientShortKey(txtAccountNo.Text.Trim(), txtShortKey.Text.Trim());
            if (ds.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString() == "1")
            {
                ViewState["dtClientShortKey"] = ds.Tables[0];
                ht["ReturnData"] = ds.Tables[0];

                divPaging.Visible = true;
            }
            else
            {
                divPaging.Visible = false;
            }

            ht["ReturnCode"] = ds.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString();
            ht["ReturnMessage"] = ds.Tables["ReturnTable"].Rows[0]["ReturnMessage"].ToString();
            return ht;
        }

        protected override Hashtable AddRecords()
        {
            Hashtable ht = new Hashtable();
            string[] wsReturn = null;
            string returnMessage = "";
            DataTable dtClientShortKey = ViewState["dtClientShortKey"] as DataTable;
            DataRow drNew = null;
            clientShortKeyService = new ClientShortKeyService(base.dbConnectionStr);


            ht.Add("ReturnData", null);
            ht.Add("ReturnCode", "");
            ht.Add("ReturnMessage", "");

            //Validate Values
            returnMessage = ValidateForm();
            if (returnMessage == "ok")
            {
                wsReturn = clientShortKeyService.InsertClientShortKey(base.userLoginId, txtAccountNo.Text.Trim(), txtShortKey.Text.Trim());
                if (wsReturn[0] == "1")
                {
                    if (dtClientShortKey == null)
                    {
                        dtClientShortKey = new DataTable();
                        dtClientShortKey.Columns.Add("AcctNo", String.Empty.GetType());
                        dtClientShortKey.Columns.Add("ShortKey", String.Empty.GetType());
                        dtClientShortKey.Columns.Add("UserID", String.Empty.GetType());
                        dtClientShortKey.Columns.Add("LNAME", String.Empty.GetType());
                    }

                    drNew = dtClientShortKey.NewRow();
                    drNew["AcctNo"] = txtAccountNo.Text.Trim();
                    drNew["ShortKey"] = txtShortKey.Text.Trim();
                    drNew["UserID"] = base.userLoginId;
                    drNew["LNAME"] = wsReturn[2];
                    dtClientShortKey.Rows.Add(drNew);

                    txtAccountNo.Text = "";
                    txtShortKey.Text = "";
                }

                ht["ReturnCode"] = wsReturn[0];
                ht["ReturnMessage"] = wsReturn[1];                
            }
            else
            {
                ht["ReturnCode"] = "-1";
                ht["ReturnMessage"] = returnMessage;
            }

            ViewState["dtClientShortKey"] = dtClientShortKey;
            ht["ReturnData"] = dtClientShortKey;

            return ht;
        }

        protected override Hashtable DeleteRecord(int deleteIndex)
        {
            string accountNo = gvClientShortKey.DataKeys[deleteIndex].Value.ToString();
            Label gvlblRowNo = (Label)gvClientShortKey.Rows[deleteIndex].FindControl("gvlblRowNo");
            int dataRowIndex = int.Parse(gvlblRowNo.Text) - 1;
            DataTable dtClientShortKey = ViewState["dtClientShortKey"] as DataTable;
            Hashtable ht = new Hashtable();
            string[] wsReturn = null;
            clientShortKeyService = new ClientShortKeyService(base.dbConnectionStr);

            wsReturn = clientShortKeyService.DeleteClientShortKey(accountNo);
            if (wsReturn[0] == "1")
            {
                dtClientShortKey.Rows.RemoveAt(dataRowIndex);
            }

            ViewState["dtClientShortKey"] = dtClientShortKey;
            ht.Add("ReturnData", dtClientShortKey);
            ht.Add("ReturnCode", wsReturn[0]);
            ht.Add("ReturnMessage", wsReturn[1]);
            return ht;
        }
        
        private string ValidateForm()
        {
            string result = "ok";

            if (String.IsNullOrEmpty(txtAccountNo.Text.Trim()))
                result = "Account Number cannot be blank!";
            else if (String.IsNullOrEmpty(txtShortKey.Text.Trim()))
                result = "Short Key cannot be blank!";

            return result;
        }

        /// <summary>
        /// Fetch User Access Right
        /// Call to ValidateUserAccessRight method with FunctionCode
        /// </summary>        
        private void LoadUserAccessRight()
        {
            List<AccessRight> accessRightList;
            if (AccessRightUtilities.ValidateUserAccessRight((DataTable)Session["UserAccessRights"], "DealerShortKey", out accessRightList))
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

        protected void gvClientShortKey_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                System.Web.UI.WebControls.Button btnDelete = (System.Web.UI.WebControls.Button)e.Row.Cells[4].FindControl("Button1");

                if (btnDelete != null)
                {
                    ((System.Web.UI.WebControls.Button)e.Row.Cells[4].FindControl("Button1")).Enabled = (bool)ViewState["DeleteAccessRight"];
                }
            }
        }

       
    }
}
