using ISM_REPAIR_MAINTENANCE.Models.ViewModel;
using ISM_REPAIR_MAINTENANCE.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISM_REPAIR_MAINTENANCE.Repository.Service
{
    public class AgingSparepartService : GeneralService, IAgingSparepart
    {
        public AgingSparePart GetViewBag_AgingSparepart()
        {
            var query_string = "select * from dbo.v_temp_ism_mobile_fixcost_agingsparepart";
            var result = _db.Database.SqlQuery<AgingSparePart>(query_string).FirstOrDefault();

            var data = new AgingSparePart();
            data.three_months = result.three_months;
            data.six_months = result.six_months;
            data.nine_months = result.nine_months;
            data.one_years = result.one_years;
            data.two_years = result.two_years;
            data.three_years = result.three_years;
            data.four_years = result.four_years;
            data.five_years = result.five_years;
            data.more_than_five_years = result.more_than_five_years;

            return data;

        }
    }
}