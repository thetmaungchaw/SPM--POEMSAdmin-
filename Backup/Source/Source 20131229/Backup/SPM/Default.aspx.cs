using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SPM.Common;
using SPM.BusinessLogic.Implementation;
using SPM.BusinessLogic.Interface;
using SPM.DataAccess;
using SPM.DataAccess.Interface;
using SPM.DataAccess.Implementation;

using Autofac;
using Autofac.Builder;

namespace SPM
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            DealerDetailProperties DealerDetailProperties = new DealerDetailProperties();
            DealerDetailProperties.UserID = "1";
            DealerDetailProperties.AECode = "1";

            var builder = new ContainerBuilder();

            //builder.Register(c => new DealerDetailBusinessCtrl(c.Resolve<IDealerDetailBusinessCtrl>()));
            //builder.RegisterType<DealerDetailBusinessCtrl>().As<IDealerDetailBusinessCtrl>();

            //builder.Register(c => new DealerDetailDataCtrl());
            //builder.RegisterType<DealerDetailDataCtrl>().As<IDealerDetailDataCtrl>();


            // Original
            //builder.Register(c => new DealerDetailBusinessCtrl(c.Resolve<IDealerDetail>()));
            //builder.RegisterType<DealerDetailDataCtrl>().As<IDealerDetail>();

            //using (var container = builder.Build(ContainerBuildOptions.Default))
            //{
            //    GridView1.DataSource = container.Resolve<DealerDetailBusinessCtrl>().DealerDetail_GetAll();
            //    GridView1.DataBind();
            //}






            // Original
            //IDealerDetail IDealerDetail = new DealerDetailDataCtrl();
            //DealerDetailBusinessCtrl DealerDetailBusinessCtrl = new DealerDetailBusinessCtrl(IDealerDetail);
            //GridView1.DataSource = DealerDetailBusinessCtrl.DealerDetail_GetAll();
            //GridView1.DataBind();

            // Testing
            //IDealerDetailBusinessCtrl IDealerDetailBusinessCtrl = new DealerDetailBusinessCtrl();
            //GridView1.DataSource = IDealerDetailBusinessCtrl.DealerDetail_GetAll();
            //GridView1.DataBind();

            DbConn DbConn = new DbConn();
            builder.Register(c => new DbConn());

            builder.Register(c => new DealerDetailBusinessCtrl(DbConn));
            builder.RegisterType<DealerDetailBusinessCtrl>().As<IDealerDetailBusinessCtrl>();

            builder.Register(c => new DealerDetailDataCtrl(DbConn));
            builder.RegisterType<DealerDetailDataCtrl>().As<IDealerDetailDataCtrl>();

            builder.Register(c => new AEListDataCtrl(DbConn));
            builder.RegisterType<AEListDataCtrl>().As<IAEListDataCtrl>();

            using (var container = builder.Build(ContainerBuildOptions.Default))
            {
                //GridView1.DataSource = container.Resolve<IDealerDetailBusinessCtrl>().DealerDetail_GetAll();
                //GridView1.DataBind();

                container.Resolve<IDealerDetailBusinessCtrl>().DealerDetail_Combine(DealerDetailProperties);
            }

            //DealerDetailBusinessCtrl DealerDetailBusinessCtrl = new DealerDetailBusinessCtrl(new DealerDetailDataCtrl());
            //DealerDetailBusinessCtrl.DealerDetail_GetAll();
            //DealerDetailBusinessCtrl.DealerDetail_Insert(DealerDetailProperties);

        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            DealerDetailProperties DealerDetailProperties = new DealerDetailProperties();
            DealerDetailProperties.UserID = "1";
            DealerDetailProperties.AECode = "1";

            DbConn DbConn = new DbConn();

            var builder = new ContainerBuilder();
            builder.Register(c => new DbConn());

            builder.Register(c => new DealerDetailBusinessCtrl(DbConn));
            builder.RegisterType<DealerDetailBusinessCtrl>().As<IDealerDetailBusinessCtrl>();

            builder.Register(c => new DealerDetailDataCtrl(DbConn));
            builder.RegisterType<DealerDetailDataCtrl>().As<IDealerDetailDataCtrl>();

            builder.Register(c => new AEListDataCtrl(DbConn));
            builder.RegisterType<AEListDataCtrl>().As<IAEListDataCtrl>();

            using (var container = builder.Build(ContainerBuildOptions.Default))
            {
                //dl.DataSource = container.Resolve<IDealerDetailBusinessCtrl>().DealerDetail_Combine(DealerDetailProperties);
                //dl.DataBind();
            }
        }
    }
}