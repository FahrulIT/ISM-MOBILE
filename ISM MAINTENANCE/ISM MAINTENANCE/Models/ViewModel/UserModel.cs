using System.ComponentModel.DataAnnotations;
//This class for binding form login

namespace ISM_MAINTENANCE.Models.ViewModel
{
    public class UserLoginView
    {
        [Key]
        [Required(ErrorMessage = "*")]
        [Display(Name = "User ID")]
        public string UserID { get; set; }


        [Display(Name = "Nama User")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "*")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }

  public class UserAkses
    {
        public string userid { get; set; }
        public string menu { get; set; }
        public string submenu { get; set; }
        public string section { get; set; }


    }

}