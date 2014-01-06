using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using SPMWebApp.BasePage;
using SPMWebApp.WebPages.ContactManagement;
using SPMWebApp.WebPages.AssignmentManagement;
using SPMWebApp.Services;
using System.Text.RegularExpressions;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using System.Runtime.InteropServices.ComTypes;
using System.Text;


namespace SPMWebApp.Utilities
{
    public class CommonUtilities
    {
        private CommonService comService;

        public CommonUtilities()
        { }

        public string GetSettingValue(string key)
        {
            string settingValue = string.Empty;
            settingValue = System.Configuration.ConfigurationManager.AppSettings.Get(key);
            return settingValue;

        }

        public static void BindDataToDropDrownList(DropDownList ddlBind, DataTable dtData, string displayCol, string valueCol, string defaultText)
        {
            ddlBind.Items.Add(new ListItem(defaultText, ""));
            for (int i = 0; i < dtData.Rows.Count; i++)
            {
                ddlBind.Items.Add(new ListItem(dtData.Rows[i][displayCol].ToString(), dtData.Rows[i][valueCol].ToString()));
            }
        }

        public static void BindDataToDropDrownList(DropDownList ddlBind, string[,] dataArray, int displayIndex, int valueIndex, string defaultText)
        {   
            //Get zero-base total rows
            int rows = dataArray.GetUpperBound(0);

            if(!String.IsNullOrEmpty(defaultText))
                ddlBind.Items.Add(new ListItem(defaultText, ""));

            for (int i = 0; i <= rows; i++)
            {
                ddlBind.Items.Add(new ListItem(dataArray[i, displayIndex], dataArray[i, valueIndex]));
            }
        }

        public static void BindDataToDropDrownListByFilter(DropDownList ddlBind, string filterValue, DataTable dtData, 
                string displayCol, string valueCol, string defaultText)
        {
            if(!String.IsNullOrEmpty(defaultText))
            {
                ddlBind.Items.Add(new ListItem(defaultText, ""));
            }

            for (int i = 0; i < dtData.Rows.Count; i++)
            {
                if (!dtData.Rows[i][valueCol].ToString().Equals(filterValue))
                {
                    ddlBind.Items.Add(new ListItem(dtData.Rows[i][displayCol].ToString(), dtData.Rows[i][valueCol].ToString()));                   
                }                
            }
        }

        public static DataTable CreateReturnTable(string returnCode, string returnMessage)
        {
            DataTable dtReturn = new DataTable("ReturnTable");
            dtReturn.Columns.Add("ReturnCode", String.Empty.GetType());
            dtReturn.Columns.Add("ReturnMessage", String.Empty.GetType());

            DataRow dr = dtReturn.NewRow();
            dr["ReturnCode"] = returnCode;
            dr["ReturnMessage"] = returnMessage;
            dtReturn.Rows.Add(dr);

            return dtReturn;
        }

        public static string GetClientRank(string clientRank)
        {
            if (clientRank == "1")
            {
                clientRank = "Excellent Relationship";
            }
            else if (clientRank == "2")
            {
                clientRank = "Good Relationship";
            }
            else if (clientRank == "3")
            {
                clientRank = "Average Relationship";
            }
            else if (clientRank == "4")
            {
                clientRank = "Fair Relationship";
            }
            else if (clientRank == "5")
            {
                clientRank = "Poor Relationship";
            }
            else
            {
                clientRank = "No Rank";
            }

            return clientRank;
        }        

        //GridView Sorting
        //private string GetSortDirection(string column)
        //{

        //    // By default, set the sort direction to ascending.
        //    string sortDirection = "ASC";

        //    // Retrieve the last column that was sorted.
        //    string sortExpression = ViewState["SortExpression"] as string;

        //    if (sortExpression != null)
        //    {
        //        // Check if the same column is being sorted.
        //        // Otherwise, the default value can be returned.
        //        if (sortExpression == column)
        //        {
        //            string lastDirection = ViewState["SortDirection"] as string;
        //            if ((lastDirection != null) && (lastDirection == "ASC"))
        //            {
        //                sortDirection = "DESC";
        //            }
        //        }
        //    }

        //    // Save new values in ViewState.
        //    ViewState["SortDirection"] = sortDirection;
        //    ViewState["SortExpression"] = column;

        //    return sortDirection;
        //}

        public static string GetSortDirection(string sortColumn, string previousColumn, string sortDirection)
        {
            string newSortDirection = "ASC";
            if (previousColumn != null)
            {
                if (previousColumn.Equals(sortColumn))
                {
                    if ((sortDirection != null) && (sortDirection == "ASC"))
                    {
                        newSortDirection = "DESC";
                    }
                }
            }
            return newSortDirection;
        }

        public static DataTable SortDataTable(DataTable dtData, string sortString)
        {
            DataTable sortedDataTable = dtData.Clone();
            DataRow[] sortedRows = null;

            sortedRows = dtData.Select("", sortString);
            foreach (DataRow dr in sortedRows)
            {
                sortedDataTable.ImportRow(dr);
            }

            return sortedDataTable;
        }

        public static string[] GetExcelReport(DataTable dtData, BasePage.BasePage basePage, List<string> columnNames)
        {
            string[] excelReportReturn = new string[] {"-1", ""};            

            try
            {
                if ((dtData != null) && (dtData.Rows.Count > 0))
                {
                    StringWriter stringWriter = new StringWriter();
                    HtmlTextWriter htmlWriter = new HtmlTextWriter(stringWriter);
                    GridView gvExcel = new GridView();

                    if (dtData.Columns.Contains("Extra"))
                    {
                        dtData.Columns.Remove("Extra");
                    }
                    if (dtData.Columns.Contains("Score"))
                    {
                        dtData.Columns.Remove("Score");
                    }
                    if (dtData.Columns.Contains("RetradeE"))
                    {
                        dtData.Columns.Remove("RetradeE");
                    }

                    gvExcel.DataSource = dtData;

                    if (basePage != null)
                    {                        
                        if (basePage is ContactHistory)
                        {
                            gvExcel.RowDataBound += new GridViewRowEventHandler(((ContactHistory)basePage).gvContactHistory_RowDataBound);                            
                        }
                        else if (basePage is CallReport)
                        {
                            gvExcel.RowDataBound += new GridViewRowEventHandler(((CallReport)basePage).gvCallReport_RowDataBound);
                            gvExcel.ShowFooter = true;
                            gvExcel.FooterStyle.Font.Bold = true;
                        }
                        else if (basePage is ContactAnalysis)
                        {
                            gvExcel.RowDataBound += new GridViewRowEventHandler(((ContactAnalysis)basePage).gvClientContact_RowDataBound);
                            gvExcel.ShowFooter = true;
                            gvExcel.FooterStyle.Font.Bold = true;
                        }
                        else if (basePage is AssignmentHistory)
                        {
                            gvExcel.RowDataBound += new GridViewRowEventHandler(((AssignmentHistory)basePage).gvAssignments_RowDataBound);
                            gvExcel.ShowFooter = true;
                            gvExcel.FooterStyle.Font.Bold = true;
                        }
                    }

                    gvExcel.DataBind();

                    //gvExcel.Columns[5].Visible = false;

                    //Add Column Name for GridView
                    if (columnNames != null)
                    {                        
                        for (int i = 0; i < columnNames.Count; i++)
                        {
                            gvExcel.HeaderRow.Cells[i].Text = columnNames[i];
                        }
                    }

                    gvExcel.RenderControl(htmlWriter);
                    gvExcel.Dispose();

                    excelReportReturn[0] = "1";
                    excelReportReturn[1] = stringWriter.ToString();
                }
                else
                {
                    excelReportReturn[0] = "-1";
                    excelReportReturn[1] = "No records to generate Excel File!";
                }
            }
            catch (Exception e)
            {
                excelReportReturn[0] = "-1";
                excelReportReturn[1] = "Error in generating Excel File! Please try again.";
            }

            return excelReportReturn;
        }

        public static bool CheckDateFormat(string dateStr, string format)
        {
            bool result = true;

            try
            {
                DateTime.ParseExact(dateStr, format, null);
            }
            catch (Exception e)
            {
                result = false;
            }

            return result;
        }

        public static string ValidateDateRange(string fromDateStr, string toDateStr, string fromDateName, string toDateName)
        {
            string validateResult = "", dateFormat = "dd/MM/yyyy";           
            bool fromDateFlag = String.IsNullOrEmpty(fromDateStr), 
                    toDateFlag = String.IsNullOrEmpty(toDateStr);
            DateTime? fromDate = null;
            DateTime? toDate = null;


            if (fromDateFlag)
            {
                validateResult = fromDateName + " cannot be blank!";
            }
            else if ((!fromDateFlag) && ((!CheckDateFormat(fromDateStr, dateFormat))))
            {
                validateResult = fromDateName + " format should be " + dateFormat;
            }
            else if (toDateFlag)
            {
                validateResult = toDateName + " cannot be blank!";
            }
            else if ((!toDateFlag) && (!CheckDateFormat(toDateStr, dateFormat)))
            {                
                validateResult = toDateName + " format should be " + dateFormat;                
            }
            else if((!fromDateFlag) && (!toDateFlag))
            {
                fromDate = DateTime.ParseExact(fromDateStr, dateFormat, null);
                toDate = DateTime.ParseExact(toDateStr, dateFormat, null);

                if (toDate.Value.CompareTo(DateTime.Now) > 0)
                {
                    validateResult = toDateName + " should not greater than Today date!";
                }
                else if (fromDate.Value.CompareTo(toDate.Value) > 0)
                {
                    validateResult = fromDateName + " should not greater than " + toDateName + "!";
                }
            }

            return validateResult;
        }
        
        public void ConvertDocToHtml(string SourceFile,string TargetFile)
        {
            //Creating the instance of Word Application
            Microsoft.Office.Interop.Word.Application newApp = new Microsoft.Office.Interop.Word.Application();

            // specifying the Source & Target file names
            object Source = SourceFile;
            object Target = TargetFile;

            // Use for the parameter whose type are not known or  
            // say Missing
            object Unknown = Type.Missing;

            // Source document open here
            // Additional Parameters are not known so that are  
            // set as a missing type
            // newApp.Documents.Open(
            newApp.Documents.Open(ref Source, ref Unknown,
                 ref Unknown, ref Unknown, ref Unknown,
                 ref Unknown, ref Unknown, ref Unknown,
                 ref Unknown, ref Unknown, ref Unknown,
                 ref Unknown, ref Unknown, ref Unknown, ref Unknown, ref Unknown);

            // Specifying the format in which you want the output file 
            object format = Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatHTML;

            //Changing the format of the document
            newApp.ActiveDocument.SaveAs(ref Target, ref format,
                    ref Unknown, ref Unknown, ref Unknown,
                    ref Unknown, ref Unknown, ref Unknown,
                    ref Unknown, ref Unknown, ref Unknown,
                    ref Unknown, ref Unknown, ref Unknown,
                    ref Unknown, ref Unknown);

            // for closing the application
            newApp.Quit(ref Unknown, ref Unknown, ref Unknown);

        }                
        

        /* For SPM III
         * Checking AccessRight
         * Add by   Yin Mon Win
         * Date     21 Sep 2011
         * */

        public string[] getAccessRight(string pageLink, string userId,string dbCon)
        {
            comService = new CommonService(dbCon);

            DataSet ds = new DataSet();
            ds = comService.getAccessRight(pageLink, userId);
            string CR = null, VR = null, MR = null, DR = null;
            
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["CreateRight"].ToString() == "Y")
                    CR = "Y";                
                else
                    CR = "N";                

                if (ds.Tables[0].Rows[0]["ViewRight"].ToString() == "Y")
                    VR = "Y";                
                else                
                    VR = "N";

                if (ds.Tables[0].Rows[0]["ModifyRight"].ToString() == "Y")
                    MR = "Y";                
                else                
                    MR = "N";

                if (ds.Tables[0].Rows[0]["DeleteRight"].ToString() == "Y")
                    DR = "Y";
                else
                    DR = "N";  

                
            }
            //else
            //{
            //    divMessage.InnerHtml = "Access Right not found!";
            //}

            return new string[] { CR,VR,MR,DR }; 

        }

        public bool isEmail(string inputEmail)
        {
           string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                  @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                  @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            Regex re = new Regex(strRegex);
            if (re.IsMatch(inputEmail))
                return (true);
            else
                return (false);
        }

        public bool IsNumber(string number)
        {
            Regex regex=new Regex(@"^[-+]?[0-9]*\.?[0-9]+$");
            return regex.IsMatch(number);
        }

        public static DataTable GetPivotTable(DataTable sourceTable)
        {
            String groupColumnName = "MonthYear";
            String totalColName = "Total";
            List<String> headerRepeatColumnNameList = new List<String> { "Total Comm", "Market Value" };
            List<String> repeatColumnHeader = new List<String> { "CA", "CFD", "CU", "KC", "M", "PFN", "S2", "UT", "UTW" };
            DataTable tempDt = new DataTable();

            //Prepare Columns
            DataColumn groupCol = new DataColumn(groupColumnName);
            tempDt.Columns.Add(groupCol);
            DataColumn groupCol2 = new DataColumn("RepeatGroupColumn");
            tempDt.Columns.Add(groupCol2);
            foreach (String strColName in repeatColumnHeader)
            {
                DataColumn itemCol = new DataColumn(strColName);
                tempDt.Columns.Add(itemCol);
            }
            DataColumn totalCol = new DataColumn(totalColName);
            tempDt.Columns.Add(totalCol);

            //Add Rows
            var groupByRows = from temp in sourceTable.AsEnumerable()
                              group temp by temp[0] into g
                              select new { Words = g };
            int groupByCount = groupByRows.Count();
            int rowCount = groupByCount * headerRepeatColumnNameList.Count;
            for (int i = 0; i < rowCount; i++)
            {
                tempDt.Rows.Add(tempDt.NewRow());
            }

           //Fill In DataItem into Table
            for (int r = 0; r < tempDt.Rows.Count; r++)
            {
                for (int col = 0; col < tempDt.Columns.Count; col++)
                {
                    if (col == 0)
                    {
                        if (sourceTable.Rows.Count == 1)
                        {
                            tempDt.Rows[r][col] = sourceTable.Rows[0][col].ToString();
                        }
                        else
                        {
                            tempDt.Rows[r][col] = sourceTable.Rows[r == 0 ? 0 : (r / 2)][col].ToString();
                        }
                    }
                    else if (col == 1)
                    {
                        if ((r % 2) == 0)
                        {
                            tempDt.Rows[r][col] = headerRepeatColumnNameList[0].ToString();
                        }
                        else
                        {
                            tempDt.Rows[r][col] = headerRepeatColumnNameList[1].ToString();
                        }
                    }
                    else
                    {
                        var result = from temp in sourceTable.AsEnumerable()
                                     where temp.Field<String>("MonthYear") == tempDt.Rows[r][0].ToString()
                                           && temp.Field<String>("AcctSvcType") == tempDt.Columns[col].ColumnName.ToString()
                                     select temp;

                        String resultValue = String.Format("{0:00.00}", 0.0d);
                        if (result != null && result.Count() > 0)
                        {
                            if ((r % 2) == 0)
                            {
                                resultValue = String.Format("{0:00.00}", Convert.ToDouble(result.First<DataRow>()["TotalComm"].ToString()));
                            }
                            else
                            {
                                resultValue = String.Format("{0:00.00}", Convert.ToDouble(result.First<DataRow>()["MarketValue"].ToString()));
                            }
                        }

                        switch (tempDt.Columns[col].ColumnName.ToString())
                        {
                            case "CA":
                                tempDt.Rows[r][col] = resultValue;
                                break;
                            case "CFD":
                                tempDt.Rows[r][col] = resultValue;
                                break;
                            case "CU":
                                tempDt.Rows[r][col] = resultValue;
                                break;
                            case "KC":
                                tempDt.Rows[r][col] = resultValue;
                                break;
                            case "M":
                                tempDt.Rows[r][col] = resultValue;
                                break;
                            case "PFN":
                                tempDt.Rows[r][col] = resultValue;
                                break;
                            case "S2":
                                tempDt.Rows[r][col] = resultValue;
                                break;
                            case "UT":
                                tempDt.Rows[r][col] = resultValue;
                                break;
                            case "UTW":
                                tempDt.Rows[r][col] = resultValue;
                                break;
                        }
                    }                    
                }
            }

            return tempDt;
        }

        public bool VerifyAccountType(string aeCode)
        {
            bool isTSeries = false;
            if (!String.IsNullOrEmpty(aeCode))
            {
                if (aeCode[0].ToString().ToLower().Equals("t"))
                {
                    isTSeries = true;
                }
                else
                {
                    if (aeCode.Length > 3)
                    {
                        isTSeries = aeCode.Substring(0, 3).ToLower().Equals("sfr");
                    }
                }
            }
            return isTSeries;
        }        

        public static MemoryStream SerializeDataTable(DataTable dataTable)
        {
            using (MemoryStream memStream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(memStream, dataTable);
                return memStream;                
            }
        }

        public static DataTable DeserializeDataTable(MemoryStream stream)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            if (stream != null)
            {
                stream.Position = 0;
                return formatter.Deserialize(stream) as DataTable;
            }
            else
            {
                return null;
            }
        }
    }
}
