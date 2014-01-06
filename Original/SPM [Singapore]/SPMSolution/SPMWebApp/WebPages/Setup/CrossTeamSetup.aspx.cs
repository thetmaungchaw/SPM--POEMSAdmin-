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

using SPMWebApp.BasePage;
using SPMWebApp.Services;
using SPMWebApp.Utilities;

namespace SPMWebApp.WebPages.Setup
{
    public partial class CrossTeamSetup : BasePage.BasePage
    {
        private CrossTeamDealerService commonDealerService;

        /// <summary>
        /// Fetch User Access Right
        /// Call to ValidateUserAccessRight method with FunctionCode
        /// </summary>        

        protected void checkAccessRight()
        {
            try
            {
                btnSearch.Enabled = (bool)ViewState["ViewAccessRight"];
                divMessage.InnerHtml = "No permission to view.";
            }
            catch (Exception e) { }
        }

        private void LoadUserAccessRight()
        {
            List<AccessRight> accessRightList;
            if (AccessRightUtilities.ValidateUserAccessRight((DataTable)Session["UserAccessRights"], "AEGpMatch", out accessRightList))
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
            base.divMessage = divMessage;
            base.gvList = gvCommonDealer;
            base.pgcPagingControl = pgcCommonDealer;

            if (!IsPostBack)
            {
                LoadUserAccessRight();
                checkAccessRight();
                PrepareForCrossTeamSetup();

                InitializeRowPerPageSetting();
                ViewState["RowPerPage"] = 20;

                divPaging.Visible = false;
            }
        }

        protected override DataTable GetDataTable()
        {
            return ViewState["dtCommonDealer"] as DataTable;
        }

        protected override void SetCurrentRowPerPage(int rowPerPage)
        {
            ViewState["RowPerPage"] = rowPerPage;
        }

        private void InitializeRowPerPageSetting()
        {
            //Setting for RowPerPage for Custom Paging Control
            pgcCommonDealer.StartRowPerPage = 10;
            pgcCommonDealer.RowPerPageIncrement = 10;
            pgcCommonDealer.EndRowPerPage = 100;
        }

        //For Paging Control
        protected override void DisplayPaging()
        {
            if (divPaging.Visible)
            {
                int rowPerPage = (int)ViewState["RowPerPage"];

                pgcCommonDealer.PageCount = gvCommonDealer.PageCount;
                pgcCommonDealer.CurrentRowPerPage = rowPerPage.ToString();
                pgcCommonDealer.DisplayPaging();
            }
        }

        private void PrepareForCrossTeamSetup()
        {
            commonDealerService = new CrossTeamDealerService(base.dbConnectionStr);
            DataSet ds = null;

            ds = commonDealerService.PrepareForCrossTeamSetup();
            if (ds.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString() == "1")
            {
                CommonUtilities.BindDataToDropDrownList(ddlTeamCode, ds.Tables[0], "TeamName", "TeamCode", "--- Select Team ---");
                CommonUtilities.BindDataToDropDrownList(ddlCrossTeamCode, ds.Tables[0], "TeamName", "TeamCode", "--- Select Team ---");
                CommonUtilities.BindDataToDropDrownList(ddlDealer, ds.Tables[1], "DisplayName", "AECode", "--- Select Dealer ---");

                ViewState["dtTeam"] = ds.Tables[0];
            }
        }

        protected override Hashtable RetrieveRecords()
        {
            DataSet ds = null;
            Hashtable ht = new Hashtable();
            commonDealerService = new CrossTeamDealerService(base.dbConnectionStr);

            ht.Add("ReturnData", null);
            ht.Add("ReturnCode", "-1");
            ht.Add("ReturnMessage", "");

            if ((!String.IsNullOrEmpty(ddlTeamCode.SelectedValue)) && (!String.IsNullOrEmpty(ddlCrossTeamCode.SelectedValue)) &&
                   (ddlTeamCode.SelectedValue.Equals(ddlCrossTeamCode.SelectedValue)))
            {
                ht["ReturnMessage"] = "Matched team can not be the original team!";
            }
            else
            {
                ds = commonDealerService.RetrieveCrossTeamDealer(ddlDealer.SelectedValue, ddlTeamCode.SelectedValue, ddlCrossTeamCode.SelectedValue);
                if (ds.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString() == "1")
                {
                    ht["ReturnData"] = ds.Tables[0];
                    ViewState["dtCommonDealer"] = ds.Tables[0];

                    divPaging.Visible = true;
                }
                else
                {
                    divPaging.Visible = false;
                }

                ht["ReturnCode"] = ds.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString();
                ht["ReturnMessage"] = ds.Tables["ReturnTable"].Rows[0]["ReturnMessage"].ToString();
            }

            return ht;
        }

        protected void gvCommonDealer_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                System.Web.UI.WebControls.Button btnDelete = (System.Web.UI.WebControls.Button)e.Row.Cells[5].FindControl("Button1");

                if (btnDelete != null)
                {
                    ((System.Web.UI.WebControls.Button)e.Row.Cells[5].FindControl("Button1")).Enabled = (bool)ViewState["DeleteAccessRight"];
                }

                if ((DataBinder.Eval(e.Row.DataItem, "Supervisor") != null) &&
                        ((DataBinder.Eval(e.Row.DataItem, "Supervisor").ToString() == "Y")))
                {
                    e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFCC99");
                }

                DropDownList gvddlMatch = (DropDownList)e.Row.FindControl("gvddlMatch");
                DataTable dtTeam = ViewState["dtTeam"] as DataTable;
                if (dtTeam != null)
                {
                    CommonUtilities.BindDataToDropDrownListByFilter(gvddlMatch, DataBinder.Eval(e.Row.DataItem, "Original").ToString(), dtTeam,
                        "TeamCode", "TeamCode", "------");

                    if ((DataBinder.Eval(e.Row.DataItem, "Match") != null) &&
                            (!String.IsNullOrEmpty(DataBinder.Eval(e.Row.DataItem, "Match").ToString())))
                    {
                        gvddlMatch.SelectedValue = DataBinder.Eval(e.Row.DataItem, "Match").ToString();
                    }
                }
            }
        }

        protected void gvCommonDealer_RowEditing(object sender, GridViewEditEventArgs e)
        {
            DataTable dtCommonDealer = ViewState["dtCommonDealer"] as DataTable;
            HiddenField gvhdfItemIndex = (HiddenField)gvCommonDealer.Rows[e.NewEditIndex].FindControl("gvhdfItemIndex");
            DropDownList gvddlMatch = (DropDownList)gvCommonDealer.Rows[e.NewEditIndex].FindControl("gvddlMatch");
            int dataRowIndex = int.Parse(gvhdfItemIndex.Value);
            string[] wsReturn = null;
            commonDealerService = new CrossTeamDealerService(base.dbConnectionStr);

            if (String.IsNullOrEmpty(dtCommonDealer.Rows[dataRowIndex]["Match"].ToString()))
            {
                if (!String.IsNullOrEmpty(gvddlMatch.SelectedValue))
                {
                    //divMessage.InnerHtml = "New Record!";

                    wsReturn = commonDealerService.InsertCrossTeamDealer(dtCommonDealer.Rows[dataRowIndex]["AECode"].ToString(),
                                    gvddlMatch.SelectedValue, base.userLoginId);
                    if (wsReturn[0] == "1")
                    {
                        dtCommonDealer.Rows[dataRowIndex]["Match"] = gvddlMatch.SelectedValue;
                        dtCommonDealer.Rows[dataRowIndex]["ModifiedUser"] = base.userLoginId;
                        dtCommonDealer.Rows[dataRowIndex]["ModifiedDate"] = DateTime.Now;
                    }
                    divMessage.InnerHtml = wsReturn[1];
                }
                else
                {
                    divMessage.InnerHtml = "Please select match team!";     //(New Record)
                }
            }
            else if (String.IsNullOrEmpty(gvddlMatch.SelectedValue))
            {
                //divMessage.InnerHtml = "Delete Record!";

                wsReturn = commonDealerService.DeleteCrossTeamDealer(dtCommonDealer.Rows[dataRowIndex]["AECode"].ToString());
                if (wsReturn[0] == "1")
                {
                    //DateTime? modifiedDt = null;

                    dtCommonDealer.Rows[dataRowIndex]["Match"] = gvddlMatch.SelectedValue;
                    dtCommonDealer.Rows[dataRowIndex]["ModifiedUser"] = "";
                    dtCommonDealer.Rows[dataRowIndex]["ModifiedDate"] = DBNull.Value;
                }
                divMessage.InnerHtml = wsReturn[1];
            }
            else
            {
                if (!gvddlMatch.SelectedValue.Equals(dtCommonDealer.Rows[dataRowIndex]["Match"].ToString()))
                {
                    //divMessage.InnerHtml = "Update Record!";
                    wsReturn = commonDealerService.UpdateCrossTeamDealer(dtCommonDealer.Rows[dataRowIndex]["AECode"].ToString(),
                                    gvddlMatch.SelectedValue, base.userLoginId);
                    if (wsReturn[0] == "1")
                    {
                        dtCommonDealer.Rows[dataRowIndex]["Match"] = gvddlMatch.SelectedValue;
                        dtCommonDealer.Rows[dataRowIndex]["ModifiedUser"] = base.userLoginId;
                        dtCommonDealer.Rows[dataRowIndex]["ModifiedDate"] = DateTime.Now;
                    }
                    divMessage.InnerHtml = wsReturn[1];
                }
                else
                {
                    divMessage.InnerHtml = "Matched team can not be the same team";     //(Update Record)
                }
            }

            ViewState["dtCommonDealer"] = dtCommonDealer;
            gvCommonDealer.DataSource = dtCommonDealer;
            gvCommonDealer.DataBind();
        }
    }
}