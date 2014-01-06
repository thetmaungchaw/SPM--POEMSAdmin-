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
    public class PreferListService
    {
        private SPMSetupService.SPMSetup  spmSetupService;
        private string dbConnStr;
        private DataTable dtReturn;

        public PreferListService()
        {
            spmSetupService = new SPMWebApp.SPMSetupService.SPMSetup();
        }

        public PreferListService(string cnStr) : this()
        {
            dbConnStr = cnStr;
        }

        public DataSet RetrievePerferList(string optionNo, string optionContent)
        {
            DataSet ds = new DataSet();
            try
            {
                ds = spmSetupService.RetrievePerferList(optionNo, optionContent, dbConnStr);
            }
            catch (Exception e)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                ds.Tables.Add(dtReturn);
            }

            return ds;
        }

        public string[] UpdatePreferList(long recId, string optionNo, string optionContent)
        {
            string[] wsReturn = null;

            try
            {
                wsReturn = spmSetupService.UpdatePreferList(recId, optionNo, optionContent, dbConnStr);
            }
            catch (Exception e)
            {
                wsReturn = new string[] { "-3", "Error in WebService Connection!" };
            }

            return wsReturn;
        }

        public string[] DeletePreferList(long recId)
        {
            string[] wsReturn = null;

            try
            {
                wsReturn = spmSetupService.DeletePreferList(recId, dbConnStr);
            }
            catch (Exception e)
            {
                wsReturn = new string[] { "-3", "Error in WebService Connection!" };
            }

            return wsReturn;
        }

        public DataSet AddPreference(string userId, string optionNo, string optionContent)
        {
            DataSet ds = new DataSet();

            try
            {
                ds = spmSetupService.AddPreference(userId, optionNo, optionContent, dbConnStr);
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
