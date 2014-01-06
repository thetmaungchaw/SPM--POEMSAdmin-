﻿using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;

using SPMWebServiceApp.Services;

namespace SPMWebServiceApp.WebServices
{
    /// <summary>
    /// Summary description for LeadsContactManagement
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    public class LeadsContactManagement : System.Web.Services.WebService
    {

        
        [WebMethod]
        public DataSet PrepareForContactEntry(string userRole, string userId, string dbConnectionStr)
        {
            LeadsContactService leadsContactService = new LeadsContactService(dbConnectionStr);
            return leadsContactService.PrepareForContactEntry(userRole, userId);
        }

        [WebMethod]
        public DataSet RetrieveUnContactedAssignment(string dealerCode, String UserID, string dbConnectionStr)
        {
            LeadsContactService leadsContactService = new LeadsContactService(dbConnectionStr);
            return leadsContactService.RetrieveUnContactedAssignment(dealerCode, UserID);
        }

        //[WebMethod]
        //public DataSet RetrieveLeadsInfoByShortKey(string shortKey, string dbConnectionStr)
        //{
        //    LeadsContactService leadsContactService = new LeadsContactService(dbConnectionStr);
        //    return leadsContactService.RetrieveClientInfoByShortKey(shortKey);
        //}

        [WebMethod]
        public DataSet getContactHistoryByLeadId(string leadId, string dbConnectionStr)
        {
            LeadsContactService leadsContactService = new LeadsContactService(dbConnectionStr);
            return leadsContactService.getContactHistoryByLeadId(leadId);
        }

        [WebMethod]
        public DataSet RetrieveContactHistoryByCriteria(string accountNo, string dealerCode, string dateFrom, string dateTo, string rank,
                            string preference, string content, string teamCode, string dbConnectionStr)
        {
            LeadsContactService leadsContactService = new LeadsContactService(dbConnectionStr);
            return leadsContactService.RetrieveContactHistoryByCriteria(accountNo, dealerCode, dateFrom, dateTo, rank, preference, content, teamCode);
        }

        [WebMethod]       
        public DataSet InsertLeadsContact(string dealerCode, string userId, string leadId,  string sex, string MobileNo,string HomeNo, string content,
                string needFollowUp, string followupDate, string preferMode, string projId, string lastLeadId, string lastLeadName,
                string dbConnectionStr)
        {
            LeadsContactService leadsContactService = new LeadsContactService(dbConnectionStr);
            return leadsContactService.InsertLeadsContact(dealerCode, userId, leadId , sex, MobileNo,HomeNo, content,needFollowUp,followupDate,preferMode,projId,lastLeadId,lastLeadName);
                                         
        }

        [WebMethod]
        public int UpdateLeadsContactFollowup(string dealerCode, string userId, string leadId, string sex, string MobileNo, string HomeNo, string content,
                string needFollowUp, string preferMode, string projId, string lastLeadId, string lastLeadName, string recId,
                string dbConnectionStr)
        {
            LeadsContactService leadsContactService = new LeadsContactService(dbConnectionStr);
            return leadsContactService.UpdateLeadsContactFollowUp(dealerCode, userId, leadId, sex, MobileNo, HomeNo, content, needFollowUp, preferMode, projId,lastLeadId,lastLeadName,recId);

        }

        [WebMethod]
        public string[] UpdateLeadsContact(string dealerCode, string userId, string leadId,  string sex, string MobileNo,string HomeNo, string content,
            string needFollowup, string followupDate, string preferMode, string projId, string recId, string dbConnectionStr)        
        {
            LeadsContactService leadsContactService = new LeadsContactService(dbConnectionStr);
            return leadsContactService.UpdateLeadsContact(dealerCode, userId, leadId, sex, MobileNo, HomeNo, content,  needFollowup, followupDate, preferMode, projId, recId);
        }

        [WebMethod]
        public DataSet DeleteLeadsContact(string recId, string dealerCode, String UserID, string dbConnectionStr)
        {
            LeadsContactService leadsContactService = new LeadsContactService(dbConnectionStr);
            return leadsContactService.DeleteLeadsContact(recId, dealerCode, UserID);
        }

        [WebMethod]
        public DataSet PrepareForContactHistory(String UserID, string dbConnectionStr)
        {
            LeadsContactService leadsContactService = new LeadsContactService(dbConnectionStr);
            return leadsContactService.PrepareForContactHistory(UserID);
        }

        [WebMethod]
        public DataSet RetrieveContactAnalysis(string dealerCode, string accountNo, string dateFrom, string dateTo, string dbConnectionStr)
        {
            LeadsContactService leadsContactService = new LeadsContactService(dbConnectionStr);
            return leadsContactService.RetrieveContactAnalysis(dealerCode, accountNo, dateFrom, dateTo);
        }

        [WebMethod]
        public DataSet RetrieveCallReport(string assignDate, string dbConnectionStr)
        {
            LeadsContactService leadsContactService = new LeadsContactService(dbConnectionStr);
            return leadsContactService.RetrieveCallReport(assignDate);
        }

        [WebMethod]
        public DataSet RetrieveCallReportDetail(string assignDate, string dealerCode, string dbConnectionStr)
        {
            LeadsContactService leadsContactService = new LeadsContactService(dbConnectionStr);
            return leadsContactService.RetrieveCallReportDetail(assignDate, dealerCode);
        }

        [WebMethod]
        public DataSet RetrieveLeadsAnalysis(string teamCode, string dealerCode, string accountCreateDate, string lastTradeDate, string dbConnectionStr)
        {
            LeadsContactService leadsContactService = new LeadsContactService(dbConnectionStr);
            return leadsContactService.RetrieveClientAnalysis(teamCode, dealerCode, accountCreateDate, lastTradeDate);
        }

        [WebMethod]
        public DataSet PrepareForLeadsAnalysis(String UserID, string dbConnectionStr)
        {
            LeadsContactService leadsContactService = new LeadsContactService(dbConnectionStr);
            return leadsContactService.PrepareForClientAnalysis(UserID);
        }

        [WebMethod]
        public DataSet RetrieveContactEntryForToday(string dealerCode, String UserID, string dbConnectionStr)
        {
            LeadsContactService leadsContactService = new LeadsContactService(dbConnectionStr);
            return leadsContactService.RetrieveContactEntryForToday(dealerCode, UserID);
        }

        [WebMethod]
        public DataTable RetrieveFollowUpLead(String AECode, String dbConnectionStr)
        {
            LeadsContactService leadsContactService = new LeadsContactService(dbConnectionStr);
            return leadsContactService.RetrieveFollowUpLead(AECode);
        }
    }
}