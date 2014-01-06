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


namespace SPMWebApp.Services
{
    public class LeadsAssignmentService
    {
        //private AssignmentManagementService.AssignmentManagement assignmentManagement;
        private LeadsAssignmentManagementService.LeadsAssignmentManagement assignmentManagement;
        private string dbConnectionStr;
        private DataTable dtReturn;

        public LeadsAssignmentService()
        {
            //assignmentManagement = new SPMWebApp.AssignmentManagementService.AssignmentManagement();
            assignmentManagement = new SPMWebApp.LeadsAssignmentManagementService.LeadsAssignmentManagement();
            dbConnectionStr = "";
        }

        public LeadsAssignmentService(string dbConnectionStr) : this()
        {
            this.dbConnectionStr = dbConnectionStr;                    
        }

        public DataSet RetrieveLeadsForAssignment(string teamCode)//, bool _2NFlag, bool emailFlag, bool mobileFlag,string accountFromDate, string accountToDate)
        {
            DataSet ds = null;

            try
            {
                ds = assignmentManagement.RetrieveLeadsForAssignment(teamCode,dbConnectionStr);//, _2NFlag, emailFlag, mobileFlag, accountFromDate,accountToDate, dbConnectionStr);
            }
            catch (Exception e)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                ds.Tables.Add(dtReturn);
            }

            return ds;
        }

        public DataSet RetrieveTeamsForLeadsAssignment()
        {
            DataSet ds = null;

            try
            {
                ds = assignmentManagement.RetrieveTeamsForLeadsAssignment(dbConnectionStr);
            }
            catch (Exception e)
            { }            

            return ds;
        }

        public DataSet RetrieveDealerAndTeamForLeadsAssignment()
        {
            DataSet ds = null;

            try
            {
                ds = assignmentManagement.RetrieveDealerAndTeamForLeadsAssignment(dbConnectionStr);
            }
            catch (Exception e)
            {
            }

            return ds;
        }

        public DataSet RetrieveDealerByTeamCodeForLeadsAssignment(string teamCode)
        {
            DataSet ds = null;

            try
            {
                //ds = assignmentManagement.r
                ds = assignmentManagement.RetrieveDealerByTeamCodeForLeadsAssignment(teamCode, dbConnectionStr);
            }
            catch (Exception e)
            {
            }

            return ds;
        }

        public DataSet RetrieveLeadsAssignmentHistoryByCriteria(string dealerCode, string accountNo, string nric, 
                            string fromDate, string toDate, bool retradeFlag)
        {
            DataSet ds = null;

            try
            {
                ds = assignmentManagement.RetrieveLeadsAssignmentHistoryByCriteria (dealerCode, accountNo, nric, fromDate, toDate, retradeFlag, dbConnectionStr);
            }
            catch (Exception e)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                ds.Tables.Add(dtReturn);
            }

            return ds;
        }

        public DataSet RetrieveAssignedLeadsInfo(string teamCode, string dealerCode, string accountNo, string assignDate)
        {
            DataSet ds = null;

            try
            {
                ds = assignmentManagement.RetrieveAssignedLeadsInfo(teamCode, dealerCode, accountNo, assignDate, dbConnectionStr);
            }
            catch (Exception e)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                ds.Tables.Add(dtReturn);
            }

            return ds;
        }

        public DataSet PrepareForAssignedLeadsInfo(String UserID)
        {
            DataSet ds = null;

            try
            {
                ds = assignmentManagement.PrepareForAssignedLeadsInfo(UserID, dbConnectionStr);
            }
            catch (Exception e)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                ds.Tables.Add(dtReturn);
            }

            return ds;
        }

        public DataSet PrepareForLeadsAssignmentHistory(String UserID)
        {
            DataSet ds = null;

            try
            {
                ds = assignmentManagement.PrepareForLeadsAssignmentHistory(UserID, dbConnectionStr);
            }
            catch (Exception e)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                ds.Tables.Add(dtReturn);
            }

            return ds;
        }

        public string[] InsertLeadsAssignment(DataSet ds,string ProjectID)
        {
            string[] wsReturn = null;

            try
            {
                wsReturn = assignmentManagement.InsertLeadsAssignment(ds,ProjectID,dbConnectionStr);
            }
            catch (Exception e)
            {
                wsReturn = new string[] { "-3", "Error in WebService Connection!" };
            }

            return wsReturn;
        }

        public DataSet BatchDeleteLeadsAssignment(DataTable dtAssignDelete)
        {
            DataSet ds = null;

            try
            {
                DataSet dsTemp = new DataSet();
                dsTemp.Tables.Add(dtAssignDelete);
                ds = assignmentManagement.BatchDeleteLeadsAssignment(dsTemp, dbConnectionStr);
            }
            catch (Exception e)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                ds.Tables.Add(dtReturn);
            }

            return ds;
        }

        public string[] DeleteLeadsAssignment(string dealerCode, string leadId, string assignDate, string cutOffDate, string modifiedUser,
                    string modifiedDate, string newModifiedUser)
        {
            string[] wsReturn = null;

            try
            {
                wsReturn = assignmentManagement.DeleteLeadsAssignment(dealerCode, leadId, assignDate, cutOffDate, modifiedUser,
                            modifiedDate, newModifiedUser, dbConnectionStr);
            }
            catch (Exception e)
            {
                wsReturn = new string[] {"-3", "Error in WebService Connection!"};
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

        public DataSet RetrieveLeadsAssignmentDateByDateRange(string fromDate, string toDate)
        {
            DataSet ds = null;

            try
            {
                ds = assignmentManagement.RetrieveLeadsAssignmentDateByDateRange(fromDate, toDate, dbConnectionStr);
            }
            catch (Exception e)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                ds.Tables.Add(dtReturn);
            }

            return ds;
        }

        public string[] SaveProjectInformation(string ProjectName,DateTime CutOffDate)
        {
            string[] wsReturn = null;

            try
            {
                wsReturn = assignmentManagement.SaveProjectInformation(ProjectName,CutOffDate, dbConnectionStr);
            }
            catch (Exception e)
            {
                wsReturn = new string[] { "-3", "Error in WebService Connection!" };
            }

            return wsReturn;
        }

        public DataSet RetrieveAssignedLeadsProjectsByUserId(String userId)
        {
            DataSet ds = new DataSet();
            try
            {
                ds = assignmentManagement.RetrieveAssignedLeadsProjectByUserId(dbConnectionStr, userId);
            }
            catch (Exception e)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!" + e.ToString());
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
    }
}
