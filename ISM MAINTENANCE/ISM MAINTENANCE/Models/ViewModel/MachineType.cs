using System;
using System.ComponentModel.DataAnnotations;

namespace ISM_MAINTENANCE.Models.ViewModel
{
    public class MachineType
    {
        [Key]
        [Required]
        public decimal dept_id { get; set; }

        [Display(Name = "Department")]
        [StringLength(20)]
        [Required]
        public string DeptName { get; set; }

        [Key]
        [Required]
        [Display(Name = "Code")]
        public int mc_id { get; set; }

        [Display(Name = "Description")]
        public string mc_name { get; set; }
        public string user_id { get; set; }
        public string act_rec { get; set; }
        public string client_ip { get; set; }
        public Nullable<System.DateTime> proc_time { get; set; }
    }
}

public class MachineTypeMCID
{
    public int Dept_id { get; set; }
    public int MaxMCID { get; set; }
}