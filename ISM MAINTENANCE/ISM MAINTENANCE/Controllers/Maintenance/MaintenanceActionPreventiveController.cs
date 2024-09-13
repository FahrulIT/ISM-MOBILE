using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ISM_MAINTENANCE.Models.DB;
using Newtonsoft.Json;
using Helper;

namespace ISM_MAINTENANCE.Controllers.Maintenance
{
    [Authorize]
    public class MaintenanceActionPreventiveController : Controller
    {
        private WvMaintenanceEntities db = new WvMaintenanceEntities();
        ButtonAccess _button = new ButtonAccess();

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

        string mtcId = "MTC30";

        public class mtc
        {
            public string mtcActionNo { get; set; }
        }

        private string GetIPAddress()
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

        private List<SelectListItem> msAction()
        {
            var sql = "select * from ms_maintenance_action " +
                "where mtc_id = '" + mtcId + "' " +
                "order by mtc_action_name asc ";
            var data = db.Database.SqlQuery<ms_maintenance_action>(sql);
            var no = data.ToList();

            List<SelectListItem> tr = new List<SelectListItem>();
            foreach (var i in no)
            {
                var v = i.mtc_action_id;
                var t = i.mtc_action_name;
                tr.Add(new SelectListItem { Value = v, Text = t });
            }

            return tr;
        }

        // GET: MaintenanceActionPreventive
        public ActionResult Index()
        {
            InitializeUserAkses();

            ViewBag.msAction = msAction();
            ViewBag.mtcAction = "";

            var sql = "select * from vw_ms_maintenance_action_preventive " +
                "where mtc_id = '" + mtcId + "' " +
                "order by mtc_action_name asc ";
            var datas = db.Database.SqlQuery<vw_ms_maintenance_action_preventive>(sql).ToList();

            return View(datas);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Index(FormCollection act)
        {
            InitializeUserAkses();

            string mtcAction = act["mtcAction"];

            var sql = "select * from vw_ms_maintenance_action_preventive " +
                "where mtc_id = '" + mtcId + "' and mtc_action_id like '%" + mtcAction + "%' " +
                "order by mtc_action_name asc ";
            var filter = db.Database.SqlQuery<vw_ms_maintenance_action_preventive>(sql).ToList();

            ViewBag.msAction = msAction();
            ViewBag.mtcAction = mtcAction;

            return View(filter);
        }

        // GET: MaintenanceActionPreventive/Create
        public ActionResult Create()
        {
            InitializeUserAkses();
            var access_button = db.ms_user_access_button.Where(x => x.userid == User.Identity.Name.ToString().Trim() && x.menu == "MASTER" && x.submenu.ToString().Trim() == "Maintenance Action");
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

            var sql = "select top 1 case when convert(int,substring(mtc_action_id, 3,2)) < 10 then substring(mtc_action_id, 1,2) + '0' + convert(varchar(3),convert(int,substring(mtc_action_id, 3,2)) + 1) else substring(mtc_action_id, 1,2) + convert(varchar(3),convert(int,substring(mtc_action_id, 3,2)) + 1) end as mtcActionNo " +
                "from ms_maintenance_action " +
                "where mtc_id = '" + mtcId + "' " +
                "order by mtc_action_id desc ";
            var mtc = db.Database.SqlQuery<mtc>(sql).FirstOrDefault().mtcActionNo;
            ViewBag.mtcAction = mtc;

            return View();
        }

        // POST: MaintenanceActionQuality/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "department, machine, troubelName, mtcActionName, dept_id, mc_id, trouble_id, mtc_action_id, mtc_id, mtc_action_name")] vw_ms_maintenance_action_preventive main)
        {
            InitializeUserAkses();

            ms_maintenance_action ms = new ms_maintenance_action();

            if (ModelState.IsValid)
            {
                using (DbContextTransaction tran = db.Database.BeginTransaction())
                {
                    try
                    {

                        var sql = "";

                        sql = "select * from ms_dept where dept_name = '" + main.department + "' ";
                        var dept = db.Database.SqlQuery<ms_dept>(sql).FirstOrDefault().dept_id;

                        sql = "select * from ms_machine_type where mc_name = '" + main.machine + "' and dept_id = " + dept + " ";
                        var mc = db.Database.SqlQuery<ms_machine_type>(sql).FirstOrDefault().mc_id;

                        var trouble = "-";
                        var mtc = main.mtc_action_id;
                        var mtcName = main.mtc_action_name.ToString().ToUpper();

                        ms.dept_id = dept;
                        ms.mc_id = mc;
                        ms.mtc_id = mtcId;
                        ms.trouble_id = trouble;
                        ms.mtc_action_id = mtc;
                        ms.mtc_action_name = mtcName;

                        ms.user_id = User.Identity.Name;
                        ms.act_rec = "A";
                        ms.client_ip = GetIPAddress();
                        ms.proc_time = DateTime.Now;

                        db.ms_maintenance_action.Add(ms);
                        db.SaveChanges();

                        tran.Commit();
                        return RedirectToAction("Index");
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        ViewBag.error = ex.Message;
                    }
                }
            }

            return View(ms);
        }

        // GET: MaintenanceActionQuality/Edit/5
        public ActionResult Edit(int? dept, int? mc, string mtc, string trouble, string act)
        {
            InitializeUserAkses();
            var access_button = db.ms_user_access_button.Where(x => x.userid == User.Identity.Name.ToString().Trim() && x.menu == "MASTER" && x.submenu.ToString().Trim() == "Maintenance Action");
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

            var sql = "select * from vw_ms_maintenance_action_preventive " +
                "where dept_id = " + dept + " and mc_id = " + mc + " " +
                "and mtc_id = '" + mtc + "' and trouble_id = '" + trouble + "' " +
                "and mtc_action_id = '" + act + "' ";
            var ms = db.Database.SqlQuery<vw_ms_maintenance_action_preventive>(sql).FirstOrDefault();

            if (ms == null)
            {
                return HttpNotFound();
            }
            return View(ms);
        }

        // POST: MaintenanceActionQuality/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "department, machine, troubelName, mtcActionName, dept_id, mc_id, trouble_id, mtc_action_id, mtc_id, mtc_action_name")] vw_ms_maintenance_action_preventive main)
        {
            InitializeUserAkses();

            var sql = "select * from ms_maintenance_action " +
                "where dept_id = " + main.dept_id + " and mc_id = " + main.mc_id + " " +
                "and mtc_id = '" + main.mtc_id + "' and trouble_id = '" + main.trouble_id + "' " +
                "and mtc_action_id = '" + main.mtc_action_id + "' ";
            var ms = db.Database.SqlQuery<ms_maintenance_action>(sql).FirstOrDefault();

            if (ModelState.IsValid)
            {

                using (DbContextTransaction tran = db.Database.BeginTransaction())
                {
                    try
                    {
                        ms.mtc_action_name = main.mtc_action_name;

                        db.Entry(ms).State = EntityState.Modified;
                        db.SaveChanges();

                        tran.Commit();
                        return RedirectToAction("Index");
                    }
                    catch (Exception ex)
                    {

                        tran.Rollback();
                        ViewBag.error = ex.Message;
                    }
                }
            }
            return View(ms);
        }

        // GET: MaintenanceActionQuality/Delete/5
        public ActionResult Delete(int? dept, int? mc, string mtc, string trouble, string act)
        {
            InitializeUserAkses();
            var access_button = db.ms_user_access_button.Where(x => x.userid == User.Identity.Name.ToString().Trim() && x.menu == "MASTER" && x.submenu.ToString().Trim() == "Maintenance Action");
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

            if (dept == null || mc == null || mtc == null || trouble == null || act == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var sql = "select * from vw_ms_maintenance_action_preventive " +
                "where dept_id = " + dept + " and mc_id = " + mc + " " +
                "and mtc_id = '" + mtc + "' and trouble_id = '" + trouble + "' " +
                "and mtc_action_id = '" + act + "' ";
            var ms = db.Database.SqlQuery<vw_ms_maintenance_action_preventive>(sql).FirstOrDefault();

            if (ms == null)
            {
                return HttpNotFound();
            }
            return View(ms);
        }

        // POST: MaintenanceActionQuality/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? dept, int? mc, string mtc, string trouble, string act)
        {
            InitializeUserAkses();

            using (DbContextTransaction tran = db.Database.BeginTransaction())
            {
                try
                {

                    //########################## (start) delete to ms_maintenance_action #######################//

                    var sql = "";
                    sql = "insert into ms_maintenance_action_hist " +
                        "select * from ms_maintenance_action " +
                        "where dept_id = " + dept + " and mc_id = " + mc + " " +
                        "and mtc_id = '" + mtc + "' and trouble_id = '" + trouble + "' " +
                        "and mtc_action_id = '" + act + "' ";
                    db.Database.ExecuteSqlCommand(sql);

                    sql = "delete from ms_maintenance_action " +
                        "where dept_id = " + dept + " and mc_id = " + mc + " " +
                        "and mtc_id = '" + mtc + "' and trouble_id = '" + trouble + "' " +
                        "and mtc_action_id = '" + act + "' ";
                    db.Database.ExecuteSqlCommand(sql);

                    //########################## (finish) delete to ms_maintenance_action #######################//

                    tran.Commit();

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {

                    tran.Rollback();
                    ViewBag.error = ex.Message;
                }
            }

            return RedirectToAction("Index");
        }

    }
}
