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
    public class CommonService
    {
        private string dbConnectionStr;
        private CommonServices.CommonSPMService commonSPMService;

        private DataTable dtReturn;


        public CommonService()
        {
            commonSPMService = new SPMWebApp.CommonServices.CommonSPMService();
            dbConnectionStr = "";
        }

        public CommonService(string cnStr) : this()
        {
            this.dbConnectionStr = cnStr;
        }

        public DataSet RetrieveTeamCodeAndName()
        {
            DataSet ds = new DataSet();
            
            try
            {                
                ds = commonSPMService.RetrieveTeamCodeAndName(dbConnectionStr);
            }
            catch (Exception e)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                ds.Tables.Add(dtReturn);
            }

            return ds;
        }

        public DataSet RetrieveAllTeamCodeAndName(String UserID)
        {
            DataSet ds = new DataSet();

            try
            {
                ds = commonSPMService.RetrieveAllTeamCodeAndName(UserID, dbConnectionStr);
            }
            catch (Exception e)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                ds.Tables.Add(dtReturn);
            }

            return ds;
        }

        public DataSet RetrieveDealerCodeAndNameByTeam(string teamCode)
        {
            DataSet ds = new DataSet();

            try
            {
                ds = commonSPMService.RetrieveDealerCodeAndNameByTeam(teamCode, dbConnectionStr);
            }
            catch (Exception e)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                ds.Tables.Add(dtReturn);
            }

            return ds;
        }

        public DataSet RetrieveDealerCodeAndNameByTeamNLoginID(string teamCode, string loginid)
        {
            DataSet ds = new DataSet();

            try
            {
                ds = commonSPMService.RetrieveDealerCodeAndNameByTeamNLoginID(teamCode, loginid, dbConnectionStr);
            }
            catch (Exception e)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                ds.Tables.Add(dtReturn);
            }

            return ds;
        }

        public DataSet RetrievePreferenceCodeAndName()
        {
            DataSet ds = new DataSet();

            try
            {
                ds = commonSPMService.RetrievePreferenceCodeAndName(dbConnectionStr);
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
            DataSet ds = new DataSet();

            try
            {
                ds = commonSPMService.RetrieveAllDealer(UserID, dbConnectionStr);
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
            DataSet ds = new DataSet();

            try
            {
                ds = commonSPMService.RetrieveAllDealerByUserOrSupervisor(Param, UserID, dbConnectionStr);
            }
            catch (Exception e)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                ds.Tables.Add(dtReturn);
            }

            return ds;
        }

        public string[,] RetrieveClientRanks()
        {
            string[,] clientRanks = { { "Excellent Relationship", "1" }, { "Good Relationship", "2" }, 
                          { "Average Relationship", "3" }, { "Fair Relationship", "4" } 
                          , { "Poor Relationship", "5" }, { "No Rank", "0" }};

            return clientRanks;
        }

        /*for SPM III
         * Add by Yin Mon Win
         * Date 21 Sep 2011
         * */

        public DataSet getAccessRight(string pageFunction,string userId)
        {
            DataSet ds = new DataSet();

            try
            {
                ds = commonSPMService.getAccessRight(pageFunction,userId, dbConnectionStr);
            }
            catch (Exception e)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                ds.Tables.Add(dtReturn);
            }

            return ds;
        } 

        public DataSet RetrieveDealerCodeAndNameByUserID(string userId)
        {
            DataSet ds = new DataSet();

            try
            {
                ds = commonSPMService.RetrieveDealerCodeAndNameByUserID(userId, dbConnectionStr);
            }
            catch (Exception e)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                ds.Tables.Add(dtReturn);
            }

            return ds;
        }

        public DataSet RetrieveTeamCodAndNameBySeries(String UserID, bool tSeries)
        {
            DataSet ds = new DataSet();

            try
            {
                ds = commonSPMService.RetrieveTeamCodeAndNameBySeries(UserID, tSeries, dbConnectionStr);
            }
            catch (Exception e)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                ds.Tables.Add(dtReturn);
            } 

            return ds;
        }

        public DataSet RetrieveTeamAndNameByDealerCode(string dealerCode)
        {
            DataSet ds = new DataSet();

            try
            {
                ds = commonSPMService.RetrieveTeamAndNameByDealerCode(dealerCode, dbConnectionStr);
            }
            catch (Exception e)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                ds.Tables.Add(dtReturn);
            }

            return ds;
        }

        public DataSet RetrieveMultipleDealerCodeAndNameByTeam(string teamCode)
        {
            DataSet ds = new DataSet();

            try
            {
                ds = commonSPMService.RetrieveMultipleDealerCodeAndNameByTeam(teamCode, dbConnectionStr);
            }
            catch (Exception e)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                ds.Tables.Add(dtReturn);
            }

            return ds;
        }

        public DataSet RetrieveProjectsByUserId(String userId, String filterByProjectName)
        {
            DataSet ds = new DataSet();
            try
            {
                ds = commonSPMService.RetrieveProjectByUserId(userId, filterByProjectName, dbConnectionStr);                
            }
            catch (Exception e)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection");
                ds.Tables.Add(dtReturn);
            }
            return ds;
        }

        /**************Update by TSM**************/
        public DataSet RetrieveAllProjectByProjectName(string projName)
        {
            DataSet ds = new DataSet();

            try
            {
                ds = commonSPMService.RetrieveAllProjectByProjectName(projName, dbConnectionStr);
            }
            catch (Exception e)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                ds.Tables.Add(dtReturn);
            }

            return ds;
        }

        /// <Added by OC>
        public DataSet RetrieveAllProjectByProjectNameByUserOrSupervisor(String Param, String UserID)
        {
            DataSet ds = new DataSet();

            try
            {
                ds = commonSPMService.RetrieveAllProjectByProjectNameByUserOrSupervisor(Param, UserID, dbConnectionStr);
            }
            catch (Exception e)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                ds.Tables.Add(dtReturn);
            }

            return ds;
        }

        internal DataSet RetrieveTotalCommissionForUser(string userId)
        {
            DataSet ds = new DataSet();
            try
            {
                ds = commonSPMService.RetrieveTotalCommissionForUserId(dbConnectionStr, userId);                
            }
            catch (Exception e)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                ds.Tables.Add(dtReturn);
            }

            return ds;
        }

        internal DataSet RetrieveProjectTotalCommByProjectId(string projectId)
        {
            DataSet ds = new DataSet();
            try
            {
                ds = commonSPMService.RetrieveProjectTotalCommByProjectId(dbConnectionStr, projectId);
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