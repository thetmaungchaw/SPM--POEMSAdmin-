using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;

using CQ.SPM.BusinessLogic.Interface;
using CQ.SPM.Common;
using CQ.SPM.DataAccess;
using CQ.SPM.DataAccess.Interface;
using CQ.SPM.DataAccess.Implementation;

// Here is the once-per-application setup information for log4net
[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace CQ.SPM.BusinessLogic.Implementation
{
    public class DealerDetailBusinessCtrl : IDealerDetailBusinessCtrl
    {
        DbConn DbConn;
        private readonly IDealerDetailDataCtrl IDealerDetailDataCtrl;
        private readonly IAEListDataCtrl IAEListDataCtrl;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public DealerDetailBusinessCtrl()
        {
            DbConn = new DbConn();
            IDealerDetailDataCtrl = new DealerDetailDataCtrl(DbConn);
            IAEListDataCtrl = new AEListDataCtrl(DbConn);
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
                log.Debug("Debug error logging", ex);
                log.Info("Info error logging", ex);
                log.Warn("Warn error logging", ex);
                log.Error("Error error logging", ex);
                log.Fatal("Fatal error logging", ex);

                return null;
            }
        }
    }
}
