using System;
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
    public class CrossTeamDealerService
    {
        private SPMSetupService.SPMSetup spmSetupService;
        private string dbConnStr;
        private DataTable dtReturn;

        public CrossTeamDealerService()
        {
            spmSetupService = new SPMWebApp.SPMSetupService.SPMSetup();
            dbConnStr = "";
        }

        public CrossTeamDealerService(string cnStr) : this()
        {
            dbConnStr = cnStr;
        }

        public DataSet PrepareForCrossTeamSetup()
        {
            DataSet ds = null;

            try
            {
                ds = spmSetupService.PrepareForCrossTeamSetup(dbConnStr);
            }
            catch (Exception e)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                ds.Tables.Add(dtReturn);
            }

            return ds;
        }

        public DataSet RetrieveCrossTeamDealer(string dealerCode, string dealerTeam, string crossTeam)
        {
            DataSet ds = null;

            try
            {
                ds = spmSetupService.RetrieveCrossTeamDealer(dealerCode, dealerTeam, crossTeam, dbConnStr);
            }
            catch (Exception e)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                ds = new DataSet();
                ds.Tables.Add(dtReturn);
            }

            return ds;
        }

        public string[] InsertCrossTeamDealer(string dealerCode, string crossTeam, string modifiedUser)
        {
            string[] wsReturn = null;

            try
            {
                wsReturn = spmSetupService.InsertCrossTeamDealer(dealerCode, crossTeam, modifiedUser, dbConnStr);
            }
            catch (Exception e)
            {
                wsReturn = new string[] { "-3", "Error in WebService Connection!" };
            }

            return wsReturn;
        }

        public string[] DeleteCrossTeamDealer(string dealerCode)
        {
            string[] wsReturn = null;

            try
            {
                wsReturn = spmSetupService.DeleteCrossTeamDealer(dealerCode, dbConnStr);
            }
            catch (Exception e)
            {
                wsReturn = new string[] { "-3", "Error in WebService Connection!" };
            }

            return wsReturn;
        }

        public string[] UpdateCrossTeamDealer(string dealerCode, string crossTeam, string modifiedUser)
        {
            string[] wsReturn = null;

            try
            {
                wsReturn = spmSetupService.UpdateCrossTeamDealer(dealerCode, crossTeam, modifiedUser, dbConnStr);
            }
            catch (Exception e)
            {
                wsReturn = new string[] { "-3", "Error in WebService Connection!" };
            }

            return wsReturn;
        }
    }
}
