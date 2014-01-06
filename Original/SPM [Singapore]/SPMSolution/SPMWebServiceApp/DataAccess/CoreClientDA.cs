/* 
 * Purpose:         Setup Webservices data access layer for Core Client
 * Created By:      Than Htike Tun
 * Date:            05/4/2010
 * 
 * Change History:
 * --------------------------------------------------------------------------------
 * Modified By      Date        Purpose
 * --------------------------------------------------------------------------------
 * 
 */


using System;
using System.Collections.Generic;
using System.Text;
using System.Data;


namespace SPMWebServiceApp.DataAccess
{
    public class CoreClientDA
    {
        private string sql;
        private GenericDA genericDA;

        public CoreClientDA(GenericDA da)
        {
            genericDA = da;
        }

        public DataTable RetrieveCoreClientList(string AECode, string AccNo)
        {
            DataSet ds = new DataSet();
            string whereClause = "";
            sql = "SELECT CC.RecId, CC.AcctNo, CC.AECode, DD.AEName, CC.ModifiedUser, CONVERT(VARCHAR(20), CC.ModifiedDate, 120) AS ModifiedDate "
                + "FROM CoreClient CC WITH (NOLOCK) LEFT JOIN DealerDetail DD WITH (NOLOCK) ON CC.AECode=DD.AECode";


            if (!String.IsNullOrEmpty(AECode))
            {
                whereClause = " WHERE CC.AECode = '" + AECode + "'";
            }

            if (!String.IsNullOrEmpty(AccNo))
            {
                if (whereClause.Equals(""))
                {
                    whereClause = " WHERE CC.AcctNo = '" + AccNo + "'";
                }
                else
                {
                    whereClause = whereClause + " AND CC.AcctNo = '" + AccNo + "'";
                }
            }

            sql = sql + whereClause;
            return genericDA.ExecuteQueryForDataTable(sql);
        }

        public DataTable CheckForCoreClientTeam(string dealerCode, string accountNo)
        {
            sql = "SELECT CLM.LACCT, CLM.LRNER " +
                    "FROM CLMAST CLM WITH (NOLOCK)  " +
                    "INNER JOIN DealerDetail DD WITH (NOLOCK) ON DD.AEGroup = CLM.LRNER " +
                    "AND DD.AECode = '" + dealerCode + "' " +
                    "AND CLM.LACCT = '" + accountNo + "'";

            return genericDA.ExecuteQueryForDataTable(sql);
        }

        public int AddCoreClient(string AECode, string AccNo, string userId)
        {
            sql = " INSERT INTO CoreClient VALUES( UPPER('" + AECode + "'), '" + AccNo + "', '" +
                        userId + "', GetDate()) ";

            return genericDA.ExecuteNonQuery(sql);
        }

        public bool IsCoreClientExist(string AECode, string AccNo)
        {
            DataSet ds = new DataSet();
            bool result = false;

            //sql = "SELECT * FROM CoreClient WHERE AECode ='" + AECode + "' AND AcctNo ='" + AccNo + "'";
            sql = "SELECT * FROM CoreClient WHERE AcctNo ='" + AccNo + "'";

            ds = genericDA.ExecuteQuery(sql);
            if (ds.Tables[0].Rows.Count > 0)
                result = true;
            
            return result;
        }

        public int DeleteCoreClient(long recId)
        {
            sql = "DELETE FROM CoreClient WHERE RecId = " + recId;
            return genericDA.ExecuteNonQuery(sql);
        }
    }
}
