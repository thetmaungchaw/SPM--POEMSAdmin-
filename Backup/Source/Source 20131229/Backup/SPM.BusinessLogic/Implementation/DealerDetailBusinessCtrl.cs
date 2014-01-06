using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPM.Common;
using SPM.DataAccess;
using SPM.Interface;
using System.Data;
using System.Web.Services;


using SPM.BusinessLogic.Interface;
using SPM.DataAccess.Interface;
using SPM.DataAccess.Implementation;

namespace SPM.BusinessLogic.Implementation
{
    public class DealerDetailBusinessCtrl : IDealerDetailBusinessCtrl
    {
        //private readonly IDealerDetail IDealerDetail;

        //public DealerDetailBusinessCtrl(IDealerDetail IDealerDetail)
        //{
        //    this.IDealerDetail = IDealerDetail;
        //}

        DbConn DbConn;
        private readonly IDealerDetailDataCtrl IDealerDetailDataCtrl;
        private readonly IAEListDataCtrl IAEListDataCtrl;

        public DealerDetailBusinessCtrl(DbConn DbConn)
        {
            this.DbConn = DbConn;
            IDealerDetailDataCtrl = new DealerDetailDataCtrl(DbConn);
            IAEListDataCtrl = new AEListDataCtrl(DbConn);
        }

        public void DealerDetail_Insert(DealerDetailProperties DealerDetailProperties)
        {
            try
            {
                //IDealerDetailBusinessCtrl.DealerDetail_Insert(DealerDetailProperties);
                IDealerDetailDataCtrl.DealerDetail_Insert(DealerDetailProperties);
            }
            catch (Exception ex)
            {  
                throw ex;
            }
        }

        public DataSet DealerDetail_GetAll()
        {
            try
            {
                //return IDealerDetailBusinessCtrl.DealerDetail_GetAll();
                return IDealerDetailDataCtrl.DealerDetail_GetAll();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public DataSet DealerDetail_Combine(DealerDetailProperties DealerDetailProperties)
        {
            try
            {
                DbConn.StartTransaction();

                DataSet ds = IDealerDetailDataCtrl.DealerDetail_GetAll();

                IAEListDataCtrl.AEList_GetAll();

                DbConn.CommitTransaction();

                return ds;
            }
            catch (Exception ex)
            {
                DbConn.RollBackTransaction();
                throw ex;
            }
        }
    }
}