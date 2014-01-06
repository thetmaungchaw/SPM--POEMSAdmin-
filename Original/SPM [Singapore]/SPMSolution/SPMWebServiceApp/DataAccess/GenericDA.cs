using System;
using System.Data;
using System.Data.OleDb;
//using System.Data.Sql;
//using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;


namespace SPMWebServiceApp.DataAccess
{
    public class GenericDA
    {
        private OleDbConnection cn;
        private OleDbTransaction trans;
        private OleDbCommand cmd;
        private string cnStr;

        //public GenericDA()
        //{
        //    cnStr = "Data Source=10.30.2.242;Initial Catalog=SPM;Integrated Security=True";
        //}

        public void SetConnectionString(string dbConnectionStr)
        {
            cnStr = dbConnectionStr;
        }

        public void OpenConnection()
        {
            cn = new OleDbConnection(cnStr);
            cn.Open();
        }        

        public void CloseConnection()
        {
            if (cn != null)
                cn.Close();
        }

        public OleDbConnection GetSqlConnection()
        {
            return cn;
        }

        public void OpenTranscation()
        {
            trans = cn.BeginTransaction();
            cmd.Transaction = trans;
        }

        public void CommitTranscation()
        {
            trans.Commit();
            trans.Dispose();
        }

        public void RollbackTranscation()
        {
            trans.Rollback();
            trans.Dispose();
        }

        public void CreateSqlCommand()
        {
            cmd = new OleDbCommand();
            cmd.Connection = cn;
            cmd.CommandType = CommandType.Text;           
        }

        public OleDbCommand GetSqlCommand()
        {
            return cmd;
        }

        public OleDbCommand GetNewSqlCommand()
        {
            OleDbCommand cmdNew = new OleDbCommand();
            cmdNew.Connection = cn;
            cmdNew.CommandType = CommandType.Text;

            return cmdNew;
        }        

        public void DisposeSqlCommand()
        {
            if (cmd != null)
            {
                cmd = null;
            }
        }

        public int ExecuteNonQuery(string sqlStr)
        {
            int result = 1;

            try
            {
                cmd.CommandText = sqlStr;
                result = cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.Write(e.StackTrace);
                result = -1;
            }
            return result;
        }

        public DataSet ExecuteQuery(string sqlStr)
        {
            DataSet ds = new DataSet();
            OleDbDataAdapter da = new OleDbDataAdapter(sqlStr, cn);

            da.Fill(ds);
            return ds;
        }

        public DataTable ExecuteQueryForDataTable(string sqlStr)
        {
            DataTable dtResult = new DataTable();
            OleDbDataAdapter da = new OleDbDataAdapter(sqlStr, cn);

            da.Fill(dtResult);
            return dtResult;
        }

        public DataTable ExecuteQueryForDataTable(string sqlStr, string tableName)
        {
            DataTable dtResult = new DataTable(tableName);
            OleDbDataAdapter da = new OleDbDataAdapter(sqlStr, cn);

            da.Fill(dtResult);
            return dtResult;
        }

        public DataSet FillDataSet(DataSet ds, string tableName, string sqlStr)
        {
            OleDbDataAdapter da = new OleDbDataAdapter(sqlStr, cn);
            da.Fill(ds, tableName);          


            return ds;
        }

        /// <summary>
        /// Added by Thet Maung Chaw
        /// </summary>
        /// <returns></returns>
        public OleDbConnection GetSqlConnectionNew()
        {
            return cn = new OleDbConnection(cnStr);
        }
    }
}