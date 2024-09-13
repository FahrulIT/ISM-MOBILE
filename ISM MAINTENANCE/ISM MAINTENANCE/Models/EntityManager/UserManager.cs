using System.Linq;
using ISM_MAINTENANCE.Models.DB;

namespace ISM_MAINTENANCE.Models.EntityManager
{
    public class UserManager
    {
        public string GetUserPassword(string UserID)
        {
            using (WvMaintenanceEntities db = new WvMaintenanceEntities())
            {
                var user = db.sysuser_app.Where(o => o.ID.ToString().Equals(UserID));
                if (user.Any())
                    return user.FirstOrDefault().Password;
                else
                    return string.Empty;
            }
        }

        public string GetUserFullname(string UserID)
        {
            using (WvMaintenanceEntities db = new WvMaintenanceEntities())
            {
                var user = db.sysuser_app.Where(o => o.ID.ToString().Equals(UserID));
                if (user.Any())
                    return user.FirstOrDefault().Fullname;
                else
                    return string.Empty;
            }
        }
    }
}