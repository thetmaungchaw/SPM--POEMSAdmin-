using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;

namespace CQ.SPM.DataAccess.Interface
{
    public interface IAEListDataCtrl
    {
        DataSet AEList_GetAll();
    }
}
