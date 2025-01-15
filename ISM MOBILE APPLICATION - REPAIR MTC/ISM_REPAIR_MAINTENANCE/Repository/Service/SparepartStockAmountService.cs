using ISM_REPAIR_MAINTENANCE.Models.ViewModel;
using ISM_REPAIR_MAINTENANCE.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISM_REPAIR_MAINTENANCE.Repository.Service
{
    public class SparepartStockAmountService : GeneralService, ISparepartStockAmount
    {
        public IList<SparepartStockAmount> GetStockAmount()
        {
            var query_str = "select dept_name, stock_amount from tr_repair_mtc_sparepart_stock_amount";
            var datarow = _db.Database.SqlQuery<SparepartStockAmount>(query_str).ToList();
            return datarow;
        }
    }
}