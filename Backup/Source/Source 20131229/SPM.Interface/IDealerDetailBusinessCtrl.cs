using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPM.Common;
using System.Data;

namespace SPM.Interface
{
    public interface IDealerDetailBusinessCtrl
    {
        void DealerDetail_Insert(DealerDetailProperties DealerDetailProperties);
        DataSet DealerDetail_GetAll();
    }
}
