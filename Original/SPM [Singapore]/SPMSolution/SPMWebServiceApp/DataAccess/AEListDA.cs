using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace SPMWebServiceApp.DataAccess
{
    public class AEListDA
    {
        private GenericDA genericDA;
        private string sql;

        public AEListDA(GenericDA da)
        {
            genericDA = da;
        }

        public DataSet RetrieveAllTeam()
        {
            sql = "SELECT RTRIM(LTRIM(DD.AEGroup)) AS TeamCode, DD.AEGroup + ' - ' + AEL.AE_Name AS TeamName " +
                        "FROM DealerDetail DD WITH (NOLOCK) " +
                        "LEFT JOIN AEList AEL ON DD.AEGroup = AEL.GroupName " +
                        "GROUP BY DD.AEGroup, AEL.AE_Name " +
                        "ORDER BY DD.AEGroup ";

            return genericDA.ExecuteQuery(sql);
        }

        #region SPM III

        /// <summary>
        /// Fetching the Dealer Information: AECode, AEName, Email,GroupName,RefMonthYr columns for each UserID
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public DataTable RetrieveTeamEmailByTeamCode(string teamCode)
        {
            sql = "  SELECT Top 1 AE_CD_Sec,ISNULL(AE_Name,'') As AE_Name,ISNULL(Email,'') As Email,ISNULL(GroupName,'') As GroupName,RefMonthYr FROM [SPM].[dbo].[AEList] WHERE GroupName='" + teamCode + "' order by RefMonthYr desc";

            return genericDA.ExecuteQueryForDataTable(sql);
        }

        public DataTable RetrieveDealerEmailByDelaerCode(string dealerCode)
        {
            sql = "  SELECT DISTINCT AECode,AEName,AEGroup,Email FROM DealerDetail WHERE AECode='" + dealerCode + "' ";

            return genericDA.ExecuteQueryForDataTable(sql);
        }

        #endregion SPM III
    }
}
