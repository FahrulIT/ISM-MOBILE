using ISM_REPAIR_MAINTENANCE.Models;
using ISM_REPAIR_MAINTENANCE.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISM_REPAIR_MAINTENANCE.Repository.Service
{
    public class DashboardService : IDashboard
    {
        protected ApplicationDbContext db;
        public DashboardService()
        {
            db = new ApplicationDbContext();
        }
        public List<decimal> GetChartValue(string dept, string data_type, string trans_type)
        {
            List<decimal> value = new List<decimal>();
            //var temp = db.ChartData.Where(x => x.Dept == dept && x.Data_Type == data_type && x.Trans_type == trans_type);

            //for (int i = 0; i < 15; i = i + 3)
            //{

            //    if (i == 0)
            //    {
            //        continue;
            //    }
            //    else
            //    {
            //        var getValue = temp.Where(x => x.Period == i);
            //        if (getValue != null && getValue.Count() > 0)
            //        {
            //            value.Add(getValue.FirstOrDefault().Qty);
            //        }
            //        else
            //        {
            //            value.Add(0);
            //        }
            //    }
            //}

            return value;
        }

        public decimal GetQuantityValue(string dept, string data_type, string trans_type)
        {
            return 0;
        }






        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    db.Dispose();
                }
                disposedValue = true;
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

      
        #endregion
    }
}