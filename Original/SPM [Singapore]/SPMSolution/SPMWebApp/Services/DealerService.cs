using System;
using System.Collections;
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
    public class DealerService
    {
        private SPMSetupService.SPMSetup spmSetupService;
        private string dbConnStr;
        private DataTable dtReturn;

        public DealerService()
        {
            spmSetupService = new SPMWebApp.SPMSetupService.SPMSetup();
            dbConnStr = "";
        }

        public DealerService(string cnStr) : this()
        {
            dbConnStr = cnStr;
        }

        public DataSet RetrieveAllDealer()
        {
            DataSet ds = null;

            try
            {
                ds = spmSetupService.RetrieveAllDealer(dbConnStr);
            }
            catch (Exception e)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                ds.Tables.Add(dtReturn);
            }

            return ds;
        }

        public DataSet RetrieveDealerByCriteria(string emailID, string dealerCode, string dealerName, string teamCode, string atsLogin, int enable,
                    string crossGroup, string supervisior, string dealerEmailID, String UserID)
        {
            DataSet ds = null;

            try
            {
                ds = spmSetupService.RetrieveDealerByCriteria(emailID, dealerCode, dealerName, teamCode, atsLogin, enable, crossGroup, supervisior, dealerEmailID, UserID, dbConnStr);
            }
            catch (Exception e)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                ds.Tables.Add(dtReturn);
            }

            return ds;
        }

        public string[] DeleteDealer(long recId)
        {
            string[] wsReturn = null;

            try
            {
                wsReturn = spmSetupService.DeleteDealer(recId, dbConnStr);
            }
            catch (Exception e)
            {
                wsReturn = new string[] { "-3", "Error in WebService Connection!" };
            }

            return wsReturn;
        }

        public string[] DeleteDealerByUserId(string userId)
        {
            string[] wsReturn = null;

            try
            {
                wsReturn = spmSetupService.DeleteDealerByUserId(userId, dbConnStr);
            }
            catch (Exception e)
            {
                wsReturn = new string[] { "-3", "Error in WebService Connection!" };
            }

            return wsReturn;
        }


        public string[] InsertDealer(string emailID, string dealerCode, string dealerName, string teamCode, string atsLogin, int enable, string crossGroup,
                        string supervisior, string modifiedUser,string dealerEmailID, string altAECode)
        {
            string[] wsReturn = null;

            try
            {
                wsReturn = spmSetupService.InsertDealer(emailID, dealerCode, dealerName, teamCode, atsLogin, enable, crossGroup, supervisior, modifiedUser,dealerEmailID,altAECode,
                                dbConnStr);
            }
            catch (Exception e)
            {
                wsReturn = new string[] { "-3", "Error in WebService Connection!" };
            }

            return wsReturn;
        }

        public string[] CheckDealerExists(string userid)
        {
            string[] wsReturn = null;

            try
            {
                wsReturn = spmSetupService.CheckDealerExists(userid, dbConnStr);
            }
            catch (Exception e)
            {
                wsReturn = new string[] { "-3", "Error in WebService Connection!" };
            }

            return wsReturn;
        }

        public string[] UpdateDealer(string recId, string emailID, string dealerCode, string dealerName, string teamCode, string atsLogin, int enable,
                        string crossGroup, string supervisior, string modifiedUser
                        , string originalDealerCode, string originalUserId,string dealerEmailID, string altAECode)
        {
            string[] wsReturn = null;

            try
            {
                wsReturn = spmSetupService.UpdateDealer(recId, emailID, dealerCode, dealerName, teamCode, atsLogin, enable, crossGroup, supervisior,
                                                modifiedUser, originalDealerCode, originalUserId,dealerEmailID, altAECode, dbConnStr);
            }
            catch (Exception e)
            {
                wsReturn = new string[] { "-3", "Error in WebService Connection!" };
            }

            return wsReturn;
        }
    }
}
