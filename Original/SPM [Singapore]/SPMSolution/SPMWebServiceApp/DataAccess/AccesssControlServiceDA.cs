﻿/* 
 * Purpose:         Access Management Webservices data access layer
 * Created By:      Li Qun
 * Date:            30/03/2010
 * 
 * Change History:
 * --------------------------------------------------------------------------------
 * Modified By      Date        Purpose
 * --------------------------------------------------------------------------------
 * Li Qun           05/04/2010  Add in audit trail
 * Li Qun           15/04/2010  Change to call SP to insert audit trail
 * 
 */
using System;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using SPMWebServiceApp.Utilities;
using System.Collections.Generic;
using SPMWebServiceApp.DataObject;

namespace SPMWebServiceApp.DataAccess
{
    public class AccesssControlServiceDA
    {
        private GenericDA genericDA;
        private string sql;

        public AccesssControlServiceDA(GenericDA da)
        {
            genericDA = da;
        }

        public DataSet RetrieveUserAccessRights(string userId)
        {
            //by Than Htike
            //sql = "SELECT FM.Function_Code, FM.Function_Desc, FM.Category, CASE WHEN UA.UserRight = 'A' THEN 'TRUE' ELSE 'FALSE' END AS UserRight " +
            //      "FROM  dbo.FunctionURL AS FM LEFT OUTER JOIN " +
            //      "dbo.AccessRight AS UA ON FM.Function_Code = UA.Functions " +
            //      "AND UA.UserID = '" + userId + "'" +
            //      "ORDER BY FM.Category";

            //modified for SPM III

            sql = "SELECT FM.Function_Code, FM.Function_Desc, FM.Category, " +
                      "CASE WHEN AR.CreateRight ='Y' THEN 'TRUE' ELSE 'FALSE' END AS CreateRight, " +
                      "CASE WHEN AR.ViewRight = 'Y' THEN 'TRUE' ELSE 'FALSE' END AS ViewRight, " +
                      "CASE WHEN AR.ModifyRight ='Y' THEN 'TRUE' ELSE 'FALSE' END AS ModifyRight, " +
                      "CASE WHEN AR.DeleteRight ='Y' THEN 'TRUE' ELSE 'FALSE' END AS DeleteRight, " +
                      "AR.RoleID, AR.UserRight, " +
                      "CASE WHEN FM.CreateRight ='D' THEN 'D' END AS hidCreate, " +
                      "CASE WHEN FM.ViewRight = 'D' THEN 'D'  END AS hidView, " +
                      "CASE WHEN FM.ModifyRight ='D' THEN 'D' END AS hidModify, " +
                      "CASE WHEN FM.DeleteRight ='D' THEN 'D' END AS hidDelete " +
                      "FROM  dbo.FunctionURL AS FM JOIN " +
                      "dbo.AccessRight AS AR ON FM.Function_Code = AR.Functions " +
                      "WHERE UserID='" + userId + "'" +
                      "ORDER BY FM.Category";


            return genericDA.ExecuteQuery(sql);
        }

        public DataSet RetrieveUserMenuOptions(string userId)
        {
            sql = "SELECT FUN.Function_Code, FUN.Function_Desc, FUN.URL " +
                    "FROM dbo.FunctionURL AS FUN " +
                    "INNER JOIN dbo.AccessRight AS UA ON FUN.Function_Code = UA.Functions " +
                    "AND UA.UserRight = 'Y' AND UA.UserID = '" + userId + "' ORDER BY FUN.Function_DESC";


            return genericDA.ExecuteQuery(sql);
        }

        public DataSet RetrieveUserIdAndName(String UserID)
        {
            sql = "SELECT UserID, RTRIM(AEName) + ' - ' + UserID AS UserIdAndName " +
                  "FROM DealerDetail WHERE UserType NOT LIKE 'FAR%' AND AEGroup IN (SELECT AEGroup FROM SuperAdmin WHERE UserID='" + UserID + "') GROUP BY UserID, AEName " +
                  "ORDER BY AEName";

            return genericDA.ExecuteQuery(sql);
        }

        /*
        public DataSet UpdateUserAccessRights(String userId, DataSet userAccessRightsUpdate, String actionUserId)
        {
            String returnCode = "1";
            String returnMessage = "";
            DataSet dsReturn = new DataSet();
            DataTable dtReturn = new DataTable();

            if (userAccessRightsUpdate == null || userAccessRightsUpdate.Tables.Count ==0 || userAccessRightsUpdate.Tables[0].Rows.Count == 0)
            {
                returnCode = "-1";
                returnMessage = "No record to update!";
                dtReturn = CommonUtilities.CreateReturnTable(returnCode, returnMessage);
                dsReturn.Tables.Add(dtReturn);
                return dsReturn;
            }

            //Parameters for AuditLog
            OleDbParameter[] sqlAuditLogParams = new OleDbParameter[] 
                                        { 
                                            new OleDbParameter("@istrUserId", OleDbType.VarChar),   
                                            new OleDbParameter("@istrFunctionCode", OleDbType.VarChar),
                                            new OleDbParameter("@istrActionCode", OleDbType.Char),
                                            new OleDbParameter("@istrActionBy", OleDbType.VarChar)
                                        };

            OleDbCommand cmd = null;
            OleDbCommand cmdAuditLog = null;
            OleDbTransaction sqlTransaction = null;

            try
            {
                genericDA.OpenConnection();
                //Start Transaction
                sqlTransaction = genericDA.GetSqlConnection().BeginTransaction();
                genericDA.CreateSqlCommand();
                cmd = genericDA.GetSqlCommand();
                cmd.Transaction = sqlTransaction;

                //Create AuditLog command
                cmdAuditLog = genericDA.GetNewSqlCommand();
                cmdAuditLog.Transaction = sqlTransaction;
                cmdAuditLog.CommandType = CommandType.StoredProcedure;
                cmdAuditLog.CommandText = "SPM_spAccessRightAuditIns";
                cmdAuditLog.Parameters.AddRange(sqlAuditLogParams);


                foreach (DataRow dr in userAccessRightsUpdate.Tables[0].Rows)
                {
                    cmdAuditLog.Parameters["@istrUserId"].Value = userId;
                    cmdAuditLog.Parameters["@istrFunctionCode"].Value = dr["Function_Code"].ToString();
                    cmdAuditLog.Parameters["@istrActionCode"].Value = dr["Action"].ToString();
                    cmdAuditLog.Parameters["@istrActionBy"].Value = actionUserId;
                    switch (dr["Action"].ToString())
                    {
                        case "D":
                            {
                                // Delete current access right records
                                sql = "DELETE FROM dbo.AccessRight WHERE UserID = '" + userId + "' AND Functions = '" + dr["Function_Code"].ToString() + "'";
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();

                                // Insert into audit trail
                                //sql = "INSERT INTO dbo.AccessRightAudit VALUES('" + userId + "', '" + dr["Function_Code"].ToString() + "', 'D', '" + actionUserId + "', current_timestamp)";
                                //cmd.CommandText = sql;
                                //cmd.ExecuteNonQuery();

                                cmdAuditLog.ExecuteNonQuery();

                                break;
                            }
                        case "A":
                            {
                                sql = "INSERT INTO dbo.AccessRight VALUES('" + userId + "', 'SP', '" + dr["Function_Code"].ToString() + "', 'A')";
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();

                                // Insert into audit trail
                                //sql = "INSERT INTO dbo.AccessRightAudit VALUES('" + userId + "', '" + dr["Function_Code"].ToString() + "', 'A', '" + actionUserId + "', current_timestamp)";
                                //cmd.CommandText = sql;
                                //cmd.ExecuteNonQuery();

                                cmdAuditLog.ExecuteNonQuery();

                                break;
                            }
                        default:
                            {
                                returnCode = "-1";
                                returnMessage = "Invalid action code: " + dr["Action"].ToString() + " !";
                                break;
                            }
                    }

                    if (returnCode == "-1")
                        break;
                }

                if (returnCode == "1")
                {
                    sqlTransaction.Commit();
                    returnCode = "1";
                    returnMessage = "User Access Rights are successfully updated!";
                }
                else
                {
                    sqlTransaction.Rollback();
                }

            } 
            catch (Exception e)
            {
                sqlTransaction.Rollback();
                returnCode = "-1";
                returnMessage = "Error updating user access rights! " + e.Message;
            }
            finally
            {
                //Create Table for ReturnCode and Return Message
                dtReturn = CommonUtilities.CreateReturnTable(returnCode, returnMessage);
                dsReturn.Tables.Add(dtReturn);

                try
                {
                    genericDA.CloseConnection();
                }
                catch (Exception e)
                { }
            }

            return dsReturn;
        }

        */

        #region SPM III

        /// <summary>
        /// Fetching the Dealer Information: AECode, CrossGroup, Supervisor columns for each UserID
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public DataSet RetrieveDealerInformation(String userId)
        {            
            sql = "SELECT AECode, CrossGroup, Supervisor FROM dbo.DealerDetail WHERE UserID = '" + userId + "'";
            return genericDA.ExecuteQuery(sql);
        }

        public DataSet UpdateUserAccessRights(String userId, String roleId, DataSet userAccessRightsUpdate, String actionUserId)
        {
            String returnCode = "1";
            String returnMessage = "";
            DataSet dsReturn = new DataSet();
            DataTable dtReturn = new DataTable();
            DataSet dsOld = new DataSet();
            String status = "";


            if (userAccessRightsUpdate == null || userAccessRightsUpdate.Tables.Count == 0 || userAccessRightsUpdate.Tables[0].Rows.Count == 0)
            {
                returnCode = "-1";
                returnMessage = "No record to update!";
                dtReturn = CommonUtilities.CreateReturnTable(returnCode, returnMessage);
                dsReturn.Tables.Add(dtReturn);
                return dsReturn;
            }

            OleDbCommand cmdOld = null;
            try
            {

                genericDA.OpenConnection();
                genericDA.CreateSqlCommand();
                cmdOld = genericDA.GetSqlCommand();

                sql = "SELECT * FROM dbo.AccessRight WHERE UserID = '" + userId + "'";

                cmdOld.CommandText = sql;
                dsOld = genericDA.ExecuteQuery(sql);

            }
            catch (Exception e)
            {
                returnCode = "-1";
                returnMessage = "Error updating user access rights! " + e.Message;
            }

            //Parameters for AuditLog
            OleDbParameter[] sqlAuditLogParams = new OleDbParameter[] 
                                        { 
                                            new OleDbParameter("@istrUserId", OleDbType.VarChar),   
                                            new OleDbParameter("@istrFunctionCode", OleDbType.VarChar),
                                            new OleDbParameter("@istrActionCode", OleDbType.Char),
                                            new OleDbParameter("@istrActionBy", OleDbType.VarChar),

                                            /// <Updated by OC>
                                            /// <Commented 4 Parameters>
                                            //new OleDbParameter("@istrOldCreateRight",OleDbType.VarChar),
                                            //new OleDbParameter("@istrOldViewRight",OleDbType.VarChar), 
                                            //new OleDbParameter("@istrOldModifyRight", OleDbType.VarChar),
                                            //new OleDbParameter("@istrOldDeleteRight", OleDbType.VarChar),
                                            new OleDbParameter("@istrNewCreateRight", OleDbType.VarChar),
                                            new OleDbParameter("@istrNewViewRight", OleDbType.VarChar),
                                            new OleDbParameter("@istrNewModifyRight", OleDbType.VarChar),
                                            new OleDbParameter("@istrNewDeleteRight",OleDbType.VarChar)
                                        };

            OleDbCommand cmd = null;
            OleDbCommand cmdAuditLog = null;
            OleDbTransaction sqlTransaction = null;

            try
            {
                genericDA.OpenConnection();
                //Start Transaction
                sqlTransaction = genericDA.GetSqlConnection().BeginTransaction();
                genericDA.CreateSqlCommand();
                cmd = genericDA.GetSqlCommand();
                cmd.Transaction = sqlTransaction;

                //Create AuditLog command
                cmdAuditLog = genericDA.GetNewSqlCommand();
                cmdAuditLog.Transaction = sqlTransaction;
                cmdAuditLog.CommandType = CommandType.StoredProcedure;
                cmdAuditLog.CommandText = "SPM_spAccessRightAuditIns";
                cmdAuditLog.Parameters.AddRange(sqlAuditLogParams);


                foreach (DataRow dr in userAccessRightsUpdate.Tables[0].Rows)
                {
                    status = "";
                    int currentIndex = 0; //countFound = 0;
                    if (dsOld != null && dsOld.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow drOld in dsOld.Tables[0].Rows)
                        {
                            if (drOld["Functions"].ToString() == dr["Function_Code"].ToString())
                            {
                                currentIndex = userAccessRightsUpdate.Tables[0].Rows.IndexOf(dr);

                                if (dr["Status"].ToString() == "C")
                                {
                                    //countFound = countFound + 1;

                                    /// <Updated by OC>
                                    //cmdAuditLog.Parameters["@istrOldCreateRight"].Value = dsOld.Tables[0].Rows[currentIndex]["CreateRight"].ToString();
                                    //cmdAuditLog.Parameters["@istrOldViewRight"].Value = dsOld.Tables[0].Rows[currentIndex]["ViewRight"].ToString();
                                    //cmdAuditLog.Parameters["@istrOldModifyRight"].Value = dsOld.Tables[0].Rows[currentIndex]["ModifyRight"].ToString();
                                    //cmdAuditLog.Parameters["@istrOldDeleteRight"].Value = dsOld.Tables[0].Rows[currentIndex]["DeleteRight"].ToString();
                                }

                                break;
                            }
                            //else
                            //{
                            //    cmdAuditLog.Parameters["@istrOldCreateRight"].Value = "";
                            //    cmdAuditLog.Parameters["@istrOldViewRight"].Value = "";
                            //    cmdAuditLog.Parameters["@istrOldModifyRight"].Value = "";
                            //    cmdAuditLog.Parameters["@istrOldDeleteRight"].Value = "";
                            //}
                        }
                    }
                    else
                    {
                        /// <Updated by OC>
                        //cmdAuditLog.Parameters["@istrOldCreateRight"].Value = "";
                        //cmdAuditLog.Parameters["@istrOldViewRight"].Value = "";
                        //cmdAuditLog.Parameters["@istrOldModifyRight"].Value = "";
                        //cmdAuditLog.Parameters["@istrOldDeleteRight"].Value = "";

                        status = "New";
                    }

                    cmdAuditLog.Parameters["@istrUserId"].Value = userId;
                    cmdAuditLog.Parameters["@istrFunctionCode"].Value = dr["Function_Code"].ToString();
                    cmdAuditLog.Parameters["@istrActionCode"].Value = dr["Action"].ToString();
                    cmdAuditLog.Parameters["@istrActionBy"].Value = actionUserId;

                    cmdAuditLog.Parameters["@istrNewCreateRight"].Value = dr["CreateRight"].ToString();
                    cmdAuditLog.Parameters["@istrNewViewRight"].Value = dr["ViewRight"].ToString();
                    cmdAuditLog.Parameters["@istrNewModifyRight"].Value = dr["ModifyRight"].ToString();
                    cmdAuditLog.Parameters["@istrNewDeleteRight"].Value = dr["DeleteRight"].ToString();

                    switch (dr["Action"].ToString())
                    {
                        case "D":
                            {
                                // Delete current access right records
                                sql = "DELETE FROM dbo.AccessRight WHERE UserID = '" + userId + "' AND Functions = '" + dr["Function_Code"].ToString() + "'";
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();

                                // Insert into audit trail
                                //sql = "INSERT INTO dbo.AccessRightAudit VALUES('" + userId + "', '" + dr["Function_Code"].ToString() + "', 'D', '" + actionUserId + "', current_timestamp)";
                                //cmd.CommandText = sql;
                                //cmd.ExecuteNonQuery();

                                cmdAuditLog.ExecuteNonQuery();

                                break;
                            }
                        case "A":
                            {
                                sql = "DELETE FROM dbo.AccessRight WHERE UserID = '" + userId + "' AND Functions = '" + dr["Function_Code"].ToString() + "'";
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();


                                sql = "INSERT INTO dbo.AccessRight VALUES ('" + userId + "', 'SP', '" + dr["Function_Code"].ToString() + "'," +
                                      "'" + dr["CreateRight"].ToString() + "','" + dr["ViewRight"].ToString() + "','" + dr["ModifyRight"].ToString() + "','" +
                                      dr["DeleteRight"].ToString() + "', '" + roleId + "','" + dr["UserRight"].ToString() + "')";

                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();

                                // Insert into audit trail
                                //sql = "INSERT INTO dbo.AccessRightAudit VALUES('" + userId + "', '" + dr["Function_Code"].ToString() + "', 'A', '" + actionUserId + "', current_timestamp)";
                                //cmd.CommandText = sql;
                                //cmd.ExecuteNonQuery();
                                if (dr["Status"].ToString() == "C" || status == "New")
                                {
                                    cmdAuditLog.ExecuteNonQuery();
                                }


                                break;
                            }
                        case "U":
                            {
                                sql = "UPDATE  dbo.AccessRight SET CreateRight = '" + dr["CreateRight"].ToString() + "',ViewRight='" + dr["ViewRight"].ToString() +
                                      "',ModifyRight='" + dr["ModifyRight"].ToString() + "',DeleteRight='" + dr["DeleteRight"].ToString() + "', " +
                                      "RoleID='" + roleId + "', UserRight='" + dr["UserRight"].ToString() + "'" +
                                      "WHERE UserId= '" + userId + "' AND Functions = '" + dr["Function_Code"].ToString() + "'";
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();

                                // Insert into audit trail
                                //sql = "INSERT INTO dbo.AccessRightAudit VALUES('" + userId + "', '" + dr["Function_Code"].ToString() + "', 'A', '" + actionUserId + "', current_timestamp)";
                                //cmd.CommandText = sql;
                                //cmd.ExecuteNonQuery();

                                if (dr["Status"].ToString() == "C")
                                {
                                    cmdAuditLog.ExecuteNonQuery();
                                }

                                break;
                            }
                        default:
                            {
                                returnCode = "-1";
                                returnMessage = "Invalid action code: " + dr["Action"].ToString() + " !";
                                break;
                            }
                    }

                    if (returnCode == "-1")
                        break;
                }

                if (returnCode == "1")
                {
                    sqlTransaction.Commit();
                    returnCode = "1";
                    returnMessage = "User Access Rights are successfully updated!";
                }
                else
                {
                    sqlTransaction.Rollback();
                }

            }
            catch (Exception e)
            {
                sqlTransaction.Rollback();
                returnCode = "-1";
                returnMessage = "Error updating user access rights! " + e.Message;
            }
            finally
            {
                //Create Table for ReturnCode and Return Message
                dtReturn = CommonUtilities.CreateReturnTable(returnCode, returnMessage);
                dsReturn.Tables.Add(dtReturn);

                try
                {
                    genericDA.CloseConnection();
                }
                catch (Exception e)
                { }
            }

            return dsReturn;
        }

        public DataSet RetrieveUserRoles()
        {
            sql = "SELECT RoleID, RTRIM(RoleName) AS RoleName " +
                  "FROM dbo.Role " +
                  "ORDER BY RoleName";

            return genericDA.ExecuteQuery(sql);
        }

        public DataSet RetrieveUserRoles(string roleId)
        {
            sql = "SELECT RoleID, RTRIM(RoleName) AS RoleName, RoleDesc " +
                  "FROM dbo.Role " +
                  "WHERE RoleID='" + roleId + "'";

            return genericDA.ExecuteQuery(sql);
        }

        public DataSet RetrieveMaxUserRoleID()
        {
            sql = "SELECT Max(RoleID) AS MaxRoleID " +
                  "FROM dbo.Role ";

            return genericDA.ExecuteQuery(sql);
        }

        public DataSet RetrieveUserRoleRights(string roleId)
        {
            if (roleId == "")
            {
                sql = "SELECT FM.Function_Code, FM.Function_Desc,FM.Category, " + // FM.CreateRight,FM.ViewRight,FM.ModifyRight,FM.DeleteRight " +
                      "CASE WHEN FM.CreateRight ='Y' THEN 'TRUE' ELSE 'FALSE' END AS CreateRight, " +
                      "CASE WHEN FM.ViewRight = 'Y' THEN 'TRUE' ELSE 'FALSE' END AS ViewRight, " +
                      "CASE WHEN FM.ModifyRight ='Y' THEN 'TRUE' ELSE 'FALSE' END AS ModifyRight, " +
                      "CASE WHEN FM.DeleteRight ='Y' THEN 'TRUE' ELSE 'FALSE' END AS DeleteRight, " +
                      "'' AS RoleID,'' AS UserRight, " +
                      "CASE WHEN FM.CreateRight ='D' THEN 'D' END AS hidCreate, " +
                      "CASE WHEN FM.ViewRight = 'D' THEN 'D'  END AS hidView, " +
                      "CASE WHEN FM.ModifyRight ='D' THEN 'D' END AS hidModify, " +
                      "CASE WHEN FM.DeleteRight ='D' THEN 'D' END AS hidDelete " +
                      "FROM  dbo.FunctionURL AS FM ORDER BY FM.Category";

            }
            else
            {
                sql = "SELECT FM.Function_Code, FM.Function_Desc, FM.Category, " +
                      "CASE WHEN RA.CreateRight ='Y' THEN 'TRUE' ELSE 'FALSE' END AS CreateRight, " +
                      "CASE WHEN RA.ViewRight = 'Y' THEN 'TRUE'  ELSE 'FALSE' END AS ViewRight, " +
                      "CASE WHEN RA.ModifyRight ='Y' THEN 'TRUE' ELSE 'FALSE' END AS ModifyRight, " +
                      "CASE WHEN RA.DeleteRight ='Y' THEN 'TRUE' ELSE 'FALSE' END AS DeleteRight, " +
                      "RoleID AS RoleID,'' AS UserRight, " +
                      "CASE WHEN FM.CreateRight ='D' THEN 'D' END AS hidCreate, " +
                      "CASE WHEN FM.ViewRight = 'D' THEN 'D'  END AS hidView, " +
                      "CASE WHEN FM.ModifyRight ='D' THEN 'D' END AS hidModify, " +
                      "CASE WHEN FM.DeleteRight ='D' THEN 'D' END AS hidDelete " +
                      "FROM  dbo.FunctionURL AS FM JOIN " +
                      "dbo.RoleAccess AS RA ON FM.Function_Code = RA.Functions " +
                      "WHERE RoleID= '" + roleId + "' " +
                      "ORDER BY FM.Category";
            }

            return genericDA.ExecuteQuery(sql);
        }

        public DataSet RetrieveAssignedRole(string roleId)
        {
            if (roleId == "")
            {

            }
            else
            {
                sql = "SELECT * FROM AccessRight WHERE RoleId='" + roleId + "'";
            }

            return genericDA.ExecuteQuery(sql);
        }

        public DataSet UpdateRole(String roleId, String roleName, String roleDes, DataSet userRoleUpdate, String actionUserId)
        {
            String returnCode = "1";
            String returnMessage = "";
            DataSet dsReturn = new DataSet();
            DataTable dtReturn = new DataTable();

            String oldCreateRight = "";
            String oldViewRight = "";
            String oldModifyRight = "";
            String oldDeleteRight = "";    

            if (userRoleUpdate == null || userRoleUpdate.Tables.Count == 0 || userRoleUpdate.Tables[0].Rows.Count == 0)
            {
                returnCode = "-1";
                returnMessage = "No record to update!";
                dtReturn = CommonUtilities.CreateReturnTable(returnCode, returnMessage);
                dsReturn.Tables.Add(dtReturn);
                return dsReturn;
            }

            //Parameters for AuditLog
            //OleDbParameter[] sqlAuditLogParams = new OleDbParameter[] 
            //                            { 
            //                                new OleDbParameter("@istrUserId", OleDbType.VarChar),   
            //                                new OleDbParameter("@istrFunctionCode", OleDbType.VarChar),
            //                                new OleDbParameter("@istrActionCode", OleDbType.Char),
            //                                new OleDbParameter("@istrActionBy", OleDbType.VarChar)
            //                            };

            OleDbCommand cmd = null;            
            OleDbTransaction sqlTransaction = null;

            try
            {
                genericDA.OpenConnection();
                //Start Transaction
                sqlTransaction = genericDA.GetSqlConnection().BeginTransaction();
                genericDA.CreateSqlCommand();
                cmd = genericDA.GetSqlCommand();
                cmd.Transaction = sqlTransaction;
                
                //Create AuditLog command
                //cmdAuditLog = genericDA.GetNewSqlCommand();
                //cmdAuditLog.Transaction = sqlTransaction;
                //cmdAuditLog.CommandType = CommandType.StoredProcedure;
                //cmdAuditLog.CommandText = "SPM_spAccessRightAuditIns";
                //cmdAuditLog.Parameters.AddRange(sqlAuditLogParams);

                switch (userRoleUpdate.Tables[0].Rows[0]["Action"].ToString())
                {
                    case "D":
                        {
                            sql = "DELETE FROM dbo.RoleAccess WHERE RoleID = '" + roleId + "'";
                            cmd.CommandText = sql;
                            cmd.ExecuteNonQuery();

                            sql = "DELETE dbo.Role WHERE RoleId='" + roleId + "'";
                            cmd.CommandText = sql;
                            cmd.ExecuteNonQuery();
                            break;
                        }
                    case "A":
                        {
                            sql = "INSERT INTO dbo.Role VALUES('" + roleId + "','" + roleName + "','" + roleDes + "')";
                            cmd.CommandText = sql;
                            cmd.ExecuteNonQuery();
                            break;
                        }
                    case "U":
                        {
                            sql = "UPDATE dbo.Role SET RoleDesc='" + roleDes + "' WHERE RoleId='" + roleId + "'";
                            cmd.CommandText = sql;
                            cmd.ExecuteNonQuery();
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }

                foreach (DataRow dr in userRoleUpdate.Tables[0].Rows)
                {
                    //cmdAuditLog.Parameters["@istrUserId"].Value = roleId;
                    //cmdAuditLog.Parameters["@istrFunctionCode"].Value = dr["Function_Code"].ToString();
                    //cmdAuditLog.Parameters["@istrActionCode"].Value = dr["Action"].ToString();
                    //cmdAuditLog.Parameters["@istrActionBy"].Value = actionUserId;
                    switch (dr["Action"].ToString())
                    {
                        case "D":
                            {
                                //if (resultCount > 0)
                                //{
                                //    returnCode = "-1";
                                //    returnMessage = "Cannot Delete since selected role is currently assigned. ";
                                //}
                                //else
                                //{
                                // Delete current access right records
                                sql = "DELETE FROM dbo.RoleAccess WHERE RoleID = '" + roleId + "' AND Functions = '" + dr["Function_Code"].ToString() + "'";
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                //}
                                // Insert into audit trail
                                //sql = "INSERT INTO dbo.AccessRightAudit VALUES('" + userId + "', '" + dr["Function_Code"].ToString() + "', 'D', '" + actionUserId + "', current_timestamp)";
                                //cmd.CommandText = sql;
                                //cmd.ExecuteNonQuery();

                                //cmdAuditLog.ExecuteNonQuery();                                
                                break;
                            }
                        case "A":
                            {
                                sql = "INSERT INTO dbo.RoleAccess VALUES('" + roleId + "', '" + dr["Function_Code"].ToString() + "'," +
                                      "'" + dr["CreateRight"].ToString() + "','" + dr["ViewRight"].ToString() + "','" + dr["ModifyRight"].ToString() + "','" + dr["DeleteRight"].ToString() + "')";
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();

                                // Insert into audit trail
                                //sql = "INSERT INTO dbo.AccessRightAudit VALUES('" + userId + "', '" + dr["Function_Code"].ToString() + "', 'A', '" + actionUserId + "', current_timestamp)";
                                //cmd.CommandText = sql;
                                //cmd.ExecuteNonQuery();

                                //cmdAuditLog.ExecuteNonQuery();                                
                                break;
                            }
                        case "U":
                            {
                                String sql = "SELECT RoleID, Functions, CreateRight, ViewRight, ModifyRight, DeleteRight FROM dbo.RoleAccess " +
                                                "WHERE RoleId = '" + roleId + "' AND Functions = '" + dr["Function_Code"].ToString() + "'";
                                cmd.CommandText = sql;
                                using (OleDbDataReader reader = cmd.ExecuteReader())
                                {
                                    if (reader.HasRows)
                                    {
                                        while (reader.Read())
                                        {
                                            oldCreateRight = reader.GetString(2);
                                            oldViewRight = reader.GetString(3);
                                            oldModifyRight = reader.GetString(4);
                                            oldDeleteRight = reader.GetString(5);
                                        }
                                    }
                                }

                                sql = "UPDATE  dbo.RoleAccess SET CreateRight = '" + dr["CreateRight"].ToString() + "',ViewRight='" + dr["ViewRight"].ToString() +
                                      "',ModifyRight='" + dr["ModifyRight"].ToString() + "',DeleteRight='" + dr["DeleteRight"].ToString() + "' " +
                                      "WHERE RoleId= '" + roleId + "' AND Functions = '" + dr["Function_Code"].ToString() + "'";
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();

                                bool isSuccessfulUpdateAccessRight = UpdateClientAccessRight(cmd, roleId, oldCreateRight, oldViewRight, oldModifyRight, oldDeleteRight, dr, out returnCode, out returnMessage);
                                // Insert into audit trail
                                //sql = "INSERT INTO dbo.AccessRightAudit VALUES('" + userId + "', '" + dr["Function_Code"].ToString() + "', 'A', '" + actionUserId + "', current_timestamp)";
                                //cmd.CommandText = sql;
                                //cmd.ExecuteNonQuery();

                                //cmdAuditLog.ExecuteNonQuery();
                                break;
                            }
                        default:
                            {
                                returnCode = "-1";
                                returnMessage = "Invalid action code: " + dr["Action"].ToString() + " !";
                                break;
                            }
                    }

                    if (returnCode == "-1")
                        break;
                }

                if (returnCode == "1")
                {
                    sqlTransaction.Commit();
                    returnCode = "1";
                    switch (userRoleUpdate.Tables[0].Rows[0]["Action"].ToString())
                    {
                        case "D":
                            { returnMessage = "User Role Rights are successfully deleted!"; break; }
                        case "A":
                            { returnMessage = "User Role Rights are successfully added!"; break; }
                        case "U":
                            { returnMessage = "User Role Rights are successfully updated!"; break; }
                        default:
                            {
                                break;
                            }
                    }
                }
                else
                {
                    sqlTransaction.Rollback();
                }

            }
            catch (Exception e)
            {
                sqlTransaction.Rollback();
                returnCode = "-1";
                returnMessage = "Error updating user role rights! " + e.Message;
            }
            finally
            {
                //Create Table for ReturnCode and Return Message
                dtReturn = CommonUtilities.CreateReturnTable(returnCode, returnMessage);
                dsReturn.Tables.Add(dtReturn);

                try
                {
                    genericDA.CloseConnection();
                }
                catch (Exception e)
                { }
            }

            return dsReturn;

        }

        private bool UpdateClientAccessRight(OleDbCommand cmd, string roleId, string oldCreateRight, string oldViewRight, string oldModifyRight, string oldDeleteRight, DataRow drUserRole, out String returnCode, out String returnMessage)
        {            
            try
            {
                returnCode = "1";
                returnMessage = "";

                switch (drUserRole["Action"].ToString())
                {
                    case "D":
                        {
                            DeleteFromAccessRight(cmd, roleId, drUserRole["Function_Code"].ToString());
                            break;
                        }
                    case "A":
                        {
                            InsertIntoAccessRight(cmd, roleId, drUserRole["Function_Code"].ToString(), drUserRole["CreateRight"].ToString(), drUserRole["ViewRight"].ToString(), drUserRole["ModifyRight"].ToString(), drUserRole["DeleteRight"].ToString());
                            break;
                        }
                    case "U":
                        {
                            UpdateAccessRight(cmd, roleId, drUserRole["Function_Code"].ToString(), drUserRole["CreateRight"].ToString(), drUserRole["ViewRight"].ToString(), drUserRole["ModifyRight"].ToString(), drUserRole["DeleteRight"].ToString(), oldCreateRight, oldViewRight, oldModifyRight, oldDeleteRight);
                            break;
                        }
                    default:
                        {
                            returnCode = "-1";
                            returnMessage = "Invalid action code: " + drUserRole["Action"].ToString() + " !";                                
                            break;
                        }
                }
                
                if (returnCode.Equals("-1"))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                returnCode = "-1";
                returnMessage = "Error Occured, Exception: " + e.ToString();
                return false;
            }
        }

        private void UpdateAccessRight(OleDbCommand cmd, string roleId, string functionCode, string createRight, string viewRight, string modifyRight, string deleteRight, string oldCreateRight, string oldViewRight, string oldModifyRight, string oldDeleteRight)
        {                 
            try
            {
                List<AccessRightObj> accessRightList = new List<AccessRightObj>();
                sql = "SELECT DISTINCT UserID, Type, Functions, RoleID, CreateRight, ViewRight, ModifyRight, DeleteRight, UserRight " +
                        "FROM AccessRight " +
                        "WHERE RoleID = '" + roleId + "' AND Functions = '" + functionCode + "' ";
                cmd.CommandText = sql;
                using (OleDbDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            AccessRightObj accessRight = new AccessRightObj();
                            accessRight.UserID = reader.GetString(0);
                            accessRight.Type = reader.GetString(1);
                            accessRight.Functions = reader.GetString(2);
                            accessRight.RoleID = reader.GetString(3);
                            accessRight.CreateRight = reader.GetString(4);
                            accessRight.ViewRight = reader.GetString(5);
                            accessRight.ModifyRight = reader.GetString(6);
                            accessRight.DeleteRight = reader.GetString(7);
                            accessRight.UserRight = reader.GetString(8);
                            accessRightList.Add(accessRight);
                        }
                    }
                }

                foreach (AccessRightObj accessRight in accessRightList)
                {
                    sql = string.Empty;
                    String newCreateRight = oldCreateRight.Equals(accessRight.CreateRight) ? createRight : accessRight.CreateRight;
                    String newViewRight = oldViewRight.Equals(accessRight.ViewRight) ? viewRight : accessRight.ViewRight;
                    String newModifyRight = oldModifyRight.Equals(accessRight.ModifyRight) ? modifyRight : accessRight.ModifyRight;
                    String newDeleteRight = oldDeleteRight.Equals(accessRight.DeleteRight) ? deleteRight : accessRight.DeleteRight;
                    bool hasUserRight = !(createRight.Equals("N") && viewRight.Equals("N") && modifyRight.Equals("N") && deleteRight.Equals("N"));
                    sql = "UPDATE  dbo.AccessRight SET " +
                                "CreateRight = '" + newCreateRight + "', " +
                                "ViewRight = '" + newViewRight + "', " +
                                "ModifyRight = '" + newModifyRight + "', " +
                                "DeleteRight = '" + newDeleteRight + "', " +
                                "UserRight = '" + (hasUserRight == true ? "Y" : "N") + "' " +
                                  "WHERE RoleId= '" + roleId + "' AND Functions = '" + functionCode + "' AND UserID = '" + accessRight.UserID + "' ";
                    cmd.CommandText = sql;
                    cmd.ExecuteScalar();
                }
            }
            catch (Exception e)
            {                
                throw new Exception(e.ToString());
            }            
        }

        private void InsertIntoAccessRight(OleDbCommand cmd, string roleId, string functionCode, string createRight, string viewRight, string modifyRight, string deleteRight)
        {
            try
            {
                sql = "SELECT DISTINCT UserID, Type, RoleID FROM AccessRight WHERE RoleID = '" + roleId + "'";
                cmd.CommandText = sql;
                using (OleDbDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            String userId = reader.GetString(0);
                            String type = reader.GetString(1);
                            sql = string.Empty;
                            bool hasUserRight = !(createRight.Equals("N") && viewRight.Equals("N") && modifyRight.Equals("N") && deleteRight.Equals("N"));
                            sql = "INSERT INTO dbo.RoleAccess (UserId, Type, Functions, CreateRight, ViewRight, ModifyRight, DeleteRight, RoleId, UserRight) " +
                                    " VALUES ('" + userId + "', '" + type + "', '" + functionCode + "', '" + createRight + "', " +
                                    " '" + viewRight + "', '" + modifyRight + "', '" + deleteRight + "', '" + roleId + "', '" + (hasUserRight == true ? "Y" : "N") + "' )";
                            cmd.CommandText = sql;
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
        }

        private void DeleteFromAccessRight(OleDbCommand cmd, String roleId, String FunctionCode)
        {            
            try
            {
                sql = "DELETE FROM dbo.AccessRight WHERE RoleId = '" + roleId + "' AND Functions = '" + FunctionCode + "'";
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();                 
            }
            catch(Exception e)
            {                                
                throw new Exception(e.ToString());
            }            
        }

        public DataSet RetrieveRoleNames(string roleName)
        {
            sql = "SELECT RoleID,RoleName,RoleDesc FROM [dbo].[Role] WHERE RoleName='" + roleName + "'";

            return genericDA.ExecuteQuery(sql);
        }

        #endregion 
    }
}