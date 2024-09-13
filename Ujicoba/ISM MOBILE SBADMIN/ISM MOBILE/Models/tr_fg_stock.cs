using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ISM_MOBILE.Models
{
    public class tr_fg_stock
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int FgID { get; set; }
        public string de { get; set; }
        public string item_category { get; set; }

        public string category { get; set; }
        public string item_no { get; set; }

        public decimal meter { get; set; }
        public DateTime proc_time { get; set; }

    }
}