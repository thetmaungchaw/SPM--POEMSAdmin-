﻿using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
//using Microsoft.Office.Interop.Excel;
using System.Collections.Generic;

using System.Reflection;

using SPMWebApp.BasePage;
using SPMWebApp.Services;
using SPMWebApp.Utilities;
using System.Data.OleDb;

namespace SPMWebApp.WebPages.LeadManagement
{
    public partial class LeadUpload : BasePage.BasePage 
    {       
        //Variables for Excel
        //private Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.ApplicationClass(); 
        int RecordCount = 0; 
        string strWorksheetName="";       

        private LeadsService leadsService;

        System.Data.DataTable dtExcelLeads;

        //To read Excel File
        OleDbConnection objXConn;
        OleDbCommand objCommand;
        //private CommonUtilities comUti;
        //private string[] accessRight= new string[4];

        protected PagingControl pgcPagingControl_LeadsHistory;

        protected void Page_Load(object sender, EventArgs e)
        {
            //comUti = new CommonUtilities();
            //accessRight = comUti.getAccessRight("LeadsData", base.userLoginId, this.dbConnectionStr);
           
            if (!IsPostBack)
            {
                LoadUserAccessRight();
                checkAccessRight();                
                TabContainer1_ActiveTabChanged(TabContainer1, null);

                PrepareForLeadsManagement(1);
                             
               // btnUpdate.Visible = false;
               // btnCancel.Visible = false;               
            }
 
            base.divMessage = divMessage;
            base.hdfModifyIndex = hdfModifyIndex;
            base.pgcPagingControl = pgcDealer;

            //ViewState["LeadHistory_RowPerPage"] = 20;
            //pgcLeadHistory.StartRowPerPage = 10;
            //pgcLeadHistory.RowPerPageIncrement = 10;
            //pgcLeadHistory.EndRowPerPage = 100;
            //this.pgcPagingControl_LeadsHistory = pgcLeadHistory;
            //pgcLeadHistory.PageCount = gvLeadsHistory.PageCount;
            //pgcLeadHistory.CurrentRowPerPage = ViewState["LeadHistory_RowPerPage"].ToString();
            //pgcLeadHistory.DisplayPaging();
            //DisplayLeadHistoryPaging();
            
            clearMessage();

            
        }

        private bool checkBlankAction()
        {
            int blankCount = 0; bool result = false; string status = "";
            System.Data.DataTable dtDuplicate = ViewState["dtDuplicate"] as System.Data.DataTable;
            foreach (DataRow dr in dtDuplicate.Rows)
            {
                if (!dr["Action"].ToString().Equals("Update") && !dr["Action"].ToString().Equals("Skip"))
                {
                    blankCount++; status = "blank";
                }               
            }

            if (blankCount == dtDuplicate.Rows.Count || status=="blank")
            {
                result = true ;
            }

            return result;
            
        }

        /// <summary>
        /// Fetch User Access Right
        /// Call to ValidateUserAccessRight method with FunctionCode
        /// </summary>        
        private void LoadUserAccessRight()
        {
            List<AccessRight> accessRightList;
            if (AccessRightUtilities.ValidateUserAccessRight((System.Data.DataTable)Session["UserAccessRights"], "LeadsData", out accessRightList))
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

        private void PrepareForLeadsManagement(int index)
        {
            DataSet ds = null;

            CommonService commonService = new CommonService(base.dbConnectionStr);

           
            //ds = commonService.RetrieveAllTeamCodeAndName();

            ds = commonService.RetrieveDealerCodeAndNameByUserID(base.userLoginId);
            

            if (ds.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString() == "1")
            {
                if (index == 0)
                {
                    //ddlTeamCodeUpload.Items.Add(new ListItem("--- Select Team ---", ""));
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                     //  ddlTeamCodeUpload.Items.Add(new ListItem(ds.Tables[0].Rows[i]["TeamName"].ToString(), ds.Tables[0].Rows[i]["TeamCode"].ToString()));
                     ddlTeamCodeUpload.Items.Add(new ListItem(ds.Tables[0].Rows[i]["TeamName"].ToString(), ds.Tables[0].Rows[i]["AECode"].ToString()));
                    }
                }
                else if(index==1)
                {
                   // ddlTeamCodeKeyIn.Items.Add(new ListItem("--- Select Team ---", ""));
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        //ddlTeamCodeKeyIn.Items.Add(new ListItem(ds.Tables[0].Rows[i]["TeamName"].ToString(), ds.Tables[0].Rows[i]["TeamCode"].ToString()));
                        ddlTeamCodeKeyIn.Items.Add(new ListItem(ds.Tables[0].Rows[i]["TeamName"].ToString(), ds.Tables[0].Rows[i]["AECode"].ToString().ToUpper()));
                    }

                    txtKeyInBy.Text = ds.Tables[0].Rows[ddlTeamCodeKeyIn.SelectedIndex]["AEName"].ToString();//ddlTeamCodeKeyIn.SelectedValue.ToString();
                    //txtAECode.Text = ds.Tables[0].Rows[ddlTeamCodeKeyIn.SelectedIndex]["AECode"].ToString();
                    //txtTeamCode.Text = ddlTeamCodeKeyIn.SelectedValue.ToString();//ds.Tables[0].Rows[ddlTeamCodeKeyIn.SelectedIndex]["AEName"].ToString()

                   // panelPopUp.Visible = true;
                }

            }

            //gvDealers.DataSource = ds.Tables[0];
            //gvDealers.DataBind();
        }

        private void InitializeRowPerPageSetting()
        {
            //Setting for RowPerPage for Custom Paging Control
            pgcDealer.StartRowPerPage = 10;
            pgcDealer.RowPerPageIncrement = 10;
            pgcDealer.EndRowPerPage = 100;
        }

        protected void checkAccessRight()
        {
            try
            {
                fileUploadLeads.Enabled = (bool) ViewState["CreateAccessRight"];
                btnUpload.Enabled = (bool) ViewState["CreateAccessRight"];
                btnAddRecord.Enabled = (bool)ViewState["CreateAccessRight"];
            }
            catch (Exception e) { }            
        }

        protected void TabContainer1_ActiveTabChanged(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();           

            leadsService = new LeadsService(this.dbConnectionStr);

            //tblDuplicate.Visible = false;
            //tblUploadCompare.Visible = false;
            //gvDuplicate.Visible = false;
            //dvExistingRecord.Visible = false;
            //dtvExisting.Visible = false; 
            

            //if (accessRight[0] == "N")
            //{
            //    fileUploadLeads.Enabled = false;
            //    btnUpload.Enabled = false;

            //    btnAddRecord.Enabled = false;

            //}
            //else
            //{
            //    fileUploadLeads.Enabled = true;
            //    btnUpload.Enabled = true;
            //    btnAddRecord.Enabled = true;
            //}

            clearMessage();
            try
            {
                if (TabContainer1.ActiveTabIndex == 0)
                {
                    if (!IsPostBack)
                    {
                        PrepareForLeadsManagement(0);
                        
                    }                    
                }

                if (TabContainer1.ActiveTabIndex == 1)
                {
                    //if (!IsPostBack)
                    //{
                    //    PrepareForLeadsManagement(1);
                    //    //dtLeadsHistory = new System.Data.DataTable();
                    //}
                    
                   
                    ds = leadsService.RetrieveLeadsByCriteria("", "", "", "", "", "", "", ddlTeamCodeKeyIn.SelectedValue.ToString(), "");
                    gvLeadsHistory.DataSource = ds.Tables[0];
                    gvLeadsHistory.DataBind();
                    gvLeadsHistory.Visible = true;

                    ViewState["dtEntryHistory"] = ds.Tables[0];
                    ViewState["dtLeadsHistory"] = ds.Tables[0];
                                     
                }

                divPaging.Visible = false;

                

                InitializeRowPerPageSetting();
                ViewState["RowPerPage"] = 20;
            }

            catch
            {
                throw;
            }
            finally
            {
               
            } 
        }

        protected void clearMessage()
        {
            divDuplicate.InnerHtml = "";
            divMessage.InnerHtml = "";
            divMessage2.InnerHtml = "";
            //divSeminarTitle.InnerHtml = "";
            div1.InnerHtml = "";
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            leadsService = new LeadsService(base.dbConnectionStr);

            if (fileUploadLeads.HasFile)
            { 
                string excelFileName = "LeadsImport_" + DateTime.Now.ToString("ddmmyyyyhhmmss") + ".xls";
                string filePath = "~/files/temp/";
                string fullFilePath = filePath + excelFileName;

                try
                {
                    // alter path for your project
                    fileUploadLeads.SaveAs(Server.MapPath(fullFilePath));

                  
                    hdfExcelPath.Value = filePath + excelFileName;
                    
                    //open_connection();

                    ImportToDataTable();


                    string LeadName = ""; string LeadNRIC = ""; string LeadMobile = "";

                    System.Data.DataTable dtDuplicateLeads = new System.Data.DataTable();

                    DataColumn dc1 = new DataColumn("Upload Name", Type.GetType("System.String"));
                    DataColumn dc2 = new DataColumn("Existing Name", Type.GetType("System.String"));
                    DataColumn dc3 = new DataColumn("Action", Type.GetType("System.String"));
                    DataColumn dc4 = new DataColumn("Reason", Type.GetType("System.String"));

                    DataColumn dc5 = new DataColumn("Uploaded Name", Type.GetType("System.String"));
                    DataColumn dc6 = new DataColumn("Upload NRIC", Type.GetType("System.String"));
                    DataColumn dc7 = new DataColumn("Upload Mobile", Type.GetType("System.String"));
                    DataColumn dc8 = new DataColumn("Upload Home", Type.GetType("System.String"));
                    DataColumn dc9 = new DataColumn("Upload Gender", Type.GetType("System.String"));
                    DataColumn dc10 = new DataColumn("Upload Event", Type.GetType("System.String"));
                    DataColumn dc11 = new DataColumn("Upload Email", Type.GetType("System.String"));
                    DataColumn dc12 = new DataColumn("LeadId", Type.GetType("System.String"));



                    dtDuplicateLeads.Columns.Add(dc1);
                    dtDuplicateLeads.Columns.Add(dc2);
                    dtDuplicateLeads.Columns.Add(dc3);
                    dtDuplicateLeads.Columns.Add(dc4);

                    dtDuplicateLeads.Columns.Add(dc5);
                    dtDuplicateLeads.Columns.Add(dc6);
                    dtDuplicateLeads.Columns.Add(dc7);
                    dtDuplicateLeads.Columns.Add(dc8);
                    dtDuplicateLeads.Columns.Add(dc9);
                    dtDuplicateLeads.Columns.Add(dc10);
                    dtDuplicateLeads.Columns.Add(dc11);
                    dtDuplicateLeads.Columns.Add(dc12);

                    if (dtExcelLeads != null && dtExcelLeads.Rows.Count > 0)
                    {
                        //Check Existing Leads
                        for (int i = 0; i < dtExcelLeads.Rows.Count; i++)
                        {
                            DataSet dsExistingLeads = new DataSet();
                            DataSet dsExistingInfo = new DataSet();

                            LeadName = dtExcelLeads.Rows[i][1].ToString().Trim();//for "Name"
                            LeadNRIC = dtExcelLeads.Rows[i][3].ToString().Trim();//for "NRIC / Passport"
                            LeadMobile = dtExcelLeads.Rows[i][4].ToString().Trim();//for Mobile No.

                            dsExistingLeads = leadsService.CheckExistingLead(LeadName,LeadMobile, LeadNRIC);
                            if (dsExistingLeads.Tables[0].Rows.Count > 0)
                            {
                                //divMessage1.InnerHtml = dsLeads.Tables[1].Rows[0]["ReturnMessage"].ToString();

                                DataRow dr = dtDuplicateLeads.NewRow();
                                //dr["Upload Name"] = dsExistingLeads + "(" + dsExistingLeads.Tables[0].Rows[0]["LeadNRIC"].ToString() + ")";


                                if (dtExcelLeads.Rows[i][3].ToString().Trim() == "")
                                {
                                    dr["Upload Name"] = dtExcelLeads.Rows[i][1].ToString() + "(-)";
                                }
                                else
                                {
                                    dr["Upload Name"] = dtExcelLeads.Rows[i][1].ToString() + "(" + dtExcelLeads.Rows[i][3].ToString() + ")";
                                }

                                if (dsExistingLeads.Tables[0].Rows[0]["LeadNRIC"].ToString().Trim() == "")
                                {
                                    dr["Existing Name"] = dsExistingLeads.Tables[0].Rows[0]["LeadName"].ToString() + "(-)";
                                }
                                else
                                {
                                    dr["Existing Name"] = dsExistingLeads.Tables[0].Rows[0]["LeadName"].ToString() + "(" + dsExistingLeads.Tables[0].Rows[0]["LeadNRIC"].ToString() + ")";
                                }
                                dr["Action"] = "";
                                dr["Reason"] = "";
                                dr["LeadId"] = dsExistingLeads.Tables[0].Rows[0]["LeadId"].ToString();

                                dr["Uploaded Name"] = dtExcelLeads.Rows[i][1].ToString();
                                dr["Upload Gender"] = dtExcelLeads.Rows[i][2].ToString();
                                dr["Upload NRIC"] = dtExcelLeads.Rows[i][3].ToString();
                                dr["Upload Mobile"] = dtExcelLeads.Rows[i][4].ToString();
                                dr["Upload Home"] = dtExcelLeads.Rows[i][5].ToString();
                                dr["Upload Email"] = dtExcelLeads.Rows[i][6].ToString();
                                dr["Upload Event"] = dtExcelLeads.Rows[i][7].ToString();

                                dtDuplicateLeads.Rows.Add(dr);

                                //to show the duplicate records

                                //dsExistingLeads = leadsService.RetrieveExistingLeadsInfo(LeadName, LeadNRIC);


                            }
                            else
                            {
                                //Insert() as new record
                                InsertforUpload(dtExcelLeads.Rows[i]);

                            }
                        }
                        ViewState["dtDuplicate"] = dtDuplicateLeads;

                        if (dtDuplicateLeads.Rows.Count > 0)
                        {

                            tblDuplicate.Visible = true;
                            fsDup.Visible = true;

                            //divDuplicate.InnerHtml = "Duplicate Records";
                            gvDuplicate.DataSource = dtDuplicateLeads;
                            gvDuplicate.DataBind();
                            DisplayPaging();

                            if (checkBlankAction())
                            {
                                btnSubmit.Enabled = false;
                            }
                            else
                            {
                                btnSubmit.Enabled = true;
                            }
                        }
                        else
                        {
                            fsDup.Visible = false;
                            tblDuplicate.Visible = false;
                            divMessage.InnerHtml = "Leads information is successfully uploaded.";
                            divPaging.Visible = false;
                        }

                    }
                    else
                    {
                        divMessage.InnerHtml = "There is no row to upload for Lead";
                    }
                    //----------------------

                    //ImportToDB();
                    //BindTraineeGrid();                    
                    
                    if (File.Exists(Server.MapPath(fullFilePath)))
                    {
                        File.Delete(Server.MapPath(fullFilePath));
                    }
                     
                }
                catch (Exception ex)
                {
                    
                    divMessage.InnerHtml = "Error: " + ex.Message.ToString();
                }

            }
            else
            {                
                divMessage.InnerHtml="Please select a file to upload.";
            }
            }

        //done
        private void ExcelConnection()
        {
            // Connect to the Excel Spreadsheet
            String xConnStr = "Provider=Microsoft.Jet.OLEDB.4.0;" +
                        "Data Source=" + Server.MapPath(hdfExcelPath.Value) + ";" +
                        "Extended Properties=Excel 8.0;";

            // create your excel connection object using the connection string
            objXConn = new OleDbConnection(xConnStr);
            objXConn.Open();
        }

        //done
        private void ImportToDataTable()
        {
            string  srNo="",leadsName="", gender="", leadsNric="", leadsEvent="", leadsMobile="", leadsHome="", leadsEmail="";

            OleDbDataReader excelReader = null;

            try
            {
                ExcelConnection();

                objCommand = new OleDbCommand("SELECT * FROM [SHEET1$]", objXConn);
                excelReader = objCommand.ExecuteReader();

                
                //DateTime createDate = DateTime.Now;
               // DateTime ordDate = DateTime.Now;

                //string[] rawDate = null;

                if ((excelReader != null) && (excelReader.HasRows))
                {

                   // CreateCmdTraineeIdParameters();
                    //CreateCmdCourseTraineeIdParameters();
                    CreateLeadsTable();
                    
                    while (excelReader.Read())
                    {
                        //srNo = excelReader[0].ToString();
                        if (excelReader[0] != null)
                        {
                            srNo = excelReader[0].ToString();
                        }


                        if (excelReader[1] != null)
                        {
                            leadsName = excelReader[1].ToString();                                     //FirstName  
                        }

                        if (excelReader[2] != null)
                        {
                            gender = excelReader[2].ToString();                                        //Gender
                        }
                        
                        if (excelReader[3] != null)
                        {
                            leadsNric = excelReader[3].ToString();                                     //NRIC
                        }

                        if (excelReader[4] != null)
                        {
                            leadsMobile = excelReader[4].ToString();                                   //Mobile
                        }

                        if (excelReader[5] != null)
                        {
                            leadsHome = excelReader[5].ToString();                                   //Home no
                        }

                        if (excelReader[6] != null)
                        {
                            leadsEmail = excelReader[6].ToString();                                   //Email
                        }

                        if (excelReader[7] != null)
                        {
                            leadsEvent = excelReader[7].ToString();                                   //Event
                        }
                                               

                        if (!IsExcelEmptyRow(srNo,leadsName, gender,  leadsNric,leadsMobile,leadsHome,leadsEmail,leadsEvent))
                        {

                            InsertLeadsRow(srNo,leadsName, gender,  leadsNric,leadsMobile,leadsHome,leadsEmail,leadsEvent);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //divMessage.InnerHtml = divMessage.InnerHtml + "<br> " + dobStr + ", " + ordStr + ", Msg : " + ex.Message;
                //divMessage.InnerHtml = divMessage.InnerHtml + "<br> ERROR in reading Excel File : " + ex.StackTrace;
            }
            finally
            {
                if (excelReader != null)
                {
                    excelReader.Close();
                }

                if (objXConn != null)
                {
                    objXConn.Close();
                }
            }
        }

        //done
        private bool IsExcelEmptyRow(string SrNo,string leadsName,  string gender, string nric, string mobilePh, string homePh,string leadsEmail, string leadsEvent)
        {
            bool result = false;


            if (String.IsNullOrEmpty(SrNo) && String.IsNullOrEmpty(leadsName) && String.IsNullOrEmpty(gender) && String.IsNullOrEmpty(nric) &&
                String.IsNullOrEmpty(mobilePh) && String.IsNullOrEmpty(homePh) && String.IsNullOrEmpty(leadsEmail) && String.IsNullOrEmpty(leadsEvent))
            {
                result = true;
            }


            return result;
        }

        //done
        private void InsertLeadsRow(string SrNo,string leadsName, string gender, string nric, string mobilePh, string homePh, string email, string leadsEvent)
        {
            DataRow drLeads = dtExcelLeads.NewRow();

            drLeads["SrNo"] = SrNo;
            drLeads["LeadsName"] = leadsName;
            drLeads["Gender"] = gender;
            drLeads["LeadNRIC"] = nric;
            drLeads["LeadMobile"] = mobilePh;
            drLeads["LeadHomeNo"] = homePh;
            drLeads["LeadEmail"] = email;
            drLeads["Event"] = leadsEvent;
            

            dtExcelLeads.Rows.Add(drLeads);
        }

        //done
        private void CreateLeadsTable()
        {
            dtExcelLeads = new DataTable();

           // dtLeads = new DataTable();
           // dtLeads.Columns.Add(new DataColumn("LeadId", typeof(string)));
            dtExcelLeads.Columns.Add(new DataColumn("SrNo", typeof(string)));
            dtExcelLeads.Columns.Add(new DataColumn("LeadsName", typeof(string)));
            dtExcelLeads.Columns.Add(new DataColumn("Gender", typeof(string)));
            dtExcelLeads.Columns.Add(new DataColumn("LeadNRIC", typeof(string)));
            dtExcelLeads.Columns.Add(new DataColumn("LeadMobile", typeof(string)));
            dtExcelLeads.Columns.Add(new DataColumn("LeadHomeNo", typeof(string)));
            //dtExcelLeads.Columns.Add(new DataColumn("Gender", typeof(string)));
            dtExcelLeads.Columns.Add(new DataColumn("LeadEmail", typeof(string)));
            dtExcelLeads.Columns.Add(new DataColumn("Event", typeof(string)));
            //dtLeads.Columns.Add(new DataColumn("AEGroup", typeof(string)));
            //dtLeads.Columns.Add(new DataColumn("AECode", typeof(string)));           
            //dtLeads.Columns.Add(new DataColumn("CreateDate", typeof(DateTime)));
            //dtLeads.Columns.Add(new DataColumn("InputType", typeof(string)));
          
        }

        #region Upload Old

        /*

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            leadsService = new LeadsService(base.dbConnectionStr);

            string strFileName = fileUploadLeads.PostedFile.FileName;
            string myFileName = System.IO.Path.GetFileName(strFileName);

            System.Data.DataTable dtExcel;// = ViewState["dtExcel"] as System.Data.DataTable; ;

            if (strFileName == "")
            {
                divMessage.InnerHtml = "Key in or Choose the file name!";
            }
            else { 

                string extension = System.IO.Path.GetExtension(myFileName);
                string fileType = extension.Substring(1).ToLower();

                if (fileType != "xls" && fileType != "xlsx")
                {
                    divMessage.InnerHtml = "Only XLS and XLSX files allowed!";
                }
                else 
                {                   
                    string filePath  = Server.MapPath("../../files/temp/") + DateTime.Now.ToString().Replace(" ","").Replace("/","").Replace(":","") + myFileName;

                    try
                    { 
                        //save the file temporarily
                        fileUploadLeads.PostedFile.SaveAs(filePath);

                        //read the file         

                        
                        Application oApp = new Application();
                        Workbook oWorkBook ;//= new Workbook();
                        Worksheet oWorkSheet;// = new Worksheet();

                        oWorkBook = oApp.Workbooks.Open(filePath.Trim(),0,true,5,"","",true,XlPlatform.xlWindows,"\t",false,false,0,true,1,0);
                        Sheets totalSheets=oWorkBook.Worksheets;

                        // Get The Active Worksheet Using Sheet Name Or Active Sheet
                        //Worksheet oWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)oWorkBook.ActiveSheet;

                        oWorkSheet =(Microsoft.Office.Interop.Excel.Worksheet)totalSheets.get_Item(1); //oWorkBook.Sheets[1]
                        Range range = oWorkSheet.UsedRange;                        
                        RecordCount = range.Rows.Count - 1;

                        strWorksheetName = oWorkSheet.Name;//Get the name of worksheet.
                        oApp.Visible = false;

                        //=================================

                        //System.IO.StreamReader objStreamReader = System.IO.File.OpenText(filePath);
                        //string fileText = objStreamReader.ReadToEnd();
                        

                        //if (ValidateFile(fileText))
                        //{

                        dtExcel = FromExcel(filePath);

                        //ViewState["dtExcel"] = dtExcel;
                        string LeadName = "";string LeadNRIC = "";

                        System.Data.DataTable dtDuplicateLeads = new System.Data.DataTable();

                        DataColumn dc1 = new DataColumn("Upload Name", Type.GetType("System.String"));
                        DataColumn dc2 = new DataColumn("Existing Name", Type.GetType("System.String"));
                        DataColumn dc3 = new DataColumn("Action", Type.GetType("System.String"));
                        DataColumn dc4 = new DataColumn("Reason", Type.GetType("System.String"));
                        
                        DataColumn dc5 = new DataColumn("Uploaded Name", Type.GetType("System.String"));
                        DataColumn dc6 = new DataColumn("Upload NRIC", Type.GetType("System.String"));
                        DataColumn dc7 = new DataColumn("Upload Mobile", Type.GetType("System.String"));
                        DataColumn dc8 = new DataColumn("Upload Home", Type.GetType("System.String"));
                        DataColumn dc9 = new DataColumn("Upload Gender", Type.GetType("System.String"));
                        DataColumn dc10 = new DataColumn("Upload Event", Type.GetType("System.String"));
                        DataColumn dc11 = new DataColumn("Upload Email", Type.GetType("System.String"));
                        DataColumn dc12 = new DataColumn("LeadId", Type.GetType("System.String"));

                        

                        dtDuplicateLeads.Columns.Add(dc1);
                        dtDuplicateLeads.Columns.Add(dc2);
                        dtDuplicateLeads.Columns.Add(dc3);
                        dtDuplicateLeads.Columns.Add(dc4);

                        dtDuplicateLeads.Columns.Add(dc5);
                        dtDuplicateLeads.Columns.Add(dc6);
                        dtDuplicateLeads.Columns.Add(dc7);
                        dtDuplicateLeads.Columns.Add(dc8);
                        dtDuplicateLeads.Columns.Add(dc9);
                        dtDuplicateLeads.Columns.Add(dc10);
                        dtDuplicateLeads.Columns.Add(dc11);
                        dtDuplicateLeads.Columns.Add(dc12);

                        if (dtExcel.Rows.Count > 0)
                        {
                            //Check Existing Leads
                            for (int i = 0; i < dtExcel.Rows.Count; i++)
                            {
                                DataSet dsExistingLeads = new DataSet();
                                DataSet dsExistingInfo = new DataSet();

                                LeadName = dtExcel.Rows[i][1].ToString().Trim();//for "Name"
                                LeadNRIC = dtExcel.Rows[i][3].ToString().Trim();//for "NRIC / Passport"

                                dsExistingLeads = leadsService.CheckExistingLead(LeadName, LeadNRIC);
                                if (dsExistingLeads.Tables[0].Rows.Count > 0)
                                {
                                    //divMessage1.InnerHtml = dsLeads.Tables[1].Rows[0]["ReturnMessage"].ToString();

                                    DataRow dr = dtDuplicateLeads.NewRow();
                                    //dr["Upload Name"] = dsExistingLeads + "(" + dsExistingLeads.Tables[0].Rows[0]["LeadNRIC"].ToString() + ")";


                                    if (dtExcel.Rows[i][3].ToString().Trim() == "")
                                    {
                                        dr["Upload Name"] = dtExcel.Rows[i][1].ToString() + "(-)";
                                    }
                                    else
                                    {
                                        dr["Upload Name"] = dtExcel.Rows[i][1].ToString() + "(" + dtExcel.Rows[i][3].ToString() + ")";
                                    }

                                    if (dsExistingLeads.Tables[0].Rows[0]["LeadNRIC"].ToString().Trim() == "")
                                    {
                                        dr["Existing Name"] = dsExistingLeads.Tables[0].Rows[0]["LeadName"].ToString() + "(-)";
                                    }
                                    else
                                    {
                                        dr["Existing Name"] = dsExistingLeads.Tables[0].Rows[0]["LeadName"].ToString() + "(" + dsExistingLeads.Tables[0].Rows[0]["LeadNRIC"].ToString() + ")";
                                    }
                                    dr["Action"] = "";
                                    dr["Reason"] = "";
                                    dr["LeadId"] = dsExistingLeads.Tables[0].Rows[0]["LeadId"].ToString();

                                    dr["Uploaded Name"] = dtExcel.Rows[i][1].ToString();
                                    dr["Upload Gender"] = dtExcel.Rows[i][2].ToString();
                                    dr["Upload NRIC"] = dtExcel.Rows[i][3].ToString();
                                    dr["Upload Mobile"] = dtExcel.Rows[i][4].ToString();
                                    dr["Upload Home"] = dtExcel.Rows[i][5].ToString();
                                    dr["Upload Email"] = dtExcel.Rows[i][6].ToString();
                                    dr["Upload Event"] = dtExcel.Rows[i][7].ToString();

                                   dtDuplicateLeads.Rows.Add(dr);

                                    //to show the duplicate records
                                   
                                   //dsExistingLeads = leadsService.RetrieveExistingLeadsInfo(LeadName, LeadNRIC);

                                  
                                }
                                else
                                { 
                                    //Insert() as new record
                                    InsertforUpload(dtExcel.Rows[i]);
                                    
                                }
                            }
                            ViewState["dtDuplicate"] = dtDuplicateLeads;

                            if (dtDuplicateLeads.Rows.Count > 0)
                            {
                               
                                tblDuplicate.Visible = true;
                                fsDup.Visible = true;
                               
                                //divDuplicate.InnerHtml = "Duplicate Records";
                                gvDuplicate.DataSource = dtDuplicateLeads;
                                gvDuplicate.DataBind();
                                DisplayPaging();

                                if (checkBlankAction())
                                {
                                    btnSubmit.Enabled = false;
                                }
                                else
                                {
                                    btnSubmit.Enabled = true;
                                }
                            }
                            else {
                                fsDup.Visible = false;
                                tblDuplicate.Visible = false;
                                divMessage.InnerHtml = "Leads information is successfully uploaded.";
                                divPaging.Visible = false;
                            }

                        }

                        System.Runtime.InteropServices.Marshal.ReleaseComObject(oWorkBook);
                        oWorkBook = null;
                        oApp.Quit();
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(oApp);
                        oApp = null;

                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }
                        //    InsertData(fileText);
                        //    divMessage.InnerHtml = "File has been uploaded.";
                        //}
                        //else
                        //{
                        //    divMessage.InnerHtml = errMsg;
                        //}                        
                    }
                    catch (Exception ex)
                    {
                        divMessage.InnerHtml = "Error : " + ex.ToString();
                        //System.Runtime.InteropServices.Marshal.ReleaseComObject(oWorkBook);
                        //oWorkBook = null;
                        //oApp.Quit();
                        //System.Runtime.InteropServices.Marshal.ReleaseComObject(oApp);
                        //oApp = null;

                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }
                    }
                
                }
            
            }
        }

        */
        
        #endregion

        protected void gvLeadsHistory_Sorting(object sender, GridViewSortEventArgs e)
        {
           System.Data.DataTable dtLeads = ViewState["dtLeadsHistory"] as System.Data.DataTable;
           System.Data.DataTable sortedDataTable = null;

            string sortDirection = "", sortString = "";

            sortDirection = CommonUtilities.GetSortDirection(e.SortExpression, ViewState["SortExpression"] as string, ViewState["SortDirection"] as string);

            ViewState["SortDirection"] = sortDirection;
            ViewState["SortExpression"] = e.SortExpression;
            sortString = e.SortExpression + " " + sortDirection;
            sortedDataTable = CommonUtilities.SortDataTable(dtLeads, sortString);

            ViewState["dtLeadsHistory"] = sortedDataTable;
            gvLeadsHistory.PageIndex = 0;
            gvLeadsHistory.DataSource = sortedDataTable;
            gvLeadsHistory.DataBind();
            //DisplayLeadHistoryPaging();
            
            dtLeads.Dispose();
        }

        private void DisplayLeadsHistoryPaging()
        {
            //if (.Visible)
            //{
            //    int rowPerPage = (int)ViewState["LeadsHistoryRowPerPage"];

            //    pgcLeadsHistory.PageCount = gvLeadsHistory.PageCount;
            //    pgcLeadsHistory.CurrentRowPerPage = rowPerPage.ToString();
            //    pgcLeadsHistory.DisplayPaging();
            //}
        }

    

        protected override void DisplayDetails(int modifyIndex)
        {
           // btnUpdateContact.Visible = true;
            btnCancel.Visible = true;
            ViewState["cmdAction"] = "modify";
            btnAddRecord.Text = "Update";

           
            //btnAddContact.Visible = false;
            this.hdfModifyIndex = new HiddenField();
            this.hdfModifyIndex.Value = modifyIndex.ToString();

            int dataItemIndex = 1;
            System.Data.DataTable dtEntryHistory = ViewState["dtEntryHistory"] as System.Data.DataTable;

            HiddenField gvhdfRecordId = (HiddenField)gvLeadsHistory.Rows[modifyIndex].FindControl("gvhdfRecordId");

            dataItemIndex = int.Parse(gvhdfRecordId.Value);
            if (dtEntryHistory != null)
            {
                DataRow drModify = dtEntryHistory.Rows[dataItemIndex];

                //hdfRecId.Value = drModify["RecId"].ToString();
                DisplayLeadsDetail(drModify["LeadId"].ToString(),drModify["LeadName"].ToString(), drModify["LeadNRIC"].ToString(),
                    drModify["LeadMobile"].ToString(), drModify["LeadHomeNo"].ToString(), drModify["LeadGender"].ToString(), drModify["LeadEmail"].ToString(),
                    drModify["Event"].ToString(), drModify["AECode"].ToString());
            }
        }

        protected void gvLeadsHistory_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {            
            Hashtable ht = DeleteRecord(e.RowIndex);
            System.Data.DataTable dtLeads = null;

            if (int.Parse(ht["ReturnCode"].ToString()) == 2)
            {
                divMessage2.InnerHtml = ht["ReturnMessage"].ToString();
            }
            else
            {
                if (int.Parse(ht["ReturnCode"].ToString()) > 0)
                {
                    dtLeads = (System.Data.DataTable)ht["ReturnData"];
                }

                gvLeadsHistory.DataSource = dtLeads;
                gvLeadsHistory.DataBind();
                ViewState["dtEntryHistory"] = dtLeads;

                divMessage2.InnerHtml = ht["ReturnMessage"].ToString();
            }
        }

        protected override Hashtable DeleteRecord(int deleteIndex)
        {
            string deleteLeadsId = gvLeadsHistory.DataKeys[deleteIndex].Value.ToString();

            HiddenField gvhdfRecordId = (HiddenField)gvLeadsHistory.Rows[deleteIndex].FindControl("gvhdfRecordId");
            leadsService = new LeadsService(dbConnectionStr);
            Hashtable ht = new Hashtable();
            System.Data.DataTable dtEntryHistory = ViewState["dtEntryHistory"] as System.Data.DataTable;
            DataSet ds = null; string dealerCode = ""; string userRole = "";
            
            ht.Add("ReturnData", null);
            ht.Add("ReturnCode", "-1");
            ht.Add("ReturnMessage", "");
            
            if (ViewState["UserRole"] != null)
            {
                userRole = ViewState["UserRole"] as string;
            }

            dealerCode = ddlTeamCodeKeyIn.SelectedValue.ToString();//ddlTeamCodeKeyIn.SelectedItem.Text.Split('-')[0].Trim();
            
            //if (userRole == "admin")
            //{
            //    dealerCode = ddlTeamCodeKeyIn.SelectedItem.Text.Split('-')[1].Trim();
            //    //userId = ddlTeamCodeKeyIn.SelectedValue;
            //}
            //else
            //{
            //   // dealerCode = hdfDealerCode.Value;
            //    dealerCode =gvLeadsHistory.Rows[deleteIndex].Cells["DealerCode"].ToString();
            //    userId = base.userLoginId;
            //}
            //dealerCode = ddlActualDealer.SelectedItem.Text.Split('-')[1].Trim();

            ds = leadsService.DeleteLeads(deleteLeadsId,dealerCode);
            if (ds.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString() == "1")
            {
                dtEntryHistory.Rows.RemoveAt(int.Parse(gvhdfRecordId.Value));

                //if (dtEntryHistory.Rows.Count < 1)
                //{
                //    divLeadsPaging.Visible = false;
                //}

                ViewState["dtEntryHistory"] = dtEntryHistory;

                ht["ReturnData"] = dtEntryHistory;
                ht["ReturnCode"] = ds.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString();
                ht["ReturnMessage"] = ds.Tables["ReturnTable"].Rows[0]["ReturnMessage"].ToString();

                // divMessage2.InnerText = ds.Tables["ReturnTable"].Rows[0]["ReturnMessage"].ToString();


            }
            else
            {
                ht["ReturnCode"] = ds.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString();
                ht["ReturnMessage"] = ds.Tables["ReturnTable"].Rows[0]["ReturnMessage"].ToString();
            }

            return ht;
        }

        protected void gvLeadsHistory_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            
            if ((e.CommandName == "AcctNo") || (e.CommandName == "ClientName"))
            {
                int index = Convert.ToInt32(e.CommandArgument);
                System.Data.DataTable dtEntryHistory = ViewState["dtEntryHistory"] as System.Data.DataTable;

                if (dtEntryHistory != null)
                {
                    DataRow drModify = dtEntryHistory.Rows[index];
                    if (e.CommandName == "AcctNo")
                    {
                        DisplayLeadsDetail(drModify["LeadId"].ToString(),drModify["LeadName"].ToString(), drModify["LeadNRIC"].ToString(),
                    drModify["LeadMobile"].ToString(), drModify["LeadHomeNo"].ToString(), drModify["LeadGender"].ToString(), drModify["LeadEmail"].ToString(),
                    drModify["Event"].ToString(), drModify["AECode"].ToString());
                    }

                    ViewState["dtEntryHistory"] = "History";
                   // lbtnContactHistory.Enabled = true;
                   // this.retrieveContactHistory(drModify["AcctNo"].ToString());
                }
            }
        }

        private void DisplayLeadsDetail(string leadsId,string leadsName, string leadsNRIC, string MobileNo,string HomeNo,
                       string gender, string Email,string Event,string teamCode)
        {
            hidLeadsId.Value = leadsId;   
            txtName.Text= leadsName;
            txtNRIC .Text = leadsNRIC ;
            txtMobileNo.Text = MobileNo ;
            txtHomeNo.Text = HomeNo ;
            if (gender.StartsWith("F"))
            {
                radFemale.Checked = true;
            }
            else
            {
                radMale.Checked = true;
            }

            txtEmail.Text = Email;
            txtEvent.Text = Event;

            ddlTeamCodeKeyIn.SelectedValue = teamCode.ToUpper();
        }
              
        protected void gvDuplicate_RowCommand(object sender,   System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            // string NameText = ""; string NRICText = "";
            string currentCommand = e.CommandName;
            if (currentCommand != "Sort")
            {
                int currentRowIndex = Convert.ToInt32(e.CommandArgument);


                // Retrieve the row that contains the button clicked 
                // by the user from the Rows collection.
                GridViewRow row = gvDuplicate.Rows[currentRowIndex];

                // Create a new ListItem object for the contact in the row.  
                System.Data.DataTable dtDuplicate = ViewState["dtDuplicate"] as System.Data.DataTable;

                if (dtDuplicate != null)
                {
                    DataRow drNew = dtDuplicate.Rows[currentRowIndex];
                    if (e.CommandName == "Details")
                    {
                        ViewState["duplicateIndex"] = currentRowIndex;

                        //NameText = drNew[1].ToString();
                        //NRICText = drNew[3].ToString();
                        ShowCompareGrid(drNew);
                    }
                }
            }
        }

        private void ShowCompareGrid(DataRow dr)
        {
            leadsService = new LeadsService(base.dbConnectionStr);
            //System.Data.DataTable dtUploadRecord = ViewState["dtExcel"] as System.Data.DataTable; 
           // System.Data.DataSet dsExistingRecord = ViewState["dsExistingInfo"] as System.Data.DataSet ;
            
            tblUploadCompare.Visible = true;
            //dvUploadRecord.DataSource = dtUploadRecord;
            //dvUploadRecord.DataBind();
            
            txtUploadName.Text = dr[4].ToString();         
            txtUploadNRIC.Text = dr[5].ToString();
            txtUploadMobileNo.Text= dr[6].ToString();
            txtUploadHome.Text=dr[7].ToString();

            if (dr[8].ToString().Trim().ToUpper().StartsWith("F"))
            {
                //txtUploadGender.Text = "Female";
                radUploadFemale .Checked = true;
            }
            else if (dr[8].ToString().Trim().ToUpper().StartsWith("M"))
            {
                //txtUploadGender.Text = "Male";
                radUploadMale.Checked = true;
            }        

            txtUploadEvent.Text= dr[9].ToString();
            txtUploadEmail.Text = dr[10].ToString();

            DataSet dsExistingLeads = leadsService.RetrieveExistingLeadsInfo(txtUploadName.Text.Trim(), txtUploadNRIC.Text.Trim());

            dvExistingRecord.DataSource = dsExistingLeads.Tables[0];
            
            dvExistingRecord.DataBind();
        }       
        
        protected void gvDuplicate_Sorting(object sender, GridViewSortEventArgs e)
        {
            System.Data.DataTable dt = ViewState["dtDuplicate"] as System.Data.DataTable;//ViewState["dtAssignments"] as DataTable;
                        
            System.Data.DataTable sortedDataTable = null;

            string sortDirection = "", sortString = "";

            sortDirection = CommonUtilities.GetSortDirection(e.SortExpression, ViewState["SortExpression"] as string, ViewState["SortDirection"] as string);

            ViewState["SortDirection"] = sortDirection;
            ViewState["SortExpression"] = e.SortExpression;

            sortString = e.SortExpression + " " + sortDirection;
            sortedDataTable = CommonUtilities.SortDataTable(dt, sortString);

            ViewState["dtDuplicate"] = sortedDataTable;
            gvDuplicate.PageIndex = 0;

            //dtAssignments.DefaultView.Sort = e.SortExpression + " " + sortDirection;
            //gvAssignments.DataSource = dtAssignments.DefaultView;

            tblDuplicate.Visible = true;
            gvDuplicate.DataSource = sortedDataTable;
            gvDuplicate.DataBind();
            DisplayPaging();

            dt.Dispose();
        }

        public System.Data.DataTable FromExcel(string fileName)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            System.Data.OleDb.OleDbConnection excelConnection = new System.Data.OleDb.OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileName + ";Extended Properties=Excel 8.0");

            try
            {
                excelConnection.Open();
                //System.Data.OleDb.OleDbCommand excelCommand = new System.Data.OleDb.OleDbCommand("SELECT * FROM [SHEET1$];", excelConnection);

                System.Data.OleDb.OleDbCommand excelCommand = new System.Data.OleDb.OleDbCommand("SELECT * FROM [" + strWorksheetName + "$];", excelConnection);

                System.Data.OleDb.OleDbDataAdapter adap = new System.Data.OleDb.OleDbDataAdapter();
                adap.SelectCommand = excelCommand;
                adap.Fill(dt);

                return dt;
            }
            catch (Exception err)
            {
                //MessageBox.Show(err.Message.ToString());
                System.Web.HttpContext.Current.Response.Write("<SCRIPT LANGUAGE='JavaScript'>alert('" + err.Message.ToString() + "')</SCRIPT>");
                return null;
            }
            finally
            {
                excelConnection.Close();
            }
        }

        /*
        private bool ValidateFile(String fileText)
        {
            #region Excel File Open
            ApplicationClass app = new ApplicationClass();
            // create the workbook object by opening  the excel file.
            Workbook workBook = app.Workbooks.Open(txtPath.Text,
            0,
            true,
            5,
            "",
            "",
            true,
             XlPlatform.xlWindows,
            "\t",
            false,
            false,
            0,
            true,
            1,
            0);
            // Get The Active Worksheet Using Sheet Name Or Active Sheet
            Worksheet workSheet = (Worksheet)workBook.ActiveSheet;
            Range range = workSheet.UsedRange;
            // int rCnt = 0; 
            RecordCount = range.Rows.Count - 1;

            Excel.Sheets sheets = workBook.Worksheets;

            Excel.Worksheet worksheet = (Excel.Worksheet)sheets.get_Item(1);//Get the reference of second worksheet

            strWorksheetName = worksheet.Name;//Get the name of worksheet.

            // progressBar1.Maximum = RecordCount;

            #endregion


            //int NumColumns = 8;

            //string[] A = fileText.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            //int i, j = 0;
            //DataSet ds = new DataSet();
            //String[] L; bool valid = true;

            ////check number of rows
            //if (A.Length < 1)
            //{
            //    errMsg = "At least one lead required!";
            //    valid = false; return valid;
            //}

            ////check number of columns
            //for (int i = 1; i < A.Length; i++)//ignore first row
            //{
            //    if (A[i] != "") //ignore empty lines
            //    {
            //        L = A[i].Split(';');

            //        if (L.GetUpperBound(0) != 7)
            //        {
            //            errLineCount++;
            //            if (errLineCount == 6)
            //            {
            //                errMsg = errMsg + "\n"; errLineCount = 0;
            //            }
            //            errMsg = errMsg + "Line " + i + ": is incorrect.";//Please choose the Correct CDP file.";
            //        }
            //    }
            //}
         
            //check number of columns
            //ds = CF.PullData("select count(*) from infoemployeefields where companyid='" & CompanyList.SelectedValue & "' and visibletohr='true' and not field in ('brokerid', 'dealerid')")
            //Dim NumColumns As Integer = ds.Tables(0).Rows(0)(0)
            //For i = 1 To UBound(A) 'ignore first row
            //    If (A(i) <> "") Then
            //        L = Split(A(i), ",")
            //        If (UBound(L) <> NumColumns - 1) Then
            //            errMsg = errMsg & "Line " & i + 1 & " : There should be exactly " & NumColumns & " columns!<br>"
            //            valid = False
            //        End If
            //    End If
            //Next
            //If (Not valid) Then
            //    ValidateFile = False
            //    Exit Function
            //End If
        
        
        }
        */
       

        protected string getNewID(string prevID)
        {
            int lenNumbers = 4;
            string prefix = "L";
            string num = "";

            //prevID = cmc.GetMaxID(0);
            if (prevID == "")
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

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearLeadsForm();
            btnAddRecord.Text = "Add New Record";
        }

        protected void btnAddRecord_Click(object sender, EventArgs e)
        {
            if (btnAddRecord.Text=="Add New Record")
            {
                leadsService = new LeadsService(base.dbConnectionStr);
                string[] wsReturn = null;
                //int enable = 1, insertedID = 1;
                //string crossGroup = "N", supervisior = "N", assign = "Y", 
                string validateResult = ""; string gender = ""; string teamCode = "";
                DataSet dsExisting = new DataSet();

                validateResult = ValidateLeadsForm();

                if (String.IsNullOrEmpty(validateResult))
                {
                    if (radFemale.Checked)
                    {
                        gender = "F";
                    }
                    else if (radMale.Checked)
                    {
                        gender = "M";
                    }

                    //Check Existing Leads
                    DataSet dsLeads = null;
                    dsLeads = leadsService.CheckExistingLead(txtName.Text.Trim(), txtMobileNo.Text.Trim(), txtNRIC.Text.Trim());
                    if (dsLeads.Tables[0].Rows.Count > 0)
                    {
                        divMessage2.InnerHtml = dsLeads.Tables[1].Rows[0]["ReturnMessage"].ToString();

                        //to show the duplicate records

                        dsExisting = leadsService.RetrieveExistingLeadsInfo(txtName.Text.Trim(), txtNRIC.Text.Trim());
                        dtvExisting.Visible = true;
                        dtvExisting.DataSource = dsExisting.Tables[0];
                        dtvExisting.DataBind();

                        //tblUploadCompare.Visible = true;
                        btnUpdate.Visible = true;
                        btnSkip.Visible = true;
                    }
                    else
                    {
                        btnUpdate.Visible = false;
                        btnSkip.Visible = false;
                        //dtvExisting.DataSource = null;
                        //dtvExisting.DataBind();

                        DataSet dsMax = new DataSet(); string num = "0";
                        dsMax = leadsService.RetrieveMaxLeadsID();

                        if (dsMax.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString() == "1")
                        {
                            if (dsMax.Tables[0].Rows.Count > 0)
                            {
                                num = dsMax.Tables[0].Rows[0]["MaxLeadID"].ToString();
                            }

                            string selectedLeadId = getNewID(num);

                            int indexOfHyphen = ddlTeamCodeKeyIn.SelectedItem.Text.IndexOf("-");

                            teamCode = ddlTeamCodeKeyIn.SelectedItem.Text.Substring(0, indexOfHyphen - 1);

                            //teamCode = txtTeamCode.Text.Trim();

                            wsReturn = leadsService.InsertLeads(selectedLeadId, txtName.Text.Trim(), txtNRIC.Text.Trim(), txtMobileNo.Text.Trim(), txtHomeNo.Text.Trim(), gender,
                                txtEmail.Text.Trim(), txtEvent.Text.Trim(), teamCode, ddlTeamCodeKeyIn.SelectedValue.ToString(),"K");


                            if (wsReturn[0] == "1")
                            {
                                dsLeads = leadsService.RetrieveLeadsByCriteria("", "", "", "", "", "", "", ddlTeamCodeKeyIn.SelectedValue.ToString(), "");

                                ViewState["dtEntryHistory"] = dsLeads.Tables[0];
                                gvLeadsHistory.PageIndex = 0;
                                gvLeadsHistory.DataSource = dsLeads.Tables[0];
                                gvLeadsHistory.DataBind();
                                this.ClearLeadsForm();

                                ViewState["dtEntryHistory"] = dsLeads.Tables[0];
                                ViewState["dtLeadsHistory"] = dsLeads.Tables[0];

                                ViewState["cmdAction"] = "new";
                                btnAddRecord.Text = "Add New Record";

                                //divPaging.Visible = true;
                                DisplayPaging();
                                divMessage2.InnerHtml = wsReturn[1];
                            }
                        }
                        else
                        {
                            divMessage2.InnerHtml = dsMax.Tables["ReturnTable"].Rows[0]["ReturnMessage"].ToString();
                        }
                    }
                }
                else
                {
                    divMessage2.InnerHtml = validateResult;
                }
            }           
            else if(btnAddRecord.Text=="Update")
            {
                btnUpdate_Click(null, null);
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            //DataSet dsMax = new DataSet(); string num = "";
            leadsService = new LeadsService(base.dbConnectionStr);
            string[] wsReturn = null; string gender = ""; string selectedLeadId = "";
            DataSet dsExisting = new DataSet();

             string validateResult = ValidateLeadsForm();

             if (String.IsNullOrEmpty(validateResult))
             {

                 if (radFemale.Checked)
                 {
                     gender = "F";
                 }
                 else if (radMale.Checked)
                 {
                     gender = "M";
                 }

                 #region Check Existing Leads
                 //Check Existing Leads
                 //DataSet dsLeads = null;
                 //dsLeads = leadsService.CheckExistingLead(txtName.Text.Trim(), txtNRIC.Text.Trim());
                 //if (dsLeads.Tables[0].Rows.Count > 0)
                 //{
                 //    divMessage2.InnerHtml = dsLeads.Tables[1].Rows[0]["ReturnMessage"].ToString();

                 //    //to show the duplicate records

                 //    dsExisting = leadsService.RetrieveExistingLeadsInfo(txtName.Text.Trim(), txtNRIC.Text.Trim());
                 //    dtvExisting.DataSource = dsExisting;
                 //    dtvExisting.DataBind();

                 //    tblUploadCompare.Visible = true;

                 //    btnUpdate.Visible = true;
                 //    btnSkip.Visible = true;
                 //}
                 #endregion
                 //else
                 //{
                     //------------------------
                     if (ViewState["cmdAction"] as string == "modify")
                     {
                         selectedLeadId = hidLeadsId.Value.ToString();
                     }
                     else
                     { 
                         System.Web.UI.WebControls.TextBox tempLeadId = (System.Web.UI.WebControls.TextBox)dtvExisting.FindControl("txtLeadId");
                         
                         if (tempLeadId != null)
                         {
                             selectedLeadId = tempLeadId.Text;
                         }
                     }
                     int indexOfHyphen = ddlTeamCodeKeyIn.SelectedItem.Text.IndexOf("-");

                     string teamCode = ddlTeamCodeKeyIn.SelectedItem.Text.Substring(0, indexOfHyphen - 1);

                     wsReturn = leadsService.UpdateLeads(selectedLeadId, txtName.Text.Trim(), txtNRIC.Text.Trim(), txtMobileNo.Text.Trim(), txtHomeNo.Text.Trim(), gender,
                         txtEmail.Text.Trim(), txtEvent.Text.Trim(), teamCode, ddlTeamCodeKeyIn.SelectedValue.ToString());

                     if (wsReturn[0] == "1")
                     {
                         //System.Data.DataTable dtLeads = ViewState["dtEntryHistory"] as System.Data.DataTable;

                         DataSet dsLeads = new DataSet();
                         //dsLeads = new DataSet();

                         dsLeads = leadsService.RetrieveAllLeads();

                         ViewState["dtEntryHistory"] = dsLeads.Tables[0];
                         gvLeadsHistory.PageIndex = 0;
                         gvLeadsHistory.DataSource = dsLeads.Tables[0];
                         gvLeadsHistory.DataBind();
                         this.ClearLeadsForm();
                         
                         ViewState["cmdAction"] = "new";
                         btnAddRecord.Text = "Add New Record";

                         //divPaging.Visible = true;
                         DisplayPaging();
                         divMessage2.InnerHtml = wsReturn[1];
                     }
                 //}
             }
             else
             {
                 divMessage2.InnerHtml = validateResult;
             }
        }

        protected void btnSkip_Click(object sender, EventArgs e)
        {
            ClearLeadsForm();
        }

        private string ValidateLeadsForm()
        {
            string validateResult = "";

            if (String.IsNullOrEmpty(txtName.Text.Trim()))
            {
                validateResult = "Leads Name cannot be blank!";
            }
            else if (String.IsNullOrEmpty(txtMobileNo.Text.Trim()))
            {
                validateResult = "Leads Mobile Number cannot be blank!";
            }            
            else if (String.IsNullOrEmpty(ddlTeamCodeKeyIn.SelectedValue))
            {
                validateResult = "Please select Team for Leads!";
            }
            else if (radFemale.Checked == false && radMale.Checked==false)
            {
                validateResult = "Please select Gender for Leads!";
            } 
                        
            return validateResult;
        }

        private void ClearLeadsForm()
        {
            txtName.Text = txtNRIC.Text = txtMobileNo.Text = txtHomeNo.Text=txtEvent.Text=txtEmail.Text= "";
            ddlTeamCodeKeyIn.SelectedIndex = 0;
            radFemale.Checked =  false;
            dtvExisting.DataSource = null;
            dtvExisting.Visible = false;

            btnUpdate.Visible = false;
            btnSkip.Visible = false;

            divMessage.InnerHtml = "";
            divMessage2.InnerHtml = "";

          
        }


        private void InsertforUpload(DataRow dr)
        {
            leadsService = new LeadsService(base.dbConnectionStr);

            DataSet dsMax = new DataSet(); string num = "";
            string[] wsReturn = null;
            dsMax = leadsService.RetrieveMaxLeadsID();
            string gender = "";

            if (dsMax.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString() == "1")
            {
                if (dsMax.Tables[0].Rows.Count > 0)
                {
                    num = dsMax.Tables[0].Rows[0]["MaxLeadID"].ToString();
                }

                string selectedLeadId = getNewID(num);

                int indexOfHyphen = ddlTeamCodeUpload.SelectedItem.Text.IndexOf("-");

                string teamCode = ddlTeamCodeUpload.SelectedItem.Text.Substring(0, indexOfHyphen - 1);

                //teamCode = txtTeamCode.Text.Trim();
                if (dr["Gender"].ToString().Trim().ToUpper().StartsWith("F"))
                {
                    gender = "F";
                }
                else if (dr["Gender"].ToString().Trim().ToUpper().StartsWith("M"))
                {
                    gender="M";
                }

                wsReturn = leadsService.InsertLeads(selectedLeadId,dr[1].ToString().Trim(), dr[3].ToString().Trim(),dr[4].ToString().Trim(),dr[5].ToString().Trim(), gender,
                    dr[6].ToString().Trim(), dr[7].ToString().Trim(), teamCode, ddlTeamCodeUpload.SelectedValue.ToString(),"U");


                if (wsReturn[0] == "1")
                {
                    System.Data.DataTable dtLeads = ViewState["dtLeads"] as System.Data.DataTable;

                    if (dtLeads == null)
                    {
                        dtLeads = new System.Data.DataTable("dtLeads");

                        dtLeads.Columns.Add("LeadId", String.Empty.GetType());
                        dtLeads.Columns.Add("LeadName", String.Empty.GetType());
                        dtLeads.Columns.Add("LeadNRIC", String.Empty.GetType());
                        dtLeads.Columns.Add("LeadMobile", String.Empty.GetType());
                        dtLeads.Columns.Add("LeadHomeNo", String.Empty.GetType());
                        dtLeads.Columns.Add("LeadGender", String.Empty.GetType());
                        dtLeads.Columns.Add("LeadEmail", String.Empty.GetType());
                        dtLeads.Columns.Add("Event", String.Empty.GetType());
                        dtLeads.Columns.Add("AEGroup", String.Empty.GetType());
                        dtLeads.Columns.Add("AECode", String.Empty.GetType());
                        //dtLeads.Columns.Add("modifiedUser", String.Empty.GetType());
                        dtLeads.Columns.Add("modifiedDate", typeof(DateTime));

                        //dtLeads.Columns.Add("OriginalAECode", String.Empty.GetType());
                        //dtLeads.Columns.Add("OriginalLeadID", String.Empty.GetType());

                        //Create Primary Key
                        dtLeads.PrimaryKey = new DataColumn[] { dtLeads.Columns["LeadId"] };
                    }

                    DataRow drNewLeads = dtLeads.NewRow();

                    drNewLeads["LeadId"] = selectedLeadId;//wsReturn[2];
                    drNewLeads["LeadName"] = dr[1].ToString().Trim();
                    drNewLeads["LeadNRIC"] = dr[3].ToString().Trim();
                    drNewLeads["LeadMobile"] = dr[4].ToString().Trim();
                    //drNewDealer["AEGroup"] = txtTeam.Text.Trim();
                    drNewLeads["LeadHomeNo"] = dr[5].ToString().Trim();
                    //drNewDealer["ATSLogin"] = txtAtsLogin.Text.Trim();
                    //drNewLeads["ATSLogin"] = "";
                    drNewLeads["LeadGender"] = gender;
                    drNewLeads["LeadEmail"] = dr[6].ToString().Trim();
                    drNewLeads["Event"] = dr[7].ToString().Trim();
                    drNewLeads["AEGroup"] = ddlTeamCodeUpload.SelectedValue; ;
                    drNewLeads["AECode"] = "";
                    // drNewLeads["modifiedUser"] = base.userLoginId;
                    drNewLeads["modifiedDate"] = DateTime.Now;

                    //drNewLeads["OriginalLeadID"] = txtLoginId.Text.Trim();
                    //drNewLeads["OriginalAECode"] = txtDealerCode.Text.Trim();

                    dtLeads.Rows.InsertAt(drNewLeads, 0);
                    //dtDealers.Rows.Add(drNewDealer);

                    ViewState["dtLeadss"] = dtLeads;
                    //gvDealers.PageIndex = 0;
                    //gvDealers.DataSource = dtDealers;
                    //gvDealers.DataBind();

                    this.ClearLeadsForm();

                    //divPaging.Visible = true;
                    DisplayPaging();
                    divMessage2.InnerHtml = wsReturn[1];
                }
            }
            else
            {
                divMessage2.InnerHtml = dsMax.Tables["ReturnTable"].Rows[0]["ReturnMessage"].ToString();
            }
        }

        private void UpdateforUpload(DataRow dr)
        {
            leadsService = new LeadsService(base.dbConnectionStr);

            string[] wsReturn = null;
            
            string gender = "";

            string selectedLeadId = dr["LeadId"].ToString();//getNewID(num);

            int indexOfHyphen = ddlTeamCodeUpload.SelectedItem.Text.IndexOf("-");

            string teamCode = ddlTeamCodeUpload.SelectedItem.Text.Substring(0, indexOfHyphen - 1);
                        
            if (dr[8].ToString().Trim().ToUpper().StartsWith("F"))
            {
                gender = "F";
            }
            else if (dr[8].ToString().Trim().ToUpper().StartsWith("M"))
            {
                gender = "M";
            }
           
            wsReturn = leadsService.UpdateLeads(selectedLeadId, dr[4].ToString().Trim(), dr[5].ToString().Trim(), dr[6].ToString().Trim(), dr[7].ToString().Trim(), gender,
                    dr[10].ToString().Trim(), dr[9].ToString().Trim(), teamCode, ddlTeamCodeUpload.SelectedValue.ToString());

            #region Comment
            /*
            if (wsReturn[0] == "1")
            {
                System.Data.DataTable dtLeads = ViewState["dtLeads"] as System.Data.DataTable;

                if (dtLeads == null)
                {
                    dtLeads = new System.Data.DataTable("dtLeads");

                    dtLeads.Columns.Add("LeadId", String.Empty.GetType());
                    dtLeads.Columns.Add("LeadName", String.Empty.GetType());
                    dtLeads.Columns.Add("LeadNRIC", String.Empty.GetType());
                    dtLeads.Columns.Add("LeadMobile", String.Empty.GetType());
                    dtLeads.Columns.Add("LeadHome", String.Empty.GetType());
                    dtLeads.Columns.Add("LeadGender", String.Empty.GetType());
                    dtLeads.Columns.Add("LeadEmail", String.Empty.GetType());
                    dtLeads.Columns.Add("Event", String.Empty.GetType());
                    dtLeads.Columns.Add("AEGroup", String.Empty.GetType());
                    dtLeads.Columns.Add("AECode", String.Empty.GetType());
                    //dtLeads.Columns.Add("modifiedUser", String.Empty.GetType());
                    dtLeads.Columns.Add("modifiedDate", typeof(DateTime));

                    //dtLeads.Columns.Add("OriginalAECode", String.Empty.GetType());
                    //dtLeads.Columns.Add("OriginalLeadID", String.Empty.GetType());

                    //Create Primary Key
                    dtLeads.PrimaryKey = new DataColumn[] { dtLeads.Columns["LeadId"] };
                }

                DataRow drNewLeads = dtLeads.NewRow();

                drNewLeads["LeadId"] = selectedLeadId;//wsReturn[2];
                drNewLeads["LeadName"] = dr[1].ToString().Trim();
                drNewLeads["LeadNRIC"] = dr[3].ToString().Trim();
                drNewLeads["LeadMobile"] = dr[4].ToString().Trim();
                //drNewDealer["AEGroup"] = txtTeam.Text.Trim();
                drNewLeads["LeadHome"] = dr[5].ToString().Trim();
                //drNewDealer["ATSLogin"] = txtAtsLogin.Text.Trim();
                //drNewLeads["ATSLogin"] = "";
                drNewLeads["LeadGender"] = gender;
                drNewLeads["LeadEmail"] = dr[6].ToString().Trim();
                drNewLeads["Event"] = dr[7].ToString().Trim();
                drNewLeads["AEGroup"] = ddlTeamCodeUpload.SelectedValue; ;
                drNewLeads["AECode"] = "";
                // drNewLeads["modifiedUser"] = base.userLoginId;
                drNewLeads["modifiedDate"] = DateTime.Now;

                //drNewLeads["OriginalLeadID"] = txtLoginId.Text.Trim();
                //drNewLeads["OriginalAECode"] = txtDealerCode.Text.Trim();

                dtLeads.Rows.InsertAt(drNewLeads, 0);
                //dtDealers.Rows.Add(drNewDealer);

                ViewState["dtLeadss"] = dtLeads;
                //gvDealers.PageIndex = 0;
                //gvDealers.DataSource = dtDealers;
                //gvDealers.DataBind();
            */
            #endregion

            this.ClearLeadsForm();

            //divPaging.Visible = true;
            DisplayPaging();
            divMessage.InnerHtml = wsReturn[1];

            gvDuplicate.DataSource = null;
            gvDuplicate.DataBind();
            fsDup.Visible = false;            
            //}            
        }

        protected void btnUpdateUpload_Click(object sender, EventArgs e)
        {
            tblUploadCompare.Visible = false;
            Int32  currentRowIndex = Convert.ToInt32(ViewState["duplicateIndex"]);
            System.Data.DataTable dtDuplicate = ViewState["dtDuplicate"] as System.Data.DataTable;

            dtDuplicate.Rows[currentRowIndex]["Action"] = "Update";
            dtDuplicate.Rows[currentRowIndex]["Reason"] = "Retain";

            gvDuplicate.DataSource = dtDuplicate;
            gvDuplicate.DataBind();

            if (checkBlankAction())
            {
                btnSubmit.Enabled = false;
            }
            else
            {
                btnSubmit.Enabled = true;
            }
        }

        protected void btnSkipUpload_Click(object sender, EventArgs e)
        {
            tblUploadCompare.Visible = false;
            System.Data.DataTable dtDuplicate = ViewState["dtDuplicate"] as System.Data.DataTable;
            int currentRowIndex = Convert.ToInt32(ViewState["duplicateIndex"]);

            dtDuplicate.Rows[currentRowIndex]["Action"] = "Skip";
            dtDuplicate.Rows[currentRowIndex]["Reason"] = "Remove";

            gvDuplicate.DataSource = dtDuplicate;
            gvDuplicate.DataBind();

            if (checkBlankAction())
            {
                btnSubmit.Enabled = false;
            }
            else
            {
                btnSubmit.Enabled = true;
            }
        }

        protected void dvExistingRecord_DataBound(object sender, EventArgs e)
        {
            //this would give you the underlying data
            //DataRowView row = dvExistingRecord.DataItem as DataRowView;
            //access any control in template field(s)
            RadioButton radExistingFemale = dvExistingRecord.FindControl("radExistingFemale") as RadioButton;
            RadioButton radExistingMale = dvExistingRecord.FindControl("radExistingMale") as RadioButton;
            HiddenField hr = dvExistingRecord.FindControl("hidGender") as HiddenField;
            // string genderText= ((HiddenField)((DetailsView)sender).FindControl("hidGender")).Value.ToString();
            if (hr != null)
            {
                if (hr.Value.ToString().StartsWith("F"))
                {
                    radExistingFemale.Checked = true;
                }
                else
                {
                    radExistingMale.Checked = true;
                }
            }
        }
        
        protected void dtvExisting_DataBound(object sender, EventArgs e)
        {
            //this would give you the underlying data
            //DataRowView row = dvExistingRecord.DataItem as DataRowView;
            //access any control in template field(s)
            RadioButton radFemale = dtvExisting.FindControl("radFemale") as RadioButton;
            RadioButton radMale = dtvExisting.FindControl("radMale") as RadioButton;
            HiddenField hr = dtvExisting.FindControl("hidGender") as HiddenField;
            // string genderText= ((HiddenField)((DetailsView)sender).FindControl("hidGender")).Value.ToString();
            if (hr != null)
            {
                if (hr.Value.ToString().StartsWith("F"))
                {
                    radFemale.Checked = true;
                }
                else
                {
                    radMale.Checked = true;
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            
            System.Data.DataTable dtDuplicate = ViewState["dtDuplicate"] as System.Data.DataTable;

            //dtDuplicate.Rows[currentRowIndex]["Action"] = "Update";
            //gvDuplicate.DataSource = dtDuplicate;

            if (countSkip(dtDuplicate) == dtDuplicate.Rows.Count) 
            {
                divMessage.InnerHtml = "There is no record to update.";
                //fsDup.Visible = false;
                return;
            }
            
                foreach (DataRow dr in dtDuplicate.Rows)
                {
                    if (dr["Action"].ToString().Equals("Update"))
                    {
                        UpdateforUpload(dr);
                    }
                }
        }

        protected int countSkip(System.Data.DataTable dt)
        {
            int skipCount = 0;
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["Action"].ToString().Equals("Skip"))
                {
                    skipCount++; 
                }               
            }

            return skipCount;
        }     

        protected void gvLeadsHistory_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                System.Web.UI.WebControls.Button btnModify = (System.Web.UI.WebControls.Button)e.Row.Cells[10].FindControl("gvbtnEdit");
                System.Web.UI.WebControls.Button btnDelete = (System.Web.UI.WebControls.Button)e.Row.Cells[10].FindControl("gvbtnDelete");
                if (btnModify != null)
                {
                    ((System.Web.UI.WebControls.Button)e.Row.Cells[10].FindControl("gvbtnEdit")).Enabled = (bool)ViewState["ModifyAccessRight"];                    
                }
                if (btnDelete != null)
                {
                    ((System.Web.UI.WebControls.Button)e.Row.Cells[10].FindControl("gvbtnDelete")).Enabled = (bool)ViewState["DeleteAccessRight"];
                }

                if (e.Row.Cells[4].Text == "F")
                {
                    e.Row.Cells[4].Text = "Female";
                }
                else
                {
                    e.Row.Cells[4].Text = "Male";
                }
            }
        }

        protected void gvLeadsHistory_PageIndexChanged(object sender, EventArgs e)
        {

        }

        protected void gvLeadsHistory_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DataTable myDataTable = (DataTable)ViewState["dtLeadsHistory"];
            gvLeadsHistory.DataSource = myDataTable;
            gvLeadsHistory.PageIndex = e.NewPageIndex;
            gvLeadsHistory.DataBind();
        }
    }
}