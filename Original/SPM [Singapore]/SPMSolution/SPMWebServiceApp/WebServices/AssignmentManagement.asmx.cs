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
    /// Summary description for AssignmentManagement
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    public class AssignmentManagement : System.Web.Services.WebService
    {        

        
        [WebMethod]
        public DataSet RetrieveClientsForAssignment(string teamCode, bool _2NFlag, bool emailFlag, bool mobileFlag,
                    string accountFromDate, string accountToDate, string dbConnectionStr,
                    bool contHisFlag, string contHis, bool trustAccFlag, string trustAcc, bool MMFFlag, string MMF, bool TPeriod, string period, bool SMarketValue, string MarketValue, String AccountTypes)
        {
            DataSet ds = null;
            ClientAssignmentService clientAssignmentService = new ClientAssignmentService(dbConnectionStr);
            ds = clientAssignmentService.RetrieveClientsForAssignment(teamCode, _2NFlag, emailFlag, mobileFlag, accountFromDate,
                                                accountToDate, contHisFlag, contHis, trustAccFlag, trustAcc, MMFFlag, MMF, TPeriod, period, SMarketValue, MarketValue, AccountTypes);
            return ds;
        }

        [WebMethod]
        public DataSet RetrieveTeamsForAssignment(string dbConnectionStr)
        {
            DataSet ds = null;
            ClientAssignmentService clientAssignmentService = new ClientAssignmentService(dbConnectionStr);

            ds = clientAssignmentService.RetrieveTeamsForAssignment();
            return ds;
        }

        [WebMethod]
        public DataSet RetrieveDealerAndTeamForAssignment(string dbConnectionStr)
        {
            DataSet ds = null;
            ClientAssignmentService clientAssignmentService = new ClientAssignmentService(dbConnectionStr);
            ds = clientAssignmentService.RetrieveDealerAndTeamForAssignment();

            return ds;
        }

        [WebMethod]
        public DataSet RetrieveDealerByTeamCodeForAssignment(string teamCode, string dbConnectionStr)
        {
            DataSet ds = null;
            ClientAssignmentService clientAssignmentService = new ClientAssignmentService(dbConnectionStr);

            ds = clientAssignmentService.RetrieveDealerByTeamCodeForAssignment(teamCode);
            return ds;
        }

        [WebMethod]
        public string[] InsertAssignment(DataSet ds, string dbConnectionStr)
        {
            ClientAssignmentService clientAssignmentService = new ClientAssignmentService(dbConnectionStr);
            return clientAssignmentService.InsertAssignment(ds);
        }

        [WebMethod]
        public string[] DeleteClientAssignment(string dealerCode, string accountNumber, string assignDate, string cutOffDate, string modifiedUser,
                    string modifiedDate, string newModifiedUser, string dbConnectionStr)
        {
            ClientAssignmentService clientAssignmentService = new ClientAssignmentService(dbConnectionStr);
            return clientAssignmentService.DeleteClientAssignment(dealerCode, accountNumber, assignDate, cutOffDate, modifiedUser, modifiedDate, newModifiedUser);
        }

        [WebMethod]
        public DataSet BatchDeleteClientAssignment(DataSet dsAssignDelete, string dbConnectionStr)
        {
            ClientAssignmentService clientAssignmentService = new ClientAssignmentService(dbConnectionStr);
            return clientAssignmentService.BatchDeleteClientAssignment(dsAssignDelete.Tables[0]);
        }

        [WebMethod]
        public DataSet RetrieveAssignmentHistoryByCriteria(string dealerCode, string accountNo, string nric, 
                            string fromDate, string toDate, bool retradeFlag, String Param, String UserID, string dbConnectionStr)
        {
            ClientAssignmentService clientAssignmentService = new ClientAssignmentService(dbConnectionStr);
            return clientAssignmentService.RetrieveAssignmentHistoryByCriteria(dealerCode, accountNo, nric, fromDate, toDate, retradeFlag, Param, UserID);
        }

        [WebMethod]
        public DataSet RetrieveAssignedClientInfo(string teamCode, string dealerCode, string accountNo, string assignDate, string dbConnectionStr)
        {
            ClientAssignmentService clientAssignmentService = new ClientAssignmentService(dbConnectionStr);
            return clientAssignmentService.RetrieveAssignedClientInfo(teamCode, dealerCode, accountNo, assignDate);
        }

        /// <Added by OC>
        [WebMethod]
        public DataSet RetrieveAssignedClientInfoByUserOrSupervisor(String assignDate, String Param, string dbConnectionStr)
        {
            ClientAssignmentService clientAssignmentService = new ClientAssignmentService(dbConnectionStr);
            return clientAssignmentService.RetrieveAssignedClientInfoByUserOrSupervisor(assignDate, Param);
        }

        [WebMethod]
        public DataSet PrepareForAssignedClientInfo(String UserID, string dbConnectionStr)
        {
            ClientAssignmentService clientAssignmentService = new ClientAssignmentService(dbConnectionStr);
            return clientAssignmentService.PrepareForAssignedClientInfo(UserID);
        }

        [WebMethod]
        public DataSet RetrieveAllDealer(String UserID, string dbConnectionStr)
        {
            ClientAssignmentService clientAssignmentService = new ClientAssignmentService(dbConnectionStr);
            return clientAssignmentService.RetrieveAllDealer(UserID);
        }

        /// <Added by OC>
        [WebMethod]
        public DataSet RetrieveAllDealerByUserOrSupervisor(String Param, String UserID, String dbConnectionStr)
        {
            ClientAssignmentService clientAssignmentService = new ClientAssignmentService(dbConnectionStr);
            return clientAssignmentService.RetrieveAllDealerByUserOrSupervisor(Param, UserID);
        }

          [WebMethod]
        public DataSet RetrieveAssignReportByDealer(string dealerCode, string dbConnectionStr)
        {
            ClientAssignmentService clientAssignmentService = new ClientAssignmentService(dbConnectionStr);
            return clientAssignmentService.RetrieveAssignReportByDealer(dealerCode);
        }

        [WebMethod]
          public DataSet RetrieveAssignReportByFollowUpDate(string dealerCode, string dbConnectionStr)
        {
          ClientAssignmentService clientAssignmentService = new ClientAssignmentService(dbConnectionStr);
          return clientAssignmentService.RetrieveAssignReportByFollowUpDate(dealerCode);
        }

        [WebMethod]
        public DataSet RetrieveFollowUpDateByProjectID(string projectID,string dealerCode, string dbConnectionStr)
        {
            ClientAssignmentService clientAssignmentService = new ClientAssignmentService(dbConnectionStr);
            return clientAssignmentService.RetrieveFollowUpDateByProjectID(projectID,dealerCode);
        }
        
        [WebMethod]
        public DataSet RetrieveCrossEnabledTeam(string dbConnectionStr)
        {
            ClientAssignmentService clientAssignmentService = new ClientAssignmentService(dbConnectionStr);
            return clientAssignmentService.RetrieveCrossEnabledTeam();
        }

        [WebMethod]
        public DataSet RetrieveCrossEnabledDealer(string crossTeamCode, string dbConnectionStr)
        {
            ClientAssignmentService clientAssignmentService = new ClientAssignmentService(dbConnectionStr);
            return clientAssignmentService.RetrieveCrossEnabledDealer(crossTeamCode);
        }
        
        [WebMethod]
        public DataSet RetrieveAssignmentDateByDateRange(String UserID, string fromDate, string toDate, string dbConnectionStr)
        {
            ClientAssignmentService clientAssignmentService = new ClientAssignmentService(dbConnectionStr);
            return clientAssignmentService.RetrieveAssignmentDateByDateRange(UserID, fromDate, toDate);
        }

        /// <Added by OC>
        [WebMethod]
        public DataSet RetrieveAssignmentDateByDateRangeByUserOrSupervisor(String UserID, String fromDate, String toDate, String Param, String dbConnectionStr)
        {
            ClientAssignmentService clientAssignmentService = new ClientAssignmentService(dbConnectionStr);
            return clientAssignmentService.RetrieveAssignmentDateByDateRangeByUserOrSupervisor(UserID, fromDate, toDate, Param);
        }

         [WebMethod]
        public DataSet RetrieveProjectAttachment(string projectID, string dbConnectionStr)
        {
            ClientAssignmentService clientAssignmentService = new ClientAssignmentService(dbConnectionStr);
            return clientAssignmentService.RetrieveProjectAttachment(projectID);
        }

        [WebMethod]
        public DataSet RetrieveAccountTypeValues(string[] accList, string dbConnectionStr)
        {
            ClientAssignmentService clientAssignmentService = new ClientAssignmentService(dbConnectionStr);
            return clientAssignmentService.RetrieveAccountTypeValues(accList);
        }

        [WebMethod]
        public string[] InsertProjectInfo(string ProjectName, string ProjectObj, String ProjectType, String FilePath, DateTime AssignDate, DateTime CutOffDate, string dbConnectionStr)
        {
            ClientAssignmentService clientAssignmentService = new ClientAssignmentService(dbConnectionStr);
            return clientAssignmentService.InsertProjectInfo(ProjectName, ProjectObj, ProjectType, FilePath, AssignDate, CutOffDate);
        }

        [WebMethod]
        public string[] InsertProjectAttachment(string FilePath, string FileName, string FileExtension, string FileSize, string ProjectID, string dbConnectionStr)
        {
            ClientAssignmentService clientAssignmentService = new ClientAssignmentService(dbConnectionStr);
            return clientAssignmentService.InsertProjectAttachment(FilePath, FileName, FileExtension, FileSize, ProjectID);
        }

        [WebMethod]
        public DataSet RetrieveAllProjectInformation(string dbConnectionStr)
        {
            ClientAssignmentService clientAssignmentService = new ClientAssignmentService(dbConnectionStr);
            return clientAssignmentService.RetrieveAllProjectInformation();
        }       


        [WebMethod]
        public DataSet GetClientInfoByAccNo(string AcctNo, string dbConnectionStr)
        {
            //ClientAssignmentService clientAssignmentService = new ClientAssignmentService(dbConnectionStr);
            ClientContactService clientContact=new ClientContactService(dbConnectionStr);
            return clientContact.GetClientByAcctNo(AcctNo);
        }

        [WebMethod]
        public DataSet RetrieveDealerEmailByDelaerCode(string dealerCode, string dbConnectionStr)
        {
            ClientAssignmentService clientAssignmentService = new ClientAssignmentService(dbConnectionStr);
            return clientAssignmentService.RetrieveDealerEmailByDelaerCode(dealerCode);
        }

        [WebMethod]
        public DataSet RetrieveTeamEmailByTeamCode(string teamCode, string dbConnectionStr)
        {
            ClientAssignmentService clientAssignmentService = new ClientAssignmentService(dbConnectionStr);
            return clientAssignmentService.RetrieveTeamEmailByTeamCode(teamCode);
        }

        [WebMethod]
        public string[] InsertEmailLog(string FromEmail, string ToEmail, string Subject, string EmailContent, string dbConnectionStr)
        {
            ClientAssignmentService clientAssignmentService = new ClientAssignmentService(dbConnectionStr);
            return clientAssignmentService.InsertEmailLog(FromEmail, ToEmail, Subject, EmailContent);
        }

        [WebMethod]
        public DataSet RetrieveProjectByProjectName(string projName, String UserID, string dbConnectionStr)
        {
            ClientAssignmentService clientAssignmentService = new ClientAssignmentService(dbConnectionStr);
            return clientAssignmentService.RetrieveProjectByProjectName(projName, UserID);
        }

        /// <Added by OC>
        [WebMethod]
        public DataSet RetrieveProjectByProjectNameByUserOrSupervisor(String Param, String UserID, String dbConnectionStr)
        {
            ClientAssignmentService clientAssignmentService = new ClientAssignmentService(dbConnectionStr);
            return clientAssignmentService.RetrieveProjectByProjectNameByUserOrSupervisor(Param, UserID);
        }

        [WebMethod]
        public DataSet RetrieveClientCallsByProjectId(String dbConnectionStr, String userId, String projectId)
        {
            ClientAssignmentService clientAssignmentService = new ClientAssignmentService(dbConnectionStr);
            return clientAssignmentService.RetrieveClientCallsByProjectId(userId, projectId);
        }

        [WebMethod]
        public DataSet RetrieveAssignedProjectByUserId(String dbConnectionStr, String userId)
        {
            ClientAssignmentService clientAssignedService = new ClientAssignmentService(dbConnectionStr);
            return clientAssignedService.RetrieveAssignedProjectByUserId(userId);
        }

        [WebMethod]
        public DataSet RetrieveCommissionEarnedByProjectId(String dbConnectionStr, String projectId)
        {
            ClientAssignmentService clientAssignedService = new ClientAssignmentService(dbConnectionStr);
            return clientAssignedService.RetrieveCommissionEarnedByProjectId(projectId);
        }
        /**************End TSM**************/


        /**************Update by TSM**************/
        //For AssignedClientInfo

        [WebMethod]
        public DataSet RetrieveAssignedClientInfoByProj(string ProjID, string dbConnectionStr)
        {
            ClientAssignmentService clientAssignmentService = new ClientAssignmentService(dbConnectionStr);
            return clientAssignmentService.RetrieveAssignedClientInfoByProj(ProjID);
        }
        /**************End TSM**************/

        /// <Added by OC>
        [WebMethod]
        public DataSet RetrieveAssignedClientInfoByProjByUserOrSupervisor(String ProjID, String Param, String dbConnectionStr)
        {
            ClientAssignmentService clientAssignmentService = new ClientAssignmentService(dbConnectionStr);
            return clientAssignmentService.RetrieveAssignedClientInfoByProjByUserOrSupervisor(ProjID, Param);
        }

        /**************Update by TSM**************/
        [WebMethod]
        public DataSet RetrieveAssignmentHistoryByProjName(string dealerCode, string accountNo, string nric,
                            string ProjName, bool retradeFlag, string dbConnectionStr)
        {
            ClientAssignmentService clientAssignmentService = new ClientAssignmentService(dbConnectionStr);
            return clientAssignmentService.RetrieveAssignmentHistoryByProjName(dealerCode, accountNo, nric, ProjName, retradeFlag);
        }
        /**************End TSM**************/

        [WebMethod]
        public DataSet RetrieveCommissionEarnedHistoryByCriteria(string dealerCode, string accountNo, string nric,
                            string fromDate, string toDate, bool retradeFlag, string dbConnectionStr)
        {
            ClientAssignmentService clientAssignmentService = new ClientAssignmentService(dbConnectionStr);
            return clientAssignmentService.RetrieveCommissionEarnedHistoryByCriteria(dealerCode, accountNo, nric, fromDate, toDate, retradeFlag);
        }

        [WebMethod]
        public DataSet RetrieveCommissionEarnedHistoryByProject(string dealerCode, string accountNo, string nric,
                            string ProjName, bool retradeFlag, string dbConnectionStr)
        {
            ClientAssignmentService clientAssignmentService = new ClientAssignmentService(dbConnectionStr);
            return clientAssignmentService.RetrieveCommissionEarnedHistoryByProjName(dealerCode, accountNo, nric, ProjName, retradeFlag);
        }
    }
}
