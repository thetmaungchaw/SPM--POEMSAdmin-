using System;
using System.Collections.Generic;
using System.Web;
using SPMWebServiceApp.DataAccess;
using System.Data;
using SPMWebServiceApp.Utilities;

namespace SPMWebServiceApp.Services
{
    public class EmailTemplateService
    {
        private EmailTemplateDA emailTemplateDA;
        private GenericDA genericDA;
        private DataTable dtReturn;

        private string returnCode = "1";
        private string returnMessage = "";



        public EmailTemplateService()
        {
            genericDA = new GenericDA();
            emailTemplateDA = new EmailTemplateDA(genericDA);
        }

        public EmailTemplateService(string dbConnectionStr)
            : this()
        {
            genericDA.SetConnectionString(dbConnectionStr);
        }


        public string[] InsertEmailTemplate(string templateName, string templateType, string subject, string contents)
        {
            int result = 1;
            string returnMessage = "Email Template is successfully created.";
            DataTable dtTemplateName = null;
            try
            {
                genericDA.OpenConnection();
                genericDA.CreateSqlCommand();

                dtTemplateName = emailTemplateDA.CheckEmailTemplateExist(templateName);
                if (dtTemplateName.Rows.Count > 0)
                {
                    result = -2;
                    returnMessage = "Template name already exists!";
                }
                else
                {
                    result = emailTemplateDA.InsertEmailTemplate(templateName, templateType, subject, contents);
                    if (result < 1)
                        returnMessage = "Error in creating email template!";

                }
                genericDA.DisposeSqlCommand();
                genericDA.CloseConnection();
                
            }
            catch (Exception e)
            {
                result = -2;

                returnMessage = "Error in creating email template!" + "<br />";
                returnMessage = returnMessage + e.Message;
            }

            return new string[] { result + "", returnMessage };
        }

        public DataSet RetrieveEmailTemplate()
        {
            DataSet ds = new DataSet();

            try
            {
                genericDA.OpenConnection();

                ds = emailTemplateDA.RetrieveEmailTemplate();
                if (ds.Tables[0].Rows.Count == 0)
                {
                    returnCode = "0";
                    returnMessage = "Email template not found!!";
                }
            }
            catch (Exception e)
            {
                returnCode = "-1";
                returnMessage = "Error in retrieving email templates!";
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

        public DataSet RetrieveEmailTemplateByID(string templateID)
        {
            DataSet ds = new DataSet();

            try
            {
                genericDA.OpenConnection();

                ds = emailTemplateDA.RetrieveEmailTemplateByID(templateID);
                if (ds.Tables[0].Rows.Count == 0)
                {
                    returnCode = "0";
                    returnMessage = "Email template not found!";
                }
            }
            catch (Exception e)
            {
                returnCode = "-1";
                returnMessage = "Error in retrieving email templates!";
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

        public string[] UpdateEmailTemplateByID(string templateID, string templateType, string subject, string contents)
        {
            int result = -1;
            string returnMessage = "Email Template is successfully updated.";

            try
            {
                genericDA.OpenConnection();
                genericDA.CreateSqlCommand();

                result = emailTemplateDA.UpdateEmailTemplateByID(templateID, templateType, subject, contents);
                if (result < 1)
                {
                    returnMessage = "Error in updating email template record!";
                }
            }
            catch (Exception e)
            {
                result = -1;
                returnMessage = "Error in updating email template record!";
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

        public DataSet DeleteEmailTemplate(string templateID)
        {
            int result = -1;
            string returnMessage = "Email Template is successfully deleted.";
            DataSet ds = new DataSet();
            DataTable dtReturn = null;

            try
            {
                genericDA.OpenConnection();
                genericDA.CreateSqlCommand();

                result = emailTemplateDA.DeleteEmailTemplate(templateID);
                if (result < 1)
                {
                    returnMessage = "Error in deleting email template record!";
                }
            }
            catch (Exception e)
            {
                result = -1;
                returnMessage = "Error in deleting email template record!";
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
            //return new string[] { result.ToString(), returnMessage }; 

            return ds;
        }

    }
}
