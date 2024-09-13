using System.ComponentModel.DataAnnotations;

namespace ISM_MAINTENANCE.Models.ViewModel
{

    public class MachineTrouble
       {
        [Key]
        [Required(ErrorMessage = "*")]
        public decimal dept_id { get; set; }

        [Key]
        [Required(ErrorMessage = "*")]
        public decimal mc_id { get; set; }

        [Key]
        [Required(ErrorMessage = "*")]
        [Display(Name = "Code")]
        [StringLength(4)]
        public string trouble_id { get; set; }

        [Display(Name = "Trouble Name")]
        [StringLength(30)]
        public string trouble_name { get; set; }

        [Display(Name = "Department")]
        public string dept_name { get; set; }

        [Display(Name = "Machine")]
        public string mc_name { get; set; }
           



    }


}