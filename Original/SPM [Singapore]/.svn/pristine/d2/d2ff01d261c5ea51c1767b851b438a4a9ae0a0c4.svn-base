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
    public class LeadsService
    {
        private GenericDA genericDA;
        private LeadsDA leadsDA;

        public LeadsService()
        {
            genericDA = new GenericDA();
            leadsDA = new LeadsDA(genericDA);
        }

        public LeadsService(string dbConnStr) : this()
        {
            genericDA.SetConnectionString(dbConnStr);
        }

        public DataSet RetrieveAllLeads()
        {
            DataSet ds = new DataSet();
            DataTable dtReturn = null;

            try
            {
                dtReturn = CommonUtilities.CreateReturnTable("1", "");

                genericDA.OpenConnection();
                ds.Tables.Add(leadsDA.RetrieveAllLeads());                
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

        public DataSet RetrieveLeadsByCriteria(string leadName, string leadNRIC, string leadMobile, string leadHome, string leadGender, string leadEmail, string teamCode, string dealerCode, string dealerName)
        {
            DataSet ds = new DataSet();
            DataTable dtReturn = null;

            try
            {
                dtReturn = CommonUtilities.CreateReturnTable("1", "");

                genericDA.OpenConnection();
                ds.Tables.Add(leadsDA.RetrieveLeadsByCriteria(leadName, leadNRIC,leadMobile,  leadHome,  leadGender,  leadEmail, teamCode,  dealerCode, dealerName));
                if (ds.Tables[0].Rows.Count < 1)
                {
                    dtReturn.Rows[0]["ReturnCode"] = "-1";
                    dtReturn.Rows[0]["ReturnMessage"] = "NO Leads found! Please change your criteria.";
                }
            }
            catch (Exception e)
            {
                dtReturn.Rows[0]["ReturnCode"] = "-1";
                dtReturn.Rows[0]["ReturnMessage"] = "Error in retrieving Leads! Please try again. Exception Message: " + e.Message;
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

        public DataSet  DeleteLeads(string  leadId,string dealerCode)
        {
                
            int result = 1;
            string returnMessage = "Leads record is deleted successfully.";
            DataSet ds = new DataSet();
            DataTable dtReturn = null;

            try
            {
                genericDA.OpenConnection();
                genericDA.CreateSqlCommand();

                result = leadsDA.DeleteLeads(leadId);

                if (result == 2)
                {
                    returnMessage = "This Lead(" + leadId + ") is alread assigned !";
                }
                else
                {

                    if (result < 1)
                    {
                        returnMessage = "Error in deleting Leads record! Leads has been deleted by other user!";
                    }
                    else
                    {
                        ds.Tables.Add(leadsDA.RetrieveLeadsByDealerCode(dealerCode));
                    }
                }
            }
            catch (Exception e)
            {
                result = -1;
                returnMessage = "Error in deleting lead contact record! Please try again. Exception Message: " + e.Message;
            }
            finally
            {
                //Create Table for ReturnCode and Return Message
                dtReturn = CommonUtilities.CreateReturnTable(result.ToString(), returnMessage);
                ds.Tables.Add(dtReturn);

                try
                {
                    genericDA.DisposeSqlCommand();
                    genericDA.CloseConnection();
                }
                catch (Exception e)
                { }
                
            }
            return ds;
        }

        public DataSet RetrieveMaxLeadsID()
        {
            DataSet ds = new DataSet();
            DataTable dtReturn = null;
            
            try
            {
                dtReturn = CommonUtilities.CreateReturnTable("1", "");
                genericDA.OpenConnection();

                ds = leadsDA.RetrieveMaxLeadsID();
                if (ds.Tables[0].Rows.Count < 1)
                {
                    //returnCode = "1";
                    //returnMessage = "";

                    dtReturn.Rows[0]["ReturnCode"] = "-1";
                    dtReturn.Rows[0]["ReturnMessage"] = "";

                }               
            }
            catch (Exception e)
            {
                dtReturn.Rows[0]["ReturnCode"] = "-1";
                dtReturn.Rows[0]["ReturnMessage"] = "Error in retrieving maximum leads id record!";
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

        public DataSet  CheckExistingLead(string LeadName,string LeadMobile, string LeadNRIC)
        {
            DataSet ds = new DataSet();
            DataTable dtReturn = null;
           

            try
            {
                dtReturn = CommonUtilities.CreateReturnTable("1", "");
                genericDA.OpenConnection();
             

                ds = leadsDA.CheckLeadsExist(LeadName, LeadMobile, LeadNRIC);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    dtReturn.Rows[0]["ReturnCode"] = "-2";
                    dtReturn.Rows[0]["ReturnMessage"] = "Leads already exist!";                   
                }
                
            }
            catch (Exception e)
            {
                //insertedID = -1;
                dtReturn.Rows[0]["ReturnCode"] = -1;
                dtReturn.Rows[0]["ReturnMessage"] = "Error in checking Leads! Please try again. Exception Message: " + e.Message;
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

        public DataSet RetrieveExistingLeadsInfo(string LeadName, string LeadNRIC)
        {
            DataSet ds = new DataSet();
            DataTable dtReturn = null;
           
            try
            {
                dtReturn = CommonUtilities.CreateReturnTable("1", "");
                genericDA.OpenConnection();
             
                ds = leadsDA.RetrieveExistingLeadsInfo(LeadName, LeadNRIC);

                ////if (ds.Tables[0].Rows.Count < 0)
                ////{
                ////    dtReturn.Rows[0]["ReturnCode"] = "-2";
                ////    dtReturn.Rows[0]["ReturnMessage"] = "Leads already exist!";                   
                ////}
                
            }
            catch (Exception e)
            {
                //insertedID = -1;
                dtReturn.Rows[0]["ReturnCode"] = -1;
                dtReturn.Rows[0]["ReturnMessage"] = "Error in retrieving Leads! Please try again. Exception Message: " + e.Message;
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
               

        public string[] InsertLeads(string LeadID,string LeadName, string LeadNRIC, string LeadMobile, string LeadHome, string LeadGender, string LeadEmail,
            string LeadEvent,string teamCode, string dealerCode,string inputType)
        {
            int result = 1, insertedID = -1;
            string returnMessage = "Leads record is saved successfully.";
            //DataTable dtLeads = null;

            try
            {
                genericDA.OpenConnection();
                genericDA.CreateSqlCommand();

                //dtLeads = leadsDA.CheckLeadsExist(LeadName,LeadNRIC);
                //if (dtLeads.Rows.Count > 0)
                //{
                //    result = -2;
                //    returnMessage = "Leads already exist!";
                //}
                //else
                //{
                    insertedID = leadsDA.InsertLeads(LeadID,LeadName, LeadNRIC,  LeadMobile, LeadHome,  LeadGender,  LeadEmail,LeadEvent,
             teamCode,  dealerCode, inputType );
                    if (insertedID < 1)
                    {
                        result = -1;
                        returnMessage = "Error in inserting Leads! Please try again.";
                    }
                //}                
            }
            catch (Exception e)
            {
                insertedID = -1;
                result = -1;
                returnMessage = "Error in inserting Leads! Please try again. Exception Message: " + e.Message;
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

        public string[] UpdateLeads(string LeadID, string LeadName, string LeadNRIC, string LeadMobile, string LeadHome, string LeadGender, string LeadEmail,
            string LeadEvent,  string teamCode, string dealerCode)//, string dealerName, string originalDealerCode, string originalUserId)
            //(string recId, string emailID, string dealerCode, string dealerName, string teamCode, string atsLogin, int enable,
                        //string crossGroup, string supervisior, string modifiedUser, 
                        //string originalDealerCode, string originalUserId)
        {
            int result = 1;
            string returnMessage = "Leads record is updated successfully.";
            DataTable dtLeads = null;

            try
            {
                genericDA.OpenConnection();
                genericDA.CreateSqlCommand();

                //dtLeads = leadsDA.RetrieveLeadsByEmailId(LeadEmail);
                //if ( ((LeadEmail.Equals(originalUserId)) && (dtLeads.Rows.Count > 1)) ||
                //     ((!LeadEmail.Equals(originalUserId)) && (dtLeads.Rows.Count > 0))
                //    )
                //{
                //    result = -2;
                //    returnMessage = "Cannot change to existing User ID!";
                //}
                //else
                //{
                //    dtLeads = null;

                //dtLeads = leadsDA.RetrieveLeadsByDealerCode(dealerCode);
                    //if (((dealerCode.Equals(originalDealerCode)) && (dtLeads.Rows.Count > 1)) ||
                    // ((!dealerCode.Equals(originalDealerCode)) && (dtLeads.Rows.Count > 0))
                    //)
                    //{
                    //    result = -2;
                    //    returnMessage = "Cannot change to existing Dealer!";
                    //}
                    //else
                    //{
                result = leadsDA.UpdateLeads(LeadID,LeadName,LeadNRIC ,LeadMobile,LeadHome,LeadGender,LeadEmail,LeadEvent,teamCode, dealerCode);
                if (result < 1)
                {
                    returnMessage = "Leads has been update by other user! Please retrieve again.";
                }
                    //}                    
                //}
            }
            catch (Exception e)
            {
                result = -1;
                returnMessage = "Error in updating Leads! Please try again. Exception Message: " + e.Message;
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

        public string[] MoveToLeadArchive(String syncType, String strCondition)
        {
            int result = 1, insertedID = -1;
            string returnMessage = "Move to Leads Archive is successfully done.";

            try
            {
                genericDA.OpenConnection();
                genericDA.CreateSqlCommand();

                insertedID = leadsDA.MoveToLeadArchive(syncType, strCondition);

                if (insertedID == 0)
                {
                    result = 0;
                    returnMessage = "No Match Data For Leads Archive";
                }
                else if (insertedID < 0)
                {
                    result = -1;
                    returnMessage = "Error in moving to Lead Archive! Please try again.";
                }
            }
            catch (Exception e)
            {
                insertedID = -1;
                result = -1;
                returnMessage = "Error in moving to Lead Archive! Please try again. Exception Message: " + e.Message;
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

        public string[] LeadsDataSync(string syncType, string strCondition)
        {
            int result = 1, insertedID = -1;
            string returnMessage = "Leads record's synchronisatin is successfully done.";
           
            try
            {
                genericDA.OpenConnection();
                genericDA.CreateSqlCommand();
                
                insertedID = leadsDA.LeadsDataSync(syncType,strCondition);

                if (insertedID == 0)
                {
                    result = 0;
                    returnMessage = "No Match Data For Leads Synchronisation! Please try again.";
                }
                else if (insertedID < 0)
                {
                    result = -1;
                    returnMessage = "Error in Leads Synchronisation! Please try again.";
                }
                //}                
            }
            catch (Exception e)
            {
                insertedID = -1;
                result = -1;
                returnMessage = "Error in Leads Synchronisation! Please try again. Exception Message: " + e.Message;
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
    }
}
