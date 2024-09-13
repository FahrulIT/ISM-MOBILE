using ISM_MOBILE_APPLICATION.Data;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Web.Mvc;

namespace ISM_MOBILE_APPLICATION.Controllers
{

    [Authorize(Roles = "Dyeing, Admin, Management")]
    public class DyeingController : Controller
    {     
        // GET: Dyeing
        public ActionResult Index()
        {
            string _username = User.Identity.GetUserName();

            if (Session["S_RolesName"] is string)
            {
                ViewBag.UserRoles = Session["S_RolesName"];
            }
            else
            {
                using (ChartDbContext _context = new ChartDbContext())
                {
                    var userRoles = (from DataUSer in _context.RolesData
                                     where DataUSer.UserName == _username
                                     select DataUSer.RolesName).ToArray();

                    Session["S_RolesName"] = string.Join(",", userRoles);
                    ViewBag.UserRoles = Session["S_RolesName"];
                }

            }
            return View();
        }
    }
}