using System;
using System.Collections.Generic;
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
    public class MaintenanceActivityController : Controller
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


        //    // GET: MaintenanceActivity
        //    [Authorize]
        //    public ActionResult Index()
        //    {
        //        IQueryable<MaintenanceActivity> obj = from data in db.v_Maintenance_Activity
        //                                              select new MaintenanceActivity()
        //                                              {
        //                                                  dept_id = data.dept_id,
        //                                                  mc_id = (int)data.mc_id,
        //                                                  mtc_slip_no1 = data.mtc_slip_no1,
        //                                                  mtc_slip_no2 = data.mtc_slip_no2,
        //                                                  mtc_id = data.mtc_id,
        //                                                  issue_date = data.issue_date,
        //                                                  block = data.block,
        //                                                  loom_no = data.loom_no,
        //                                                  dept_name = data.dept_name,
        //                                                  mc_name = data.mc_name,
        //                                                  mtc_name = data.mtc_name,
        //                                                  mtc_status = data.mtc_status,
        //                                                  machine_no = data.block + "." + data.loom_no,
        //                                                  maintenance_slip = data.mtc_slip_no1 + "-" + data.mtc_slip_no2
        //                                              };
        //        return View(obj);

        //    }


        // GET: MaintenanceActivity/Create
        [Authorize]
    public ActionResult Create()
    {
        string dept_code_cim = db.sysuser_app.Where(x => x.ID == User.Identity.Name).Select(x => x.departement).FirstOrDefault();
        string dept_name_cim = db.departement_master.Where(x => x.Dept_ID == dept_code_cim).Select(x => x.Dept_Desc).FirstOrDefault().ToUpper();
        decimal dept_id_system = db.ms_dept.Where(x => x.dept_name == dept_name_cim).Select(x => x.dept_id).FirstOrDefault();

        ViewBag.dept_id = new SelectList(db.ms_dept.Where(x => x.dept_id == dept_id_system), "dept_id", "dept_name");
        ViewBag.mc_id = new SelectList(db.ms_machine_type.Where(x => x.dept_id == dept_id_system && x.mc_id == 70), "mc_id", "mc_name");
        ViewBag.mtc_id = new SelectList(db.ms_maintenance, "mtc_id", "mtc_name");
        ViewBag.machine_no = new SelectList(db.v_Machine_Master_AJL, "MachineNo", "MachineNo");

        MaintenanceActivity obj = new MaintenanceActivity();
        return View(obj);
    }

        //    // POST: MaintenanceActivity/Create
        //    // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //    // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //    [HttpPost]
        //    [Authorize]
        //    public ActionResult Create([Bind(Include = "dept_id,mc_id,mtc_slip_no1,mtc_slip_no2,issue_date,machine_no,mtc_id")] MaintenanceActivity FrmData)
        //    {
        //        string dept_code_cim, dept_name_cim;
        //        decimal dept_id_system;

        //        tr_maintenance Duplicate = db.tr_maintenance.Find(FrmData.dept_id, FrmData.mc_id, FrmData.mtc_slip_no1, FrmData.mtc_slip_no2);
        //        if (Duplicate != null)
        //        {
        //            ModelState.AddModelError("", "Duplicate Data");

        //            dept_code_cim = db.sysuser_app.Where(x => x.ID == User.Identity.Name).Select(x => x.departement).FirstOrDefault();
        //            dept_name_cim = db.departement_master.Where(x => x.Dept_ID == dept_code_cim).Select(x => x.Dept_Desc).FirstOrDefault().ToUpper();
        //            dept_id_system = db.ms_dept.Where(x => x.dept_name == dept_name_cim).Select(x => x.dept_id).FirstOrDefault();

        //            ViewBag.dept_id = new SelectList(db.ms_dept.Where(x => x.dept_id == dept_id_system), "dept_id", "dept_name");
        //            ViewBag.mc_id = new SelectList(db.ms_machine_type.Where(x => x.dept_id == dept_id_system), "mc_id", "mc_name");
        //            ViewBag.mtc_id = new SelectList(db.ms_maintenance, "mtc_id", "mtc_name");
        //            ViewBag.machine_no = new SelectList(db.v_Machine_Master_AJL, "MachineNo", "MachineNo");

        //            return View("");
        //        }

        //        if (ModelState.IsValid)
        //        {
        //            string loom_no = ""; string block = "";
        //            string[] machine_no = FrmData.machine_no.ToString().Split('.');
        //            if (machine_no.Length != 1)
        //            {
        //                block = machine_no[0].ToString();
        //                loom_no = machine_no[1].ToString();
        //            }
        //            else
        //            {
        //                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //            }

        //            tr_maintenance obj = new tr_maintenance();
        //            obj.dept_id = FrmData.dept_id;
        //            obj.mc_id = FrmData.mc_id;
        //            obj.mtc_slip_no1 = FrmData.mtc_slip_no1;
        //            obj.mtc_slip_no2 = FrmData.mtc_slip_no2;
        //            obj.issue_date = FrmData.issue_date;
        //            obj.block = block;
        //            obj.loom_no = loom_no;
        //            obj.mtc_id = FrmData.mtc_id;
        //            obj.user_id = User.Identity.Name;
        //            obj.rec_sts = "A";
        //            obj.proc_no = 1;
        //            obj.proc_time = DateTime.Now;
        //            obj.client_ip = GetIPAddress();

        //            db.tr_maintenance.Add(obj);
        //            db.SaveChanges();
        //            return RedirectToAction("Index");
        //        }

        //        //Error state & validation error
        //        dept_code_cim = db.sysuser_app.Where(x => x.ID == User.Identity.Name).Select(x => x.departement).FirstOrDefault();
        //        dept_name_cim = db.departement_master.Where(x => x.Dept_ID == dept_code_cim).Select(x => x.Dept_Desc).FirstOrDefault().ToUpper();
        //        dept_id_system = db.ms_dept.Where(x => x.dept_name == dept_name_cim).Select(x => x.dept_id).FirstOrDefault();

        //        ViewBag.dept_id = new SelectList(db.ms_dept.Where(x => x.dept_id == dept_id_system), "dept_id", "dept_name");
        //        ViewBag.mc_id = new SelectList(db.ms_machine_type.Where(x => x.dept_id == dept_id_system), "mc_id", "mc_name");
        //        ViewBag.mtc_id = new SelectList(db.ms_maintenance, "mtc_id", "mtc_name");
        //        ViewBag.machine_no = new SelectList(db.v_Machine_Master_AJL, "MachineNo", "MachineNo");
        //        return View("");
        //    }

        //    // GET: MaintenanceActivity/Edit/5
        //    [Authorize]
        //    public ActionResult Edit(string id)
        //    {

        //        if (id == null)
        //        {
        //            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //        }

        //        decimal deptid, mcid;
        //        string mtc_slip_01, mtc_slip_02;

        //        string[] par = id.Split(';');
        //        if (par.Length != 1)
        //        {
        //            deptid = Convert.ToDecimal(par[0]);
        //            mcid = Convert.ToDecimal(par[1]);
        //            mtc_slip_01 = par[2].ToString();
        //            mtc_slip_02 = par[3].ToString();
        //        }
        //        else
        //        {
        //            deptid = 0; mcid = 0; mtc_slip_01 = ""; mtc_slip_02 = "";
        //            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //        }

        //        IQueryable<MaintenanceActivity> obj = from data in db.v_Maintenance_Activity
        //                                              where data.dept_id == deptid && data.mc_id == mcid && data.mtc_slip_no1 == mtc_slip_01 && data.mtc_slip_no2 == mtc_slip_02
        //                                              select new MaintenanceActivity()
        //                                              {
        //                                                  dept_id = data.dept_id,
        //                                                  mc_id = (int)data.mc_id,
        //                                                  mtc_slip_no1 = data.mtc_slip_no1,
        //                                                  mtc_slip_no2 = data.mtc_slip_no2,
        //                                                  mtc_id = data.mtc_id,
        //                                                  issue_date = data.issue_date,
        //                                                  block = data.block,
        //                                                  loom_no = data.loom_no,
        //                                                  dept_name = data.dept_name,
        //                                                  mc_name = data.mc_name,
        //                                                  mtc_name = data.mtc_name,
        //                                                  mtc_status = data.mtc_status,
        //                                                  machine_no = data.block + "." + data.loom_no,
        //                                                  maintenance_slip = data.mtc_slip_no1 + "-" + data.mtc_slip_no2
        //                                              };

        //        if (obj == null)
        //        {
        //            return HttpNotFound();
        //        }
        //        return View(obj.FirstOrDefault());
        //    }

        //    // POST: MaintenanceActivity/Edit/5
        //    // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //    // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //    [HttpPost]
        //    [Authorize]
        //    public ActionResult Edit([Bind(Include = "dept_id,mc_id,mtc_slip_no1,mtc_slip_no2,issue_date,block,loom_no,mtc_id,user_id,rec_sts,proc_no,client_ip,proc_time")] tr_maintenance tr_maintenance)
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            db.Entry(tr_maintenance).State = EntityState.Modified;
        //            db.SaveChanges();
        //            return RedirectToAction("Index");
        //        }
        //        return View(tr_maintenance);
        //    }

        //    // GET: MaintenanceActivity/Delete/5
        //    public ActionResult Delete(string id)
        //    {
        //        if (id == null)
        //        {
        //            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //        }

        //        decimal deptid, mcid;
        //        string mtc_slip_01, mtc_slip_02;

        //        string[] par = id.Split(';');
        //        if (par.Length != 1)
        //        {
        //            deptid = Convert.ToDecimal(par[0]);
        //            mcid = Convert.ToDecimal(par[1]);
        //            mtc_slip_01 = par[2].ToString();
        //            mtc_slip_02 = par[3].ToString();
        //        }
        //        else
        //        {
        //            deptid = 0; mcid = 0; mtc_slip_01 = ""; mtc_slip_02 = "";
        //            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //        }

        //        IQueryable<MaintenanceActivity> obj = from data in db.v_Maintenance_Activity
        //                                              where data.dept_id == deptid && data.mc_id == mcid && data.mtc_slip_no1 == mtc_slip_01 && data.mtc_slip_no2 == mtc_slip_02
        //                                              select new MaintenanceActivity()
        //                                              {
        //                                                  dept_id = data.dept_id,
        //                                                  mc_id = (int)data.mc_id,
        //                                                  mtc_slip_no1 = data.mtc_slip_no1,
        //                                                  mtc_slip_no2 = data.mtc_slip_no2,
        //                                                  mtc_id = data.mtc_id,
        //                                                  issue_date = data.issue_date,
        //                                                  block = data.block,
        //                                                  loom_no = data.loom_no,
        //                                                  dept_name = data.dept_name,
        //                                                  mc_name = data.mc_name,
        //                                                  mtc_name = data.mtc_name,
        //                                                  mtc_status = data.mtc_status,
        //                                                  machine_no = data.block + "." + data.loom_no,
        //                                                  maintenance_slip = data.mtc_slip_no1 + "-" + data.mtc_slip_no2
        //                                              };


        //        if (obj == null)
        //        {
        //            return HttpNotFound();
        //        }
        //        return View(obj.FirstOrDefault());
        //    }

        //    // POST: MaintenanceActivity/Delete/5
        //    [HttpPost, ActionName("Delete")]
        //    public ActionResult DeleteConfirmed(string id)
        //    {
        //        if (id == null)
        //        {
        //            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //        }

        //        decimal deptid, mcid;
        //        string mtc_slip_01, mtc_slip_02;

        //        string[] par = id.Split(';');
        //        if (par.Length != 1)
        //        {
        //            deptid = Convert.ToDecimal(par[0]);
        //            mcid = Convert.ToDecimal(par[1]);
        //            mtc_slip_01 = par[2].ToString();
        //            mtc_slip_02 = par[3].ToString();
        //        }
        //        else
        //        {
        //            deptid = 0; mcid = 0; mtc_slip_01 = ""; mtc_slip_02 = "";
        //            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //        }

        //        tr_maintenance tr_maintenance = db.tr_maintenance.Find(deptid, mcid, mtc_slip_01, mtc_slip_02);
        //        db.tr_maintenance.Remove(tr_maintenance);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    protected override void Dispose(bool disposing)
        //    {
        //        if (disposing)
        //        {
        //            db.Dispose();
        //        }
        //        base.Dispose(disposing);
        //    }
    }
}
