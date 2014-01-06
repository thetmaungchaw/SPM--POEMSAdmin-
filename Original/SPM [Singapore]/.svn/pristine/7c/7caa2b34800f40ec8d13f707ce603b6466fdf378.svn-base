/* 
 * Purpose:         Setup Webservices data access layer for Client Short Key setup
 * Created By:      Than Htike Tun
 * Date:            6/4/2010
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
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace SPMWebServiceApp.DataAccess
{
    public class ClientShortKeyDA
    {
        private GenericDA genericDA;
        private string sql;

        public ClientShortKeyDA(GenericDA da)
        {
            genericDA = da;
        }

        public DataTable RetrieveClientShortKey(string accountNo, string shortKey)
        {
            sql = "SELECT SK.AcctNo, SK.ShortKey, SK.UserID, CL.LNAME FROM ShortKey SK WITH (NOLOCK) LEFT JOIN CLMAST CL WITH (NOLOCK) ON SK.AcctNo=CL.LACCT ";
            bool whereFlag = false;
            StringBuilder sb = new StringBuilder(sql);

            if(!String.IsNullOrEmpty(accountNo))
            {
                sb.Append(" WHERE SK.AcctNo = '").Append(accountNo).Append("' ");
                whereFlag = true;
            }

            if (!String.IsNullOrEmpty(shortKey))
            {
                if (whereFlag)
                {
                    sb.Append(" OR SK.ShortKey = '").Append(shortKey).Append("' ");        //previous condition => AND
                }
                else
                {
                    sb.Append(" WHERE SK.ShortKey = '").Append(shortKey).Append("' ");
                }
            }

            return genericDA.ExecuteQueryForDataTable(sb.ToString());
        }

        public DataTable RetrieveClientInfo(string accountNo)
        {
            sql = "SELECT LNAME FROM CLMAST WITH (NOLOCK) WHERE LACCT = '" + accountNo + "'";
            return genericDA.ExecuteQueryForDataTable(sql);
        }

        public int InsertClientShortKey(string userId, string accountNo, string shortKey)
        {
            sql = " INSERT INTO ShortKey VALUES(LOWER('" + userId + "'), UPPER('" + shortKey + "'), UPPER('" + accountNo + "')) ";
            return genericDA.ExecuteNonQuery(sql);
        }

        public int DeleteClientShortKey(string accountNo)
        {
            sql = "DELETE FROM ShortKey WHERE AcctNo = '" + accountNo + "'";
            return genericDA.ExecuteNonQuery(sql);
        }
    }
}
