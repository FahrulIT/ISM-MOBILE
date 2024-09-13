using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ISM_MOBILE_APPLICATION.Models.Home
{
    public class tr_mobile_plan_actual
    {
        [Key, Column(Order = 0), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }

        [Key, Column(Order = 1)]
        public int Bulan { get; set; }

        [Key, Column(Order = 2)]
        public int Tahun { get; set; }

        [Key, Column(Order = 3)]
        public string Dept { get; set; }

        [Key, Column(Order = 4)]
        public string Data_type { get; set; }

        [Key, Column(Order = 5)]
        public string Trans_Type { get; set; }

        private decimal? _Qty;
        public decimal? Qty
        {
            get
            {
                return _Qty;
            }
            set
            {
                _Qty = value ?? 0;
            }
        }


    }
}