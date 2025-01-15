using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISM_REPAIR_MAINTENANCE.Repository.Interface
{
   public interface IFixCost :IDisposable
    {
        decimal GetViewBagPersentase(int dept, string cost_type, int bulan, int tahun);
        decimal GetViewBagDetailAmount(int dept, string cost_type, string amount_type, int bulan, int tahun);

        //void GetFixCostData();
    }
}
