using System.ComponentModel.DataAnnotations;


namespace ISM_MAINTENANCE.Models.ViewModel
{
    public class MaintenanceType
    {
        [Key]
        [Required]
        [Display(Name ="Code")]
        [StringLength(5)]
        public string mtc_id { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name ="Description")]
        public string mtc_name { get; set; }

    }
}