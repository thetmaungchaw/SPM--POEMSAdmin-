/* 
 * Purpose:         EmailTemplate Webservices data access layer
 * Created By:      Thiri Lwin
 * Date:            05/10/2011
 * 
 * Change History:
 * --------------------------------------------------------------------------------
 * Modified By      Date        Purpose
 * --------------------------------------------------------------------------------
 *
 * 
 */


using System;
using System.Collections.Generic;
using System.Web;
using System.Data.OleDb;
using System.Text;
using System.Data;

namespace SPMWebServiceApp.DataAccess
{
    public class EmailTemplateDA
    {
        private GenericDA genericDA;
        private string sql;

        public EmailTemplateDA(GenericDA da)
        {
            genericDA = da;
        }

        public DataTable CheckEmailTemplateExist(string templateName)
        {
            sql = " SELECT * FROM SPM.dbo.EmailTemplate WHERE ( TemplateName='" + templateName + "') ";

            return genericDA.ExecuteQueryForDataTable(sql);
        }

        public int UpdateEmailTemplateByID(string templateID, string templateType, string subject, string contents)
        {
            int result = -1;
            OleDbParameter[] oledbParams = null;

            sql = "UPDATE SPM.dbo.EmailTemplate SET TemplateType=?, Subject=?,Contents=?, ModifiedDate=getdate() WHERE TemplateID=?";

            oledbParams=new OleDbParameter[]
            {
                new OleDbParameter("@templateType",OleDbType.Char),
                new OleDbParameter("@subject",OleDbType.VarChar),
                new OleDbParameter("@contents", OleDbType.VarChar),
                new OleDbParameter("@templateID", OleDbType.VarChar)
            };

            OleDbTransaction sqlTrans = null;
            OleDbCommand cmd = null;
            StringBuilder sb = new StringBuilder("");

            try
            {
                sqlTrans = genericDA.GetSqlConnection().BeginTransaction();
                cmd = genericDA.GetSqlCommand();
                cmd.Transaction = sqlTrans;
                cmd.CommandText = sql;
                //Use OleDbParameter class to insert upper comma '
                cmd.Parameters.AddRange(oledbParams);

                cmd.Parameters["@templateType"].Value = templateType;
                cmd.Parameters["@subject"].Value = subject;
                cmd.Parameters["@contents"].Value = contents;
                cmd.Parameters["@templateID"].Value = templateID;
                result = cmd.ExecuteNonQuery();

                sqlTrans.Commit();
            }
            catch (Exception e)
            {
                result = -1;
            }
            finally
            {
                if (result < 1)
                {
                    try
                    {
                        if (sqlTrans != null)
                            sqlTrans.Rollback();
                    }
                    catch (Exception ex)
                    { }
                }
            }

            return result;

        }

        public int InsertEmailTemplate(string templateName, string templateType, string subject, string contents)
        {
            int result = -1;
            OleDbParameter[] oledbParams = null;

            sql = " INSERT INTO SPM.dbo.EmailTemplate(TemplateName, TemplateType, Subject, Contents, ModifiedDate) " +
                   " VALUES(?, ?, ?, ?,getdate()); " +
                   "SELECT SCOPE_IDENTITY();";

            oledbParams = new OleDbParameter[] 
                                        { 
                                            new OleDbParameter("@templateName", OleDbType.VarChar), 
                                            new OleDbParameter("@templateType", OleDbType.Char),
                                            new OleDbParameter("@subject", OleDbType.VarChar),
                                            new OleDbParameter("@contents", OleDbType.VarChar)
                                        };

            OleDbTransaction sqlTrans = null;
            OleDbCommand cmd = null;
            StringBuilder sb = new StringBuilder("");
            try
            {
                sqlTrans = genericDA.GetSqlConnection().BeginTransaction();
                cmd = genericDA.GetSqlCommand();
                cmd.Transaction = sqlTrans;
                cmd.CommandText = sql;
                //Use OleDbParameter class to insert upper comma '
                cmd.Parameters.AddRange(oledbParams);

                cmd.Parameters["@templateName"].Value = templateName;
                cmd.Parameters["@templateType"].Value = templateType;
                cmd.Parameters["@subject"].Value = subject;
                cmd.Parameters["@contents"].Value = contents;

                //result = cmd.ExecuteNonQuery();
                result = Convert.ToInt32(cmd.ExecuteScalar().ToString());

                sqlTrans.Commit();
            }
            catch (Exception e)
            {
                result = -1;
            }
            finally
            {
                if (result < 1)
                {
                    try
                    {
                        if (sqlTrans != null)
                            sqlTrans.Rollback();
                    }
                    catch (Exception ex)
                    { }
                }
            }
            return result;
        }

        public DataSet RetrieveEmailTemplate()
        {
            sql = "SELECT ET.TemplateID, ET.TemplateName, ET.TemplateType, ET.Subject, ET.Contents " +
                  "FROM dbo.EmailTemplate AS ET " +
                  "ORDER BY TemplateID";

            return genericDA.ExecuteQuery(sql);
        }

        public DataSet RetrieveEmailTemplateByID(string templateID)
        {
            sql = "select TemplateID,TemplateName,TemplateType,Subject,Contents,ModifiedDate from SPM.dbo.EmailTemplate " +
                "where TemplateID='" + templateID + "'";

            return genericDA.ExecuteQuery(sql);
        }

        public int DeleteEmailTemplate(string templateID)
        {
            sql = "DELETE FROM SPM.dbo.EmailTemplate WHERE TemplateID = '" + templateID + "'";
            return genericDA.ExecuteNonQuery(sql);
        }

    }
}
