using ISM_MAINTENANCE.Models.DB;
using System.Linq;
using System.Web.Mvc;

namespace ISM_MAINTENANCE.Controllers
{
    [Authorize]
    public class OtorisasiController : Controller
    {
        // GET: Otorisasi
        private void InitializeUserAkses()
        {
            using (Models.DB.WvMaintenanceEntities db = new WvMaintenanceEntities())
            {
                if (db.v_user_ism_maintenance.Where(x => x.ID == User.Identity.Name.ToString().Trim()).FirstOrDefault() == null)
                {
                    ViewBag.Section = ""; ViewBag.Roles = ""; ViewBag.UserAksesMenu = "";
                }
                else
                {
                    ViewBag.UserAksesMenu = db.ms_user_access_menu.Where(x => x.userid == User.Identity.Name.ToString().Trim()).ToList();
                    ViewBag.Section = db.v_user_ism_maintenance.Where(x => x.ID == User.Identity.Name.ToString().Trim()).FirstOrDefault().section.ToString().Trim();
                    ViewBag.Roles = db.v_user_ism_maintenance.Where(x => x.ID == User.Identity.Name.ToString().Trim()).FirstOrDefault().roles.ToString().Trim();
                }
            }            
        }

        public ActionResult AccessDenied()
        {
            InitializeUserAkses();

            using (Models.DB.WvMaintenanceEntities db = new WvMaintenanceEntities())
            {
                ViewBag.Name = db.sysuser_app.Where(x => x.ID == User.Identity.Name).Select(x => x.Fullname).FirstOrDefault();
                
            }
            return View();

        }
    }
}