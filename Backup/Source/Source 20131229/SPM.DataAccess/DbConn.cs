using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace SPM.DataAccess
{
    public class DbConn
    {
        public SqlConnection DBConnection;
        public SqlTransaction DBTransaction;

        public DbConn()
        {
            DBConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["main"].ConnectionString);
        }

        #region Connection

        public void OpenConnection()
        {
            if (DBConnection.State == ConnectionState.Closed)
                DBConnection.Open();
        }

        public void CloseConnection()
        {
            if (DBConnection.State == ConnectionState.Open)
                DBConnection.Close();
        }

        #endregion

        #region Transaction

        public void StartTransaction()
        {
            OpenConnection();

            if (DBTransaction == null)
                DBTransaction = DBConnection.BeginTransaction();
        }

        public void CommitTransaction()
        {
            if (DBTransaction != null)
            {
                DBTransaction.Commit();
                DBTransaction = null;
            }

            CloseConnection();
        }

        public void RollBackTransaction()
        {
            if (DBTransaction != null)
                DBTransaction.Rollback();

            CloseConnection();
        }

        #endregion
    }
}
