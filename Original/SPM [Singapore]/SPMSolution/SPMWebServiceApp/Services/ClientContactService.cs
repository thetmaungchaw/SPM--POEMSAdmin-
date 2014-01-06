﻿using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using SPMWebServiceApp.DataAccess;
using SPMWebServiceApp.Utilities;
using System.Text;

namespace SPMWebServiceApp.Services
{
    public class ClientContactService
    {
        private GenericDA genericDA;
        private ClientContactDA clientContactDA;


        public ClientContactService()
        {
            genericDA = new GenericDA();
            clientContactDA = new ClientContactDA(genericDA);
        }

        public ClientContactService(string dbConnectionStr) : this()
        {
            genericDA.SetConnectionString(dbConnectionStr);
        }

        public DataSet PrepareForContactEntry(string userRole, string userId)
        {
            DataSet ds = new DataSet();
            DataTable dtReturn = null;
            DealerDA dealerDA = new DealerDA(genericDA);
            CommonServiceDA commonServiceDA = new CommonServiceDA(genericDA);
            ClientAssignmentDA clientAssignmentDA = new ClientAssignmentDA(genericDA);
            /* Total Tables in DataSet              
             * Index 0 : Dealer                     (Dealer)
             * Index 1 : PreferenceList             (dtPreference)
             * Index 2 : Uncontacted Assignment     (dtUnContactedAssignment)
             * Index 3 : Contact Entry for Today    (dtHistoryEntry)
             * Index 4 : Return DataTable which contains return code and return message     (ReturnTable).
             */


            try
            {
                dtReturn = CommonUtilities.CreateReturnTable("1", "");

                genericDA.OpenConnection();

                if (userRole == "admin")
                {
                    //Retrieve all Dealer for Contact Entry Admin
                    ds.Tables.Add(commonServiceDA.RetrieveAllDealer(userId));
                    ds.Tables.Add(clientAssignmentDA.RetrieveContactEntryProjectInfo(userId));
                }
                else
                {
                    //Retrieve Dealer code
                    ds.Tables.Add(dealerDA.RetrieveDealerByUserId(userId));
                }                

                //Retrieve Preference List
                ds.Tables.Add(commonServiceDA.RetrievePreferenceCodeAndName());

                if (userRole == "user")
                {
                    if (ds.Tables[0].Rows.Count < 1)
                    {
                        dtReturn.Rows[0]["ReturnCode"] = "0";
                        dtReturn.Rows[0]["ReturnMessage"] = "No Dealer Code found! Please try again.";
                    }
                    else
                    {
                        #region Added by Thet Maung Chaw to be able to select all AECodes

                        StringBuilder sb = new StringBuilder();
                        String AECodes;

                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            sb.Append(dr["AECode"].ToString().Trim() + "','");
                        }

                        AECodes = sb.ToString().Substring(0, sb.Length - 3);

                        #endregion

                        /// <Updated by Thet Maung Chaw>
                        //ds.Tables.Add(clientContactDA.RetrieveUnContactedAssignment(ds.Tables[0].Rows[0]["AECode"].ToString()));
                        ds.Tables.Add(clientContactDA.RetrieveUnContactedAssignment(AECodes, userId));

                        if (ds.Tables[0].Rows.Count < 1)
                        {
                            dtReturn.Rows[0]["ReturnMessage"] = "No Uncontacted Assignments exist for current dealer.";
                        }

                        //Retrieve Contact Entry for Today
                        /// <Updated by Thet Maung Chaw>
                        //clientContactDA.RetrieveContactEntryForToday(ds, ds.Tables[0].Rows[0]["AECode"].ToString());
                        clientContactDA.RetrieveContactEntryForToday(ds, AECodes, userId);
                    }
                }
            }
            catch (Exception e)
            {
                dtReturn.Rows[0]["ReturnCode"] = "-1";
                dtReturn.Rows[0]["ReturnMessage"] = "Error in preparing for Contact Entry! Please try again.";
            }
            finally
            {
                try
                {
                    genericDA.CloseConnection();
                }
                catch (Exception e)
                { }
            }

            ds.Tables.Add(dtReturn);

            return ds;
        }

        /// <OC>
        public DataSet RetrieveByAccountNo(String AccountNo, String Param)
        {
            DataSet ds = new DataSet();
            DataTable dtReturn = null;

            try
            {
                dtReturn = CommonUtilities.CreateReturnTable("1", "");

                genericDA.OpenConnection();
                
                ds.Tables.Add(clientContactDA.RetrieveByAccountNo(AccountNo, Param));

                if (ds.Tables[0].Rows.Count < 1)
                {
                    dtReturn.Rows[0]["ReturnCode"] = "0";
                    dtReturn.Rows[0]["ReturnMessage"] = "No Record found! Please try again.";
                }
            }
            catch (Exception e)
            {
                dtReturn.Rows[0]["ReturnCode"] = "-1";
                dtReturn.Rows[0]["ReturnMessage"] = "Error in retrieving for Contact Entry! Please try again.";
            }
            finally
            {
                try
                {
                    genericDA.CloseConnection();
                }
                catch (Exception e)
                { }
            }

            ds.Tables.Add(dtReturn);

            return ds;
        }

        public DataSet PrepareForContactHistory(String UserID)
        {
            DataSet ds = new DataSet();
            DataTable dtReturn = null;
            CommonServiceDA commonServiceDA = new CommonServiceDA(genericDA);

            try
            {
                dtReturn = CommonUtilities.CreateReturnTable("1", "");

                genericDA.OpenConnection();

                //Retrieve All Dealer code
                ds.Tables.Add(commonServiceDA.RetrieveAllDealer(UserID));

                //Retrieve Preference List
                ds.Tables.Add(commonServiceDA.RetrievePreferenceCodeAndName());

                //Retrieve All Team Name and Code
                ds.Tables.Add(commonServiceDA.RetrieveTeamCodeAndNameByDataTable());

                if (ds.Tables[0].Rows.Count < 1)
                {
                    dtReturn.Rows[0]["ReturnCode"] = "0";
                    dtReturn.Rows[0]["ReturnMessage"] = "No Dealer Code found! Please try again.";
                }                
            }
            catch (Exception e)
            {
                dtReturn.Rows[0]["ReturnCode"] = "-1";
                dtReturn.Rows[0]["ReturnMessage"] = "Error in preparing for Contact History! Please try again.";
            }
            finally
            {
                try
                {
                    genericDA.CloseConnection();
                }
                catch (Exception e)
                { }
            }

            ds.Tables.Add(dtReturn);
            return ds;
        }

        /// <Added by OC>
        public DataSet PrepareForContactHistoryByUserOrSupervisor(String Param, String UserID)
        {
            DataSet ds = new DataSet();
            DataTable dtReturn = null;
            CommonServiceDA commonServiceDA = new CommonServiceDA(genericDA);

            try
            {
                dtReturn = CommonUtilities.CreateReturnTable("1", "");

                genericDA.OpenConnection();

                //Retrieve All Dealer code
                ds.Tables.Add(commonServiceDA.RetrieveAllDealerByUserOrSupervisor("UserType NOT LIKE 'FAR%' ", UserID));

                //Retrieve Preference List
                ds.Tables.Add(commonServiceDA.RetrievePreferenceCodeAndName());

                //Retrieve All Team Name and Code
                ds.Tables.Add(commonServiceDA.RetrieveTeamCodeAndNameByDataTableByUserOrSupervisor(Param, UserID));

                if (ds.Tables[0].Rows.Count < 1)
                {
                    dtReturn.Rows[0]["ReturnCode"] = "0";
                    dtReturn.Rows[0]["ReturnMessage"] = "No Dealer Code found! Please try again.";
                }
            }
            catch (Exception e)
            {
                dtReturn.Rows[0]["ReturnCode"] = "-1";
                dtReturn.Rows[0]["ReturnMessage"] = "Error in preparing for Contact History! Please try again.";
            }
            finally
            {
                try
                {
                    genericDA.CloseConnection();
                }
                catch (Exception e)
                { }
            }

            ds.Tables.Add(dtReturn);
            return ds;
        }

        //For Contact Entry Admin
        public DataSet RetrieveUnContactedAssignment(string dealerCode, String UserID)
        {
            DataSet ds = new DataSet();
            DataTable dtReturn = null;
            string returnCode = "1", returnMessage = "";

            try
            {
                genericDA.OpenConnection();

                ds.Tables.Add(clientContactDA.RetrieveUnContactedAssignment(dealerCode, UserID));
                if (ds.Tables[0].Rows.Count < 1)
                {
                    //returnCode = "0";
                    returnMessage = "No Uncontacted Assignment Found!";
                }

                //To retrieve Entry History for Contact Entry Admin
                clientContactDA.RetrieveContactEntryForToday(ds, dealerCode, UserID);
                if (ds.Tables["dtEntryHistory"].Rows.Count < 1)
                {
                    returnMessage = "";
                }
            }
            catch (Exception e)
            {
                returnCode = "-1";
                returnMessage = "Error in retrieving Uncontacted Assignment!";
            }
            finally
            {
                //Create Table for ReturnCode and Return Message
                dtReturn = CommonUtilities.CreateReturnTable(returnCode, returnMessage);
                ds.Tables.Add(dtReturn);

                try
                {
                    genericDA.CloseConnection();
                }
                catch (Exception e)
                { }
            }

            return ds;
        }

        public DataSet RetrieveUnContactedAssignmentByProjectID(string dealerCode, String UserID)
        {
            DataSet ds = new DataSet();
            DataTable dtReturn = null;
            string returnCode = "1", returnMessage = "";

            try
            {
                genericDA.OpenConnection();

                ds.Tables.Add(clientContactDA.RetrieveUnContactedAssignmentByProjectID(dealerCode));
                if (ds.Tables[0].Rows.Count < 1)
                {
                    //returnCode = "0";
                    returnMessage = "No Uncontacted Assignment Found!";
                }

                //To retrieve Entry History for Contact Entry Admin
                clientContactDA.RetrieveContactEntryForToday(ds, dealerCode, UserID);
                if (ds.Tables["dtEntryHistory"].Rows.Count < 1)
                {
                    returnMessage = "";
                }
            }
            catch (Exception e)
            {
                returnCode = "-1";
                returnMessage = "Error in retrieving Uncontacted Assignment!";
            }
            finally
            {
                //Create Table for ReturnCode and Return Message
                dtReturn = CommonUtilities.CreateReturnTable(returnCode, returnMessage);
                ds.Tables.Add(dtReturn);

                try
                {
                    genericDA.CloseConnection();
                }
                catch (Exception e)
                { }
            }

            return ds;
        }
        
        public DataSet RetrieveClientInfoByShortKey(string shortKey)
        {
            DataSet ds = new DataSet();
            DataTable dtReturn = null;
            string returnCode = "1", returnMessage = "";

            try
            {
                genericDA.OpenConnection();

                ds.Tables.Add(clientContactDA.RetrieveClientInfoByShortKey(shortKey));
                if (ds.Tables[0].Rows.Count < 1)
                {
                    returnCode = "0";
                    returnMessage = "Client not Found! Please change search Short Key.";
                }
            }
            catch (Exception e)
            {
                returnCode = "-1";
                returnMessage = "Error in retrieving Client by Short Key!";
            }
            finally
            {
                //Create Table for ReturnCode and Return Message
                dtReturn = CommonUtilities.CreateReturnTable(returnCode, returnMessage);
                ds.Tables.Add(dtReturn);

                try
                {
                    genericDA.CloseConnection();
                }
                catch (Exception e)
                { }
            }

            return ds;
        }


        public DataSet GetDetailUserInformation(string accountNo)
        {
            DataSet ds = new DataSet();
            DataTable dtReturn = null;
            string returnCode = "1", returnMessage = "";

            try
            {
                genericDA.OpenConnection();

                ds.Tables.Add(clientContactDA.GetDetailUserInformation(accountNo));
                if (ds.Tables[0].Rows.Count < 1)
                {
                    returnCode = "0";
                    returnMessage = "User Information not Found!";
                }
            }
            catch (Exception e)
            {
                returnCode = "-1";
                returnMessage = "Error in retrieving User Information records!";
            }
            finally
            {
                //Create Table for ReturnCode and Return Message
                dtReturn = CommonUtilities.CreateReturnTable(returnCode, returnMessage);
                ds.Tables.Add(dtReturn);

                try
                {
                    genericDA.CloseConnection();
                }
                catch (Exception e)
                { }
            }

            return ds;
        }

        public DataSet GetSeminorRegistrationByAccNo(string acctNo)
        {
            DataSet ds = new DataSet();
            DataTable dtReturn = null;
            string returnCode = "1", returnMessage = "";
            try
            {
                genericDA.OpenConnection();

                ds.Tables.Add(clientContactDA.GetSeminorInformationByAccNo(acctNo));
                if (ds.Tables[0].Rows.Count < 1)
                {
                    returnCode = "0";
                    returnMessage = "Seminar Registration not Found!";
                }
            }
            catch (Exception e)
            {
                returnCode = "-1";
                returnMessage = "Error in retrieving Seminar Registration records!";
            }
            finally
            {
                //Create Table for ReturnCode and Return Message
                dtReturn = CommonUtilities.CreateReturnTable(returnCode, returnMessage);
                ds.Tables.Add(dtReturn);

                try
                {
                    genericDA.CloseConnection();
                }
                catch (Exception e)
                { }
            }

            return ds;
        }

        public DataSet getContactHistoryByAccountNo(string accountNo)
        {
            DataSet ds = new DataSet();
            DataTable dtReturn = null;
            string returnCode = "1", returnMessage = "";

            try
            {
                genericDA.OpenConnection();

                ds.Tables.Add(clientContactDA.getContactHistoryByAccountNo(accountNo));
                if (ds.Tables[0].Rows.Count < 1)
                {
                    returnCode = "0";
                    returnMessage = "Contact History not Found!";
                }
            }
            catch (Exception e)
            {
                returnCode = "-1";
                returnMessage = "Error in retrieving Contact History records!";
            }
            finally
            {
                //Create Table for ReturnCode and Return Message
                dtReturn = CommonUtilities.CreateReturnTable(returnCode, returnMessage);
                ds.Tables.Add(dtReturn);

                try
                {
                    genericDA.CloseConnection();
                }
                catch (Exception e)
                { }
            }

            return ds;
        }

        public DataSet RetrieveContactHistoryByCriteria(string accountNo, string dealerCode, string dateFrom, string dateTo, string rank,
                            string preference, string content, string teamCode, String UserID)
        {
            DataSet ds = new DataSet();
            DataTable dtReturn = null;
            string returnCode = "1", returnMessage = "";

            try
            {
                genericDA.OpenConnection();

                ds.Tables.Add(clientContactDA.RetrieveContactHistoryByCriteria(accountNo, dealerCode, dateFrom, dateTo, rank, preference, content, teamCode, UserID));
                if (ds.Tables[0].Rows.Count < 1)
                {
                    returnCode = "0";
                    returnMessage = "No record found! Pls change the search criteria!";
                }
            }
            catch (Exception e)
            {
                returnCode = "-1";
                returnMessage = "Error in retrieving Contact History records!";
            }
            finally
            {
                //Create Table for ReturnCode and Return Message
                dtReturn = CommonUtilities.CreateReturnTable(returnCode, returnMessage);
                ds.Tables.Add(dtReturn);

                try
                {
                    genericDA.CloseConnection();
                }
                catch (Exception e)
                { }
            }

            return ds;
        }

        public DataSet RetrieveContactAnalysis(string dealerCode, string accountNo, string dateFrom, string dateTo)
        {
            DataSet ds = new DataSet();
            DataTable dtReturn = null;
            string returnCode = "1", returnMessage = "";

            try
            {
                genericDA.OpenConnection();

                ds.Tables.Add(clientContactDA.RetrieveContactAnalysis(dealerCode, accountNo, dateFrom, dateTo));
                if (ds.Tables[0].Rows.Count < 1)
                {
                    returnCode = "0";
                    returnMessage = "Contact Analysis not Found!";
                }
            }
            catch (Exception e)
            {
                returnCode = "-1";
                returnMessage = "Error in retrieving Client Analysis records! <br /> Error Message : " + e.Message;
            }
            finally
            {
                //Create Table for ReturnCode and Return Message
                dtReturn = CommonUtilities.CreateReturnTable(returnCode, returnMessage);
                ds.Tables.Add(dtReturn);

                try
                {
                    genericDA.CloseConnection();
                }
                catch (Exception e)
                { }
            }

            return ds;
        }

        public DataSet RetrieveCallReport(string assignDate, String UserID)
        {
            DataSet ds = new DataSet();
            DataTable dtReturn = null;
            string returnCode = "1", returnMessage = "";

            try
            {
                genericDA.OpenConnection();

                ds.Tables.Add(clientContactDA.RetrieveCallReport(assignDate, UserID));
                if (ds.Tables[0].Rows.Count < 1)
                {
                    returnCode = "0";
                    returnMessage = "Call Report not Found!";
                }
            }
            catch (Exception e)
            {
                returnCode = "-1";
                returnMessage = "Error in retrieving Call Report records! <br /> Error Message : " + e.Message;
            }
            finally
            {
                //Create Table for ReturnCode and Return Message
                dtReturn = CommonUtilities.CreateReturnTable(returnCode, returnMessage);
                ds.Tables.Add(dtReturn);

                try
                {
                    genericDA.CloseConnection();
                }
                catch (Exception e)
                { }
            }

            return ds;
        }

        public DataSet RetrieveCallReportByUserOrSupervisor(String assignDate, String Param, String UserID)
        {
            DataSet ds = new DataSet();
            DataTable dtReturn = null;
            string returnCode = "1", returnMessage = "";

            try
            {
                genericDA.OpenConnection();

                ds.Tables.Add(clientContactDA.RetrieveCallReportByUserOrSupervisor(assignDate, Param, UserID));
                if (ds.Tables[0].Rows.Count < 1)
                {
                    returnCode = "0";
                    returnMessage = "Call Report not Found!";
                }
            }
            catch (Exception e)
            {
                returnCode = "-1";
                returnMessage = "Error in retrieving Call Report records! <br /> Error Message : " + e.Message;
            }
            finally
            {
                //Create Table for ReturnCode and Return Message
                dtReturn = CommonUtilities.CreateReturnTable(returnCode, returnMessage);
                ds.Tables.Add(dtReturn);

                try
                {
                    genericDA.CloseConnection();
                }
                catch (Exception e)
                { }
            }

            return ds;
        }

        public DataSet RetrieveCallReportDetail(string assignDate, string dealerCode)
        {
            DataSet ds = new DataSet();
            DataTable dtReturn = null;
            string returnCode = "1", returnMessage = "";

            try
            {
                genericDA.OpenConnection();

                ds.Tables.Add(clientContactDA.RetrieveCallReportDetail(assignDate, dealerCode));
                if (ds.Tables[0].Rows.Count < 1)
                {
                    returnCode = "0";
                    returnMessage = "Call Report Details not Found!";
                }
            }
            catch (Exception e)
            {
                returnCode = "-1";
                returnMessage = "Error in retrieving Call Report Details records! <br /> Error Message : " + e.Message;
            }
            finally
            {
                //Create Table for ReturnCode and Return Message
                dtReturn = CommonUtilities.CreateReturnTable(returnCode, returnMessage);
                ds.Tables.Add(dtReturn);

                try
                {
                    genericDA.CloseConnection();
                }
                catch (Exception e)
                { }
            }

            return ds;
        }
        public DataSet RetrieveCallReportProjectDetail(string ProjectName, string dealerCode)
        {
            DataSet ds = new DataSet();
            DataTable dtReturn = null;
            string returnCode = "1", returnMessage = "";
            string projtype = RetrieveProjectType(ProjectName);

            try
            {
                genericDA.OpenConnection();
                if (projtype == "C")
                {
                 ds.Tables.Add(clientContactDA.RetrieveCallReportProjectDetail(ProjectName, dealerCode)); 
                }

                if (projtype == "L")
                {
                    ds.Tables.Add(clientContactDA.RetrieveCallReportLeadProjectDetail(ProjectName, dealerCode));
                }
                if (ds.Tables[0].Rows.Count < 1)
                {
                    returnCode = "0";
                    returnMessage = "Call Report Details not Found!";
                }
            }
            catch (Exception e)
            {
                returnCode = "-1";
                returnMessage = "Error in retrieving Call Report Details records! <br /> Error Message : " + e.Message;
            }
            finally
            {
                //Create Table for ReturnCode and Return Message
                dtReturn = CommonUtilities.CreateReturnTable(returnCode, returnMessage);
                ds.Tables.Add(dtReturn);

                try
                {
                    genericDA.CloseConnection();
                }
                catch (Exception e)
                { }
            }

            return ds;
        }

        public DataSet RetrieveClientAnalysis(string teamCode, string dealerCode, string accountCreateDate, string lastTradeDate)
        {
            DataSet ds = new DataSet();
            DataTable dtReturn = null;
            string returnCode = "1", returnMessage = "";

            try
            {
                genericDA.OpenConnection();

                ds.Tables.Add(clientContactDA.RetrieveClientAnalysis(teamCode, dealerCode, accountCreateDate, lastTradeDate));
                if (ds.Tables[0].Rows.Count < 1)
                {
                    returnCode = "0";
                    returnMessage = "Client Analysis not found! Please change your criteria.";
                }
            }
            catch (Exception e)
            {
                returnCode = "-1";
                returnMessage = "Error in retrieving Call Report Details records! <br /> Error Message : " + e.Message;
            }
            finally
            {
                //Create Table for ReturnCode and Return Message
                dtReturn = CommonUtilities.CreateReturnTable(returnCode, returnMessage);
                ds.Tables.Add(dtReturn);

                try
                {
                    genericDA.CloseConnection();
                }
                catch (Exception e)
                { }
            }

            return ds;
        }

        public DataSet PrepareForClientAnalysis(String UserID)
        {
            CommonServiceDA commonServiceDA = new CommonServiceDA(genericDA);
            DataSet ds = new DataSet();
            DataTable dtReturn = null;
            string returnCode = "1", returnMessage = "";

            try
            {
                genericDA.OpenConnection();

                //ds.Tables.Add(commonServiceDA.RetrieveTeamCodeAndNameByDataTable());
                ds.Tables.Add(commonServiceDA.RetrieveAllTeamCodeAndName(UserID));
                
                if (ds.Tables[0].Rows.Count < 1)
                {
                    returnCode = "0";
                    returnMessage = "Dealer Team cannot retrieve.";
                }

                //ds.Tables.Add(commonServiceDA.RetrieveAllDealer());
                //if (ds.Tables[1].Rows.Count < 1)
                //{
                //    returnCode = "0";
                //    returnMessage = "Dealers cannot retrieve.";
                //}
            }
            catch (Exception e)
            {
                returnCode = "-1";
                returnMessage = "";
                //returnMessage = "Error in retrieving Call Report Details records! <br /> Error Message : " + e.Message;
            }
            finally
            {
                //Create Table for ReturnCode and Return Message
                dtReturn = CommonUtilities.CreateReturnTable(returnCode, returnMessage);
                ds.Tables.Add(dtReturn);

                try
                {
                    genericDA.CloseConnection();
                }
                catch (Exception e)
                { }
            }

            return ds;
        }

        /// <Added by OC>
        public DataSet PrepareForClientAnalysisByUserOrSupervisor(String Param, String UserID)
        {
            CommonServiceDA commonServiceDA = new CommonServiceDA(genericDA);
            DataSet ds = new DataSet();
            DataTable dtReturn = null;
            string returnCode = "1", returnMessage = "";

            try
            {
                genericDA.OpenConnection();

                //ds.Tables.Add(commonServiceDA.RetrieveTeamCodeAndNameByDataTable());
                ds.Tables.Add(commonServiceDA.RetrieveTeamCodeAndNameByDataTableByUserOrSupervisor(Param, UserID));

                if (ds.Tables[0].Rows.Count < 1)
                {
                    returnCode = "0";
                    returnMessage = "Dealer Team cannot retrieve.";
                }

                //ds.Tables.Add(commonServiceDA.RetrieveAllDealer());
                //if (ds.Tables[1].Rows.Count < 1)
                //{
                //    returnCode = "0";
                //    returnMessage = "Dealers cannot retrieve.";
                //}
            }
            catch (Exception e)
            {
                returnCode = "-1";
                returnMessage = "";
                //returnMessage = "Error in retrieving Call Report Details records! <br /> Error Message : " + e.Message;
            }
            finally
            {
                //Create Table for ReturnCode and Return Message
                dtReturn = CommonUtilities.CreateReturnTable(returnCode, returnMessage);
                ds.Tables.Add(dtReturn);

                try
                {
                    genericDA.CloseConnection();
                }
                catch (Exception e)
                { }
            }

            return ds;
        }

        public DataSet InsertClientContact(string dealerCode, string userId, string acctNo, string shortKey, string sex, string contactPhone, string content,
                string preferA, string preferB, string remark, string rank, string keep, string adminId, string lastAcctNo, string lastClientName, int followUpStatus, string followUpDealer, string followUpDate,string projectID)
        {
            int result = 1, insertedId = 0;
            string returnMessage = "Contact Record is saved successfully.", teamCode = "", assignDate = ""; 
            DataSet ds = new DataSet();
            DataTable dtContactHistory = null, dtReturn = null;
            DataRow drNewContact = null;
            DealerDA dealerDA = new DealerDA(genericDA);

            try
            {
                genericDA.OpenConnection();
                genericDA.CreateSqlCommand();

                result = clientContactDA.InsertClientContact(dealerCode, userId, acctNo, shortKey, sex, contactPhone, content, preferA, preferB, remark,
                                    rank, keep, adminId, "", followUpStatus, followUpDealer, followUpDate, projectID);

                if (result < 1)
                {
                    returnMessage = "Error in saving Contact Record!";
                }
                else
                {
                    insertedId = result;
                    result = 1;     //Return as successful code

                    if (!lastAcctNo.Equals(acctNo))
                    {
                        //New Inserted
                        dtContactHistory = clientContactDA.getContactHistoryByAccountNo(acctNo);
                    }
                    else
                    {
                        DataTable dtDealer = null, dtClient = null, dtAssignment = null;
                        ClientAssignmentDA clientAssignmentDA = new ClientAssignmentDA(genericDA);

                        //Old way to retrieve ClientName, TeamCode for adding Contact Entry for existing AccountNo
                        if (String.IsNullOrEmpty(lastClientName))
                        {
                            dtClient = clientContactDA.GetClientByAcctNo(acctNo);
                            if (dtClient.Rows.Count > 0)
                            {
                                lastClientName = dtClient.Rows[0]["LNAME"].ToString();
                            }
                        }

                        //Retrieve Dealer Team Code for new added Account No
                        dtDealer = dealerDA.RetrieveDealerByDealerCode(dealerCode);
                        if (dtDealer.Rows.Count > 0)
                        {
                            teamCode = dtDealer.Rows[0]["AEGroup"].ToString();
                        }

                        //Check for Extra Call
                        dtAssignment = clientAssignmentDA.RetrieveAssignmentForExtraCall(acctNo, dealerCode, DateTime.Now.ToString("yyyy-MM-dd"));
                        if (dtAssignment.Rows.Count > 0)
                        {
                            assignDate = dtAssignment.Rows[0]["AssignDate"].ToString();
                        }

                        dtContactHistory = new DataTable("dtContactHistory");
                        dtContactHistory.Columns.Add("RecId", String.Empty.GetType());
                        dtContactHistory.Columns.Add("ContactDate", String.Empty.GetType());
                        dtContactHistory.Columns.Add("AEGroup", String.Empty.GetType());
                        dtContactHistory.Columns.Add("AcctNo", String.Empty.GetType());
                        dtContactHistory.Columns.Add("CName", String.Empty.GetType());
                        dtContactHistory.Columns.Add("Sex", String.Empty.GetType());
                        dtContactHistory.Columns.Add("Phone", String.Empty.GetType());
                        dtContactHistory.Columns.Add("Content", String.Empty.GetType());
                        dtContactHistory.Columns.Add("Remarks", String.Empty.GetType());
                        dtContactHistory.Columns.Add("PreferA", String.Empty.GetType());
                        dtContactHistory.Columns.Add("PreferB", String.Empty.GetType());
                        dtContactHistory.Columns.Add("Rank", String.Empty.GetType());
                        dtContactHistory.Columns.Add("ModifiedUser", String.Empty.GetType());
                        dtContactHistory.Columns.Add("AssignDate", String.Empty.GetType());
                        dtContactHistory.Columns.Add("RankText", String.Empty.GetType());
                        dtContactHistory.Columns.Add("projectID", String.Empty.GetType());

                        dtContactHistory.Columns.Add("SeminarName", String.Empty.GetType());
                        dtContactHistory.Columns.Add("FollowUpBy", String.Empty.GetType());
                        dtContactHistory.Columns.Add("FollowUpStatus", String.Empty.GetType());
                        dtContactHistory.Columns.Add("FollowUpDate", String.Empty.GetType());

                        drNewContact = dtContactHistory.NewRow();
                        drNewContact["RecId"] = insertedId.ToString();
                        drNewContact["ContactDate"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        drNewContact["AEGroup"] = teamCode;
                        drNewContact["AcctNo"] = acctNo;
                        drNewContact["CName"] = lastClientName;
                        drNewContact["Sex"] = sex;
                        drNewContact["Phone"] = contactPhone;
                        drNewContact["Content"] = content;
                        drNewContact["Remarks"] = remark;
                        drNewContact["PreferA"] = preferA;
                        drNewContact["PreferB"] = preferB;
                        drNewContact["Rank"] = rank;
                        drNewContact["ModifiedUser"] = dealerCode;
                        drNewContact["AssignDate"] = assignDate;
                        drNewContact["RankText"] = CommonUtilities.GetClientRank(int.Parse(rank));
                        drNewContact["projectID"] = projectID;

                        //20110927
                        drNewContact["SeminarName"] = "";
                        //drNewContact["FollowUpStatus"] = "";  Updated by OC
                        //drNewContact["FollowUpDate"] = "1990-01-01";   Updated by OC

                        /// <Updated by Thet Maung Chaw>
                        if (followUpStatus == 1)
                        {
                            drNewContact["FollowUpStatus"] = "Not Yet";
                        }
                        else if (followUpStatus == 2)
                        {
                            drNewContact["FollowUpStatus"] = "Done";
                        }
                        else
                        {
                            drNewContact["FollowUpStatus"] = "No Need";
                        }

                        drNewContact["FollowUpDate"] = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff"); //followUpDate;
                        drNewContact["FollowUpBy"] = followUpDealer;

                        dtContactHistory.Rows.Add(drNewContact);
                    }

                    ds.Tables.Add(dtContactHistory);
                }
            }
            catch (Exception e)
            {
                result = -1;
                returnMessage = "Error in saving Contact Record! Exception Message : " + e.Message;
            }
            finally
            {
                try
                {
                    dtReturn = CommonUtilities.CreateReturnTable(result.ToString(), returnMessage);
                    ds.Tables.Add(dtReturn);

                    genericDA.DisposeSqlCommand();
                    genericDA.CloseConnection();
                }
                catch (Exception e)
                { }
            }

            return ds;
        }

        public string[] UpdateClientContact(string dealerCode, string userId, string acctNo, string shortKey, string sex, string contactPhone, string content,
                string preferA, string preferB, string remark, string rank, string keep, string adminId, string recId,int followup, string followUpDealer, string followUpDate,string projectID)
        {            
            int result = -1;
            string returnMessage = "Client Contact record is updated successfully.";

            try
            {
                genericDA.OpenConnection();
                genericDA.CreateSqlCommand();

                //result = clientContactDA.UpdateClientContact(dealerCode, userId, acctNo, shortKey, sex, contactPhone, content, preferA, preferB, remark,
                //                    rank, keep, adminId, recId, followup, followUpDealer, followUpDate, projectID);

                result = clientContactDA.UpdateClientContact_OnlyTheFollowUpStatus(dealerCode, userId, acctNo, shortKey, sex, contactPhone, content, preferA, preferB, remark,
                                    rank, keep, adminId, recId, followup, followUpDealer, followUpDate, projectID);
                if (result < 1)
                {
                    returnMessage = "Error in updating Client Contact record!";
                }
            }
            catch (Exception e)
            {
                result = -1;
                returnMessage = "Error in updating Client Contact record! Please try again. Exception Message: " + e.Message;
            }
            finally
            {
                try
                {
                    genericDA.DisposeSqlCommand();
                    genericDA.CloseConnection();
                }
                catch (Exception e)
                { }
            }

            return new string[] { result.ToString(), returnMessage }; 
        }

        public String[] UpdateClientContactFollowUpStatus(String recId, String followUpStatus)
        {
            int result = -1;
            String returnMessage = "Client Contact Follow-up status is updated successfully.";
            try
            {
                genericDA.OpenConnection();
                genericDA.CreateSqlCommand();

                result = clientContactDA.UpdateClientContactFollowUpStatus(recId, followUpStatus);
                if (result < 1)
                {
                    returnMessage = "Error in updating client contact follow-up status";
                }
            }
            catch (Exception e)
            {
                result = -1;
                returnMessage = "Error in updating Client Contact Follow-Up status! Please try again. Exception Message: " + e.Message;
            }
            finally
            {
                try
                {
                    genericDA.DisposeSqlCommand();
                    genericDA.CloseConnection();
                }
                catch (Exception e)
                { }
            }
            return new String[] { result.ToString(), returnMessage };
        }

        public DataSet DeleteClientContact(string recId, string dealerCode, String UserID)
        {
            int result = -1;
            string returnMessage = "Client Contact record is deleted successfully.";
            DataSet ds = new DataSet();
            DataTable dtReturn = null;

            try
            {
                genericDA.OpenConnection();
                genericDA.CreateSqlCommand();

                result = clientContactDA.DeleteClientContact(recId);
                if (result < 1)
                {
                    returnMessage = "Error in deleting Client Contact record! Client Contact has been deleted by other user!";
                }
                else
                {
                    ds.Tables.Add(clientContactDA.RetrieveUnContactedAssignment(dealerCode, UserID));
                }
            }
            catch (Exception e)
            {
                result = -1;
                returnMessage = "Error in deleting Client Contact record! Please try again. Exception Message: " + e.Message;
            }
            finally
            {
                //Create Table for ReturnCode and Return Message
                dtReturn = CommonUtilities.CreateReturnTable(result.ToString(), returnMessage);
                ds.Tables.Add(dtReturn);

                try
                {
                    genericDA.DisposeSqlCommand();
                    genericDA.CloseConnection();
                }
                catch (Exception e)
                { }
            }

            //return new string[] { result.ToString(), returnMessage }; 

            return ds;
        }


        public DataSet RetrieveContactEntryForToday(string dealerCode, String UserID)
        {
            DataSet ds = new DataSet();

            DataTable dtReturn = null;
            string returnCode = "1", returnMessage = "";

            try
            {
                genericDA.OpenConnection();

                clientContactDA.RetrieveContactEntryForToday(ds, dealerCode, UserID);
                if (ds.Tables[0].Rows.Count < 1)
                {
                    returnCode = "0";
                    returnMessage = "Client Contact not found!";
                }
            }
            catch (Exception e)
            {
                returnCode = "-1";
                returnMessage = "Error in retrieving Entry History records! <br /> Error Message : " + e.Message;
            }
            finally
            {
                //Create Table for ReturnCode and Return Message
                dtReturn = CommonUtilities.CreateReturnTable(returnCode, returnMessage);
                ds.Tables.Add(dtReturn);

                try
                {
                    genericDA.CloseConnection();
                }
                catch (Exception e)
                { }
            }

            return ds;
        }

        //Dev Thiri
        public DataSet GetClientByAcctNo(string AccNo)
        {
            DataSet ds = new DataSet();

            DataTable dtReturn = null;
            string returnCode = "1", returnMessage = "";

            try
            {
                genericDA.OpenConnection();

                ds.Tables.Add(clientContactDA.GetClientByAcctNo(AccNo));
                if (ds.Tables[0].Rows.Count < 1)
                {
                    returnCode = "0";
                    returnMessage = "Client Contact not found!";
                }
            }
            catch (Exception e)
            {
                returnCode = "-1";
                returnMessage = "Error in retrieving Client Contact records! <br /> Error Message : " + e.Message;
            }
            finally
            {
                //Create Table for ReturnCode and Return Message
                dtReturn = CommonUtilities.CreateReturnTable(returnCode, returnMessage);
                ds.Tables.Add(dtReturn);

                try
                {
                    genericDA.CloseConnection();
                }
                catch (Exception e)
                { }
            }
            return ds;
        }
        //End Dev Thiri

        /**************Update by TSM**************/

        public DataSet RetrieveCallReportByProjectName(string projectname, String UserID)
        {

            DataSet ds = new DataSet();
            DataTable dtReturn = null;
            string returnCode = "1", returnMessage = "";
            string projtype = RetrieveProjectType(projectname);
            try
            {
                genericDA.OpenConnection();
                if (projtype == "C")
                {
                    ds.Tables.Add(clientContactDA.RetrieveCallReportByProject(projectname, UserID));
                }
                else
                {
                    ds.Tables.Add(clientContactDA.RetrieveCallReportByLeadProject(projectname, UserID));
                }


                if (ds.Tables[0].Rows.Count < 1)
                {
                    returnCode = "0";
                    returnMessage = "Call Report not Found!";
                }
            }
            catch (Exception e)
            {
                returnCode = "-1";
                returnMessage = "Error in retrieving Call Report records! <br /> Error Message : " + e.Message;
            }

            finally
            {
                //Create Table for ReturnCode and Return Message
                dtReturn = CommonUtilities.CreateReturnTable(returnCode, returnMessage);
                ds.Tables.Add(dtReturn);

                try
                {
                    genericDA.CloseConnection();
                }
                catch (Exception e)
                { }
            }

            return ds;
        }

        /// <Added by OC>
        public DataSet RetrieveCallReportByProjectNameByUserOrSupervisor(String projectname, String Param)
        {

            DataSet ds = new DataSet();
            DataTable dtReturn = null;
            string returnCode = "1", returnMessage = "";
            string projtype = RetrieveProjectType(projectname);
            try
            {
                genericDA.OpenConnection();
                if (projtype == "C")
                {
                    ds.Tables.Add(clientContactDA.RetrieveCallReportByProjectByUserOrSupervisor(projectname, Param));
                }
                else
                {
                    ds.Tables.Add(clientContactDA.RetrieveCallReportByLeadProjectByUserOrSupervisor(projectname, Param));
                }

                if (ds.Tables[0].Rows.Count < 1)
                {
                    returnCode = "0";
                    returnMessage = "Call Report not Found!";
                }
            }
            catch (Exception e)
            {
                returnCode = "-1";
                returnMessage = "Error in retrieving Call Report records! <br /> Error Message : " + e.Message;
            }

            finally
            {
                //Create Table for ReturnCode and Return Message
                dtReturn = CommonUtilities.CreateReturnTable(returnCode, returnMessage);
                ds.Tables.Add(dtReturn);

                try
                {
                    genericDA.CloseConnection();
                }
                catch (Exception e)
                { }
            }

            return ds;
        }

        public String RetrieveProjectType(string projectname)
        {
            DataTable dtProjtype = new DataTable();
            DataSet ds = new DataSet();

            //DataTable dtReturn = null;
            //string returnCode = "1", returnMessage = "";
            string ProjectType = null;


            genericDA.OpenConnection();
            dtProjtype = clientContactDA.RetrieveProjectType(projectname);

            if (dtProjtype != null)
            {
                if (dtProjtype.Rows.Count > 0)
                {
                    ProjectType = dtProjtype.Rows[0]["ProjectType"].ToString();
                }
            }
            else
            {
                ProjectType = null;

            }

            return ProjectType;
        }
        /**************End TSM**************/

        /**************Update by TSM**************/
        public DataSet RetrieveContactHistoryByProjName(string accountNo, string dealerCode, string ProjName, string rank,
                            string preference, string content, string teamCode, String UserID)
        {
            DataSet ds = new DataSet();
            DataTable dtReturn = null;
            string returnCode = "1", returnMessage = "";
            string projtype = RetrieveProjectType(ProjName);
          
            try
            {
                genericDA.OpenConnection();
                if (projtype == "C")
                {
                    ds.Tables.Add(clientContactDA.RetrieveContactHistoryByClientProjName(accountNo, dealerCode, ProjName, rank, preference, content, teamCode, UserID));
                }
                else if (projtype == "L")
                {
                    ds.Tables.Add(clientContactDA.RetrieveContactHistoryByLeadProjName(accountNo, dealerCode, ProjName, rank, preference, content, teamCode, UserID));
                }
               

                
                if (ds.Tables[0].Rows.Count < 1)
                {
                    returnCode = "0";
                    returnMessage = "No record found! Pls change the search criteria!";
                }
            }
            catch (Exception e)
            {
                returnCode = "-1";
                returnMessage = "Error in retrieving Contact History records!";
            }
            finally
            {
                //Create Table for ReturnCode and Return Message
                dtReturn = CommonUtilities.CreateReturnTable(returnCode, returnMessage);
                ds.Tables.Add(dtReturn);

                try
                {
                    genericDA.CloseConnection();
                }
                catch (Exception e)
                { }
            }

            return ds;
        }
        /**************End by TSM**************/

        public DataSet RetrieveContactAnalysisByProjectName(string dealerCode, string accountNo, string ProjID)
        {
            DataSet ds = new DataSet();
            DataTable dtReturn = null;
            string returnCode = "1", returnMessage = "";
            string projtype = RetrieveProjectType(ProjID);
            try
            {
                genericDA.OpenConnection();
                if (projtype == "C")
                {
                    ds.Tables.Add(clientContactDA.RetrieveContactAnalysisByProjectName(dealerCode, accountNo, ProjID));
                }
                else if (projtype == "L")
                {
                    ds.Tables.Add(clientContactDA.RetrieveContactAnalysisByLeadProjectName(dealerCode, accountNo, ProjID));
                }



               // ds.Tables.Add(clientContactDA.RetrieveContactAnalysisByProjectName(dealerCode, accountNo, ProjID));
                if (ds.Tables[0].Rows.Count < 1)
                {
                    returnCode = "0";
                    returnMessage = "Contact Analysis not Found!";
                }
            }
            catch (Exception e)
            {
                returnCode = "-1";
                returnMessage = "Error in retrieving Client Analysis records! <br /> Error Message : " + e.Message;
            }
            finally
            {
                //Create Table for ReturnCode and Return Message
                dtReturn = CommonUtilities.CreateReturnTable(returnCode, returnMessage);
                ds.Tables.Add(dtReturn);

                try
                {
                    genericDA.CloseConnection();
                }
                catch (Exception e)
                { }
            }

            return ds;
        }
        /**************End by TSM**************/

        public DataSet RetrieveCommissionEarnedByClientAcctNo(String acctNo)
        {
            DataSet ds = new DataSet();
            DataTable dtReturn = null;
            String returnCode = "1";
            String returnMessage = "";
            try
            {
                genericDA.OpenConnection();
                ds = clientContactDA.RetrieveCommissionEarnedByClientAcctNo(acctNo);
                if (ds.Tables[0].Rows.Count < 1)
                {
                    returnCode = "0";
                    returnMessage = "No Commission Earned for this Client";
                }
            }
            catch (Exception e)
            {
                returnCode = "-1";
                returnMessage = "Error in retrieving Commission Earned for the client<br />Error Message: " + e.Message;
            }
            finally
            {
                dtReturn = CommonUtilities.CreateReturnTable(returnCode, returnMessage);
                ds.Tables.Add(dtReturn);
                try
                {
                    genericDA.CloseConnection();
                }
                catch (Exception e) { }
            }
            return ds;
        }


        internal DataSet RetrieveCashAndEquivalentByUserAcctNo(string acctNo)
        {
            DataSet ds = new DataSet();
            DataTable dtReturn = null;
            String returnCode = "1";
            String returnMessage = "";
            try
            {
                genericDA.OpenConnection();
                ds = clientContactDA.RetrieveCashAndEquivalentByUserAcctNo(acctNo);
                if (ds.Tables[0].Rows.Count < 1)
                {
                    returnCode = "0";
                    returnMessage = "No Records founds for the user";
                }
            }
            catch (Exception e)
            {
                returnCode = "-1";
                returnMessage = "Error in retrieving Cash and Equivalent for the client<br />Error Message: " + e.Message;
            }
            finally
            {
                dtReturn = CommonUtilities.CreateReturnTable(returnCode, returnMessage);
                ds.Tables.Add(dtReturn);
                try
                {
                    genericDA.CloseConnection();
                }
                catch (Exception e) { }
            }
            return ds;
        }

        internal DataSet RetrieveAvailableFundsByUserAcctNo(string acctNo)
        {
            DataSet ds = new DataSet();
            DataTable dtReturn = null;
            String returnCode = "1";
            String returnMessage = "";
            try
            {
                genericDA.OpenConnection();
                ds = clientContactDA.RetrieveAvailableFundsByUserAcctNo(acctNo);
                if (ds.Tables[0].Rows.Count < 1)
                {
                    returnCode = "0";
                    returnMessage = "No Records founds for the user";
                }
            }
            catch (Exception e)
            {
                returnCode = "-1";
                returnMessage = "Error in retrieving Available Funds for the client<br />Error Message: " + e.Message;
            }
            finally
            {
                dtReturn = CommonUtilities.CreateReturnTable(returnCode, returnMessage);
                ds.Tables.Add(dtReturn);
                try
                {
                    genericDA.CloseConnection();
                }
                catch (Exception e) { }
            }
            return ds;
        }

        internal DataSet RetrieveClientServiceTypeByClientAcctNo(string acctNo)
        {
            DataSet ds = new DataSet();
            DataTable dtReturn = null;
            String returnCode = "1";
            String returnMessage = "";
            try
            {
                genericDA.OpenConnection();
                ds = clientContactDA.RetrieveClientServiceTypeByClientAcctNo(acctNo);
                if (ds.Tables[0].Rows.Count < 1)
                {
                    returnCode = "0";
                    returnMessage = "No Records founds for the user";
                }
            }
            catch (Exception e)
            {
                returnCode = "-1";
                returnMessage = "Error in retrieving Client Account by Account Type<br />Error Message: " + e.Message;
            }
            finally
            {
                dtReturn = CommonUtilities.CreateReturnTable(returnCode, returnMessage);
                ds.Tables.Add(dtReturn);
                try
                {
                    genericDA.CloseConnection();
                }
                catch (Exception e) { }
            }
            return ds;
        }

        internal DataSet PrepareForContactToFollowUp(string userId, String UserRole)
        {
            DataSet ds = new DataSet();
            DataTable dtReturn = null;
            string returnCode = "1", returnMessage = "";
            try
            {
                genericDA.OpenConnection();
                ds = clientContactDA.PrepareForContactToFollowUp(userId, UserRole);
                if (ds.Tables[0].Rows.Count < 1)
                {
                    returnCode = "0";
                    returnMessage = "No Contact to Follow-up Found!";
                }
            }
            catch (Exception e)
            {
                returnCode = "-1";
                returnMessage = "Error in retrieving Contact to Follow-up!";
            }
            finally
            {
                dtReturn = CommonUtilities.CreateReturnTable(returnCode, returnMessage);
                ds.Tables.Add(dtReturn);
                try
                {
                    genericDA.CloseConnection();
                }
                catch (Exception e)
                { }
            }
            return ds;
        }
    }
}