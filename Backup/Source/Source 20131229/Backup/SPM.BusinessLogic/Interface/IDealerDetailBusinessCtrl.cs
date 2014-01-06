using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPM.Common;
using System.Data;

namespace SPM.BusinessLogic.Interface
{
    public interface IDealerDetailBusinessCtrl
    {
        void DealerDetail_Insert(DealerDetailProperties DealerDetailProperties);
        DataSet DealerDetail_GetAll();
        DataSet DealerDetail_Combine(DealerDetailProperties DealerDetailProperties); 
    }
}
