using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPM.Common;
using System.Data;

namespace SPM.DataAccess.Interface
{
    public interface IAEListDataCtrl
    {
        DataSet AEList_GetAll();
    }
}
