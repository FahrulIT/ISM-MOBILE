using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISM_REPAIR_MAINTENANCE.Repository.Interface
{
    public interface IDashboard :IDisposable
    {


        List<decimal> GetChartValue(string dept, string data_type, string trans_type);
        decimal GetQuantityValue(string dept, string data_type, string trans_type);
    }
}
