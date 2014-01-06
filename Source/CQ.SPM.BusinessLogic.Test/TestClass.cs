using System;

using CQ.SPM.Common;
using CQ.SPM.BusinessLogic.Interface;
using CQ.SPM.BusinessLogic.Implementation;

using NUnit;
using NUnit.Framework;

namespace CQ.SPM.BusinessLogic.Test
{
    [TestFixture]
    public class TestClass
    {
        [TestCase]
        public void DealerDetail_Combine()
        {
            DealerDetailProperties DealerDetailProperties = new DealerDetailProperties();
            DealerDetailProperties.UserID = "1";
            DealerDetailProperties.AECode = "1";

            IDealerDetailBusinessCtrl IDealerDetailBusinessCtrl = new DealerDetailBusinessCtrl();
            System.Data.DataSet ds = IDealerDetailBusinessCtrl.DealerDetail_Combine(DealerDetailProperties);
            Assert.NotNull(ds);
        }
    }
}