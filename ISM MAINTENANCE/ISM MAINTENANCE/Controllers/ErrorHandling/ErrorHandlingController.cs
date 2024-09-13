using System.Web.Mvc;

namespace ISM_MAINTENANCE.Controllers.ErrorHandling
{
    public class ErrorHandlingController : Controller
    {
        // GET: ErrorHandling
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult NotFound()
        {
            return View();

        }
    }
}