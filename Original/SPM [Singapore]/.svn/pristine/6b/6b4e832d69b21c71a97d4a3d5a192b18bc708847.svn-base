using System;
using System.Collections.Generic;
using System.Web;

using System.Data;
using System.Data.OleDb;

namespace SPMWebServiceApp.DataAccess
{
    public class SuperAdminDA
    {
        private GenericDA genericDA;
        OleDbCommand Command;
        OleDbConnection DBConnection;
        OleDbTransaction DBTransaction;

        public SuperAdminDA(GenericDA da)
        {
            genericDA = da;
        }

        public DataTable SuperAdmin_GetByFilters(String UserID, String AEGroup, int Start, int PageLength)
        {
            Command = new OleDbCommand("SPM_SuperAdmin_GetByFilters", genericDA.GetSqlConnectionNew());
            Command.CommandType = CommandType.StoredProcedure;
            Command.Parameters.AddWithValue("UserID", UserID);
            Command.Parameters.AddWithValue("AEGroup", AEGroup);
            Command.Parameters.AddWithValue("Start", Start);
            Command.Parameters.AddWithValue("PageLength", PageLength);

            OleDbDataAdapter DataAdapter = new OleDbDataAdapter(Command);
            DataTable dt = new DataTable();
            DataAdapter.Fill(dt);

            return dt;
        }

        public DataTable SuperAdmin_GetAEGroup(String AEGroup, String rdbtnAEGroupOption)
        {
            String sql = String.Empty;

            switch (rdbtnAEGroupOption)
            {
                case "S":
                    sql = "SELECT AE_CD_Sec AS AEGroup FROM AEList WHERE RefMonthYr = (SELECT MAX(RefMonthYr) FROM AEList) AND AE_CD_Sec LIKE '" + AEGroup + "%' AND AE_CD_Sec NOT IN (SELECT AECode FROM DealerDetail WHERE UserType LIKE 'FAR%');";
                    break;

                case "E":
                    sql = "SELECT AE_CD_Sec AS AEGroup FROM AEList WHERE RefMonthYr = (SELECT MAX(RefMonthYr) FROM AEList) AND AE_CD_Sec LIKE '%" + AEGroup + "' AND AE_CD_Sec NOT IN (SELECT AECode FROM DealerDetail WHERE UserType LIKE 'FAR%');";
                    break;

                case "C":
                    sql = "SELECT AE_CD_Sec AS AEGroup FROM AEList WHERE RefMonthYr = (SELECT MAX(RefMonthYr) FROM AEList) AND AE_CD_Sec LIKE '%" + AEGroup + "%' AND AE_CD_Sec NOT IN (SELECT AECode FROM DealerDetail WHERE UserType LIKE 'FAR%');";
                    break;

                default:
                    sql = "SELECT AE_CD_Sec AS AEGroup FROM AEList WHERE RefMonthYr = (SELECT MAX(RefMonthYr) FROM AEList) AND AE_CD_Sec NOT IN (SELECT AECode FROM DealerDetail WHERE UserType LIKE 'FAR%');";
                    break;
            }

            return genericDA.ExecuteQueryForDataTable(sql, "dtAEGroup");
        }

        public DataTable SuperAdmin_GetUserID(String UserID, String rdbtnUserIDOption)
        {
            String sql = String.Empty;

            switch (rdbtnUserIDOption)
            {
                case "S":
                    sql = "SELECT UserID, AEName FROM DealerDetail AS DD WHERE DD.UserType NOT LIKE 'FAR%' AND (DD.UserType IS NULL OR DD.UserType = '') AND AEName LIKE '" + UserID + "%' GROUP BY DD.UserID, DD.AEName ORDER BY DD.UserID;";
                    break;

                case "E":
                    sql = "SELECT UserID, AEName FROM DealerDetail AS DD WHERE DD.UserType NOT LIKE 'FAR%' AND (DD.UserType IS NULL OR DD.UserType = '') AND AEName LIKE '%" + UserID + "' GROUP BY DD.UserID, DD.AEName ORDER BY DD.UserID;";
                    break;

                case "C":
                    sql = "SELECT UserID, AEName FROM DealerDetail AS DD WHERE DD.UserType NOT LIKE 'FAR%' AND (DD.UserType IS NULL OR DD.UserType = '') AND AEName LIKE '%" + UserID + "%' GROUP BY DD.UserID, DD.AEName ORDER BY DD.UserID;";
                    break;

                default:
                    sql = "SELECT UserID, AEName FROM DealerDetail AS DD WHERE DD.UserType NOT LIKE 'FAR%' AND (DD.UserType IS NULL OR DD.UserType = '') GROUP BY DD.UserID, DD.AEName ORDER BY DD.UserID;";
                    break;
            }

            return genericDA.ExecuteQueryForDataTable(sql, "dtUserID");
        }        

        public DataTable SuperAdmin_GetAECode()
        {
            String sql = String.Empty;

            sql = "SELECT AECode FROM DealerDetail AS DD WHERE DD.UserType NOT LIKE 'FAR%' AND (DD.UserType IS NULL OR DD.UserType = '') GROUP BY DD.AECode ORDER BY DD.AECode;";

            return genericDA.ExecuteQueryForDataTable(sql, "dtAECode");
        }

        public String SuperAdmin_Delete(String UserID, String AEGroup)
        {
            try
            {
                DBConnection = new OleDbConnection(genericDA.GetSqlConnectionNew().ConnectionString);

                String sql = String.Empty;

                if (UserID != String.Empty && AEGroup != String.Empty)
                {
                    sql = "DELETE SuperAdmin WHERE UserID = '" + UserID + "' AND AEGroup = '" + AEGroup + "';";
                }
                else if (UserID != String.Empty && AEGroup == String.Empty)
                {
                    sql = "DELETE SuperAdmin WHERE UserID = '" + UserID + "';";
                }

                DBConnection.Open();
                DBTransaction = DBConnection.BeginTransaction();

                Command = new OleDbCommand(sql, DBConnection, DBTransaction);
                Command.ExecuteNonQuery();

                DBTransaction.Commit();
                DBConnection.Close();
            }
            catch (Exception ex)
            {
                DBTransaction.Rollback();
                DBConnection.Close();

                return ex.Message;
            }

            return "1";
        }

        public String SuperAdmin_Insert(String UserID, String AEGroup)
        {
            try
            {
                DBConnection = new OleDbConnection(genericDA.GetSqlConnectionNew().ConnectionString);

                String sql = String.Empty;

                sql = "IF NOT EXISTS(SELECT NULL FROM SuperAdmin WHERE UserID = '" + UserID + "' AND AEGroup = '" + AEGroup + "') " +
                "BEGIN " +
                "INSERT INTO SuperAdmin (UserID, AEGroup) VALUES ('" + UserID + "', '" + AEGroup + "'); " +
                "END";

                DBConnection.Open();
                DBTransaction = DBConnection.BeginTransaction();

                Command = new OleDbCommand(sql, DBConnection, DBTransaction);
                Command.ExecuteNonQuery();

                DBTransaction.Commit();
                DBConnection.Close();
            }
            catch (Exception ex)
            {
                DBTransaction.Rollback();
                DBConnection.Close();

                return ex.Message;
            }

            return "1";
        }
    }
}