using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;

using SPMWebServiceApp.Services;

namespace SPMWebServiceApp.WebServices
{
    /// <summary>
    /// Summary description for SPMSetup
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    public class SPMSetup : System.Web.Services.WebService
    {

        /* Web Services for Core Client functions
          * 
          */
        [WebMethod]
        public DataSet RetrieveCoreClientList(string AECode, string AccNo, string dbConnStr)
        {
            DataSet ds = null;
            CoreClientService coreClientService = new CoreClientService(dbConnStr);

            ds = coreClientService.RetrieveCoreClientList(AECode, AccNo);
            return ds;
        }

        [WebMethod]
        public DataSet AddCoreClient(string AECode, string AccNo, string userId, string dbConnStr)
        {
            DataSet ds = new DataSet();
            CoreClientService coreClientService = new CoreClientService(dbConnStr);

            ds = coreClientService.AddCoreClient(AECode, AccNo, userId);
            return ds;
        }

        [WebMethod]
        public string[] DeleteCoreClient(long recId, string dbConnStr)
        {
            CoreClientService coreClientService = new CoreClientService(dbConnStr);
            return coreClientService.DeleteCoreClient(recId);
        }


        /* Web Services for PreferList functions
         * 
         */
        [WebMethod]
        public DataSet RetrievePerferList(string optionNo, string optionContent, string dbConnStr)
        {
            PreferListService preferListService = new PreferListService(dbConnStr);
            return preferListService.RetrievePerferList(optionNo, optionContent);
        }

        [WebMethod]
        public string[] UpdatePreferList(long recId, string optionNo, string optionContent, string dbConnStr)
        {
            PreferListService preferListService = new PreferListService(dbConnStr);
            return preferListService.UpdatePreferList(recId, optionNo, optionContent);
        }

        [WebMethod]
        public string[] DeletePreferList(long recId, string dbConnStr)
        {
            PreferListService preferListService = new PreferListService(dbConnStr);
            return preferListService.DeletePreferList(recId);
        }

        [WebMethod]
        public DataSet AddPreference(string userId, string optionNo, string optionContent, string dbConnStr)
        {
            PreferListService preferListService = new PreferListService(dbConnStr);
            return preferListService.AddPreference(userId, optionNo, optionContent);
        }

        /* 
         * Web Services for Dealer Management
         */
        [WebMethod]
        public DataSet RetrieveAllDealer(string dbConnStr)
        {
            DealerService dealerService = new DealerService(dbConnStr);
            return dealerService.RetrieveAllDealer();
        }

        [WebMethod]
        public string[] DeleteDealer(long recId, string dbConnStr)
        {
            DealerService dealerService = new DealerService(dbConnStr);
            return dealerService.DeleteDealer(recId);
        }

        [WebMethod]
        public string[] DeleteDealerByUserId(string userId, string dbConnStr)
        {
            DealerService dealerService = new DealerService(dbConnStr);
            return dealerService.DeleteDealerByUserId(userId);
        }

        [WebMethod]
        public string[] InsertDealer(string emailID, string dealerCode, string dealerName, string teamCode, string atsLogin, int enable, string crossGroup,
                        string supervisior, string modifiedUser,string dealerEmailID, string altAECode, string dbConnectionStr)
        {
            DealerService dealerService = new DealerService(dbConnectionStr);
            return dealerService.InsertDealer(emailID, dealerCode, dealerName, teamCode, atsLogin, enable, crossGroup, supervisior, modifiedUser, dealerEmailID, altAECode);
        }

        [WebMethod]
        public string[] CheckDealerExists(string userid, string dbConnectionStr)
        {
            DealerService dealerService = new DealerService(dbConnectionStr);
            return dealerService.CheckDealerExists(userid);
        }       

        [WebMethod]
        public string[] UpdateDealer(string recId, string emailID, string dealerCode, string dealerName, string teamCode, string atsLogin, int enable,
                        string crossGroup, string supervisior, string modifiedUser, 
                        string originalDealerCode, string originalUserId,string dealerEmailID, string altAECode, string dbConnectionStr)
        {
            DealerService dealerService = new DealerService(dbConnectionStr);
            return dealerService.UpdateDealer(recId, emailID, dealerCode, dealerName, teamCode, atsLogin, enable,
                crossGroup, supervisior, modifiedUser, originalDealerCode, originalUserId, dealerEmailID, altAECode);
        }

        [WebMethod]
        public DataSet RetrieveDealerByCriteria(string emailID, string dealerCode, string dealerName, string teamCode, string atsLogin, int enable,
                    string crossGroup, string supervisior, string dealerEmailID, String UserID, string dbConnectionStr)
        {
            DealerService dealerService = new DealerService(dbConnectionStr);
            return dealerService.RetrieveDealerByCriteria(emailID, dealerCode, dealerName, teamCode, atsLogin, enable, crossGroup, supervisior, dealerEmailID, UserID);
        }

        /* Cross Team Setup WebServices */
        [WebMethod]
        public DataSet RetrieveCrossTeamDealer(string dealerCode, string dealerTeam, string crossTeam, string dbConnectionStr)
        {
            CrossTeamDealerService commonDealerService = new CrossTeamDealerService(dbConnectionStr);
            return commonDealerService.RetrieveCrossTeamDealer(dealerCode, dealerTeam, crossTeam);
        }

        [WebMethod]
        public DataSet PrepareForCrossTeamSetup(string dbConnectionStr)
        {
            CrossTeamDealerService commonDealerService = new CrossTeamDealerService(dbConnectionStr);
            return commonDealerService.PrepareForCrossTeamSetup();
        }

        [WebMethod]
        public string[] InsertCrossTeamDealer(string dealerCode, string crossTeam, string modifiedUser, string dbConnectionStr)
        {
            CrossTeamDealerService commonDealerService = new CrossTeamDealerService(dbConnectionStr);
            return commonDealerService.InsertCrossTeamDealer(dealerCode, crossTeam, modifiedUser);
        }

        [WebMethod]
        public string[] DeleteCrossTeamDealer(string dealerCode, string dbConnectionStr)
        {
            CrossTeamDealerService commonDealerService = new CrossTeamDealerService(dbConnectionStr);
            return commonDealerService.DeleteCrossTeamDealer(dealerCode);
        }

        [WebMethod]
        public string[] UpdateCrossTeamDealer(string dealerCode, string crossTeam, string modifiedUser, string dbConnectionStr)
        {
            CrossTeamDealerService commonDealerService = new CrossTeamDealerService(dbConnectionStr);
            return commonDealerService.UpdateCrossTeamDealer(dealerCode, crossTeam, modifiedUser);
        }

        /* Client Short Key Setup WebServices */
        [WebMethod]
        public DataSet RetrieveClientShortKey(string accountNo, string shortKey, string dbConnectionStr)
        {
            ClientShortKeyService clientShortKeyService = new ClientShortKeyService(dbConnectionStr);
            return clientShortKeyService.RetrieveClientShortKey(accountNo, shortKey);
        }

        [WebMethod]
        public string[] InsertClientShortKey(string userId, string accountNo, string shortKey, string dbConnectionStr)
        {
            ClientShortKeyService clientShortKeyService = new ClientShortKeyService(dbConnectionStr);
            return clientShortKeyService.InsertClientShortKey(userId, accountNo, shortKey);
        }

        [WebMethod]
        public string[] DeleteClientShortKey(string accountNo, string dbConnectionStr)
        {
            ClientShortKeyService clientShortKeyService = new ClientShortKeyService(dbConnectionStr);
            return clientShortKeyService.DeleteClientShortKey(accountNo);
        }

        //For SPM III
        /* 
         * Web Services for Leads Management
         */
        [WebMethod]
        public DataSet RetrieveAllLeads(string dbConnStr)
        {
            LeadsService leadsService = new LeadsService(dbConnStr);
            return leadsService.RetrieveAllLeads();
        }

        [WebMethod]
        public DataSet  DeleteLeads(string leadId,string dealerCode, string dbConnStr)
        {
            LeadsService leadsService = new LeadsService(dbConnStr);
            return leadsService.DeleteLeads(leadId,dealerCode);
        }

        [WebMethod]
        public string[] InsertLeads(string LeadID,string LeadName, string LeadNRIC, string LeadMobile, string LeadHome, string LeadGender,string LeadEmail,
            string LeadEvent, string teamCode, string dealerCode,string inputType, string dbConnectionStr)
        {
            LeadsService leadsService = new LeadsService(dbConnectionStr);
            return leadsService.InsertLeads(LeadID,LeadName,  LeadNRIC,  LeadMobile,  LeadHome,  LeadGender, LeadEmail,LeadEvent,
               teamCode, dealerCode, inputType);
        }

        [WebMethod]
        public string[] MoveToLeadArchive(string syncType, string strCondition, string dbConnectionStr)
        {
            LeadsService leadsService = new LeadsService(dbConnectionStr);
            return leadsService.MoveToLeadArchive(syncType, strCondition);
        }

        [WebMethod]
        public string[] LeadsDataSync(string syncType, string strCondition, string dbConnectionStr)
        {
            LeadsService leadsService = new LeadsService(dbConnectionStr);
            return leadsService.LeadsDataSync(syncType,strCondition);
        }

        [WebMethod]
        public string[] UpdateLeads(string LeadID, string LeadName, string LeadNRIC, string LeadMobile, string LeadHome, string LeadGender, string LeadEmail,
            string LeadEvent, string teamCode, string dealerCode, string dbConnectionStr)
           // string dealerName, string originalDealerCode, string originalUserId, string dbConnectionStr)
        {
            LeadsService leadsService = new LeadsService(dbConnectionStr);
            return leadsService.UpdateLeads(LeadID, LeadName, LeadNRIC, LeadMobile, LeadHome, LeadGender, LeadEmail, LeadEvent, teamCode, dealerCode);//, dealerName);
                //originalDealerCode, originalUserId);
        }

        [WebMethod]
        public DataSet RetrieveLeadsByCriteria(string leadName, string leadNRIC, string leadMobile, string leadHome, string leadGender, string leadEmail, string teamCode, string dealerCode, string dealerName, string dbConnectionStr)
        {
            LeadsService leadsService = new LeadsService(dbConnectionStr);
            return leadsService.RetrieveLeadsByCriteria(leadName, leadNRIC,leadMobile,  leadHome,  leadGender,  leadEmail, teamCode,  dealerCode, dealerName);
        }

        [WebMethod]
        public DataSet RetrieveMaxLeadsID(string dbConnectionStr)
        {
            LeadsService leadsService = new LeadsService(dbConnectionStr);
            return leadsService.RetrieveMaxLeadsID();
        }


         [WebMethod]
        public DataSet  CheckExistingLead(string LeadName, string LeadMobile, string LeadNRIC, string dbConnectionStr)
        {
            LeadsService leadsService = new LeadsService(dbConnectionStr);
            return leadsService.CheckExistingLead(LeadName, LeadMobile, LeadNRIC);
        }

         [WebMethod]
         public DataSet RetrieveExistingLeadsInfo(string LeadName, string LeadNRIC, string dbConnectionStr)
         {
             LeadsService leadsService = new LeadsService(dbConnectionStr);
             return leadsService.RetrieveExistingLeadsInfo(LeadName, LeadNRIC);
         }
        
    }
}
