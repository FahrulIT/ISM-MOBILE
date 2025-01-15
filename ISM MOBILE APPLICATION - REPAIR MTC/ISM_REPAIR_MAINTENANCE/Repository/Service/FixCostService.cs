using ISM_REPAIR_MAINTENANCE.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISM_REPAIR_MAINTENANCE.Repository.Service
{
    public class FixCostService : GeneralService, IFixCost
    {

        public decimal GetViewBagPersentase(int dept, string cost_type, int bulan, int tahun)
        {
            var result = _db.repair_mtc_fix_cost_persentase.Where(x => x.dept == dept && x.cost_type == cost_type && x.tahun == tahun && x.bulan == bulan);
            if (result.Count() > 0)
            {
                return Math.Round(result.FirstOrDefault().persentase,2);
            }
            else
            {
                return 0;
            }
        }

        public decimal GetViewBagDetailAmount(int dept, string cost_type, string amount_type, int bulan, int tahun)
        {
            var result = _db.repair_mtc_fix_cost_amount.Where(x => x.dept == dept && x.cost_type == cost_type && x.amount_type == amount_type && x.tahun == tahun && x.bulan == bulan);
            if (result.Count() > 0)
            {
                return Math.Round(result.FirstOrDefault().amount, 2);
            }
            else
            {
                return 0;
            }
        }

        //public void GetFixCostData()
        //{
        //    var query = "exec z_insert_repair_mtc_fix_cost_persentase";
        //    _db.Database.ExecuteSqlCommand(query);
        //}
    }
}