/* 
 * Purpose:             Access Management data service at presentation layer
 * Created By:          Li Qun
 * Date:                30/03/2010
 * 
 * Change History:
 * --------------------------------------------------------------------------------
 * Modified By          Date                Purpose
 * Yin Mon Win          15 August 2011      1. to get User Role Data
 * --------------------------------------------------------------------------------
 * 
 */

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
    public class AccessControlService
    {
        private string dbConnectionStr;
        private AccessManagementService.AccessManagementService accessManagementService;
        

        private DataTable dtReturn;


        public AccessControlService()
        {
            accessManagementService = new SPMWebApp.AccessManagementService.AccessManagementService();
            dbConnectionStr = "";
        }

        public AccessControlService(string cnStr)
            : this()
        {
            this.dbConnectionStr = cnStr;
        }

        public DataSet RetrieveUserAccessRights(string userId)
        {
            DataSet ds = new DataSet();

            try
            {
                ds = accessManagementService.RetrieveUserAccessRights(userId, dbConnectionStr);
            }
            catch (Exception e)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                ds.Tables.Add(dtReturn);
            }

            return ds;
        }

        public DataSet RetrieveUserMenuOptions(string userId)
        {
            DataSet ds = new DataSet();

            try
            {
                ds = accessManagementService.RetrieveUserMenuOptions(userId, dbConnectionStr);
            }
            catch (Exception e)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                ds.Tables.Add(dtReturn);
            }

            return ds;
        }

        public DataSet RetrieveUserIdAndName(String UserID)
        {
            DataSet ds = new DataSet();

            try
            {
                ds = accessManagementService.RetrieveUserIdAndName(UserID, dbConnectionStr);
            }
            catch (Exception e)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                ds.Tables.Add(dtReturn);
            }

            return ds;
        }

        public DataSet UpdateUserAccessRights(String userId, String roleId,DataSet userAccessRightsUpdate, String actionUserId)
        {
            DataSet ds = new DataSet();

            try
            {
               ds = accessManagementService.UpdateUserAccessRights(dbConnectionStr, userId, roleId, userAccessRightsUpdate, actionUserId);
            }
            catch (Exception e)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                ds.Tables.Add(dtReturn);
            }

            return ds;
        }

        #region for SPM III

        public DataSet RetrieveUserRoles()
        {
            DataSet ds = new DataSet();

            try
            {
               ds = accessManagementService.RetrieveUserRoles(dbConnectionStr);
            }
            catch (Exception e)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                ds.Tables.Add(dtReturn);
            }

            return ds;
        }

        public DataSet RetrieveUserRoles(string roleId)
        {
            DataSet ds = new DataSet();

            try
            {
                ds = accessManagementService.RetrieveUserRolesByRoleId(dbConnectionStr, roleId);
            }
            catch (Exception e)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                ds.Tables.Add(dtReturn);
            }

            return ds;
        }
        
        public DataSet RetrieveUserRoleRights(string roleId)
        {
            DataSet ds = new DataSet();

            try
            {
                ds = accessManagementService.RetrieveUserRoleRights(roleId, dbConnectionStr);
            }
            catch (Exception e)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                ds.Tables.Add(dtReturn);
            }

            return ds;
        }

        public DataSet RetrieveMaxUserRoleID()
        {
            DataSet   ds = new DataSet ();

            try
            {
                ds = accessManagementService.RetrieveMaxUserRoleID(dbConnectionStr);
            }
            catch (Exception e)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                ds.Tables.Add(dtReturn);

               
            }

            return ds;
        }



        public DataSet UpdateUserRole(String roleId,String roleName, String roleDes,DataSet userRoleUpdate, String actionUserId)
        {
            DataSet ds = new DataSet();

            try
            {
                ds = accessManagementService.UpdateUserRole(dbConnectionStr, roleId,roleName,roleDes, userRoleUpdate, actionUserId);
            }
            catch (Exception e)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                ds.Tables.Add(dtReturn);
            }

            return ds;
        }

        public DataSet RetrieveAssignedRole(string roleId)
        {
            DataSet ds = new DataSet();

            try
            {
               ds = accessManagementService.RetrieveAssignedRole(dbConnectionStr, roleId);
            }
            catch (Exception e)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                ds.Tables.Add(dtReturn);
            }

            return ds;
        }

        public DataSet RetrieveDealerInformation(String userId)
        {
            DataSet ds = new DataSet();
            try
            {
                ds = accessManagementService.RetrieveDealerInformation(dbConnectionStr, userId);
            }
            catch (Exception e)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebSerbice Connection!");
                ds.Tables.Add(dtReturn);
            }
            return ds;
        }

        public DataSet RetrieveRoleNames(string roleName)
        {
            DataSet ds = new DataSet();

            try
            {
                ds = accessManagementService.RetrieveRoleNames(roleName,dbConnectionStr);
            }
            catch (Exception e)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!");
                ds.Tables.Add(dtReturn);


            }

            return ds;
        }

        #endregion
    }
}
