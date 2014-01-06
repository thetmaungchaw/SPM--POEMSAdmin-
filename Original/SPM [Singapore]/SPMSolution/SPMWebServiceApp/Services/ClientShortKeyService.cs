using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using SPMWebServiceApp.Utilities;
using SPMWebServiceApp.DataAccess;

namespace SPMWebServiceApp.Services
{
    public class ClientShortKeyService
    {
        private GenericDA genericDA;
        private ClientShortKeyDA clientShortKeyDA;

        public ClientShortKeyService()
        {
            genericDA = new GenericDA();
            clientShortKeyDA = new ClientShortKeyDA(genericDA);
        }

        public ClientShortKeyService(string dbConnStr) : this()
        {
            genericDA.SetConnectionString(dbConnStr);
        }

        public DataSet RetrieveClientShortKey(string accountNo, string shortKey)
        {
            DataSet ds = new DataSet();
            DataTable dtReturn = null;

            try
            {
                dtReturn = CommonUtilities.CreateReturnTable("1", "");

                genericDA.OpenConnection();
                ds.Tables.Add(clientShortKeyDA.RetrieveClientShortKey(accountNo, shortKey));
                if (ds.Tables[0].Rows.Count < 1)
                {
                    dtReturn.Rows[0]["ReturnCode"] = "0";
                    dtReturn.Rows[0]["ReturnMessage"] = "No Short Key found! Please change your search criteria.";
                }
            }
            catch (Exception e)
            {
                dtReturn.Rows[0]["ReturnCode"] = "-1";
                dtReturn.Rows[0]["ReturnMessage"] = "Error in retrieving Client Short Key! Please try again. Exception Message: " + e.Message;
            }
            finally
            {
                try
                {
                    genericDA.CloseConnection();
                }
                catch (Exception e)
                { }
            }

            ds.Tables.Add(dtReturn);
            return ds;
        }

        public string[] InsertClientShortKey(string userId, string accountNo, string shortKey)
        {
            int result = 1;
            string returnMessage = "The short key is successfully saved", clientName = "";
            DataTable dtShortKey = null;
            DataTable dtClient = null;

            try
            {
                genericDA.OpenConnection();
                genericDA.CreateSqlCommand();

                dtClient = clientShortKeyDA.RetrieveClientInfo(accountNo);
                if (dtClient.Rows.Count > 0)
                {
                    dtShortKey = clientShortKeyDA.RetrieveClientShortKey("", shortKey);
                    if (dtShortKey.Rows.Count == 0)
                    {
                        result = clientShortKeyDA.InsertClientShortKey(userId, accountNo, shortKey);
                        if (result < 1)
                        {
                            //returnMessage = "Error in saving the short key!";
                            returnMessage = "User cannot add same account number!";
                        }
                        else
                        {
                            clientName = dtClient.Rows[0]["LNAME"].ToString();
                        }
                    }
                    else
                    {
                        result = 0;
                        //returnMessage = "Short Key: " + shortKey + " has been used for A/C No: " + accountNo;
                        returnMessage = "Short Key: " + shortKey + " has been used for A/C No: "
                                            + dtShortKey.Rows[0]["AcctNo"].ToString();
                    }
                }
                else
                {
                    result = -2;
                    returnMessage = "Account Number does not exist!";
                }
            }
            catch (Exception e)
            {
                result = -1;
                returnMessage = "Error in saving the short key! Exception Message: " + e.Message;
            }
            finally
            {
                try
                {
                    genericDA.DisposeSqlCommand();
                    genericDA.CloseConnection();
                }
                catch (Exception e)
                { }
            }

            return new string[] { result.ToString(), returnMessage, clientName};
        }

        public string[] DeleteClientShortKey(string accountNo)
        {
            int result = 1;
            string returnMessage = "Short Key Successfully deleted.";

            try
            {
                genericDA.OpenConnection();
                genericDA.CreateSqlCommand();

                result = clientShortKeyDA.DeleteClientShortKey(accountNo);
                if (result < 1)
                {
                    returnMessage = "Client Short Key has been deleted by other user!";
                }
            }
            catch (Exception e)
            {
                result = -1;
                returnMessage = "Error in deleting dealer short key! Exception Message: " + e.Message;
            }
            finally
            {
                try
                {
                    genericDA.DisposeSqlCommand();
                    genericDA.CloseConnection();
                }
                catch (Exception e)
                { }
            }

            return new string[] { result.ToString(), returnMessage };
        }
    }
}
