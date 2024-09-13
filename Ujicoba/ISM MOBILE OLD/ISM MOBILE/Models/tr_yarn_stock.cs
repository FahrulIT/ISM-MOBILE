using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ISM_MOBILE.Models
{
    public class tr_yarn_stock
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int YarnId { get; set; }
        public string item_no { get; set; }
        public decimal lbs { get; set; }
        public string type { get; set; }
        public DateTime proc_time { get; set; }

    }


}