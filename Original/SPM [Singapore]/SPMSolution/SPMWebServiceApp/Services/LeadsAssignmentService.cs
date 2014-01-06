using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using SPMWebServiceApp.DataAccess;
using SPMWebServiceApp.Utilities;


namespace SPMWebServiceApp.Services
{
    public class LeadsAssignmentService
    {
        private LeadsAssignmentDA leadsAssignmentDA;
        private GenericDA genericDA;

        public LeadsAssignmentService()
        {
            genericDA = new GenericDA();
            leadsAssignmentDA = new LeadsAssignmentDA(genericDA);
        }

        public LeadsAssignmentService(string dbConnectionStr) : this()
        {            
            genericDA.SetConnectionString(dbConnectionStr);
        }

        public DataSet RetrieveTeamsForLeadsAssignment()
        {
            DataSet ds = new DataSet();
            
            try
            {
                AEListDA aeListDA = new AEListDA(genericDA);
                genericDA.OpenConnection();

                ds = aeListDA.RetrieveAllTeam();

                genericDA.CloseConnection();
            }
            catch (Exception e)
            {
            }

            return ds;
        }

        public DataSet RetrieveDealerAndTeamForLeadsAssignment()
        {
            DataSet ds = new DataSet();
            DataTable dtDealer = new DataTable();

            try
            {
                AEListDA aeListDA = new AEListDA(genericDA);
                DealerDA dealerDA = new DealerDA(genericDA);

                genericDA.OpenConnection();

                ds = aeListDA.RetrieveAllTeam();
                if (ds.Tables[0].Rows.Count > 0)
                {                    
                    ds = dealerDA.FillDealerByTeamCode(ds, "dtDealer", ds.Tables[0].Rows[0]["TeamCode"].ToString());
                }
                                
                genericDA.CloseConnection();
            }
            catch (Exception e)
            {
            }

            return ds;
        }

        public DataSet RetrieveDealerByTeamCodeForLeadsAssignment(string teamCode)
        {
            DataSet ds = new DataSet();
            DealerDA dealerDA = new DealerDA(genericDA);

            try
            {
                genericDA.OpenConnection();
                ds = dealerDA.RetrieveDealerByTeamCode(teamCode);
                genericDA.CloseConnection();
            }
            catch (Exception e)
            {
            }

            return ds;
        }

        public DataSet RetrieveLeadsForAssignment(string teamCode)//, bool _2NFlag, bool emailFlag, bool mobileFlag, string accountFromDate, string accountToDate)
        {
            DataSet ds = new DataSet();
            DataTable dtReturn = null;

            try
            {
                dtReturn = CommonUtilities.CreateReturnTable("1", "");

                genericDA.OpenConnection();

                ds = leadsAssignmentDA.RetrieveLeadsForAssignment(teamCode);// _2NFlag, emailFlag, mobileFlag, accountFromDate, accountToDate);               

                if (ds.Tables[0].Rows.Count < 1)
                {
                    dtReturn.Rows[0]["ReturnCode"] = "0";
                    dtReturn.Rows[0]["ReturnMessage"] = "No Client found! Please change your search criteria.";
                }
            }
            catch (Exception e)
            {
                dtReturn.Rows[0]["ReturnCode"] = "-1";
                dtReturn.Rows[0]["ReturnMessage"] = "Error in retrieving Clients! Please try again. Exception Message: " + e.Message;
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

        public DataSet RetrieveLeadsAssignmentHistoryByCriteria(string dealerCode, string accountNo, string nric,
                    string fromDate, string toDate, bool retradeFlag)
        {
            DataSet ds = new DataSet();
            DataTable dtReturn = null;

            try
            {
                dtReturn = CommonUtilities.CreateReturnTable("1", "");

                genericDA.OpenConnection();             

                ds.Tables.Add(leadsAssignmentDA.RetrieveLeadsAssignmentHistoryByCriteria(dealerCode, accountNo, nric, fromDate, toDate, retradeFlag));
                
                if (ds.Tables[0].Rows.Count < 1)
                {
                    dtReturn.Rows[0]["ReturnCode"] = "0";
                    dtReturn.Rows[0]["ReturnMessage"] = "No Assignments found! Please change your search criteria.";
                }
            }
            catch (Exception e)
            {
                dtReturn.Rows[0]["ReturnCode"] = "-1";
                dtReturn.Rows[0]["ReturnMessage"] = "Error in retrieving Assignments! Please try again." + " Exception Message: " 
                            + e.Message;
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

        public DataSet RetrieveAssignedLeadsInfo(string teamCode, string dealerCode, string accountNo, string assignDate)
        {
            DataSet ds = new DataSet();
            DataTable dtReturn = null;

            try
            {
                dtReturn = CommonUtilities.CreateReturnTable("1", "");

                genericDA.OpenConnection();

                ds.Tables.Add(leadsAssignmentDA.RetrieveAssignedLeadsInfo(teamCode, dealerCode, accountNo, assignDate));

                if (ds.Tables[0].Rows.Count < 1)
                {
                    dtReturn.Rows[0]["ReturnCode"] = "0";
                    dtReturn.Rows[0]["ReturnMessage"] = "No Assigned Client Info found! Please change your search criteria.";
                }
            }
            catch (Exception e)
            {
                dtReturn.Rows[0]["ReturnCode"] = "-1";
                dtReturn.Rows[0]["ReturnMessage"] = "Error in retrieving Assigned Client Info! Please try again." + " Exception Message: "
                            + e.Message;
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

        public DataSet PrepareForAssignedLeadsInfo(String UserID)
        {
            CommonServiceDA commonServiceDA = new CommonServiceDA(genericDA);
            DataSet ds = new DataSet();
            DataTable dtReturn = null;
            string returnCode = "1", returnMessage = "";

            try
            {
                genericDA.OpenConnection();

                ds.Tables.Add(commonServiceDA.RetrieveTeamCodeAndNameByDataTable());
                if (ds.Tables[0].Rows.Count < 1)
                {
                    returnCode = "0";
                    returnMessage = "Dealer Team cannot retrieve.";
                }

                ds.Tables.Add(commonServiceDA.RetrieveAllDealer(UserID));
                if (ds.Tables[1].Rows.Count < 1)
                {
                    returnCode = "0";
                    returnMessage = "Dealers cannot retrieve.";
                }
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

        public DataSet PrepareForLeadsAssignmentHistory(String UserID)
        {
            DataSet ds = new DataSet();
            DataTable dtReturn = null;

            CommonServiceDA commonServiceDA = new CommonServiceDA(genericDA);

            try
            {
                dtReturn = CommonUtilities.CreateReturnTable("1", "");

                genericDA.OpenConnection();

                //ds.Tables.Add(clientAssignmentDA.RetrieveAssignmentHistoryByCriteria(dealerCode, accountNo, nric, fromDate, toDate));

                ds.Tables.Add(commonServiceDA.RetrieveAllDealer(UserID));

                if (ds.Tables[0].Rows.Count < 1)
                {
                    dtReturn.Rows[0]["ReturnCode"] = "0";
                    dtReturn.Rows[0]["ReturnMessage"] = "No Dealers found! Please check Database.";
                }                
            }
            catch (Exception e)
            {
                dtReturn.Rows[0]["ReturnCode"] = "-1";
                dtReturn.Rows[0]["ReturnMessage"] = "Error in loading for Assignmentment History! Please try again." + " Exception Message: "
                            + e.Message;
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

        public string[] InsertLeadsAssignment(DataSet ds,string ProjectID)
        {
            int result = 1; int leadresult = 1;
            string returnMessage = "Leads Assignment is saved successfully.";

            try
            {
                genericDA.OpenConnection();
                genericDA.CreateSqlCommand();

                result = leadsAssignmentDA.InsertLeadsAssignment (ds);

                genericDA.DisposeSqlCommand();
                genericDA.CloseConnection();

                if (result < 1)
                {                    
                    returnMessage = "Error in saving Leads Assignment!";

                    leadresult=leadsAssignmentDA.DeleteLeadsProjects(ProjectID);
                }
            }
            catch (Exception e)
            {
                result = -2;
                leadresult = leadsAssignmentDA.DeleteLeadsProjects(ProjectID);
                
                returnMessage = "Error in saving Leads Assignment!" + "<br />";
                returnMessage = returnMessage + e.Message;
            }

            return new string[] { result + "", returnMessage };
        }


        public DataSet BatchDeleteLeadsAssignment(DataTable dtAssignDelete)
        {
            DataSet ds = new DataSet();
            DataTable dtReturn = null;

            try
            {
                genericDA.OpenConnection();
                genericDA.CreateSqlCommand();

                dtReturn = CommonUtilities.CreateReturnTable("1", "The Leads Assignment record is deleted successfully.");
                ds.Tables.Add(leadsAssignmentDA.BatchDeleteLeadsAssignment(dtAssignDelete));

            }
            catch (Exception e)
            {
                dtReturn.Rows[0]["ReturnCode"] = "-1";
                dtReturn.Rows[0]["ReturnMessage"] = "Error in deleting Assignment. Exception Message: " + e.Message;
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

            ds.Tables.Add(dtReturn);
            return ds;
        }

        public string[] DeleteLeadsAssignment(string dealerCode, string leadId, string assignDate, string cutOffDate, string modifiedUser,
                    string modifiedDate, string newModifiedUser)
        {
            int result = 1;
            string returnMessage = "The Leads Assignment record is deleted successfully.";

            try
            {
                genericDA.OpenConnection();
                genericDA.CreateSqlCommand();

                result = leadsAssignmentDA.DeleteLeadsAssignment (dealerCode, leadId , assignDate, cutOffDate, modifiedUser, modifiedDate, newModifiedUser);

                genericDA.DisposeSqlCommand();
                genericDA.CloseConnection();

                if(result < 1)
                    returnMessage = "Record is deleted by other user! Please retrieve leads assignment again.";
            }
            catch (Exception e)
            {
                result = -1;
                returnMessage = "Error deleting the Record! Pls try again.";
            }

            return new string[] { result + "", returnMessage};            
        }

        public DataSet RetrieveCrossEnabledTeam()
        {
            DataSet ds = new DataSet();
            DataTable dtReturn = null;

            try
            {
                dtReturn = CommonUtilities.CreateReturnTable("1", "");

                genericDA.OpenConnection();

                ds.Tables.Add(leadsAssignmentDA.RetrieveCrossEnabledTeam());

                if (ds.Tables[0].Rows.Count < 1)
                {
                    dtReturn.Rows[0]["ReturnCode"] = "0";
                    dtReturn.Rows[0]["ReturnMessage"] = "No Cross Enabled Team found!";
                }
            }
            catch (Exception e)
            {
                dtReturn.Rows[0]["ReturnCode"] = "-1";
                dtReturn.Rows[0]["ReturnMessage"] = "Error in retrieving Cross Enabled Team! Please try again. Exception Message: " + e.Message;
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

        public DataSet RetrieveCrossEnabledDealer(string crossTeamCode)
        {
            DataSet ds = new DataSet();
            DataTable dtReturn = null;

            try
            {
                dtReturn = CommonUtilities.CreateReturnTable("1", "");

                genericDA.OpenConnection();

                ds.Tables.Add(leadsAssignmentDA.RetrieveCrossEnabledDealer(crossTeamCode));

                if (ds.Tables[0].Rows.Count < 1)
                {
                    dtReturn.Rows[0]["ReturnCode"] = "0";
                    dtReturn.Rows[0]["ReturnMessage"] = "No Cross Enabled Dealer found!";
                }
            }
            catch (Exception e)
            {
                dtReturn.Rows[0]["ReturnCode"] = "-1";
                dtReturn.Rows[0]["ReturnMessage"] = "Error in retrieving Cross Enabled Dealer! Please try again. Exception Message: " + e.Message;
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

        public DataSet RetrieveLeadsAssignmentDateByDateRange(string fromDate, string toDate)
        {
            DataSet ds = new DataSet();
            DataTable dtReturn = null;

            try
            {
                dtReturn = CommonUtilities.CreateReturnTable("1", "");

                genericDA.OpenConnection();

                ds.Tables.Add(leadsAssignmentDA.RetrieveLeadsAssignmentDateByDateRange (fromDate, toDate));

                if (ds.Tables[0].Rows.Count < 1)
                {
                    dtReturn.Rows[0]["ReturnCode"] = "0";
                    dtReturn.Rows[0]["ReturnMessage"] = "No Assignment found! Please change your search criteria!";
                }
            }
            catch (Exception e)
            {
                dtReturn.Rows[0]["ReturnCode"] = "-1";
                dtReturn.Rows[0]["ReturnMessage"] = "Error in retrieving Assignment! Please try again. Exception Message: " + e.Message;
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

        public string[] SaveProjectInformation(string ProjectName,DateTime  CutOffDate)
        {
            int result = 1;
            string returnMessage = "Project Information is saved successfully.";

            try
            {
                genericDA.OpenConnection();
                genericDA.CreateSqlCommand();

                string projectID = leadsAssignmentDA.SaveProjectInfo(ProjectName,CutOffDate);

                genericDA.DisposeSqlCommand();
                genericDA.CloseConnection();

                if (projectID == "")
                {
                    returnMessage = "Error in saving Project Information!";
                }
                else
                {
                    returnMessage = projectID;
                }

            }
            catch (Exception e)
            {
                result = -2;

                returnMessage = "Error in saving Project Information!" + "<br />";
                returnMessage = returnMessage + e.Message;
            }

            return new string[] { result + "", returnMessage };
        }

        public DataSet RetrieveAllProjectInformation()
        {
            DataSet ds = new DataSet();
            DataTable dtReturn = null;

            try
            {
                dtReturn = CommonUtilities.CreateReturnTable("1", "");

                genericDA.OpenConnection();

                ds.Tables.Add(leadsAssignmentDA.RetrieveAllProjectInfo());

                if (ds.Tables[0].Rows.Count < 1)
                {
                    dtReturn.Rows[0]["ReturnCode"] = "0";
                    dtReturn.Rows[0]["ReturnMessage"] = "No Project Information found! Please change your search criteria!";
                }
            }
            catch (Exception e)
            {
                dtReturn.Rows[0]["ReturnCode"] = "-1";
                dtReturn.Rows[0]["ReturnMessage"] = "Error in retrieving Project Information! Please try again. Exception Message: " + e.Message;
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

        internal DataSet RetrieveLeadsAssignedProjectByUserId(String userId)
        {
            String returnCode = "1";
            String returnMessage = String.Empty;
            DataTable dtReturn;
            DataSet ds = new DataSet();
            try
            {
                genericDA.OpenConnection();
                ds = leadsAssignmentDA.RetrieveLeadsAssignedProjectByUserId(userId);
                if (ds.Tables[0].Rows.Count == 0)
                {
                    returnCode = "0";
                    returnMessage = "Leads Records not found!";
                }
            }
            catch (Exception e)
            {
                returnCode = "-1";
                returnMessage = "Error in retrieving Assigned Projects!";
            }
            finally
            {
                try
                {
                    dtReturn = CommonUtilities.CreateReturnTable(returnCode, returnMessage);
                    ds.Tables.Add(dtReturn);
                    genericDA.CloseConnection();
                }
                catch (Exception e)
                {
                }
            }
            return ds;
        }

        public DataSet RetrieveDealerEmailByDelaerCode(string dealerCode)
        {
            DataSet ds = new DataSet();
            DataTable dtReturn = null;
            AEListDA aeListDA = new AEListDA(genericDA);
            try
            {
                dtReturn = CommonUtilities.CreateReturnTable("1", "");

                genericDA.OpenConnection();

                ds.Tables.Add(aeListDA.RetrieveDealerEmailByDelaerCode(dealerCode));

                if (ds.Tables[0].Rows.Count < 1)
                {
                    dtReturn.Rows[0]["ReturnCode"] = "0";
                    dtReturn.Rows[0]["ReturnMessage"] = "No Delaer Info found! Please change your search criteria!";
                }
            }
            catch (Exception e)
            {
                dtReturn.Rows[0]["ReturnCode"] = "-1";
                dtReturn.Rows[0]["ReturnMessage"] = "Error in retrieving Delaer Info! Please try again. Exception Message: " + e.Message;
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

        public string[] InsertEmailLog(string FromEmail, string ToEmail, string Subject, string EmailContent)
        {
            int result = 1;
            string returnMessage = "Email Log is saved successfully.";

            try
            {
                genericDA.OpenConnection();
                genericDA.CreateSqlCommand();

                result = leadsAssignmentDA.InsertEmailLog(FromEmail, ToEmail, Subject, EmailContent);

                genericDA.DisposeSqlCommand();
                genericDA.CloseConnection();

                if (result < 1)
                    returnMessage = "Error in saving Email Log!";

            }
            catch (Exception e)
            {
                result = -2;

                returnMessage = "Error in saving Email Log!" + "<br />";
                returnMessage = returnMessage + e.Message;
            }

            return new string[] { result + "", returnMessage };
        }      
    }
}
