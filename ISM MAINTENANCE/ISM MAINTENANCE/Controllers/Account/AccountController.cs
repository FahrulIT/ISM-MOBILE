using System.Web.Mvc;
using System.Web.Security;
using ISM_MAINTENANCE.Models.EntityManager;
using ISM_MAINTENANCE.Models.ViewModel;
namespace ISM_MAINTENANCE.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult LogIn()
        {
            return View();
        }

        [Authorize]
        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult LogIn(UserLoginView ULV, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                UserManager UM = new UserManager();
                string password = UM.GetUserPassword(ULV.UserID).ToUpper();
                string fullname = UM.GetUserFullname(ULV.UserID).ToUpper();
                //ViewBag.Fullname = fullname;
                if (string.IsNullOrEmpty(password))
                    ModelState.AddModelError("", "The user login or password provided is incorrect.");
                else
                {
                    if (ULV.Password.ToUpper().Equals(password))
                    {
                        FormsAuthentication.SetAuthCookie(ULV.UserID, false);
                        //Session["UserID"] = ULV.UserID;
                        return RedirectToAction("Welcome", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "The password provided is incorrect.");
                    }
                }
            }

            // If we got this far, something failed, redisplay form  
            return View(ULV);
        }

   
    }
}