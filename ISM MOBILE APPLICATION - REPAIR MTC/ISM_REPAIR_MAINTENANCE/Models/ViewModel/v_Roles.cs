using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ISM_REPAIR_MAINTENANCE.Models
{
    [Table("v_Roles")]
    public class v_Roles
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int vRolesID { get; set; }
        public string UserId { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string RoleID { get; set; }

        public string RolesName { get; set; }
    }
}