using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ISM_MAINTENANCE.Models.DB;
using ISM_MAINTENANCE.Models.ViewModel;
using System.Text.RegularExpressions;
using Helper;
using System.Collections.Generic;

namespace ISM_MAINTENANCE.Controllers
{
    [Authorize]
    public class MachineTroubleController : Controller
    {
        private WvMaintenanceEntities db = new WvMaintenanceEntities();
        ButtonAccess _button = new ButtonAccess();

        private void InitializeUserAkses()
        {
            if (db.v_user_ism_maintenance.Where(x => x.ID == User.Identity.Name.ToString().Trim()).AsNoTracking().FirstOrDefault() == null)
            {
                ViewBag.Section = ""; ViewBag.Roles = ""; ViewBag.UserAksesMenu = "";
            }
            else
            {
                ViewBag.UserAksesMenu = db.ms_user_access_menu.Where(x => x.userid == User.Identity.Name.ToString().Trim()).AsNoTracking().ToList();
                ViewBag.Section = db.v_user_ism_maintenance.Where(x => x.ID == User.Identity.Name.ToString().Trim()).AsNoTracking().FirstOrDefault().section.ToString().Trim();
                ViewBag.Roles = db.v_user_ism_maintenance.Where(x => x.ID == User.Identity.Name.ToString().Trim()).AsNoTracking().FirstOrDefault().roles.ToString().Trim();
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


        // GET: MachineTrouble
        [HandleError]
        public ActionResult Index()
        {
            InitializeUserAkses();

            ViewBag.dept_id = new SelectList(db.ms_dept, "dept_id", "dept_name");
            IQueryable<MachineTrouble> obj = from data in db.v_Machine_Trouble
                                             select new MachineTrouble()
                                             {
                                                 dept_id = data.dept_id,
                                                 mc_id = data.mc_id,
                                                 trouble_id = data.Code,
                                                 trouble_name = data.Description,
                                                 dept_name = data.Department,
                                                 mc_name = data.Machine
                                             };
            return View(obj.AsNoTracking());
        }


        // GET: MachineTrouble/Create
        public ActionResult Create()
        {
            InitializeUserAkses();
            var access_button = db.ms_user_access_button.Where(x => x.userid == User.Identity.Name.ToString().Trim() && x.menu == "MASTER" && x.submenu.ToString().Trim() == "Machine Trouble");
            if (access_button.Count() > 0)
            {
                List<string> _button_list = access_button.AsNoTracking().FirstOrDefault().button.ToString().Split(';').ToList();
                if (!_button.GetAccess(_button_list, "Add New"))
                {
                    return RedirectToAction("AccessDenied", "Otorisasi");
                }
            }
            else
            {
                return RedirectToAction("AccessDenied", "Otorisasi");
            }


            string dept_code_cim = db.sysuser_app.Where(x => x.ID == User.Identity.Name).Select(x => x.departement).FirstOrDefault();
            string dept_name_cim = db.departement_master.Where(x => x.Dept_ID == dept_code_cim).Select(x => x.Dept_Desc).FirstOrDefault().ToUpper();
            decimal dept_id_system = db.ms_dept.Where(x => x.dept_name == dept_name_cim).Select(x => x.dept_id).FirstOrDefault();
            
            ViewBag.dept_id = new SelectList(db.ms_dept.Where(x => x.dept_id == dept_id_system), "dept_id", "dept_name", 800);
            ViewBag.mc_id = new SelectList(db.ms_machine_type.Where(x => x.dept_id == dept_id_system && x.mc_id == 70), "mc_id", "mc_name", 70);
            var max_no = db.ms_machine_trouble.Where(x => x.dept_id == dept_id_system && x.mc_id == 70).OrderByDescending(x => x.trouble_id).Select(x => x.trouble_id).FirstOrDefault();

            string No;
            if (max_no == null || max_no == "")
            {
                No = "01";
            }
            else
            {
                No = Regex.Match(max_no.ToString(), @"\d+").Value;
                if (No.Length == 1)
                {
                    No = "0" + (Convert.ToInt16(No) + 1);
                }
                else
                {
                    No = Convert.ToString((Convert.ToInt16(No) + 1));
                }
            }

            MachineTrouble EmptyObejct = new MachineTrouble();
            EmptyObejct.trouble_id = max_no.Substring(0, 2).ToString().Trim() + No.Trim();
            return View(EmptyObejct);
        }

        // POST: MachineTrouble/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [HandleError]
        //[ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "dept_id,mc_id,trouble_id,trouble_name")] MachineTrouble frmData)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.dept_id = new SelectList(db.ms_dept, "dept_id", "dept_name");
                ViewBag.mc_id = new SelectList(db.ms_machine_type, "mc_id", "mc_name");
                return View();
            }

            ms_machine_trouble Duplicate = db.ms_machine_trouble.Find(frmData.dept_id, frmData.mc_id, frmData.trouble_id);
            if (Duplicate != null)
            {
                ModelState.AddModelError("", "Duplicate Data");
                ViewBag.dept_id = new SelectList(db.ms_dept, "dept_id", "dept_name");
                ViewBag.mc_id = new SelectList(db.ms_machine_type, "mc_id", "mc_name");
                return View("");
            }

            ms_machine_trouble obj = new ms_machine_trouble();
            obj.dept_id = frmData.dept_id;
            obj.mc_id = frmData.mc_id;
            obj.trouble_id = frmData.trouble_id;
            obj.trouble_name = frmData.trouble_name.ToUpper();
            obj.user_id = User.Identity.Name.ToUpper();
            obj.act_rec = "A";
            obj.client_ip = GetIPAddress();
            obj.proc_time = DateTime.Now;

            if (ModelState.IsValid)
            {
                db.ms_machine_trouble.Add(obj);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View("");
        }

        // GET: MachineTrouble/Show/5
        public ActionResult Show(string id)
        {
            InitializeUserAkses();

            var access_button = db.ms_user_access_button.Where(x => x.userid == User.Identity.Name.ToString().Trim() && x.menu == "MASTER" && x.submenu.ToString().Trim() == "Machine Trouble");
            if (access_button.Count() > 0)
            {
                List<string> _button_list = access_button.AsNoTracking().FirstOrDefault().button.ToString().Split(';').ToList();
                if (!_button.GetAccess(_button_list, "Show"))
                {
                    return RedirectToAction("AccessDenied", "Otorisasi");
                }
            }
            else
            {
                return RedirectToAction("AccessDenied", "Otorisasi");
            }


            if (id == null)
            {
                return RedirectToAction("Index");
            }
            int deptid, mcid;
            string troubleid;

            string[] par = id.Split('-');
            if (par.Length != 1)
            {
                deptid = Convert.ToInt32(par[0]);
                mcid = Convert.ToInt32(par[1]);
                troubleid = par[2].ToString();
            }
            else
            {
                deptid = 0; mcid = 0; troubleid = "";
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            IQueryable<MachineTrouble> obj = from data in db.v_Machine_Trouble
                                             where data.dept_id == deptid && data.mc_id == mcid && data.Code == troubleid.ToString()
                                             select new MachineTrouble()
                                             {
                                                 dept_id = data.dept_id,
                                                 mc_id = data.mc_id,
                                                 trouble_id = data.Code,
                                                 trouble_name = data.Description,
                                                 dept_name = data.Department,
                                                 mc_name = data.Machine
                                             };

            if (obj == null)
            {
                return HttpNotFound();
            }
            return View(obj.AsNoTracking().FirstOrDefault());
        }

        // GET: MachineTrouble/Edit/5
        public ActionResult Edit(string id)
        {
            InitializeUserAkses();
            var access_button = db.ms_user_access_button.Where(x => x.userid == User.Identity.Name.ToString().Trim() && x.menu == "MASTER" && x.submenu.ToString().Trim() == "Machine Trouble");
            if (access_button.Count() > 0)
            {
                List<string> _button_list = access_button.AsNoTracking().FirstOrDefault().button.ToString().Split(';').ToList();
                if (!_button.GetAccess(_button_list, "Edit"))
                {
                    return RedirectToAction("AccessDenied", "Otorisasi");
                }
            }
            else
            {
                return RedirectToAction("AccessDenied", "Otorisasi");
            }


            if (id == null)
            {
                return RedirectToAction("Index");
            }
            int deptid, mcid;
            string troubleid;

            string[] par = id.Split('-');
            if (par.Length != 1)
            {
                deptid = Convert.ToInt32(par[0]);
                mcid = Convert.ToInt32(par[1]);
                troubleid = par[2].ToString();
            }
            else
            {
                deptid = 0; mcid = 0; troubleid = "";
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            IQueryable<MachineTrouble> obj = from data in db.v_Machine_Trouble
                                             where data.dept_id == deptid && data.mc_id == mcid && data.Code == troubleid.ToString()
                                             select new MachineTrouble()
                                             {
                                                 dept_id = data.dept_id,
                                                 mc_id = data.mc_id,
                                                 trouble_id = data.Code,
                                                 trouble_name = data.Description,
                                                 dept_name = data.Department,
                                                 mc_name = data.Machine
                                             };

            if (obj == null)
            {
                return HttpNotFound();
            }
            return View(obj.AsNoTracking().FirstOrDefault());
        }

        // POST: MachineTrouble/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "dept_id,mc_id,trouble_id,trouble_name")] MachineTrouble FrmData)
        {
            InitializeUserAkses();
            if (ModelState.IsValid)
            {
                using (DbContextTransaction transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        ms_machine_trouble obj = db.ms_machine_trouble.Single(x => x.dept_id == FrmData.dept_id && x.mc_id == FrmData.mc_id && x.trouble_id == FrmData.trouble_id);

                        ms_machine_trouble_hist history = new ms_machine_trouble_hist();
                        history.dept_id = obj.dept_id;
                        history.mc_id = obj.mc_id;
                        history.trouble_id = obj.trouble_id;
                        history.trouble_name = obj.trouble_name;
                        history.act_rec = obj.act_rec;
                        history.proc_time = obj.proc_time;
                        history.client_ip = obj.client_ip;
                        history.user_id = obj.user_id;

                        db.ms_machine_trouble_hist.Add(history);
                        db.SaveChanges();

                        obj.trouble_name = FrmData.trouble_name.ToUpper();
                        obj.user_id = User.Identity.Name.ToUpper();
                        obj.client_ip = Request.UserHostAddress;
                        obj.proc_time = DateTime.Now;
                        obj.act_rec = "T";

                        db.Entry(obj).State = EntityState.Modified;
                        db.SaveChanges();
                        transaction.Commit();
                    }

                    catch (Exception ex)
                    {
                        try
                        {
                            transaction.Rollback();
                        }
                        catch (Exception InnerEx)
                        {
                            throw new SystemException("Rollback error - " + InnerEx.Message.ToString() + Environment.NewLine);
                        }

                        ModelState.AddModelError("", "Failed Transaction because " + ex.Message.ToString());
                        InitializeUserAkses();
                        return View(FrmData);
                    }
                }

                return RedirectToAction("Index");
            }

            ViewBag.dept_id = new SelectList(db.ms_dept, "dept_id", "dept_name");
            ViewBag.mc_id = new SelectList(db.ms_machine_type, "mc_id", "mc_name");
            return View();
        }

        // GET: MachineTrouble/Delete/5
        public ActionResult Delete(string id)
        {
            ViewBag.HideYesButton = "";
            InitializeUserAkses();
            var access_button = db.ms_user_access_button.Where(x => x.userid == User.Identity.Name.ToString().Trim() && x.menu == "MASTER" && x.submenu.ToString().Trim() == "Machine Trouble");
            if (access_button.Count() > 0)
            {
                List<string> _button_list = access_button.AsNoTracking().FirstOrDefault().button.ToString().Split(';').ToList();
                if (!_button.GetAccess(_button_list, "Delete"))
                {
                    return RedirectToAction("AccessDenied", "Otorisasi");
                }
            }
            else
            {
                return RedirectToAction("AccessDenied", "Otorisasi");
            }


            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            int deptid, mcid;
            string troubleid;

            string[] par = id.Split('-');
            if (par.Length != 1)
            {
                deptid = Convert.ToInt32(par[0]);
                mcid = Convert.ToInt32(par[1]);
                troubleid = par[2].ToString();
            }
            else
            {
                deptid = 0; mcid = 0; troubleid = "";
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            IQueryable<MachineTrouble> obj = from data in db.v_Machine_Trouble
                                             where data.dept_id == deptid && data.mc_id == mcid && data.Code == troubleid.ToString()
                                             select new MachineTrouble()
                                             {
                                                 dept_id = data.dept_id,
                                                 mc_id = data.mc_id,
                                                 trouble_id = data.Code,
                                                 trouble_name = data.Description,
                                                 dept_name = data.Department,
                                                 mc_name = data.Machine
                                             };

            if (obj == null)
            {
                return HttpNotFound();
            }
            return View(obj.AsNoTracking().FirstOrDefault());
        }

        // POST: MachineTrouble/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            InitializeUserAkses();

            int deptid, mcid;
            string troubleid;

            string[] par = id.Split('-');
            if (par.Length != 1)
            {
                deptid = Convert.ToInt32(par[0]);
                mcid = Convert.ToInt32(par[1]);
                troubleid = par[2].ToString();
            }
            else
            {
                deptid = 0; mcid = 0; troubleid = "";
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var Find_ShiftLeaderNotes_Detail = db.tr_shift_leader_note_detail.Where(x => x.dept_id == deptid && x.mc_id == mcid && x.trouble_id == troubleid).ToList();
            if (Find_ShiftLeaderNotes_Detail.Count > 0)
            {
                ViewBag.HideYesButton = "True";
                ModelState.AddModelError("", "!!Remove Data Failed, Data already used in Shift Leader Notes !!");
                IQueryable<MachineTrouble> obj = from data in db.v_Machine_Trouble
                                                 where data.dept_id == deptid && data.mc_id == mcid && data.Code == troubleid.ToString()
                                                 select new MachineTrouble()
                                                 {
                                                     dept_id = data.dept_id,
                                                     mc_id = data.mc_id,
                                                     trouble_id = data.Code,
                                                     trouble_name = data.Description,
                                                     dept_name = data.Department,
                                                     mc_name = data.Machine
                                                 };
                return View(obj.FirstOrDefault());
            }
            
            var FindMs_Mtc_action = db.ms_maintenance_action.Where(x => x.dept_id == deptid && x.mc_id == mcid && x.trouble_id == troubleid).ToList();
            if (FindMs_Mtc_action.Count > 0)
            {
                ViewBag.HideYesButton = "True";
                ModelState.AddModelError("", "!!Remove Data Failed, Data already used in Master Maintenance Action !!");
                IQueryable<MachineTrouble> obj = from data in db.v_Machine_Trouble
                                                 where data.dept_id == deptid && data.mc_id == mcid && data.Code == troubleid.ToString()
                                                 select new MachineTrouble()
                                                 {
                                                     dept_id = data.dept_id,
                                                     mc_id = data.mc_id,
                                                     trouble_id = data.Code,
                                                     trouble_name = data.Description,
                                                     dept_name = data.Department,
                                                     mc_name = data.Machine
                                                 };
                return View(obj.FirstOrDefault());
            }

            using (DbContextTransaction trans = db.Database.BeginTransaction())
            {
                try
                {
                    //throw new System.Exception("Tes Error");

                    ms_machine_trouble ms_machine_trouble = db.ms_machine_trouble.Find(deptid, mcid, troubleid);

                    ms_machine_trouble_hist History = new ms_machine_trouble_hist();
                    History.dept_id = ms_machine_trouble.dept_id;
                    History.mc_id = ms_machine_trouble.mc_id;
                    History.trouble_id = ms_machine_trouble.trouble_id;
                    History.trouble_name = ms_machine_trouble.trouble_name;
                    History.user_id = User.Identity.Name.ToString().ToUpper().Trim();
                    History.client_ip = GetIPAddress();
                    History.act_rec = ms_machine_trouble.act_rec;
                    History.proc_time = DateTime.Now;
                    db.ms_machine_trouble_hist.Add(History);
                    db.SaveChanges();

                    db.ms_machine_trouble.Remove(ms_machine_trouble);
                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    try
                    {

                        trans.Rollback();
                    }
                    catch (Exception InnerEx)
                    {
                        throw new System.Exception("Rollback Isssue - " + InnerEx.Message.ToString() + Environment.NewLine);
                    }

                    ModelState.AddModelError("", "Failed Transaction because " + ex.Message.ToString());

                    IQueryable<MachineTrouble> obj = from data in db.v_Machine_Trouble
                                                     where data.dept_id == deptid && data.mc_id == mcid && data.Code == troubleid.ToString()
                                                     select new MachineTrouble()
                                                     {
                                                         dept_id = data.dept_id,
                                                         mc_id = data.mc_id,
                                                         trouble_id = data.Code,
                                                         trouble_name = data.Description,
                                                         dept_name = data.Department,
                                                         mc_name = data.Machine
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
