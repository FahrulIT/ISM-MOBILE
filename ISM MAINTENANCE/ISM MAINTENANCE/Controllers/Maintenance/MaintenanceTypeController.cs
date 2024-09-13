using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ISM_MAINTENANCE.Models.DB;
using ISM_MAINTENANCE.Models.ViewModel;


namespace ISM_MAINTENANCE.Controllers
{
    public class MaintenanceTypeController : Controller
    {
        private WvMaintenanceEntities db = new WvMaintenanceEntities();
        private void InitializeUserAkses()
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
        public string GetIPAddress()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] addresses = ipAddress.Split(',');
                if (addresses.Length != 0)
                {
                    return addresses[0];
                }
            }

            return context.Request.ServerVariables["REMOTE_ADDR"];
        }

 
        // GET: MaintenanceType
        [Authorize]
        public ActionResult Index()
        {

            IQueryable<MaintenanceType> obj = from data in db.ms_maintenance
                                              select new MaintenanceType()
                                              {
                                                  mtc_id = data.mtc_id,
                                                  mtc_name = data.mtc_name
                                          };

            return View(obj);
        }

        // GET: MaintenanceType/Create
        [Authorize]
        public ActionResult Create()
        {
            MaintenanceType obj = new MaintenanceType();
            return View(obj);
        }

        // POST: MaintenanceType/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create([Bind(Include = "mtc_id, mtc_name")] MaintenanceType DataForm)
        {
            
            ms_maintenance Duplicate = db.ms_maintenance.Find(DataForm.mtc_id);
            if (Duplicate != null)
            {
                ModelState.AddModelError("", "Duplicate Data");             
                return View("");
            }

            ms_maintenance obj = new ms_maintenance();
            if (ModelState.IsValid)
            {
                obj.mtc_id = DataForm.mtc_id;
                obj.mtc_name = DataForm.mtc_name;
                obj.client_ip = GetIPAddress();
                obj.proc_time = DateTime.Now;
                obj.act_rec = "A";
                obj.user_id = User.Identity.Name;

                db.ms_maintenance.Add(obj);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View("");
        }

        // GET: MaintenanceType/Edit/5
        [Authorize]
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ms_maintenance ms_maintenance = db.ms_maintenance.Find(id);
            MaintenanceType obj = new MaintenanceType();

            obj.mtc_id = ms_maintenance.mtc_id;
            obj.mtc_name = ms_maintenance.mtc_name;


            if (obj == null)
            {
                return HttpNotFound();
            }
            return View(obj);
        }

        // POST: MaintenanceType/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit([Bind(Include = "mtc_id,mtc_name,act_rec, user_id,client_ip,proc_time")] ms_maintenance DataForm)
        {
            ms_maintenance obj = db.ms_maintenance.Single(x => x.mtc_id == DataForm.mtc_id);
            obj.mtc_name = DataForm.mtc_name;
            obj.user_id = User.Identity.Name;
            obj.client_ip = GetIPAddress();
            obj.proc_time = DateTime.Now;
            obj.act_rec = "A";

            if (ModelState.IsValid)
            {
                db.Entry(obj).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(DataForm);
        }

        // GET: MaintenanceType/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ms_maintenance ms_maintenance = db.ms_maintenance.Find(id);
            MaintenanceType obj = new MaintenanceType();

            obj.mtc_id = ms_maintenance.mtc_id;
            obj.mtc_name = ms_maintenance.mtc_name;
            if (obj == null)
            {
                return HttpNotFound();
            }
            return View(obj);
        }

        // POST: MaintenanceType/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            ms_maintenance ms_maintenance = db.ms_maintenance.Find(id);
            db.ms_maintenance.Remove(ms_maintenance);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
