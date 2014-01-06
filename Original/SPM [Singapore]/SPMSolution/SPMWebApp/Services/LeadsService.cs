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
    public class LeadsService
    {
        private SPMSetupService.SPMSetup spmSetupService;
        private string dbConnStr;
        private DataTable dtReturn;

        public LeadsService()
        {
            spmSetupService = new SPMWebApp.SPMSetupService.SPMSetup();
            dbConnStr = "";
        }

        public LeadsService(string cnStr) : this()
        {
            dbConnStr = cnStr;
        }

        public DataSet RetrieveAllLeads()
        {
            DataSet ds = null;

            try
            {
                ds = spmSetupService.RetrieveAllLeads(dbConnStr);
            }
            catch (Exception e)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                ds.Tables.Add(dtReturn);
            }

            return ds;
        }

        public DataSet RetrieveLeadsByCriteria(string leadName, string leadNRIC, string leadMobile, string leadHome, string leadGender, string leadEmail, string teamCode, string dealerCode, string dealerName)
        {
            DataSet ds = null;

            try
            {
                ds = spmSetupService.RetrieveLeadsByCriteria(leadName, leadNRIC,leadMobile,  leadHome,  leadGender,  leadEmail, teamCode,  dealerCode, dealerName,dbConnStr);
            }
            catch (Exception e)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                ds.Tables.Add(dtReturn);
            }

            return ds;
        }

        public DataSet  DeleteLeads(string leadId,string dealerCode)
        {
            DataSet ds = null;

            try
            {
                ds = spmSetupService.DeleteLeads(leadId, dealerCode, dbConnStr );
            }
            catch (Exception e)
            {
                //wsReturn = new string[] { "-3", "Error in WebService Connection!" };
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                ds = new DataSet();
                ds.Tables.Add(dtReturn);
            }

            return ds;

            
        }

        public string[] InsertLeads(string LeadID,string LeadName, string LeadNRIC, string LeadMobile, string LeadHome, string LeadGender, string LeadEmail,
            string LeadEvent,string teamCode, string dealerCode,string inputType)
        {
            string[] wsReturn = null;

            try
            {
                wsReturn = spmSetupService.InsertLeads(LeadID,LeadName, LeadNRIC,  LeadMobile,  LeadHome,  LeadGender,  LeadEmail,LeadEvent,
             teamCode,  dealerCode,inputType ,  dbConnStr);
            }
            catch (Exception e)
            {
                wsReturn = new string[] { "-3", "Error in WebService Connection!" };
            }

            return wsReturn;
        }


        public string[] MoveToLeadArchive(string syncType, string strCondition)
        {
            string[] wsReturn = null;

            try
            {
                wsReturn = spmSetupService.MoveToLeadArchive(syncType, strCondition, dbConnStr);
            }
            catch (Exception e)
            {
                wsReturn = new string[] { "-3", "Error in WebService Connection!" };
            }

            return wsReturn;
        }

        public string[] LeadsDataSync(string syncType, string strCondition)
        {
            string[] wsReturn = null;

            try
            {
                wsReturn = spmSetupService.LeadsDataSync(syncType, strCondition, dbConnStr);
            }
            catch (Exception e)
            {
                wsReturn = new string[] { "-3", "Error in WebService Connection!" };
            }

            return wsReturn;
        }


        public DataSet RetrieveMaxLeadsID()
        {
            DataSet ds = new DataSet();

            try
            {
                ds = spmSetupService.RetrieveMaxLeadsID(dbConnStr);
            }
            catch (Exception e)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                ds.Tables.Add(dtReturn);
            }

            return ds;
        }


        public DataSet CheckExistingLead(string LeadName, string LeadMobile, string LeadNRIC)
        {
            DataSet ds = new DataSet();

            try
            {
                ds = spmSetupService.CheckExistingLead(LeadName, LeadMobile, LeadNRIC, dbConnStr);
            }
            catch (Exception e)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                ds.Tables.Add(dtReturn);
            }

            return ds;
        }

        public DataSet RetrieveExistingLeadsInfo(string LeadName, string LeadNRIC)
        {
            DataSet ds = new DataSet();

            try
            {
                ds = spmSetupService.RetrieveExistingLeadsInfo(LeadName, LeadNRIC, dbConnStr);
            }
            catch (Exception e)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                ds.Tables.Add(dtReturn);
            }

            return ds;
        }

        public string[] UpdateLeads(string LeadID,string LeadName, string LeadNRIC, string LeadMobile, string LeadHome, string LeadGender, string LeadEmail,
            string LeadEvent,  string teamCode, string dealerCode)//, string dealerName , string originalDealerCode, string originalUserId)
        {
            string[] wsReturn = null;

            try
            {
                wsReturn = spmSetupService.UpdateLeads(LeadID, LeadName, LeadNRIC, LeadMobile, LeadHome, LeadGender, LeadEmail, LeadEvent, teamCode, dealerCode,  dbConnStr);//,originalDealerCode, originalUserId, dbConnStr);
            }
            catch (Exception e)
            {
                wsReturn = new string[] { "-3", "Error in WebService Connection!" };
            }

            return wsReturn;
        }
    }
}
