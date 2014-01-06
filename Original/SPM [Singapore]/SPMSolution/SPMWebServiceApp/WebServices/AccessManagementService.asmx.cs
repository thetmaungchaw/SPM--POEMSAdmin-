﻿/* 
 * Purpose:         Access Management Webservices
 * Created By:      Li Qun
 * Date:            30/03/2010
 * 
 * Change History:
 * --------------------------------------------------------------------------------
 * Modified By      Date        Purpose
 * --------------------------------------------------------------------------------
 * 
 */

using System;
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
    /// Summary description for AccessManagementService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    public class AccessManagementService : System.Web.Services.WebService
    {

        [WebMethod]
        public DataSet RetrieveUserAccessRights(string userId, string dbConnectionStr)
        {
            AccessControlService accessControlService = new AccessControlService(dbConnectionStr);
            return accessControlService.RetrieveUserAccessRights(userId);
        }

        [WebMethod]
        public DataSet RetrieveUserMenuOptions(string userId, string dbConnectionStr)
        {
            AccessControlService accessControlService = new AccessControlService(dbConnectionStr);
            return accessControlService.RetrieveUserMenuOptions(userId);
        }

        [WebMethod]
        public DataSet RetrieveUserIdAndName(String UserID, string dbConnectionStr)
        {
            AccessControlService accessControlService = new AccessControlService(dbConnectionStr);
            return accessControlService.RetrieveUserIdAndName(UserID);
        }

        //[WebMethod]
        //public DataSet UpdateUserAccessRights(string dbConnectionStr, String userId, DataSet userAccessRightsUpdate, String actionUserId)
        //{
        //    AccessControlService accessControlService = new AccessControlService(dbConnectionStr);
        //    return accessControlService.UpdateUserAccessRights(userId, userAccessRightsUpdate, actionUserId);
        //}

        /********** For SPM III ***************
         * Add by        YMW
         * Date         
         * ************************************/

        [WebMethod]
        public DataSet RetrieveDealerInformation(String dbConnectionStr, String userId)
        {
            AccessControlService accessControlService = new AccessControlService(dbConnectionStr);
            return accessControlService.RetrieveDealerInformation(userId);
        }

        [WebMethod]
        public DataSet UpdateUserAccessRights(string dbConnectionStr, String userId, String roleId, DataSet userAccessRightsUpdate, String actionUserId)
        {
            AccessControlService accessControlService = new AccessControlService(dbConnectionStr);
            return accessControlService.UpdateUserAccessRights(userId, roleId, userAccessRightsUpdate, actionUserId);
        }

        [WebMethod]
        public DataSet RetrieveUserRoles(string dbConnectionStr)
        {
            AccessControlService accessControlService = new AccessControlService(dbConnectionStr);
            return accessControlService.RetrieveUserRoles();
        }

        [WebMethod]
        public DataSet RetrieveUserRolesByRoleId(string dbConnectionStr, String roleId)
        {
            AccessControlService accessControlService = new AccessControlService(dbConnectionStr);
            return accessControlService.RetrieveUserRoles(roleId);
        }

        [WebMethod]
        public DataSet UpdateUserRole(string dbConnectionStr, String roleId, String roleName, String roleDes, DataSet userRoleUpdate, String actionUserId)
        {
            AccessControlService accessControlService = new AccessControlService(dbConnectionStr);
            return accessControlService.UpdateRole(roleId, roleName, roleDes, userRoleUpdate, actionUserId);
        }

        [WebMethod]
        public DataSet RetrieveAssignedRole(string dbConnectionStr, String roleId)
        {
            AccessControlService accessControlService = new AccessControlService(dbConnectionStr);
            return accessControlService.RetrieveAssignedRole(roleId);
        }

        [WebMethod]
        public DataSet RetrieveUserRoleRights(string roleId, string dbConnectionStr)
        {
            AccessControlService accessControlService = new AccessControlService(dbConnectionStr);
            return accessControlService.RetrieveUserRoleRights(roleId);
        }

        [WebMethod]
        public DataSet RetrieveMaxUserRoleID(string dbConnectionStr)
        {
            AccessControlService accessControlService = new AccessControlService(dbConnectionStr);
            return accessControlService.RetrieveMaxUserRoleID();
        }

         
        /**************Update by TSM**************/
        // To bind the ProjName DDl
        [WebMethod]
        public DataSet RetrieveProjectByProjectName(string projName, String UserID, string dbConnectionStr)
        {
            ClientAssignmentService clientAssignmentService = new ClientAssignmentService(dbConnectionStr);
            return clientAssignmentService.RetrieveProjectByProjectName(projName, UserID);
        }
        /**************End TSM**************/

        [WebMethod]
        public DataSet RetrieveAssignedClientInfoByProj(string ProjID, string dbConnectionStr)
        {
            ClientAssignmentService clientAssignmentService = new ClientAssignmentService(dbConnectionStr);
            return clientAssignmentService.RetrieveAssignedClientInfoByProj(ProjID);
        }
        /**************End TSM**************/

        [WebMethod]
        public DataSet RetrieveRoleNames(string roleName, string dbConnectionStr)
        {
            AccessControlService accessControlService = new AccessControlService(dbConnectionStr);
            return accessControlService.RetrieveRoleNames(roleName);
        }
        /**************End YMW **************/
    }
}