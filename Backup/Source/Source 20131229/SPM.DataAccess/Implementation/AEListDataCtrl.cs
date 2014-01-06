using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPM.Common;
using SPM.Interface;
using System.Data;
using System.Data.SqlClient;
using SPM.DataAccess.Interface;

namespace SPM.DataAccess.Implementation
{
    public class AEListDataCtrl : IAEListDataCtrl
    {
        SqlCommand Command;
        SqlDataAdapter DataAdapter;
        DbConn DbConn;

        public AEListDataCtrl(DbConn DbConn)
        {
            this.DbConn = DbConn;
        }

        public DataSet AEList_GetAll()
        {
            //DbConn.StartTransaction();

            Command = new SqlCommand("SELECT TOP 10 * FROM AEList", DbConn.DBConnection, DbConn.DBTransaction);
            DataAdapter = new SqlDataAdapter(Command);
            DataSet ds = new DataSet();
            DataAdapter.Fill(ds);

            //DbConn.CommitTransaction();

            return ds;
        }
    }
}