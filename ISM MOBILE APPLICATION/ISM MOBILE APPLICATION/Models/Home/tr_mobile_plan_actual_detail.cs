using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ISM_MOBILE_APPLICATION.Models.Home
{
    public class tr_mobile_plan_actual_detail
    {
        [Key, Column(Order =0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id {get; set; }

        [Key, Column(Order = 1)]
        public int Bulan { get; set; }

        [Key, Column(Order = 2)]
        public int Tahun { get; set; }

        [Key, Column(Order = 3)]
        public string Dept { get; set; }

        [Key, Column(Order = 4)]
        public string Item { get; set; }

        [Key, Column(Order = 5)]
        public string Data_type { get; set; }
        public decimal Persentase { get; set; }
        public decimal Plan { get; set; }
        public decimal? Actual { get; set; }

        public decimal? PlanofMonth { get; set; }
        public decimal ?CurrentInv { get; set; }
    }
}