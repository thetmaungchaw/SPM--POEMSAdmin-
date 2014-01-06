﻿using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;

using SPMWebServiceApp.Services;

namespace SPMWebServiceApp.WebServices
{
    /// <summary>
    /// Summary description for CommonSPMService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    public class CommonSPMService : System.Web.Services.WebService
    {


        [WebMethod]
        public DataSet RetrieveTeamCodeAndNameBySeries(String UserID, bool tSeries, string dbConnectionStr)
        {
            CommonService commonService = new CommonService(dbConnectionStr);
            return commonService.RetrieveTeamCodeAndNameBySeries(UserID, tSeries);
        }

        [WebMethod]
        public DataSet RetrieveTeamAndNameByDealerCode(string dealerCode, string dbConnectionStr)
        {
            CommonService commonService = new CommonService(dbConnectionStr);
            return commonService.RetrieveTeamAndNameByDealerCode(dealerCode);
        }

        [WebMethod]
        public DataSet RetrieveTeamCodeAndName(string dbConnectionStr)
        {
            CommonService commonService = new CommonService(dbConnectionStr);
            return commonService.RetrieveTeamCodeAndName();
        }

        [WebMethod]
        public DataSet RetrieveAllTeamCodeAndName(String UserID, string dbConnectionStr)
        {
            CommonService commonService = new CommonService(dbConnectionStr);
            return commonService.RetrieveAllTeamCodeAndName(UserID);
        }

        [WebMethod]
        public DataSet RetrieveDealerCodeAndNameByTeam(string teamCode, string dbConnectionStr)
        {
            CommonService commonService = new CommonService(dbConnectionStr);
            return commonService.RetrieveDealerCodeAndNameByTeam(teamCode);
        }

        [WebMethod]
        public DataSet RetrieveDealerCodeAndNameByTeamNLoginID(string teamCode, string loginid, string dbConnectionStr)
        {
            CommonService commonService = new CommonService(dbConnectionStr);
            return commonService.RetrieveDealerCodeAndNameByTeamNLoginID(teamCode, loginid);
        }

        [WebMethod]
        public DataSet RetrievePreferenceCodeAndName(string dbConnectionStr)
        {
            CommonService commonService = new CommonService(dbConnectionStr);
            return commonService.RetrievePreferenceCodeAndName();
        }

        [WebMethod]
        public DataSet RetrieveAllDealer(String UserID, string dbConnectionStr)
        {
            CommonService commonService = new CommonService(dbConnectionStr);
            return commonService.RetrieveAllDealer(UserID);
        }

        /// <Added by OC>
        [WebMethod]
        public DataSet RetrieveAllDealerByUserOrSupervisor(String Param, String UserID, String dbConnectionStr)
        {
            CommonService commonService = new CommonService(dbConnectionStr);
            return commonService.RetrieveAllDealerByUserOrSupervisor(Param, UserID);
        }

        //Development By Thiri
        [WebMethod]
        public string[] InsertEmailTemplate(string templateName, string templateType, string subject, string contents, string dbConnectionStr)
        {
            EmailTemplateService emailService = new EmailTemplateService(dbConnectionStr);
            return emailService.InsertEmailTemplate(templateName, templateType, subject, contents);
        }

        [WebMethod]
        public DataSet RetrieveEmailTemplate(string dbConnectionStr)
        {
            EmailTemplateService emailService = new EmailTemplateService(dbConnectionStr);
            return emailService.RetrieveEmailTemplate();
        }

        [WebMethod]
        public DataSet RetrieveEmailTemplateByID(string dbConnectionStr, string templateID)
        {
            EmailTemplateService emailService = new EmailTemplateService(dbConnectionStr);
            return emailService.RetrieveEmailTemplateByID(templateID);
        }

        [WebMethod]
        public string[] UpdateEmailTemplate(string templateID, string templateType, string subject, string contents, string dbConnectionStr)
        {
            EmailTemplateService emailService = new EmailTemplateService(dbConnectionStr);
            return emailService.UpdateEmailTemplateByID(templateID, templateType, subject, contents);
        }

        [WebMethod]
        public DataSet DeleteEmailTemplate(string dbConnectionStr, string templateID)
        {
            EmailTemplateService emailService = new EmailTemplateService(dbConnectionStr);
            return emailService.DeleteEmailTemplate(templateID);
        }
        //End Development By Thiri

        /*
        //for SPM III
         * Add by   Yin Mon Win
         * Date 16 September 2011
         * 
         * */

        [WebMethod]
        public DataSet getAccessRight(string pageFunction,string userId, string dbConnectionStr)
        {
            CommonService commonService = new CommonService(dbConnectionStr);
            return commonService.getAccessRight(pageFunction,userId);
        }

        [WebMethod]
        public DataSet RetrieveDealerCodeAndNameByUserID(string userId, string dbConnectionStr)
        {
            CommonService commonService = new CommonService(dbConnectionStr);
            return commonService.RetrieveDealerCodeAndNameByUserID(userId);
        }

        [WebMethod]
        public DataSet RetrieveMultipleDealerCodeAndNameByTeam(string teamCode, string dbConnectionStr)
        {
            CommonService commonService = new CommonService(dbConnectionStr);
            return commonService.RetrieveMultipleDealerCodeAndNameByTeam(teamCode);
        }

        [WebMethod]
        public DataSet RetrieveProjectByUserId(String userId, String filterByProjectName, String dbConnectionStr)
        {
            CommonService commonService = new CommonService(dbConnectionStr);
            return commonService.RetrieveProjectByUserId(userId, filterByProjectName);
        }

        [WebMethod]
        public DataSet RetrieveTotalCommissionForUserId(String dbConnectionStr, String userId)
        {
            CommonService commonService = new CommonService(dbConnectionStr);
            return commonService.RetrieveTotalCommissionForUserId(userId);
        }

        /**************Update by TSM**************/
        [WebMethod]
        public DataSet RetrieveAllProjectByProjectName(string PID, string dbConnectionStr)
        {
            CommonService commonService = new CommonService(dbConnectionStr);
            return commonService.RetrieveAllProjectByProjectName(PID);
        }

        /// <Added by OC>
        [WebMethod]
        public DataSet RetrieveAllProjectByProjectNameByUserOrSupervisor(String Param, String UserID, String dbConnectionStr)
        {
            CommonService commonService = new CommonService(dbConnectionStr);
            return commonService.RetrieveAllProjectByProjectNameByUserOrSupervisor(Param, UserID);
        }

        [WebMethod]
        public DataSet RetrieveProjectTotalCommByProjectId(String dbConnectionStr, String projectId)
        {
            CommonService commonService = new CommonService(dbConnectionStr);
            return commonService.RetrieveProjectTotalCommByProjectId(projectId);
        }
    }
}
