﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Data.SqlClient;

using CQ.SPM.Common;
using CQ.SPM.DataAccess.Interface;

namespace CQ.SPM.DataAccess.Implementation
{
    public class DealerDetailDataCtrl : IDealerDetailDataCtrl
    {
        SqlCommand Command;
        SqlDataAdapter DataAdapter;
        DbConn DbConn;

        public DealerDetailDataCtrl(DbConn DbConn)
        {
            //DbConn = new DbConn();
            this.DbConn = DbConn;
        }

        public DealerDetailDataCtrl()
        {
        
        }

        public void DealerDetail_Insert(DealerDetailProperties DealerDetailProperties)
        {
            //DbConn.StartTransaction();

            Command = new SqlCommand("DealerDetail_Insert", DbConn.DBConnection, DbConn.DBTransaction);
            Command.CommandType = System.Data.CommandType.StoredProcedure;
            Command.Parameters.AddWithValue("UserID", DealerDetailProperties.UserID);
            Command.Parameters.AddWithValue("AECode", DealerDetailProperties.AECode);
            Command.ExecuteNonQuery();

            //DbConn.CommitTransaction();
        }

        public DataSet DealerDetail_GetAll()
        {
            //DbConn.StartTransaction();

            Command = new SqlCommand("SELECT TOP 10 * FROM DealerDetail", DbConn.DBConnection, DbConn.DBTransaction);
            DataAdapter = new SqlDataAdapter(Command);
            DataSet ds = new DataSet();
            DataAdapter.Fill(ds);

            //DbConn.CommitTransaction();

            return ds;
        }
    }
}
