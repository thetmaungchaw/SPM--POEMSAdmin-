﻿/* 
 * Purpose:         Access Management Webservices implementation
 * Created By:      Li Qun
 * Date:            30/04/2010
 * 
 * Change History:
 * --------------------------------------------------------------------------------
 * Modified By      Date        Purpose
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

using SPMWebServiceApp.DataAccess;
using SPMWebServiceApp.Utilities;


namespace SPMWebServiceApp.Services
{
    public class AccessControlService
    {
        private GenericDA genericDA;
        private AccesssControlServiceDA accessControlServiceDA;
        private DataTable dtReturn;

        private string returnCode;
        private string returnMessage;

        public AccessControlService()
        {
            genericDA = new GenericDA();
            accessControlServiceDA = new AccesssControlServiceDA(genericDA);

            returnCode = "1";
            returnMessage = "";
        }

        public AccessControlService(string dbConnectionStr) : this()
        {
            genericDA.SetConnectionString(dbConnectionStr);
        }

        public DataSet RetrieveUserAccessRights(string userId)
        {
            DataSet ds = new DataSet();

            try
            {
                genericDA.OpenConnection();

                ds = accessControlServiceDA.RetrieveUserAccessRights(userId);
                if (ds.Tables[0].Rows.Count == 0)
                {
                    returnCode = "0";
                    returnMessage = "User access rights record not found!";
                }
            }
            catch (Exception e)
            {
                returnCode = "-1";
                returnMessage = "Error in retrieving user access rights records!Exception:" + e.ToString();
            }
            finally
            {
                //Create Table for ReturnCode and Return Message
                dtReturn = CommonUtilities.CreateReturnTable(returnCode, returnMessage);
                ds.Tables.Add(dtReturn);

                try
                {
                    genericDA.CloseConnection();
                }
                catch (Exception e)
                { }
            }

            return ds;
        }

        public DataSet RetrieveUserMenuOptions(string userId)
        {
            DataSet ds = new DataSet();
            returnCode = "1";
            returnMessage = "";

            try
            {
                genericDA.OpenConnection();

                ds = accessControlServiceDA.RetrieveUserMenuOptions(userId);
                if (ds.Tables[0].Rows.Count == 0)
                {
                    returnCode = "-1";
                    returnMessage = "User has no functions to access!";
                }
            }
            catch (Exception e)
            {
                returnCode = "-1";
                returnMessage = "Error in retrieving user menu options! " + e.Message;
            }
            finally
            {
                //Create Table for ReturnCode and Return Message
                dtReturn = CommonUtilities.CreateReturnTable(returnCode, returnMessage);
                ds.Tables.Add(dtReturn);

                try
                {
                    genericDA.CloseConnection();
                }
                catch (Exception e)
                { }
            }

            return ds;
        }

        public DataSet RetrieveUserIdAndName(String UserID)
        {
            DataSet ds = new DataSet();
            returnCode = "1";
            returnMessage = "";

            try
            {
                genericDA.OpenConnection();

                ds = accessControlServiceDA.RetrieveUserIdAndName(UserID);
                if (ds.Tables[0].Rows.Count == 0)
                {
                    returnCode = "-1";
                    returnMessage = "No user found in the system!";
                }
            }
            catch (Exception e)
            {
                returnCode = "-1";
                returnMessage = "Error in retrieving user list!";
            }
            finally
            {
                //Create Table for ReturnCode and Return Message
                dtReturn = CommonUtilities.CreateReturnTable(returnCode, returnMessage);
                ds.Tables.Add(dtReturn);

                try
                {
                    genericDA.CloseConnection();
                }
                catch (Exception e)
                { }
            }

            return ds;
        }

        //public DataSet UpdateUserAccessRights(String userId, DataSet userAccessRightsUpdate, String actionUserId)
        //{
        //    return accessControlServiceDA.UpdateUserAccessRights(userId, userAccessRightsUpdate, actionUserId);
        //}

        /*************
        * For SPM III
        **************/

        public DataSet RetrieveDealerInformation(String userId)
        {
            DataSet ds = new DataSet();
            returnCode = "1";
            returnMessage = "";

            try
            {
                genericDA.OpenConnection();
                ds = accessControlServiceDA.RetrieveDealerInformation(userId);
                if (ds.Tables[0].Rows.Count == 0)
                {
                    returnCode = "-1";
                    returnMessage = "No user found in the system!";
                }
            }
            catch (Exception e)
            {
                returnCode = "-1";
                returnMessage = "Error in retrieving Dealer Information! Exception:" + e.ToString();
            }
            finally
            {
                try
                {
                    genericDA.CloseConnection();
                    dtReturn = CommonUtilities.CreateReturnTable(returnCode, returnMessage);
                    ds.Tables.Add(dtReturn);
                }
                catch (Exception e)
                { }
            }
            return ds;
        }

        public DataSet RetrieveRoleNames(string roleName)
        {
            DataSet ds = new DataSet();
            returnCode = "1";
            returnMessage = "";

            try
            {
                genericDA.OpenConnection();

                ds = accessControlServiceDA.RetrieveRoleNames(roleName);
                if (ds.Tables[0].Rows.Count == 0)
                {
                    //returnCode = "-1";
                    //returnMessage = "No user found in the system!";
                }
            }
            catch (Exception e)
            {
                returnCode = "-1";
                returnMessage = "Error in retrieving role list!";
            }
            finally
            {
                //Create Table for ReturnCode and Return Message
                dtReturn = CommonUtilities.CreateReturnTable(returnCode, returnMessage);
                ds.Tables.Add(dtReturn);

                try
                {
                    genericDA.CloseConnection();
                }
                catch (Exception e)
                { }
            }

            return ds;
        }

        public DataSet UpdateUserAccessRights(String userId, String roleId, DataSet userAccessRightsUpdate, String actionUserId)
        {
            return accessControlServiceDA.UpdateUserAccessRights(userId, roleId, userAccessRightsUpdate, actionUserId);
        }

        public DataSet RetrieveUserRoles()
        {
            DataSet ds = new DataSet();
            returnCode = "1";
            returnMessage = "";

            try
            {
                genericDA.OpenConnection();

                ds = accessControlServiceDA.RetrieveUserRoles();
                if (ds.Tables[0].Rows.Count == 0)
                {
                    returnCode = "-1";
                    returnMessage = "No user role found in the system!";
                }
            }
            catch (Exception e)
            {
                returnCode = "-1";
                returnMessage = "Error in retrieving user list!";
            }
            finally
            {
                //Create Table for ReturnCode and Return Message
                dtReturn = CommonUtilities.CreateReturnTable(returnCode, returnMessage);
                ds.Tables.Add(dtReturn);

                try
                {
                    genericDA.CloseConnection();
                }
                catch (Exception e)
                { }
            }

            return ds;
        }

        public DataSet RetrieveUserRoles(string roleId)
        {
            DataSet ds = new DataSet();
            returnCode = "1";
            returnMessage = "";

            try
            {
                genericDA.OpenConnection();

                ds = accessControlServiceDA.RetrieveUserRoles(roleId);
                if (ds.Tables[0].Rows.Count == 0)
                {
                    returnCode = "-1";
                    returnMessage = "No user role found in the system!";
                }
            }
            catch (Exception e)
            {
                returnCode = "-1";
                returnMessage = "Error in retrieving user list!";
            }
            finally
            {
                //Create Table for ReturnCode and Return Message
                dtReturn = CommonUtilities.CreateReturnTable(returnCode, returnMessage);
                ds.Tables.Add(dtReturn);

                try
                {
                    genericDA.CloseConnection();
                }
                catch (Exception e)
                { }
            }

            return ds;
        }

        public DataSet RetrieveUserRoleRights(string roleId)
        {
            DataSet ds = new DataSet();

            try
            {
                genericDA.OpenConnection();

                ds = accessControlServiceDA.RetrieveUserRoleRights(roleId);
                if (ds.Tables[0].Rows.Count == 0)
                {
                    returnCode = "0";
                    returnMessage = "User role rights record not found!";
                }
            }
            catch (Exception e)
            {
                returnCode = "-1";
                returnMessage = "Error in retrieving user role rights records!";
            }
            finally
            {
                //Create Table for ReturnCode and Return Message
                dtReturn = CommonUtilities.CreateReturnTable(returnCode, returnMessage);
                ds.Tables.Add(dtReturn);

                try
                {
                    genericDA.CloseConnection();
                }
                catch (Exception e)
                { }
            }

            return ds;
        }

        public DataSet RetrieveAssignedRole(string roleId)
        {
            DataSet ds = new DataSet();

            try
            {
                genericDA.OpenConnection();

                ds = accessControlServiceDA.RetrieveAssignedRole(roleId);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    returnCode = "1";
                    returnMessage = "Selected Role has been assigned.";
                }
                else if (ds.Tables[0].Rows.Count == 0)
                {
                    returnCode = "-2";
                    returnMessage = "Not Assigned Role.";
                }
            }
            catch (Exception e)
            {
                returnCode = "-1";
                returnMessage = "Error in retrieving user role rights records!";
            }
            finally
            {
                //Create Table for ReturnCode and Return Message
                dtReturn = CommonUtilities.CreateReturnTable(returnCode, returnMessage);
                ds.Tables.Add(dtReturn);

                try
                {
                    genericDA.CloseConnection();
                }
                catch (Exception e)
                { }
            }

            return ds;
        }

        public DataSet RetrieveMaxUserRoleID()
        {
            DataSet ds = new DataSet();

            try
            {
                genericDA.OpenConnection();

                ds = accessControlServiceDA.RetrieveMaxUserRoleID();
                if (ds.Tables[0].Rows.Count == 0)
                {
                    returnCode = "0";
                    returnMessage = "User role rights record not found!";
                }
            }
            catch (Exception e)
            {
                returnCode = "-1";
                returnMessage = "Error in retrieving user role rights records!";
            }
            finally
            {
                //Create Table for ReturnCode and Return Message
                dtReturn = CommonUtilities.CreateReturnTable(returnCode, returnMessage);
                ds.Tables.Add(dtReturn);

                try
                {
                    genericDA.CloseConnection();
                }
                catch (Exception e)
                { }
            }

            return ds;
        }

        public DataSet UpdateRole(String roleId, String roleName, String roleDes, DataSet userRoleUpdate, String actionUserId)
        {            
            DataSet ds = RetrieveAssignedRole(roleId);            
            if (userRoleUpdate.Tables[0].Rows[0]["Action"].ToString() == "D")
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    dtReturn = CommonUtilities.CreateReturnTable("-1", "Cannot Delete since selected role is currently assigned. ");
                    ds.Tables.Add(dtReturn);
                    return ds;
                }
                else
                {
                    return accessControlServiceDA.UpdateRole(roleId, roleName, roleDes, userRoleUpdate, actionUserId);
                }
            }
            else
            {
                return accessControlServiceDA.UpdateRole(roleId, roleName, roleDes, userRoleUpdate, actionUserId);
            }
        }
    }
}