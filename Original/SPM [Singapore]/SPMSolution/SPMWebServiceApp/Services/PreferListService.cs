using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using SPMWebServiceApp.DataAccess;
using SPMWebServiceApp.Utilities;

namespace SPMWebServiceApp.Services
{
    public class PreferListService
    {
        private GenericDA genericDA;
        private PreferListDA preferListDA;

        public PreferListService()
        {
            genericDA = new GenericDA();
            preferListDA = new PreferListDA(genericDA);
        }

        public PreferListService(string dbConnStr) : this()
        {
            genericDA.SetConnectionString(dbConnStr);
        }

        public DataSet RetrievePerferList(string optionNo, string optionContent)
        {
            DataSet ds = new DataSet();
            DataTable dtReturn = null;

            try
            {
                dtReturn = CommonUtilities.CreateReturnTable("1", "");

                genericDA.OpenConnection();
                ds.Tables.Add(preferListDA.RetrievePerferList(optionNo, optionContent));
                if (ds.Tables[0].Rows.Count < 1)
                {
                    dtReturn.Rows[0]["ReturnCode"] = "0";
                    dtReturn.Rows[0]["ReturnMessage"] = "No record found! Pls change the search criteria!";
                }
            }
            catch (Exception e)
            {
                dtReturn.Rows[0]["ReturnCode"] = "-1";
                dtReturn.Rows[0]["ReturnMessage"] = "Error in retrieving Preferences! Please try again. Exception Message: " + e.Message;
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

        public string[] UpdatePreferList(long recId, string optionNo, string optionContent)
        {
            int result = 1;
            string returnMessage = "Preference is updated successfully.";
            DataTable dtPreference = null;
            try
            {
                genericDA.OpenConnection();
                genericDA.CreateSqlCommand();

                dtPreference = preferListDA.GetPreferenceByOptionNo(optionNo);
                if ((dtPreference.Rows.Count == 0) || ((dtPreference.Rows.Count == 1) && (dtPreference.Rows[0]["RecId"].ToString().Equals(recId.ToString()))))
                {
                    result = preferListDA.UpdatePreferList(recId, optionNo, optionContent);
                    if (result < 1)
                    {
                        returnMessage = "Preference has been update by other user! Please retrieve again.";
                    }
                }
                else
                {
                    result = -2;
                    returnMessage = "Preference is already exists!";
                }

                /*
                if (dtPreference.Rows.Count == 0)
                {
                    result = preferListDA.UpdatePreferList(recId, optionNo, optionContent);
                    if (result < 1)
                    {
                        returnMessage = "Preference has been update by other user! Please retrieve again.";
                    }
                }
                else
                {
                    result = -2;
                    returnMessage = "Preference is already exists!"; 
                }
                */
            }
            catch (Exception e)
            {
                result = -1;
                returnMessage = "Error in updating Preference! Please try again. Exception Message: " + e.Message;
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

        public string[] DeletePreferList(long recId)
        {
            int result = 1;
            string returnMessage = "Preference is deleted successfully.";

            try
            {
                genericDA.OpenConnection();
                genericDA.CreateSqlCommand();

                result = preferListDA.DeletePreferList(recId);
                if (result < 1)
                {
                    returnMessage = "Preference has been delete by other user! Please retrieve again.";
                }
            }
            catch (Exception e)
            {
                result = -1;
                returnMessage = "Error in deleting Preference! Please try again. Exception Message: " + e.Message;
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

        public DataSet AddPreference(string userId, string optionNo, string optionContent)
        {
            DataSet ds = new DataSet();
            DataTable dtReturn = null;
            int result = 1;

            try
            {
                dtReturn = CommonUtilities.CreateReturnTable("1", "");

                genericDA.OpenConnection();

                //ds.Tables.Add(preferListDA.GetPreferenceByOptionNo(optionNo));
                if (preferListDA.GetPreferenceByOptionNo(optionNo).Rows.Count == 0)
                {
                    genericDA.CreateSqlCommand();
                    result = preferListDA.AddPreference(userId, optionNo, optionContent);
                    if (result > 0)
                    {
                        //If Preference is successfully inserted into database, inserted Preference will be returned.
                        ds.Tables.Add(preferListDA.GetPreferenceByOptionNo(optionNo));
                        dtReturn.Rows[0]["ReturnCode"] = "1";
                        dtReturn.Rows[0]["ReturnMessage"] = "Preference is successfully saved.";
                    }
                    else
                    {
                        //Return as Error in insert record
                        dtReturn.Rows[0]["ReturnCode"] = "0";
                        dtReturn.Rows[0]["ReturnMessage"] = "Error in adding Preference! Please try again.";
                    }
                }
                else
                {
                    //If Preference already exists in database, empty DataSet will be returned.
                    //ds.Tables.RemoveAt(0);
                    dtReturn.Rows[0]["ReturnCode"] = "2";
                    dtReturn.Rows[0]["ReturnMessage"] = "Preference is already exists!";
                }
            }
            catch (Exception e)
            {
                dtReturn.Rows[0]["ReturnCode"] = "-1";
                dtReturn.Rows[0]["ReturnMessage"] = "Error in adding Preference! Please try again. Exception Message: " + e.Message;
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

            ds.Tables.Add(dtReturn);
            return ds;
        }
    }
}
