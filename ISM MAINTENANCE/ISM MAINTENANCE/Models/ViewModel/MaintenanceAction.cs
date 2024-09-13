using System.ComponentModel.DataAnnotations;

namespace ISM_MAINTENANCE.Models.ViewModel
{
    public class MaintenanceAction
    {
        [Required]
        [Key]
        public decimal dept_id { get; set; }

        [Required]
        [Key]
        public decimal mc_id { get; set; }

        [Required]
        [Key]
        public string mtc_id { get; set; }

        [Required]
        [Key]
        public string trouble_id { get; set; }

        [Required]
        [Key]
        [Display(Name ="Code")]
        [StringLength(6)]
        public string mtc_action_id { get; set; }

        [Display(Name = "Department")]
        public string dept_name { get; set; }

        [Display(Name = "Machine Type")]
        public string mc_name { get; set; }

        [Display(Name = "Maintenance Type")]
        public string mtc_name { get; set; }
        
        [Display(Name = "Trouble Name")]
        public string trouble_name { get; set; }

        [Display(Name = "Description")]
        [StringLength(50)]
        public string mtc_action_name { get; set; }
    }
}