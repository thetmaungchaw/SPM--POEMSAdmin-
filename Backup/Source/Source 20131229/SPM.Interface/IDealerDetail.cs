using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPM.Common;
using System.Data;

namespace SPM.Interface
{
    public interface IDealerDetail
    {
        DataTable DealerDetail_GetAll();
        void DealerDetail_Insert(DealerDetailProperties DealerDetailProperties);
    }
}