/* 
 * Purpose:         Ancestor of all web page controllers 
 * Created By:      Than Htike Tun
 * Date:            10/02/2010
 * 
 * Change History:
 * ----------------------------------------------------------------------------------
 * Modified By      Date        Purpose
 * ----------------------------------------------------------------------------------
 * Li Qun           22/03/2010  Add in OnInit() to get user id and dbstr from session
 * Than Htike Tun   30/03/2010  Add in Pagination control
 * 
 */

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

using SPMWebApp.Services;
using SPMWebApp.Utilities;
using System.Security.Principal;

namespace SPMWebApp.BasePage
{
    public abstract class BasePage : System.Web.UI.Page
    {
        protected GridView gvList;
        protected HtmlGenericControl divMessage;
        protected HiddenField hdfModifyIndex;
        protected HtmlGenericControl divSeminarTitle;
        protected string userLoginId;
        protected string dbConnectionStr;

        protected PagingControl pgcPagingControl;
        private CommonService commonService;

        protected override void OnInit(EventArgs e)
        {
            try
            {
                //Check Request is from Microsoft Excel
                if (Request.UserAgent != "Microsoft Office Existence Discovery")
                {                    
                    if (Session["UserId"] == null || String.IsNullOrEmpty(Session["UserId"].ToString().Trim()))
                    {
                        Response.Redirect("~/SessionExpiry.aspx");
                        //Response.Write("<html><body><CENTER><font color=red FACE=ARIAL><STRONG>Session Expired! Pls re-login!</STRONG></FONT></CENTER></body></html>");
                        //Response.End();
                    }
                    else
                    {
                        userLoginId = Session["UserId"].ToString();
                        dbConnectionStr = Session["DbConnStr"].ToString();
                    }
                }                
            }
            catch (Exception ex)
            {

            }
            base.OnInit(e);
        }

        protected void gvListDelete_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Hashtable ht = DeleteRecord(e.RowIndex);
            DataTable dtCoreClient = null;

            if (int.Parse(ht["ReturnCode"].ToString()) > 0)
            {
                dtCoreClient = (DataTable)ht["ReturnData"];
            }

            gvList.DataSource = dtCoreClient;
            gvList.DataBind();
            divMessage.InnerHtml = ht["ReturnMessage"].ToString();
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            Hashtable ht = AddRecords();
            DataTable dtReturnData = null;

            divMessage.InnerHtml = ht["ReturnMessage"].ToString();

            if (ht["ReturnData"] != null)
            {
                dtReturnData = (DataTable)ht["ReturnData"];
            }

            gvList.DataSource = dtReturnData;
            gvList.DataBind();
        }

        protected void gvList_RowEditing(object sender, GridViewEditEventArgs e)
        {
            DisplayDetails(e.NewEditIndex);
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            Hashtable ht = UpdateRecord(int.Parse(this.hdfModifyIndex.Value));
            divMessage.InnerHtml = ht["ReturnMessage"].ToString();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            SearchAndDisplay();
        }

        protected void PagingControl_RowPerPageChanged(object sender, EventArgs<Int32> e)
        {
            DataTable dtData = GetDataTable();
            if (dtData != null)
            {
                gvList.PageSize = e.Value;
                gvList.PageIndex = 0;
                gvList.DataSource = dtData;
                gvList.DataBind();

                //Store Current Row Per Page in child page.
                SetCurrentRowPerPage(e.Value);
                pgcPagingControl.PageCount = gvList.PageCount;
                pgcPagingControl.DisplayPaging();
            }
        }

        protected void PagingControl_PageNoChange(object sender, EventArgs<Int32> e)
        {
            gvList.PageIndex = e.Value - 1;
            DataTable dtData = GetDataTable();
            if (dtData != null)
            {
                gvList.DataSource = dtData;
                gvList.DataBind();
            }
        }

        protected virtual void SearchAndDisplay()
        {
            DataTable dtReturnData = null;
            Hashtable ht = RetrieveRecords();

            divMessage.InnerHtml = ht["ReturnMessage"].ToString();

            if (ht["ReturnData"] != null)
            {
                dtReturnData = (DataTable)ht["ReturnData"];                
            }

            gvList.PageIndex = 0;
            gvList.DataSource = dtReturnData;
            gvList.DataBind();
          
            //divMessage.InnerHtml = divMessage.InnerHtml + "<br /> Total Page : " + gvList.PageCount + ", Current Page : " + gvList.PageIndex;
            DisplayPaging();
        }
        
        protected virtual Hashtable AddRecords()
        {
            return null;
        }

        protected virtual Hashtable RetrieveRecords()
        {
            return null;
        }

        protected virtual Hashtable DeleteRecord(int deleteIndex)
        {
            return null;
        }

        protected virtual Hashtable UpdateRecord(int modifyIndex)
        {
            return null;
        }

        protected virtual void DisplayDetails(int modifyIndex)
        {

        }

        protected virtual DataTable GetDataTable()
        {
            return null;
        }

        protected virtual void DisplayPaging()
        {
        }

        protected virtual void SetCurrentRowPerPage(int rowPerPage)
        {
        }

        //Method For Paging
        protected int GetGridViewPageCount()
        {
            return gvList.PageCount;
        }

        protected int GetGridViewCurrentPage()
        {
            return gvList.PageIndex;
        }
    
    
        //Common Services
        protected DataSet RetrieveTeamCodeAndName()
        {
            commonService = new CommonService(dbConnectionStr);
            return commonService.RetrieveTeamCodeAndName();
        }


        //Only used in Client Assignment
        protected DataSet RetrieveDealerCodeAndNameByTeam(string teamCode)
        {
            commonService = new CommonService(dbConnectionStr);
            return commonService.RetrieveDealerCodeAndNameByTeam(teamCode);
        }

        protected DataSet RetrieveDealerCodeAndNameByTeamNLoginID(string teamCode, string loginid)
        {
            commonService = new CommonService(dbConnectionStr);
            return commonService.RetrieveDealerCodeAndNameByTeamNLoginID(teamCode, loginid);
        }


        /** for SPM III 
         * User for Leads Assignment
         * Add by   Yin Mon Win
         * Date     16 September 2011
         * * */
        protected DataSet RetrieveMultipleDealerCodeAndNameByTeam(string teamCode)
        {
            commonService = new CommonService(dbConnectionStr);
            return commonService.RetrieveMultipleDealerCodeAndNameByTeam(teamCode);
        }

        /// <summary>
        /// Used to retrieve Projects by UserId
        /// Filter by Project Name if filterByProjectName is not Null or Empty
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="filterByProjectName"></param>
        /// <returns>DataSet</returns>
        protected DataSet RetrieveProjectsByUserId(String userId, String filterByProjectName)
        {
            commonService = new CommonService(dbConnectionStr);
            return commonService.RetrieveProjectsByUserId(userId, filterByProjectName);
        }

        /// <summary>
        /// Used to retrieve Total Commession by the Project Id
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        protected DataSet RetrieveProjectTotalCommByProjectId(string projectId)
        {
            commonService = new CommonService(dbConnectionStr);
            return commonService.RetrieveProjectTotalCommByProjectId(projectId);
        }
    }
}
