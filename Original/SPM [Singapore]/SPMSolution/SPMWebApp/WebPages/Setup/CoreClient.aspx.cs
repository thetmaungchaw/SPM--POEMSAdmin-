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
    public partial class CoreClient : BasePage.BasePage
    {
        private CoreClientService coreClientService;

        protected void Page_Load(object sender, EventArgs e)
        {
            base.gvList = this.gvList;
            base.divMessage = this.divMessage;
            base.pgcPagingControl = pgcCoreClient;

            if (!IsPostBack)
            {
                LoadUserAccessRight();
                checkAccessRight();

                PrepareForCoreClient(base.userLoginId);

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

        /// <summary>
        /// Fetch User Access Right
        /// Call to ValidateUserAccessRight method with FunctionCode
        /// </summary>        
        private void LoadUserAccessRight()
        {
            List<AccessRight> accessRightList;
            if (AccessRightUtilities.ValidateUserAccessRight((DataTable)Session["UserAccessRights"], "CoreClient", out accessRightList))
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

        protected override DataTable GetDataTable()
        {
            return ViewState["dtCoreClient"] as DataTable;
        }

        protected override void SetCurrentRowPerPage(int rowPerPage)
        {
            ViewState["RowPerPage"] = rowPerPage;
        }

        private void InitializeRowPerPageSetting()
        {
            //Setting for RowPerPage for Custom Paging Control
            pgcCoreClient.StartRowPerPage = 10;
            pgcCoreClient.RowPerPageIncrement = 10;
            pgcCoreClient.EndRowPerPage = 100;
        }

        //For Paging Control
        protected override void DisplayPaging()
        {
            if (divPaging.Visible)
            {
                int rowPerPage = (int)ViewState["RowPerPage"];

                pgcCoreClient.PageCount = this.gvList.PageCount;
                pgcCoreClient.CurrentRowPerPage = rowPerPage.ToString();
                pgcCoreClient.DisplayPaging();
            }
        }

        private void PrepareForCoreClient(String UserID)
        {
            coreClientService = new CoreClientService(base.dbConnectionStr);
            DataSet ds = null;

            ds = coreClientService.PrepareForCoreClient(UserID);
            if (ds.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString() == "1")
            {
                ddlDealer.Items.Add(new ListItem("--- Select Dealer ---", ""));
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    ddlDealer.Items.Add(new ListItem(ds.Tables[0].Rows[i]["DisplayName"].ToString(), ds.Tables[0].Rows[i]["AECode"].ToString()));
                    if (ds.Tables[0].Rows[i]["UserID"].ToString().Equals(base.userLoginId))
                    {
                        ddlDealer.SelectedValue = ds.Tables[0].Rows[i]["AECode"].ToString();
                    }
                }
            }
        }
                      
        protected override Hashtable DeleteRecord(int deleteIndex)
        {
            long deleteRecId = long.Parse(gvList.DataKeys[deleteIndex].Value.ToString());
            int result = 5;
            string accNo = "", AECode = "", returnMessage = "";
            Hashtable ht = new Hashtable();
            DataTable dtCoreClient = null;
            DataRow[] drDelete = null;
            string[] wsReturn = null;
            coreClientService = new CoreClientService(base.dbConnectionStr);

            wsReturn = coreClientService.DeleteCoreClient(deleteRecId);
            if (wsReturn[0] == "1")
            {
                if (ViewState["dtCoreClient"] != null)
                {
                    dtCoreClient = (DataTable)ViewState["dtCoreClient"];

                    drDelete = dtCoreClient.Select("RecId = " + deleteRecId);
                    if (drDelete != null)
                    {
                        accNo = drDelete[0]["AcctNo"].ToString();
                        AECode = drDelete[0]["AECode"].ToString();
                        dtCoreClient.Rows.Remove(drDelete[0]);
                    }

                    ViewState["dtCoreClient"] = dtCoreClient;
                    drDelete = null;
                }

                //returnMessage = "AECode : " + AECode + "   A/C: " + accNo + "<br>Successfully deleted";
            }

            ht.Add("ReturnCode", wsReturn[0]);
            ht.Add("ReturnMessage", wsReturn[1]);
            ht.Add("ReturnData", dtCoreClient);

            return ht;
        }        

        protected override Hashtable RetrieveRecords()
        {
            coreClientService = new CoreClientService(base.dbConnectionStr);
            DataSet ds = new DataSet();
            Hashtable ht = new Hashtable();
            int returnCode = 1;
            string returnMessage = "";

            ht.Add("ReturnData", null);
            ht.Add("ReturnCode", "-1");
            ht.Add("ReturnMessage", "");

            ds = coreClientService.RetrieveCoreClientList(ddlDealer.SelectedValue, txtAccNo.Text.Trim());
            if (ds.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString() == "1")
            {
                ViewState["dtCoreClient"] = ds.Tables[0];
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
            DataSet ds = null;
            Hashtable ht = new Hashtable();
            string returnMessage = "";
            DataTable dtCoreClient = ViewState["dtCoreClient"] as DataTable;
            coreClientService = new CoreClientService(base.dbConnectionStr);

            ht.Add("ReturnData", dtCoreClient);
            ht.Add("ReturnCode", "-1");
            ht.Add("ReturnMessage", "");

            if (String.IsNullOrEmpty(ddlDealer.SelectedValue))
            {
                returnMessage = "Please choose Dealer for Client!";                
            }
            else if (String.IsNullOrEmpty(txtAccNo.Text.Trim()))
            {
                returnMessage = "Account No cannot be blank!";
            }
            else
            {
                ds = coreClientService.AddCoreClient(ddlDealer.SelectedValue, txtAccNo.Text.Trim(), base.userLoginId);
                if (ds.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString() == "1")
                {
                    if (dtCoreClient == null)
                    {
                        dtCoreClient = new DataTable();
                        dtCoreClient.Columns.Add("RecId", String.Empty.GetType());
                        dtCoreClient.Columns.Add("AcctNo", String.Empty.GetType());
                        dtCoreClient.Columns.Add("AECode", String.Empty.GetType());
                        dtCoreClient.Columns.Add("AEName", String.Empty.GetType());
                        dtCoreClient.Columns.Add("ModifiedUser", String.Empty.GetType());
                        dtCoreClient.Columns.Add("ModifiedDate", String.Empty.GetType());
                    }


                    DataRow drNew = dtCoreClient.NewRow();

                    for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
                    {
                        drNew[i] = ds.Tables[0].Rows[0][i].ToString();
                    }
                    dtCoreClient.Rows.Add(drNew);

                    ViewState["dtCoreClient"] = dtCoreClient;
                    ht["ReturnData"] = dtCoreClient;
                    txtAccNo.Text = "";
                }
                ht["ReturnCode"] = ds.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString();
                returnMessage = ds.Tables["ReturnTable"].Rows[0]["ReturnMessage"].ToString();
            }

            ht["ReturnMessage"] = returnMessage;
            return ht;
        }

        protected void gvList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                System.Web.UI.WebControls.Button btnDelete = (System.Web.UI.WebControls.Button)e.Row.Cells[6].FindControl("Button1");

                if (btnDelete != null)
                {
                    ((System.Web.UI.WebControls.Button)e.Row.Cells[6].FindControl("Button1")).Enabled = (bool)ViewState["DeleteAccessRight"];
                }
            }
        }

     
    }
}