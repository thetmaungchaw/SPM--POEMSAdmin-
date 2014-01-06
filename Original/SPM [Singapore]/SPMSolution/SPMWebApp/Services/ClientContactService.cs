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
    public class ClientContactService
    {
        private ContactManagementService.ContactManagement contactManagement;
        private string dbConnectionStr;
        private DataTable dtReturn;

        public ClientContactService()
        {
            contactManagement = new SPMWebApp.ContactManagementService.ContactManagement();            
        }

        public ClientContactService(string cnStr) : this()
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

        public DataSet PrepareForContactHistoryByUserOrSupervisor(String Param, String UserID)
        {
            DataSet ds = null;

            try
            {
                ds = contactManagement.PrepareForContactHistoryByUserOrSupervisor(Param, UserID, dbConnectionStr);
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

        public DataSet RetrieveUnContactedAssignmentByProjectID(string dealerCode, String UserID)
        {
            DataSet ds = null;

            try
            {
                ds = contactManagement.RetrieveUnContactedAssignmentByProjectID(dealerCode, UserID, dbConnectionStr);
            }
            catch (Exception e)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                ds = new DataSet();
                ds.Tables.Add(dtReturn);
            }

            return ds;
        }

        public DataSet RetrieveClientInfoByShortKey(string shortKey)
        {
            DataSet ds = null;

            try
            {
                ds = contactManagement.RetrieveClientInfoByShortKey(shortKey, dbConnectionStr);
            }
            catch (Exception e)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                ds = new DataSet();
                ds.Tables.Add(dtReturn);
            }

            return ds;
        }

        public DataSet GetDetailUserInformation(string accountNo)
        {
            DataSet ds = null;

            try
            {
                ds = contactManagement.GetDetailUserInformation(accountNo, dbConnectionStr);
            }
            catch (Exception e)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                ds = new DataSet();
                ds.Tables.Add(dtReturn);
            }

            return ds;
        }

        public DataSet GetSeminorRegistrationByAccNo(string accountNo)
        {
            DataSet ds = null;

            try
            {
                ds = contactManagement.GetSeminorRegistrationByAccNo(accountNo, dbConnectionStr);
            }
            catch (Exception e)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                ds = new DataSet();
                ds.Tables.Add(dtReturn);
            }

            return ds;
        }

        public DataSet getContactHistoryByAccountNo(string accountNo)
        {
            DataSet ds = null;

            try
            {
                ds = contactManagement.getContactHistoryByAccountNo(accountNo, dbConnectionStr);
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
                            string preference, string content, string teamCode, String UserID)
        {
            DataSet ds = new DataSet();

            try
            {
                ds = contactManagement.RetrieveContactHistoryByCriteria(accountNo, dealerCode, dateFrom, dateTo, rank, preference,
                            content, teamCode, UserID, dbConnectionStr);
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

        public DataSet RetrieveCallReport(string assignDate, String UserID)
        {
            DataSet ds = new DataSet();

            try
            {
                ds = contactManagement.RetrieveCallReport(assignDate, UserID, dbConnectionStr);
            }
            catch (Exception e)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                ds.Tables.Add(dtReturn);
            }

            return ds;
        }

        /// <Added by OC>
        public DataSet RetrieveCallReportByUserOrSupervisor(String assignDate, String Param, String UserID)
        {
            DataSet ds = new DataSet();

            try
            {
                ds = contactManagement.RetrieveCallReportByUserOrSupervisor(assignDate, Param, UserID, dbConnectionStr);
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
        public DataSet RetrieveCallReportProjectDetail(string ProjectName, string dealerCode)
        {
            DataSet ds = null;

            try
            {
                ds = contactManagement.RetrieveCallReportProjectDetail(ProjectName, dealerCode, dbConnectionStr);
            }
            catch (Exception e)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                ds = new DataSet();
                ds.Tables.Add(dtReturn);
            }

            return ds;
        }

        public DataSet RetrieveClientAnalysis(string teamCode, string dealerCode, string accountCreateDate, string lastTradeDate)
        {
            DataSet ds = null;

            try
            {
                ds = contactManagement.RetrieveClientAnalysis(teamCode, dealerCode, accountCreateDate, lastTradeDate, dbConnectionStr);
            }
            catch (Exception e)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                ds = new DataSet();
                ds.Tables.Add(dtReturn);
            }

            return ds;
        }

        public DataSet PrepareForClientAnalysis(String UserID)
        {
            DataSet ds = null;

            try
            {
                ds = contactManagement.PrepareForClientAnalysis(UserID, dbConnectionStr);
            }
            catch (Exception e)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                ds = new DataSet();
                ds.Tables.Add(dtReturn);
            }

            return ds;
        }

        /// <Added by OC>
        public DataSet PrepareForClientAnalysisByUserOrSupervisor(String Param, String UserID)
        {
            DataSet ds = null;

            try
            {
                ds = contactManagement.PrepareForClientAnalysisByUserOrSupervisor(Param, UserID, dbConnectionStr);
            }
            catch (Exception e)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                ds = new DataSet();
                ds.Tables.Add(dtReturn);
            }

            return ds;
        }

        //If followUpStatus = 1 THEN Y
        //If followUpStatus = 0 THEN N
        //If followUpStatus = 2 THEN F
        public DataSet InsertClientContact(string dealerCode, string userId, string acctNo, string shortKey, string sex, string contactPhone, string content,
                string preferA, string preferB, string remark, string rank, string keep, string adminId, string lastAcctNo, string lastClientName,int followUpStatus,string followUpDealer,string followUpDate,string ProjectID)
        {            
            DataSet ds = null;

            try
            {
                ds = contactManagement.InsertClientContact(dealerCode, userId, acctNo, shortKey, sex, contactPhone, content, preferA, preferB,
                                            remark, rank, keep, adminId, lastAcctNo, lastClientName, followUpStatus, followUpDealer, followUpDate, ProjectID, this.dbConnectionStr);
            }
            catch (Exception e)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                ds = new DataSet();
                ds.Tables.Add(dtReturn);
            }

            return ds;
        }

        public string[] UpdateClientContact(string dealerCode, string userId, string acctNo, string shortKey, string sex, string contactPhone, string content,
                string preferA, string preferB, string remark, string rank, string keep, string adminId, string recId,int followup,string followUpdealer,string followUpDate,string projectID)
        {
            string[] wsReturn = null;

            try
            {
                wsReturn = contactManagement.UpdateClientContact(dealerCode, userId, acctNo, shortKey, sex, contactPhone, content, preferA, preferB,
                                            remark, rank, keep, adminId, recId, followup, followUpdealer, followUpDate, projectID, this.dbConnectionStr);
            }
            catch (Exception e)
            {
                wsReturn = new string[] { "-3", "Error in WebService Connection!" };
            }

            return wsReturn;
        }

        public String[] UpdateClientContactFollowUpStatus(String recId, String followUpStatus)
        {
            String[] wsReturn = null;
            try
            {
                wsReturn = contactManagement.UpdateClientContactFollowUpStatus(this.dbConnectionStr, recId, followUpStatus);
            }
            catch (Exception e)
            {
                wsReturn = new String[] { "-3", "Error in WebService Connection!" };
            }
            return wsReturn;
        }

        public DataSet DeleteClientContact(string recId, string dealerCode, String UserID)
        {
            DataSet ds = null;

            try
            {
                ds = contactManagement.DeleteClientContact(recId, dealerCode, UserID, this.dbConnectionStr);
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

        /**************Update by TSM**************/
        public DataSet RetrieveCallReportByProjectName(string ProjName, String UserID)
        {
            DataSet ds = new DataSet();

            try
            {
                ds = contactManagement.RetrieveCallReportByProjectName(ProjName, UserID, dbConnectionStr);
            }
            catch (Exception e)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                ds.Tables.Add(dtReturn);
            }
            return ds;
        }

        /**************End TSM**************/

        /// <Added by OC>
        public DataSet RetrieveCallReportByProjectNameByUserOrSupervisor(String ProjName, String Param)
        {
            DataSet ds = new DataSet();

            try
            {
                ds = contactManagement.RetrieveCallReportByProjectNameByUserOrSupervisor(ProjName, Param, dbConnectionStr);
            }
            catch (Exception e)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                ds.Tables.Add(dtReturn);
            }
            return ds;
        }

        /**************Update by TSM**************/
        public DataSet RetrieveContactHistoryByProjName(string accountNo, string dealerCode, string ProjName, string rank,
                          string preference, string content, string teamCode, String UserID)
        {
            DataSet ds = new DataSet();

            try
            {
               
                ds = contactManagement.RetrieveContactHistoryByProjName(accountNo, dealerCode, ProjName, rank, preference,
                            content, teamCode, UserID, dbConnectionStr);
            }
            catch (Exception e)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                ds.Tables.Add(dtReturn);
            }

            return ds;
        }
        /**************End by TSM**************/


        /**************Update by TSM**************/
        public DataSet RetrieveContactAnalysisByProjectName(string teamCode, string dealerCode, string ProjID)
        {
            DataSet ds = null;

            try
            {
                ds = contactManagement.RetrieveContactAnalysisByProjectName(teamCode, dealerCode, ProjID, dbConnectionStr);
            }
            catch (Exception e)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                ds = new DataSet();
                ds.Tables.Add(dtReturn);
            }

            return ds;
        }
        /**************End by TSM**************/

        public DataSet RetrieveCommissionEarnedByClientAcctNo(String acctNo)
        {
            DataSet ds = null;
            try
            {
                ds = contactManagement.RetrieveCommissionEarnedByClientAcctNo(dbConnectionStr,acctNo);
            }
            catch (Exception e)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                ds = new DataSet();
                ds.Tables.Add(dtReturn);
            }
            return ds;
        }

        internal DataSet RetrieveCashAndEquivalentsByUserAcctNo(string userAcctNo)
        {
            DataSet ds = null;
            try
            {
                ds = contactManagement.RetrieveCashAndEquivalentByUserAcctNo(dbConnectionStr, userAcctNo);
            }
            catch (Exception e)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                ds = new DataSet();
                ds.Tables.Add(dtReturn);
            }
            return ds;
        }

        internal DataSet RetrieveAvailableFundsByUserAcctNo(string userAcctNo)
        {
            DataSet ds = null;
            try
            {
                ds = contactManagement.RetrieveAvailableFundsByUserAcctNo(dbConnectionStr, userAcctNo);
            }
            catch (Exception e)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                ds = new DataSet();
                ds.Tables.Add(dtReturn);
            }
            return ds;
        }

        internal DataSet RetrieveClientServiceTypeByClientAcct(string nric)
        {
            DataSet ds = null;
            try
            {
                ds = contactManagement.RetrieveClientServiceTypeByClientAcctNo(dbConnectionStr, nric);
            }
            catch (Exception e)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                ds = new DataSet();
                ds.Tables.Add(dtReturn);
            }
            return ds;
        }

        internal DataSet PrepareForContactToFollowUp(string userId, String UserRole)
        {
            DataSet ds = null;
            try
            {
                ds = contactManagement.PrepareForContactToFollowUp(userId, UserRole, dbConnectionStr);
            }
            catch (Exception e)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                ds = new DataSet();
                ds.Tables.Add(dtReturn);
            }
            return ds;
        }

        /// <OC>
        public DataSet RetrieveByAccountNo(String AccountNo, String Param)
        {
            DataSet ds = null;

            try
            {
                ds = contactManagement.RetrieveByAccountNo(AccountNo, Param, dbConnectionStr);
            }
            catch (Exception e)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                ds = new DataSet();
                ds.Tables.Add(dtReturn);
            }

            return ds;
        }
    }
}