using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;


using SPMWebApp.BasePage;
using SPMWebApp.Utilities;
using SPMWebApp.Services;


namespace SPMWebApp.WebPages.LeadManagement
{
    public partial class LeadAssign : BasePage.BasePage
    {
        //private ClientAssignmentService clientAssignmentService;
        private LeadsAssignmentService  leadsAssignmentService;
        private int cellCount = 1;

        //private CommonUtilities comUti;
        //private string[] accessRight = new string[4];

        private const string ASSIGNMENT_NEW = "NEW";
        private const string ASSIGNMENT_UPDATE = "UPDATE";
        private const string ASSIGNMENT_MISS = "MISS";
        private const string ASSIGNMENT_EXIST = "EXIST";
        private const string ASSIGNMENT_CORE = "CORE";
        private const string ASSIGNMENT_2N = "2N";
        private const string ASSIGNMENT_CONTACTED = "CONTACTED";

        protected void checkAccessRight()
        {
            try
            {
                btnLeadSearch.Enabled = (bool)ViewState["ViewAccessRight"];
                btnSaveAssignment.Enabled = (bool)ViewState["CreateAccessRight"];
                btnAssignmentDelete.Enabled = (bool)ViewState["DeleteAccessRight"];
            }
            catch (Exception e) { }            
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //comUti = new CommonUtilities();
            //accessRight = comUti.getAccessRight("LeadsAssign", base.userLoginId, this.dbConnectionStr);

            base.gvList = gvLeadAssign;
            base.divMessage = divMessage;
            base.pgcPagingControl = pgcLeadAssign;

            
            if (!IsPostBack)
            {
                LoadUserAccessRight();
                //btnSearch.Attributes.Add("onclick", "document.body.style.cursor = 'wait';");

                if (Session["UserId"] == null)
                {
                }
                
                divPaging.Visible = false;
                InitializeRowPerPageSetting();
                ViewState["RowPerPage"] = 10;

                //calAcctFromDate.DateTextFromValue = DateTime.Now.AddMonths(-3).ToString("dd/MM/yyy");
                //calAcctToDate.DateTextFromValue = DateTime.Now.AddDays(-1.0).ToString("dd/MM/yyy");
                
                if (Request["type"] != null)
                {
                    ViewState["AssignmentType"] = "cross";
                    divTitle.InnerHtml = divTitle.InnerHtml + " - Cross Team";
                }
                else
                {
                    ViewState["AssignmentType"] = "team";
                }

                PrepareForLeadsAssignment();
                btnSaveAssignment.Visible = false;
                btnAssignmentDelete.Visible = false;
                //checkAccessRight();
            }

            lblProjectName.Visible = true;
            txtProjectName.Visible = true;
            lblCutOff.Visible = true;
            calCutOffDate.Visible = true;

            divMessage.InnerHtml = "";
        }

        /// <summary>
        /// Fetch User Access Right
        /// Call to ValidateUserAccessRight method with FunctionCode
        /// </summary>        
        private void LoadUserAccessRight()
        {
            List<AccessRight> accessRightList;
            if (AccessRightUtilities.ValidateUserAccessRight((DataTable)Session["UserAccessRights"], "LeadsAssign", out accessRightList))
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
            /* This method is called from BasePage whenever Page No (or) Row Per Page is changed. 
             * So need to called SaveClientAssignForPageChange method to save current page changes.
             */
            if (SaveLeadsAssignForPageChange())
                return ViewState["dtLeadsAssign"] as DataTable;
            else
                return null;
        }

        protected override void SetCurrentRowPerPage(int rowPerPage)
        {
            ViewState["RowPerPage"] = rowPerPage;
        }

        private void InitializeRowPerPageSetting()
        {
            //Setting for RowPerPage for Custom Paging Control
            pgcLeadAssign.StartRowPerPage = 10;
            pgcLeadAssign.RowPerPageIncrement = 10;
            pgcLeadAssign.EndRowPerPage = 100;
        }

        //For Paging Control
        protected override void DisplayPaging()
        {
            if (divPaging.Visible)
            {
                int rowPerPage = (int)ViewState["RowPerPage"];

                pgcLeadAssign.PageCount = gvLeadAssign.PageCount;
                pgcLeadAssign.CurrentRowPerPage = rowPerPage.ToString();
                pgcLeadAssign.DisplayPaging();
            }
        }

        protected void lstTeamCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            //lblProjectName.Visible = false ;
            //txtProjectName.Visible = false ;
            //lblCutOff.Visible = false ;
            //calCutOffDate.Visible = false ;

            DataSet ds = null;
            int titleIndex = 0;
            string selectedTeamCode = "";

            //if (ddlTeamCode.SelectedIndex > 0)
            if(lstTeamCode.SelectedIndex>-1)
            {
                foreach(ListItem  li in lstTeamCode.Items)
                {                    
                    if (li.Selected)
                    {
                        selectedTeamCode = selectedTeamCode + "'" + li.Value + "',";
                    }
                }
                selectedTeamCode = selectedTeamCode.Remove(selectedTeamCode.Length - 1, 1);

                hidSelectedTeamCode.Value = selectedTeamCode;
                
               string assignmentType = ViewState["AssignmentType"] as string;

                if (assignmentType == "cross")
                {
                    leadsAssignmentService = new LeadsAssignmentService(base.dbConnectionStr);
                    ds = leadsAssignmentService.RetrieveCrossEnabledDealer(selectedTeamCode);
                }
                else
                {
                    ds = base.RetrieveMultipleDealerCodeAndNameByTeam(selectedTeamCode);
                    //ds = base.RetrieveDealerCodeAndNameByTeam(ddlTeamCode.SelectedValue.Trim());
                }


                if (ds.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString() == "1")
                {
                    ViewState["dtAssignDealer"] = ds.Tables[0];
                    if (ds.Tables[0].Rows.Count > 0)
                    {

                        gvDealer.DataSource = ds.Tables[0];
                    }
                    else
                    {
                        gvDealer.DataSource = null;
                    }
                    gvDealer.DataBind();

                    //Added for Email Sending
                    Dictionary<string, int> dealerList = ProcessAssignNumber(ds.Tables[0]);
                    ViewState["dealerList"] = dealerList;
                   
                }
                else {
                    gvDealer.DataSource = null;
                    gvDealer.DataBind();
                }


                gvLeadAssign.DataSource = null;
                gvLeadAssign.DataBind();

                rptAutoAssign.DataSource = null;
                rptAutoAssign.DataBind();

                btnSaveAssignment.Visible = false;
                btnAssignmentDelete.Visible = false;
                divPaging.Visible = false;
            }
        }

        private void PrepareForLeadsAssignment()
        {
            try
            {
                DataSet ds = null;
                string assignmentType = ViewState["AssignmentType"] as string;

                if (assignmentType == "cross")
                {
                    leadsAssignmentService = new LeadsAssignmentService(base.dbConnectionStr);
                    ds = leadsAssignmentService.RetrieveCrossEnabledTeam();
                }
                else
                {
                    //ds = base.RetrieveTeamCodeAndName();
                    bool isAdministrator = false;
                    DataSet dsDealerCode = null;
                    string aeCode = "";

                    CommonService commonService = new CommonService(base.dbConnectionStr);
                    dsDealerCode = commonService.RetrieveDealerCodeAndNameByUserID(base.userLoginId);
                    if (dsDealerCode.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString() == "1")
                    {

                        aeCode = dsDealerCode.Tables[0].Rows[0]["AECode"].ToString();
                        if (!String.IsNullOrEmpty(aeCode))
                        {
                            isAdministrator = CheckDealerAccessRight(aeCode);

                            if (isAdministrator)
                            {
                                CommonUtilities common = new CommonUtilities();
                                bool isTSeries = common.VerifyAccountType(aeCode);
                                ds = commonService.RetrieveTeamCodAndNameBySeries(base.userLoginId, isTSeries);
                            }
                            else
                            {
                                ds = commonService.RetrieveTeamAndNameByDealerCode(aeCode);
                            }
                        }
                    }
                    else
                    {
                        div1.InnerHtml = dsDealerCode.Tables["ReturnTable"].Rows[0]["ReturnMessage"].ToString();
                    }
                }

                //TeamName, TeamCode
                if (ds != null)
                {
                    if (ds.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString() == "1")
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            lstTeamCode.Items.Add(new ListItem(dr["TeamName"].ToString(), dr["TeamCode"].ToString()));
                        }
                    }
                    else
                    {
                        div1.InnerHtml = ds.Tables["ReturnTable"].Rows[0]["ReturnMessage"].ToString();
                    }
                }

                calCutOffDate.DateTextFromValue = DateTime.Now.AddDays(7.0).ToString("dd/MM/yyyy") + " 23:59:59";
            }
            catch (Exception ex)
            {
                div1.InnerHtml = ex.Message.ToString();
            }
            finally
            {
            }
        }

        private bool CheckDealerAccessRight(string aeCode)
        {
            DataSet dsDealerRight = null;
            string supervisor = "";
            DealerService dealerService = new DealerService(base.dbConnectionStr);
            dsDealerRight = dealerService.RetrieveDealerByCriteria("", aeCode, "", "", "", 1, "", "", "", base.userLoginId);
            if (dsDealerRight.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString() == "1")
            {
                for (int i = 0; i < dsDealerRight.Tables[0].Rows.Count; i++)
                {
                    supervisor = dsDealerRight.Tables[0].Rows[i]["Supervisor"].ToString();
                }
            }
            if (supervisor == "Y")
            {
                return true;
            }
            return false;
        }

        protected void gvLeadAssign_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                //Find the checkbox control in header and add an attribute
                ((CheckBox)e.Row.FindControl("gvchkSelectAll2")).Attributes.Add("onclick", "javascript:SelectAll2('" +
                        ((CheckBox)e.Row.FindControl("gvchkSelectAll2")).ClientID + "')");
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                
                DataTable dtLeadsAssign = ViewState["dtLeadsAssign"] as DataTable;

                string backColor = "", assignmentStatus = "NEW";

                bool assignFlag = false, deleteFlag = false;
                DateTime lastAssignDt = DateTime.Now;
                DateTime cutOffDt = DateTime.Now;

                //Set Current Date as Contact Date to check with CutOff date, if Contact Date is NULL.
                DateTime contactDt = DateTime.Now;

                string contactDtStr = "", cutOffDtStr = "";
                TextBox gvtxtCutOffDate = (TextBox)e.Row.FindControl("gvtxtCutOffDate");
                HiddenField gvhdfAssignStatus = (HiddenField)e.Row.FindControl("gvhdfAssignStatus");

                if (DataBinder.Eval(e.Row.DataItem, "LastCallDate") != null)
                {
                    contactDtStr = DataBinder.Eval(e.Row.DataItem, "LastCallDate").ToString();
                    if (!String.IsNullOrEmpty(contactDtStr))
                    {
                        //Get Contact Date
                        //If Contact Date is NULL, it will set Current Date as Contact Date for miss call checking
                        //contactDt = DateTime.ParseExact(contactDtStr, "yyyy-MM-dd HH:mm:ss", null);
                        contactDt = Convert.ToDateTime(contactDtStr);
                        
                        //e.Row.Cells[14].Text = contactDt.ToString("dd/MM/yyyy HH:mm:ss");
                    }                    
                }

                if (DataBinder.Eval(e.Row.DataItem, "LastCutOffDate") != null)
                {
                    if (!String.IsNullOrEmpty(DataBinder.Eval(e.Row.DataItem, "LastCutOffDate").ToString()))
                    {
                        //cutOffDt = DateTime.ParseExact(Convert.ToString(DataBinder.Eval(e.Row.DataItem, "CutOffDate")), "yyyy-MM-dd HH:mm:ss", null);
                        cutOffDt = Convert.ToDateTime(DataBinder.Eval(e.Row.DataItem, "LastCutOffDate"));

                        cutOffDtStr = cutOffDt.ToString("dd/MM/yyyy HH:mm:ss");

                        assignFlag = true;                        
                    }
                    else
                    {
                        //gvtxtCutOffDate.Text = calCutOffDate.DateTextFromValue;
                        cutOffDtStr = "";
                    }
                }

                if (assignFlag)
                {
                    if (DataBinder.Eval(e.Row.DataItem, "AssignDate") != null)
                    {
                        if (!String.IsNullOrEmpty(DataBinder.Eval(e.Row.DataItem, "AssignDate").ToString()))
                        {
                            //lastAssignDt = DateTime.ParseExact(Convert.ToString(DataBinder.Eval(e.Row.DataItem, "AssignDate")), "dd/MM/yyyy", null);

                            lastAssignDt = Convert.ToDateTime(DataBinder.Eval(e.Row.DataItem, "AssignDate"));
                            assignFlag = true;
                        }
                        else
                        {
                            assignFlag = false;
                        }
                    }
                }

                if (assignFlag)
                {
                    int compareResult = 0, assignCompareResult = 0;
                    assignmentStatus = "UPDATE";

                    compareResult = contactDt.CompareTo(cutOffDt);
                    assignCompareResult = contactDt.CompareTo(lastAssignDt);    //Compare ContactDate with Assignment
                    if (compareResult > 0)
                    {
                        // Miss Call
                        backColor = "#C00000";
                        cutOffDtStr = calCutOffDate.DateTextFromValue;
                    }                   
                    else if (String.IsNullOrEmpty(contactDtStr))    //It is for LastCallDate is NULL
                    {
                        //If contact date string is null, new assigned record withou making the contact yet.
                        //Else it is already contacted within valid cut off date.

                        //Valid Assignment
                        backColor = "#FFFF99";
                        deleteFlag = true;
                    }
                    else if ((compareResult < 1) && (assignCompareResult < 1))     //LastCallDate is not NULL and check for Miss Called
                    {                        
                        if (cutOffDt.CompareTo(DateTime.Now) < 1)
                        {
                            // Miss Call
                            backColor = "#C00000";
                            cutOffDtStr = calCutOffDate.DateTextFromValue;
                        }
                        else
                        {
                            //Valid Assignment without contact yet
                            backColor = "#FFFF99";
                            deleteFlag = true;
                        }
                    }
                    else if ((compareResult < 1) && (assignCompareResult > 0))  //LastCallDate is not NULL and check for already contacted
                    {
                        //Assignment which already contacted within valid Cut-off Date
                        assignmentStatus = "NEW";
                        deleteFlag = false;

                        cutOffDtStr = calCutOffDate.DateTextFromValue;
                    }
                    else
                    {
                        //Valid Assignment
                        backColor = "#FFFF99";
                        deleteFlag = true;
                    }
                }
                
                if (!assignFlag)
                {                    
                    //int twoNClient = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "TwoNClient"));
                    //if (twoNClient > 0)
                    //{
                    //    backColor = "#FFCCFF";
                    //}
                    //else if (DataBinder.Eval(e.Row.DataItem, "CoreAECode") != null)
                    //{
                    //    if (!String.IsNullOrEmpty(DataBinder.Eval(e.Row.DataItem, "CoreAECode").ToString()))
                    //    {
                    //        backColor = "#FFCC99";
                    //    }
                    //}

                    backColor = "WHITE";

                    //Fill Cut-off date for New Assignment Records
                    //gvtxtCutOffDate.Text = txtCutOffDate.Text;
                    //gvtxtCutOffDate.Text = calCutOffDate.DateTextFromValue;
                }

                //Fill CutOff textbox in Assignment GridView
                if (DataBinder.Eval(e.Row.DataItem, "CutOffDate") != null)
                {
                    if (!String.IsNullOrEmpty(DataBinder.Eval(e.Row.DataItem, "CutOffDate").ToString()))
                    {
                        gvtxtCutOffDate.Text = Convert.ToDateTime(DataBinder.Eval(e.Row.DataItem, "CutOffDate").ToString()).ToString("dd/MM/yyyy HH:mm:ss");
                    }
                }

                /*
                if (String.IsNullOrEmpty(cutOffDtStr))
                {
                    gvtxtCutOffDate.Text = calCutOffDate.DateTextFromValue.Trim();
                }
                else
                {
                    gvtxtCutOffDate.Text = cutOffDtStr.Trim();
                }
                */

                //dtLeadsAssign.Rows[e.Row.DataItemIndex]["CutOffDate"] = gvtxtCutOffDate.Text;

                //Set the Assignment Status (NEW or UPDATE)
                gvhdfAssignStatus.Value = assignmentStatus;
                
                System.Drawing.Color c = System.Drawing.ColorTranslator.FromHtml(backColor);
                e.Row.BackColor = c;


                //Hide delete button for not currently assigned assignment
                if (!deleteFlag)
                {
                    //Delete button visible off                    
                    foreach (Control control in e.Row.Cells[17].Controls)
                    {
                        if (control is Button)
                        {
                            control.Visible = false;
                            break;
                        }                                         
                    }                    
                }

                //Binding Dealers to Assign To dropdownlist in GridView
                DropDownList gvcboDealer = (DropDownList)e.Row.FindControl("gvcboDealer");
                if (gvcboDealer != null)
                {
                    DataTable dtAssignDealer = null;                    
                    string tableName = "dtAssignDealer";

                    if ((chkAutoAssign.Checked) && (assignmentStatus.Equals("NEW")))
                    {                        
                        tableName = "dtAutoAssignDealer";
                    }
                    
                    gvcboDealer = (DropDownList)e.Row.FindControl("gvcboDealer");
                    gvcboDealer.Items.Add(new ListItem("-----", ""));
                    dtAssignDealer = ViewState[tableName] as DataTable;
                    if (dtAssignDealer != null)
                    {
                        foreach (DataRow dr in dtAssignDealer.Rows)
                        {
                            gvcboDealer.Items.Add(new ListItem(dr["AECode"].ToString(), dr["AECode"].ToString()));
                        }
                        gvcboDealer.SelectedValue = DataBinder.Eval(e.Row.DataItem, "AssignDealer").ToString();
                    }
                }

                //for AccessRight

                DropDownList ddlAssignTo = (DropDownList)e.Row.Cells[15].FindControl("gvcboDealer");
                System.Web.UI.WebControls.Button btnDelete = (System.Web.UI.WebControls.Button)e.Row.Cells[17].FindControl("gvBtnDelete");

                if (ddlAssignTo != null)
                {

                   // if (accessRight[2] == "N" && accessRight[0]=="N")//not allow New and Modify
                    if(!(bool)ViewState["CreateAccessRight"] && !(bool)ViewState["ModifyAccessRight"])
                    {
                        ((DropDownList)e.Row.Cells[15].FindControl("gvcboDealer")).Enabled = false;
                        //btnSaveAssignment.Enabled = false;
                    }
                    //else if(accessRight[2] == "N" && accessRight[0]=="Y")//allow New but not Modify
                    else if ((bool)ViewState["CreateAccessRight"] && !(bool)ViewState["ModifyAccessRight"])
                    {
                        if (!assignFlag) //new
                        {
                            ((DropDownList)e.Row.Cells[15].FindControl("gvcboDealer")).Enabled = true;
                        }
                        else 
                        {
                            ((DropDownList)e.Row.Cells[15].FindControl("gvcboDealer")).Enabled = false;  
                        }
                        //btnSaveAssignment.Enabled = true;
                    }
                    //else if(accessRight[2] == "Y" && accessRight[0]=="N")//allow Modify but not New
                    else if (!(bool)ViewState["CreateAccessRight"] && (bool)ViewState["ModifyAccessRight"])
                    {
                        if (!assignFlag)//new
                        {
                            ((DropDownList)e.Row.Cells[15].FindControl("gvcboDealer")).Enabled = false;
                        }
                        else
                        {
                            ((DropDownList)e.Row.Cells[15].FindControl("gvcboDealer")).Enabled = true;
                        }
                        //btnSaveAssignment.Enabled = true;
                    }
                    //else if (accessRight[2] == "Y" && accessRight[0] == "Y")//allow Modify and New
                    else if ((bool)ViewState["CreateAccessRight"] && (bool)ViewState["ModifyAccessRight"])
                    {
                        ((DropDownList)e.Row.Cells[15].FindControl("gvcboDealer")).Enabled = true;
                        //btnSaveAssignment.Enabled = true;
                    }

                    #region old
                    /*
                    if (accessRight[2] == "N" && accessRight[0]=="N")//not allow New and Modify
                    {
                        ((DropDownList)e.Row.Cells[15].FindControl("gvcboDealer")).Enabled = false;
                        btnSaveAssignment.Enabled = false;
                    }
                    else if(accessRight[2] == "N" && accessRight[0]=="Y")//allow New but not Modify
                    {
                        if (!assignFlag) //new
                        {
                            ((DropDownList)e.Row.Cells[15].FindControl("gvcboDealer")).Enabled = true;
                        }
                        else 
                        {
                            ((DropDownList)e.Row.Cells[15].FindControl("gvcboDealer")).Enabled = false;  
                        }
                        btnSaveAssignment.Enabled = true;
                    }
                    else if(accessRight[2] == "Y" && accessRight[0]=="N")//allow Modify but not New
                    {
                        if (!assignFlag)//new
                        {
                            ((DropDownList)e.Row.Cells[15].FindControl("gvcboDealer")).Enabled = false;
                        }
                        else
                        {
                            ((DropDownList)e.Row.Cells[15].FindControl("gvcboDealer")).Enabled = true;
                        }
                        btnSaveAssignment.Enabled = true;
                    }
                    else if (accessRight[2] == "Y" && accessRight[0] == "Y")//allow Modify and New
                    {
                        ((DropDownList)e.Row.Cells[15].FindControl("gvcboDealer")).Enabled = true;
                        btnSaveAssignment.Enabled = true;
                    }*/
                    #endregion
                }

                if (btnDelete != null)
                {
                    ((System.Web.UI.WebControls.Button)e.Row.Cells[17].FindControl("gvBtnDelete")).Enabled = (bool)ViewState["DeleteAccessRight"];
                    //if (accessRight[3] == "N")//Delete
                    //{
                    //    ((System.Web.UI.WebControls.Button)e.Row.Cells[17].FindControl("gvBtnDelete")).Enabled = false;
                    //}
                    //else
                    //{
                    //    ((System.Web.UI.WebControls.Button)e.Row.Cells[17].FindControl("gvBtnDelete")).Enabled = true;
                    //}
                }

                //--------------

            }
        }

        /*
        protected void gvLeadAssign_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataTable dtAssignDealer = null;
                DropDownList gvcboDealer = null;
                string tableName = "dtAssignDealer";

                if (chkAutoAssign.Checked)
                {
                    tableName = "dtAutoAssignDealer";
                }

                dtAssignDealer = (DataTable)ViewState[tableName];
                gvcboDealer = (DropDownList)e.Row.FindControl("gvcboDealer");
                
                gvcboDealer.Items.Add(new ListItem("-----", "na"));
                
                foreach (DataRow dr in dtAssignDealer.Rows)
                {
                    gvcboDealer.Items.Add(new ListItem(dr["AECode"].ToString(), dr["AECode"].ToString()));
                }
            }
        }
        */

        protected void gvLeadAssign_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Hashtable ht = DeleteRecord(e.RowIndex);
            DataTable dtReturnData = null;
            //DataView dvReturnData = null;

            dtReturnData = (DataTable)ht["ReturnData"];

            gvLeadAssign.DataSource = dtReturnData;
            gvLeadAssign.DataBind();
            divMessage.InnerHtml = ht["ReturnMessage"].ToString();

            /*
              <asp:CommandField ButtonType="Button" ShowDeleteButton="True">
                            <ControlStyle CssClass="normal" />
                            <HeaderStyle CssClass="grayBorder" />
                            <ItemStyle CssClass="grayBorder" Width="50px"/>
                        </asp:CommandField>
             */
        }

        protected override Hashtable DeleteRecord(int deleteIndex)
        {
            //LastAssignDealer, AcctNo, AssignDate
            leadsAssignmentService = new LeadsAssignmentService(base.dbConnectionStr);
            Hashtable ht = new Hashtable();            
            DataTable dtLeadsAssign = (DataTable)ViewState["dtLeadsAssign"];

            //DataView dvClientAssign = (DataView)ViewState["dtLeadsAssign"];

            GridViewRow gvrDelete = gvLeadAssign.Rows[deleteIndex];
            HiddenField gvhdfLastAssignDealer = (HiddenField)gvrDelete.FindControl("gvhdfLastAssignDealer");
            //Delete by DataItem Index, gvhdfRecordId
            HiddenField gvhdfRecordId = (HiddenField)gvrDelete.FindControl("gvhdfRecordId");
            DataRow[] drArr = null;
            string filter = "", returnMessage = "", assignDate = "", cutOffDate = "", modifiedDate = "";
            int result = 1, dataRowIndex = 1;
            string[] wsReturn = null;

            dataRowIndex = int.Parse(gvhdfRecordId.Value);

            //Delete without insert into AuditLog
            //wsReturn = clientAssignmentService.DeleteClientAssignment(gvhdfLastAssignDealer.Value.Trim(),
            //    gvrDelete.Cells[1].Text.Trim(), DateTime.ParseExact( gvrDelete.Cells[16].Text.Trim(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd"));

            if (!String.IsNullOrEmpty(dtLeadsAssign.Rows[dataRowIndex]["AssignDate"].ToString()))
            {
                assignDate = Convert.ToDateTime(dtLeadsAssign.Rows[dataRowIndex]["AssignDate"]).ToString("yyyy-MM-dd");

            }
            if(!String.IsNullOrEmpty(dtLeadsAssign.Rows[dataRowIndex]["LastCutOffDate"].ToString()))
            {
            cutOffDate = Convert.ToDateTime(dtLeadsAssign.Rows[dataRowIndex]["LastCutOffDate"]).ToString("yyyy-MM-dd HH:mm:ss");
            
            }
            if(!String.IsNullOrEmpty(dtLeadsAssign.Rows[dataRowIndex]["ModifiedDate"].ToString()))
            {
                modifiedDate = Convert.ToDateTime(dtLeadsAssign.Rows[dataRowIndex]["ModifiedDate"]).ToString("yyyy-MM-dd HH:mm:ss");

            }
            //dtLeadsAssign.Rows[dataRowIndex]["LastAssignDealer"];
            //dtLeadsAssign.Rows[dataRowIndex]["AcctNo"];
            //dtLeadsAssign.Rows[dataRowIndex]["ModifiedUser"];

            wsReturn = leadsAssignmentService.DeleteLeadsAssignment(dtLeadsAssign.Rows[dataRowIndex]["LastAssignDealer"].ToString(),
                            dtLeadsAssign.Rows[dataRowIndex]["LeadId"].ToString(), assignDate, cutOffDate,
                            dtLeadsAssign.Rows[dataRowIndex]["ModifiedUser"].ToString(), modifiedDate, base.userLoginId);


            if (wsReturn[0].Equals("1"))
            {
                // Delete by using select from DataTable and remove method.
                /*
                filter = "AssignDealer = '" + gvhdfLastAssignDealer.Value + "' AND "
                        + " AcctNo = '" + gvrDelete.Cells[1].Text + "' AND "
                        + "AssignDate = '" + gvrDelete.Cells[16].Text + "'";

                drArr = dtLeadsAssign.Select(filter);
                dtLeadsAssign.Rows.Remove(drArr[0]);
                */


                dtLeadsAssign.Rows.RemoveAt(dataRowIndex);
                //dvClientAssign.Delete(int.Parse(gvhdfRecordId.Value));

                ViewState["dtLeadsAssign"] = dtLeadsAssign;
                returnMessage = "The Leads Assignment record is deleted successfully";
            }

            if (dtLeadsAssign.Rows.Count < 0)
            {
                btnSaveAssignment.Visible = false;
                btnAssignmentDelete.Visible = false;
            }

            ht.Add("ReturnCode", wsReturn[0]);
            ht.Add("ReturnData", dtLeadsAssign);
            ht.Add("ReturnMessage", wsReturn[1]);
            //ht.Add("ReturnMessage", "Row Count : " + drArr.Length + "<br />" + gvhdfLastAssignDealer.Value + ","
            //            + gvrDelete.Cells[1].Text + "," + gvhdfLastAssignDealer.Value);

            return ht;
        }

        protected override void SearchAndDisplay()
        {
            DataTable dtReturnData = null;
            Hashtable ht = RetrieveRecords();

            divMessage.InnerHtml = ht["ReturnMessage"].ToString();

            if (ht["ReturnData"] != null)
            {
                dtReturnData = (DataTable)ht["ReturnData"];
            }

            gvLeadAssign.PageIndex = 0;
            gvLeadAssign.DataSource = dtReturnData;
            gvLeadAssign.DataBind();

            //divMessage.InnerHtml = divMessage.InnerHtml + "<br /> Total Page : " + gvList.PageCount + ", Current Page : " + gvList.PageIndex;
            DisplayPaging();
        }

        protected override Hashtable RetrieveRecords()
        {

            btnLeadSearch.Enabled = false;
            divMessage.InnerHtml = "Search the Leadss record, Please wait ...";

            Hashtable ht = new Hashtable();
            string returnMessage = "";// validateResult = "";
            int returnCode = 1; //pageCount = 0;

            //clientAssignmentService = new ClientAssignmentService(base.dbConnectionStr);
            leadsAssignmentService = new LeadsAssignmentService(base.dbConnectionStr);
            
            DataSet ds = null;

            ht.Add("ReturnData", null);

            returnMessage = ValidateCutOffDate(calCutOffDate.DateTextFromValue.Trim());
            //validateResult = ValidateAccountDates(calAcctFromDate.DateTextFromValue, calAcctToDate.DateTextFromValue);

           // validateResult = CommonUtilities.ValidateDateRange(calAcctFromDate.DateTextFromValue.Trim(), calAcctToDate.DateTextFromValue.Trim(),
                                    //"Account Opening From Date", "Account Opening To Date");

            //if (String.IsNullOrEmpty(ddlTeamCode.SelectedValue))
            if(String.IsNullOrEmpty(lstTeamCode.SelectedValue))
            {
                returnCode = -1;
                returnMessage = "Please select team first before searching leads!";
            }
            else if (returnMessage != "OK")
            {
                returnCode = -1;
            }
            //else if (!String.IsNullOrEmpty(validateResult))
            //{
            //    returnCode = -1;
            //    returnMessage = validateResult;
            //}
            else
            {
                if (chkAutoAssign.Checked)
                {
                    if (CreateAutoAssignDealer() < 1)
                    {
                        returnMessage = "Please select Dealers for Auto Assignment.";
                        returnCode = -1;
                        
                        divPaging.Visible = false;
                    }
                }

                if (returnCode == 1)
                {
                    //ds = leadsAssignmentService.RetrieveLeadsForAssignment(ddlTeamCode.SelectedValue.Trim(), chk2NOnly.Checked, chkEmail.Checked,
                    //    chkMobile.Checked, DateTime.ParseExact(calAcctFromDate.DateTextFromValue.Trim(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd"),
                    //    DateTime.ParseExact(calAcctToDate.DateTextFromValue.Trim(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd"));

                    //lblProjectName.Visible = true;
                    //txtProjectName.Visible = true;
                    //lblCutOff.Visible = true;
                    //calCutOffDate.Visible = true;

                    calCutOffDate.DateTextRequiredText = "Select cutoff Date!";
                    calCutOffDate.DateTextRequired = true;


                    ds = leadsAssignmentService.RetrieveLeadsForAssignment(hidSelectedTeamCode.Value.ToString().Trim());
                    
                    returnCode = int.Parse(ds.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString());
                    returnMessage = ds.Tables["ReturnTable"].Rows[0]["ReturnMessage"].ToString();
                    
                    if (returnCode == 1)
                    {
                        btnSaveAssignment.Visible = true;
                        btnAssignmentDelete.Visible = true;
                                                                        
                        if (chkAutoAssign.Checked)
                        {
                            ProcessAutoAssign(ds.Tables[0]);
                        }
                        else
                        {
                            //Fill Cutoff Date for Miss Call and Already Contacted Assignment
                            InitailizeForAssignment(ds.Tables[0]);
                        }

                        ViewState["SortingString"] = "";
                        ViewState["dtLeadsAssign"] = ds.Tables[0];
                        ht["ReturnData"] = ds.Tables[0];                        
                        divPaging.Visible = true;
                    }                    
                }                
            }

            if (returnCode != 1)
            {
                //lblProjectName.Visible = false ;
                //txtProjectName.Visible = false;
                //lblCutOff.Visible = false;
                //calCutOffDate.Visible = false;

                calCutOffDate.DateTextRequiredText = "Select cutoff Date!";
                calCutOffDate.DateTextRequired = true;

                divPaging.Visible = false;
                btnSaveAssignment.Visible = false;
                btnAssignmentDelete.Visible = false;
                if (!chkAutoAssign.Checked)
                {
                    rptAutoAssign.DataSource = null;
                    rptAutoAssign.DataBind();
                }
            }
            
            ht.Add("ReturnCode", returnCode);
            ht.Add("ReturnMessage", returnMessage);

            btnLeadSearch.Enabled = true;

            return ht;
        }        

        private int CreateAutoAssignDealer()
        {
            DataTable dtAutoAssignDealer = new DataTable();
            dtAutoAssignDealer.Columns.Add("AECode", String.Empty.GetType());
            dtAutoAssignDealer.Columns.Add("AssignedCount", String.Empty.GetType());

            //CheckBox dlstchkAutoSelect = null;
           // Label dlstlblDealerCode = null;
            DataRow drAutoAssignDealer = null;

            CheckBox gvchkSelect= null;
            //Label lblDealerCode= null;

            int result = 0;

            //foreach (DataListItem dlItem in dlstDealer.Items)
            //{
            //    dlstchkAutoSelect = (CheckBox)dlItem.FindControl("dlstchkAutoSelect");
            //    if (dlstchkAutoSelect.Checked)
            //    {
            //        dlstlblDealerCode = (Label)dlItem.FindControl("dlstlblDealerCode");

            //        drAutoAssignDealer = dtAutoAssignDealer.NewRow();
            //        drAutoAssignDealer["AECode"] = dlstlblDealerCode.Text;
            //        dtAutoAssignDealer.Rows.Add(drAutoAssignDealer);
            //    }
            //}
            
            foreach (GridViewRow dr in gvDealer.Rows)
            {
                gvchkSelect = (CheckBox)dr.Cells[0].FindControl("gvchkAutoSelect");

                if (gvchkSelect.Checked)
                {
                    //lblDealerCode.Text = dr["AECode"].ToString(); //(Label)dlItem.FindControl("dlstlblDealerCode");

                    drAutoAssignDealer = dtAutoAssignDealer.NewRow();
                    drAutoAssignDealer["AECode"] = dr.Cells[1].Text.Trim();//dr.Cells["AECode"].ToString();//dlstlblDealerCode.Text;
                    
                    dtAutoAssignDealer.Rows.Add(drAutoAssignDealer);
                }
            }

            result = dtAutoAssignDealer.Rows.Count;            
            ViewState["dtAutoAssignDealer"] = dtAutoAssignDealer;

            return result;
        }

        private void ProcessAutoAssign(DataTable dtLeadsAssign)
        {
            //DataTable dtLeadsAssign = (DataTable)ViewState["dtLeadsAssign"];

            DataTable dtAutoAssignDealer = (DataTable)ViewState["dtAutoAssignDealer"];
            DateTime newCutOffDate = DateTime.ParseExact(calCutOffDate.DateTextFromValue.Trim(), "dd/MM/yyyy HH:mm:ss", null);
            int[] autoAssignCount = new int[dtAutoAssignDealer.Rows.Count];
            int dealerIndex = 0;
            int dealerSize = autoAssignCount.Length - 1;

            int cutOffCompareResult = 0, assignCompareResult = 0;
            DateTime? cutOffDt = null;
            DateTime? assignDt = null;
            DateTime todayDt = DateTime.Now;

            //Set Current Date as Contact Date to check with CutOff date, if Contact Date is NULL.
            DateTime contactDt = DateTime.Now;


            //Initialize count to zero
            for (int i = 0; i < autoAssignCount.Length; i++)
            {
                autoAssignCount[i] = 0;
            }
            
            foreach (DataRow drClientAssign in dtLeadsAssign.Rows)
            {
                //CheckForValidAssignment
                if ((!String.IsNullOrEmpty(drClientAssign["AssignDate"].ToString())) && (!String.IsNullOrEmpty(drClientAssign["CutOffDate"].ToString()))
                        )
                {
                    if(String.IsNullOrEmpty(drClientAssign["LastCallDate"].ToString()))
                    {
                        contactDt = DateTime.Now;
                    }
                    else
                    {
                        contactDt = Convert.ToDateTime(drClientAssign["LastCallDate"].ToString());
                    }

                    assignDt = Convert.ToDateTime(drClientAssign["AssignDate"].ToString());
                    cutOffDt = Convert.ToDateTime(drClientAssign["CutOffDate"].ToString());

                    cutOffCompareResult = contactDt.CompareTo(cutOffDt.Value);
                    assignCompareResult = contactDt.CompareTo(assignDt.Value);

                    //Check for Valid Assignment
                    if ((cutOffCompareResult < 0) && (assignCompareResult > 0) && (String.IsNullOrEmpty(drClientAssign["LastCallDate"].ToString())))
                    {
                        continue;
                    }
                    else if ((assignCompareResult < 1) && (cutOffDt.Value.CompareTo(todayDt) > 0))
                    {
                        continue;
                    }
                }
               
                drClientAssign["AssignDealer"] = dtAutoAssignDealer.Rows[dealerIndex]["AECode"].ToString();
                drClientAssign["CutOffDate"] = newCutOffDate;
                autoAssignCount[dealerIndex] = autoAssignCount[dealerIndex] + 1;
                dealerIndex++;

                if (dealerIndex > dealerSize)
                    dealerIndex = 0;                
            }

            for (int j = 0; j < autoAssignCount.Length; j++)
            {
                dtAutoAssignDealer.Rows[j]["AssignedCount"] = autoAssignCount[j] + "";
            }

            ViewState["dtAutoAssignDealer"] = dtAutoAssignDealer;
            rptAutoAssign.DataSource = dtAutoAssignDealer;
            rptAutoAssign.DataBind();
        }

        protected void btnSaveAssignment_Click(object sender, EventArgs e)
        {
            string[] ProjectInfo = null;
            string projectID = "";

            ProjectInfo = SaveProjectInformation();

            if (ProjectInfo != null)
            {
                projectID = ProjectInfo[0].ToString();
                if (projectID != "")
                {
                    //save changes on current page before saving records to database.
                    if (SaveLeadsAssignForPageChange())
                    {
                        DataTable dtLeadsAssign = (DataTable)ViewState["dtLeadsAssign"];
                        DataTable dtSaveAssign = new DataTable();

                        //Set Nullable value to null. To check value use => if (cutOffDt.HasValue)
                        DateTime? cutOffDt = null;
                        DateTime? assignDt = null;
                        DateTime todayDt = DateTime.Now;

                        //Set Current Date as Contact Date to check with CutOff date, if Contact Date is NULL.
                        DateTime contactDt = DateTime.Now;
                        string assignmentStatus = "";
                        bool assignmentNullFlag = false, insertFlag = false, emptyFlag = true;
                        int result = 1;

                        List<int> savedIndexList = new List<int>();
                        string[] wsReturn = null;

                        dtSaveAssign.Columns.Add("ContactDate", String.Empty.GetType());
                        dtSaveAssign.Columns.Add("AEGroup", String.Empty.GetType());
                        dtSaveAssign.Columns.Add("LeadId", String.Empty.GetType());
                        dtSaveAssign.Columns.Add("AssignDealer", String.Empty.GetType());
                        dtSaveAssign.Columns.Add("AssignmentStatus", String.Empty.GetType());
                        dtSaveAssign.Columns.Add("AssignDate", String.Empty.GetType());
                        dtSaveAssign.Columns.Add("CutOffDate", String.Empty.GetType());
                        dtSaveAssign.Columns.Add("ModifiedUser", String.Empty.GetType());
                        dtSaveAssign.Columns.Add("LastAssignDealer", String.Empty.GetType());
                        dtSaveAssign.Columns.Add("OldCutOffDate", String.Empty.GetType());
                        dtSaveAssign.Columns.Add("OldModifiedUser", String.Empty.GetType());
                        dtSaveAssign.Columns.Add("OldModifiedDate", String.Empty.GetType());
                        dtSaveAssign.Columns.Add("ProjectID", String.Empty.GetType());

                        for (int i = 0; i < dtLeadsAssign.Rows.Count; i++)
                        {
                            if ((!String.IsNullOrEmpty(dtLeadsAssign.Rows[i]["AssignDealer"].ToString())) &&
                                    (!String.IsNullOrEmpty(dtLeadsAssign.Rows[i]["CutOffDate"].ToString())))
                            {
                                //initialized as New Assignment
                                assignmentStatus = ASSIGNMENT_NEW;
                                insertFlag = true;

                                //Check for Existing Assignment or not
                                assignmentNullFlag = String.IsNullOrEmpty(dtLeadsAssign.Rows[i]["AssignDate"].ToString());
                                if (!assignmentNullFlag)
                                {
                                    assignmentStatus = ASSIGNMENT_UPDATE;
                                    //assignDt = DateTime.ParseExact(dtLeadsAssign.Rows[i]["AssignDate"].ToString(), "dd/MM/yyyy", null);
                                    assignDt = Convert.ToDateTime(dtLeadsAssign.Rows[i]["AssignDate"]);

                                    if (String.IsNullOrEmpty(dtLeadsAssign.Rows[i]["LastCallDate"].ToString()))
                                    {
                                        //If contact date is null, assume current date as contact date to check with cut-off date
                                        contactDt = DateTime.Now;
                                    }
                                    else
                                    {
                                        //contactDt = DateTime.ParseExact(dtLeadsAssign.Rows[i]["LastCallDate"].ToString(), "yyyy-MM-dd HH:mm:ss", null);
                                        contactDt = Convert.ToDateTime(dtLeadsAssign.Rows[i]["LastCallDate"].ToString());
                                    }


                                    //Compare with Current Cutoff Date (not correct with user change Cutoff Date textbox)
                                    //cutOffDt = DateTime.ParseExact(dtLeadsAssign.Rows[i]["CutOffDate"].ToString(), "yyyy-MM-dd HH:mm:ss", null);

                                    //compare with LastCutOffDate. It cannot be null because this condition will process when there is Assignment.
                                    //If there is assignment, there will be CutOffDate. 
                                    //cutOffDt = DateTime.ParseExact(dtLeadsAssign.Rows[i]["LastCutOffDate"].ToString(), "yyyy-MM-dd HH:mm:ss", null);

                                    /// <Updated by OC>
                                    cutOffDt = Convert.ToDateTime(dtLeadsAssign.Rows[i]["LastCutOffDate"].ToString());

                                    int compareResult = contactDt.CompareTo(cutOffDt);
                                    int assignCompareResult = contactDt.CompareTo(assignDt.Value);

                                    if (compareResult > 0)
                                    {
                                        //miss called, update assignment        (Inital Requirement)
                                        //assignmentStatus = ASSIGNMENT_UPDATE;

                                        //Miss Called should allow for New Assignment       (Changes Requirement) 
                                        assignmentStatus = ASSIGNMENT_NEW;
                                    }
                                    else if (String.IsNullOrEmpty(dtLeadsAssign.Rows[i]["LastCallDate"].ToString()))
                                    {
                                        //not contacted assignment but still has valid cut-off date
                                        assignmentStatus = ASSIGNMENT_UPDATE;
                                    }
                                    else if ((compareResult < 0) && (assignCompareResult > 0))     //Assignment is already contacted with valid CutOff Date
                                    {
                                        assignmentStatus = ASSIGNMENT_NEW;
                                    }
                                    else if ((assignCompareResult < 1) && (cutOffDt.Value.CompareTo(todayDt) > 0))
                                    {
                                        //not contacted assignment but still has valid cut-off date
                                        assignmentStatus = ASSIGNMENT_UPDATE;
                                    }
                                    else if (assignCompareResult < 1)
                                    {
                                        //Miss Called with old contacted date
                                        assignmentStatus = ASSIGNMENT_NEW;
                                    }
                                }

                                if (assignmentStatus.Equals(ASSIGNMENT_UPDATE))
                                {
                                    if ((dtLeadsAssign.Rows[i]["AssignDealer"].ToString().
                                        Equals(dtLeadsAssign.Rows[i]["LastAssignDealer"].ToString())) &&
                                        (dtLeadsAssign.Rows[i]["CutOffDate"].ToString().
                                        Equals(dtLeadsAssign.Rows[i]["LastCutOffDate"].ToString())))
                                    {
                                        insertFlag = false;
                                    }
                                    else
                                    {
                                        //Checking for CutOff Date
                                        //cutOffDt = DateTime.ParseExact(dtLeadsAssign.Rows[i]["CutOffDate"].ToString(), "yyyy-MM-dd HH:mm:ss", null);
                                        cutOffDt = Convert.ToDateTime(dtLeadsAssign.Rows[i]["CutOffDate"].ToString());

                                        //assignDt = DateTime.ParseExact(dtLeadsAssign.Rows[i]["AssignDate"].ToString(), "dd/MM/yyyy", null);
                                        assignDt = Convert.ToDateTime(dtLeadsAssign.Rows[i]["AssignDate"]);

                                        if (cutOffDt.Value.CompareTo(assignDt) < 1)
                                        {
                                            insertFlag = false;
                                        }
                                    }
                                }
                                else
                                {
                                    //NEW Assignment

                                    //cutOffDt = DateTime.ParseExact(dtLeadsAssign.Rows[i]["CutOffDate"].ToString(), "yyyy-MM-dd HH:mm:ss", null);
                                    cutOffDt = Convert.ToDateTime(dtLeadsAssign.Rows[i]["CutOffDate"].ToString());

                                    if (cutOffDt.Value.CompareTo(DateTime.Now) > 0)
                                    {
                                        //dtLeadsAssign.Rows[i]["AssignDate"] = DateTime.Now.ToString("dd/MM/yyyy");
                                        dtLeadsAssign.Rows[i]["AssignDate"] = DateTime.Now;
                                    }
                                    else
                                    {
                                        insertFlag = false;
                                    }
                                }

                                if (insertFlag)
                                {
                                    DataRow drNewRow = dtSaveAssign.NewRow();
                                    drNewRow["AssignDealer"] = dtLeadsAssign.Rows[i]["AssignDealer"].ToString();
                                    drNewRow["LeadId"] = dtLeadsAssign.Rows[i]["LeadId"].ToString();

                                    //drNewRow["AssignDate"] = dtLeadsAssign.Rows[i]["AssignDate"].ToString(); //Before Date Sorting

                                    //Correct Date Format, but need to parse again in DataAccess layer
                                    //drNewRow["AssignDate"] = Convert.ToDateTime(dtLeadsAssign.Rows[i]["AssignDate"].ToString()).ToString("dd/MM/yyyy");
                                    drNewRow["AssignDate"] = Convert.ToDateTime(dtLeadsAssign.Rows[i]["AssignDate"].ToString()).ToString("yyyy-MM-dd");

                                    //drNewRow["CutOffDate"] = dtLeadsAssign.Rows[i]["CutOffDate"].ToString();
                                    drNewRow["CutOffDate"] = Convert.ToDateTime(dtLeadsAssign.Rows[i]["CutOffDate"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");

                                    drNewRow["AssignmentStatus"] = assignmentStatus;
                                    drNewRow["LastAssignDealer"] = dtLeadsAssign.Rows[i]["LastAssignDealer"].ToString();
                                    drNewRow["ModifiedUser"] = base.userLoginId;

                                    //For AuditLog
                                    if (assignmentStatus.Equals(ASSIGNMENT_UPDATE))
                                    {
                                        drNewRow["OldModifiedUser"] = dtLeadsAssign.Rows[i]["ModifiedUser"].ToString();
                                        drNewRow["OldModifiedDate"] = Convert.ToDateTime(dtLeadsAssign.Rows[i]["ModifiedDate"]).ToString("yyyy-MM-dd HH:mm:ss");
                                        drNewRow["OldCutOffDate"] = Convert.ToDateTime(dtLeadsAssign.Rows[i]["LastCutOffDate"]).ToString("yyyy-MM-dd HH:mm:ss");
                                    }
                                    drNewRow["ProjectID"] = projectID;
                                    dtSaveAssign.Rows.Add(drNewRow);

                                    //Store row index to update LastAssignDealer with CurrentAssignDealer and LastCutOffDate with CutOffDate
                                    savedIndexList.Add(i);

                                    //Update for AuditLog
                                    if (assignmentStatus.Equals(ASSIGNMENT_NEW))
                                    {
                                        dtLeadsAssign.Rows[i]["ModifiedUser"] = base.userLoginId;
                                        dtLeadsAssign.Rows[i]["ModifiedDate"] = DateTime.Now;
                                    }
                                }
                            }
                        } //End of Assignments for loop   

                        if (dtSaveAssign.Rows.Count > 0)
                        {
                            DataSet ds = new DataSet();
                            ds.Tables.Add(dtSaveAssign);
                            leadsAssignmentService = new LeadsAssignmentService(base.dbConnectionStr);


                            wsReturn = leadsAssignmentService.InsertLeadsAssignment(ds, projectID);
                            if (wsReturn[0].Equals("1"))
                            {
                                SentAnnouncementEmailToDealer(dtSaveAssign);

                                //Update LastAssignDealer with CurrentAssignDealer and LastCutOffDate with CutOffDate                       
                                foreach (int i in savedIndexList)
                                {
                                    dtLeadsAssign.Rows[i]["LastAssignDealer"] = dtLeadsAssign.Rows[i]["AssignDealer"].ToString();
                                    dtLeadsAssign.Rows[i]["LastCutOffDate"] = dtLeadsAssign.Rows[i]["CutOffDate"].ToString();
                                }

                                ViewState["dtLeadsAssign"] = dtLeadsAssign;
                                gvLeadAssign.DataSource = dtLeadsAssign;
                                gvLeadAssign.DataBind();
                            }
                            divMessage.InnerHtml = wsReturn[1];
                        }
                        else
                        {
                            divMessage.InnerHtml = "There is no assignments to save!";
                        }
                    }
                } //End of checking error in SaveAssignment for Current Page        
            }

            txtProjectName.Text = "";
        }

        private string[] SaveProjectInformation()
        {
            if (txtProjectName.Text == "")
            {
                //lblReqField.Visible = true;
                //rfvProjectName.Visible = true;     

                divMessage.InnerHtml = "Project Name cannot be blank!"; return null;
            }

            if (ValidateProjectNameIsDuplicate(txtProjectName.Text))
            {
                divMessage.InnerHtml = "Project Name already exists!"; return null;
            }

            string[] result = null;
            string proName = "";
           
            proName = txtProjectName.Text;
            //DateTime cutOffDate = Convert.ToDateTime(calCutOffDate.DateTextFromValue);


            
            IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
            String datetime = calCutOffDate.DateTextFromValue;
            DateTime cutOffDate = DateTime.Parse(datetime, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                        
            if (proName == "")
            {
                //lblReqField.Visible = true;
                //rfvProjectName.Visible = true;     
                result = new string[] { "Project Name cannot be blank!", "" };
                divMessage.InnerHtml = "Project Name cannot be blank!";
            }
            else
            {
                try
                {   
                    leadsAssignmentService = new LeadsAssignmentService(base.dbConnectionStr);
                    string[] wsReturnInfo = leadsAssignmentService.SaveProjectInformation(proName,cutOffDate);
                    if (wsReturnInfo[0].Equals("1"))
                    {
                        result = new string[] { wsReturnInfo[1].ToString(),""};
                        // result = wsReturnInfo[1].ToString();
                    }
                    else
                    {
                        divMessage.InnerHtml = wsReturnInfo[1];
                    }
                }
                catch (Exception ex)
                {
                    result = new string[] { "", "" };
                }
            }
            return result;
        }

        private bool ValidateProjectNameIsDuplicate(String projectName)
        {
            ClientAssignmentService clientAssignmentService = new ClientAssignmentService(base.dbConnectionStr);
            DataSet ds = clientAssignmentService.RetrieveAllProjectInfo();
            if (ds.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString() == "1")
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (projectName.Equals(dr["ProjectName"].ToString()))
                    {                        
                        return true;
                    }
                }
            }

            return false;
        }
               
        private bool SaveLeadsAssignForPageChange()
        {
            DataTable dtLeadsAssign = (DataTable)ViewState["dtLeadsAssign"];

            DropDownList gvcboDealer = null;
            TextBox gvtxtCutOffDate = null;
            HiddenField gvhdfRecordId = null;
            DateTime cutOffDate = DateTime.Now;
            string cutoffDateStr = "", leadId = "", assignDealer = "";
            bool saveResult = true;
            int rowIndex = 0;            

            for (int i = 0; i < gvLeadAssign.Rows.Count; i++)
            {                
                gvhdfRecordId = (HiddenField)gvLeadAssign.Rows[i].FindControl("gvhdfRecordId");
                gvcboDealer = (DropDownList)gvLeadAssign.Rows[i].FindControl("gvcboDealer");
                gvtxtCutOffDate = (TextBox)gvLeadAssign.Rows[i].FindControl("gvtxtCutOffDate");

                rowIndex = int.Parse(gvhdfRecordId.Value);

                if (!CommonUtilities.CheckDateFormat(gvtxtCutOffDate.Text.Trim(), "dd/MM/yyyy HH:mm:ss"))
                {
                    leadId = dtLeadsAssign.Rows[rowIndex]["LeadId"].ToString();
                    saveResult = false;
                    break;
                }
                                
                if (!gvcboDealer.SelectedValue.Equals(dtLeadsAssign.Rows[rowIndex]["AssignDealer"].ToString()))
                {                   
                    dtLeadsAssign.Rows[rowIndex]["AssignDealer"] = gvcboDealer.SelectedValue.Trim();
                }

                //Previously compare with CutOffDate
                if ((dtLeadsAssign.Rows[rowIndex]["CutOffDate"] != null) && 
                        (!String.IsNullOrEmpty(dtLeadsAssign.Rows[rowIndex]["CutOffDate"].ToString())))
                {
                    cutoffDateStr = Convert.ToDateTime(dtLeadsAssign.Rows[rowIndex]["CutOffDate"].ToString()).ToString("dd/MM/yyyy HH:mm:ss");
                }
                
                if (!gvtxtCutOffDate.Text.Trim().Equals(cutoffDateStr))
                {
                    if (String.IsNullOrEmpty(gvtxtCutOffDate.Text.Trim()))
                    {
                        //dtLeadsAssign.Rows[rowIndex]["CutOffDate"] = gvtxtCutOffDate.Text.Trim();
                        dtLeadsAssign.Rows[rowIndex]["CutOffDate"] = DBNull.Value;
                    }
                    else
                    {
                        //dtLeadsAssign.Rows[rowIndex]["CutOffDate"] = DateTime.ParseExact(gvtxtCutOffDate.Text, "dd/MM/yyyy HH:mm:ss", null).ToString("yyyy-MM-dd HH:mm:ss");
                        dtLeadsAssign.Rows[rowIndex]["CutOffDate"] = DateTime.ParseExact(gvtxtCutOffDate.Text, "dd/MM/yyyy HH:mm:ss", null);
                    }
                }                
            }

            if (saveResult)
            {
                ViewState["dtLeadsAssign"] = dtLeadsAssign;
                divMessage.InnerHtml = "";
            }
            else
            {
                divMessage.InnerHtml = "CutOff Date Format should be dd/MM/yyyy HH:mm:ss";
            }

            return saveResult;
        }

        //Prepare New, Already Contacted and Miss Call Assignments for New Assignment
        private void InitailizeForAssignment(DataTable dtLeadsAssignment)
        {
            int cutOffCompareResult = 0, assignCompareResult = 0;
            DateTime? cutOffDt = null;
            DateTime? assignDt = null;
            DateTime todayDt = DateTime.Now;

            //Set Current Date as Contact Date to check with CutOff date, if Contact Date is NULL.
            DateTime contactDt = DateTime.Now;


            foreach (DataRow dr in dtLeadsAssignment.Rows)
            {
                //String.IsNullOrEmpty(dr["LastCallDate"].ToString())
                if ((!String.IsNullOrEmpty(dr["AssignDate"].ToString())) && (!String.IsNullOrEmpty(dr["CutOffDate"].ToString())))
                {
                    if (String.IsNullOrEmpty(dr["LastCallDate"].ToString()))
                    {
                        contactDt = DateTime.Now;
                    }
                    else
                    {
                        contactDt = Convert.ToDateTime(dr["LastCallDate"].ToString());
                    }

                    assignDt = Convert.ToDateTime(dr["AssignDate"].ToString());
                    cutOffDt = Convert.ToDateTime(dr["CutOffDate"].ToString());

                    cutOffCompareResult = contactDt.CompareTo(cutOffDt.Value);
                    assignCompareResult = contactDt.CompareTo(assignDt.Value);

                    //Check for Valid Assignment
                    if ((cutOffCompareResult < 0) && (assignCompareResult > 0) && (String.IsNullOrEmpty(dr["LastCallDate"].ToString())))
                    {
                        continue;
                    }
                    else if ((assignCompareResult < 1) && (cutOffDt.Value.CompareTo(todayDt) > 0))
                    {
                        continue;
                    }
                }

                dr["AssignDealer"] = "";
                dr["CutOffDate"] = DateTime.ParseExact(calCutOffDate.DateTextFromValue.Trim(), "dd/MM/yyyy HH:mm:ss", null);
            }
        }

        //For GridView Sorting
        protected void gvLeadAssign_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Save current changes before sorting
            if (SaveLeadsAssignForPageChange())
            {
                DataTable dtLeadsAssign = ViewState["dtLeadsAssign"] as DataTable;
                DataTable sortedClientAssign = dtLeadsAssign.Clone();
                DataRow[] sortedRows = null;

                string sortDirection = "", sortString = "";

                sortDirection = GetSortDirection(e.SortExpression);
                sortString = e.SortExpression + " " + sortDirection;

                //Sorting with DataTable
                sortedRows = dtLeadsAssign.Select("", sortString);
                foreach (DataRow dr in sortedRows)
                {
                    sortedClientAssign.ImportRow(dr);
                }
                ViewState["dtLeadsAssign"] = sortedClientAssign;
                /////////////////////////////

                ViewState["SortingString"] = sortString;

                gvLeadAssign.PageIndex = 0;
                gvLeadAssign.DataSource = sortedClientAssign;
                gvLeadAssign.DataBind();

                DisplayPaging();
            }
        }

        private string GetSortDirection(string column)
        {

            // By default, set the sort direction to ascending.
            string sortDirection = "ASC";

            // Retrieve the last column that was sorted.
            string sortExpression = ViewState["SortExpression"] as string;

            if (sortExpression != null)
            {
                // Check if the same column is being sorted.
                // Otherwise, the default value can be returned.
                if (sortExpression == column)
                {
                    string lastDirection = ViewState["SortDirection"] as string;
                    if ((lastDirection != null) && (lastDirection == "ASC"))
                    {
                        sortDirection = "DESC";
                    }
                }
            }

            // Save new values in ViewState.
            ViewState["SortDirection"] = sortDirection;
            ViewState["SortExpression"] = column;

            return sortDirection;
        }
    
    
        //Helper Methods
        private bool CheckForValidAssignment(string assignDtStr, string cutOffDtStr, DateTime contactDt)
        {
            int cutOffCompareResult = 0, assignCompareResult = 0;
            DateTime? cutOffDt = null;
            DateTime? assignDt = null;
            bool result = false;

            assignDt = Convert.ToDateTime(assignDtStr);
            cutOffDt = Convert.ToDateTime(cutOffDtStr);

            cutOffCompareResult = contactDt.CompareTo(cutOffDt.Value);
            assignCompareResult = contactDt.CompareTo(assignDt.Value);

                       
            if ((cutOffCompareResult < 0) && (assignCompareResult > 0))
            {
                result = true;
            }
           

            return result;
        }

        private string ValidateCutOffDate(string cutOffDtStr)
        {
            string returnMessage = "OK";
            DateTime cutOffDt = DateTime.Now;

            try
            {
                if (String.IsNullOrEmpty(cutOffDtStr))
                {
                    returnMessage = "Cutoff Date can not be blank!";
                }
                else
                {
                    cutOffDt = DateTime.ParseExact(cutOffDtStr, "dd/MM/yyyy HH:mm:ss", null);
                    if (cutOffDt.CompareTo(DateTime.Now.AddDays(1.0)) < 1)
                    {
                        returnMessage = "CutOff Date must be one day after current date!";
                    }
                }
            }
            catch(Exception e)
            {
                returnMessage = "CutOff Date format must be dd/MM/yyyy HH:mm:ss";
            }

            return returnMessage;
        }     

        protected void gvDealer_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                //Find the checkbox control in header and add an attribute
                ((CheckBox)e.Row.FindControl("gvchkSelectAll")).Attributes.Add("onclick", "javascript:SelectAll('" +
                        ((CheckBox)e.Row.FindControl("gvchkSelectAll")).ClientID + "')");
            }

        }

        protected void btnLeadSearch_Click(object sender, EventArgs e)
        {            
            base.btnSearch_Click(sender, e);
        }

        protected void btnAssignmentDelete_Click(object sender, EventArgs e)
        {
            int countCheck = 0;
            foreach (GridViewRow row in gvLeadAssign.Rows)
            {
                CheckBox cb = (CheckBox)row.FindControl("chkRecID");
                if ((cb != null) && (cb.Checked))
                {
                    countCheck++; break;
                }
            }
            if (countCheck == 0)
            {
                divMessage.InnerHtml = "Please tick in the checkbox to do batch delete."; return;
            }
            
            DataSet dsResult = new DataSet();
            string filter = "", returnMessage = "";
            int result = 1;
            leadsAssignmentService = new LeadsAssignmentService(base.dbConnectionStr);
            DataTable dtLeadsAssign = (DataTable)ViewState["dtLeadsAssign"];

            //Create delete data table
            DataTable dtAssignDelete = new DataTable();
            dtAssignDelete.Columns.Add("DataRowIndex", String.Empty.GetType());
            dtAssignDelete.Columns.Add("dealerCode", String.Empty.GetType());
            dtAssignDelete.Columns.Add("leadId", String.Empty.GetType());
            dtAssignDelete.Columns.Add("assignDate", String.Empty.GetType());
            dtAssignDelete.Columns.Add("cutOffDate", String.Empty.GetType());
            dtAssignDelete.Columns.Add("modifiedUser", String.Empty.GetType());
            dtAssignDelete.Columns.Add("modifiedDate", String.Empty.GetType());
            dtAssignDelete.Columns.Add("newModifiedUser", String.Empty.GetType());

            DataRow drAssignDelete = null;

            //get dataRow to delete
            foreach (GridViewRow row in gvLeadAssign.Rows)
            {
                CheckBox cb = (CheckBox)row.FindControl("chkRecID");
                if ((cb != null) && (cb.Checked))
                {
                    GridViewRow gvrDelete = row;

                    HiddenField gvhdfLastAssignDealer = (HiddenField)gvrDelete.FindControl("gvhdfLastAssignDealer");
                    HiddenField gvhdfRecordId = (HiddenField)gvrDelete.FindControl("gvhdfRecordId");
                    string assignDate = "", cutOffDate = "", modifiedDate = "";
                    int dataRowIndex = 1;

                    dataRowIndex = int.Parse(gvhdfRecordId.Value);

                    if (!String.IsNullOrEmpty(dtLeadsAssign.Rows[dataRowIndex]["AssignDate"].ToString()))
                    {
                        assignDate = Convert.ToDateTime(dtLeadsAssign.Rows[dataRowIndex]["AssignDate"]).ToString("yyyy-MM-dd");
                    }
                    if (!String.IsNullOrEmpty(dtLeadsAssign.Rows[dataRowIndex]["LastCutOffDate"].ToString()))
                    {
                        cutOffDate = Convert.ToDateTime(dtLeadsAssign.Rows[dataRowIndex]["LastCutOffDate"]).ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    if (!String.IsNullOrEmpty(dtLeadsAssign.Rows[dataRowIndex]["ModifiedDate"].ToString()))
                    {
                        modifiedDate = Convert.ToDateTime(dtLeadsAssign.Rows[dataRowIndex]["ModifiedDate"]).ToString("yyyy-MM-dd HH:mm:ss");

                    }

                    drAssignDelete = dtAssignDelete.NewRow();
                    drAssignDelete["DataRowIndex"] = dataRowIndex;
                    drAssignDelete["dealerCode"] = dtLeadsAssign.Rows[dataRowIndex]["LastAssignDealer"].ToString();
                    drAssignDelete["leadId"] = dtLeadsAssign.Rows[dataRowIndex]["LeadId"].ToString();
                    drAssignDelete["assignDate"] = assignDate;
                    drAssignDelete["cutOffDate"] = cutOffDate;
                    drAssignDelete["modifiedUser"] = dtLeadsAssign.Rows[dataRowIndex]["ModifiedUser"].ToString();
                    drAssignDelete["modifiedDate"] = modifiedDate;
                    drAssignDelete["newModifiedUser"] = base.userLoginId;

                    dtAssignDelete.Rows.Add(drAssignDelete);
                }

            }

            dsResult = leadsAssignmentService.BatchDeleteLeadsAssignment(dtAssignDelete);
            if (dsResult.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString() == "1")
            {
                divMessage.InnerHtml  = dsResult.Tables["ReturnTable"].Rows[0]["ReturnMessage"].ToString();
                DataTable dtTable = new DataTable();
                dtTable = dsResult.Tables[0];
                int noOfIndex = 0;
                for (int i = 0; i < dtTable.Rows.Count; i++)
                {
                    if (Convert.ToInt32(dtTable.Rows[i]["result"].ToString()) > 0)
                    {
                        int index = Convert.ToInt32(dtTable.Rows[i]["DataRowIndex"].ToString());
                        if (i == 0)
                        {
                            dtLeadsAssign.Rows.RemoveAt(index);
                            noOfIndex = noOfIndex + 1;            
                        }
                        else
                        {
                            dtLeadsAssign.Rows.RemoveAt(index - noOfIndex);
                            noOfIndex = noOfIndex + 1;            
                        }
                    }
                }

                ViewState["dtLeadsAssign"] = dtLeadsAssign;
                if (dtLeadsAssign.Rows.Count < 0)
                {
                    btnSaveAssignment.Visible = false;
                    btnAssignmentDelete.Visible = false;
                    pgcLeadAssign.Visible = false;
                }

                gvLeadAssign.DataSource = dtLeadsAssign;
                gvLeadAssign.DataBind();

            }
            else
            {
                divMessage.InnerHtml = dsResult.Tables["ReturnTable"].Rows[0]["ReturnMessage"].ToString();
            }
        }

        private void SentAnnouncementEmailToDealer(DataTable dtSaveAssign)
        {
            Dictionary<string, int> dealerlist = ViewState["dealerList"] as Dictionary<string, int>;
            for (int i = 0; i < dtSaveAssign.Rows.Count; i++)
            {
                if (dealerlist.ContainsKey(dtSaveAssign.Rows[i]["AssignDealer"].ToString()))
                {
                    dealerlist[dtSaveAssign.Rows[i]["AssignDealer"].ToString()] += 1;
                }
            }

            string EmailContent = string.Empty;
            EmailManager emailSender = new EmailManager();
            leadsAssignmentService = new LeadsAssignmentService(base.dbConnectionStr);

            //get Assignment Announcement Email
            EmailContent = emailSender.getAssignmentAnnouncementEmail(EmailManager.LEADS_TEMPLATEID, base.dbConnectionStr);

            foreach (KeyValuePair<string, int> pair in dealerlist)
            {
                int noOfClient = pair.Value;
                string EmailContentsInfo = "";
                string dealerEmail = "";
                DataSet ds = new DataSet();

                if (noOfClient > 0)
                {
                    ds = leadsAssignmentService.RetrieveDealerEmailByDealerCode(pair.Key);
                    if (ds.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString() == "1")
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            dealerEmail = dr["Email"].ToString();

                            string dealerName = dr["AEName"].ToString();
                            string deadlineDate = String.Format("{0:dd/MM/yyyy}", calCutOffDate.DateTextFromValue.Trim());
                            //EmailContentsInfo = emailSender.ReplaceSpecialcharacter(EmailContent, dealerName, "");
                            //EmailContentsInfo = emailSender.ReplaceDeadlineDate(EmailContentsInfo, deadlineDate);
                            //EmailContentsInfo = emailSender.ReplaceTotalClient(EmailContentsInfo, noOfClient);
                            //EmailContentsInfo = emailSender.ReplaceSpecialcharacter(EmailContentsInfo);
                            EmailContentsInfo = emailSender.ReplaceSpecialcharacterForLead(EmailContent, dealerName, noOfClient, deadlineDate);
                        }
                    }
                    if (!String.IsNullOrEmpty(dealerEmail))
                    {
                        CommonUtilities common = new CommonUtilities();
                        if (common.isEmail(dealerEmail))
                        {
                            string EmailSubject = "Announcement on SPM Assignment - <<" + txtProjectName.Text.Trim() + ">>";
                            emailSender.SendEmail("spm@phillip.com.sg", dealerEmail, EmailSubject, EmailContentsInfo);
                            leadsAssignmentService.InsertEmailLog("spm@phillip.com.sg", dealerEmail, EmailSubject, EmailContentsInfo);
                        }
                    }

                }
            }
            for (int i = 0; i < dtSaveAssign.Rows.Count; i++)
            {
                if (dealerlist.ContainsKey(dtSaveAssign.Rows[i]["AssignDealer"].ToString()))
                {
                    dealerlist[dtSaveAssign.Rows[i]["AssignDealer"].ToString()] = 0;
                }
            }
        }

        private Dictionary<String, Int32> ProcessAssignNumber(DataTable dataTable)
        {
            Dictionary<String, Int32> x;
            x = new Dictionary<string, int>();
            x.Clear();
            if (dataTable.Rows.Count > 0)
            {
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    x.Add(dataTable.Rows[i]["AECode"].ToString(), 0);
                }
            }
            return x;
        }
    }
}
