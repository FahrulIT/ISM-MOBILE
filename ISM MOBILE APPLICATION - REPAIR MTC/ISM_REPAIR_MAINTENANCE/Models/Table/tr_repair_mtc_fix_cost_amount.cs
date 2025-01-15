using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ISM_REPAIR_MAINTENANCE.Models.Table
{
    public class tr_repair_mtc_fix_cost_amount
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }

        public Int16 tahun { get; set; }
        public Int16 bulan { get; set; }
        public Int16 dept { get; set; }
        public string cost_type { get; set; }
        public string amount_type { get; set; }
        public decimal amount { get; set; }
    }
}