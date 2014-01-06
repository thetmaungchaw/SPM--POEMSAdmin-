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
    public class CoreClientService
    {
        private SPMSetupService.SPMSetup spmSetupService;
        private string dbConnStr;
        private DataTable dtReturn;

        public CoreClientService()
        {
            spmSetupService = new SPMWebApp.SPMSetupService.SPMSetup();
        }

        public CoreClientService(String cnStr) : this()
        {
            dbConnStr = cnStr;
        }

        public DataSet PrepareForCoreClient(String UserID)
        {
            DataSet ds = null;

            try
            {
                CommonServices.CommonSPMService commonSPMService = new SPMWebApp.CommonServices.CommonSPMService();
                ds = commonSPMService.RetrieveAllDealer(UserID, dbConnStr);
            }
            catch
            {                
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                ds = new DataSet();
                ds.Tables.Add(dtReturn);
            }

            return ds;
        }

        public DataSet RetrieveCoreClientList(string AECode, string AccNo)
        {
            DataSet ds = new DataSet();

            try
            {
                ds = spmSetupService.RetrieveCoreClientList(AECode, AccNo, dbConnStr);
            }
            catch (Exception e)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                ds.Tables.Add(dtReturn);
            }
            return ds;
        }

        public DataSet AddCoreClient(string AECode, string AccNo, string userId)
        {
            DataSet ds = new DataSet();

            try
            {
                ds = spmSetupService.AddCoreClient(AECode, AccNo, userId, dbConnStr);
            }
            catch (Exception e)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                ds.Tables.Add(dtReturn);
            }

            return ds;
        }

        public string[] DeleteCoreClient(long recId)
        {
            string[] wsReturn = null;

            try
            {
                wsReturn = spmSetupService.DeleteCoreClient(recId, dbConnStr);
            }
            catch (Exception e)
            {
                wsReturn = new string[] { "-3", "Error in WebService Connection!" };
            }

            return wsReturn;
        }
    }
}