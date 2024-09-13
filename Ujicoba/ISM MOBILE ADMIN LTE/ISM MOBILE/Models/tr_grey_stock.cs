using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ISM_MOBILE.Models
{
    public class tr_grey_stock
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int GreyId { get; set; }

        public string grey_no { get; set;  }
   
        public string grade { get; set; }
        //{
        //    get
        //    {
        //        return _grade;
        //    }

        //    set
        //    {
        //        if (value == "AS")
        //        {
        //            _grade = "A";
        //        }
        //        else if (value == "BS")
        //        {
        //            _grade = "B";
        //        }

        //        else if (value == "CS")
        //        {
        //            _grade = "C";
        //        }

        //        else if (value == "A3S")
        //        {
        //            _grade = "A3";
        //        }

        //        else if (value == "A2S")
        //        {
        //            _grade = "A2";
        //        }
        //        else { _grade = value; }

        //    }
        //}
        public string status_pcs { get; set; }
        public decimal meter { get; set; }
        public DateTime proc_time { get; set; }
    }
}