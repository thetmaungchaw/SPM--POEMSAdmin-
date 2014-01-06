﻿using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using SPMWebApp.Utilities;

namespace SPMWebApp.Services
{
    public class LeadsContactService
    {
        private LeadsContactManagementService.LeadsContactManagement contactManagement;
        private string dbConnectionStr;
        private DataTable dtReturn;

        public LeadsContactService()
        {
            contactManagement = new SPMWebApp.LeadsContactManagementService.LeadsContactManagement();            
        }

        public LeadsContactService(string cnStr) : this()
        {
            this.dbConnectionStr = cnStr;
        }

        public DataSet PrepareForContactEntry(string userRole, string userId)
        {
            DataSet ds = null;

            try
            {
                ds = contactManagement.PrepareForContactEntry(userRole, userId, dbConnectionStr);
            }
            catch (Exception e)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                ds = new DataSet();
                ds.Tables.Add(dtReturn);
            }

            return ds;
        }

        public DataSet PrepareForContactHistory(String UserID)
        {
            DataSet ds = null;

            try
            {
                ds = contactManagement.PrepareForContactHistory(UserID, dbConnectionStr);
            }
            catch (Exception e)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                ds = new DataSet();
                ds.Tables.Add(dtReturn);
            }

            return ds;
        }


        //For Contact Entry Admin
        public DataSet RetrieveUnContactedAssignment(string dealerCode, String UserID)
        {
            DataSet ds = null;

            try
            {
                ds = contactManagement.RetrieveUnContactedAssignment(dealerCode, UserID, dbConnectionStr);
            }
            catch (Exception e)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                ds = new DataSet();
                ds.Tables.Add(dtReturn);
            }

            return ds;
        }

        //public DataSet RetrieveClientInfoByShortKey(string shortKey)
        //{
        //    DataSet ds = null;

        //    try
        //    {
        //        ds = contactManagement.RetrieveClientInfoByShortKey(shortKey, dbConnectionStr);
        //    }
        //    catch (Exception e)
        //    {
        //        dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
        //        ds = new DataSet();
        //        ds.Tables.Add(dtReturn);
        //    }

        //    return ds;
        //}

        public DataSet getContactHistoryByLeadId(string leadId)
        {
            DataSet ds = null;

            try
            {
                ds = contactManagement.getContactHistoryByLeadId(leadId, dbConnectionStr);
            }
            catch (Exception e)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                ds = new DataSet();
                ds.Tables.Add(dtReturn);
            }

            return ds;
        }

        public DataSet RetrieveContactHistoryByCriteria(string accountNo, string dealerCode, string dateFrom, string dateTo, string rank,
                            string preference, string content, string teamCode)
        {
            DataSet ds = new DataSet();

            try
            {
                ds = contactManagement.RetrieveContactHistoryByCriteria(accountNo, dealerCode, dateFrom, dateTo, rank, preference,
                            content, teamCode, dbConnectionStr);
            }
            catch (Exception e)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                ds.Tables.Add(dtReturn);
            }

            return ds;
        }

        public DataSet RetrieveContactAnalysis(string dealerCode, string accountNo, string dateFrom, string dateTo)
        {
            DataSet ds = null;

            try
            {
                ds = contactManagement.RetrieveContactAnalysis(dealerCode, accountNo, dateFrom, dateTo, dbConnectionStr);
            }
            catch (Exception e)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                ds = new DataSet();
                ds.Tables.Add(dtReturn);
            }

            return ds;
        }

        public DataSet RetrieveCallReport(string assignDate)
        {
            DataSet ds = new DataSet();

            try
            {
                ds = contactManagement.RetrieveCallReport(assignDate, dbConnectionStr);
            }
            catch (Exception e)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                ds.Tables.Add(dtReturn);
            }

            return ds;
        }

        public DataSet RetrieveCallReportDetail(string assignDate, string dealerCode)
        {
            DataSet ds = null;

            try
            {
                ds = contactManagement.RetrieveCallReportDetail(assignDate, dealerCode, dbConnectionStr);
            }
            catch (Exception e)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                ds = new DataSet();
                ds.Tables.Add(dtReturn);
            }

            return ds;
        }

        public DataSet RetrieveLeadsAnalysis(string teamCode, string dealerCode, string accountCreateDate, string lastTradeDate)
        {
            DataSet ds = null;

            try
            {
                ds = contactManagement.RetrieveLeadsAnalysis(teamCode, dealerCode, accountCreateDate, lastTradeDate, dbConnectionStr);
            }
            catch (Exception e)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                ds = new DataSet();
                ds.Tables.Add(dtReturn);
            }

            return ds;
        }

        public DataSet PrepareForLeadsAnalysis(String UserID)
        {
            DataSet ds = null;

            try
            {
                ds = contactManagement.PrepareForLeadsAnalysis(UserID, dbConnectionStr);
            }
            catch (Exception e)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                ds = new DataSet();
                ds.Tables.Add(dtReturn);
            }

            return ds;
        }

        public DataSet InsertLeadsContact(string dealerCode, string userId, string leadId, string sex, string MobileNo, string HomeNo, string content,
                string needFollowUp, string followupDate, string preferMode, string projId, string lastLeadId, string lastLeadName)
        {            
            DataSet ds = null;

            try
            {
                ds = contactManagement.InsertLeadsContact(dealerCode, userId, leadId, sex, MobileNo, HomeNo, content, needFollowUp, followupDate, preferMode, projId, lastLeadId, lastLeadName, this.dbConnectionStr);
                //ds = contactManagement.InsertLeadsContact(dealerCode, userId, leadId, sex, MobileNo, HomeNo, content, lEvent, needFollowUp, followupDate, preferMode, projId, lastLeadId, lastLeadName, this.dbConnectionStr);
            }
            catch (Exception e)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                ds = new DataSet();
                ds.Tables.Add(dtReturn);
            }

            return ds;
        }


        public int UpdateLeadsContactFollowup(string dealerCode, string userId, string leadId, string sex, string MobileNo, string HomeNo, string content,
        string followUpStatus, string preferMode, string projId, string lastLeadId, string lastLeadName,string recId)
        {
            //DataSet ds = null;
            int result = 1;
            try
            {
                result  = contactManagement.UpdateLeadsContactFollowup(dealerCode, userId, leadId, sex, MobileNo, HomeNo, content, followUpStatus, preferMode, projId, lastLeadId, lastLeadName,recId, this.dbConnectionStr);
                //ds = contactManagement.InsertLeadsContact(dealerCode, userId, leadId, sex, MobileNo, HomeNo, content, lEvent, needFollowUp, followupDate, preferMode, projId, lastLeadId, lastLeadName, this.dbConnectionStr);
            }
            catch (Exception e)
            {
                //dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                //ds = new DataSet();
                //ds.Tables.Add(dtReturn);
                result = -1;
            }

            return result;
        }

        public string[] UpdateLeadsContact(string dealerCode, string userId, string leadId,  string sex, string MobileNo,string HomeNo, string content,
            string needFollowup, string followupDate, string preferMode, string projId, string recId)   
        {
            string[] wsReturn = null;

            try
            {
                wsReturn = contactManagement.UpdateLeadsContact(dealerCode, userId, leadId, sex, MobileNo, HomeNo, content, needFollowup, followupDate, preferMode, projId, recId, this.dbConnectionStr);
                //wsReturn = contactManagement.UpdateLeadsContact(dealerCode, userId, leadId, sex, MobileNo, HomeNo, content, lEvent, needFollowup, followupDate, preferMode, projId, recId, this.dbConnectionStr);
            }
            catch (Exception e)
            {
                wsReturn = new string[] { "-3", "Error in WebService Connection!" };
            }

            return wsReturn;
        }

        public DataSet DeleteLeadsContact(string recId, string dealerCode, String UserID)
        {
            DataSet ds = null;

            try
            {
                ds = contactManagement.DeleteLeadsContact(recId, dealerCode, UserID, this.dbConnectionStr);
            }
            catch (Exception e)
            {
                //wsReturn = new string[] { "-3", "Error in WebService Connection!" };
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                ds = new DataSet();
                ds.Tables.Add(dtReturn);
            }

            return ds;
        }

        public DataSet RetrieveContactEntryForToday(string dealerCode, String UserID)
        {
            DataSet ds = null;

            try
            {
                ds = contactManagement.RetrieveContactEntryForToday(dealerCode, UserID, dbConnectionStr);
            }
            catch (Exception e)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                ds = new DataSet();
                ds.Tables.Add(dtReturn);
            }

            return ds;
        }

        public DataTable RetrieveFollowUpLead(String AECode)
        {
            return contactManagement.RetrieveFollowUpLead(AECode, dbConnectionStr);
        }
    }
}