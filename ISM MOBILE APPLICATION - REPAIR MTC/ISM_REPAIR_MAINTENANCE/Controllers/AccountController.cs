using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using ISM_REPAIR_MAINTENANCE.Models;


namespace ISM_REPAIR_MAINTENANCE.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager )
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set 
            { 
                _signInManager = value; 
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {           
            if (User.Identity.Name != "")
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
       
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            //var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            var result = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

        //
   

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                string[] order = new[] { "Spinning", "Weaving", "Dyeing", "Management" };
                ViewBag.RolesName = new SelectList(_context.Roles.Where(u => !u.Name.Contains("Admin"))
                                                 .OrderByDescending(i => i.Name == "Spinning")
                                                 .ThenByDescending(i => i.Name == "Weaving")
                                                 .ThenByDescending(i => i.Name == "Dyeing")
                                                 .ThenByDescending(i => i.Name == "Management").ToList(), "Name", "Name");
            }
        
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            //if (ModelState.IsValid)
            //{
            //    var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
            //    var result = await UserManager.CreateAsync(user, model.Password);
            //    if (result.Succeeded)
            //    {
            //        await SignInManager.SignInAsync(user, isPersistent:false, rememberBrowser:false);

            //        // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
            //        // Send an email with this link
            //        // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
            //        // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
            //        // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

            //        return RedirectToAction("Index", "Home");
            //    }
            //    AddErrors(result);
            //}

            //// If we got this far, something failed, redisplay form
            //return View(model);
            if (ModelState.IsValid)
            {
                //var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var user = new ApplicationUser { UserName = model.UserName, Email = model.Email };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await this.UserManager.AddToRoleAsync(user.Id, model.UserRoles);
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");
                    string _username = model.UserName;
                    using (ApplicationDbContext _context = new ApplicationDbContext())
                    {
                        var userRoles = (from DataUSer in _context.RolesData
                                         where DataUSer.UserName == _username
                                         select DataUSer.RolesName).ToArray();

                        //ViewBag.UserRoles = string.Join(",", userRoles);
                        Session["S_RolesName"] = string.Join(",", userRoles);
                    }
                    return RedirectToAction("Index", "Home");
                }

                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                string[] order = new[] { "Spinning", "Weaving", "Dyeing", "Management" };
                ViewBag.RolesName = new SelectList(_context.Roles.Where(u => !u.Name.Contains("Admin"))
                                                 .OrderByDescending(i => i.Name == "Spinning")
                                                 .ThenByDescending(i => i.Name == "Weaving")
                                                 .ThenByDescending(i => i.Name == "Dyeing")
                                                 .ThenByDescending(i => i.Name == "Management").ToList(), "Name", "Name");
            }
            return View(model);
        }
      
        
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

  

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

      
    }
}