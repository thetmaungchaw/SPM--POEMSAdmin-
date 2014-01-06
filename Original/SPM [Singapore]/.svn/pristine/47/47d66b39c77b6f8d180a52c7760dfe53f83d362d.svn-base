using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace SPMWebServiceApp.Utilities
{
    public class CommonUtilities
    {
        public CommonUtilities()
        { }

        public static DataTable CreateReturnTable(string returnCode, string returnMessage)
        {
            DataTable dtReturn = new DataTable("ReturnTable");
            dtReturn.Columns.Add("ReturnCode", String.Empty.GetType());
            dtReturn.Columns.Add("ReturnMessage", String.Empty.GetType());

            DataRow dr = dtReturn.NewRow();
            dr["ReturnCode"] = returnCode;
            dr["ReturnMessage"] = returnMessage;
            dtReturn.Rows.Add(dr);

            return dtReturn;
        }

        public static string GetClientRank(int clientRank)
        {
            string[] ranks = new string[] { "No Rank", "Excellent Relationship", "Good Relationship", 
                                "Average Relationship", "Fair Relationship", "Poor Relationship"};
            return ranks[clientRank];
        }

        public static MemoryStream SerializeDataTable(DataTable dataTable)
        {
            using (MemoryStream memStream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(memStream, dataTable);
                return memStream;
            }
        }

        public static DataTable DeserializeDataTable(MemoryStream stream)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            if (stream != null)
            {
                stream.Position = 0;
                return formatter.Deserialize(stream) as DataTable;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Created by Thet Maung Chaw.
        /// Select the Clients' records which are synchronised from Leads
        /// </summary>
        /// <param name="AcctNo"></param>
        /// <param name="ProjectID"></param>
        /// <param name="AEGroup"></param>
        /// <param name="AECode"></param>
        /// <param name="Content"></param>
        /// <returns></returns>
        public static String ForLeadSync(String AcctNo, String ProjectID, String AEGroup, String AECode, String Content)
        {
            String SQL = String.Empty;
            Boolean whereFlag = false;

            SQL = "SELECT CC.ContactDate, DD.AEGroup AS Team, CC.AcctNo AS AcctNo,ISNULL(CM.LNAME, '') AS Name, '' AS Sex, ISNULL(CM.LMOBILE, '') AS ContactNo, ISNULL(CC.Content, '') AS Content," + "\r\n";
            SQL += "       CC.Remarks, '' AS Rank, '' AS PreferA, '' AS PreferB, CC.ModifiedUser AS Dealer, NULL AS AssignDate, '' AS AdminId, RankText = 'No Rank'" + "\r\n";
            SQL += "FROM   ClientContact CC" + "\r\n";
            SQL += "       INNER JOIN DealerDetail DD ON CC.ModifiedUser=DD.AECode" + "\r\n";
            SQL += "       INNER JOIN CLMAST CM ON CC.AcctNo=CM.LACCT" + "\r\n";

            if (ProjectID != String.Empty)
            {
                SQL += "WHERE  CC.ProjectID='" + ProjectID + "'" + "\r\n";
                whereFlag = true;
            }

            if (!String.IsNullOrEmpty(AcctNo))
            {
                if (whereFlag)
                {
                    SQL += " AND CC.AcctNo='" + AcctNo + "'" + "\r\n";
                }
                else
                {
                    SQL += " WHERE CC.AcctNo='" + AcctNo + "'" + "\r\n";
                    whereFlag = true;
                }
            }

            if (whereFlag)
            {
                SQL += "       AND CC.ModifiedUser=DD.AECode" + "\r\n";
            }
            else
            {
                SQL += "      WHERE CC.ModifiedUser=DD.AECode" + "\r\n";
                whereFlag = true;
            }

            if (AEGroup != String.Empty)
            {
                if (whereFlag)
                {
                    SQL += "       AND DD.AEGroup='" + AEGroup + "'" + "\r\n";
                }
                else
                {
                    SQL += "       WHERE DD.AEGroup='" + AEGroup + "'" + "\r\n";
                    whereFlag = true;
                }
            }

            if (AECode != String.Empty)
            {
                if (whereFlag)
                {
                    SQL += "       AND CC.ModifiedUser='" + AECode + "'" + "\r\n";
                }
                else
                {
                    SQL += "       WHERE CC.ModifiedUser='" + AECode + "'" + "\r\n";
                    whereFlag = true;
                }
            }

            if (Content != String.Empty)
            {
                if (whereFlag)
                {
                    SQL += " AND CC.Content = '" + Content + "'" + "\r\n";
                }
                else
                {
                    SQL += " WHERE CC.Content = '" + Content + "'" + "\r\n";
                    whereFlag = true;
                }
            }

            return SQL;
        }

        /// <summary>
        /// Created by Thet Maung Chaw.
        /// Search the records which are synchronised from Leads to Clients
        /// </summary>
        /// <returns>String</returns>
        public static String SearchSyncRecords()
        {
            String SQL = String.Empty;

            SQL = " NOT IN (SELECT LD.LeadID" + "\r\n";
            SQL += "                   FROM   LeadContact LC" + "\r\n";
            SQL += "                          INNER JOIN LeadDetail LD" + "\r\n";
            SQL += "                                  ON LC.LeadId = Ld.LeadID" + "\r\n";
            SQL += "                          INNER JOIN CLMAST CM" + "\r\n";
            SQL += "                                  ON ( LD.LeadNRIC = CM.NRIC" + "\r\n";
            SQL += "                                       AND CM.NRIC <> '' )" + "\r\n";
            SQL += "                                      OR ( LD.LeadName = CM.LNAME )" + "\r\n";
            SQL += "                   WHERE  EXISTS (SELECT 1" + "\r\n";
            SQL += "                                  FROM   ClientContact" + "\r\n";
            SQL += "                                  WHERE  CM.LACCT = ClientContact.AcctNo" + "\r\n";
            SQL += "                                         AND LC.ContactDate = ClientContact.ContactDate))" + "\r\n";

            return SQL;
        }
    }
}
