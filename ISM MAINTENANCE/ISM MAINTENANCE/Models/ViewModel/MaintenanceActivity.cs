using System.ComponentModel.DataAnnotations;

namespace ISM_MAINTENANCE.Models.ViewModel
{
    public class MaintenanceActivity
    {
        [Key]
        [Required]
        public decimal dept_id { get; set; }

        [Key]
        [Required]
        public int mc_id { get; set; }

        [Key]
        [Required]
        [MaxLength(6)]
        public string mtc_slip_no1 { get; set; }

        [Key]
        [Required]
        [MaxLength(2)]
        public string mtc_slip_no2 { get; set; }

        [Display(Name = "Date")]
       [DisplayFormat(DataFormatString ="{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public System.DateTime issue_date { get; set; }

        [MaxLength(1)]
        public string block { get; set; }

        [MaxLength(3)]
        public string loom_no { get; set; }

        [MaxLength(5)]
        public string mtc_id { get; set; }

        [Display(Name = "Department")]
        public string dept_name { get; set; }

        [Display(Name = "Machine")]
        public string mc_name { get; set; }

        [Display(Name = "Maintenance Type")]
        public string mtc_name { get; set; }

        [Display(Name = "Machine No")]
        public string machine_no
        {
            get; set;

        }


        [Display(Name = "Maintenance Slip No")]
        public string maintenance_slip
        {
            get; set;
        }


        [Display(Name = "Status")]
        public string mtc_status
        {
            get; set;

        }

    }
}