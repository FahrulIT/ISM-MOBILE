using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ISM_MAINTENANCE.Models.DB;
using ISM_MAINTENANCE.Models.ViewModel;

namespace ISM_MAINTENANCE.Controllers
{
    public class MachineTypeController : Controller
    {
        private WvMaintenanceEntities db = new WvMaintenanceEntities();

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

        // GET: MachineType 
        [Authorize]
        public ActionResult Index()
        {
            IQueryable<MachineType> obj = from data in db.v_Machine_Type
                                          select new MachineType()
                                          {
                                              dept_id = data.dept_id,
                                              DeptName = data.dept_name,
                                              mc_id = (int)data.mc_id,
                                              mc_name = data.mc_name,
                                              user_id = data.user_id,
                                              act_rec = data.act_rec,
                                              client_ip = data.client_ip,
                                              proc_time = data.proc_time
                                          };

            return View(obj);
        }

        // GET: MachineType/Create
        public ActionResult Create()
        {
            ViewBag.dept_id = new SelectList(db.ms_dept, "dept_id", "dept_name");
            IQueryable<MachineTypeMCID> MaxObj = from DATA in db.ms_machine_type group DATA by DATA.dept_id into DATA2 select new MachineTypeMCID() {
                Dept_id = (int)DATA2.FirstOrDefault().dept_id,
                MaxMCID = (int)DATA2.Max(c => c.mc_id) + 1
            };

            //if (MaxObj.Count() == 0) {

            //}

            ViewBag.MaxDeptId = MaxObj;
            MachineType obj = new MachineType();
            return View(obj);
        }

        // POST: MachineType/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create([Bind(Include = "dept_id,mc_id,mc_name,user_id,act_rec,client_ip,proc_time")] ms_machine_type DataForm)
        {

            if (!ModelState.IsValid)
            {
                ViewBag.dept_id = new SelectList(db.ms_dept, "dept_id", "dept_name");
                MachineType obj = new MachineType();
                return View(obj);
            }

            ms_machine_type Duplicate = db.ms_machine_type.Find(DataForm.dept_id, DataForm.mc_id);
            if (Duplicate != null)
            {
                ModelState.AddModelError("", "Duplicate Data");
                ViewBag.dept_id = new SelectList(db.ms_dept, "dept_id", "dept_name");
                return View("");
            }

            if ((int)DataForm.mc_id >= 100) {
                ModelState.AddModelError("", "Machine ID/Code Melebihi Batas 100");
                ViewBag.dept_id = new SelectList(db.ms_dept, "dept_id", "dept_name");
                return View("");
            }

            DataForm.user_id = User.Identity.Name;
            DataForm.act_rec = "A";
            DataForm.client_ip = GetIPAddress();
            DataForm.proc_time = DateTime.Now;
            
            if (ModelState.IsValid)
            {
                db.ms_machine_type.Add(DataForm);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.dept_id = new SelectList(db.ms_dept, "dept_id", "dept_name", DataForm.dept_id);
            return View(DataForm);
        }

        [AllowAnonymous]
        [Authorize]
        // GET: MachineType/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            int deptid, mcid;

            string[] par = id.Split('-');
            if (par.Length != 1)
            {
                deptid = Convert.ToInt32(par[0]);
                mcid = Convert.ToInt32(par[1]);          
            }
            else
            {
                deptid = 0; mcid = 0;
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //ms_machine_type ms_machine_type = db.ms_machine_type.Find(deptid, mcid);
            IQueryable<MachineType> MachineType = from data in db.v_Machine_Type
                                                  where data.dept_id == deptid && data.mc_id == mcid
                                                  select new MachineType()
                                                  {
                                                      dept_id = data.dept_id,
                                                      DeptName = data.dept_name,
                                                      mc_id = (int)data.mc_id,
                                                      mc_name = data.mc_name,
                                                      user_id = data.user_id,
                                                      act_rec = data.act_rec,
                                                      client_ip = data.client_ip,
                                                      proc_time = data.proc_time
                                                  };

            if (MachineType == null)
            {
                return HttpNotFound();
            }
            return View(MachineType.FirstOrDefault());
        }

        // POST: MachineType/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Edit([Bind(Include = "dept_id,mc_id,mc_name,user_id,client_ip,proc_time")] ms_machine_type DataForm)
        {
            ms_machine_type obj = db.ms_machine_type.Single(x => x.dept_id == DataForm.dept_id && x.mc_id == DataForm.mc_id);
            obj.mc_name = DataForm.mc_name;
            obj.user_id = User.Identity.Name;
            obj.client_ip = Request.UserHostAddress;
            obj.proc_time = DateTime.Now;

            if (ModelState.IsValid)
            {
                db.Entry(obj).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(DataForm);
        }

        // GET: MachineType/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            int deptid, mcid;

            string[] par = id.Split('-');
            if (par.Length != 1)
            {
                deptid = Convert.ToInt32(par[0]);
                mcid = Convert.ToInt32(par[1]);
            }
            else
            {
                deptid = 0; mcid = 0;
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            IQueryable<MachineType> ms_machine_type = from data in db.v_Machine_Type
                                                      where data.dept_id == deptid && data.mc_id == mcid
                                                      select new MachineType()
                                                      {
                                                          dept_id = data.dept_id,
                                                          DeptName = data.dept_name,
                                                          mc_id = (int)data.mc_id,
                                                          mc_name = data.mc_name,
                                                          user_id = data.user_id,
                                                          act_rec = data.act_rec,
                                                          client_ip = data.client_ip,
                                                          proc_time = data.proc_time
                                                      };


            if (ms_machine_type == null)
            {
                return HttpNotFound();
            }
            return View(ms_machine_type.FirstOrDefault());
        }

        // POST: MachineType/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
        {
            int deptid, mcid;
            string[] par = id.Split('-');
            if (par.Length != 1)
            {
                deptid = Convert.ToInt32(par[0]);
                mcid = Convert.ToInt32(par[1]);
            }
            else
            {
                deptid = 0; mcid = 0;
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ms_machine_type ms_machine_type = db.ms_machine_type.Find(deptid, mcid);
            db.ms_machine_type.Remove(ms_machine_type);
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

