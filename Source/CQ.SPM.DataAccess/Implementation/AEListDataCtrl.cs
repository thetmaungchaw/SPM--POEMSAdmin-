using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Data.SqlClient;

using CQ.SPM.DataAccess.Interface;

namespace CQ.SPM.DataAccess.Implementation
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

        public AEListDataCtrl()
        {

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
