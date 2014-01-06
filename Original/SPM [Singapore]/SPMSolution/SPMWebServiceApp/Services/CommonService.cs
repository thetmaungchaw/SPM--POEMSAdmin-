﻿using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using SPMWebServiceApp.DataAccess;
using SPMWebServiceApp.Utilities;


namespace SPMWebServiceApp.Services
{
    public class CommonService
    {
        private GenericDA genericDA;
        private CommonServiceDA commonServiceDA;
        private DataTable dtReturn;

        private string returnCode;
        private string returnMessage;
        
        public CommonService()
        {
            genericDA = new GenericDA();
            commonServiceDA = new CommonServiceDA(genericDA);

            returnCode = "1";
            returnMessage = "";
        }

        public CommonService(string dbConnectionStr) : this()
        {
            genericDA.SetConnectionString(dbConnectionStr);
        }

        public DataSet RetrieveTeamCodeAndNameBySeries(String UserID, bool tSeries)
        {
            DataSet ds = new DataSet();

            try
            {
                genericDA.OpenConnection();

                if (tSeries)
                {
                    ds = commonServiceDA.RetrieveTeamCodeAndNameByTSeries(UserID);
                }
                else
                {
                    ds = commonServiceDA.RetrieveTeamCodeAndNameByNonTSeries(UserID);
                }
                if (ds.Tables[0].Rows.Count == 0)
                {
                    returnCode = "0";
                    returnMessage = "Team records Not Found!";
                }
            }
            catch (Exception e)
            {
                returnCode = "-1";
                returnMessage = "Error in retrieving Team records!";
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

        public DataSet RetrieveTeamAndNameByDealerCode(string dealerCode)
        {
            DataSet ds = new DataSet();

            try
            {
                genericDA.OpenConnection();

                ds = commonServiceDA.RetrieveTeamAndNameByDealerCode(dealerCode);
                if (ds.Tables[0].Rows.Count == 0)
                {
                    returnCode = "0";
                    returnMessage = "Team records Not Found!";
                }
            }
            catch (Exception e)
            {
                returnCode = "-1";
                returnMessage = "Error in retrieving Team records!";
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


        public DataSet RetrieveTeamCodeAndName()
        {
            DataSet ds = new DataSet();
            
            try
            {                                
                genericDA.OpenConnection();

                ds = commonServiceDA.RetrieveTeamCodeAndName();
                if (ds.Tables[0].Rows.Count == 0)
                {                    
                    returnCode = "0";
                    returnMessage = "Team records Not Found!";
                }
            }
            catch (Exception e)
            {
                returnCode = "-1";
                returnMessage = "Error in retrieving Team records!";
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

        public DataSet RetrieveAllTeamCodeAndName(String UserID)
        {
            DataSet ds = new DataSet();

            try
            {
                genericDA.OpenConnection();

                //ds = commonServiceDA.RetrieveTeamCodeAndName();

                ds.Tables.Add(commonServiceDA.RetrieveAllTeamCodeAndName(UserID));
                if (ds.Tables[0].Rows.Count == 0)
                {
                    returnCode = "0";
                    returnMessage = "Team records Not Found!";
                }
            }
            catch (Exception e)
            {
                returnCode = "-1";
                returnMessage = "Error in retrieving Team records!";
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

        public DataSet RetrieveDealerCodeAndNameByTeam(string teamCode)
        {
            DataSet ds = new DataSet();

            try
            {
                genericDA.OpenConnection();

                ds = commonServiceDA.RetrieveDealerCodeAndNameByTeam(teamCode);
                if (ds.Tables[0].Rows.Count == 0)
                {
                    returnCode = "0";
                    returnMessage = "Dealer records Not Found!";
                }
            }
            catch (Exception e)
            {
                returnCode = "-1";
                returnMessage = "Error in retrieving Dealer records!";
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

        public DataSet RetrieveDealerCodeAndNameByTeamNLoginID(string teamCode, string loginid)
        {
            DataSet ds = new DataSet();

            try
            {
                genericDA.OpenConnection();

                ds = commonServiceDA.RetrieveDealerCodeAndNameByTeamNLoginID(teamCode, loginid);
                if (ds.Tables[0].Rows.Count == 0)
                {
                    returnCode = "0";
                    returnMessage = "Dealer records Not Found!";
                }
            }
            catch (Exception e)
            {
                returnCode = "-1";
                returnMessage = "Error in retrieving Dealer records!";
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
            DataTable dtResult = null;
            try
            {
                genericDA.OpenConnection();

                dtResult = commonServiceDA.RetrieveAllDealer(UserID);
                ds.Tables.Add(dtResult);
                if (dtResult.Rows.Count < 1)
                {
                    returnCode = "0";
                    returnMessage = "Dealer records Not Found!";
                }
            }
            catch (Exception e)
            {
                returnCode = "-1";
                returnMessage = "Error in retrieving Dealer records!";
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

        /// <Added by OC>
        public DataSet RetrieveAllDealerByUserOrSupervisor(String Param, String UserID)
        {
            DataSet ds = new DataSet();
            DataTable dtResult = null;
            try
            {
                genericDA.OpenConnection();

                dtResult = commonServiceDA.RetrieveAllDealerByUserOrSupervisor(Param, UserID);
                ds.Tables.Add(dtResult);
                if (dtResult.Rows.Count < 1)
                {
                    returnCode = "0";
                    returnMessage = "Dealer records Not Found!";
                }
            }
            catch (Exception e)
            {
                returnCode = "-1";
                returnMessage = "Error in retrieving Dealer records!";
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

        public DataSet RetrievePreferenceCodeAndName()
        {
            DataSet ds = new DataSet();
            DataTable dtResult = null;
            try
            {
                genericDA.OpenConnection();

                dtResult = commonServiceDA.RetrievePreferenceCodeAndName();
                ds.Tables.Add(dtResult);
                if (dtResult.Rows.Count == 0)
                {
                    returnCode = "0";
                    returnMessage = "Preference records Not Found!";
                }                
            }
            catch (Exception e)
            {
                returnCode = "-1";
                returnMessage = "Error in retrieving Preference records!";
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

        /**  for SPM III
         * Add by   Yin Mon Win
         * Date     15 September 2011
         * */

        public DataSet   getAccessRight(string pageFunction, string userId)
        {
            DataSet ds = new DataSet();
            DataTable  dt = new DataTable();
            try
            {
                genericDA.OpenConnection();

                dt = commonServiceDA.getAccessRight(pageFunction,userId);
                if (dt.Rows.Count == 0)
                {
                    returnCode = "0";
                    returnMessage = "AccessRight record Not Found!";
                }
                else
                {
                    ds.Tables.Add(dt);
                }
            }
            catch (Exception e)
            {
                returnCode = "-1";
                returnMessage = "Error in retrieving Dealer records!";
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

        public DataSet RetrieveDealerCodeAndNameByUserID(string userId)
        {
            DataSet ds = new DataSet();

            try
            {
                genericDA.OpenConnection();

                ds = commonServiceDA.RetrieveDealerCodeAndNameByUserID(userId);
                if (ds.Tables[0].Rows.Count == 0)
                {
                    returnCode = "0";
                    returnMessage = "Dealer records Not Found!";
                }
            }
            catch (Exception e)
            {
                returnCode = "-1";
                returnMessage = "Error in retrieving Dealer records!";
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

        public DataSet RetrieveMultipleDealerCodeAndNameByTeam(string teamCode)
        {
            DataSet ds = new DataSet();

            try
            {
                genericDA.OpenConnection();

                ds = commonServiceDA.RetrieveMultipleDealerCodeAndNameByTeam(teamCode);
                if (ds.Tables[0].Rows.Count == 0)
                {
                    returnCode = "0";
                    returnMessage = "Dealer records Not Found!";
                }
            }
            catch (Exception e)
            {
                returnCode = "-1";
                returnMessage = "Error in retrieving Dealer records!";
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

        public DataSet RetrieveProjectByUserId(String userId, String filterByProjectName)
        {
            DataSet ds = new DataSet();
            try
            {
                genericDA.OpenConnection();
                ds = commonServiceDA.RetrieveProjectByUserId(userId, filterByProjectName);
                if (ds.Tables[0].Rows.Count == 0)
                {
                    returnCode = "0";
                    returnMessage = "Projects not found!";
                }
            }
            catch (Exception e)
            {
                returnCode = "-1";
                returnMessage = "Error in retrieving Projects! Exception: " + e.ToString();
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
        public DataSet RetrieveAllProjectByProjectName(string Pn)
        {
            DataSet ds = new DataSet();

            try
            {
                genericDA.OpenConnection();
                ds.Tables.Add(commonServiceDA.RetrieveAllProjectByProjectName(Pn));
            

                if (ds.Tables[0].Rows.Count == 0)
                {
                    returnCode = "0";
                    returnMessage = "Project records Not Found!";
                }
            }
            catch (Exception e)
            {
                returnCode = "-1";
                returnMessage = "Error in retrieving project records!";
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

        /**************End TSM**************/

        /// <Added by OC>
        public DataSet RetrieveAllProjectByProjectNameByUserOrSupervisor(String Param, String UserID)
        {
            DataSet ds = new DataSet();

            try
            {
                genericDA.OpenConnection();
                ds.Tables.Add(commonServiceDA.RetrieveAllProjectByProjectNameByUserOrSupervisor(Param, UserID));


                if (ds.Tables[0].Rows.Count == 0)
                {
                    returnCode = "0";
                    returnMessage = "Project records Not Found!";
                }
            }
            catch (Exception e)
            {
                returnCode = "-1";
                returnMessage = "Error in retrieving project records!";
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

        internal DataSet RetrieveTotalCommissionForUserId(String userId)
        {
            DataSet ds = new DataSet();

            try
            {
                genericDA.OpenConnection();
                ds = commonServiceDA.RetrieveTotalCommissionForUserId(userId);                
                if (ds.Tables[0].Rows.Count == 0)
                {
                    returnCode = "0";
                    returnMessage = "No Records Found for Commission!";
                }
            }
            catch (Exception e)
            {
                returnCode = "-1";
                returnMessage = "Error in retrieving total commission for user";
            }
            finally
            {                
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

        internal DataSet RetrieveProjectTotalCommByProjectId(string projectId)
        {
            DataSet ds = new DataSet();
            returnCode = "1";
            returnMessage = "";
            try
            {
                genericDA.OpenConnection();
                ds = commonServiceDA.RetrieveProjectTotalCommByProjectId(projectId);
                if (ds.Tables[0].Rows.Count == 0)
                {
                    returnCode = "0";
                    returnMessage = "No Records found for the project";
                }
            }
            catch (Exception e)
            {
                returnCode = "-1";
                returnMessage = "Error in retrieving total commission for the project";
            }
            finally
            {
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
    }
}
