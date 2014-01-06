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
using System.IO;


namespace SPMWebApp.WebPages.AssignmentManagement
{
    public partial class ClientAssignLite : BasePage.BasePage
    {
        private ClientAssignmentService clientAssignmentService;
        private int cellCount = 1;

        private const string ASSIGNMENT_NEW = "NEW";
        private const string ASSIGNMENT_UPDATE = "UPDATE";
        private const string ASSIGNMENT_MISS = "MISS";
        private const string ASSIGNMENT_EXIST = "EXIST";
        private const string ASSIGNMENT_CORE = "CORE";
        private const string ASSIGNMENT_2N = "2N";
        private const string ASSIGNMENT_CONTACTED = "CONTACTED";

        /// <summary>
        /// Fetch User Access Right
        /// Call to ValidateUserAccessRight method with FunctionCode
        /// </summary>        
        private void LoadUserAccessRight(string pageURL)
        {
            List<AccessRight> accessRightList;
            if (AccessRightUtilities.ValidateUserAccessRight((DataTable)Session["UserAccessRights"], pageURL, out accessRightList))
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
                btnUpload.Enabled = (bool)ViewState["CreateAccessRight"];
                btnSaveAssignment.Enabled = (bool)ViewState["CreateAccessRight"];
                btnSearch.Enabled = (bool)ViewState["ViewAccessRight"];
            }
            catch (Exception e) { }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            base.gvList = gvClientAssign;
            base.divMessage = divMessage;
            base.pgcPagingControl = pgcClientAssign;

            if (!IsPostBack)
            {
                ViewState["AccountTypes"] = String.Empty;
                InitiazlizePromotionFileTable();
                InitiazlizeAccountFields();

                if (Session["UserId"] == null)
                {
                }

                divPaging.Visible = false;
                InitializeRowPerPageSetting();
                ViewState["RowPerPage"] = 10;

                calAcctFromDate.DateTextFromValue = DateTime.Now.AddMonths(-3).ToString("dd/MM/yyy");
                calAcctToDate.DateTextFromValue = DateTime.Now.AddDays(-1.0).ToString("dd/MM/yyy");

                if (Request["type"] != null)
                {
                    LoadUserAccessRight("ClientAssignCross");
                    ViewState["AssignmentType"] = "cross";
                    divTitle.InnerHtml = divTitle.InnerHtml + " - Cross Team";
                }
                else
                {
                    LoadUserAccessRight("ClientAssignLite");

                    ViewState["AssignmentType"] = "team";
                }
                checkAccessRight();

                PrepareForClientAssignment();
                btnSaveAssignment.Visible = false;
                btnAssignmentDelete.Visible = false;
            }
            divMessage.InnerHtml = "";
        }

        //For Promotion Template

        private void InitiazlizePromotionFileTable()
        {
            DataTable dtEmailFile = new DataTable();
            dtEmailFile.Columns.Add("Files", String.Empty.GetType());
            dtEmailFile.Columns.Add("FileType", String.Empty.GetType());
            dtEmailFile.Columns.Add("FileExtension", String.Empty.GetType());
            dtEmailFile.Columns.Add("FileSize", String.Empty.GetType());

            ViewState["dtEmailFile"] = dtEmailFile as DataTable;
        }

        private void DeletePromotionTemplate()
        {
            try
            {
                string[] filelist = Directory.GetFiles(Server.MapPath("~/PromotionTemplate\\"));
                if (filelist.Length > 0)
                {
                    for (int i = 0; i < filelist.Length; i++)
                    {
                        File.Delete(filelist[i].ToString());
                    }
                }
            }
            catch
            {
            }
            finally
            {
            }
        }

        private bool DeletePromotionTemplate(string FileName)
        {
            try
            {
                string FilePath = Server.MapPath("~/PromotionTemplate\\") + FileName;
                File.Delete(FilePath);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        //End Promotion Template

        protected override DataTable GetDataTable()
        {
            /* This method is called from BasePage whenever Page No (or) Row Per Page is changed. 
             * So need to called SaveClientAssignForPageChange method to save current page changes.
             */
            if (SaveClientAssignForPageChange())
                return ViewState["dtClientAssign"] as DataTable;
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
            pgcClientAssign.StartRowPerPage = 10;
            pgcClientAssign.RowPerPageIncrement = 10;
            pgcClientAssign.EndRowPerPage = 100;
        }

        //For Paging Control
        protected override void DisplayPaging()
        {
            if (divPaging.Visible)
            {
                int rowPerPage = (int)ViewState["RowPerPage"];

                pgcClientAssign.PageCount = gvClientAssign.PageCount;
                pgcClientAssign.CurrentRowPerPage = rowPerPage.ToString();
                pgcClientAssign.DisplayPaging();
            }
        }

        protected void ddlTeamCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataSet ds = null;
            int titleIndex = 0;

            if (ddlTeamCode.SelectedIndex > 0)
            {
                string assignmentType = ViewState["AssignmentType"] as string;

                if (assignmentType == "cross")
                {
                    clientAssignmentService = new ClientAssignmentService(base.dbConnectionStr);
                    ds = clientAssignmentService.RetrieveCrossEnabledDealer(ddlTeamCode.SelectedValue);
                }
                else
                {
                    /// <Win Moe Soe>
                    //ds = base.RetrieveDealerCodeAndNameByTeamNLoginID(ddlTeamCode.SelectedValue.Trim(), Session["UserId"].ToString());
                    ds = base.RetrieveDealerCodeAndNameByTeam(ddlTeamCode.SelectedValue.Trim());
                }


                if (ds.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString() == "1")
                {
                    ViewState["dtAssignDealer"] = ds.Tables[0];
                    dlstDealer.DataSource = ds.Tables[0];
                    dlstDealer.DataBind();

                    Dictionary<string, int> dealerList = ProcessAssignNumber(ds.Tables[0]);
                    ViewState["dealerList"] = dealerList;

                    //Calculate to display Title in Dealers
                    titleIndex = (ds.Tables[0].Rows.Count / dlstDealer.RepeatColumns);
                    if ((ds.Tables[0].Rows.Count % dlstDealer.RepeatColumns) > 0)
                    {
                        titleIndex++;
                    }

                    //Display Header for Dealer DataList
                    for (int i = 0; i < titleIndex; i++)
                    {
                        ((Label)dlstDealer.Items[i].FindControl("dlstlblDealerTitle")).Visible = true;
                        ((Label)dlstDealer.Items[i].FindControl("dlstlblDealerNameTitle")).Visible = true;
                        ((Label)dlstDealer.Items[i].FindControl("dlstlblAssignCount")).Visible = true;
                        ((Label)dlstDealer.Items[i].FindControl("dlstlblSelect")).Visible = true;
                    }
                }


                gvClientAssign.DataSource = null;
                gvClientAssign.DataBind();

                rptAutoAssign.DataSource = null;
                rptAutoAssign.DataBind();

                btnSaveAssignment.Visible = false;
                btnAssignmentDelete.Visible = false;
                pnlProjectInfo.Visible = false;

                divPaging.Visible = false;
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

        private void PrepareForClientAssignment()
        {
            try
            {
                DataSet ds = null;
                string assignmentType = ViewState["AssignmentType"] as string;

                if (assignmentType == "cross")
                {
                    clientAssignmentService = new ClientAssignmentService(base.dbConnectionStr);
                    ds = clientAssignmentService.RetrieveCrossEnabledTeam();
                }
                else
                {
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
                        ddlTeamCode.Items.Add(new ListItem("---Select Team---", ""));
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            ddlTeamCode.Items.Add(new ListItem(dr["TeamName"].ToString(), dr["TeamCode"].ToString()));
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

        protected void gvClientAssign_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                ((CheckBox)e.Row.FindControl("gvchkSelectAll")).Attributes.Add("onclick", "javascript:SelectAssign('" +
                        ((CheckBox)e.Row.FindControl("gvchkSelectAll")).ClientID + "')");
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataTable dtClientAssign = ViewState["dtClientAssign"] as DataTable;

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
                    if (!String.IsNullOrEmpty(contactDtStr))   //Get Contact Date 
                    {
                        contactDt = Convert.ToDateTime(contactDtStr);
                    }
                }

                if (DataBinder.Eval(e.Row.DataItem, "LastCutOffDate") != null)
                {
                    if (!String.IsNullOrEmpty(DataBinder.Eval(e.Row.DataItem, "LastCutOffDate").ToString()))
                    {
                        cutOffDt = Convert.ToDateTime(DataBinder.Eval(e.Row.DataItem, "LastCutOffDate"));
                        cutOffDtStr = cutOffDt.ToString("dd/MM/yyyy HH:mm:ss");
                        assignFlag = true;
                    }
                    else
                    {
                        cutOffDtStr = "";
                    }
                }

                if (assignFlag)
                {
                    if (DataBinder.Eval(e.Row.DataItem, "AssignDate") != null)
                    {
                        if (!String.IsNullOrEmpty(DataBinder.Eval(e.Row.DataItem, "AssignDate").ToString()))
                        {
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
                        backColor = "#C00000";  // Miss Call
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

                            backColor = "#C00000";     // Miss Call
                            cutOffDtStr = calCutOffDate.DateTextFromValue;
                        }
                        else
                        {
                            backColor = "#FFFF99";     //Valid Assignment without contact yet
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
                    int twoNClient = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "TwoNClient"));
                    if (twoNClient > 0)
                    {
                        //backColor = "#FFCCFF";
                        backColor = "#FFFFFF";
                    }
                    else if (DataBinder.Eval(e.Row.DataItem, "CoreAECode") != null)
                    {
                        if (!String.IsNullOrEmpty(DataBinder.Eval(e.Row.DataItem, "CoreAECode").ToString()))
                        {
                            backColor = "#FFCC99";
                        }
                    }

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

                //Set the Assignment Status (NEW or UPDATE)
                gvhdfAssignStatus.Value = assignmentStatus;

                System.Drawing.Color c = System.Drawing.ColorTranslator.FromHtml(backColor);
                e.Row.BackColor = c;


                //Hide delete button for not currently assigned assignment
                if (!deleteFlag)
                {
                    //Delete button visible off                    
                    foreach (Control control in e.Row.Cells[20].Controls)
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

                DropDownList ddlAssignTo = (DropDownList)e.Row.Cells[22].FindControl("gvcboDealer");
                System.Web.UI.WebControls.Button btnDelete = (System.Web.UI.WebControls.Button)e.Row.Cells[17].FindControl("gvBtnDelete");

                if (ddlAssignTo != null)
                {
                    if (!(bool)ViewState["CreateAccessRight"] && !(bool)ViewState["ModifyAccessRight"])
                    {
                        ((DropDownList)e.Row.Cells[22].FindControl("gvcboDealer")).Enabled = false;

                    }

                    else if ((bool)ViewState["CreateAccessRight"] && !(bool)ViewState["ModifyAccessRight"])
                    {
                        if (!assignFlag) //new
                        {
                            ((DropDownList)e.Row.Cells[22].FindControl("gvcboDealer")).Enabled = true;
                        }
                        else
                        {
                            ((DropDownList)e.Row.Cells[22].FindControl("gvcboDealer")).Enabled = false;
                        }
                    }

                    else if (!(bool)ViewState["CreateAccessRight"] && (bool)ViewState["ModifyAccessRight"])
                    {
                        if (!assignFlag)//new
                        {
                            ((DropDownList)e.Row.Cells[22].FindControl("gvcboDealer")).Enabled = false;
                        }
                        else
                        {
                            ((DropDownList)e.Row.Cells[22].FindControl("gvcboDealer")).Enabled = true;
                        }
                    }

                    else if ((bool)ViewState["CreateAccessRight"] && (bool)ViewState["ModifyAccessRight"])
                    {
                        ((DropDownList)e.Row.Cells[22].FindControl("gvcboDealer")).Enabled = true;
                    }
                }

                if (btnDelete != null)
                {
                    ((System.Web.UI.WebControls.Button)e.Row.Cells[24].FindControl("gvBtnDelete")).Enabled = (bool)ViewState["DeleteAccessRight"];

                }

            }
        }

        protected void gvClientAssign_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Hashtable ht = DeleteRecord(e.RowIndex);
            DataTable dtReturnData = null;
            dtReturnData = (DataTable)ht["ReturnData"];

            gvClientAssign.DataSource = dtReturnData;
            gvClientAssign.DataBind();
            divMessage.InnerHtml = ht["ReturnMessage"].ToString();

        }

        protected override Hashtable DeleteRecord(int deleteIndex)
        {
            //LastAssignDealer, AcctNo, AssignDate
            clientAssignmentService = new ClientAssignmentService(base.dbConnectionStr);
            Hashtable ht = new Hashtable();
            DataTable dtClientAssign = (DataTable)ViewState["dtClientAssign"];

            GridViewRow gvrDelete = gvClientAssign.Rows[deleteIndex];
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

            if (!String.IsNullOrEmpty(dtClientAssign.Rows[dataRowIndex]["AssignDate"].ToString()))
            {
                assignDate = Convert.ToDateTime(dtClientAssign.Rows[dataRowIndex]["AssignDate"]).ToString("yyyy-MM-dd HH:mm:ss");
            }
            if (!String.IsNullOrEmpty(dtClientAssign.Rows[dataRowIndex]["LastCutOffDate"].ToString()))
            {
                cutOffDate = Convert.ToDateTime(dtClientAssign.Rows[dataRowIndex]["LastCutOffDate"]).ToString("yyyy-MM-dd HH:mm:ss");
            }
            if (!String.IsNullOrEmpty(dtClientAssign.Rows[dataRowIndex]["ModifiedDate"].ToString()))
            {
                modifiedDate = Convert.ToDateTime(dtClientAssign.Rows[dataRowIndex]["ModifiedDate"]).ToString("yyyy-MM-dd HH:mm:ss");

            }

            wsReturn = clientAssignmentService.DeleteClientAssignment(dtClientAssign.Rows[dataRowIndex]["LastAssignDealer"].ToString(),
                            dtClientAssign.Rows[dataRowIndex]["AcctNo"].ToString(), assignDate, cutOffDate,
                            dtClientAssign.Rows[dataRowIndex]["ModifiedUser"].ToString(), modifiedDate, base.userLoginId);


            if (wsReturn[0].Equals("1"))
            {
                // Delete by using select from DataTable and remove method.               

                dtClientAssign.Rows.RemoveAt(dataRowIndex);
                ViewState["dtClientAssign"] = dtClientAssign;
                returnMessage = "The Assignment record is deleted successfully";
            }

            if (dtClientAssign.Rows.Count < 0)
            {
                btnSaveAssignment.Visible = false;
                btnAssignmentDelete.Visible = false;
            }

            ht.Add("ReturnCode", wsReturn[0]);
            ht.Add("ReturnData", dtClientAssign);
            ht.Add("ReturnMessage", wsReturn[1]);
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

            gvClientAssign.PageIndex = 0;
            gvClientAssign.DataSource = dtReturnData;
            gvClientAssign.DataBind();
            DisplayPaging();
        }

        protected override Hashtable RetrieveRecords()
        {
            btnSearch.Enabled = false;
            divMessage.InnerHtml = "Search the Clients record, Please wait ...";

            Hashtable ht = new Hashtable();
            string returnMessage = "", validateResult = "";
            int returnCode = 1, pageCount = 0;

            clientAssignmentService = new ClientAssignmentService(base.dbConnectionStr);
            DataSet ds = null;

            ht.Add("ReturnData", null);

            returnMessage = ValidateCutOffDate(calCutOffDate.DateTextFromValue.Trim());
            //validateResult = ValidateAccountDates(calAcctFromDate.DateTextFromValue, calAcctToDate.DateTextFromValue);

            validateResult = CommonUtilities.ValidateDateRange(calAcctFromDate.DateTextFromValue.Trim(), calAcctToDate.DateTextFromValue.Trim(),
                                    "Account Opening From Date", "Account Opening To Date");

            if (String.IsNullOrEmpty(ddlTeamCode.SelectedValue))
            {
                returnCode = -1;
                returnMessage = "Please select team first before searching clients!";
            }
            else if (returnMessage != "OK")
            {
                returnCode = -1;
            }
            else if (!String.IsNullOrEmpty(validateResult))
            {
                returnCode = -1;
                returnMessage = validateResult;
            }
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
                    rptAutoAssign.Visible = true;
                }
                else
                {
                    rptAutoAssign.Visible = false;
                }
                if (returnCode == 1)
                {
                    //Validation For Contact History 
                    if ((chkConHistory.Checked == true) && (txtConHistory.Text == ""))
                    {
                        divMessage.InnerHtml = "You have not specified a value";
                        returnMessage = "You have not specified a value";
                        returnCode = -1;
                    }
                    if ((chkConHistory.Checked == true) && (txtConHistory.Text != ""))
                    {
                        bool test = isNumeric(txtConHistory.Text);
                        if (!isNumeric(txtConHistory.Text))
                        {
                            divMessage.InnerHtml = "Please Enter Numeric value";
                            returnMessage = "Please Enter Numeric value";
                            returnCode = -1;
                        }
                    }

                    //Validation For Trust Balance
                    if ((chkTrustBal.Checked == true) && (txtTrustBal.Text == ""))
                    {
                        divMessage.InnerHtml = "You have not specified a value";
                        returnMessage = "You have not specified a value";
                        returnCode = -1;
                    }
                    if ((chkTrustBal.Checked == true) && (txtTrustBal.Text != ""))
                    {
                        if (!isNumeric(txtTrustBal.Text))
                        {
                            divMessage.InnerHtml = "Please Enter Numeric value";
                            returnMessage = "Please Enter Numeric value";
                            returnCode = -1;
                        }
                    }

                    //Validation for MMFBalance
                    if ((chkMMFBal.Checked == true) && (txtMMFBal.Text == ""))
                    {
                        divMessage.InnerHtml = "You have not specified a value";
                        returnMessage = "You have not specified a value";
                        returnCode = -1;
                    }
                    if ((chkMMFBal.Checked == true) && (txtMMFBal.Text != ""))
                    {
                        if (!isNumeric(txtMMFBal.Text))
                        {
                            divMessage.InnerHtml = "Please Enter Numeric value";
                            returnMessage = "Please Enter Numeric value";
                            returnCode = -1;
                        }
                    }

                    //Validation for No trade 
                    if ((chkTPeriod.Checked == true) && (txtTPeriod.Text == ""))
                    {
                        divMessage.InnerHtml = "You have not specified a value";
                        returnMessage = "You have not specified a value";
                        returnCode = -1;
                    }
                    if ((chkTPeriod.Checked == true) && (txtTPeriod.Text != ""))
                    {
                        if (!isNumeric(txtTPeriod.Text))
                        {
                            divMessage.InnerHtml = "Please Enter Numeric value";
                            returnMessage = "Please Enter Numeric value";
                            returnCode = -1;
                        }
                    }

                    //Validation For stock market value
                    if ((chkSMarketVal.Checked == true) && (txtSMarketVal.Text == ""))
                    {
                        divMessage.InnerHtml = "You have not specified a value";
                        returnMessage = "You have not specified a value";
                        returnCode = -1;
                    }
                    if ((chkSMarketVal.Checked == true) && (txtSMarketVal.Text != ""))
                    {
                        if (!isNumeric(txtSMarketVal.Text))
                        {
                            divMessage.InnerHtml = "Please Enter Numeric value";
                            returnMessage = "Please Enter Numeric value";
                            returnCode = -1;
                        }
                    }
                }

                if (returnCode == 1)
                {
                    hfTeamCode.Value = "";
                    ds = clientAssignmentService.RetrieveClientsForAssignment(ddlTeamCode.SelectedValue.Trim(), false, chkEmail.Checked,
                        chkMobile.Checked, DateTime.ParseExact(calAcctFromDate.DateTextFromValue.Trim(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd"),
                        DateTime.ParseExact(calAcctToDate.DateTextFromValue.Trim(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd"),
                        chkConHistory.Checked, txtConHistory.Text.Trim(), chkTrustBal.Checked, txtTrustBal.Text.Trim(), chkMMFBal.Checked, txtMMFBal.Text.Trim(),
                        chkTPeriod.Checked, txtTPeriod.Text.Trim(), chkSMarketVal.Checked, txtSMarketVal.Text.Trim(), ViewState["AccountTypes"].ToString());

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
                        ViewState["dtClientAssign"] = ds.Tables[0];
                        ht["ReturnData"] = ds.Tables[0];
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            pnlProjectInfo.Visible = true;
                        }
                        divPaging.Visible = true;
                        hfTeamCode.Value = ddlTeamCode.SelectedValue.Trim();
                    }
                }
            }

            if (returnCode != 1)
            {
                divPaging.Visible = false;
                btnSaveAssignment.Visible = false;
                btnAssignmentDelete.Visible = false;
                pnlProjectInfo.Visible = false;
                if (!chkAutoAssign.Checked)
                {
                    rptAutoAssign.DataSource = null;
                    rptAutoAssign.DataBind();
                }
            }

            ht.Add("ReturnCode", returnCode);
            ht.Add("ReturnMessage", returnMessage);

            btnSearch.Enabled = true;

            return ht;

        }

        private int CreateAutoAssignDealer()
        {
            DataTable dtAutoAssignDealer = new DataTable();
            dtAutoAssignDealer.Columns.Add("AECode", String.Empty.GetType());
            dtAutoAssignDealer.Columns.Add("AssignedCount", String.Empty.GetType());

            CheckBox dlstchkAutoSelect = null;
            Label dlstlblDealerCode = null;
            DataRow drAutoAssignDealer = null;
            int result = 0;

            foreach (DataListItem dlItem in dlstDealer.Items)
            {
                dlstchkAutoSelect = (CheckBox)dlItem.FindControl("dlstchkAutoSelect");
                if (dlstchkAutoSelect.Checked)
                {
                    dlstlblDealerCode = (Label)dlItem.FindControl("dlstlblDealerCode");

                    drAutoAssignDealer = dtAutoAssignDealer.NewRow();
                    drAutoAssignDealer["AECode"] = dlstlblDealerCode.Text;
                    dtAutoAssignDealer.Rows.Add(drAutoAssignDealer);
                }
            }

            result = dtAutoAssignDealer.Rows.Count;
            ViewState["dtAutoAssignDealer"] = dtAutoAssignDealer;

            return result;
        }

        private void ProcessAutoAssign(DataTable dtClientAssign)
        {
            //DataTable dtClientAssign = (DataTable)ViewState["dtClientAssign"];

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

            foreach (DataRow drClientAssign in dtClientAssign.Rows)
            {
                //CheckForValidAssignment
                if ((!String.IsNullOrEmpty(drClientAssign["AssignDate"].ToString())) && (!String.IsNullOrEmpty(drClientAssign["CutOffDate"].ToString()))
                        )
                {
                    if (String.IsNullOrEmpty(drClientAssign["LastCallDate"].ToString()))
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
            //save changes on current page before saving records to database.

            if (SaveClientAssignForPageChange())
            {
                if (validateProjectName())
                {

                    DataTable dtClientAssign = (DataTable)ViewState["dtClientAssign"];

                    //ViewState["dtTemp"] = dtClientAssign;

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
                    dtSaveAssign.Columns.Add("AcctNo", String.Empty.GetType());
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
                    dtSaveAssign.Columns.Add("Email", String.Empty.GetType());
                    dtSaveAssign.Columns.Add("LEMAIL", String.Empty.GetType());

                    for (int i = 0; i < dtClientAssign.Rows.Count; i++)
                    {
                        if ((!String.IsNullOrEmpty(dtClientAssign.Rows[i]["AssignDealer"].ToString())) &&
                                (!String.IsNullOrEmpty(dtClientAssign.Rows[i]["CutOffDate"].ToString())))
                        {
                            //initialized as New Assignment
                            assignmentStatus = ASSIGNMENT_NEW;
                            insertFlag = true;

                            //Check for Existing Assignment or not
                            assignmentNullFlag = String.IsNullOrEmpty(dtClientAssign.Rows[i]["AssignDate"].ToString());
                            if (!assignmentNullFlag)
                            {
                                assignmentStatus = ASSIGNMENT_UPDATE;
                                assignDt = Convert.ToDateTime(dtClientAssign.Rows[i]["AssignDate"]);

                                if (String.IsNullOrEmpty(dtClientAssign.Rows[i]["LastCallDate"].ToString()))
                                {
                                    //If contact date is null, assume current date as contact date to check with cut-off date
                                    contactDt = DateTime.Now;
                                }
                                else
                                {
                                    //contactDt = DateTime.ParseExact(dtClientAssign.Rows[i]["LastCallDate"].ToString(), "yyyy-MM-dd HH:mm:ss", null);
                                    contactDt = Convert.ToDateTime(dtClientAssign.Rows[i]["LastCallDate"].ToString());
                                }

                                if (!String.IsNullOrEmpty(dtClientAssign.Rows[i]["LastCutOffDate"].ToString()))
                                {
                                    cutOffDt = Convert.ToDateTime(dtClientAssign.Rows[i]["LastCutOffDate"].ToString());
                                }

                                int compareResult = contactDt.CompareTo(cutOffDt);
                                int assignCompareResult = contactDt.CompareTo(assignDt.Value);

                                if (compareResult > 0)
                                {
                                    //miss called, update assignment        (Inital Requirement)
                                    //assignmentStatus = ASSIGNMENT_UPDATE;

                                    //Miss Called should allow for New Assignment       (Changes Requirement) 
                                    assignmentStatus = ASSIGNMENT_NEW;
                                }
                                else if (String.IsNullOrEmpty(dtClientAssign.Rows[i]["LastCallDate"].ToString()))
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
                                if ((dtClientAssign.Rows[i]["AssignDealer"].ToString().
                                    Equals(dtClientAssign.Rows[i]["LastAssignDealer"].ToString())) &&
                                    (dtClientAssign.Rows[i]["CutOffDate"].ToString().
                                    Equals(dtClientAssign.Rows[i]["LastCutOffDate"].ToString())))
                                {
                                    insertFlag = false;
                                }
                                else
                                {
                                    //Checking for CutOff Date
                                    //cutOffDt = DateTime.ParseExact(dtClientAssign.Rows[i]["CutOffDate"].ToString(), "yyyy-MM-dd HH:mm:ss", null);
                                    cutOffDt = Convert.ToDateTime(dtClientAssign.Rows[i]["CutOffDate"].ToString());

                                    //assignDt = DateTime.ParseExact(dtClientAssign.Rows[i]["AssignDate"].ToString(), "dd/MM/yyyy", null);
                                    assignDt = Convert.ToDateTime(dtClientAssign.Rows[i]["AssignDate"]);

                                    if (cutOffDt.Value.CompareTo(assignDt) < 1)
                                    {
                                        insertFlag = false;
                                    }
                                }
                            }
                            else
                            {
                                //NEW Assignment
                                //cutOffDt = DateTime.ParseExact(dtClientAssign.Rows[i]["CutOffDate"].ToString(), "yyyy-MM-dd HH:mm:ss", null);
                                cutOffDt = Convert.ToDateTime(dtClientAssign.Rows[i]["CutOffDate"].ToString());

                                if (cutOffDt.Value.CompareTo(DateTime.Now) > 0)
                                {
                                    //dtClientAssign.Rows[i]["AssignDate"] = DateTime.Now.ToString("dd/MM/yyyy");
                                    dtClientAssign.Rows[i]["AssignDate"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                }
                                else
                                {
                                    insertFlag = false;
                                }
                            }

                            if (insertFlag)
                            {
                                DataRow drNewRow = dtSaveAssign.NewRow();
                                drNewRow["AssignDealer"] = dtClientAssign.Rows[i]["AssignDealer"].ToString();
                                drNewRow["AcctNo"] = dtClientAssign.Rows[i]["AcctNo"].ToString();

                                //Correct Date Format, but need to parse again in DataAccess layer
                                drNewRow["AssignDate"] = Convert.ToDateTime(dtClientAssign.Rows[i]["AssignDate"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");

                                //drNewRow["CutOffDate"] = dtClientAssign.Rows[i]["CutOffDate"].ToString();
                                drNewRow["CutOffDate"] = Convert.ToDateTime(dtClientAssign.Rows[i]["CutOffDate"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");

                                drNewRow["AssignmentStatus"] = assignmentStatus;
                                drNewRow["LastAssignDealer"] = dtClientAssign.Rows[i]["LastAssignDealer"].ToString();
                                drNewRow["ModifiedUser"] = base.userLoginId;

                                //For AuditLog
                                if (assignmentStatus.Equals(ASSIGNMENT_UPDATE))
                                {
                                    drNewRow["OldModifiedUser"] = dtClientAssign.Rows[i]["ModifiedUser"].ToString();
                                    if (!String.IsNullOrEmpty(dtClientAssign.Rows[i]["ModifiedDate"].ToString()))
                                    {
                                        drNewRow["OldModifiedDate"] = Convert.ToDateTime(dtClientAssign.Rows[i]["ModifiedDate"]).ToString("yyyy-MM-dd HH:mm:ss");
                                    }
                                    if (!String.IsNullOrEmpty(dtClientAssign.Rows[i]["LastCutOffDate"].ToString()))
                                    {
                                        drNewRow["OldCutOffDate"] = Convert.ToDateTime(dtClientAssign.Rows[i]["LastCutOffDate"]).ToString("yyyy-MM-dd HH:mm:ss");
                                    }
                                }

                                drNewRow["ProjectID"] = "";
                                drNewRow["Email"] = dtClientAssign.Rows[i]["Email"].ToString();
                                drNewRow["LEMAIL"] = dtClientAssign.Rows[i]["LEMAIL"].ToString();
                                dtSaveAssign.Rows.Add(drNewRow);

                                //Store row index to update LastAssignDealer with CurrentAssignDealer and LastCutOffDate with CutOffDate
                                savedIndexList.Add(i);
                                //Update for AuditLog
                                if (assignmentStatus.Equals(ASSIGNMENT_NEW))
                                {
                                    dtClientAssign.Rows[i]["ModifiedUser"] = base.userLoginId;
                                    dtClientAssign.Rows[i]["ModifiedDate"] = DateTime.Now;
                                }
                            }
                        }
                        // **** End of dtAssignment
                        //End of Assignments for loop   

                        /* Step 1 - count the assign user from dtClientAssign 
                           Step 2 - if no of user greater than zero , save the project info 
                           Step 3 - Save all assign information with project info
                         */
                    }

                    /// <Added By OC>
                    foreach (DataRow dr in dtSaveAssign.Rows)
                    {
                        DataRow[] drArray = dtSaveAssign.Select("AcctNo = '" + dr["AcctNo"].ToString() + "'");

                        if (drArray.Length > 1)
                        {
                            divMessage.InnerHtml = "Account Number should not be duplicate!";
                            return;
                        }
                    }

                    if (dtSaveAssign.Rows.Count > 0)
                    {
                        string[] ProjectInfo = null;
                        string projectName = "";
                        projectName = txtProName.Text;
                        ProjectInfo = SaveProjectInformation();
                        if (ProjectInfo != null)
                        {
                            string projectID = "";
                            string promotionFilePath = "";
                            projectID = ProjectInfo[0].ToString();
                            promotionFilePath = ProjectInfo[1].ToString();
                            if (!String.IsNullOrEmpty(projectID))
                            {
                                for (int j = 0; j < dtSaveAssign.Rows.Count; j++)
                                {
                                    dtSaveAssign.Rows[j]["ProjectID"] = projectID;
                                }
                            }

                            DataSet ds = new DataSet();
                            ds.Tables.Add(dtSaveAssign);
                            clientAssignmentService = new ClientAssignmentService(base.dbConnectionStr);
                            wsReturn = clientAssignmentService.InsertAssignment(ds);
                            if (wsReturn[0].Equals("1"))
                            {

                                pnlFileView.Visible = false;
                                txtProName.Text = "";
                                txtProObj.Text = "";


                                if (String.IsNullOrEmpty(promotionFilePath))
                                {
                                }
                                else if (String.IsNullOrEmpty(hfTeamCode.Value))
                                {
                                }
                                else
                                {
                                    string teamEmail = "";
                                    EmailManager emailSender = new EmailManager();
                                    teamEmail = emailSender.GetTeamEmail(hfTeamCode.Value, base.dbConnectionStr);
                                    if (!String.IsNullOrEmpty(teamEmail))
                                    {
                                        String Subject = String.Empty;
                                        DataTable dtEmailFile = ViewState["dtEmailFile"] as DataTable;

                                        if (dtEmailFile != null && dtEmailFile.Rows.Count != 0)
                                        {
                                            Subject = dtEmailFile.Rows[0]["Files"].ToString();
                                        }

                                        SentNotificationEmail(dtSaveAssign, promotionFilePath, projectID, teamEmail, Subject);
                                    }

                                }

                                SentAnnouncementEmailToDealer(dtSaveAssign, projectName);


                                //Update LastAssignDealer with CurrentAssignDealer and LastCutOffDate with CutOffDate                       
                                foreach (int k in savedIndexList)
                                {
                                    dtClientAssign.Rows[k]["LastAssignDealer"] = dtClientAssign.Rows[k]["AssignDealer"].ToString();
                                    dtClientAssign.Rows[k]["LastCutOffDate"] = dtClientAssign.Rows[k]["CutOffDate"].ToString();
                                }

                                ViewState["dtClientAssign"] = dtClientAssign;
                                gvClientAssign.DataSource = dtClientAssign;
                                gvClientAssign.DataBind();

                                pnlProjectInfo.Visible = true;
                                InitiazlizeAccountFields();
                                divMessage.InnerHtml = wsReturn[1];
                                ViewState["dtEmailFile"] = null;
                                InitiazlizePromotionFileTable();
                                DeletePromotionTemplate();
                                btnUploadAttachment.Enabled = false;

                            }
                            else
                            {
                                divMessage.InnerHtml = wsReturn[1];
                            }
                        }
                        else
                        {
                            if (divMessage.InnerHtml == "")
                            {
                                divMessage.InnerHtml = "Error in saving project information!";
                            }
                        }
                    }
                    else
                    {
                        divMessage.InnerHtml = "There is no assignments to save!";
                    }
                }
            } //End of checking error in SaveAssignment for Current Page    

        }

        private void SentAnnouncementEmailToDealer(DataTable dtSaveAssign, string projectName)
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
            clientAssignmentService = new ClientAssignmentService(base.dbConnectionStr);

            //get Assignment Announcement Email
            EmailContent = emailSender.getAssignmentAnnouncementEmail(EmailManager.TEMPLATEID, base.dbConnectionStr);

            foreach (KeyValuePair<string, int> pair in dealerlist)
            {
                int noOfClient = pair.Value;
                string EmailContentsInfo = "";
                string dealerEmail = "";
                DataSet ds = new DataSet();

                if (noOfClient > 0)
                {
                    ds = clientAssignmentService.RetrieveDealerEmailByDealerCode(pair.Key);
                    if (ds.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString() == "1")
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            dealerEmail = dr["Email"].ToString();

                            string dealerName = dr["AEName"].ToString();
                            string deadlineDate = String.Format("{0:dd/MM/yyyy}", calCutOffDate.DateTextFromValue.Trim());

                            /// <Updated by OC>
                            //EmailContentsInfo = emailSender.ReplaceSpecialcharacter(EmailContent, dealerName, "");
                            //EmailContentsInfo = emailSender.ReplaceDeadlineDate(EmailContentsInfo, deadlineDate);
                            //EmailContentsInfo = emailSender.ReplaceTotalClient(EmailContentsInfo, noOfClient);
                            //EmailContentsInfo = emailSender.ReplaceSpecialcharacter(EmailContentsInfo);
                            EmailContentsInfo = emailSender.ReplaceSpecialcharacterForAssignment(EmailContent, dealerName, noOfClient, deadlineDate);
                        }
                    }
                    if (!String.IsNullOrEmpty(dealerEmail))
                    {
                        CommonUtilities common = new CommonUtilities();
                        if (common.isEmail(dealerEmail.Trim()))
                        {
                            //string EmailSubject = "Test: Announcement on SPM Assignment - << " + projectName + " >>";
                            string EmailSubject = "Announcement on SPM Assignment - << " + projectName + " >>";
                            emailSender.SendEmail("spm@phillip.com.sg", dealerEmail, EmailSubject, EmailContentsInfo);
                            clientAssignmentService.InsertEmailLog("spm@phillip.com.sg", dealerEmail, EmailSubject, EmailContentsInfo);
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

        private void SentNotificationEmail(DataTable dtSaveAssign, string promotionFilePath, string projectID, string teamEmail, String Subject)
        {
            try
            {
                EmailManager emailSender = new EmailManager();
                CommonUtilities common = new CommonUtilities();
                clientAssignmentService = new ClientAssignmentService(base.dbConnectionStr);
                DataSet dsAttach = new DataSet();

                string ClientEmailContent = string.Empty;
                string ClientEmailTo = string.Empty;
                string ClientEmailFlag = string.Empty;
                ClientEmailContent = emailSender.ReadTextFile(Server.MapPath(promotionFilePath));
                DataTable dtAttach = new DataTable();
                dsAttach = clientAssignmentService.RetrieveProjectAttachment(projectID);
                if (dsAttach.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString() == "1")
                {
                    dtAttach = dsAttach.Tables[0];
                }

                for (int i = 0; i < dtSaveAssign.Rows.Count; i++)
                {
                    string EmailContentsInfo = "";
                    string EmailSubject = "";

                    // Retrive client emailflag,email, email content
                    ClientEmailFlag = dtSaveAssign.Rows[i]["Email"].ToString();
                    ClientEmailTo = dtSaveAssign.Rows[i]["LEMAIL"].ToString();

                    string pLogo = "";
                    pLogo = Server.MapPath("~/images/logo.jpg");

                    string ClientName = "";
                    //if there have email sent to client 
                    if (ClientEmailFlag == "Y" && ClientEmailTo != "")
                    {
                        //Retrieve client name and replace at promotion template accordingly
                        DataSet ds = new DataSet();
                        ds = clientAssignmentService.GetClientInfoByAcctNo(dtSaveAssign.Rows[i]["AcctNo"].ToString());
                        if (ds.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString() == "1")
                        {
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                ClientName = dr["LNAME"].ToString();
                                string AcctNo = dr["LACCT"].ToString();

                                /// <Commented by Thet Maung Chaw.  Updated in the region.>
                                //EmailContentsInfo = emailSender.ReplaceSpecialcharacter(ClientEmailContent, ClientName, AcctNo);

                                #region Added by Thet Maung Chaw to be able to replace photos and not to go into Junk Folder
                                /// <"RemoveUnNecessaryText"
                                /// "ReadTextFile"
                                /// "ReplacePhotos">

                                String readPath = Server.MapPath(promotionFilePath);
                                String writePath = Server.MapPath("~/PromotionTemplate/Temp.html");

                                emailSender.RemoveUnNecessaryText(readPath, writePath);
                                ClientEmailContent = emailSender.ReadTextFile(writePath);

                                String[] Photos = labImageList.Text.Split(',');
                                EmailContentsInfo = emailSender.ReplacePhotos(ClientEmailContent, Photos);

                                EmailContentsInfo = emailSender.ReplaceUnNecessaryText(EmailContentsInfo);

                                EmailContentsInfo = emailSender.ReplaceSpecialcharacter(EmailContentsInfo, ClientName, AcctNo);

                                #endregion
                            }
                        }

                        //send email to user from dealer email
                        //DealerEmail = emailSender.GetDealerEmailByDealerCode(dtSaveAssign.Rows[i]["AssignDealer"].ToString(),base.dbConnectionStr);

                        //sent email to user from team email

                        /// <Updated by OC>
                        /// <Remove "Test:">
                        //EmailSubject = "Test: Promotion letter to : " + ClientName + " ( " + dtSaveAssign.Rows[i]["AcctNo"].ToString() + " )";
                        //EmailSubject = promotionFilePath.Substring(0, promotionFilePath.Length - 5);
                        //EmailSubject = EmailSubject.Split("\\".ToCharArray())[2].ToString();
                        if (Subject != String.Empty)
                        {
                            EmailSubject = Subject.Substring(0, Subject.Length - 5);
                        }

                        if ((common.isEmail(teamEmail)) && common.isEmail(ClientEmailTo))
                        {

                            emailSender.SendPromotionEmail(teamEmail, ClientEmailTo, EmailSubject, EmailContentsInfo, dtAttach, pLogo);
                            clientAssignmentService.InsertEmailLog(teamEmail, ClientEmailTo, EmailSubject, EmailContentsInfo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //throw ex;
            }
            finally
            {
            }
        }

        private string[] SaveProjectInformation()
        {
            string[] result = null;
            string proName = "";
            string proObjective = "";
            string dirPath = "";
            string fileName = "";
            proName = txtProName.Text;
            proObjective = txtProObj.Text;
            if (proName == "")
            {
                lblReqField.Visible = true;
            }
            else
            {
                try
                {
                    DataTable dtEmailFile = (DataTable)ViewState["dtEmailFile"];
                    string[] wsReturnInfo = null;
                    for (int i = 0; i < dtEmailFile.Rows.Count; i++)
                    {
                        if (dtEmailFile.Rows[i]["FileType"].ToString() == "T")
                        {
                            string DocName = dtEmailFile.Rows[i]["Files"].ToString();
                            string SourcePath = Server.MapPath("~/PromotionTemplate\\" + DocName);
                            string DesPath = "";

                            if (File.Exists(SourcePath))
                            {

                                CommonUtilities common = new CommonUtilities();
                                fileName = String.Format("{0:dMyyyy_HHmmss}", DateTime.Now).ToString() + ".html"; // Path.GetExtension(dirPath);
                                dirPath = "~/TemplateFiles\\PromotionEmailTemplates\\" + fileName;
                                DesPath = Server.MapPath(dirPath);

                                if (File.Exists(SourcePath))
                                {
                                    File.Move(SourcePath, DesPath);
                                }
                                //common.ConvertDocToHtml(SourcePath, DesPath);                              
                            }
                        }
                    }

                    clientAssignmentService = new ClientAssignmentService(base.dbConnectionStr);
                    wsReturnInfo = clientAssignmentService.SaveProjectInformation(proName, proObjective, "C", dirPath, System.DateTime.Now, DateTime.ParseExact(calCutOffDate.DateTextFromValue.Trim(), "dd/MM/yyyy HH:mm:ss", null));
                    if (wsReturnInfo[0].Equals("1"))
                    {
                        result = new string[] { wsReturnInfo[1].ToString(), dirPath };
                    }

                    if (wsReturnInfo[0].Equals("1"))
                    {
                        string ProjectID = wsReturnInfo[1].ToString();
                        if (!String.IsNullOrEmpty(ProjectID))
                        {
                            string FolderName = CreatePromotionTemplateFolder(ProjectID);
                            for (int i = 0; i < dtEmailFile.Rows.Count; i++)
                            {
                                if (dtEmailFile.Rows[i]["FileType"].ToString() == "A")
                                {

                                    string DocName = dtEmailFile.Rows[i]["Files"].ToString();
                                    string FilePath = "~/TemplateFiles\\" + ProjectID + "\\" + DocName;
                                    string FileExtension = dtEmailFile.Rows[i]["FileExtension"].ToString();
                                    string FileSize = dtEmailFile.Rows[i]["FileSize"].ToString(); ;
                                    string SourcePath = Server.MapPath("~/PromotionTemplate\\" + DocName);
                                    string DesPath = Server.MapPath("~/TemplateFiles\\" + ProjectID + "\\" + DocName);

                                    if (File.Exists(SourcePath))
                                    {
                                        File.Move(SourcePath, DesPath);
                                    }

                                    clientAssignmentService = new ClientAssignmentService(base.dbConnectionStr);
                                    wsReturnInfo = clientAssignmentService.InsertProjectAttachment(FilePath, DocName, FileExtension, FileSize, ProjectID);
                                    if (wsReturnInfo[0].Equals("1"))
                                    {
                                        //result = new string[] { wsReturnInfo[1].ToString(), dirPath };
                                        // result = wsReturnInfo[1].ToString();                               
                                    }
                                    else
                                    {
                                        divMessage.InnerHtml = wsReturnInfo[1];
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        divMessage.InnerHtml = wsReturnInfo[1];
                    }
                }
                catch (Exception ex)
                {
                    divMessage.InnerHtml = ex.Message.ToString();
                }
            }
            return result;
        }

        private string CreatePromotionTemplateFolder(string ProjectID)
        {
            string dirPath = Server.MapPath("~/TemplateFiles\\" + ProjectID);
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
            return dirPath;
        }

        private bool validateProjectName()
        {
            if (String.IsNullOrEmpty(txtProName.Text))
            {
                //lblReqField.Visible = true;
                divMessage.InnerHtml = "Please enter project name!";
                return false;
            }

            clientAssignmentService = new ClientAssignmentService(base.dbConnectionStr);
            DataSet ds = clientAssignmentService.RetrieveAllProjectInfo();
            if (ds.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString() == "1")
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (txtProName.Text == dr["ProjectName"].ToString())
                    {
                        divMessage.InnerHtml = "Project Name Already Exists in System.";
                        return false;
                    }
                }
            }

            return true;
        }

        private bool SaveClientAssignForPageChange()
        {
            DataTable dtClientAssign = (DataTable)ViewState["dtClientAssign"];

            DropDownList gvcboDealer = null;
            TextBox gvtxtCutOffDate = null;
            HiddenField gvhdfRecordId = null;
            DateTime cutOffDate = DateTime.Now;
            string cutoffDateStr = "", accountNo = "", assignDealer = "";
            bool saveResult = true;
            int rowIndex = 0;

            for (int i = 0; i < gvClientAssign.Rows.Count; i++)
            {
                gvhdfRecordId = (HiddenField)gvClientAssign.Rows[i].FindControl("gvhdfRecordId");
                gvcboDealer = (DropDownList)gvClientAssign.Rows[i].FindControl("gvcboDealer");
                gvtxtCutOffDate = (TextBox)gvClientAssign.Rows[i].FindControl("gvtxtCutOffDate");

                rowIndex = int.Parse(gvhdfRecordId.Value);

                if (!CommonUtilities.CheckDateFormat(gvtxtCutOffDate.Text.Trim(), "dd/MM/yyyy HH:mm:ss"))
                {
                    accountNo = dtClientAssign.Rows[rowIndex]["AcctNo"].ToString();
                    saveResult = false;
                    break;
                }

                if (!gvcboDealer.SelectedValue.Equals(dtClientAssign.Rows[rowIndex]["AssignDealer"].ToString()))
                {
                    dtClientAssign.Rows[rowIndex]["AssignDealer"] = gvcboDealer.SelectedValue.Trim();
                }

                //Previously compare with CutOffDate
                if ((dtClientAssign.Rows[rowIndex]["CutOffDate"] != null) &&
                        (!String.IsNullOrEmpty(dtClientAssign.Rows[rowIndex]["CutOffDate"].ToString())))
                {
                    cutoffDateStr = Convert.ToDateTime(dtClientAssign.Rows[rowIndex]["CutOffDate"].ToString()).ToString("dd/MM/yyyy HH:mm:ss");
                }

                if (!gvtxtCutOffDate.Text.Trim().Equals(cutoffDateStr))
                {
                    if (String.IsNullOrEmpty(gvtxtCutOffDate.Text.Trim()))
                    {
                        //dtClientAssign.Rows[rowIndex]["CutOffDate"] = gvtxtCutOffDate.Text.Trim();
                        dtClientAssign.Rows[rowIndex]["CutOffDate"] = DBNull.Value;
                    }
                    else
                    {
                        //dtClientAssign.Rows[rowIndex]["CutOffDate"] = DateTime.ParseExact(gvtxtCutOffDate.Text, "dd/MM/yyyy HH:mm:ss", null).ToString("yyyy-MM-dd HH:mm:ss");
                        dtClientAssign.Rows[rowIndex]["CutOffDate"] = DateTime.ParseExact(gvtxtCutOffDate.Text, "dd/MM/yyyy HH:mm:ss", null);
                    }
                }
            }

            if (saveResult)
            {
                ViewState["dtClientAssign"] = dtClientAssign;
                divMessage.InnerHtml = "";
            }
            else
            {
                divMessage.InnerHtml = "Account No: " + accountNo + ", CutOff Date Format should be dd/MM/yyyy HH:mm:ss";
            }

            return saveResult;
        }

        //Prepare New, Already Contacted and Miss Call Assignments for New Assignment
        private void InitailizeForAssignment(DataTable dtClientAssignment)
        {
            int cutOffCompareResult = 0, assignCompareResult = 0;
            DateTime? cutOffDt = null;
            DateTime? assignDt = null;
            DateTime todayDt = DateTime.Now;

            //Set Current Date as Contact Date to check with CutOff date, if Contact Date is NULL.
            DateTime contactDt = DateTime.Now;


            foreach (DataRow dr in dtClientAssignment.Rows)
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
        protected void gvClientAssign_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Save current changes before sorting
            if (SaveClientAssignForPageChange())
            {
                DataTable dtClientAssign = ViewState["dtClientAssign"] as DataTable;
                DataTable sortedClientAssign = dtClientAssign.Clone();
                DataRow[] sortedRows = null;

                string sortDirection = "", sortString = "";

                sortDirection = GetSortDirection(e.SortExpression);
                sortString = e.SortExpression + " " + sortDirection;

                //Sorting with DataTable
                sortedRows = dtClientAssign.Select("", sortString);
                foreach (DataRow dr in sortedRows)
                {
                    sortedClientAssign.ImportRow(dr);
                }
                ViewState["dtClientAssign"] = sortedClientAssign;
                /////////////////////////////

                ViewState["SortingString"] = sortString;

                gvClientAssign.PageIndex = 0;
                gvClientAssign.DataSource = sortedClientAssign;
                gvClientAssign.DataBind();

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
            catch (Exception e)
            {
                returnMessage = "CutOff Date format must be dd/MM/yyyy HH:mm:ss";
            }

            return returnMessage;
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            string x = "";
            if (fileUpload.HasFile)
                try
                {
                    if (CheckFileExists())
                    {

                        string dirPath = Server.MapPath("~/PromotionTemplate\\" + fileUpload.FileName);
                        string fileExtention = Path.GetExtension(dirPath);
                        if (fileExtention == ".html")
                        {
                            if (fileUpload.PostedFile.ContentLength < 10485760)
                            {
                                fileUpload.SaveAs(dirPath);

                                DataTable dtEmailFile = ViewState["dtEmailFile"] as DataTable;

                                DataRow drEmailFile = null;
                                drEmailFile = dtEmailFile.NewRow();
                                drEmailFile["Files"] = fileUpload.FileName;// fileUpload.PostedFile.FileName;
                                drEmailFile["FileType"] = "T";
                                drEmailFile["FileExtension"] = fileExtention;
                                drEmailFile["FileSize"] = fileUpload.PostedFile.ContentLength; // GetFileSize(fileUpload.PostedFile.ContentLength);

                                dtEmailFile.Rows.Add(drEmailFile);

                                if (dtEmailFile.Rows.Count > 0)
                                {
                                    gvEmailFiles.DataSource = dtEmailFile;
                                    gvEmailFiles.DataBind();
                                    btnUploadAttachment.Enabled = true;
                                }
                                else
                                {
                                    gvEmailFiles.DataSource = null;
                                    gvEmailFiles.DataBind();
                                }

                                pnlFileView.Visible = true;
                                lblReqField.Visible = false;

                                //System.Configuration.Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
                                //HttpRuntimeSection section = config.GetSection("system.web/httpRuntime") as HttpRuntimeSection;
                                //double maxFileSize = Math.Round(section.MaxRequestLength / 1024.0, 1);
                                //FileSizeLimit.Text = string.Format("Make sure your file is under {0:0.#} MB.", maxFileSize);
                            }
                            else
                            {
                                x = "Upload Status: File size cannot be exceed 10MB!";
                            }
                        }
                        else
                        {
                            x = "Upload Status: Only .html files are accepted!";
                        }
                    }
                    else
                    {
                        x = "Upload Status: Email template already uploaded.";
                    }
                }
                catch (Exception ex)
                {
                    x = "ERROR: " + ex.Message.ToString();
                }
            else
            {
                x = "Please select a file to upload.";
            }
            divMessage.InnerHtml = x;
        }

        protected void btnUploadImage_Click(object sender, EventArgs e)
        {
            Boolean flag = false;
            String x = "";
            String[] AllowedFileExtensions = { ".jpg", ".png" };

            if (fpImage.HasFile)
            {
                try
                {
                    String dirPath = Server.MapPath("~/images\\" + fpImage.FileName);

                    if (File.Exists(dirPath))
                    {
                        //ScriptManager.RegisterStartupScript(this, GetType(), "script", "alert('The uploaded file name exists !');", true);

                        //return;
                        File.Delete(dirPath);
                    }

                    String fileExtention = Path.GetExtension(dirPath);

                    foreach (String item in AllowedFileExtensions)
                    {
                        if (item == fileExtention)
                        {
                            flag = true;
                        }
                    }

                    if (flag == false)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "script", "alert('Only .jpg and .png file formats are allowed !');", true);
                        return;
                    }

                    fpImage.SaveAs(dirPath);

                    if (labImageList.Text != String.Empty)
                    {
                        labImageList.Text += "," + fpImage.FileName;
                    }
                    else
                    {
                        labImageList.Text = fpImage.FileName;
                    }
                }
                catch (Exception ex)
                {
                    x = "ERROR: " + ex.Message.ToString();
                }
            }
            else
            {
                x = "Please select a file to upload.";
            }

            divMessage.InnerHtml = x;
        }

        private decimal GetFileSize(int FileSize)
        {
            //Object FSO;
            //FSO=Server.CreateObject("Scripting.FileSystemObject");
            //FSO.
            // f = FSO.getfile(dirPath);
            decimal f = 0;
            f = Convert.ToDecimal(FileSize);
            f = Math.Round(f / (2 ^ 20), 2);
            return f;
        }

        private bool CheckFileExists()
        {
            bool checkStatus = true;
            DataTable dtEmailFile = ViewState["dtEmailFile"] as DataTable;
            if (dtEmailFile.Rows.Count > 0)
            {
                for (int i = 0; i < dtEmailFile.Rows.Count; i++)
                {
                    if (dtEmailFile.Rows[i]["FileType"].ToString() == "T")
                    {
                        checkStatus = false;
                        break;
                    }
                }
            }
            return checkStatus;
        }

        private bool CheckFile(string filename)
        {
            bool checkStatus = true;
            DataTable dtEmailFile = ViewState["dtEmailFile"] as DataTable;
            if (dtEmailFile.Rows.Count > 0)
            {
                for (int i = 0; i < dtEmailFile.Rows.Count; i++)
                {
                    if (dtEmailFile.Rows[i]["Files"].ToString() == filename)
                    {
                        checkStatus = false;
                        break;
                    }
                }
            }
            return checkStatus;
        }

        protected void gvEmailFiles_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            DataTable dtEmailFile = (DataTable)ViewState["dtEmailFile"];
            bool checkTemplate = true;
            //System should not allow user to add attached files if user didn't upload the email template

            if (dtEmailFile.Rows[e.RowIndex]["FileType"].ToString() == "T")
            {
                for (int i = 0; i < dtEmailFile.Rows.Count; i++)
                {
                    if (dtEmailFile.Rows[i]["FileType"].ToString() == "A")
                    {
                        checkTemplate = false;
                    }
                }

            }

            if (checkTemplate)
            {
                if (DeletePromotionTemplate(dtEmailFile.Rows[e.RowIndex]["Files"].ToString()))
                {
                    dtEmailFile.Rows.RemoveAt(e.RowIndex);
                    ViewState["dtEmailFile"] = dtEmailFile;

                    gvEmailFiles.DataSource = dtEmailFile;
                    gvEmailFiles.DataBind();
                }
                else
                {
                    divMessage.InnerHtml = "Error in removing data file...";
                }
                if (dtEmailFile.Rows.Count == 0)
                {
                    btnUploadAttachment.Enabled = false;
                }
                else
                {

                    for (int i = 0; i < dtEmailFile.Rows.Count; i++)
                    {
                        if (dtEmailFile.Rows[i]["FileType"].ToString() == "T")
                        {
                            btnUploadAttachment.Enabled = true;
                        }
                        else
                        {
                            btnUploadAttachment.Enabled = false;
                        }
                    }
                }
            }
            else
            {
                divMessage.InnerHtml = "Please remove the attachment files first!";
            }
        }

        protected void btnUploadAttachment_Click(object sender, EventArgs e)
        {
            try
            {
                if (fileUpload.HasFile == false)
                {
                    divMessage.InnerHtml = "Please select a file to upload.";
                }
                else
                {
                    if (CheckFile(fileUpload.FileName))
                    {
                        string dirPath = Server.MapPath("~/PromotionTemplate\\" + fileUpload.FileName);
                        string fileExtention = Path.GetExtension(dirPath);
                        fileUpload.SaveAs(dirPath);

                        DataTable dtEmailFile = ViewState["dtEmailFile"] as DataTable;

                        DataRow drEmailFile = null;
                        drEmailFile = dtEmailFile.NewRow();
                        drEmailFile["Files"] = fileUpload.FileName;  // fileUpload.PostedFile.FileName;
                        drEmailFile["FileType"] = "A";
                        drEmailFile["FileExtension"] = fileExtention;
                        drEmailFile["FileSize"] = fileUpload.PostedFile.ContentLength; // GetFileSize(fileUpload.PostedFile.ContentLength);

                        dtEmailFile.Rows.Add(drEmailFile);
                        if (dtEmailFile.Rows.Count > 0)
                        {
                            gvEmailFiles.DataSource = dtEmailFile;
                            gvEmailFiles.DataBind();
                        }
                        else
                        {
                            gvEmailFiles.DataSource = null;
                            gvEmailFiles.DataBind();
                        }
                        pnlFileView.Visible = true;
                        lblReqField.Visible = false;
                    }
                    else
                    {
                        divMessage.InnerHtml = "Upload status:File Already Uploaded.";
                    }
                }

            }
            catch (Exception ex)
            {
                divMessage.InnerHtml = ex.Message.ToString();
            }
            finally
            {
            }

        }

        private void FilterAccountFields()
        {
            ClearAccountFields();
            int j = 0;
            for (int i = 0; i < lstAccountTypes.Items.Count - 1; i++)
            {
                if (lstAccountTypes.Items[i].Selected)
                {
                    j = j + 1;
                }
            }
            if (j > 0)
            {
                string[] accList = new string[j];
                int k = 0;
                for (int i = 0; i < lstAccountTypes.Items.Count - 1; i++)
                {
                    if (lstAccountTypes.Items[i].Selected)
                    {
                        accList[k] = lstAccountTypes.Items[i].Value;
                        k++;
                    }
                }
                DataSet ds = null;
                clientAssignmentService = new ClientAssignmentService(base.dbConnectionStr);
                ds = clientAssignmentService.RetrieveAccountTypeValues(accList);
                if (ds != null)
                {
                    if (ds.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString() == "1")
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            if (dr["AllowConHist"].ToString() == "Y")
                            {
                                chkConHistory.Enabled = true;
                            }
                            if (dr["AllowTrustBal"].ToString() == "Y")
                            {
                                chkTrustBal.Enabled = true;
                            }
                            if (dr["AllowMMFBal"].ToString() == "Y")
                            {
                                chkMMFBal.Enabled = true;
                            }
                            if (dr["AllowTPeriod"].ToString() == "Y")
                            {
                                chkTPeriod.Enabled = true;
                            }
                            if (dr["AllowSMarketVal"].ToString() == "Y")
                            {
                                chkSMarketVal.Enabled = true;
                            }
                        }
                    }
                }
            }
        }

        private void InitiazlizeAccountFields()
        {
            chkConHistory.Enabled = true;
            chkTrustBal.Enabled = true;
            chkMMFBal.Enabled = true;
            chkTPeriod.Enabled = true;
            chkSMarketVal.Enabled = true;

            txtConHistory.Text = "";
            txtTrustBal.Text = "";
            txtMMFBal.Text = "";
            txtTPeriod.Text = "";
            txtSMarketVal.Text = "";

            txtConHistory.Enabled = false;
            txtTrustBal.Enabled = false;
            txtMMFBal.Enabled = false;
            txtTPeriod.Enabled = false;
            txtSMarketVal.Enabled = false;

        }

        private void ClearAccountFields()
        {
            chkConHistory.Checked = false;
            chkTrustBal.Checked = false;
            chkMMFBal.Checked = false;
            chkTPeriod.Checked = false;
            chkSMarketVal.Checked = false;

            chkConHistory.Enabled = false;
            chkTrustBal.Enabled = false;
            chkMMFBal.Enabled = false;
            chkTPeriod.Enabled = false;
            chkSMarketVal.Enabled = false;

            txtConHistory.Text = "";
            txtTrustBal.Text = "";
            txtMMFBal.Text = "";
            txtTPeriod.Text = "";
            txtSMarketVal.Text = "";

            txtConHistory.Enabled = false;
            txtTrustBal.Enabled = false;
            txtMMFBal.Enabled = false;
            txtTPeriod.Enabled = false;
            txtSMarketVal.Enabled = false;

        }

        public bool isNumeric(string val)
        {
            Double result;
            return Double.TryParse(val, out result);
        }

        protected void chkConHistory_CheckedChanged(object sender, EventArgs e)
        {
            if (chkConHistory.Checked)
            {
                txtConHistory.Enabled = true;
            }
            else
            {
                txtConHistory.Enabled = false;
            }
        }

        protected void chkTrustBal_CheckedChanged(object sender, EventArgs e)
        {
            if (chkTrustBal.Checked)
            {
                txtTrustBal.Enabled = true;
            }
            else
            {
                txtTrustBal.Enabled = false;
            }
        }

        protected void chkMMFBal_CheckedChanged(object sender, EventArgs e)
        {
            if (chkMMFBal.Checked)
            {
                txtMMFBal.Enabled = true;
            }
            else
            {
                txtMMFBal.Enabled = false;
            }
        }

        protected void chkTPeriod_CheckedChanged(object sender, EventArgs e)
        {
            if (chkTPeriod.Checked)
            {
                txtTPeriod.Enabled = true;
            }
            else
            {
                txtTPeriod.Enabled = false;
            }
        }

        protected void chkSMarketVal_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSMarketVal.Checked)
            {
                txtSMarketVal.Enabled = true;
            }
            else
            {
                txtSMarketVal.Enabled = true;
            }
        }

        protected void lstAccountTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(lstAccountTypes.SelectedValue))
            {
                FilterAccountFields();
            }

            String AccountTypes = String.Empty;

            for (int i = 0; i < lstAccountTypes.Items.Count; i++)
            {
                if (lstAccountTypes.Items[i].Selected)
                {
                    AccountTypes += lstAccountTypes.Items[i].Value + "','";
                }
            }

            if (AccountTypes != String.Empty)
            {
                AccountTypes = AccountTypes.Substring(0, AccountTypes.Length - 3);
            }

            ViewState["AccountTypes"] = AccountTypes;
        }

        protected void btnAssignmentDelete_Click(object sender, EventArgs e)
        {
            DataSet dsResult = new DataSet();
            string filter = "", returnMessage = "";
            int result = 1;
            clientAssignmentService = new ClientAssignmentService(base.dbConnectionStr);
            DataTable dtClientAssign = (DataTable)ViewState["dtClientAssign"];

            //Create delete data table
            DataTable dtAssignDelete = new DataTable();
            dtAssignDelete.Columns.Add("DataRowIndex", String.Empty.GetType());
            dtAssignDelete.Columns.Add("dealerCode", String.Empty.GetType());
            dtAssignDelete.Columns.Add("accountNumber", String.Empty.GetType());
            dtAssignDelete.Columns.Add("assignDate", String.Empty.GetType());
            dtAssignDelete.Columns.Add("cutOffDate", String.Empty.GetType());
            dtAssignDelete.Columns.Add("modifiedUser", String.Empty.GetType());
            dtAssignDelete.Columns.Add("modifiedDate", String.Empty.GetType());
            dtAssignDelete.Columns.Add("newModifiedUser", String.Empty.GetType());

            DataRow drAssignDelete = null;

            //get dataRow to delete
            foreach (GridViewRow row in gvClientAssign.Rows)
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

                    if (!String.IsNullOrEmpty(dtClientAssign.Rows[dataRowIndex]["AssignDate"].ToString()))
                    {
                        assignDate = Convert.ToDateTime(dtClientAssign.Rows[dataRowIndex]["AssignDate"]).ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    if (!String.IsNullOrEmpty(dtClientAssign.Rows[dataRowIndex]["LastCutOffDate"].ToString()))
                    {
                        cutOffDate = Convert.ToDateTime(dtClientAssign.Rows[dataRowIndex]["LastCutOffDate"]).ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    if (!String.IsNullOrEmpty(dtClientAssign.Rows[dataRowIndex]["ModifiedDate"].ToString()))
                    {
                        modifiedDate = Convert.ToDateTime(dtClientAssign.Rows[dataRowIndex]["ModifiedDate"]).ToString("yyyy-MM-dd HH:mm:ss");

                    }

                    drAssignDelete = dtAssignDelete.NewRow();
                    drAssignDelete["DataRowIndex"] = dataRowIndex;
                    drAssignDelete["dealerCode"] = dtClientAssign.Rows[dataRowIndex]["LastAssignDealer"].ToString();
                    drAssignDelete["accountNumber"] = dtClientAssign.Rows[dataRowIndex]["AcctNo"].ToString();
                    drAssignDelete["assignDate"] = assignDate;
                    drAssignDelete["cutOffDate"] = cutOffDate;
                    drAssignDelete["modifiedUser"] = dtClientAssign.Rows[dataRowIndex]["ModifiedUser"].ToString();
                    drAssignDelete["modifiedDate"] = modifiedDate;
                    drAssignDelete["newModifiedUser"] = base.userLoginId;

                    dtAssignDelete.Rows.Add(drAssignDelete);
                }

            }

            if (dtAssignDelete.Rows.Count > 0)
            {
                dsResult = clientAssignmentService.BatchDeleteClientAssignment(dtAssignDelete);
                if (dsResult.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString() == "1")
                {
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
                                dtClientAssign.Rows.RemoveAt(index);
                                noOfIndex = noOfIndex + 1;
                            }
                            else
                            {
                                dtClientAssign.Rows.RemoveAt(index - noOfIndex);
                                noOfIndex = noOfIndex + 1;
                            }
                        }
                    }

                    ViewState["dtClientAssign"] = dtClientAssign;
                    if (dtClientAssign.Rows.Count < 0)
                    {
                        btnSaveAssignment.Visible = false;
                        btnAssignmentDelete.Visible = false;
                    }

                    gvClientAssign.DataSource = dtClientAssign;
                    gvClientAssign.DataBind();
                    divMessage.InnerHtml = dsResult.Tables["ReturnTable"].Rows[0]["ReturnMessage"].ToString();
                }
                else
                {
                    divMessage.InnerHtml = dsResult.Tables["ReturnTable"].Rows[0]["ReturnMessage"].ToString();
                }
            }
            else
            {
                divMessage.InnerHtml = "There is no assignments to delete!";
            }
        }

        /// <Added by Thet Maung Chaw>
        protected void gvbtnPreview_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow gvr = ((Control)sender).Parent.NamingContainer as GridViewRow;
                HiddenField hdfFiles = gvr.FindControl("hdfFiles") as HiddenField;

                String FileName = hdfFiles.Value;
                String Path = Server.MapPath("~/PromotionTemplate/");

                EmailManager emailSender = new EmailManager();
                String Content = File.ReadAllText(Path + FileName);

                if (labImageList.Text != String.Empty)
                {
                    String[] Photos = labImageList.Text.Split(',');
                    Content = emailSender.ReplacePhotos(Content, Photos);
                }

                StreamWriter sw = new StreamWriter(Path + "Preview.html");
                sw.Write(Content);
                sw.Close();

                ScriptManager.RegisterStartupScript(this, GetType(), "script", "window.open('" + HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + HttpRuntime.AppDomainAppVirtualPath + "/PromotionTemplate/Preview.html" + "','_blank');", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "script", "alert('" + ex.Message + "')", true);
            }
        }

        /// <Added by Thet Maung Chaw>
        protected void gvEmailFiles_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Button gvbtnPreview = e.Row.FindControl("gvbtnPreview") as Button;
                Label labFileType = e.Row.FindControl("labFileType") as Label;

                if (labFileType.Text == "A")
                {
                    gvbtnPreview.Visible = false;
                }
            }
        }

        /// <Added by Thet Maung Chaw>
        protected void lbtnClearImageList_Click(object sender, EventArgs e)
        {
            labImageList.Text = String.Empty;
        }
    }
}