using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;

using Autofac;
using Autofac.Builder;

using CQ.SPM.Common;
using CQ.SPM.BusinessLogic.Interface;
using CQ.SPM.BusinessLogic.Implementation;
using CQ.SPM.DataAccess.Interface;
using CQ.SPM.DataAccess.Implementation;

public partial class MainMenu : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("A");

            DataRow dr = dt.NewRow();
            dr["A"] = "0001";
            dr["B"] = "0002";

            dt.Rows.Add(dr);
        }
        catch (Exception ex)
        {
            //log.Debug("Debug error logging", ex);
            //log.Info("Info error logging", ex);
            //log.Warn("Warn error logging", ex);
            //log.Error("Error error logging", ex);
            //log.Fatal("Fatal error logging", ex);
        }
    }

    protected void Button2_Click(object sender, EventArgs e)
    {
        DealerDetailProperties DealerDetailProperties = new DealerDetailProperties();
        DealerDetailProperties.UserID = "1";
        DealerDetailProperties.AECode = "1";

        var builder = new ContainerBuilder();

        builder.Register(c => new DealerDetailBusinessCtrl());
        builder.RegisterType<DealerDetailBusinessCtrl>().As<IDealerDetailBusinessCtrl>();

        builder.Register(c => new DealerDetailDataCtrl());
        builder.RegisterType<DealerDetailDataCtrl>().As<IDealerDetailDataCtrl>();

        builder.Register(c => new AEListDataCtrl());
        builder.RegisterType<AEListDataCtrl>().As<IAEListDataCtrl>();

        using (var container = builder.Build())
        {
            //DataSet ds = container.Resolve<IDealerDetailBusinessCtrl>().DealerDetail_Combine(DealerDetailProperties);
        }
    }
}