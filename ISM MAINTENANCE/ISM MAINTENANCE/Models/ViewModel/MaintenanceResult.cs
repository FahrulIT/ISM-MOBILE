using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ISM_MAINTENANCE.Models.ViewModel
{
    public class MaintenanceResultView
    {

        public decimal dept_id { get; set; }
        public decimal mc_id { get; set; }
        public string mtc_slip_no1 { get; set; }
        public string mtc_slip_no2 { get; set; }

        public string mtc_id { get; set; }

        [Display(Name = "Maintenance Type")]
        public string mtc_name { get; set; }

        [Display(Name = "Machine No")]
        public string machine_no { get; set; }

        public string loom_no { get; set; }
        public string block { get; set; }

        [Display(Name = "Maintenance Slip")]
        public string MaintenanceSlip { get; set; }

        [Display(Name = "MTC Ke")]
        public int mtc_ser_no { get; set; }

        public string mtc_status { get; set; }

        [Display(Name = "Status")]
        public string status { get; set; }

        public string mtc_pic { get; set; }

        [Display(Name = "PIC")]
        public string mtc_pic_name { get; set; }

      
    }

    public class VoucherSlip {
        public string mtc_slip_no1 { get; set; }
        public string mtc_slip_no2 { get; set; }

        [Display(Name = "Maintenance Slip")]
        public string MaintenanceSlip { get; set; }
        
        [Display(Name = "Maintenance Type")]
        public string mtc_name { get; set; }

        public string loom_no { get; set; }
        public string block { get; set; }

        [Display(Name ="Machine No")]
        public string machine_no { get; set; }


    }

    //public class TRM_Header
    //{
    //    public decimal dept_id { get; set; }

    //    public decimal mc_id { get; set; }

    //    public string mtc_slip_no1 { get; set; }
    //    public string mtc_slip_no2 { get; set; }
    //    public System.DateTime issue_date { get; set; }
    //    public string block { get; set; }
    //    public string loom_no { get; set; }
    //    public string mtc_id { get; set; }

    //    public List<TRM_detail_1> Detail1 { get; set; }
    //}

    public class TRM_detail_1
    {
        [Key]
        public decimal dept_id { get; set; }

        [Key]
        public decimal mc_id { get; set; }

        [Key]
        public string mtc_slip_no1 { get; set; }

        [Key]
        public string mtc_slip_no2 { get; set; }

        [Key]
        [Display(Name ="Mtc Ke: ")]
        public int mtc_ser_no { get; set; }

        [Display(Name = "Start Maintenance")]
    
        public System.DateTime mtc_start { get; set; }

        [Display(Name = "Finish Maintenance")]
       
        public System.DateTime mtc_end { get; set; }
        
        public string mtc_pic { get; set; }

        [Display(Name = "PIC Maintenance")]
        public string mtc_pic_name { get; set; }

        public string mtc_status { get; set; }


        [Display(Name = "Status Maintenance")]
        public string mtc_status_name { get; set; }

        public List<TRM_detail_2> Detail2 { get; set; }
    }

    public class TRM_detail_2
    {
        [Key]
        public decimal dept_id { get; set; }

        [Key]
        public decimal mc_id { get; set; }

        [Key]
        public string mtc_slip_no1 { get; set; }

        [Key]
        public string mtc_slip_no2 { get; set; }

        [Key]
        public int mtc_ser_no { get; set; }

        [Key]
        public string trouble_id { get; set; }

        public string trouble_name { get; set; }

        public List<TRM_detail_3> Detail3 { get; set; }
    }

    public class TRM_detail_3
    {
        [Key]
        public decimal dept_id { get; set; }

        [Key]
        public decimal mc_id { get; set; }

        [Key]
        public string mtc_slip_no1 { get; set; }

        [Key]
        public string mtc_slip_no2 { get; set; }

        [Key]
        public int mtc_ser_no { get; set; }

        [Key]
        public string trouble_id { get; set; }

        [Key]
        public string mtc_action_id { get; set; }

        public string mtc_action_name { get; set; }

        public List<TRM_detail_4> Detail4 { get; set; }
    }

    public class TRM_detail_4
    {
        [Key]
        public decimal dept_id { get; set; }

        [Key]
        public decimal mc_id { get; set; }

        [Key]
        public string mtc_slip_no1 { get; set; }

        [Key]
        public string mtc_slip_no2 { get; set; }

        [Key]
        public int mtc_ser_no { get; set; }

        [Key]
        public string trouble_id { get; set; }

        [Key]
        public string mtc_action_id { get; set; }

        [Key]
        public string item_code { get; set; }

        public string item_name { get; set; }

        public string mtc_action_name { get; set; }

        public decimal price { get; set; }
        public decimal quantity { get; set; }
    }

  

}