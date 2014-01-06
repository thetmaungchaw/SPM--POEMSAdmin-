using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace SPMWebServiceApp.DataAccess
{
    public class PreferListDA
    {
        private string sql;
        private GenericDA genericDA;

        public PreferListDA(GenericDA da)
        {
            genericDA = da;
        }

        public DataTable RetrievePerferList(string optionNo, string optionContent)
        {
            string whereClause = "";
            sql = " SELECT PL.RecId, PL.OptionNo, PL.OptionContent, PL.ModifiedUser, PL.ModifiedDate " +
                "FROM PreferenceList PL WITH (NOLOCK) ";

            if (!String.IsNullOrEmpty(optionNo))
            {
                whereClause = " WHERE OptionNo LIKE '%" + optionNo + "%'";
            }

            if (!String.IsNullOrEmpty(optionContent))
            {
                if (whereClause.Equals(""))
                {
                    whereClause = " WHERE OptionContent LIKE '%" + optionContent + "%'";
                }
                else
                {
                    whereClause = whereClause + " AND OptionContent LIKE '%" + optionContent + "%'";
                }
            }

            sql = sql + whereClause + " ORDER BY OptionNo";
            return genericDA.ExecuteQueryForDataTable(sql);
        }

        public int UpdatePreferList(long recId, string optionNo, string optionContent)
        {            
            sql = "UPDATE PreferenceList SET OptionNo = '" + optionNo + "', OptionContent = '" + optionContent
                        + "' WHERE RecId = " + recId;

            return genericDA.ExecuteNonQuery(sql);
        }

        public int DeletePreferList(long recId)
        {
            sql = "DELETE FROM PreferenceList WHERE RecId = " + recId;

            return genericDA.ExecuteNonQuery(sql);
        }

        public int AddPreference(string userId, string optionNo, string optionContent)
        {
            sql = " INSERT INTO PreferenceList( OptionNo, OptionContent, ModifiedUser, ModifiedDate) " +
                    " VALUES( '" + optionNo + "', '" + optionContent + "', '" + userId + "', GetDate()) ";

            return genericDA.ExecuteNonQuery(sql);
        }

        public DataTable GetPreferenceByOptionNo(string optionNo)
        {
            
            DataSet ds = null;
            sql = "SELECT * FROM PreferenceList WHERE OptionNo = '" + optionNo + "'";
            return genericDA.ExecuteQueryForDataTable(sql);
        }
    }
}
