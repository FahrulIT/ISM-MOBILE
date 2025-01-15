using ISM_REPAIR_MAINTENANCE.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISM_REPAIR_MAINTENANCE.Repository.Interface
{
    public interface IAgingSparepart :IDisposable
    {
        AgingSparePart GetViewBag_AgingSparepart();
    }
}
