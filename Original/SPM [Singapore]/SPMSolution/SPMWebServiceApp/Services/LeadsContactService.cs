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
    public class LeadsContactService
    {
        private GenericDA genericDA;
        private LeadsContactDA leadsContactDA;


        public LeadsContactService()
        {
            genericDA = new GenericDA();
            leadsContactDA = new LeadsContactDA(genericDA);
        }

        public LeadsContactService(string dbConnectionStr) : this()
        {
            genericDA.SetConnectionString(dbConnectionStr);
        }

        public DataSet PrepareForContactEntry(string userRole, string userId)
        {
            DataSet ds = new DataSet();
            DataTable dtReturn = null;
            DealerDA dealerDA = new DealerDA(genericDA);
            CommonServiceDA commonServiceDA = new CommonServiceDA(genericDA);

            /* Total Tables in DataSet              
             * Index 0 : Dealer       (Dealer)
             * Index 1 : Team
             * Index 2 : Uncontacted Assignment     (dtUnContactedAssignment)
             * Index 3 : Contact Entry for Today    (dtHistoryEntry)            
             * Index 4 : FollowUp Assignment
             * Index 5 : Return DataTable which contains return code and return message     (ReturnTable).
             */


            try
            {
                dtReturn = CommonUtilities.CreateReturnTable("1", "");

                genericDA.OpenConnection();

                if (userRole == "admin")
                {
                    //Retrieve all Dealer for Contact Entry Admin
                    ds.Tables.Add(commonServiceDA.RetrieveAllDealer(userId));
                }
                else
                {
                    //Retrieve Dealer code
                    ds.Tables.Add(dealerDA.RetrieveDealerByUserId(userId));
                }


                 ds.Tables.Add(commonServiceDA.dtRetrieveDealerCodeAndNameByUserID(userId));
                
                //Retrieve Preference List
                //ds.Tables.Add(commonServiceDA.RetrievePreferenceCodeAndName());

                //if (userRole == "admin")
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

                        //ds.Tables.Add(leadsContactDA.RetrieveUnContactedAssignment(ds.Tables[0].Rows[0]["AECode"].ToString()));
                        ds.Tables.Add(leadsContactDA.RetrieveUnContactedAssignment(AECodes, userId));

                        if (ds.Tables[0].Rows.Count < 1)
                        {
                            dtReturn.Rows[0]["ReturnMessage"] = "No Uncontacted Assignments exist for current dealer.";
                        }

                        //Retrieve Contact Entry for Today
                        /// <Updated by Thet Maung Chaw>
                        //leadsContactDA.RetrieveContactEntryForToday(ds, ds.Tables[0].Rows[0]["AECode"].ToString());
                        leadsContactDA.RetrieveContactEntryForToday(ds, AECodes, userId);

                        //Retrieve FollowUp Assignment
                        /// <Updated by Thet Maung Chaw>
                        //ds.Tables.Add(leadsContactDA.RetrieveFollowUpLeads(ds.Tables[0].Rows[0]["AECode"].ToString()));
                        ds.Tables.Add(leadsContactDA.RetrieveFollowUpLeads(AECodes));
                        
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

        //For Contact Entry Admin
        public DataSet RetrieveUnContactedAssignment(string dealerCode, String UserID)
        {
            DataSet ds = new DataSet();
            DataTable dtReturn = null;
            string returnCode = "1", returnMessage = "";

            try
            {
                genericDA.OpenConnection();

                ds.Tables.Add(leadsContactDA.RetrieveUnContactedAssignment(dealerCode, UserID));
                if (ds.Tables[0].Rows.Count < 1)
                {
                    //returnCode = "0";
                    returnMessage = "No Uncontacted Assignment Found!";
                }

                //To retrieve Entry History for Contact Entry Admin
                leadsContactDA.RetrieveContactEntryForToday(ds, dealerCode, UserID);
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


        public DataSet getContactHistoryByLeadId(string leadId)
        {
            DataSet ds = new DataSet();
            DataTable dtReturn = null;
            string returnCode = "1", returnMessage = "";

            try
            {
                genericDA.OpenConnection();

                ds.Tables.Add(leadsContactDA.getContactHistoryByLeadId(leadId));
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
                            string preference, string content, string teamCode)
        {
            DataSet ds = new DataSet();
            DataTable dtReturn = null;
            string returnCode = "1", returnMessage = "";

            try
            {
                genericDA.OpenConnection();

                ds.Tables.Add(leadsContactDA.RetrieveContactHistoryByCriteria(accountNo, dealerCode, dateFrom, dateTo, rank, preference, content, teamCode));
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

                ds.Tables.Add(leadsContactDA.RetrieveContactAnalysis(dealerCode, accountNo, dateFrom, dateTo));
                if (ds.Tables[0].Rows.Count < 1)
                {
                    returnCode = "0";
                    returnMessage = "Contact Analysis not Found!";
                }
            }
            catch (Exception e)
            {
                returnCode = "-1";
                returnMessage = "Error in retrieving Leads Analysis records! <br /> Error Message : " + e.Message;
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

        public DataSet RetrieveCallReport(string assignDate)
        {
            DataSet ds = new DataSet();
            DataTable dtReturn = null;
            string returnCode = "1", returnMessage = "";

            try
            {
                genericDA.OpenConnection();

                ds.Tables.Add(leadsContactDA.RetrieveCallReport(assignDate));
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

                ds.Tables.Add(leadsContactDA.RetrieveCallReportDetail(assignDate, dealerCode));
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

                ds.Tables.Add(leadsContactDA.RetrieveClientAnalysis(teamCode, dealerCode, accountCreateDate, lastTradeDate));
                if (ds.Tables[0].Rows.Count < 1)
                {
                    returnCode = "0";
                    returnMessage = "Leads Analysis not found! Please change your criteria.";
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

        public DataSet InsertLeadsContact(string dealerCode, string userId, string leadId,string sex,string MobileNo, string HomeNo, string content,
                string needFollowUp, string followupDate, string preferMode, string projId, string lastLeadId, string lastLeadName)
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

                result = leadsContactDA.InsertLeadsContact(dealerCode, userId, leadId, sex, MobileNo, HomeNo, content,  needFollowUp, followupDate, preferMode, projId, "", "new");

                if (result < 1)
                {
                    returnMessage = "Error in saving Contact Record!";
                }
                else
                {
                    insertedId = result;
                    result = 1;     //Return as successful code
                    
                    if (!lastLeadId.Equals(leadId))
                    {
                        //New Inserted
                        dtContactHistory = leadsContactDA.getContactHistoryByLeadId(leadId);
                    }
                    else
                    {
                        DataTable dtDealer = null, dtLeads = null, dtAssignment = null;
                        LeadsAssignmentDA leadsAssignmentDA = new LeadsAssignmentDA(genericDA);

                        //Old way to retrieve LeadsName, TeamCode for adding Contact Entry for existing Lead Id
                        if (String.IsNullOrEmpty(lastLeadName))
                        {
                            dtLeads = leadsContactDA.GetLeadsByLeadId(leadId);
                            if (dtLeads.Rows.Count > 0)
                            {
                                lastLeadName = dtLeads.Rows[0]["LeadName"].ToString();
                            }
                        }

                        //Retrieve Dealer Team Code for new added Account No
                        dtDealer = dealerDA.RetrieveDealerByDealerCode(dealerCode);
                        if (dtDealer.Rows.Count > 0)
                        {
                            teamCode = dtDealer.Rows[0]["AEGroup"].ToString();
                        }

                        ////Check for Extra Call
                        //dtAssignment = clientAssignmentDA.RetrieveAssignmentForExtraCall(acctNo, dealerCode, DateTime.Now.ToString("yyyy-MM-dd"));
                        //if (dtAssignment.Rows.Count > 0)
                        //{
                        //    assignDate = dtAssignment.Rows[0]["AssignDate"].ToString();
                        //}

                        dtContactHistory = new DataTable("dtContactHistory");
                        dtContactHistory.Columns.Add("RecId", String.Empty.GetType());
                        dtContactHistory.Columns.Add("ContactDate", String.Empty.GetType());
                        dtContactHistory.Columns.Add("AEGroup", String.Empty.GetType());
                        dtContactHistory.Columns.Add("leadId", String.Empty.GetType());
                        dtContactHistory.Columns.Add("LeadName", String.Empty.GetType());
                        dtContactHistory.Columns.Add("Sex", String.Empty.GetType());
                        dtContactHistory.Columns.Add("MobileNo", String.Empty.GetType());
                        dtContactHistory.Columns.Add("HomeNo", String.Empty.GetType());
                        dtContactHistory.Columns.Add("Content", String.Empty.GetType());
                        //dtContactHistory.Columns.Add("Remarks", String.Empty.GetType());
                        //dtContactHistory.Columns.Add("PreferA", String.Empty.GetType());
                        //dtContactHistory.Columns.Add("PreferB", String.Empty.GetType());
                        //dtContactHistory.Columns.Add("Rank", String.Empty.GetType());
                        dtContactHistory.Columns.Add("ModifiedUser", String.Empty.GetType());
                        dtContactHistory.Columns.Add("AssignDate", String.Empty.GetType());

                        /// <Updated by OC>
                        dtContactHistory.Columns.Add("AECode", typeof(String));
                        dtContactHistory.Columns.Add("NeedFollowUp", typeof(String));
                        dtContactHistory.Columns.Add("FollowUpDate", typeof(String));
                        dtContactHistory.Columns.Add("PreferMode", typeof(String));

                        //dtContactHistory.Columns.Add("RankText", String.Empty.GetType());

                        drNewContact = dtContactHistory.NewRow();
                        drNewContact["RecId"] = insertedId.ToString();
                        drNewContact["ContactDate"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        drNewContact["AEGroup"] = teamCode;
                        drNewContact["LeadId"] = leadId;
                        drNewContact["LeadName"] = lastLeadName;
                        drNewContact["Sex"] = sex;
                        drNewContact["MobileNo"] = MobileNo;
                        drNewContact["HomeNo"] = HomeNo;
                        drNewContact["Content"] = content;
                        //drNewContact["Remarks"] = remark;
                        //drNewContact["PreferA"] = preferA;
                        //drNewContact["PreferB"] = preferB;
                        //drNewContact["Rank"] = rank;
                        drNewContact["ModifiedUser"] = dealerCode;
                        drNewContact["AssignDate"] = assignDate;
                       // drNewContact["RankText"] = CommonUtilities.GetClientRank(int.Parse(rank));

                        /// <Updated by OC>
                        drNewContact["Content"] = content;
                        drNewContact["PreferMode"] = preferMode;
                        drNewContact["NeedFollowUp"] = needFollowUp;
                        drNewContact["FollowUpDate"] = followupDate;

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

        //dealerCode, userId, leadId, sex, MobileNo, HomeNo, content, needFollowUp, preferMode, projId, recId
        public int UpdateLeadsContactFollowUp(string dealerCode, string userId, string leadId, string sex, string MobileNo, string HomeNo, string content,
                string needFollowUp, string preferMode, string projId, string lastLeadId, string lastLeadName,string recId)
        {
            int result = 1; //updatedId = 0;
            //string returnMessage = "Contact Record is updated successfully.", teamCode = "", assignDate = "";
            DataSet ds = new DataSet();
           // DataTable dtContactHistory = null, dtReturn = null;
            //DataRow drNewContact = null;
            //DealerDA dealerDA = new DealerDA(genericDA);

            try
            {
                genericDA.OpenConnection();
                genericDA.CreateSqlCommand();

                result = leadsContactDA.UpdateLeadsContactFollowUp(dealerCode, userId, leadId, sex, MobileNo, HomeNo, content, needFollowUp, preferMode, projId, recId);

                if (result < 1)
                {
                    result = -1;
                }
                else
                {
                    //updatedId = result;
                    result = 1;     //Return as successful code
                }
            }
            catch (Exception ex)
            { 
                
            }
            return result;
        }

        public string[] UpdateLeadsContact(string dealerCode, string userId, string leadId,  string sex, string MobileNo,string HomeNo, string content,
            string needFollowup, string followupDate, string preferMode, string projId, string recId)    
            //string preferA, string preferB, string remark, string rank, string keep, string adminId, string recId)
        {            
            int result = -1;
            string returnMessage = "Leads Contact record is updated successfully.";

            try
            {
                genericDA.OpenConnection();
                genericDA.CreateSqlCommand();

                result = leadsContactDA.InsertLeadsContact(dealerCode, userId, leadId, sex, MobileNo, HomeNo, content,  needFollowup, followupDate, preferMode, projId, recId, "update");
                if (result < 1)
                {
                    returnMessage = "Error in updating Leads Contact record!";
                }
            }
            catch (Exception e)
            {
                result = -1;
                returnMessage = "Error in updating Leads Contact record! Please try again. Exception Message: " + e.Message;
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

        public DataSet DeleteLeadsContact(string recId, string dealerCode, String UserID)
        {
            int result = -1;
            string returnMessage = "Leads Contact record is deleted successfully.";
            DataSet ds = new DataSet();
            DataTable dtReturn = null;

            try
            {
                genericDA.OpenConnection();
                genericDA.CreateSqlCommand();

                result = leadsContactDA.DeleteLeadsContact(recId);
                if (result < 1)
                {
                    returnMessage = "Error in deleting Leads Contact record! Leads Contact has been deleted by other user!";
                }
                else
                {
                    ds.Tables.Add(leadsContactDA.RetrieveUnContactedAssignment(dealerCode, UserID));
                }
            }
            catch (Exception e)
            {
                result = -1;
                returnMessage = "Error in deleting Leads Contact record! Please try again. Exception Message: " + e.Message;
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

                leadsContactDA.RetrieveContactEntryForToday(ds, dealerCode, UserID);
                if (ds.Tables[0].Rows.Count < 1)
                {
                    returnCode = "0";
                    returnMessage = "Leads Contact not found!";
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

        /// <summary>
        /// Added by Thet Maung Chaw
        /// To retrieve follow up records for Lead Contact Page
        /// </summary>
        /// <param name="AECode"></param>
        /// <returns></returns>
        public DataTable RetrieveFollowUpLead(String AECode)
        {
            genericDA.OpenConnection();

            DataTable dt = leadsContactDA.RetrieveFollowUpLead(AECode);

            dt.TableName = "dtFollowUp";

            genericDA.CloseConnection();

            return dt;
        }
    }
}
