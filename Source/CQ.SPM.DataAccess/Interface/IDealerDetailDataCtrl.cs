using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;

using CQ.SPM.Common;

namespace CQ.SPM.DataAccess.Interface
{
    public interface IDealerDetailDataCtrl
    {
        void DealerDetail_Insert(DealerDetailProperties DealerDetailProperties);
        DataSet DealerDetail_GetAll();
    }
}