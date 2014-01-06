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
    public class DealerService
    {
        private GenericDA genericDA;
        private DealerDA dealerDA;

        public DealerService()
        {
            genericDA = new GenericDA();
            dealerDA = new DealerDA(genericDA);
        }

        public DealerService(string dbConnStr) : this()
        {
            genericDA.SetConnectionString(dbConnStr);
        }

        public DataSet RetrieveAllDealer()
        {
            DataSet ds = new DataSet();
            DataTable dtReturn = null;

            try
            {
                dtReturn = CommonUtilities.CreateReturnTable("1", "");

                genericDA.OpenConnection();
                ds.Tables.Add(dealerDA.RetrieveAllDealer());                
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

    
        public DataSet RetrieveDealerByCriteria(string emailID, string dealerCode, string dealerName, string teamCode, string atsLogin, int enable,
                    string crossGroup, string supervisior,string dealerEmailID, String UserID)
        {
            DataSet ds = new DataSet();
            DataTable dtReturn = null;

            try
            {
                dtReturn = CommonUtilities.CreateReturnTable("1", "");

                genericDA.OpenConnection();
                ds.Tables.Add(dealerDA.RetrieveDealerByCriteria(emailID, dealerCode, dealerName, teamCode, atsLogin, enable, crossGroup, supervisior, dealerEmailID, UserID));
                if (ds.Tables[0].Rows.Count < 1)
                {
                    dtReturn.Rows[0]["ReturnCode"] = "-1";
                    dtReturn.Rows[0]["ReturnMessage"] = "NO Dealers found! Please change your criteria.";
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

        public string[] DeleteDealer(long recId)
        {
            //DataSet ds = new DataSet();
            int result = -1;
            string returnMessage = "Dealer record is deleted successfully.";

            try
            {
                genericDA.OpenConnection();
                genericDA.CreateSqlCommand();

                result = dealerDA.DeleteDealer(recId);
                if (result < 1)
                {
                    returnMessage = "Error in deleting Dealer record! The Dealer record has been deleted by other user.";
                }
            }
            catch (Exception e)
            {
                result = -1;
                returnMessage = "Error in deleting Dealer! Please try again. Exception Message: " + e.Message;
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

            return new string[] { result + "", returnMessage }; 
        }

        public string[] DeleteDealerByUserId(string userId)
        {
            //DataSet ds = new DataSet();
            int result = -1;
            string returnMessage = "Dealer record is deleted successfully.";

            try
            {
                genericDA.OpenConnection();
                genericDA.CreateSqlCommand();

                result = dealerDA.DeleteDealerByUserID(userId);
                if (result < 1)
                {
                    returnMessage = "Error in deleting Dealer record! The Dealer record has been deleted by other user. blah blah";
                }
            }
            catch (Exception e)
            {
                result = -1;
                returnMessage = "Error in deleting Dealer! Please try again. Exception Message: " + e.Message;
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

            return new string[] { result + "", returnMessage };
        }

        public string[] InsertDealer(string emailID, string dealerCode, string dealerName, string teamCode, string atsLogin, int enable, string crossGroup,
                        string supervisior, string modifiedUser, string dealerEmailID, string altAECode)
        {
            int result = 1, insertedID = -1;
            string returnMessage = "Dealer record is inserted successfully.";
            DataTable dtDealer = null;

            try
            {
                genericDA.OpenConnection();
                genericDA.CreateSqlCommand();

                dtDealer = dealerDA.CheckDealerExist(emailID, dealerCode);
                if (dtDealer.Rows.Count > 0)
                {
                    result = -2;
                    returnMessage = "Dealer already exists!";
                }
                else
                {
                    insertedID = dealerDA.InsertDealer(emailID, dealerCode, dealerName, teamCode, atsLogin, enable, crossGroup, supervisior, modifiedUser, dealerEmailID, altAECode);
                    if (insertedID < 1)
                    {
                        result = -1;
                        returnMessage = "Error in inserting Dealer! Please try again.";
                    }
                }
                
            }
            catch (Exception e)
            {
                insertedID = -1;
                result = -1;
                returnMessage = "Error in inserting Dealer! Please try again. Exception Message: " + e.Message;
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

            return new string[] { result.ToString(), returnMessage, insertedID.ToString() };
        }

        public string[] CheckDealerExists(string userid)
        {
            DataTable dtDealer = null;
            int result = 1, insertedID = -1;
            string returnMessage = "Dealer not exists.";
            
            genericDA.OpenConnection();
            genericDA.CreateSqlCommand();

            dtDealer = dealerDA.CheckDealerExist(userid, string.Empty);
            if (dtDealer.Rows.Count > 0)
            {
                result = -2;
                returnMessage = "Dealer already exists!";
            }
            return new string[] { result.ToString(), returnMessage, insertedID.ToString() };
        }

        public string[] UpdateDealer(string recId, string emailID, string dealerCode, string dealerName, string teamCode, string atsLogin, int enable,
                        string crossGroup, string supervisior, string modifiedUser, 
                        string originalDealerCode, string originalUserId,string dealerEmailID, string altAECode)
        {
            int result = 1;
            string returnMessage = "Dealer record is updated successfully.";
            DataTable dtDealer = null;

            try
            {
                genericDA.OpenConnection();
                genericDA.CreateSqlCommand();

                dtDealer = dealerDA.RetrieveDealerByUserId(emailID);
                //if ( ((emailID.Equals(originalUserId)) && (dtDealer.Rows.Count > 1)) ||
                //     ((!emailID.Equals(originalUserId)) && (dtDealer.Rows.Count > 0))
                //    )
                //{
                //    result = -2;
                //    returnMessage = "Cannot change to existing User ID!";
                //}
                //else
                //{
                    dtDealer = null;

                    dtDealer = dealerDA.RetrieveDealerByDealerCode(dealerCode);
                    if (((dealerCode.Equals(originalDealerCode)) && (dtDealer.Rows.Count > 1)) ||
                     ((!dealerCode.Equals(originalDealerCode)) && (dtDealer.Rows.Count > 0))
                    )
                    {
                        result = -2;
                        returnMessage = "Cannot change to existing Dealer!";
                    }
                    else
                    {
                        result = dealerDA.UpdateDealer(recId, emailID, dealerCode, dealerName, teamCode, atsLogin, enable, crossGroup, supervisior, modifiedUser,dealerEmailID, altAECode);
                        if (result < 1)
                        {
                            returnMessage = "Dealer has been update by other user! Please retrieve again.";
                        }
                    }                    
                //}
            }
            catch (Exception e)
            {
                result = -1;
                returnMessage = "Error in updating Dealer! Please try again. Exception Message: " + e.Message;
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

            return new string[] { result.ToString(), returnMessage};
        }
    }
}