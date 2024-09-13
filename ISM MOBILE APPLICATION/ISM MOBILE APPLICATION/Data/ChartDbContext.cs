using System.Data.Entity;
using ISM_MOBILE_APPLICATION.Models.IdentityReplace;
using ISM_MOBILE_APPLICATION.Models.Home;

namespace ISM_MOBILE_APPLICATION.Data
{
    public class ChartDbContext : DbContext
      
    {
        public ChartDbContext() : base("name = AppConnectionDB") { }
        public virtual DbSet<v_Roles> RolesData { get; set; }
        public virtual DbSet<tr_mobile_plan_actual> SummaryPlanActual { get; set; }
        public virtual DbSet<tr_mobile_plan_actual_detail> DetailPlanActual { get; set; }
    }
}