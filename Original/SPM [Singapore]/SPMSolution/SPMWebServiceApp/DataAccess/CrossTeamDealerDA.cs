/* 
 * Purpose:         Assignment Management Webservices data access layer
 * Created By:      Than Htike Tun
 * Date:            04/04/2010
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
    public class CrossTeamDealerDA
    {
        private GenericDA genericDA;
        private string sql;

        public CrossTeamDealerDA(GenericDA da)
        {
            genericDA = da;
        }

        public DataTable RetrieveCrossTeamDealer(string dealerCode, string dealerTeam, string crossTeam)
        {
            sql = "SELECT DD.AECode, DD.AEName, DD.AEGroup AS Original, ISNULL(CA.AEGroup, '') AS Match, DD.Supervisor, CA.ModifiedUser, CA.ModifiedDate " +                       
                    "FROM DealerDetail DD WITH (NOLOCK) " +
                    "LEFT JOIN CommAECode CA WITH (NOLOCK) ON DD.AECode=CA.AECode " +
                    "WHERE Enable = 1 AND DD.CrossGroup = 'Y' ";
                //-- AND DD.AECode = 'TA_CQ'
                //-- AND CA.AEGroup = 'TIB'
                //-- AND	DD.AEGroup = 'TA'

            StringBuilder sb = new StringBuilder(sql);

            if (!String.IsNullOrEmpty(dealerCode))
            {
                sb.Append(" AND DD.AECode = '").Append(dealerCode).Append("' ");
            }

            if (!String.IsNullOrEmpty(crossTeam))
            {
                sb.Append(" AND CA.AEGroup = '").Append(crossTeam).Append("' ");
            }

            if (!String.IsNullOrEmpty(dealerTeam))
            {
                sb.Append(" AND DD.AEGroup = '").Append(dealerTeam).Append("' ");
            }

            return genericDA.ExecuteQueryForDataTable(sb.ToString());
        }        

        public int InsertCrossTeamDealer(string dealerCode, string crossTeam, string modifiedUser)
        {
            sql = " INSERT INTO CommAECode (AECode, AEGroup, ModifiedUser, ModifiedDate) " +
                        " VALUES('" + dealerCode + "', '" + crossTeam + "', '" + modifiedUser + "', GETDATE() ) ";

            return genericDA.ExecuteNonQuery(sql);
        }

        public int DeleteCrossTeamDealer(string dealerCode)
        {
            sql = "DELETE FROM CommAECode WHERE AECode='" + dealerCode + "'";
            return genericDA.ExecuteNonQuery(sql);
        }

        public int UpdateCrossTeamDealer(string dealerCode, string crossTeam, string modifiedUser)
        {
            sql = " UPDATE CommAECode " +
                    " SET AEGroup='" + crossTeam + "', ModifiedUser='" + modifiedUser + "', ModifiedDate=GETDATE() " +
                    " WHERE AECode='" + dealerCode + "' ";

            return genericDA.ExecuteNonQuery(sql);
        }
    }
}
