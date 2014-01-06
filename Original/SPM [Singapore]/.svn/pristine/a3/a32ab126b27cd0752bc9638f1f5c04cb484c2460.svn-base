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
    public class CrossTeamDealerService
    {
        private GenericDA genericDA;
        private CrossTeamDealerDA crossTeamDealerDA;

        public CrossTeamDealerService()
        {
            genericDA = new GenericDA();
            crossTeamDealerDA = new CrossTeamDealerDA(genericDA);
        }

        public CrossTeamDealerService(string dbConnStr) : this()
        {
            genericDA.SetConnectionString(dbConnStr);
        }

        public DataSet PrepareForCrossTeamSetup()
        {
            CommonServiceDA commonServiceDA = new CommonServiceDA(genericDA);
            DataSet ds = new DataSet();
            DataTable dtReturn = null;
            string returnCode = "1", returnMessage = "";

            try
            {
                genericDA.OpenConnection();

                ds.Tables.Add(commonServiceDA.RetrieveTeamCodeAndNameByDataTable());
                if (ds.Tables[0].Rows.Count < 1)
                {
                    returnCode = "0";
                    returnMessage = "Dealer Team cannot retrieve.";
                }

                //ds.Tables.Add(commonServiceDA.RetrieveAllDealer());
                ds.Tables.Add(commonServiceDA.RetrieveCrossEnableDealer());
                if (ds.Tables[1].Rows.Count < 1)
                {
                    returnCode = "0";
                    returnMessage = "Dealers cannot retrieve.";
                }
            }
            catch (Exception e)
            {
                returnCode = "-1";
                returnMessage = "";
                //returnMessage = "Error in retrieving Call Report Details records! <br /> Error Message : " + e.Message;
            }
            finally
            {
                //Create Table for ReturnCode and Return Message
                dtReturn = CommonUtilities.CreateReturnTable(returnCode, returnMessage);
                ds.Tables.Add(dtReturn);

                try
                {
                    genericDA.CloseConnection();
                }
                catch (Exception e)
                { }
            }

            return ds;
        }

        public DataSet RetrieveCrossTeamDealer(string dealerCode, string dealerTeam, string crossTeam)
        {
            DataSet ds = new DataSet();
            DataTable dtReturn = null;

            try
            {
                dtReturn = CommonUtilities.CreateReturnTable("1", "");

                genericDA.OpenConnection();
                ds.Tables.Add(crossTeamDealerDA.RetrieveCrossTeamDealer(dealerCode, dealerTeam, crossTeam));
                if (ds.Tables[0].Rows.Count < 1)
                {
                    dtReturn.Rows[0]["ReturnCode"] = "-1";
                    dtReturn.Rows[0]["ReturnMessage"] = "NO Dealers found! Please change your search criteria.";
                }
            }
            catch (Exception e)
            {
                dtReturn.Rows[0]["ReturnCode"] = "-1";
                dtReturn.Rows[0]["ReturnMessage"] = "Error in retrieving Dealers! Please try again. Exception Message: " + e.Message;
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

        public string[] InsertCrossTeamDealer(string dealerCode, string crossTeam, string modifiedUser)
        {
            int result = 1;
            string returnMessage = "Dealer matching is successfully updated.";

            try
            {
                genericDA.OpenConnection();
                genericDA.CreateSqlCommand();

                result = crossTeamDealerDA.InsertCrossTeamDealer(dealerCode, crossTeam, modifiedUser);
                if (result < 1)
                {
                    returnMessage = "Error in updating dealer matching record!";
                }

            }
            catch (Exception e)
            {
                result = -1;
                returnMessage = "Error in updating dealer matching record! Exception Message: " + e.Message;
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

        public string[] DeleteCrossTeamDealer(string dealerCode)
        {
            int result = 1;
            string returnMessage = "Dealer matching is successfully updated.";

            try
            {
                genericDA.OpenConnection();
                genericDA.CreateSqlCommand();

                result = crossTeamDealerDA.DeleteCrossTeamDealer(dealerCode);
                if (result < 1)
                {
                    returnMessage = "Error in updating dealer matching record!";
                }
            }
            catch (Exception e)
            {
                result = -1;
                returnMessage = "Error in updating dealer matching record! Exception Message: " + e.Message;
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

        public string[] UpdateCrossTeamDealer(string dealerCode, string crossTeam, string modifiedUser)
        {
            int result = 1;
            string returnMessage = "Dealer matching is successfully updated.";

            try
            {
                genericDA.OpenConnection();
                genericDA.CreateSqlCommand();

                result = crossTeamDealerDA.UpdateCrossTeamDealer(dealerCode, crossTeam, modifiedUser);
                if (result < 1)
                {
                    returnMessage = "Error in updating dealer matching record!";
                }
            }
            catch (Exception e)
            {
                result = -1;
                returnMessage = "Error in updating dealer matching record! Exception Message: " + e.Message;
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
