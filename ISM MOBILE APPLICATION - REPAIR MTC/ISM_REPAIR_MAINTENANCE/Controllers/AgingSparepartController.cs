using ISM_REPAIR_MAINTENANCE.Repository.Interface;
using ISM_REPAIR_MAINTENANCE.Repository.Service;
using System.Web.Mvc;

namespace ISM_REPAIR_MAINTENANCE.Controllers
{
    [Authorize]
    public class AgingSparepartController : Controller
    {
        private IAgingSparepart _as;

        public AgingSparepartController()
        {
            _as = new AgingSparepartService();
        }

        // GET: AgingSparepart
        public ActionResult Index()
        {
            ViewBag.UserName = User.Identity.Name.ToUpper();
            ViewBag.AgingSparepart = _as.GetViewBag_AgingSparepart();
            return View();
        }
    }
}