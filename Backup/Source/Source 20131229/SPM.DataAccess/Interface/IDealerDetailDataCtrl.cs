using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPM.Common;
using System.Data;

namespace SPM.DataAccess.Interface
{
    public interface IDealerDetailDataCtrl
    {
        void DealerDetail_Insert(DealerDetailProperties DealerDetailProperties);
        DataSet DealerDetail_GetAll();
    }
}
