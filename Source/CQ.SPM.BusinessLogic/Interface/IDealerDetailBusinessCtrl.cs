using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;

using CQ.SPM.Common;

namespace CQ.SPM.BusinessLogic.Interface
{
    public interface IDealerDetailBusinessCtrl
    {
        DataSet DealerDetail_Combine(DealerDetailProperties DealerDetailProperties);
    }
}