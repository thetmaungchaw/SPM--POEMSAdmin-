using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using SPMWebApp.Utilities;

namespace SPMWebApp.Services
{
    public class EmailTemplateService
    {
        private CommonServices.CommonSPMService emailTemplateService;
        private string dbConnectionStr;
        private DataTable dtReturn;

        public EmailTemplateService()
        {
            emailTemplateService = new SPMWebApp.CommonServices.CommonSPMService();
            dbConnectionStr = "";
        }

        public EmailTemplateService(string dbConnectionStr)
            : this()
        {
            this.dbConnectionStr = dbConnectionStr;
        }

        public string[] InsertEmailTemplate(string templateName, string templateType, string subject, string contents, string status)
        {
            string[] wsReturn = null;
            try
            {
                wsReturn = emailTemplateService.InsertEmailTemplate(templateName, templateType, subject, contents, dbConnectionStr);
            }
            catch (Exception e)
            {
                wsReturn = new string[] { "-3", "Error in WebService Connection!" };
            }

            return wsReturn;
        }

        public DataSet RetrieveEmailTemplate()
        {
            DataSet ds = new DataSet();
            try
            {
                ds = emailTemplateService.RetrieveEmailTemplate(dbConnectionStr);
            }
            catch (Exception e)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                ds.Tables.Add(dtReturn);
            }

            return ds;
        }

        public DataSet RetrieveEmailTemplateByID(string templateID)
        {
            DataSet ds = new DataSet();
            try
            {
                ds = emailTemplateService.RetrieveEmailTemplateByID(dbConnectionStr, templateID);
            }
            catch (Exception e)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                ds.Tables.Add(dtReturn);
            }

            return ds;
        }

        public string[] UpdateEmailTemplate(string templateID, string templateType, string subject, string contents)
        {
            string[] wsReturn = null;
            try
            {
                wsReturn = emailTemplateService.UpdateEmailTemplate(templateID, templateType, subject, contents, dbConnectionStr);
            }
            catch (Exception e)
            {
                wsReturn = new string[] { "-3", "Error in WebService Connection!" };
            }

            return wsReturn;
        }

        public DataSet DeleteTemplate(string templateID)
        {
            DataSet ds = null;
            try
            {
                ds = emailTemplateService.DeleteEmailTemplate(dbConnectionStr, templateID);
            }
            catch (Exception e)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                ds = new DataSet();
                ds.Tables.Add(dtReturn);
            }
            return ds;
        }
    }
}
