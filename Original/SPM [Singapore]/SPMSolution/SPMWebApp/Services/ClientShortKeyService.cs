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
    public class ClientShortKeyService
    {
        private SPMSetupService.SPMSetup spmSetupService;
        private string dbConnStr;
        private DataTable dtReturn;

        public ClientShortKeyService()
        {
            spmSetupService = new SPMWebApp.SPMSetupService.SPMSetup();
            dbConnStr = "";
        }

        public ClientShortKeyService(string cnStr) : this()
        {
            dbConnStr = cnStr;
        }

        public DataSet RetrieveClientShortKey(string accountNo, string shortKey)
        {
            DataSet ds = null;

            try
            {
                ds = spmSetupService.RetrieveClientShortKey(accountNo, shortKey, dbConnStr);
            }
            catch (Exception e)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                ds = new DataSet();
                ds.Tables.Add(dtReturn);
            }

            return ds;
        }

        public string[] InsertClientShortKey(string userId, string accountNo, string shortKey)
        {
            string[] wsReturn = null;

            try
            {
                wsReturn = spmSetupService.InsertClientShortKey(userId, accountNo, shortKey, dbConnStr);
            }
            catch (Exception e)
            {
                wsReturn = new string[] { "-3", "Error in WebService Connection!" };
            }

            return wsReturn;
        }

        public string[] DeleteClientShortKey(string accountNo)
        {
            string[] wsReturn = null;

            try
            {
                wsReturn = spmSetupService.DeleteClientShortKey(accountNo, dbConnStr);
            }
            catch (Exception e)
            {
                wsReturn = new string[] { "-3", "Error in WebService Connection!" };
            }

            return wsReturn;
        }
    }
}
