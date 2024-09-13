using ISM_MAINTENANCE.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Helper;

namespace ISM_MAINTENANCE.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private WvMaintenanceEntities db = new WvMaintenanceEntities();
    
        private string GetValueFromEntity(string obj)
        {
            string _value = "";


            return _value;
        }

        public ActionResult Index()
        {
            ViewBag.Name = db.sysuser_app.Where(x => x.ID == User.Identity.Name.ToString().Trim()).Select(x => x.Fullname).FirstOrDefault();
            ViewBag.Roles = db.v_user_ism_maintenance.Where(x => x.ID == User.Identity.Name.ToString().Trim()).FirstOrDefault().roles.ToString().Trim();
            ViewBag.Name = db.sysuser_app.Where(x => x.ID == User.Identity.Name.ToString().Trim()).Select(x => x.Fullname).FirstOrDefault();
            return View();
        }

        public ActionResult Welcome()
        {
            ViewBag.Name = db.sysuser_app.Where(x => x.ID == User.Identity.Name.ToString().Trim()).Select(x => x.Fullname).FirstOrDefault();

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
            return View();
        }

    }
}