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
    public class ClientAssignmentService
    {
        private ClientAssignmentDA clientAssignmentDA;
        private GenericDA genericDA;

        public ClientAssignmentService()
        {
            genericDA = new GenericDA();
            clientAssignmentDA = new ClientAssignmentDA(genericDA);
        }

        public ClientAssignmentService(string dbConnectionStr) : this()
        {            
            genericDA.SetConnectionString(dbConnectionStr);
        }

        public DataSet RetrieveTeamsForAssignment()
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

        public DataSet RetrieveDealerAndTeamForAssignment()
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

        public DataSet RetrieveDealerByTeamCodeForAssignment(string teamCode)
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
       
        public DataSet RetrieveClientsForAssignment(string teamCode, bool _2NFlag, bool emailFlag, bool mobileFlag,
                  string accountFromDate, string accountToDate, bool contHisFlag, string contHis, bool trustAccFlag, string trustAcc, bool MMFFlag, string MMF, bool TPeriod, string period, bool SMarketValue, string MarketValue, String AccountTypes)
        {
            DataSet ds = new DataSet();
            DataTable dtReturn = null;

            try
            {
                dtReturn = CommonUtilities.CreateReturnTable("1", "");

                genericDA.OpenConnection();

                ds = clientAssignmentDA.RetrieveClientsForAssignment(teamCode, _2NFlag, emailFlag, mobileFlag, accountFromDate,
                                                accountToDate, contHisFlag, contHis, trustAccFlag, trustAcc, MMFFlag, MMF, TPeriod, period, SMarketValue, MarketValue, AccountTypes);

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

        public DataSet RetrieveAssignmentHistoryByCriteria(string dealerCode, string accountNo, string nric,
                    string fromDate, string toDate, bool retradeFlag, String Param, String UserID)
        {
            DataSet ds = new DataSet();
            DataTable dtReturn = null;

            try
            {
                dtReturn = CommonUtilities.CreateReturnTable("1", "");

                genericDA.OpenConnection();             

                ds.Tables.Add(clientAssignmentDA.RetrieveAssignmentHistoryByCriteria(dealerCode, accountNo, nric, fromDate, toDate, retradeFlag, Param, UserID));
                
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

        public DataSet RetrieveAssignedClientInfo(string teamCode, string dealerCode, string accountNo, string assignDate)
        {
            DataSet ds = new DataSet();
            DataTable dtReturn = null;

            try
            {
                dtReturn = CommonUtilities.CreateReturnTable("1", "");

                genericDA.OpenConnection();

                ds.Tables.Add(clientAssignmentDA.RetrieveAssignedClientInfo(assignDate));

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

        public DataSet RetrieveAssignedClientInfoByUserOrSupervisor(String assignDate, String Param)
        {
            DataSet ds = new DataSet();
            DataTable dtReturn = null;

            try
            {
                dtReturn = CommonUtilities.CreateReturnTable("1", "");

                genericDA.OpenConnection();

                ds.Tables.Add(clientAssignmentDA.RetrieveAssignedClientInfoByUserOrSupervisor(assignDate, Param));

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

        public DataSet PrepareForAssignedClientInfo(String UserID)
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

        public DataSet RetrieveAllDealer(String UserID)
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

        /// <Added by OC>
        public DataSet RetrieveAllDealerByUserOrSupervisor(String Param, String UserID)
        {
            DataSet ds = new DataSet();
            DataTable dtReturn = null;

            CommonServiceDA commonServiceDA = new CommonServiceDA(genericDA);

            try
            {
                dtReturn = CommonUtilities.CreateReturnTable("1", "");

                genericDA.OpenConnection();

                //ds.Tables.Add(clientAssignmentDA.RetrieveAssignmentHistoryByCriteria(dealerCode, accountNo, nric, fromDate, toDate));

                ds.Tables.Add(commonServiceDA.RetrieveAllDealerByUserOrSupervisor(Param, UserID));

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
              
        public DataSet RetrieveAssignReportByDealer(string dealerCode)
        {
            DataSet ds = new DataSet();
            DataTable dtReturn = null;

            CommonServiceDA commonServiceDA = new CommonServiceDA(genericDA);
            try
            {
                dtReturn = CommonUtilities.CreateReturnTable("1", "");

                genericDA.OpenConnection();

                ds=clientAssignmentDA.RetrieveAssignReportByDealer(dealerCode);
                if (ds.Tables[0].Rows.Count < 1)
                {
                    dtReturn.Rows[0]["ReturnCode"] = "0";
                    dtReturn.Rows[0]["ReturnMessage"] = "No Record found! Please check Database.";
                }
            }
            catch (Exception e)
            {
                dtReturn.Rows[0]["ReturnCode"] = "-1";
                dtReturn.Rows[0]["ReturnMessage"] = "Error in loading for Assign Report! Please try again." + " Exception Message: "
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

        public DataSet RetrieveFollowUpDateByProjectID(string projectID,string dealerCode)
        {
            DataSet ds = new DataSet();
            DataTable dtReturn = null;

            CommonServiceDA commonServiceDA = new CommonServiceDA(genericDA);
            try
            {
                dtReturn = CommonUtilities.CreateReturnTable("1", "");

                genericDA.OpenConnection();

                ds = clientAssignmentDA.RetrieveFollowUpDateByProjectID(projectID,dealerCode);
                if (ds.Tables[0].Rows.Count < 1)
                {
                    dtReturn.Rows[0]["ReturnCode"] = "0";
                    dtReturn.Rows[0]["ReturnMessage"] = "No Record found! Please check Database.";
                }
            }
            catch (Exception e)
            {
                dtReturn.Rows[0]["ReturnCode"] = "-1";
                dtReturn.Rows[0]["ReturnMessage"] = "Error in loading for Assign Report! Please try again." + " Exception Message: "
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

        public DataSet RetrieveAssignReportByFollowUpDate(string dealerCode)
        {
            DataSet ds = new DataSet();
            DataTable dtReturn = null;

            CommonServiceDA commonServiceDA = new CommonServiceDA(genericDA);
            try
            {
                dtReturn = CommonUtilities.CreateReturnTable("1", "");

                genericDA.OpenConnection();

                ds = clientAssignmentDA.RetrieveAssignReportByFollowUpDate(dealerCode);
                if (ds.Tables[0].Rows.Count < 1)
                {
                    dtReturn.Rows[0]["ReturnCode"] = "0";
                    dtReturn.Rows[0]["ReturnMessage"] = "No Record found! Please check Database.";
                }
            }
            catch (Exception e)
            {
                dtReturn.Rows[0]["ReturnCode"] = "-1";
                dtReturn.Rows[0]["ReturnMessage"] = "Error in loading for Assign Report! Please try again." + " Exception Message: "
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
        
        public string[] InsertAssignment(DataSet ds)
        {
            int result = 1;
            string returnMessage = "Client Assignment is saved successfully.";

            try
            {
                genericDA.OpenConnection();
                genericDA.CreateSqlCommand();

                result = clientAssignmentDA.InsertAssignment(ds);

                genericDA.DisposeSqlCommand();
                genericDA.CloseConnection();

                if(result < 1)
                    returnMessage = "Error in saving Client Assignment!";

            }
            catch (Exception e)
            {
                result = -2;
                
                returnMessage = "Error in saving Client Assignment!" + "<br />";
                returnMessage = returnMessage + e.Message;
            }

            return new string[] { result + "", returnMessage };
        }
            
        public string[] DeleteClientAssignment(string dealerCode, string accountNumber, string assignDate, string cutOffDate, string modifiedUser,
                    string modifiedDate, string newModifiedUser)
        {
            int result = 1;
            string returnMessage = "The Assignment record is deleted successfully.";

            try
            {
                genericDA.OpenConnection();
                genericDA.CreateSqlCommand();

                result = clientAssignmentDA.DeleteClientAssignment(dealerCode, accountNumber, assignDate, cutOffDate, modifiedUser, modifiedDate, newModifiedUser);

                genericDA.DisposeSqlCommand();
                genericDA.CloseConnection();

                if(result < 1)
                    returnMessage = "Record is deleted by other user! Please retrieve assignment again.";
            }
            catch (Exception e)
            {
                result = -1;
                returnMessage = "Error deleting the Record! Pls try again.";
            }

            return new string[] { result + "", returnMessage};            
        }

        public DataSet BatchDeleteClientAssignment(DataTable dtAssignDelete)
        {
            DataSet ds = new DataSet();
            DataTable dtReturn = null;

            try
            {
                genericDA.OpenConnection();
                genericDA.CreateSqlCommand();

                dtReturn = CommonUtilities.CreateReturnTable("1", "The assignment records are deleted successfully.");
                ds.Tables.Add(clientAssignmentDA.BatchDeleteClientAssignment(dtAssignDelete));             

            }
            catch (Exception e)
            {
                dtReturn.Rows[0]["ReturnCode"] = "-1";
                dtReturn.Rows[0]["ReturnMessage"] = "Error deleting assignment records!";           
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


        public DataSet RetrieveCrossEnabledTeam()
        {
            DataSet ds = new DataSet();
            DataTable dtReturn = null;

            try
            {
                dtReturn = CommonUtilities.CreateReturnTable("1", "");

                genericDA.OpenConnection();

                ds.Tables.Add(clientAssignmentDA.RetrieveCrossEnabledTeam());

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

                ds.Tables.Add(clientAssignmentDA.RetrieveCrossEnabledDealer(crossTeamCode));

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

        public DataSet RetrieveAssignmentDateByDateRange(String UserID, string fromDate, string toDate)
        {
            DataSet ds = new DataSet();
            DataTable dtReturn = null;

            try
            {
                dtReturn = CommonUtilities.CreateReturnTable("1", "");

                genericDA.OpenConnection();

                ds.Tables.Add(clientAssignmentDA.RetrieveAssignmentDateByDateRange(UserID, fromDate, toDate));

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

        /// <Added By OC>
        public DataSet RetrieveAssignmentDateByDateRangeByUserOrSupervisor(String UserID, String fromDate, String toDate, String Param)
        {
            DataSet ds = new DataSet();
            DataTable dtReturn = null;

            try
            {
                dtReturn = CommonUtilities.CreateReturnTable("1", "");

                genericDA.OpenConnection();

                ds.Tables.Add(clientAssignmentDA.RetrieveAssignmentDateByDateRangeByUserOrSupervisor(UserID, fromDate, toDate, Param));

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

        public DataSet RetrieveProjectAttachment(string projectID)
        {
            DataSet ds = new DataSet();
            DataTable dtReturn = null;

            try
            {
                dtReturn = CommonUtilities.CreateReturnTable("1", "");

                genericDA.OpenConnection();

                ds.Tables.Add(clientAssignmentDA.RetrieveProjectAttachment(projectID));

                if (ds.Tables[0].Rows.Count < 1)
                {
                    dtReturn.Rows[0]["ReturnCode"] = "0";
                    dtReturn.Rows[0]["ReturnMessage"] = "No Attachment found!";
                }
            }
            catch (Exception e)
            {
                dtReturn.Rows[0]["ReturnCode"] = "-1";
                dtReturn.Rows[0]["ReturnMessage"] = "Error in retrieving Attachment! Please try again. Exception Message: " + e.Message;
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
       
        public string[] InsertProjectInfo(string ProjectName, string ProjectObj, String ProjectType, String FilePath, DateTime AssignDate, DateTime CutOffDate)
        {
            int result = 1;
            string returnMessage = "Project Information is saved successfully.";

            try
            {
                genericDA.OpenConnection();
                genericDA.CreateSqlCommand();

                string projectID = clientAssignmentDA.InsertProjectInfo(ProjectName, ProjectObj, ProjectType, FilePath, AssignDate, CutOffDate);

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

        public string[] InsertProjectAttachment(string FilePath, string FileName, string FileExtension, string FileSize, string ProjectID)
        {
            int result = 1;
            string returnMessage = "Project Attachment is saved successfully.";

            try
            {
                genericDA.OpenConnection();
                genericDA.CreateSqlCommand();

                string projectID = clientAssignmentDA.InsertProjectAttachment(FilePath,FileName,FileExtension,FileSize,ProjectID);

                genericDA.DisposeSqlCommand();
                genericDA.CloseConnection();

                if (projectID == "")
                {
                    returnMessage = "Error in saving Project Attachment!";
                }
                else
                {
                    returnMessage = projectID;
                }

            }
            catch (Exception e)
            {
                result = -2;

                returnMessage = "Error in saving Project Attachment!" + "<br />";
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

                ds.Tables.Add(clientAssignmentDA.RetrieveAllProjectInfo());

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

        public DataSet RetrieveAccountTypeValues(string[] AccList)
        {
            DataSet ds = new DataSet();
            DataTable dtReturn = null;

            try
            {
                dtReturn = CommonUtilities.CreateReturnTable("1", "");

                genericDA.OpenConnection();

                ds.Tables.Add(clientAssignmentDA.RetrieveAccountTypeValues(AccList));

                if (ds.Tables[0].Rows.Count < 1)
                {
                    dtReturn.Rows[0]["ReturnCode"] = "0";
                    dtReturn.Rows[0]["ReturnMessage"] = "No AccountTypeValues found! Please change your search criteria!";
                }
            }
            catch (Exception e)
            {
                dtReturn.Rows[0]["ReturnCode"] = "-1";
                dtReturn.Rows[0]["ReturnMessage"] = "Error in retrieving AccountTypeValues! Please try again. Exception Message: " + e.Message;
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

        public DataSet RetrieveTeamEmailByTeamCode(string teamCode)
        {
            DataSet ds = new DataSet();
            DataTable dtReturn = null;
            AEListDA aeListDA = new AEListDA(genericDA);
            try
            {
                dtReturn = CommonUtilities.CreateReturnTable("1", "");

                genericDA.OpenConnection();

                ds.Tables.Add(aeListDA.RetrieveTeamEmailByTeamCode(teamCode));

                if (ds.Tables[0].Rows.Count < 1)
                {
                    dtReturn.Rows[0]["ReturnCode"] = "0";
                    dtReturn.Rows[0]["ReturnMessage"] = "No Team Email Info found! Please change your search criteria!";
                }
            }
            catch (Exception e)
            {
                dtReturn.Rows[0]["ReturnCode"] = "-1";
                dtReturn.Rows[0]["ReturnMessage"] = "Error in retrieving No Team Email Info! Please try again. Exception Message: " + e.Message;
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

                result = clientAssignmentDA.InsertEmailLog(FromEmail, ToEmail, Subject, EmailContent);

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
        
        /// <summary>
        /// THA - Retrieving Commission Earned by Project ID 
        /// and construct the format that UI required.
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public DataSet RetrieveCommissionEarnedByProjectId(String projectId)
        {
            String returnCode = "1";
            String returnMessage = String.Empty;
            DataTable dtReturn;
            double tempTotalComm;
            DataSet ds = new DataSet();
            try
            {
                genericDA.OpenConnection();
                ds = clientAssignmentDA.RetrieveCommissionEarnedByProjecId(projectId);
                if (ds.Tables[0].Rows.Count == 0)
                {
                    returnCode = "0";
                    returnMessage = "No records for Commission Earned!";
                }
                else 
                {
                    DataTable dtCommissionEarned = GetCommissionEarnedTable();
                    DataRow drCommissionEarned = dtCommissionEarned.NewRow();
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        tempTotalComm = 0.00;
                        tempTotalComm = !String.IsNullOrEmpty(dr["TotalComm"].ToString()) ? Convert.ToDouble(dr["TotalComm"].ToString()) : 0.00;
                        switch (dr["AcctSvcType"].ToString())
                        {
                            case CashTrading:
                                {                                    
                                    drCommissionEarned["CashTrading"] = tempTotalComm;
                                    break;
                                }
                            case CFD:
                                {
                                    drCommissionEarned["CFD"] = tempTotalComm;
                                    break;
                                }
                            case Custodian:
                                {
                                    drCommissionEarned["Custodian"] = tempTotalComm;
                                    break;
                                }
                            case CashManagement:
                                {
                                    drCommissionEarned["CashManagement"] = tempTotalComm;
                                    break;
                                }
                            case PhillipMargin:
                                {
                                    drCommissionEarned["PhillipMargin"] = tempTotalComm;
                                    break;
                                }
                            case PhillipFinancial:
                                {
                                    drCommissionEarned["PhillipFinancial"] = tempTotalComm;
                                    break;
                                }
                            case DiscretionaryAccounts:
                                {
                                    drCommissionEarned["DiscretionaryAccounts"] = tempTotalComm;
                                    break;
                                }
                            case UnitTrustNonWrap:
                                {
                                    drCommissionEarned["UnitTrustNonWrap"] = tempTotalComm;
                                    break;
                                }
                            case AdvisoryAccounts:
                                {
                                    drCommissionEarned["AdvisoryAccounts"] = tempTotalComm;
                                    break;
                                }
                            default:
                                {
                                    break;
                                }
                        }
                    }

                    dtCommissionEarned.Rows.Add(drCommissionEarned);
                    ds.Tables.Add(dtCommissionEarned);
                }
            }
            catch (Exception e)
            {
                returnCode = "-1";
                returnMessage = "Error in retrieving Project's Commission Earned!";
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

        #region Commission Earned Table
        private const String CashTrading = "CA";
        private const String CFD = "CFD";
        private const String Custodian = "CU";
        private const String CashManagement = "KC";
        private const String PhillipMargin = "M";
        private const String PhillipFinancial = "PFN";
        private const String DiscretionaryAccounts = "S2";
        private const String UnitTrustNonWrap = "UT";
        private const String AdvisoryAccounts = "UTW";

        private DataTable GetCommissionEarnedTable()
        {
            DataTable table = new DataTable("CommissionEarned");
            table.Columns.Add("CashTrading", typeof(double));
            table.Columns.Add("CFD", typeof(double));
            table.Columns.Add("Custodian", typeof(double));
            table.Columns.Add("CashManagement", typeof(double));
            table.Columns.Add("PhillipMargin", typeof(double));
            table.Columns.Add("PhillipFinancial", typeof(double));
            table.Columns.Add("DiscretionaryAccounts", typeof(double));
            table.Columns.Add("UnitTrustNonWrap", typeof(double));
            table.Columns.Add("AdvisoryAccounts", typeof(double));            
            return table;
        }
        #endregion

        public DataSet RetrieveClientCallsByProjectId(String userId, String projectId)
        {
            String returnCode = "1";
            String returnMessage = String.Empty;
            DataTable dtReturn;
            DataSet ds = new DataSet();
            try
            {
                genericDA.OpenConnection();
                ds = clientAssignmentDA.RetrieveClientCallsByProjectId(userId, projectId);
                if (ds != null && ds.Tables[0].Rows.Count == 0)
                {
                    returnCode = "0";
                    returnMessage = "Records not found!";
                }
            }
            catch (Exception e)
            {
                returnCode = "-1";
                returnMessage = "Error in retrieving Client Calls!";
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

        public DataSet RetrieveAssignedProjectByUserId(String userId)
        {
            String returnCode = "1";
            String returnMessage = String.Empty;
            DataTable dtReturn;
            DataSet ds = new DataSet();
            try
            {
                genericDA.OpenConnection();
                ds = clientAssignmentDA.RetrieveAssignedProjectByUserId(userId);
                if (ds.Tables[0].Rows.Count == 0)
                {
                    returnCode = "0";
                    returnMessage = "Assigned Clients Records not found!";
                }
            }
            catch (Exception e)
            {
                returnCode = "-1";
                returnMessage = "Error in retrieving Assigned Clients!";
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

        /**************Update by TSM**************/
        // To bind the ProjName DDl
        public DataSet RetrieveProjectByProjectName(string Pn, String UserID)
        {
            DataSet ds = new DataSet();
            DataTable dtReturn = null;

            try
            {
                dtReturn = CommonUtilities.CreateReturnTable("1", "");

                genericDA.OpenConnection();

                ds.Tables.Add(clientAssignmentDA.RetrieveProjectByProjectName(Pn, UserID));

                if (ds.Tables[0].Rows.Count < 1)
                {
                    dtReturn.Rows[0]["ReturnCode"] = "0";
                    dtReturn.Rows[0]["ReturnMessage"] = "No Project found! Please type your project Name!";
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

        /// <Added by OC>
        public DataSet RetrieveProjectByProjectNameByUserOrSupervisor(String Param, String UserID)
        {
            DataSet ds = new DataSet();
            DataTable dtReturn = null;

            try
            {
                dtReturn = CommonUtilities.CreateReturnTable("1", "");

                genericDA.OpenConnection();

                ds.Tables.Add(clientAssignmentDA.RetrieveProjectByProjectNameByUserOrSupervisor(Param, UserID));

                if (ds.Tables[0].Rows.Count < 1)
                {
                    dtReturn.Rows[0]["ReturnCode"] = "0";
                    dtReturn.Rows[0]["ReturnMessage"] = "No Project found! Please type your project Name!";
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

        /**************End TSM**************/

        /**************Update by TSM**************/
        public DataSet RetrieveAssignedClientInfoByProj(string ProjID)
        {
            DataSet ds = new DataSet();
            DataTable dtReturn = null;
            string projtype = RetrieveProjectType(ProjID);

            try
            {
                dtReturn = CommonUtilities.CreateReturnTable("1", "");

                genericDA.OpenConnection();

                //ds.Tables.Add(clientAssignmentDA.RetrieveAssignedClientInfo(teamCode, dealerCode, accountNo, assignDate));
                if (projtype == "C")
                {
                    ds.Tables.Add(clientAssignmentDA.RetrieveAssignedClientInfoByProj(ProjID));
                }
                else
                {
                    ds.Tables.Add(clientAssignmentDA.RetrieveAssignedLeadInfoByProj(ProjID));
                }

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

        /// <Added by OC>
        public DataSet RetrieveAssignedClientInfoByProjByUserOrSupervisor(String ProjID, String Param)
        {
            DataSet ds = new DataSet();
            DataTable dtReturn = null;
            string projtype = RetrieveProjectType(ProjID);

            try
            {
                dtReturn = CommonUtilities.CreateReturnTable("1", "");

                genericDA.OpenConnection();

                //ds.Tables.Add(clientAssignmentDA.RetrieveAssignedClientInfo(teamCode, dealerCode, accountNo, assignDate));
                if (projtype == "C")
                {
                    ds.Tables.Add(clientAssignmentDA.RetrieveAssignedClientInfoByProjByUserOrSupervisor(ProjID, Param));
                }
                else
                {
                    ds.Tables.Add(clientAssignmentDA.RetrieveAssignedLeadInfoByProjByUserOrSupervisor(ProjID, Param));
                }

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

        public String RetrieveProjectType(string projectname)
        {
            DataTable dtProjtype = new DataTable();
            DataSet ds = new DataSet();


            string ProjectType = null;


            genericDA.OpenConnection();
            dtProjtype = clientAssignmentDA.RetrieveProjectType(projectname);

            if (dtProjtype != null)
            {
                if (dtProjtype.Rows.Count > 0)
                {
                    ProjectType = dtProjtype.Rows[0]["ProjectType"].ToString();
                }
            }
            else
            {
                ProjectType = null;

            }

            return ProjectType;
        }


        /**************End TSM**************/
        public DataSet RetrieveAssignmentHistoryByProjName(string dealerCode, string accountNo, string nric,
             string ProjName, bool retradeFlag)
        {
            DataSet ds = new DataSet();
            DataTable dtReturn = null;
            string projtype = RetrieveProjectType(ProjName);

            try
            {
                dtReturn = CommonUtilities.CreateReturnTable("1", "");

                genericDA.OpenConnection();
                
                if (projtype == "C")
                {
                    ds.Tables.Add(clientAssignmentDA.RetrieveAssignmentHistoryByProjName(dealerCode, accountNo, nric, ProjName, retradeFlag));
                }
                else if(projtype == "L")
                {
                    ds.Tables.Add(clientAssignmentDA.RetrieveAssignmentHistoryByLeadProjName(dealerCode, accountNo, nric, ProjName, retradeFlag));
                
                }

                if (projtype == null)
                {
                    dtReturn.Rows[0]["ReturnCode"] = "0";
                    dtReturn.Rows[0]["ReturnMessage"] = "No Assignments found! Please change your search criteria.";
                }
                else if (ds.Tables[0].Rows.Count < 1)
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
        /**************End TSM**************/

        public DataSet RetrieveCommissionEarnedHistoryByCriteria(string dealerCode, string accountNo, string nric,
                    string fromDate, string toDate, bool retradeFlag)
        {
            DataSet ds = new DataSet();
            DataTable dtReturn = null;
            try
            {
                dtReturn = CommonUtilities.CreateReturnTable("1", "");
                genericDA.OpenConnection();

                ds.Tables.Add(clientAssignmentDA.RetrieveCommissionEarnedHistoryByCriteria(dealerCode, accountNo, nric, fromDate, toDate, retradeFlag));

                if (ds.Tables[0].Rows.Count < 1)
                {
                    dtReturn.Rows[0]["ReturnCode"] = "0";
                    dtReturn.Rows[0]["ReturnMessage"] = "No Records found!";
                }
                else
                {
                    double tempTotalComm;
                    DataTable dtCommissionEarned = GetCommissionEarnedTable();
                    DataRow drCommissionEarned = dtCommissionEarned.NewRow();
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        tempTotalComm = 0.00;
                        tempTotalComm = !String.IsNullOrEmpty(dr["TotalComm"].ToString()) ? Convert.ToDouble(dr["TotalComm"].ToString()) : 0.00;
                        switch (dr["AccServiceType"].ToString())
                        {
                            case CashTrading:
                                {
                                    drCommissionEarned["CashTrading"] = tempTotalComm;
                                    break;
                                }
                            case CFD:
                                {
                                    drCommissionEarned["CFD"] = tempTotalComm;
                                    break;
                                }
                            case Custodian:
                                {
                                    drCommissionEarned["Custodian"] = tempTotalComm;
                                    break;
                                }
                            case CashManagement:
                                {
                                    drCommissionEarned["CashManagement"] = tempTotalComm;
                                    break;
                                }
                            case PhillipMargin:
                                {
                                    drCommissionEarned["PhillipMargin"] = tempTotalComm;
                                    break;
                                }
                            case PhillipFinancial:
                                {
                                    drCommissionEarned["PhillipFinancial"] = tempTotalComm;
                                    break;
                                }
                            case DiscretionaryAccounts:
                                {
                                    drCommissionEarned["DiscretionaryAccounts"] = tempTotalComm;
                                    break;
                                }
                            case UnitTrustNonWrap:
                                {
                                    drCommissionEarned["UnitTrustNonWrap"] = tempTotalComm;
                                    break;
                                }
                            case AdvisoryAccounts:
                                {
                                    drCommissionEarned["AdvisoryAccounts"] = tempTotalComm;
                                    break;
                                }
                            default:
                                {
                                    break;
                                }
                        }
                    }

                    dtCommissionEarned.Rows.Add(drCommissionEarned);
                    ds.Tables.Add(dtCommissionEarned);
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

        public DataSet RetrieveCommissionEarnedHistoryByProjName(string dealerCode, string accountNo, string nric,
                    string ProjName, bool retradeFlag)
        {
            DataSet ds = new DataSet();
            DataTable dtReturn = null;
            string projtype = RetrieveProjectType(ProjName);

            try
            {
                dtReturn = CommonUtilities.CreateReturnTable("1", "");
                genericDA.OpenConnection();
                if (projtype == "C")
                {
                    ds.Tables.Add(clientAssignmentDA.RetrieveCommissionEarnedHistoryByProjName(dealerCode, accountNo, nric, ProjName, retradeFlag));

                    if (ds.Tables[0].Rows.Count < 1)
                    {
                        dtReturn.Rows[0]["ReturnCode"] = "0";
                        dtReturn.Rows[0]["ReturnMessage"] = "No Records found!";
                    }
                    else
                    {
                        double tempTotalComm;
                        DataTable dtCommissionEarned = GetCommissionEarnedTable();
                        DataRow drCommissionEarned = dtCommissionEarned.NewRow();
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            tempTotalComm = 0.00;
                            tempTotalComm = !String.IsNullOrEmpty(dr["TotalComm"].ToString()) ? Convert.ToDouble(dr["TotalComm"].ToString()) : 0.00;
                            switch (dr["AccServiceType"].ToString())
                            {
                                case CashTrading:
                                    {
                                        drCommissionEarned["CashTrading"] = tempTotalComm;
                                        break;
                                    }
                                case CFD:
                                    {
                                        drCommissionEarned["CFD"] = tempTotalComm;
                                        break;
                                    }
                                case Custodian:
                                    {
                                        drCommissionEarned["Custodian"] = tempTotalComm;
                                        break;
                                    }
                                case CashManagement:
                                    {
                                        drCommissionEarned["CashManagement"] = tempTotalComm;
                                        break;
                                    }
                                case PhillipMargin:
                                    {
                                        drCommissionEarned["PhillipMargin"] = tempTotalComm;
                                        break;
                                    }
                                case PhillipFinancial:
                                    {
                                        drCommissionEarned["PhillipFinancial"] = tempTotalComm;
                                        break;
                                    }
                                case DiscretionaryAccounts:
                                    {
                                        drCommissionEarned["DiscretionaryAccounts"] = tempTotalComm;
                                        break;
                                    }
                                case UnitTrustNonWrap:
                                    {
                                        drCommissionEarned["UnitTrustNonWrap"] = tempTotalComm;
                                        break;
                                    }
                                case AdvisoryAccounts:
                                    {
                                        drCommissionEarned["AdvisoryAccounts"] = tempTotalComm;
                                        break;
                                    }
                                default:
                                    {
                                        break;
                                    }
                            }
                        }

                        dtCommissionEarned.Rows.Add(drCommissionEarned);
                        ds.Tables.Add(dtCommissionEarned);
                    }
                }
                else if (projtype == "L")
                {
                    dtReturn.Rows[0]["ReturnCode"] = "-2";
                    dtReturn.Rows[0]["ReturnMessage"] = "Not applicable for Lead Project";
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
    }
}
