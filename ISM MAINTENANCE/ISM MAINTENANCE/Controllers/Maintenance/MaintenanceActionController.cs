using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ISM_MAINTENANCE.Models.DB;
using ISM_MAINTENANCE.Models.ViewModel;

namespace ISM_MAINTENANCE.Controllers.Maintenance
{
    public class MaintenanceActionController : Controller
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

        // GET: MaintenanceAction
        [Authorize]
        public ActionResult Index()
        {
            IQueryable<MaintenanceAction> obj = from data in db.v_Maintenance_Action
                                                select new MaintenanceAction()
                                                {
                                                    dept_id = data.dept_id,
                                                    mc_id = data.mc_id,
                                                    mtc_id = data.mtc_id,
                                                    trouble_id = data.trouble_id,
                                                    trouble_name = data.trouble_name,
                                                    mtc_action_id = data.mtc_action_id,
                                                    mtc_action_name = data.mtc_action_name,
                                                    dept_name = data.dept_name,
                                                    mc_name = data.mc_name,
                                                    mtc_name = data.mtc_name
                                                };
            return View(obj);

        }


        // GET: MaintenanceAction/Create
        [Authorize]
        public ActionResult Create()
        {
            string dept_code_cim = db.sysuser_app.Where(x => x.ID == User.Identity.Name).Select(x => x.departement).FirstOrDefault();
            string dept_name_cim = db.departement_master.Where(x => x.Dept_ID == dept_code_cim).Select(x => x.Dept_Desc).FirstOrDefault().ToUpper();
            decimal dept_id_system = db.ms_dept.Where(x => x.dept_name == dept_name_cim).Select(x => x.dept_id).FirstOrDefault();

            ViewBag.dept_id = new SelectList(db.ms_dept.Where(x => x.dept_id == dept_id_system), "dept_id", "dept_name", 800);
            ViewBag.mc_id = new SelectList(db.ms_machine_type.Where(x => x.dept_id == dept_id_system && x.mc_id == 70), "mc_id", "mc_name", 70);
            ViewBag.mtc_id = new SelectList(db.ms_maintenance, "mtc_id", "mtc_name");
            ViewBag.trouble_id = new SelectList(db.ms_machine_trouble.Where(x => x.dept_id == 0), "trouble_id", "trouble_name");
            ViewBag.Test = db.v_Maintenance_Type_for_Maintenance_Action.OrderBy(x => x.trouble_name);
            ViewBag.MtcAction = db.ms_maintenance_action.Select(x => new { x.dept_id, x.mc_id, x.mtc_id, x.trouble_id, x.mtc_action_id});

            //ViewBag.trouble_id_Quality = "";
            //ViewBag.trouble_id_Preventive = "";
            MaintenanceAction obj = new MaintenanceAction();
            return View(obj);
        }

        // POST: MaintenanceAction/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create([Bind(Include = "dept_id,mc_id,mtc_id,trouble_id,mtc_action_id,mtc_action_name")] MaintenanceAction FrmData)
        {
            string dept_code_cim, dept_name_cim;
            decimal dept_id_system;

            ms_maintenance_action Duplicate = db.ms_maintenance_action.Find(FrmData.dept_id, FrmData.mc_id, FrmData.mtc_id, FrmData.trouble_id, FrmData.mtc_action_id);
            if (Duplicate != null)
            {
                ModelState.AddModelError("", "Duplicate Data");

                dept_code_cim = db.sysuser_app.Where(x => x.ID == User.Identity.Name).Select(x => x.departement).FirstOrDefault();
                dept_name_cim = db.departement_master.Where(x => x.Dept_ID == dept_code_cim).Select(x => x.Dept_Desc).FirstOrDefault().ToUpper();
                dept_id_system = db.ms_dept.Where(x => x.dept_name == dept_name_cim).Select(x => x.dept_id).FirstOrDefault();

                ViewBag.dept_id = new SelectList(db.ms_dept.Where(x => x.dept_id == dept_id_system), "dept_id", "dept_name");
                ViewBag.mc_id = new SelectList(db.ms_machine_type.Where(x => x.dept_id == dept_id_system), "mc_id", "mc_name");
                ViewBag.mtc_id = new SelectList(db.ms_maintenance, "mtc_id", "mtc_name");
                ViewBag.trouble_id = new SelectList(db.ms_machine_trouble.Where(x => x.dept_id == 0), "trouble_id", "trouble_name");
                ViewBag.Test = db.v_Maintenance_Type_for_Maintenance_Action.OrderBy(x => x.trouble_name);
                return View("");
            }

            if (ModelState.IsValid)
            {
                ms_maintenance_action obj = new ms_maintenance_action();
                obj.dept_id = FrmData.dept_id;
                obj.mc_id = FrmData.mc_id;
                obj.mtc_id = FrmData.mtc_id;
                obj.trouble_id = FrmData.trouble_id;
                obj.mtc_action_id = FrmData.mtc_action_id;

                obj.mtc_action_name = FrmData.mtc_action_name.ToUpper();
                obj.user_id = User.Identity.Name.ToUpper();
                obj.act_rec = "A";
                obj.proc_time = DateTime.Now;
                obj.client_ip = GetIPAddress();

                db.ms_maintenance_action.Add(obj);
                db.SaveChanges();
                return RedirectToAction("Index");
            }


            dept_code_cim = db.sysuser_app.Where(x => x.ID == User.Identity.Name).Select(x => x.departement).FirstOrDefault();
            dept_name_cim = db.departement_master.Where(x => x.Dept_ID == dept_code_cim).Select(x => x.Dept_Desc).FirstOrDefault().ToUpper();
            dept_id_system = db.ms_dept.Where(x => x.dept_name == dept_name_cim).Select(x => x.dept_id).FirstOrDefault();

            ViewBag.dept_id = new SelectList(db.ms_dept.Where(x => x.dept_id == dept_id_system), "dept_id", "dept_name");
            ViewBag.mc_id = new SelectList(db.ms_machine_type.Where(x => x.dept_id == dept_id_system), "mc_id", "mc_name");
            ViewBag.mtc_id = new SelectList(db.ms_maintenance, "mtc_id", "mtc_name");

            if (FrmData.trouble_id == "-")
            {
                ViewBag.trouble_id = new SelectList(db.v_Maintenance_Type_for_Maintenance_Action.Where(x => x.trouble_id == "-"), "trouble_id", "trouble_name");
            }
            else if (FrmData.mtc_id == "MTC20")
            {
                ViewBag.trouble_id = new SelectList(db.v_Maintenance_Type_for_Maintenance_Action.Where(x => x.Trouble_Type == "QUALITY TROUBLE"), "trouble_id", "trouble_name");
            }
            else
            {
                ViewBag.trouble_id = new SelectList(db.ms_machine_trouble.Where(x => x.dept_id == dept_id_system), "trouble_id", "trouble_name");
            }

            ViewBag.Test = db.v_Maintenance_Type_for_Maintenance_Action.OrderBy(x => x.trouble_name);

            return View("");
        }

        // GET: MaintenanceAction/Edit/5
        [Authorize]
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            decimal deptid, mcid;
            string troubleid, mtcid, mtc_actionid;

            string[] par = id.Split(';');
            if (par.Length != 1)
            {
                deptid = Convert.ToDecimal(par[0]);
                mcid = Convert.ToDecimal(par[1]);
                mtcid = par[2].ToString();
                troubleid = par[3].ToString();
                mtc_actionid = par[4].ToString();
            }
            else
            {
                deptid = 0; mcid = 0; troubleid = "";
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            IQueryable<MaintenanceAction> obj = from data in db.v_Maintenance_Action.Where(x => x.dept_id == deptid && x.mc_id == mcid && x.mtc_id == mtcid &&
                                                x.trouble_id == troubleid && x.mtc_id == mtcid && x.mtc_action_id == mtc_actionid)
                                                select new MaintenanceAction()
                                                {
                                                    dept_id = data.dept_id,
                                                    mc_id = data.mc_id,
                                                    mtc_id = data.mtc_id,
                                                    trouble_id = data.trouble_id,
                                                    trouble_name = data.trouble_name,
                                                    mtc_action_id = data.mtc_action_id,
                                                    mtc_action_name = data.mtc_action_name,
                                                    dept_name = data.dept_name,
                                                    mc_name = data.mc_name,
                                                    mtc_name = data.mtc_name
                                                };


            if (obj == null)
            {
                return HttpNotFound();
            }
            return View(obj.FirstOrDefault());
        }

        // POST: MaintenanceAction/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        public ActionResult Edit([Bind(Include = "dept_id,mc_id,mtc_id,trouble_id,mtc_action_id,mtc_action_name,user_id,act_rec,client_ip,proc_time")] ms_maintenance_action ms_maintenance_action)
        {
            if (ModelState.IsValid)
            {
                ms_maintenance_action.proc_time = DateTime.Now;
                ms_maintenance_action.mtc_action_name = ms_maintenance_action.mtc_action_name.ToUpper();
                ms_maintenance_action.act_rec = "T";
                ms_maintenance_action.client_ip = GetIPAddress();
                ms_maintenance_action.user_id = User.Identity.Name.ToUpper();
                db.Entry(ms_maintenance_action).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ms_maintenance_action);
        }

        // GET: MaintenanceAction/Delete/5
        [Authorize]
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            decimal deptid, mcid;
            string troubleid, mtcid, mtc_actionid;

            string[] par = id.Split(';');
            if (par.Length != 1)
            {
                deptid = Convert.ToDecimal(par[0]);
                mcid = Convert.ToDecimal(par[1]);
                mtcid = par[2].ToString();
                troubleid = par[3].ToString();
                mtc_actionid = par[4].ToString();
            }
            else
            {
                deptid = 0; mcid = 0; troubleid = "";
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            IQueryable<MaintenanceAction> obj = from data in db.v_Maintenance_Action.Where(x => x.dept_id == deptid && x.mc_id == mcid && x.mtc_id == mtcid &&
                                                x.trouble_id == troubleid && x.mtc_id == mtcid && x.mtc_action_id == mtc_actionid)
                                                select new MaintenanceAction()
                                                {
                                                    dept_id = data.dept_id,
                                                    mc_id = data.mc_id,
                                                    mtc_id = data.mtc_id,
                                                    trouble_id = data.trouble_id,
                                                    trouble_name = data.trouble_name,
                                                    mtc_action_id = data.mtc_action_id,
                                                    mtc_action_name = data.mtc_action_name,
                                                    dept_name = data.dept_name,
                                                    mc_name = data.mc_name,
                                                    mtc_name = data.mtc_name
                                                };

            if (obj == null)
            {
                return HttpNotFound();
            }
            return View(obj.FirstOrDefault());
        }

        // POST: MaintenanceAction/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            decimal deptid, mcid;
            string troubleid, mtcid, mtc_actionid;

            string[] par = id.Split(';');
            if (par.Length != 1)
            {
                deptid = Convert.ToDecimal(par[0]);
                mcid = Convert.ToDecimal(par[1]);
                mtcid = par[2].ToString();
                troubleid = par[3].ToString();
                mtc_actionid = par[4].ToString();
            }
            else
            {
                deptid = 0; mcid = 0; troubleid = "";
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            var Find_tr_mtc = db.tr_maintenance_action.Where(x => x.dept_id == deptid && x.mc_id == mcid && x.trouble_id == troubleid && x.mtc_action_id == mtc_actionid).ToList();
            var Find_tr_prev_mtc = db.tr_prev_maintenance.Where(x => x.dept_id == deptid && x.mc_id == mcid && x.mtc_action_id == mtc_actionid).ToList();

            if (Find_tr_mtc.Count > 0 || Find_tr_prev_mtc.Count > 0)
            {
                ModelState.AddModelError("", "!!Remove Data Failed, Data already used in Machine Quality Trouble History or Preventive Maintenannce History");

                IQueryable<MaintenanceAction> obj = from data in db.v_Maintenance_Action.Where(x => x.dept_id == deptid && x.mc_id == mcid && x.mtc_id == mtcid &&
                                              x.trouble_id == troubleid && x.mtc_id == mtcid && x.mtc_action_id == mtc_actionid)
                                                    select new MaintenanceAction()
                                                    {
                                                        dept_id = data.dept_id,
                                                        mc_id = data.mc_id,
                                                        mtc_id = data.mtc_id,
                                                        trouble_id = data.trouble_id,
                                                        trouble_name = data.trouble_name,
                                                        mtc_action_id = data.mtc_action_id,
                                                        mtc_action_name = data.mtc_action_name,
                                                        dept_name = data.dept_name,
                                                        mc_name = data.mc_name,
                                                        mtc_name = data.mtc_name
                                                    };
                return View(obj.FirstOrDefault());
            }

            using (DbContextTransaction trans = db.Database.BeginTransaction()) {
                try
                {
                    ms_maintenance_action ms_maintenance_action = db.ms_maintenance_action.Find(deptid, mcid, mtcid, troubleid, mtc_actionid);
                    db.ms_maintenance_action.Remove(ms_maintenance_action);
                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    try
                    {
                        trans.Rollback();
                    } catch (Exception InnerEx)
                    {
                        throw new System.Exception("Rollback Issue - " + InnerEx.Message.ToString() + " " + Environment.NewLine);
                    }
                    ModelState.AddModelError("", "Failed Transaction because " + ex.Message.ToString());

                    IQueryable<MaintenanceAction> obj = from data in db.v_Maintenance_Action.Where(x => x.dept_id == deptid && x.mc_id == mcid && x.mtc_id == mtcid &&
                                            x.trouble_id == troubleid && x.mtc_id == mtcid && x.mtc_action_id == mtc_actionid)
                                                        select new MaintenanceAction()
                                                        {
                                                            dept_id = data.dept_id,
                                                            mc_id = data.mc_id,
                                                            mtc_id = data.mtc_id,
                                                            trouble_id = data.trouble_id,
                                                            trouble_name = data.trouble_name,
                                                            mtc_action_id = data.mtc_action_id,
                                                            mtc_action_name = data.mtc_action_name,
                                                            dept_name = data.dept_name,
                                                            mc_name = data.mc_name,
                                                            mtc_name = data.mtc_name
                                                        };
                    return View(obj.FirstOrDefault());
                }
                
            }
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
