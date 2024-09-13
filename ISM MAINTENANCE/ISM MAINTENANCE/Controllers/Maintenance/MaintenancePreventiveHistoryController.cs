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
using System.Globalization;
using Helper;

namespace ISM_MAINTENANCE.Controllers.Maintenance
{
    public class MaintenancePreventiveHistoryController : Controller
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

        private class procNo
        {
            public int proc_no { get; set; }
        }

        private class sparePartHist
        {
            public decimal? quantity { get; set; }
        }

        private class machine
        {
            public string mtc { get; set; }
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

        private List<SelectListItem> machineNo(string block, string loom)
        {
            var sql = "select * from machine_master " +
                "where loom_type = 'AJL' and rec_sts = 'A' and block + '.' + loom_no not in ( " +
                "select x.block + '.' + x.loom_no from tr_prev_maintenance x " +
                "where format(x.mtc_schedule, 'MM-yyyy') = format(getdate(), 'MM-yyyy') " +
                "and x.block + '.' + x.loom_no not in ('" + block + "." + loom + "') " +
                ") " +
                "order by block asc, loom_no asc ";
            var data = db.Database.SqlQuery<machine_master>(sql);
            var no = data.ToList();

            List<SelectListItem> machine = new List<SelectListItem>();
            foreach (var i in no)
            {
                var obj = i.block + "." + i.loom_no;
                machine.Add(new SelectListItem { Value = obj, Text = obj });
            }

            return machine;
        }

        //private List<SelectListItem> userName()
        //{
        //    string id = User.Identity.Name.ToString();
        //    var sql = "select * from sysuser_app " +
        //        "where departement = 'W' and id = '" + id + "' ";
        //    var data = db.Database.SqlQuery<sysuser_app>(sql);
        //    var full = data.ToList();

        //    List<SelectListItem> usr = new List<SelectListItem>();
        //    foreach (var i in full)
        //    {
        //        var obj = i.Fullname;
        //        usr.Add(new SelectListItem { Value = obj, Text = obj });
        //    }

        //    return usr;
        //}

        private List<SelectListItem> mtcAction()
        {
            var sql = "select * from ms_maintenance_action " +
                "where mtc_id = 'MTC30' and act_rec = 'A' order by mtc_action_name asc ";
            var data = db.Database.SqlQuery<ms_maintenance_action>(sql);
            var mtc = data.ToList();

            List<SelectListItem> action = new List<SelectListItem>();
            foreach (var i in mtc)
            {
                var obj = i.mtc_action_name;
                action.Add(new SelectListItem { Value = obj, Text = obj });
            }

            return action;
        }

        private List<SelectListItem> spareParts()
        {
            var sql = "SELECT * FROM [wppc].[v_SAP_OITM] ORDER BY ItemName ";
            var data = db.Database.SqlQuery<v_SAP_OITM>(sql);
            var spare = data.ToList();

            List<SelectListItem> parts = new List<SelectListItem>();
            foreach (var i in spare)
            {
                var obj = i.ItemName;
                parts.Add(new SelectListItem { Value = obj, Text = obj });
            }

            return parts;
        }

        private List<vw_preventivemaintenance_sparepart> trPrevSpareParts(int? dept, int? mc, DateTime mtc, string block, string loom)
        {
            var sql = "select * from [vw_preventivemaintenance_sparepart] " +
                "where dept_id = " + dept + " and mc_id = " + mc + " " +
                "and format(mtc_schedule, 'yyyy-MM-dd') = '" + mtc.ToString("yyyy-MM-dd") + "' " +
                "and block = '" + block + "' and loom_no = '" + loom + "' " +
                "order by ItemName asc ";
            var data = db.Database.SqlQuery<vw_preventivemaintenance_sparepart>(sql);
            var spareParts = data.ToList();

            return spareParts;
        }

        // GET: MaintenancePreventiveHistory
        public ActionResult Index()
        {
            InitializeUserAkses();

            ViewBag.sch = DateTime.Now.ToString("MM-yyyy");
            ViewBag.mtc = "";

            var sql = "select * from vw_preventivemaintenance_history " +
            "where mtc_schedule between (SELECT TOP 1 case when format(mtcSpace,'MM') = format(mtc_schedule, 'MM') then mtcSpace else format(mtc_schedule, 'yyyy-MM-01') end FROM vw_preventivemaintenance_history ORDER BY mtc_schedule DESC) " +
            "and (select top 1 mtc_schedule from vw_preventivemaintenance_history order by mtc_schedule desc) " +
            "order by mtc_schedule desc, mtc_start desc, mtc_finish desc ";
            var datas = db.Database.SqlQuery<vw_preventivemaintenance_history>(sql).ToList();

            return View(datas.ToList());
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Index(FormCollection hist)
        {
            InitializeUserAkses();

            string mtcNo = hist["mtcNo"];
            string[] scheduleDate = hist["mtcSchedule"].Split('-');
            string modSchedule = scheduleDate[1] + "-" + scheduleDate[0] + "-" + DateTime.Now.ToString("dd");
            DateTime mtcSchedule = DateTime.Parse(modSchedule, CultureInfo.CurrentCulture);

            var sql = "";
            if (mtcNo == "")
            {
                sql = "select * from vw_preventivemaintenance_history " +
                    "where mtc_schedule between (SELECT TOP 1 case when format(mtcSpace,'MM') = format(mtc_schedule, 'MM') then mtcSpace else format(mtc_schedule, 'yyyy-MM-01') end FROM vw_preventivemaintenance_history WHERE format(mtc_schedule,'MM-yyyy') = '" + mtcSchedule.ToString("MM-yyyy") + "'  ORDER BY mtc_schedule DESC) " +
                    "and (select top 1 mtc_schedule from vw_preventivemaintenance_history WHERE format(mtc_schedule,'MM-yyyy') = '" + mtcSchedule.ToString("MM-yyyy") + "' ORDER BY mtc_schedule DESC) " +
                    "and mtcNo like '%" + mtcNo + "%' " +
                    "order by mtc_schedule desc, mtc_start desc, mtc_finish desc ";

            
            } else
            {
                sql = "select * from vw_preventivemaintenance_history " +
                    "where mtcSchedule = '" + mtcSchedule.ToString("MM-yyyy") + "' and mtcNo like '%" + mtcNo + "%' " +
                    "order by mtc_schedule desc, mtc_start desc, mtc_finish desc ";
            }
            
            var filter = db.Database.SqlQuery<vw_preventivemaintenance_history>(sql).ToList();

            ViewBag.sch = mtcSchedule.ToString("MM-yyyy");
            ViewBag.mtc = mtcNo;

            return View(filter.ToList());
        }
        
        // GET: MaintenancePreventiveHistory/Details/5
        public ActionResult Details(int? dept, int? mc, DateTime mtc, string block, string loom)
        {
            InitializeUserAkses();
            var access_button = db.ms_user_access_button.Where(x => x.userid == User.Identity.Name.ToString().Trim() && x.menu == "TRANSACTION" && x.submenu.ToString().Trim() == "Preventive Maintenance");
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

            if (dept == null || mc == null || mtc == null || block == null || loom == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBag.machineNo = machineNo(block, loom);
            //ViewBag.userName = userName();
            ViewBag.mtcAction = mtcAction();
            ViewBag.spareParts = spareParts();
            ViewBag.prevSpareParts = trPrevSpareParts(dept, mc, mtc, block, loom);

            var sql = "select * from vw_preventivemaintenance_history " +
                "where dept_id = " + dept + " and mc_id = " + mc + " " +
                "and format(mtc_schedule, 'yyyy-MM-dd') = '" + mtc.ToString("yyyy-MM-dd") + "' " +
                "and block = '" + block + "' and loom_no = '" + loom + "' ";
            var vw_preventivemaintenance_history = db.Database.SqlQuery<vw_preventivemaintenance_history>(sql).FirstOrDefault();

            if (vw_preventivemaintenance_history == null)
            {
                return HttpNotFound();
            }

            return View(vw_preventivemaintenance_history);
        }

        // GET: MaintenancePreventiveHistory/Create
        public ActionResult Create()
        {
            InitializeUserAkses();
            var access_button = db.ms_user_access_button.Where(x => x.userid == User.Identity.Name.ToString().Trim() && x.menu == "TRANSACTION" && x.submenu.ToString().Trim() == "Preventive Maintenance");
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


            string id = User.Identity.Name.ToString();
            var sql = "select * from sysuser_app " +
                "where departement = 'W' and id = '" + id + "' ";
            var userName = db.Database.SqlQuery<sysuser_app>(sql).FirstOrDefault().Fullname;

            ViewBag.machineNo = machineNo("","");
            ViewBag.userName = userName;
            ViewBag.mtcAction = mtcAction();
            ViewBag.spareParts = spareParts();

            return View();
        }

        // POST: MaintenancePreventiveHistory/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "mtcSchedule, mtcNo, startMtc, finishMtc, mtcDuration, dept_name, mc_name, mtc_name, mtc_action_name, Fullname, mtc_schedule, block, loom_no, mtc_start, mtc_finish, dept_id, mc_id, mtc_id, mtc_action_id, pic_mtc, proc_no")] vw_preventivemaintenance_history prev, FormCollection spare)
        {
            tr_prev_maintenance main = new tr_prev_maintenance();

            if (ModelState.IsValid)
            {

                using (DbContextTransaction transaction = db.Database.BeginTransaction())
                {
                    try
                    {

                        var sql = "select * from ms_dept where dept_name = '" + prev.dept_name + "' ";
                        var dept = db.Database.SqlQuery<ms_dept>(sql).FirstOrDefault();

                        sql = "select a.* from ms_machine_type a " +
                            "left join ms_dept b on a.dept_id = b.dept_id " +
                            "where b.dept_name = '" + prev.dept_name + "' and  a.mc_name = '" + prev.mc_name + "' ";
                        var mc = db.Database.SqlQuery<ms_machine_type>(sql).FirstOrDefault();

                        sql = "select * from machine_master where block + '.' + loom_no = '" + prev.mtcNo + "' ";
                        var mach = db.Database.SqlQuery<machine_master>(sql).FirstOrDefault();

                        //sql = "select * from sysuser_app where fullname = '" + prev.Fullname + "' ";
                        //var pic = db.Database.SqlQuery<sysuser_app>(sql).FirstOrDefault();

                        var mtcId = "MTC30";
                        sql = "select * from ms_maintenance_action where mtc_id = '" + mtcId + "' and mtc_action_name = '" + prev.mtc_action_name + "' ";
                        var action = db.Database.SqlQuery<ms_maintenance_action>(sql).FirstOrDefault();

                        //########################## (start) set to prev_maintenance #######################//

                        main.dept_id = dept.dept_id;
                        main.mc_id = mc.mc_id;

                        //DateTime mtcSchedule = DateTime.Parse(spare["mtc_schedule"] + "-" + DateTime.Now.ToString("dd"), CultureInfo.CurrentCulture);
                        //main.mtc_schedule = mtcSchedule;  //prev.mtc_schedule;

                        string[] scheduleDate = spare["scheduleDate"].Split('-');
                        string modSchedule = scheduleDate[1] + "-" + scheduleDate[0] + "-" + DateTime.Now.ToString("dd");
                        DateTime mtcSchedule = DateTime.Parse(modSchedule, CultureInfo.CurrentCulture);
                        main.mtc_schedule = mtcSchedule;  //prev.mtc_schedule;

                        main.block = mach.block;
                        main.loom_no = mach.loom_no;

                        //main.mtc_start = prev.mtc_start;
                        //main.mtc_finish = prev.mtc_finish;

                        string[] startSplit = spare["mtcStart"].Split(' ');
                        string[] startDate = startSplit[0].Split('-');
                        string[] startTime = startSplit[1].Split(':');

                        string[] finishSplit = spare["mtcFinish"].Split(' ');
                        string[] finishDate = finishSplit[0].Split('-');
                        string[] finishTime = finishSplit[1].Split(':');

                        string modStart = startDate[2] + "-" + startDate[1] + "-" + startDate[0] + " " + startTime[0] + ":" + startTime[1];
                        string modFinsih = finishDate[2] + "-" + finishDate[1] + "-" + finishDate[0] + " " + finishTime[0] + ":" + finishTime[1];

                        DateTime mtcStart = DateTime.Parse(modStart, CultureInfo.CurrentCulture);
                        DateTime mtcFinish = DateTime.Parse(modFinsih, CultureInfo.CurrentCulture);

                        main.mtc_start = mtcStart;
                        main.mtc_finish = mtcFinish;

                        main.mtc_id = mtcId;
                        main.mtc_action_id = action.mtc_action_id;
                        main.pic_mtc = User.Identity.Name;

                        main.user_id = User.Identity.Name;
                        main.rec_sts = "A";

                        sql = "select isnull(max(proc_no),0) from tr_prev_maintenance " +
                            "where dept_id = " + dept.dept_id + " and mc_id = " + mc.mc_id + " " +
                            "and format(mtc_schedule, 'yyyy-MM-dd') = '" + mtcSchedule.ToString("yyyy-MM-dd") + "' " +
                            "and block = '" + mach.block + "' and loom_no = '" + mach.loom_no + "' ";
                        var procNo = db.Database.SqlQuery<procNo>(sql).FirstOrDefault();

                        main.proc_no = procNo.proc_no + 1;
                        main.client_ip = GetIPAddress();
                        main.proc_time = DateTime.Now;

                        //########################## (finish) set to prev_maintenance #######################//

                        //########################## (start) set to tr_prev_maintenance_sparepart #######################//

                        var i = 1;
                        foreach (var item in spare.AllKeys.Where(a => a.Contains("Detail_2[" + i + "]")))
                        {

                            //########################## (start) save to tr_prev_maintenance_sparepart #######################//

                            tr_prev_maintenance_sparepart part = new tr_prev_maintenance_sparepart();

                            //var itemName = spare["Detail_2[" + i + "].item_name"];
                            //sql = "select * from [wppc].v_SAP_OITM where trim(ItemName) = trim('" + itemName + "') ";
                            var itemCode = spare["Detail_2[" + i + "].item_name"];  //db.Database.SqlQuery<v_SAP_OITM>(sql).FirstOrDefault().ItemCode;

                            var price = decimal.Parse(spare["Detail_2[" + i + "].price"], CultureInfo.InvariantCulture);
                            var quantity = decimal.Parse(spare["Detail_2[" + i + "].quantity"], CultureInfo.InvariantCulture);

                            part.dept_id = dept.dept_id;
                            part.mc_id = mc.mc_id;
                            part.mtc_schedule = mtcSchedule; // prev.mtc_schedule;
                            part.block = mach.block;
                            part.loom_no = mach.loom_no;

                            part.item_code = itemCode;
                            part.price = price;
                            part.quantity = quantity;

                            part.proc_time = DateTime.Now;

                            db.tr_prev_maintenance_sparepart.Add(part);
                            db.SaveChanges();

                            //########################## (finish) save to tr_prev_maintenance_sparepart #######################//

                            i = i + 1;
                        }

                        //########################## (finish) set to tr_prev_maintenance_sparepart #######################//

                        //########################## (start) save to prev_maintenance #######################//

                        db.tr_prev_maintenance.Add(main);
                        db.SaveChanges();

                        //########################## (finish) save to prev_maintenance #######################//

                        transaction.Commit();

                        return RedirectToAction("Index");
                    }
                    catch (Exception ex)
                    {

                        transaction.Rollback();
                        ViewBag.error = ex.Message;
                    }
                }
            }

            return View(prev);
        }

        // GET: MaintenancePreventiveHistory/Edit/5
        public ActionResult Edit(int? dept, int? mc, DateTime mtc, string block, string loom)
        {
            InitializeUserAkses();

            var access_button = db.ms_user_access_button.Where(x => x.userid == User.Identity.Name.ToString().Trim() && x.menu == "TRANSACTION" && x.submenu.ToString().Trim() == "Preventive Maintenance");
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


            if (dept == null || mc == null || mtc == null || block == null || loom == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBag.machineNo = machineNo(block, loom);
            //ViewBag.userName = userName();
            ViewBag.mtcAction = mtcAction();
            ViewBag.spareParts = spareParts();
            ViewBag.prevSpareParts = trPrevSpareParts(dept, mc, mtc, block, loom);

            var sql = "select * from vw_preventivemaintenance_history " +
                "where dept_id = " + dept + " and mc_id = " + mc + " " +
                "and format(mtc_schedule, 'yyyy-MM-dd') = '" + mtc.ToString("yyyy-MM-dd") + "' " +
                "and block = '" + block + "' and loom_no = '" + loom + "' ";
            var vw_preventivemaintenance_history = db.Database.SqlQuery<vw_preventivemaintenance_history>(sql).FirstOrDefault();

            if (vw_preventivemaintenance_history == null)
            {
                return HttpNotFound();
            }

            return View(vw_preventivemaintenance_history);
        }

        // POST: MaintenancePreventiveHistory/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "mtcSchedule,mtcNo,startMtc,finishMtc,mtcDuration,dept_name,mc_name,mtc_name,mtc_action_name,Fullname,mtc_schedule,block,loom_no,mtc_start,mtc_finish,dept_id,mc_id,mtc_id,mtc_action_id,pic_mtc,proc_no")] vw_preventivemaintenance_history prev, FormCollection spare)
        {

            tr_prev_maintenance main = new tr_prev_maintenance();

            if (ModelState.IsValid)
            {
                using (DbContextTransaction transaction = db.Database.BeginTransaction())
                {
                    try
                    {

                        var sql = "select * from ms_dept where dept_name = '" + prev.dept_name + "' ";
                        var dept = db.Database.SqlQuery<ms_dept>(sql).FirstOrDefault();

                        sql = "select a.* from ms_machine_type a " +
                            "left join ms_dept b on a.dept_id = b.dept_id " +
                            "where b.dept_name = '" + prev.dept_name + "' and  a.mc_name = '" + prev.mc_name + "' ";
                        var mc = db.Database.SqlQuery<ms_machine_type>(sql).FirstOrDefault();

                        sql = "select * from machine_master where block + '.' + loom_no = '" + prev.mtcNo + "' ";
                        var mach = db.Database.SqlQuery<machine_master>(sql).FirstOrDefault();

                        //sql = "select * from sysuser_app where fullname = '" + prev.Fullname + "' ";
                        //var pic = db.Database.SqlQuery<sysuser_app>(sql).FirstOrDefault();

                        var mtcId = "MTC30";
                        sql = "select * from ms_maintenance_action where mtc_id = '" + mtcId + "' and mtc_action_name = '" + prev.mtc_action_name + "' ";
                        var action = db.Database.SqlQuery<ms_maintenance_action>(sql).FirstOrDefault();

                        //########################## (start) set to prev_maintenance #######################//

                        main.dept_id = dept.dept_id;
                        main.mc_id = mc.mc_id;

                        //string[] ms = spare["scheduleDate"].Split(',');
                        //string[] scheduleDate = ms[1].Split('-');
                        //string modSchedule = scheduleDate[1] + "-" + scheduleDate[0] + "-01";
                        //DateTime mtcSchedule = DateTime.Parse(modSchedule, CultureInfo.CurrentCulture);

                        DateTime mtcSchedule = prev.mtc_schedule;
                        main.mtc_schedule = mtcSchedule;  //prev.mtc_schedule;

                        main.block = mach.block;
                        main.loom_no = mach.loom_no;

                        //main.mtc_start = prev.mtc_start;
                        //main.mtc_finish = prev.mtc_finish;

                        string[] startSplit = spare["mtcStart"].Split(' ');
                        string[] startDate = startSplit[0].Split('-');
                        string[] startTime = startSplit[1].Split(':');

                        string[] finishSplit = spare["mtcFinish"].Split(' ');
                        string[] finishDate = finishSplit[0].Split('-');
                        string[] finishTime = finishSplit[1].Split(':');

                        string modStart = startDate[2] + "-" + startDate[1] + "-" + startDate[0] + " " + startTime[0] + ":" + startTime[1];
                        string modFinsih = finishDate[2] + "-" + finishDate[1] + "-" + finishDate[0] + " " + finishTime[0] + ":" + finishTime[1];

                        DateTime mtcStart = DateTime.Parse(modStart, CultureInfo.CurrentCulture);
                        DateTime mtcFinish = DateTime.Parse(modFinsih, CultureInfo.CurrentCulture);

                        main.mtc_start = mtcStart;
                        main.mtc_finish = mtcFinish;

                        main.mtc_id = mtcId;
                        main.mtc_action_id = action.mtc_action_id;
                        main.pic_mtc = User.Identity.Name;

                        main.user_id = User.Identity.Name;
                        main.rec_sts = "T";
                        main.proc_no = prev.proc_no + 1;
                        main.client_ip = GetIPAddress();
                        main.proc_time = DateTime.Now;

                        //########################## (finish) set to prev_maintenance #######################//

                        //########################## (start) set to tr_prev_maintenance_sparepart #######################//

                        var i = 1;
                        foreach (var item in spare.AllKeys.Where(a => a.Contains("Detail_2[" + i + "]")))
                        {

                            //var itemName = spare["Detail_2[" + i + "].item_name"];
                            //sql = "select * from [wppc].v_SAP_OITM where trim(ItemName) = trim('" + itemName + "') ";
                            var itemCode = spare["Detail_2[" + i + "].item_name"]; //db.Database.SqlQuery<v_SAP_OITM>(sql).FirstOrDefault().ItemCode;

                            var price = decimal.Parse(spare["Detail_2[" + i + "].price"], CultureInfo.InvariantCulture);
                            var quantity = decimal.Parse(spare["Detail_2[" + i + "].quantity"], CultureInfo.InvariantCulture);

                            tr_prev_maintenance_sparepart part = new tr_prev_maintenance_sparepart();

                            part.dept_id = dept.dept_id;
                            part.mc_id = mc.mc_id;
                            part.mtc_schedule = mtcSchedule; // prev.mtc_schedule;
                            part.block = mach.block;
                            part.loom_no = mach.loom_no;

                            part.item_code = itemCode;
                            part.price = price;
                            part.quantity = quantity;

                            part.proc_time = DateTime.Now;

                            //######################### validate #########################

                            sql = "select * from vw_preventivemaintenance_sparepart " +
                                "where dept_id = " + dept.dept_id + " and mc_id = " + mc.mc_id + " " +
                                "and format(mtc_schedule, 'yyyy-MM-dd') = '" + mtcSchedule.ToString("yyyy-MM-dd") + "' " +
                                "and block = '" + mach.block + "' and loom_no = '" + mach.loom_no + "' " +
                                "and item_code = '" + itemCode + "' ";
                            var dupt = db.Database.SqlQuery<vw_preventivemaintenance_sparepart>(sql).FirstOrDefault();
                            if (dupt == null)
                            {

                                //########################## (start) save to tr_prev_maintenance_sparepart #######################//

                                db.tr_prev_maintenance_sparepart.Add(part);
                                db.SaveChanges();

                                //########################## (finish) save to tr_prev_maintenance_sparepart #######################//

                            }
                            else
                            {

                                //########################## (start) edit to tr_prev_maintenance_sparepart_hist #######################//

                                sql = "select top 1 quantity from tr_prev_maintenance_sparepart_hist " +
                                    "where dept_id = " + dept.dept_id + " and mc_id = " + mc.mc_id + " " +
                                    "and format(mtc_schedule, 'yyyy-MM-dd') = '" + mtcSchedule.ToString("yyyy-MM-dd") + "' " +
                                    "and block = '" + mach.block + "' and loom_no = '" + mach.loom_no + "' " +
                                    "and item_code = '" + itemCode + "' " +
                                    "order by proc_time desc ";
                                var qty = db.Database.SqlQuery<sparePartHist>(sql).FirstOrDefault();

                                if (qty == null)
                                {

                                    sql = "insert into tr_prev_maintenance_sparepart_hist " +
                                        "select * from tr_prev_maintenance_sparepart " +
                                        "where dept_id = " + dept.dept_id + " and mc_id = " + mc.mc_id + " " +
                                        "and format(mtc_schedule, 'yyyy-MM-dd') = '" + mtcSchedule.ToString("yyyy-MM-dd") + "' " +
                                        "and block = '" + mach.block + "' and loom_no = '" + mach.loom_no + "' " +
                                        "and item_code = '" + itemCode + "' ";
                                    db.Database.ExecuteSqlCommand(sql);

                                }

                                if (qty != null)
                                {

                                    if (qty.quantity != quantity)
                                    {

                                        sql = "insert into tr_prev_maintenance_sparepart_hist " +
                                            "select * from tr_prev_maintenance_sparepart " +
                                            "where dept_id = " + dept.dept_id + " and mc_id = " + mc.mc_id + " " +
                                            "and format(mtc_schedule, 'yyyy-MM-dd') = '" + mtcSchedule.ToString("yyyy-MM-dd") + "' " +
                                            "and block = '" + mach.block + "' and loom_no = '" + mach.loom_no + "' " +
                                            "and item_code = '" + itemCode + "' ";
                                        db.Database.ExecuteSqlCommand(sql);

                                    }
                                } 

                                //########################## (finish) edit to tr_prev_maintenance_sparepart_hist #######################//


                                //########################## (start) edit to tr_prev_maintenance_sparepart #######################//

                                db.Entry(part).State = EntityState.Modified;
                                db.SaveChanges();

                                //########################## (finish) edit to tr_prev_maintenance_sparepart #######################//

                            }

                            i = i + 1;
                        }

                        //########################## (finish) set to tr_prev_maintenance_sparepart #######################//


                        //########################## (start) edit to prev_maintenance_hist #######################//

                        sql = "insert into tr_prev_maintenance_hist " +
                            "select * from tr_prev_maintenance " +
                            "where dept_id = " + dept.dept_id + " and mc_id = " + mc.mc_id + " " +
                            "and format(mtc_schedule, 'yyyy-MM-dd') = '" + mtcSchedule.ToString("yyyy-MM-dd") + "' " +
                            "and block = '" + mach.block + "' and loom_no = '" + mach.loom_no + "' ";
                        db.Database.ExecuteSqlCommand(sql);

                        //########################## (finish) edit to prev_maintenance_hist #######################//


                        //########################## (start) edit to prev_maintenance #######################//

                        db.Entry(main).State = EntityState.Modified;
                        db.SaveChanges();

                        //########################## (finish) edit to prev_maintenance #######################//

                        transaction.Commit();

                        return RedirectToAction("Index");
                    }
                    catch (Exception ex)
                    {

                        transaction.Rollback();
                        ViewBag.error = ex.Message;
                    }
                }
            }

            return View(prev);
        }

        // GET: MaintenancePreventiveHistory/Delete/5
        public ActionResult Delete(int? dept, int? mc, DateTime mtc, string block, string loom)
        {
            InitializeUserAkses();
            var access_button = db.ms_user_access_button.Where(x => x.userid == User.Identity.Name.ToString().Trim() && x.menu == "TRANSACTION" && x.submenu.ToString().Trim() == "Preventive Maintenance");
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

            if (dept == null || mc == null || mtc == null || block == null || loom == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBag.machineNo = machineNo(block, loom);
            //ViewBag.userName = userName();
            ViewBag.mtcAction = mtcAction();
            ViewBag.spareParts = spareParts();
            ViewBag.prevSpareParts = trPrevSpareParts(dept, mc, mtc, block, loom);

            var sql = "select * from vw_preventivemaintenance_history " +
                "where dept_id = " + dept + " and mc_id = " + mc + " " +
                "and format(mtc_schedule, 'yyyy-MM-dd') = '" + mtc.ToString("yyyy-MM-dd") + "' " +
                "and block = '" + block + "' and loom_no = '" + loom + "' ";
            var vw_preventivemaintenance_history = db.Database.SqlQuery<vw_preventivemaintenance_history>(sql).FirstOrDefault();

            if (vw_preventivemaintenance_history == null)
            {
                return HttpNotFound();
            }

            return View(vw_preventivemaintenance_history);
        }

        // POST: MaintenancePreventiveHistory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? dept, int? mc, DateTime mtc, string block, string loom)
        {
            

            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {

                    //########################## (start) delete to prev_maintenance_sparepart #######################//

                    var sql = "";
                    sql = "insert into tr_prev_maintenance_sparepart_hist " +
                        "select * from tr_prev_maintenance_sparepart " +
                        "where dept_id = " + dept + " and mc_id = " + mc + " " +
                        "and format(mtc_schedule, 'yyyy-MM-dd') = '" + mtc.ToString("yyyy-MM-dd") + "' " +
                        "and block = '" + block + "' and loom_no = '" + loom + "' ";
                    db.Database.ExecuteSqlCommand(sql);

                    sql = "delete from tr_prev_maintenance_sparepart " +
                        "where dept_id = " + dept + " and mc_id = " + mc + " " +
                        "and format(mtc_schedule, 'yyyy-MM-dd') = '" + mtc.ToString("yyyy-MM-dd") + "' and block = '" + block + "' " +
                        "and loom_no = '" + loom + "' ";
                    db.Database.ExecuteSqlCommand(sql);

                    //########################## (finish) delete to prev_maintenance_sparepart #######################//

                    //########################## (start) delete to prev_maintenance #######################//

                    sql = "insert into tr_prev_maintenance_hist " +
                        "select * from tr_prev_maintenance " +
                        "where dept_id = " + dept + " and mc_id = " + mc + " " +
                        "and format(mtc_schedule, 'yyyy-MM-dd') = '" + mtc.ToString("yyyy-MM-dd") + "' " +
                        "and block = '" + block + "' and loom_no = '" + loom + "' ";
                    db.Database.ExecuteSqlCommand(sql);

                    sql = "delete from tr_prev_maintenance " +
                        "where dept_id = " + dept + " and mc_id = " + mc + " " +
                        "and format(mtc_schedule, 'yyyy-MM-dd') = '" + mtc.ToString("yyyy-MM-dd") + "' and block = '" + block + "' " +
                        "and loom_no = '" + loom + "' ";
                    db.Database.ExecuteSqlCommand(sql);

                    //########################## (finish) delete to prev_maintenance #######################//

                    transaction.Commit();

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {

                    transaction.Rollback();
                    ViewBag.error = ex.Message;
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

        [HttpGet]
        [AllowAnonymous]
        public ActionResult getUnitPrice(string itemName)
        {
            var sql = "SELECT * FROM [wppc].[v_SAP_OITM_PRICE] " +
                "WHERE ItemName = '" + itemName + "' ";
            var rates = db.Database.SqlQuery<v_SAP_OITM_PRICE>(sql).FirstOrDefault();
            var settings = new JsonSerializerSettings() { ContractResolver = null };
            var a = JsonConvert.SerializeObject(rates, settings);
            return Content(a, "application/json");
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult delDetail(string itemCode,
            int? dept, int? mc, string mtc, string block, string loom)
        {

            var sql = "";
            sql = "insert into tr_prev_maintenance_sparepart_hist " +
                "select * from tr_prev_maintenance_sparepart " +
                "where dept_id = " + dept + " " +
                "and mc_id = " + mc + " " +
                "and format(mtc_schedule, 'yyyy-MM-dd') = '" + mtc + "' " +
                "and block = '" + block + "' " +
                "and loom_no = '" + loom + "' " +
                "and item_code = '" + itemCode + "' ";
            var hist = db.Database.ExecuteSqlCommand(sql);

            sql = "delete from tr_prev_maintenance_sparepart " +
                "where dept_id = " + dept + " " +
                "and mc_id = " + mc + " " +
                "and format(mtc_schedule, 'yyyy-MM-dd') = '" + mtc + "' " +
                "and block = '" + block + "' " +
                "and loom_no = '" + loom + "' " +
                "and item_code = '" + itemCode + "' ";
            var del = db.Database.ExecuteSqlCommand(sql);

            var settings = new JsonSerializerSettings() { ContractResolver = null };
            var a = JsonConvert.SerializeObject(del, settings);
            return Content(a, "application/json");
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult getDataMachine(string sch)
        {

            var sql = "select block + '.' + loom_no as mtc from machine_master " +
                "where loom_type = 'AJL' and rec_sts = 'A' and block + '.' + loom_no not in ( " +
                "select x.block + '.' + x.loom_no from tr_prev_maintenance x where format(x.mtc_schedule, 'MM-yyyy') = '" + sch + "' " +
                ")" +
                "order by block asc, loom_no asc ";
            var datas = db.Database.SqlQuery<machine>(sql).ToList();

            var settings = new JsonSerializerSettings() { ContractResolver = null };
            var a = JsonConvert.SerializeObject(datas, settings);
            return Content(a, "application/json");
        }
    }
}
