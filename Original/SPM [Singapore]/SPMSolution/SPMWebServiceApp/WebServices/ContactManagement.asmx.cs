using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;

using SPMWebServiceApp.Services;

namespace SPMWebServiceApp.WebServices
{
    /// <summary>
    /// Summary description for ContactManagement
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    public class ContactManagement : System.Web.Services.WebService
    {
        [WebMethod]
        public DataSet PrepareForContactEntry(string userRole, string userId, string dbConnectionStr)
        {
            ClientContactService clientContactService = new ClientContactService(dbConnectionStr);
            return clientContactService.PrepareForContactEntry(userRole, userId);
        }

        /// <OC>
        [WebMethod]
        public DataSet RetrieveByAccountNo(String AccountNo, String Param, String dbConnectionStr)
        {
            ClientContactService clientContactService = new ClientContactService(dbConnectionStr);
            return clientContactService.RetrieveByAccountNo(AccountNo, Param);
        }

        [WebMethod]
        public DataSet RetrieveUnContactedAssignment(string dealerCode, String UserID, string dbConnectionStr)
        {
            ClientContactService clientContactService = new ClientContactService(dbConnectionStr);
            return clientContactService.RetrieveUnContactedAssignment(dealerCode, UserID);
        }

        [WebMethod]
        public DataSet RetrieveUnContactedAssignmentByProjectID(string dealerCode, String UserID, string dbConnectionStr)
        {
            ClientContactService clientContactService = new ClientContactService(dbConnectionStr);
            return clientContactService.RetrieveUnContactedAssignmentByProjectID(dealerCode, UserID);
        }

        [WebMethod]
        public DataSet RetrieveClientInfoByShortKey(string shortKey, string dbConnectionStr)
        {
            ClientContactService clientContactService = new ClientContactService(dbConnectionStr);
            return clientContactService.RetrieveClientInfoByShortKey(shortKey);
        }
        
        
        [WebMethod]
        public DataSet GetDetailUserInformation(string accountNo, string dbConnectionStr)
        {
            ClientContactService clientContactService = new ClientContactService(dbConnectionStr);
            return clientContactService.GetDetailUserInformation(accountNo);
        }

        [WebMethod]
        public DataSet GetSeminorRegistrationByAccNo(string accountNo, string dbConnectionStr)
        {
            ClientContactService clientContactService = new ClientContactService(dbConnectionStr);
            return clientContactService.GetSeminorRegistrationByAccNo(accountNo);
        }

        [WebMethod]
        public DataSet getContactHistoryByAccountNo(string accountNo, string dbConnectionStr)
        {
            ClientContactService clientContactService = new ClientContactService(dbConnectionStr);
            return clientContactService.getContactHistoryByAccountNo(accountNo);
        }

        [WebMethod]
        public DataSet RetrieveContactHistoryByCriteria(string accountNo, string dealerCode, string dateFrom, string dateTo, string rank,
                            string preference, string content, string teamCode, String UserID, string dbConnectionStr)
        {
            ClientContactService clientContactService = new ClientContactService(dbConnectionStr);
            return clientContactService.RetrieveContactHistoryByCriteria(accountNo, dealerCode, dateFrom, dateTo, rank, preference, content, teamCode, UserID);
        }
        
        [WebMethod]
        public DataSet InsertClientContact(string dealerCode, string userId, string acctNo, string shortKey, string sex, string contactPhone, string content,
                string preferA, string preferB, string remark, string rank, string keep, string adminId, string lastAcctNo, string lastClientName,
                int followupStatus, string followUpDealer,  string followUpDate,string projectID, string dbConnectionStr)
        {
            ClientContactService clientContactService = new ClientContactService(dbConnectionStr);
            return clientContactService.InsertClientContact(dealerCode, userId, acctNo, shortKey, sex, contactPhone, content, preferA, preferB,
                                            remark, rank, keep, adminId, lastAcctNo, lastClientName, followupStatus, followUpDealer, followUpDate, projectID);
        }

        [WebMethod]
        public string[] UpdateClientContact(string dealerCode, string userId, string acctNo, string shortKey, string sex, string contactPhone, string content,
            string preferA, string preferB, string remark, string rank, string keep, string adminId, string recId,
            int folowup,string followUpDealer,  string followUpDate,string projectID, string dbConnectionStr)
        {
            ClientContactService clientContactService = new ClientContactService(dbConnectionStr);
            return clientContactService.UpdateClientContact(dealerCode, userId, acctNo, shortKey, sex, contactPhone, content, preferA, preferB,
                                            remark, rank, keep, adminId, recId, folowup, followUpDealer, followUpDate,projectID);
        }

        [WebMethod]
        public String[] UpdateClientContactFollowUpStatus(String dbConnectionStr, String recId, String followUpStatus)
        {
            ClientContactService clientContactService = new ClientContactService(dbConnectionStr);
            return clientContactService.UpdateClientContactFollowUpStatus(recId, followUpStatus);
        }

        [WebMethod]
        public DataSet DeleteClientContact(string recId, string dealerCode, String UserID, string dbConnectionStr)
        {
            ClientContactService clientContactService = new ClientContactService(dbConnectionStr);
            return clientContactService.DeleteClientContact(recId, dealerCode, UserID);
        }

        [WebMethod]
        public DataSet PrepareForContactHistory(String UserID, string dbConnectionStr)
        {
            ClientContactService clientContactService = new ClientContactService(dbConnectionStr);
            return clientContactService.PrepareForContactHistory(UserID);
        }

        /// <Added by OC>
        [WebMethod]
        public DataSet PrepareForContactHistoryByUserOrSupervisor(String Param, String UserID, String dbConnectionStr)
        {
            ClientContactService clientContactService = new ClientContactService(dbConnectionStr);
            return clientContactService.PrepareForContactHistoryByUserOrSupervisor(Param, UserID);
        }

        [WebMethod]
        public DataSet RetrieveContactAnalysis(string dealerCode, string accountNo, string dateFrom, string dateTo, string dbConnectionStr)
        {
            ClientContactService clientContactService = new ClientContactService(dbConnectionStr);
            return clientContactService.RetrieveContactAnalysis(dealerCode, accountNo, dateFrom, dateTo);
        }

        [WebMethod]
        public DataSet RetrieveCallReport(string assignDate, String UserID, string dbConnectionStr)
        {
            ClientContactService clientContactService = new ClientContactService(dbConnectionStr);
            return clientContactService.RetrieveCallReport(assignDate, UserID);
        }

        /// <Added by OC>
        [WebMethod]
        public DataSet RetrieveCallReportByUserOrSupervisor(String assignDate, String Param, String UserID, String dbConnectionStr)
        {
            ClientContactService clientContactService = new ClientContactService(dbConnectionStr);
            return clientContactService.RetrieveCallReportByUserOrSupervisor(assignDate, Param, UserID);
        }

        [WebMethod]
        public DataSet RetrieveCallReportDetail(string assignDate, string dealerCode, string dbConnectionStr)
        {
            ClientContactService clientContactService = new ClientContactService(dbConnectionStr);
            return clientContactService.RetrieveCallReportDetail(assignDate, dealerCode);
        }

         [WebMethod]
        public DataSet RetrieveCallReportProjectDetail(string ProjectName, string dealerCode, string dbConnectionStr)
        {
            ClientContactService clientContactService = new ClientContactService(dbConnectionStr);
            return clientContactService.RetrieveCallReportProjectDetail(ProjectName, dealerCode);
        }
       
        [WebMethod]
        public DataSet RetrieveClientAnalysis(string teamCode, string dealerCode, string accountCreateDate, string lastTradeDate, string dbConnectionStr)
        {
            ClientContactService clientContactService = new ClientContactService(dbConnectionStr);
            return clientContactService.RetrieveClientAnalysis(teamCode, dealerCode, accountCreateDate, lastTradeDate);
        }

        [WebMethod]
        public DataSet PrepareForClientAnalysis(String UserID, string dbConnectionStr)
        {
            ClientContactService clientContactService = new ClientContactService(dbConnectionStr);
            return clientContactService.PrepareForClientAnalysis(UserID);
        }

        /// <Added by OC>
        [WebMethod]
        public DataSet PrepareForClientAnalysisByUserOrSupervisor(String Param, String UserID, String dbConnectionStr)
        {
            ClientContactService clientContactService = new ClientContactService(dbConnectionStr);
            return clientContactService.PrepareForClientAnalysisByUserOrSupervisor(Param, UserID);
        }

        [WebMethod]
        public DataSet RetrieveContactEntryForToday(string dealerCode, String UserID, string dbConnectionStr)
        {
            ClientContactService clientContactService = new ClientContactService(dbConnectionStr);
            return clientContactService.RetrieveContactEntryForToday(dealerCode, UserID);
        }        

        /**************Update by TSM**************/
        [WebMethod]
        public DataSet RetrieveCallReportByProjectName(string ProjName, String UserID, string dbConnectionStr)
        {
            ClientContactService clientContactService = new ClientContactService(dbConnectionStr);
            return clientContactService.RetrieveCallReportByProjectName(ProjName, UserID);
        }
        /**************End TSM**************/

        /// <Added by OC>
        [WebMethod]
        public DataSet RetrieveCallReportByProjectNameByUserOrSupervisor(String ProjName, String Param, String dbConnectionStr)
        {
            ClientContactService clientContactService = new ClientContactService(dbConnectionStr);
            return clientContactService.RetrieveCallReportByProjectNameByUserOrSupervisor(ProjName, Param);
        }

        /**************Update by TSM**************/
        [WebMethod]
        public DataSet RetrieveContactHistoryByProjName(string accountNo, string dealerCode, string ProjID, string rank,
                            string preference, string content, string teamCode, String UserID, string dbConnectionStr)
        {
            ClientContactService clientContactService = new ClientContactService(dbConnectionStr);
            return clientContactService.RetrieveContactHistoryByProjName(accountNo, dealerCode, ProjID, rank, preference, content, teamCode, UserID);
        }
        /**************End by TSM**************/

        /**************Update by TSM**************/
        [WebMethod]
        public DataSet RetrieveContactAnalysisByProjectName(string dealerCode, string accountNo, string ProjID, string dbConnectionStr)
        {
            ClientContactService clientContactService = new ClientContactService(dbConnectionStr);
            return clientContactService.RetrieveContactAnalysisByProjectName(dealerCode, accountNo, ProjID);
        }
        /**************End by TSM**************/

        [WebMethod]
        public DataSet RetrieveCommissionEarnedByClientAcctNo(String dbConnectionStr, String acctNo)
        {
            ClientContactService clientContactService = new ClientContactService(dbConnectionStr);
            return clientContactService.RetrieveCommissionEarnedByClientAcctNo(acctNo);
        }

        [WebMethod]
        public DataSet RetrieveCashAndEquivalentByUserAcctNo(String dbConnectionStr, String acctNo)
        {
            ClientContactService clientContactService = new ClientContactService(dbConnectionStr);
            return clientContactService.RetrieveCashAndEquivalentByUserAcctNo(acctNo);
        }

        [WebMethod]
        public DataSet RetrieveAvailableFundsByUserAcctNo(String dbConnectionStr, String acctNo)
        {
            ClientContactService clientContactService = new ClientContactService(dbConnectionStr);
            return clientContactService.RetrieveAvailableFundsByUserAcctNo(acctNo);
        }

        [WebMethod]
        public DataSet RetrieveClientServiceTypeByClientAcctNo(String dbConnectionStr, String acctNo)
        {
            ClientContactService clientContactService = new ClientContactService(dbConnectionStr);
            return clientContactService.RetrieveClientServiceTypeByClientAcctNo(acctNo);
        }

        [WebMethod]
        public DataSet PrepareForContactToFollowUp(string userId, String UserRole, string dbConnectionStr)
        {
            ClientContactService clientContactService = new ClientContactService(dbConnectionStr);
            return clientContactService.PrepareForContactToFollowUp(userId, UserRole);
        }
    }
}
