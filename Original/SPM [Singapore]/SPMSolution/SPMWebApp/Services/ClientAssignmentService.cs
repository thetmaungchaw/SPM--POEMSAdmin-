using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using SPMWebApp.Utilities;
using System.IO;


namespace SPMWebApp.Services
{
    public class ClientAssignmentService
    {
        private AssignmentManagementService.AssignmentManagement assignmentManagement;
        private string dbConnectionStr;
        private DataTable dtReturn;


        public ClientAssignmentService()
        {
            assignmentManagement = new SPMWebApp.AssignmentManagementService.AssignmentManagement();
            dbConnectionStr = "";
        }

        public ClientAssignmentService(string dbConnectionStr) : this()
        {
            this.dbConnectionStr = dbConnectionStr;                    
        }

        public DataSet RetrieveClientsForAssignment(string teamCode, bool _2NFlag, bool emailFlag, bool mobileFlag,
                    string accountFromDate, string accountToDate, bool contHisFlag, string contHis, bool trustAccFlag,
                    string trustAcc, bool MMFFlag, string MMF, bool TPeriod, string period, bool SMarketValue, string MarketValue, String AccountTypes)
        {
            DataSet ds = null;

            try
            {
                ds = assignmentManagement.RetrieveClientsForAssignment(teamCode, _2NFlag, emailFlag, mobileFlag, accountFromDate,
                        accountToDate, dbConnectionStr,
                        contHisFlag, contHis, trustAccFlag, trustAcc, MMFFlag, MMF, TPeriod, period, SMarketValue, MarketValue, AccountTypes);
            }
            catch (Exception e)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                ds.Tables.Add(dtReturn);
            }

            return ds;
        }
        //public DataSet RetrieveClientsForAssignment(string teamCode, bool _2NFlag, bool emailFlag, bool mobileFlag,
        //            string accountFromDate, string accountToDate)
        //{
        //    DataSet ds = null;

        //    try
        //    {
        //        ds = assignmentManagement.RetrieveClientsForAssignment(teamCode, _2NFlag, emailFlag, mobileFlag, accountFromDate,
        //                accountToDate, dbConnectionStr);
        //    }
        //    catch (Exception e)
        //    {
        //        dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
        //        ds.Tables.Add(dtReturn);
        //    }

        //    return ds;
        //}

        public DataSet RetrieveTeamsForAssignment()
        {
            DataSet ds = null;

            try
            {
                ds = assignmentManagement.RetrieveTeamsForAssignment(dbConnectionStr);
            }
            catch (Exception e)
            { }            

            return ds;
        }

        public DataSet RetrieveDealerAndTeamForAssignment()
        {
            DataSet ds = null;

            try
            {
                ds = assignmentManagement.RetrieveDealerAndTeamForAssignment(dbConnectionStr);
            }
            catch (Exception e)
            {
            }

            return ds;
        }

        public DataSet RetrieveDealerByTeamCodeForAssignment(string teamCode)
        {
            DataSet ds = null;

            try
            {
                //ds = assignmentManagement.r
                ds = assignmentManagement.RetrieveDealerByTeamCodeForAssignment(teamCode, dbConnectionStr);
            }
            catch (Exception e)
            {
            }

            return ds;
        }

        public DataSet RetrieveAssignmentHistoryByCriteria(string dealerCode, string accountNo, string nric, 
                            string fromDate, string toDate, bool retradeFlag, String Param, String UserID)
        {
            DataSet ds = null;

            try
            {
                ds = assignmentManagement.RetrieveAssignmentHistoryByCriteria(dealerCode, accountNo, nric, fromDate, toDate, retradeFlag, Param, UserID, dbConnectionStr);
            }
            catch (Exception e)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                ds.Tables.Add(dtReturn);
            }

            return ds;
        }

        /**************Update by TSM**************/
        public DataSet RetrieveAssignmentHistoryByProjectName(string dealerCode, string accountNo, string nric,
                          string ProjName, bool retradeFlag)
        {
            DataSet ds = null;

            try
            {
                ds = assignmentManagement.RetrieveAssignmentHistoryByProjName(dealerCode, accountNo, nric, ProjName, retradeFlag, dbConnectionStr);
            }
            catch (Exception e)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                ds.Tables.Add(dtReturn);
            }

            return ds;
        }
        /**************End by TSM**************/

        public DataSet RetrieveAssignedClientInfo(string teamCode, string dealerCode, string accountNo, string assignDate)
        {
            DataSet ds = null;

            try
            {
                ds = assignmentManagement.RetrieveAssignedClientInfo(teamCode, dealerCode, accountNo, assignDate, dbConnectionStr);
            }
            catch (Exception e)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                ds.Tables.Add(dtReturn);
            }

            return ds;
        }

        /// <Added by OC>
        public DataSet RetrieveAssignedClientInfoByUserOrSupervisor(String assignDate, String Param)
        {
            DataSet ds = null;

            try
            {
                ds = assignmentManagement.RetrieveAssignedClientInfoByUserOrSupervisor(assignDate, Param, dbConnectionStr);
            }
            catch (Exception e)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                ds.Tables.Add(dtReturn);
            }

            return ds;
        }

        public DataSet PrepareForAssignedClientInfo(String UserID)
        {
            DataSet ds = null;

            try
            {
                ds = assignmentManagement.PrepareForAssignedClientInfo(UserID, dbConnectionStr);
            }
            catch (Exception e)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                ds.Tables.Add(dtReturn);
            }

            return ds;
        }

        public DataSet RetrieveAllDealer(String UserID)
        {
            DataSet ds = null;

            try
            {
                ds = assignmentManagement.RetrieveAllDealer(UserID, dbConnectionStr);
            }
            catch (Exception e)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                ds.Tables.Add(dtReturn);
            }

            return ds;
        }

        /// <Added by OC>
        public DataSet RetrieveAllDealerByUserOrSupervisor(String Param, String UserID)
        {
            DataSet ds = null;

            try
            {
                ds = assignmentManagement.RetrieveAllDealerByUserOrSupervisor(Param, UserID, dbConnectionStr);
            }
            catch (Exception e)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                ds.Tables.Add(dtReturn);
            }

            return ds;
        }

        public DataSet RetrieveAssignReportByDealer(string dealerCode)
        {
            DataSet ds = null;

            try
            {
                ds = assignmentManagement.RetrieveAssignReportByDealer(dealerCode,dbConnectionStr);
            }
            catch (Exception e)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                ds.Tables.Add(dtReturn);
            }

            return ds;
        }

        public DataSet RetrieveAssignReportByFollowUpDate(string dealerCode)
        {
            DataSet ds = null;

            try
            {
                ds = assignmentManagement.RetrieveAssignReportByFollowUpDate(dealerCode,dbConnectionStr);
            }
            catch (Exception e)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                ds.Tables.Add(dtReturn);
            }

            return ds;
        }

        public DataSet RetrieveFollowUpDateByProjectID(string projectID,string dealerCode)
        {
            DataSet ds = null;

            try
            {
                ds = assignmentManagement.RetrieveFollowUpDateByProjectID(projectID, dealerCode, dbConnectionStr);
            }
            catch (Exception e)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                ds.Tables.Add(dtReturn);
            }

            return ds;
        }

        public string[] InsertAssignment(DataSet ds)
        {
            string[] wsReturn = null;

            try
            {
                wsReturn = assignmentManagement.InsertAssignment(ds, dbConnectionStr);
            }
            catch (Exception e)
            {
                wsReturn = new string[] { "-3", "Error in WebService Connection!" };
            }

            return wsReturn;
        }
        
        public DataSet BatchDeleteClientAssignment(DataTable dtAssignDelete)
        {
            DataSet ds= null;

            try
            {
                DataSet dsTemp = new DataSet();
                dsTemp.Tables.Add(dtAssignDelete);
                ds = assignmentManagement.BatchDeleteClientAssignment(dsTemp, dbConnectionStr);
            }
            catch (Exception e)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                ds.Tables.Add(dtReturn);
            }

            return ds;
        }

        public string[] DeleteClientAssignment(string dealerCode, string accountNumber, string assignDate, string cutOffDate, string modifiedUser,
                   string modifiedDate, string newModifiedUser)
        {
            string[] wsReturn = null;

            try
            {
                wsReturn = assignmentManagement.DeleteClientAssignment(dealerCode, accountNumber, assignDate, cutOffDate, modifiedUser,
                            modifiedDate, newModifiedUser, dbConnectionStr);
            }
            catch (Exception e)
            {
                wsReturn = new string[] { "-3", "Error in WebService Connection!" };
            }

            return wsReturn;
        }

        public DataSet RetrieveCrossEnabledTeam()
        {
            DataSet ds = null;

            try
            {
                ds = assignmentManagement.RetrieveCrossEnabledTeam(dbConnectionStr);
            }
            catch (Exception e)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                ds.Tables.Add(dtReturn);
            }

            return ds;
        }

        public DataSet RetrieveCrossEnabledDealer(string crossTeamCode)
        {
            DataSet ds = null;

            try
            {
                ds = assignmentManagement.RetrieveCrossEnabledDealer(crossTeamCode, dbConnectionStr);
            }
            catch (Exception e)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                ds.Tables.Add(dtReturn);
            }

            return ds;
        }

        public DataSet RetrieveAssignmentDateByDateRange(String UserID, string fromDate, string toDate)
        {
            DataSet ds = null;

            try
            {
                ds = assignmentManagement.RetrieveAssignmentDateByDateRange(UserID, fromDate, toDate, dbConnectionStr);
            }
            catch (Exception e)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                ds.Tables.Add(dtReturn);
            }

            return ds;
        }
        
        /// <Added by OC>
        public DataSet RetrieveAssignmentDateByDateRangeByUserOrSupervisor(String UserID, String fromDate, String toDate, String Param)
        {
            DataSet ds = null;

            try
            {
                ds = assignmentManagement.RetrieveAssignmentDateByDateRangeByUserOrSupervisor(UserID, fromDate, toDate, Param, dbConnectionStr);
            }
            catch (Exception e)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                ds.Tables.Add(dtReturn);
            }

            return ds;
        }

        public DataSet RetrieveAccountTypeValues(string[] accList)
        {
            DataSet ds = null;

            try
            {
                ds = assignmentManagement.RetrieveAccountTypeValues(accList, dbConnectionStr);
            }
            catch (Exception e)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                ds.Tables.Add(dtReturn);
            }

            return ds;
        }

        public string[] SaveProjectInformation(string ProjectName, string ProjectObj, String ProjectType, String FilePath,DateTime AssignDate,DateTime CutoffDate )
        {
            string[] wsReturn = null;

            try
            {
                wsReturn = assignmentManagement.InsertProjectInfo(ProjectName, ProjectObj, ProjectType, FilePath,AssignDate,CutoffDate, dbConnectionStr);
            }
            catch (Exception e)
            {
                wsReturn = new string[] { "-3", "Error in WebService Connection!" };
            }

            return wsReturn;
        }

        public string[] InsertProjectAttachment(string FilePath, string FileName, string FileExtension, string FileSize, string ProjectID)
        {
            string[] wsReturn = null;

            try
            {
                wsReturn = assignmentManagement.InsertProjectAttachment(FilePath,FileName,FileExtension,FileSize,ProjectID,dbConnectionStr);
            }
            catch (Exception e)
            {
                wsReturn = new string[] { "-3", "Error in WebService Connection!" };
            }

            return wsReturn;
        }

        public DataSet RetrieveAllProjectInfo()
        {
            DataSet ds = null;
            try
            {
                ds = assignmentManagement.RetrieveAllProjectInformation(dbConnectionStr);
            }
            catch (Exception e)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                ds.Tables.Add(dtReturn);
            }

            return ds;
        }

        public DataSet GetClientInfoByAcctNo(string AccNo)
        {

            DataSet ds = null;

            try
            {
                ds = assignmentManagement.GetClientInfoByAccNo(AccNo, dbConnectionStr);
            }
            catch (Exception e)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                ds.Tables.Add(dtReturn);
            }

            return ds;            
        }

        public DataSet RetrieveDealerEmailByDealerCode(string dealerCode)
        {

            DataSet ds = null;

            try
            {
               ds = assignmentManagement.RetrieveDealerEmailByDelaerCode(dealerCode, dbConnectionStr);
            }
            catch (Exception e)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                ds.Tables.Add(dtReturn);
            }

            return ds;
        }

        public DataSet RetrieveTeamEmailByTeamCode(string teamCode)
        {

            DataSet ds = null;

            try
            {
                ds = assignmentManagement.RetrieveTeamEmailByTeamCode(teamCode, dbConnectionStr);
            }
            catch (Exception e)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                ds.Tables.Add(dtReturn);
            }

            return ds;
        }


        public DataSet RetrieveProjectAttachment(string projectID)
        {

            DataSet ds = null;

            try
            {
                ds = assignmentManagement.RetrieveProjectAttachment(projectID, dbConnectionStr);
            }
            catch (Exception e)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                ds.Tables.Add(dtReturn);
            }

            return ds;
        }

        public string[] InsertEmailLog(string FromEmail, string ToEmail, string Subject, string EmailContent)
        {
            string[] wsReturn = null;
            try
            {
                wsReturn = assignmentManagement.InsertEmailLog(FromEmail, ToEmail, Subject, EmailContent, dbConnectionStr);
            }
            catch (Exception e)
            {
                wsReturn = new string[] { "-3", "Error in WebService Connection!" };
            }

            return wsReturn;
        }

        public DataSet RetrieveCommissionEarnedByProjectId(String projectId)
        {
            DataSet ds = new DataSet();
            try
            {
                ds = assignmentManagement.RetrieveCommissionEarnedByProjectId(dbConnectionStr, projectId);
            }
            catch (Exception e)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                ds.Tables.Add(dtReturn);
            }
            return ds;
        }

        public DataSet RetrieveClientCallsByProjectId(String userId, String projectId)
        {
            DataSet ds = new DataSet();
            try
            {
                ds = assignmentManagement.RetrieveClientCallsByProjectId(dbConnectionStr, userId, projectId);
            }
            catch
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                ds.Tables.Add(dtReturn);
            }
            return ds;
        }

        public DataSet RetrieveAssignedProjectsByUserId(String userId)
        {
            DataSet ds = new DataSet();
            try
            {
                ds = assignmentManagement.RetrieveAssignedProjectByUserId(dbConnectionStr, userId);
            }
            catch (Exception e)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                ds.Tables.Add(dtReturn);
            }
            return ds;
        }

        /**************Update by TSM**************/
        public DataSet RetrieveProjectByProjectName(string PName, String UserID)
        {
            DataSet ds = null;

            try
            {
                ds = assignmentManagement.RetrieveProjectByProjectName(PName, UserID, dbConnectionStr);
            }
            catch (Exception e)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                ds.Tables.Add(dtReturn);
            }

            return ds;
        }

        /// <Added by OC>
        public DataSet RetrieveProjectByProjectNameByUserOrSupervisor(String Param, String UserID)
        {
            DataSet ds = null;

            try
            {
                ds = assignmentManagement.RetrieveProjectByProjectNameByUserOrSupervisor(Param, UserID, dbConnectionStr);
            }
            catch (Exception e)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                ds.Tables.Add(dtReturn);
            }

            return ds;
        }

        /**************Update by TSM**************/
        public DataSet RetrieveAssignedClientInfoByProj(string ProjID)
        {
            DataSet ds = null;
            try
            {
                ds = assignmentManagement.RetrieveAssignedClientInfoByProj(ProjID, dbConnectionStr);
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
        public DataSet RetrieveAssignedClientInfoByProjByUserOrSupervisor(String ProjID, String Param)
        {
            DataSet ds = null;
            try
            {
                ds = assignmentManagement.RetrieveAssignedClientInfoByProjByUserOrSupervisor(ProjID, Param, dbConnectionStr);
            }
            catch (Exception e)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                ds.Tables.Add(dtReturn);
            }
            return ds;
        }

        public DataSet RetrieveCommissionEarnedHistoryByCriteria(string dealerCode, string accountNo, string nric,
                            string fromDate, string toDate, bool retradeFlag)
        {
            DataSet ds = new DataSet();
            try
            {
                ds = assignmentManagement.RetrieveCommissionEarnedHistoryByCriteria(dealerCode, accountNo, nric, fromDate, toDate, retradeFlag, dbConnectionStr);
            }
            catch (Exception e)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                ds.Tables.Add(dtReturn);
            }
            return ds;
        }

        public DataSet RetrieveCommissionEarnedHistoryByProjectName(string dealerCode, string accountNo, string nric,
                          string ProjName, bool retradeFlag)
        {
            DataSet ds = new DataSet();
            try
            {
                ds = assignmentManagement.RetrieveCommissionEarnedHistoryByProject(dealerCode, accountNo, nric, ProjName, retradeFlag, dbConnectionStr);
            }
            catch (Exception e)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                ds.Tables.Add(dtReturn);
            }
            return ds;
        }
    }
}
