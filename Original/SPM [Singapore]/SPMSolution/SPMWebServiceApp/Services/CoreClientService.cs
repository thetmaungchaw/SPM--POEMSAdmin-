using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;

using SPMWebServiceApp.DataAccess;
using SPMWebServiceApp.Utilities;

namespace SPMWebServiceApp.Services
{
    public class CoreClientService
    {
        private GenericDA genericDA;
        private CoreClientDA coreClientDA;

        public CoreClientService()
        {
            genericDA = new GenericDA();
            coreClientDA = new CoreClientDA(genericDA);
        }

        public CoreClientService(string dbConnStr) : this()
        {
            genericDA.SetConnectionString(dbConnStr);
        }

        public DataSet RetrieveCoreClientList(string AECode, string AccNo)
        {
            DataSet ds = new DataSet();
            DataTable dtReturn = null;

            try
            {
                dtReturn = CommonUtilities.CreateReturnTable("1", "");

                genericDA.OpenConnection();
                ds.Tables.Add(coreClientDA.RetrieveCoreClientList(AECode, AccNo));
                if (ds.Tables[0].Rows.Count < 1)
                {
                    dtReturn.Rows[0]["ReturnCode"] = "0";
                    dtReturn.Rows[0]["ReturnMessage"] = "Core Client not found! Please change your search criteria.";
                }
            }
            catch (Exception e)
            {
                dtReturn.Rows[0]["ReturnCode"] = "-1";
                dtReturn.Rows[0]["ReturnMessage"] = "Error in retrieving Dealers! Please try again. Exception Message: " + e.Message;
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

        public DataSet AddCoreClient(string AECode, string AccNo, string userId)
        {
            int result = 1;
            DataSet ds = new DataSet();
            DataTable dtReturn = null;
            
            try
            {
                dtReturn = CommonUtilities.CreateReturnTable("1", "");

                genericDA.OpenConnection();

                if (coreClientDA.CheckForCoreClientTeam(AECode, AccNo).Rows.Count > 0)
                {
                    if (!coreClientDA.IsCoreClientExist(AECode, AccNo))
                    {
                        genericDA.CreateSqlCommand();
                        result = coreClientDA.AddCoreClient(AECode, AccNo, userId);

                        if (result > 0)
                        {
                            //ds = coreClientDA.RetrieveCoreClientList(AECode, AccNo);
                            ds.Tables.Add(coreClientDA.RetrieveCoreClientList(AECode, AccNo));
                            dtReturn.Rows[0]["ReturnMessage"] = "Core Client is successfully saved.";
                        }
                        else
                        {
                            //Return as Error in insert record
                            dtReturn.Rows[0]["ReturnCode"] = "0";
                            dtReturn.Rows[0]["ReturnMessage"] = "Error in adding Core Client! Please try again.";
                        }
                    }
                    else
                    {
                        //Return as existing core client
                        dtReturn.Rows[0]["ReturnCode"] = "2";
                        dtReturn.Rows[0]["ReturnMessage"] = "The client is already a core client of a dealer!";
                    }
                }
                else
                {
                    //Return as existing core client
                    dtReturn.Rows[0]["ReturnCode"] = "-2";
                    dtReturn.Rows[0]["ReturnMessage"] = "Client is not in the dealer’s team!";
                }

                
            }
            catch (Exception e)
            {                
                dtReturn.Rows[0]["ReturnCode"] = "-1";
                dtReturn.Rows[0]["ReturnMessage"] = "Error in saving the core client! Please try again. Exception Message: " + e.Message;
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

        public string[] DeleteCoreClient(long recId)
        {
            int result = -1;
            string returnMessage = "Core Client is deleted successfully.";

            try
            {
                genericDA.OpenConnection();
                genericDA.CreateSqlCommand();

                result = coreClientDA.DeleteCoreClient(recId);
                if (result < 1)
                {
                    returnMessage = "Error in deleting Core Client! The Core Client has been deleted by other user.";
                }          
            }
            catch (Exception e)
            {
                result = -1;
                returnMessage = "Error in deleting Core Client! Please try again. Exception Message: " + e.Message;
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

            return new string[] { result + "", returnMessage }; 
        }
    }
}
