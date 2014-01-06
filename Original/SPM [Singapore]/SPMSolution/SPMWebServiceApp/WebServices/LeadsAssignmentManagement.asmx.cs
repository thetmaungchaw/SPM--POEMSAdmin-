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
    /// Summary description for LeadsAssignmentManagement
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    public class LeadsAssignmentManagement : System.Web.Services.WebService
    {

                
        [WebMethod]
        public DataSet RetrieveLeadsForAssignment(string teamCode, string dbConnectionStr)//, bool _2NFlag, bool emailFlag, bool mobileFlag, string accountFromDate, string accountToDate, string dbConnectionStr)
        {            
            DataSet ds = null;
            LeadsAssignmentService leadsAssignmentService = new LeadsAssignmentService(dbConnectionStr);
            ds = leadsAssignmentService.RetrieveLeadsForAssignment(teamCode);//, _2NFlag, emailFlag, mobileFlag, accountFromDate,accountToDate);
            return ds;
        }

        [WebMethod]
        public DataSet RetrieveTeamsForLeadsAssignment(string dbConnectionStr)
        {
            DataSet ds = null;
            LeadsAssignmentService leadsAssignmentService = new LeadsAssignmentService(dbConnectionStr);

            ds = leadsAssignmentService.RetrieveTeamsForLeadsAssignment();
            return ds;
        }

        [WebMethod]
        public DataSet RetrieveDealerAndTeamForLeadsAssignment(string dbConnectionStr)
        {
            DataSet ds = null;
            LeadsAssignmentService leadsAssignmentService = new LeadsAssignmentService(dbConnectionStr);
            ds = leadsAssignmentService.RetrieveDealerAndTeamForLeadsAssignment();

            return ds;
        }

        [WebMethod]
        public DataSet RetrieveDealerByTeamCodeForLeadsAssignment(string teamCode, string dbConnectionStr)
        {
            DataSet ds = null;
            LeadsAssignmentService leadsAssignmentService = new LeadsAssignmentService(dbConnectionStr);

            ds = leadsAssignmentService.RetrieveDealerByTeamCodeForLeadsAssignment(teamCode);
            return ds;
        }

        [WebMethod]
        public string[] InsertLeadsAssignment(DataSet ds,string ProjectID, string dbConnectionStr)
        {
            LeadsAssignmentService leadsAssignmentService = new LeadsAssignmentService(dbConnectionStr);
            return leadsAssignmentService.InsertLeadsAssignment(ds,ProjectID);
        }

        [WebMethod]
        public string[] DeleteLeadsAssignment(string dealerCode, string accountNumber, string assignDate, string cutOffDate, string modifiedUser,
                    string modifiedDate, string newModifiedUser, string dbConnectionStr)
        {
            LeadsAssignmentService leadsAssignmentService = new LeadsAssignmentService(dbConnectionStr);
            return leadsAssignmentService.DeleteLeadsAssignment(dealerCode, accountNumber, assignDate, cutOffDate, modifiedUser, modifiedDate, newModifiedUser);
        }


        [WebMethod]
        public DataSet BatchDeleteLeadsAssignment(DataSet dsAssignDelete, string dbConnectionStr)
        {
            LeadsAssignmentService leadsAssignmentService = new LeadsAssignmentService(dbConnectionStr);
            return leadsAssignmentService.BatchDeleteLeadsAssignment(dsAssignDelete.Tables[0]);
        }

        [WebMethod]
        public DataSet RetrieveLeadsAssignmentHistoryByCriteria(string dealerCode, string accountNo, string nric, 
                            string fromDate, string toDate, bool retradeFlag, string dbConnectionStr)
        {
            LeadsAssignmentService leadsAssignmentService = new LeadsAssignmentService(dbConnectionStr);
            return leadsAssignmentService.RetrieveLeadsAssignmentHistoryByCriteria(dealerCode, accountNo, nric, fromDate, toDate, retradeFlag);
        }

        [WebMethod]
        public DataSet RetrieveAssignedLeadsInfo(string teamCode, string dealerCode, string accountNo, string assignDate, string dbConnectionStr)
        {
            LeadsAssignmentService leadsAssignmentService = new LeadsAssignmentService(dbConnectionStr);
            return leadsAssignmentService.RetrieveAssignedLeadsInfo(teamCode, dealerCode, accountNo, assignDate);
        }

        [WebMethod]
        public DataSet PrepareForAssignedLeadsInfo(String UserID, string dbConnectionStr)
        {
            LeadsAssignmentService leadsAssignmentService = new LeadsAssignmentService(dbConnectionStr);
            return leadsAssignmentService.PrepareForAssignedLeadsInfo(UserID);
        }

        [WebMethod]
        public DataSet PrepareForLeadsAssignmentHistory(String UserID, string dbConnectionStr)
        {
            LeadsAssignmentService leadsAssignmentService = new LeadsAssignmentService(dbConnectionStr);
            return leadsAssignmentService.PrepareForLeadsAssignmentHistory(UserID);
        }

        [WebMethod]
        public DataSet RetrieveCrossEnabledTeam(string dbConnectionStr)
        {
            LeadsAssignmentService leadsAssignmentService = new LeadsAssignmentService(dbConnectionStr);
            return leadsAssignmentService.RetrieveCrossEnabledTeam();
        }

        [WebMethod]
        public DataSet RetrieveCrossEnabledDealer(string crossTeamCode, string dbConnectionStr)
        {
            LeadsAssignmentService leadsAssignmentService = new LeadsAssignmentService(dbConnectionStr);
            return leadsAssignmentService.RetrieveCrossEnabledDealer(crossTeamCode);
        }

        [WebMethod]
        public DataSet RetrieveLeadsAssignmentDateByDateRange(string fromDate, string toDate, string dbConnectionStr)
        {
            LeadsAssignmentService leadsAssignmentService = new LeadsAssignmentService(dbConnectionStr);
            return leadsAssignmentService.RetrieveLeadsAssignmentDateByDateRange(fromDate, toDate);
        }

        [WebMethod]
        public string[] SaveProjectInformation(string ProjectName,DateTime  CutOffDate, string  dbConnectionStr)
        {
            LeadsAssignmentService leadsAssignmentService = new LeadsAssignmentService(dbConnectionStr);
            return leadsAssignmentService.SaveProjectInformation(ProjectName,CutOffDate );
        }

        [WebMethod]
        public DataSet RetrieveAllProjectInformation(string dbConnectionStr)
        {
            LeadsAssignmentService leadsAssignmentService = new LeadsAssignmentService(dbConnectionStr);
            return leadsAssignmentService.RetrieveAllProjectInformation();
        }

        [WebMethod]
        public DataSet RetrieveAssignedLeadsProjectByUserId(String dbConnectionStr, String userId)
        {
            LeadsAssignmentService leadsAssignmentService = new LeadsAssignmentService(dbConnectionStr);            
            return leadsAssignmentService.RetrieveLeadsAssignedProjectByUserId(userId);
        }

        [WebMethod]
        public DataSet RetrieveDealerEmailByDelaerCode(string dealerCode, string dbConnectionStr)
        {
            LeadsAssignmentService leadsAssignmentService = new LeadsAssignmentService(dbConnectionStr);
            return leadsAssignmentService.RetrieveDealerEmailByDelaerCode(dealerCode);
        }

        [WebMethod]
        public string[] InsertEmailLog(string FromEmail, string ToEmail, string Subject, string EmailContent, string dbConnectionStr)
        {
            LeadsAssignmentService leadsAssignmentService = new LeadsAssignmentService(dbConnectionStr);
            return leadsAssignmentService.InsertEmailLog(FromEmail, ToEmail, Subject, EmailContent);
        }
    }
}