﻿using System;
using System.Data;
using System.Text;
using System.Data.OleDb;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace SPMWebServiceApp.DataAccess
{
    public class DealerDA
    {
        private GenericDA genericDA;
        private string sql;

        public DealerDA(GenericDA da)
        {
            genericDA = da;
        }

        public DataSet RetrieveDealerByTeamCode(string teamCode)
        {
            sql = "SELECT DD.AECode, DD.AEName, DD.AEGroup, ISNULL(CA.CurrentAssign, 0) AS CurrentAssign " +
                        "FROM DealerDetail DD " +
                        "LEFT JOIN " +
                        "( " +
                        "SELECT AECode, COUNT(AECode) AS CurrentAssign " +
                        "FROM ClientAssign WITH (NOLOCK) " +
                        "WHERE AssignDate < CutOffDate " +
                        "GROUP BY AECode " +
                        ") CA ON DD.AECode = CA.AECode " +
                        "WHERE DD.AEGroup = '" + teamCode + "'  " +
                        "ORDER BY CurrentAssign, DD.AECode";

            return genericDA.ExecuteQuery(sql);
        }

        public DataSet FillDealerByTeamCode(DataSet ds, string tableName, string teamCode)
        {
            sql = "SELECT DD.AECode, DD.AEName, DD.AEGroup, ISNULL(CA.CurrentAssign, 0) AS CurrentAssign " +
                        "FROM DealerDetail DD " +
                        "LEFT JOIN " +
                        "( " +
                        "SELECT AECode, COUNT(AECode) AS CurrentAssign " +
                        "FROM ClientAssign WITH (NOLOCK) " +
                        "WHERE AssignDate < CutOffDate " +
                        "GROUP BY AECode " +
                        ") CA ON DD.AECode = CA.AECode " +
                        "WHERE DD.AEGroup = '" + teamCode + "'";

            return genericDA.FillDataSet(ds, tableName, sql);
        }

        public DataTable RetrieveDealerByUserId(string userId)
        {
            sql = "SELECT AECode,AEGroup FROM DealerDetail WHERE UserID='" + userId + "'";
            return genericDA.ExecuteQueryForDataTable(sql);
        }

        public DataTable RetrieveDealerByDealerCode(string dealerCode)
        {
            sql = "SELECT AEGroup, AECode, AEName, UserID FROM DealerDetail WHERE AECode='" + dealerCode + "'";
            return genericDA.ExecuteQueryForDataTable(sql);
        }

        public DataTable RetrieveAllDealer()
        {
            sql = "SELECT RecId, UserID, AECode, AEName, AEGroup, ATSLogin, CrossGroup, Supervisor, ModifiedUser, ModifiedDate, " +
                    " Enable = CASE Enable WHEN 1 THEN 'Y' WHEN 0 THEN 'N' END FROM DealerDetail ORDER BY AEGroup, AECode";
            return genericDA.ExecuteQueryForDataTable(sql);
        }        

        public DataTable RetrieveDealerByCriteria(string emailID, string dealerCode, string dealerName, string teamCode, string atsLogin, int enable,
                    string crossGroup, string supervisior,string dealerEmailID, String UserID)
        {
            sql = "SELECT RecId, UserID, AECode, AEName, AEGroup, ATSLogin, CrossGroup, Supervisor, ModifiedUser, ModifiedDate, " +
                    " AECode AS OriginalAECode,  UserID AS OriginalUserID, Email , " +
                    " Enable = CASE Enable WHEN 1 THEN 'Y' WHEN 0 THEN 'N' END, AltAECode FROM DealerDetail ";

            StringBuilder sb = new StringBuilder(sql);
            bool whereFlag = false;

            if (enable == 1)
            {
                sb.Append(" WHERE Enable =").Append(enable);
                whereFlag = true;
            }

            if (!String.IsNullOrEmpty(crossGroup))
            {
                if(whereFlag)
                    sb.Append(" AND CrossGroup = '").Append(crossGroup).Append("' ");
                else
                    sb.Append(" WHERE CrossGroup = '").Append(crossGroup).Append("' ");

                whereFlag = true;
            }

            if (!String.IsNullOrEmpty(supervisior))
            {
                if (whereFlag)
                    sb.Append(" AND Supervisor = '").Append(supervisior).Append("' ");
                else
                    sb.Append(" WHERE Supervisor = '").Append(supervisior).Append("' ");

                whereFlag = true;
            }

            if (!String.IsNullOrEmpty(emailID))
            {
                if (whereFlag)
                    sb.Append(" AND UserID = '").Append(emailID).Append("' ");
                else
                    sb.Append(" WHERE UserID = '").Append(emailID).Append("' ");

                whereFlag = true;
            }

            if (!String.IsNullOrEmpty(dealerCode))
            {
                if (whereFlag)
                    sb.Append(" AND AECode = '").Append(dealerCode).Append("' ");
                else
                    sb.Append(" WHERE AECode = '").Append(dealerCode).Append("' ");

                whereFlag = true;
            }

            if (!String.IsNullOrEmpty(dealerName))
            {
                if (whereFlag)
                    sb.Append(" AND AEName LIKE '%").Append(dealerName).Append("%' ");
                else
                    sb.Append(" WHERE AEName LIKE '%").Append(dealerName).Append("%' ");

                whereFlag = true;
            }

            if (!String.IsNullOrEmpty(teamCode))
            {
                if (whereFlag)
                    sb.Append(" AND AEGroup = '").Append(teamCode).Append("' ");
                else
                    sb.Append(" WHERE AEGroup = '").Append(teamCode).Append("' ");

                whereFlag = true;
            }

            if (!String.IsNullOrEmpty(atsLogin))
            {
                if (whereFlag)
                    sb.Append(" AND ATSLogin = '").Append(atsLogin).Append("' ");
                else
                    sb.Append(" WHERE ATSLogin = '").Append(atsLogin).Append("' ");

                whereFlag = true;
            }

            if(!String.IsNullOrEmpty(dealerEmailID))
            {
                if (whereFlag)
                    sb.Append(" AND Email = '").Append(dealerEmailID).Append("' ");
                else
                    sb.Append(" WHERE Email = '").Append(dealerEmailID).Append("' ");

                whereFlag = true;
               
            }

            if (whereFlag)
            {
                sb.Append(" AND UserType NOT LIKE 'FAR%' ");
            }
            else
            {
                sb.Append(" WHERE UserType NOT LIKE 'FAR%' ");
            }

            // commented to list down all the groups regardless of log-in user access right on groups
            //sb.Append("AND AEGroup IN (SELECT AEGroup FROM SuperAdmin WHERE UserID='" + UserID + "')");

            sb.Append(" ORDER BY AEGroup, AECode ");
            
            return genericDA.ExecuteQueryForDataTable(sb.ToString());
        }

        public int DeleteDealer(long recId)
        {
            sql = "DELETE FROM DealerDetail WHERE RecId = " + recId;

            return genericDA.ExecuteNonQuery(sql);
        }

        public int DeleteDealerByUserID(string userId)
        {
            sql = "DELETE FROM DealerDetail WHERE userid = '" + userId + "'";
            return genericDA.ExecuteNonQuery(sql);
        }

        public int InsertDealer(string emailID, string dealerCode, string dealerName, string teamCode, string atsLogin, int enable,
                    string crossGroup, string supervisior, string modifiedUser, string dealerEmailID, string altAECode)
        {
            string insertedIdStr = "";
            int result = -1;
            OleDbCommand cmd = null;

            sql = " INSERT INTO DealerDetail (UserID, AECode, AEName, AEGroup, ATSLogin, Enable, CrossGroup, Supervisor, modifiedUser, modifiedDate,Email, AltAECode) "
                    + " VALUES(LOWER('" + emailID + "'), UPPER('" + dealerCode + "'), '" + dealerName + "', UPPER('" + teamCode + "'), UPPER('"
                    + atsLogin + "'), " + enable + ", '" + crossGroup + "', '" + supervisior + "', '" + modifiedUser + "', GetDate(), '" + dealerEmailID + "', '" + altAECode + "')"
                    + ";SELECT SCOPE_IDENTITY();";

            //sql = sql + ";SELECT @@IDENTITY AS InsertedID";

            
            cmd = genericDA.GetSqlCommand();
            cmd.CommandText = sql;
            insertedIdStr =  cmd.ExecuteScalar().ToString();

            if (!String.IsNullOrEmpty(insertedIdStr))
            {
                result = int.Parse(insertedIdStr);
            }

            //OleDbCommand cmdGetIdentity = genericDA.GetNewSqlCommand();
            //cmdGetIdentity.CommandText = "SELECT SCOPE_IDENTITY()";

            //result = genericDA.ExecuteNonQuery(sql);

            ////Get Inserted ID
            //if(result > 0)
            //    result = int.Parse(cmdGetIdentity.ExecuteScalar().ToString());

            //cmdGetIdentity.Dispose();


           
            return result;
        }

        public DataTable CheckDealerExist(string userId, string dealerCode)
        {
            if (dealerCode.Trim() == "")
            {
                sql = " SELECT * FROM DealerDetail WHERE ( UserID='" + userId + "') ";
            }
            else
            {
                sql = " SELECT * FROM DealerDetail WHERE ( UserID='" + userId + "' AND AECode='" + dealerCode + "' ) ";
            }

            return genericDA.ExecuteQueryForDataTable(sql);
        }

        public int UpdateDealer(string recId, string emailID, string dealerCode, string dealerName, string teamCode, string atsLogin, int enable,
                        string crossGroup, string supervisior, string modifiedUser,string dealerEmailID, string altAECode)
        {
            sql = " UPDATE SPM.dbo.DealerDetail SET UserID = LOWER('" + emailID + "'), AECode = UPPER('" + dealerCode + "'), " +
                    " AEGroup = UPPER('" + teamCode + "'),  AEName = '" + dealerName + "', " +
                    " ATSLogin = UPPER('" + atsLogin + "'), Enable = '" + enable + "', CrossGroup = '" + crossGroup + "', " +
                    " Supervisor = '" + supervisior + "', ModifiedUser = '" + modifiedUser + "', ModifiedDate = GETDATE(), Email = '" + dealerEmailID + "', " +
                    " AltAECode = '" + altAECode + "' " +
                    " WHERE RecId = " + recId;

            return genericDA.ExecuteNonQuery(sql);
        }       
    }
}
