using ISM_MAINTENANCE.Models.DB;
using System.Web.Mvc;
using ISM_MAINTENANCE.Models.ViewModel;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Data.Entity;
using Helper;

namespace ISM_MAINTENANCE.Controllers.Maintenance
{
    [Authorize]
    public class MaintenanceHistoryController : Controller
    {
        private WvMaintenanceEntities db = new WvMaintenanceEntities();
        ButtonAccess _button = new ButtonAccess();
        Nullvalue CheckNullStr = new Nullvalue();

        private void InitializeUserAkses()
        {
            if (db.v_user_ism_maintenance.Where(x => x.ID == User.Identity.Name.ToString().Trim()).FirstOrDefault() == null)
            {
                ViewBag.Section = ""; ViewBag.Roles = ""; ViewBag.UserAksesMenu = "";
            }
            else
            {

                ViewBag.UserAksesMenu = db.ms_user_access_menu.Where(x => x.userid == User.Identity.Name.ToString().Trim()).AsNoTracking().ToList();
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

        private void initEditForm(MaintenanceHistoryHeader FrmData)
        {
            var get_roles = db.v_user_ism_maintenance.Where(x => x.ID == User.Identity.Name.ToUpper().Trim());
            string roles = "";

            if (get_roles != null)
            {
                roles = get_roles.FirstOrDefault().roles.Trim();
            }

            if (roles == "ADMIN")
            {
                ViewBag.pic_id = new SelectList(db.sysuser_app.Where(x => x.ID == User.Identity.Name.ToUpper().Trim()).OrderBy(x => x.Fullname), "ID", "Fullname", User.Identity.Name.ToUpper());
            }
            else
            {
                ViewBag.pic_id = new SelectList(db.sysuser_app.Where(x => x.departement == "W" && x.section == "MT" & x.ID == User.Identity.Name.ToUpper()).OrderBy(x => x.Fullname), "ID", "Fullname", User.Identity.Name.ToUpper());
            }

            var temp1 = from xx in db.v_MaintenanceActionShiftLeaderNotes
                        where xx.dept_id == FrmData.dept_id && xx.mc_id == FrmData.mc_id && xx.note_date == FrmData.note_date && xx.shift == FrmData.shift && xx.block == FrmData.block && xx.loom_no == FrmData.loom
                        select new MtcAction_Shift()
                        {
                            mtc_id = xx.mtc_id,
                            trouble_id = xx.trouble_id,
                            mtc_action_id = xx.mtc_action_id,
                            mtc_action_name = xx.mtc_action_name,
                            trouble_name = xx.trouble_name,
                            mtc_type = xx.mtc_name,
                            loom = xx.loom_no,
                            block = xx.block,
                            note_date = xx.note_date,
                            shift = xx.shift
                        };

            ViewBag.Detil_mtc_action = temp1.ToList();
            string query = "select ItemCode COLLATE DATABASE_DEFAULT as ItemCode, ItemName COLLATE DATABASE_DEFAULT as ItemName, PRICE  from wppc.v_SAP_OITM_PRICE";
            var result = db.Database.SqlQuery<SAPITEM>(query);
            ViewBag.SapItemWithPrice = result.ToList();
            ViewBag.SapItem = new SelectList(result.ToList(), "ItemCode", "ItemName");
        }

        private void initform()
        {
            string dept_code_cim = db.sysuser_app.Where(x => x.ID == User.Identity.Name).Select(x => x.departement).FirstOrDefault();
            string dept_name_cim = db.departement_master.Where(x => x.Dept_ID == dept_code_cim).Select(x => x.Dept_Desc).FirstOrDefault().ToUpper();
            decimal dept_id_system = db.ms_dept.Where(x => x.dept_name == dept_name_cim).Select(x => x.dept_id).FirstOrDefault();

            var temp = from data in db.v_Shift_Leader_Header
                       where data.dept_id == dept_id_system && data.stop_to == null
                       orderby data.note_date descending
                       select new LookupShiftLeaderNotes()
                       {
                           dept_id = data.dept_id,
                           mc_id = data.mc_id,
                           block = data.block,
                           loom_no = data.loom_no,
                           note_date = data.note_date,
                           stop_from = data.stop_from,
                           shift = data.shift,
                           machine_no = data.block + "." + data.loom_no
                       };

            List<LookupShiftLeaderNotes> obj = new List<LookupShiftLeaderNotes>();
            foreach (var item in temp)
            {
                tr_maintenance mtc = db.tr_maintenance.Find(item.dept_id, item.mc_id, item.note_date, item.shift, item.block, item.loom_no);
                if (mtc == null)
                {
                    obj.Add(item);
                }
            }

            //ViewBag.ShiftLeaderNotes = temp.ToList();
            ViewBag.ShiftLeaderNotes = obj.OrderByDescending(x => x.LamaStopNumerik);

            var temp1 = from data in db.v_MaintenanceActionforModal
                            //where data.mtc_id != "MTC30"
                        select new MtcAction_Shift()
                        {
                            mtc_id = data.mtc_id,
                            trouble_id = data.trouble_id,
                            mtc_action_id = data.mtc_action_id,
                            mtc_action_name = data.mtc_action_name,
                            trouble_name = data.trouble_name,
                            mtc_type = data.mtc_name,
                            loom = data.loom_no,
                            block = data.block,
                            note_date = data.note_date,
                            shift = data.shift
                        };
            ViewBag.MaintenanceAction = temp1.ToList();

            string query = "SELECT '0' AS ItemCode, '0' as ItemName, 0 as PRICE union select ItemCode COLLATE DATABASE_DEFAULT as ItemCode, ItemName COLLATE DATABASE_DEFAULT as ItemName, PRICE  from wppc.v_SAP_OITM_PRICE where price > 0 order by ItemName";
            var result = db.Database.SqlQuery<SAPITEM>(query);
            ViewBag.SapItemWithPrice = result.ToList();
            ViewBag.SapItem = new SelectList(result.ToList(), "ItemCode", "ItemName");//result.ToList();
            ViewBag.dept_id = new SelectList(db.ms_dept.Where(x => x.dept_id == dept_id_system), "dept_id", "dept_name", dept_id_system);
            ViewBag.mc_id = new SelectList(db.ms_machine_type.Where(x => x.dept_id == dept_id_system && x.mc_id == 70), "mc_id", "mc_name", 70);

            var get_roles = db.v_user_ism_maintenance.Where(x => x.ID == User.Identity.Name.ToUpper().Trim());
            string roles = "";

            if (get_roles != null)
            {
                roles = get_roles.FirstOrDefault().roles.Trim();
            }

            if (roles == "ADMIN")
            {
                ViewBag.pic_id = new SelectList(db.sysuser_app.Where(x => x.ID == User.Identity.Name.ToUpper().Trim()).OrderBy(x => x.Fullname), "ID", "Fullname", User.Identity.Name.ToUpper());
            }
            else
            {
                ViewBag.pic_id = new SelectList(db.sysuser_app.Where(x => x.departement == "W" && x.section == "mt" && x.ID == User.Identity.Name.ToUpper()).OrderBy(x => x.Fullname), "ID", "Fullname", User.Identity.Name.ToUpper());
            }

        }

        // GET: MaintenanceHistory
        public ActionResult Index(string _block, string id)
        {
            InitializeUserAkses();

            string dept_code_cim = db.sysuser_app.Where(x => x.ID == User.Identity.Name).Select(x => x.departement).FirstOrDefault();
            string dept_name_cim = db.departement_master.Where(x => x.Dept_ID == dept_code_cim).Select(x => x.Dept_Desc).FirstOrDefault().ToUpper();
            decimal dept_id_system = db.ms_dept.Where(x => x.dept_name == dept_name_cim).Select(x => x.dept_id).FirstOrDefault();
            DateTime ThreeDaysBefore = DateTime.Now.AddDays(-3);
            DateTime Today = DateTime.Now;

            ////Testing
            //DateTime ThreeDaysBefore = DateTime.Now.AddDays(4).AddDays(-3);
            //DateTime Today = DateTime.Now.AddDays(4);

            if (id != null)
            {
                var par = id.Split(';');
                var tgl = par[2].ToString().Split('-');
                var _note_date = new DateTime(Convert.ToInt16(tgl[2]), Convert.ToInt16(tgl[1]), Convert.ToInt16(tgl[0]));
                var _dept_id = Convert.ToDecimal(par[0]);
                var _mc_id = Convert.ToDecimal(par[1]);
                var _shift = par[3];
                var block = par[4];
                var _loom = par[5];

                //&& obj.mtc_start >= ThreeDaysBefore && obj.mtc_start <= Today
                IQueryable<MaintenanceHistory> data = from obj in db.v_MaintenanceHistory
                                                      where obj.dept_id == _dept_id && obj.mc_id == _mc_id && obj.note_date == _note_date && obj.shift == _shift && obj.block == block && obj.loom_no == _loom
                                                    && obj.mtc_start >= ThreeDaysBefore && obj.mtc_start <= Today
                                                      orderby obj.mtc_start descending
                                                      select new MaintenanceHistory()
                                                      {
                                                          dept_id = obj.dept_id,
                                                          mc_id = obj.mc_id,
                                                          note_date = obj.note_date,
                                                          shift = obj.shift,
                                                          block = obj.block,
                                                          loom_no = obj.loom_no,
                                                          machine_no = obj.block + "." + obj.loom_no,
                                                          dept_name = obj.dept_name,
                                                          mtc_start = obj.mtc_start,
                                                          mtc_finish = (DateTime)obj.mtc_finish,
                                                          pic_mtc = obj.pic_mtc,
                                                          pic_mtc_name = obj.Fullname,
                                                          finish_sts = obj.finish_sts,
                                                          mtc_name = obj.mtc_name
                                                      };
                return View(data.AsNoTracking().ToList());

            }
            else
            {
                IQueryable<MaintenanceHistory> data = from obj in db.v_MaintenanceHistory
                                                      where obj.dept_id == dept_id_system && obj.mc_id == 70 &&
                                                           (_block == "All" ? obj.block.Equals("A") || obj.block.Equals("B") || obj.block.Equals("C") || obj.block.Equals("D") : obj.block.Equals(_block))
                                                             && obj.mtc_start >= ThreeDaysBefore && obj.mtc_start <= Today
                                                      orderby obj.mtc_start descending
                                                      select new MaintenanceHistory()
                                                      {
                                                          dept_id = obj.dept_id,
                                                          mc_id = obj.mc_id,
                                                          note_date = obj.note_date,
                                                          shift = obj.shift,
                                                          block = obj.block,
                                                          loom_no = obj.loom_no,
                                                          machine_no = obj.block + "." + obj.loom_no,
                                                          dept_name = obj.dept_name,
                                                          mtc_start = obj.mtc_start,
                                                          mtc_finish = (DateTime)obj.mtc_finish,
                                                          pic_mtc = obj.pic_mtc,
                                                          pic_mtc_name = obj.Fullname,
                                                          finish_sts = obj.finish_sts,
                                                          mtc_name = obj.mtc_name
                                                      };

                return View(data.AsNoTracking().ToList());
            }

        }

        // GET: MaintenanceHistory/Create
        public ActionResult Create()
        {
            InitializeUserAkses();
            var access_button = db.ms_user_access_button.Where(x => x.userid == User.Identity.Name.ToString().Trim() && x.menu == "TRANSACTION" && x.submenu.ToString().Trim() == "Machine Quality Trouble/History");
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


            initform();
            MaintenanceHistoryHeader obj = new MaintenanceHistoryHeader();

            return View(obj);
        }

        // POST: MaintenanceHistory/Create
        [HttpPost]
        public ActionResult Create([Bind(Include = "dept_id, mc_id, note_date, shift, pic_id, block, loom, machine_no, mtc_start_str, mtc_stop_str, stop_from_str, TanggalString, finish_sts, Detail_1, Detail_2")] MaintenanceHistoryHeader FrmData)
        {
            InitializeUserAkses();

            Nullable<System.DateTime> _tanggal = null;
            string[] _mc_number;

            var mtc_start = DateTime.Now;
            var mtc_finish = DateTime.Now;

            if (ModelState.IsValid)
            {
                try
                {
                    string[] mtc_start_input = FrmData.mtc_start_str.Trim().Split(' ');
                    string[] date_mtc_start = mtc_start_input[0].Split('-');
                    string[] time_mtc_start = mtc_start_input[1].Split(':');
                    mtc_start = new DateTime(Convert.ToInt16(date_mtc_start[2]), Convert.ToInt16(date_mtc_start[1]), Convert.ToInt16(date_mtc_start[0]), Convert.ToInt16(time_mtc_start[0]), Convert.ToInt16(time_mtc_start[1]), 0);

                    if (FrmData.mtc_stop_str != null)
                    {
                        string[] mtc_finish_input = FrmData.mtc_stop_str.Trim().Split(' ');
                        string[] date_mtc_finish = mtc_finish_input[0].Split('-');
                        string[] time_mtc_finish = mtc_finish_input[1].Split(':');
                        mtc_finish = new DateTime(Convert.ToInt16(date_mtc_finish[2]), Convert.ToInt16(date_mtc_finish[1]), Convert.ToInt16(date_mtc_finish[0]), Convert.ToInt16(time_mtc_finish[0]), Convert.ToInt16(time_mtc_finish[1]), 0);

                    }

                }
                catch (Exception ExDatetime)
                {
                    ModelState.AddModelError("", "Datetime Issue - " + ExDatetime.ToString());
                    initform();
                    return View(FrmData);

                }
            }

            if (ModelState.IsValid)
            {
                _mc_number = FrmData.machine_no.ToString().Split('.');
                var _loom = _mc_number[1];
                var _block = _mc_number[0];

                tr_maintenance CheckTrM = db.tr_maintenance.Find(FrmData.dept_id, FrmData.mc_id, FrmData.note_date, FrmData.shift, _block.ToString().Trim(), _loom.ToString().Trim());
                if (CheckTrM != null)
                {
                    ModelState.AddModelError("", "the data already exists in the database, Choose another shift leader note or Use Edit button to update data !!");
                    initform();
                    return View();
                }

                var CheckTrM_Action = db.tr_maintenance_action.Where(x => x.dept_id == FrmData.dept_id && x.mc_id == FrmData.mc_id && x.note_date == FrmData.note_date && x.shift == FrmData.shift && x.block == _block.ToString().Trim() && x.loom_no == _loom.ToString().Trim());
                if (CheckTrM_Action.Count() != 0)
                {
                    ModelState.AddModelError("", "the data already exists in Maintenance Action");
                    initform();
                    return View();
                }

                var CheckTrM_Sparepart = db.tr_maintenance_sparepart.Where(x => x.dept_id == FrmData.dept_id && x.mc_id == FrmData.mc_id && x.note_date == FrmData.note_date && x.shift == FrmData.shift && x.block == _block.ToString().Trim() && x.loom_no == _loom.ToString().Trim());
                if (CheckTrM_Sparepart.Count() != 0)
                {
                    ModelState.AddModelError("", "the data already exists in Maintenance Sparepart");
                    initform();
                    return View();
                }

                var SplitStopFrom = FrmData.stop_from_str.Trim().Split(' ');
                string[] date_stop_from = SplitStopFrom[0].Split('-');
                string[] time_stop_from = SplitStopFrom[1].Split(':');
                DateTime _stop_from = new DateTime(Convert.ToInt16(date_stop_from[2]), Convert.ToInt16(date_stop_from[1]), Convert.ToInt16(date_stop_from[0]), Convert.ToInt16(time_stop_from[0]), Convert.ToInt16(time_stop_from[1]), 0);

                if (mtc_start < _stop_from)
                {
                    ModelState.AddModelError("mtc_start_str", "Tanggal mulai maintenance start tidak boleh kurang dari tanggal stop mesin");
                    initform();
                    return View();
                }

                if (mtc_finish < _stop_from)
                {
                    ModelState.AddModelError("mtc_stop_str", "Tanggal selesai maintenance tidak boleh kurang dari tanggal stop mesin");
                    initform();
                    return View();
                }

                using (DbContextTransaction transaction = db.Database.BeginTransaction())
                {
                    try
                    {

                        tr_maintenance InsertTrMaintenance = new tr_maintenance();
                        InsertTrMaintenance.dept_id = FrmData.dept_id;
                        InsertTrMaintenance.mc_id = FrmData.mc_id;
                        InsertTrMaintenance.note_date = FrmData.note_date;
                        InsertTrMaintenance.shift = FrmData.shift;
                        InsertTrMaintenance.block = _block.Trim();
                        InsertTrMaintenance.loom_no = _loom.Trim();
                        InsertTrMaintenance.mtc_start = mtc_start;//FrmData.mtc_start;
                        InsertTrMaintenance.mtc_finish = FrmData.mtc_stop_str == null || FrmData.mtc_stop_str == "" ? _tanggal : mtc_finish; //FrmData.mtc_stop == null ? _tanggal : FrmData.mtc_stop;
                        InsertTrMaintenance.pic_mtc = FrmData.pic_id;
                        InsertTrMaintenance.finish_sts = FrmData.finish_sts == null ? "" : FrmData.finish_sts;
                        InsertTrMaintenance.user_id = User.Identity.Name.ToUpper();
                        InsertTrMaintenance.rec_sts = "A";
                        InsertTrMaintenance.proc_no = 1;
                        InsertTrMaintenance.client_ip = GetIPAddress();
                        InsertTrMaintenance.proc_time = DateTime.Now;
                        db.tr_maintenance.Add(InsertTrMaintenance);
                        db.SaveChanges();

                        if (FrmData.finish_sts != null)
                        {
                            if (FrmData.finish_sts.ToString().Trim() == "F")
                            {
                                tr_shift_leader_note getData = db.tr_shift_leader_note.Find(FrmData.dept_id, FrmData.mc_id, FrmData.note_date, FrmData.shift, FrmData.block, FrmData.loom);
                                getData.stop_to = mtc_finish;//FrmData.mtc_stop;
                                getData.proc_no = getData.proc_no + 1;
                                getData.proc_time = DateTime.Now;
                                getData.rec_sts = "T";
                                getData.user_id = User.Identity.Name.ToUpper();
                                getData.client_ip = GetIPAddress();
                                db.Entry(getData).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                        }

                        foreach (var item in FrmData.Detail_1)
                        {
                            tr_maintenance_action InsertTrM_Action = new tr_maintenance_action();
                            InsertTrM_Action.dept_id = FrmData.dept_id;
                            InsertTrM_Action.mc_id = FrmData.mc_id;
                            InsertTrM_Action.note_date = FrmData.note_date;
                            InsertTrM_Action.shift = FrmData.shift;
                            InsertTrM_Action.block = _block.Trim();
                            InsertTrM_Action.loom_no = _loom.Trim();
                            InsertTrM_Action.mtc_id = item.mtc_id.Trim();
                            InsertTrM_Action.trouble_id = item.trouble_id.Trim();
                            InsertTrM_Action.mtc_action_id = item.mtc_action_id.Trim();
                            InsertTrM_Action.proc_time = DateTime.Now;
                            db.tr_maintenance_action.Add(InsertTrM_Action);
                            db.SaveChanges();
                        }

                        if (FrmData.Detail_2 != null)
                        {
                            foreach (var item in FrmData.Detail_2)
                            {
                                tr_maintenance_sparepart InsertTrM_Sparepart = new tr_maintenance_sparepart();
                                InsertTrM_Sparepart.dept_id = FrmData.dept_id;
                                InsertTrM_Sparepart.mc_id = FrmData.mc_id;
                                InsertTrM_Sparepart.note_date = FrmData.note_date;
                                InsertTrM_Sparepart.shift = FrmData.shift;
                                InsertTrM_Sparepart.block = _block.Trim();
                                InsertTrM_Sparepart.loom_no = _loom.Trim();
                                InsertTrM_Sparepart.item_code = item.item_name.Trim();
                                InsertTrM_Sparepart.price = item.price;
                                InsertTrM_Sparepart.quantity = item.quantity;
                                InsertTrM_Sparepart.proc_time = DateTime.Now;
                                db.tr_maintenance_sparepart.Add(InsertTrM_Sparepart);
                                db.SaveChanges();
                            }

                        }

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
                            throw new System.Exception("Rollback Issue - " + InnerEx.Message.ToString() + " " + Environment.NewLine);
                        }
                        ModelState.AddModelError("", "Failed Transaction because " + ex.Message.ToString());
                        initform();
                        return View();
                    }

                }

                return RedirectToAction("Index", new { id = FrmData.dept_id + ";" + FrmData.mc_id + ";" + FrmData.note_date.ToString("dd-MM-yyyy") + ";" + FrmData.shift + ";" + FrmData.block + ";" + FrmData.loom });
            }

            initform();
            return View(FrmData);

        }

        // GET: MaintenanceHistory/Show/5
        public ActionResult Show(string id)
        {
            InitializeUserAkses();
            var access_button = db.ms_user_access_button.Where(x => x.userid == User.Identity.Name.ToString().Trim() && x.menu == "TRANSACTION" && x.submenu.ToString().Trim() == "Machine Quality Trouble/History");
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


            decimal deptid, mcid;
            DateTime note_date;
            string shift, block, loom;

            if (id == null)
            {
                return RedirectToAction("index");
            }

            string[] par = id.Split(';');
            if (par.Length != 1)
            {
                deptid = Convert.ToDecimal(par[0]);
                mcid = Convert.ToDecimal(par[1]);
                note_date = Convert.ToDateTime(par[2].ToString().Replace("_", "-").ToString());
                shift = par[3].ToString();
                block = par[4].ToString();
                loom = par[5].ToString();
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            string dept_code_cim = db.sysuser_app.Where(x => x.ID == User.Identity.Name).Select(x => x.departement).FirstOrDefault();
            string dept_name_cim = db.departement_master.Where(x => x.Dept_ID == dept_code_cim).Select(x => x.Dept_Desc).FirstOrDefault().ToUpper();
            decimal dept_id_system = db.ms_dept.Where(x => x.dept_name == dept_name_cim).Select(x => x.dept_id).FirstOrDefault();

            IQueryable<MaintenanceHistoryHeader> data = from obj in db.v_MaintenanceHistory
                                                        where obj.dept_id == dept_id_system && obj.mc_id == mcid && obj.note_date == note_date && obj.shift == shift && obj.loom_no == loom && obj.block == block
                                                        select new MaintenanceHistoryHeader()
                                                        {
                                                            dept_id = obj.dept_id,
                                                            mc_id = obj.mc_id,
                                                            mc_type = obj.mc_name,
                                                            note_date = obj.note_date,
                                                            shift = obj.shift,
                                                            block = obj.block,
                                                            loom = obj.loom_no,
                                                            machine_no = obj.block + "." + obj.loom_no,
                                                            dept_name = obj.dept_name,
                                                            mtc_start = obj.mtc_start,
                                                            mtc_stop = (DateTime)obj.mtc_finish,
                                                            pic_id = obj.pic_mtc,
                                                            pic_name = obj.Fullname,
                                                            finish_sts = obj.finish_sts,
                                                            stop_from = db.tr_shift_leader_note.Where(x => x.dept_id == obj.dept_id && x.mc_id == obj.mc_id && x.note_date == obj.note_date && x.shift == obj.shift && x.loom_no == obj.loom_no && x.block == obj.block).Select(x => x.stop_from).FirstOrDefault()
                                                        };

            MaintenanceHistoryHeader temp = new MaintenanceHistoryHeader();

            temp.dept_id = data.Select(x => x.dept_id).FirstOrDefault();
            temp.mc_id = data.Select(x => x.mc_id).FirstOrDefault();
            temp.mc_type = data.Select(x => x.mc_type).FirstOrDefault();
            temp.note_date = data.Select(x => x.note_date).FirstOrDefault();
            temp.shift = data.Select(x => x.shift).FirstOrDefault();
            temp.block = data.Select(x => x.block).FirstOrDefault();
            temp.loom = data.Select(x => x.loom).FirstOrDefault();
            temp.machine_no = data.Select(x => x.machine_no).FirstOrDefault();
            temp.dept_name = data.Select(x => x.dept_name).FirstOrDefault();
            temp.mtc_start = data.Select(x => x.mtc_start).FirstOrDefault();
            temp.mtc_stop = data.Select(x => x.mtc_stop).FirstOrDefault();
            temp.pic_id = data.Select(x => x.pic_id).FirstOrDefault();
            temp.pic_name = data.Select(x => x.pic_name).FirstOrDefault();
            temp.finish_sts = data.Select(x => x.finish_sts).FirstOrDefault();
            temp.stop_from = data.Select(x => x.stop_from).FirstOrDefault();

            IQueryable<MtcAction_Detail> Detail1 = from obj in db.tr_maintenance_action
                                                   where obj.dept_id == dept_id_system && obj.mc_id == mcid && obj.note_date == note_date && obj.shift == shift && obj.loom_no == loom && obj.block == block
                                                   select new MtcAction_Detail()
                                                   {
                                                       mtc_id = obj.mtc_id,
                                                       trouble_id = obj.trouble_id,
                                                       trouble_name = db.v_Maintenance_Action.Where(x => x.dept_id == obj.dept_id && x.mc_id == obj.mc_id && x.mtc_id == obj.mtc_id && x.trouble_id == obj.trouble_id & x.mtc_action_id == obj.mtc_action_id).Select(x => x.trouble_name).FirstOrDefault(),
                                                       mtc_type = db.v_Maintenance_Action.Where(x => x.dept_id == obj.dept_id && x.mc_id == obj.mc_id && x.mtc_id == obj.mtc_id && x.trouble_id == obj.trouble_id & x.mtc_action_id == obj.mtc_action_id).Select(x => x.mtc_name).FirstOrDefault(),
                                                       mtc_action_id = obj.mtc_action_id,
                                                       mtc_action_name = db.v_Maintenance_Action.Where(x => x.dept_id == obj.dept_id && x.mc_id == obj.mc_id && x.mtc_id == obj.mtc_id && x.trouble_id == obj.trouble_id & x.mtc_action_id == obj.mtc_action_id).Select(x => x.mtc_action_name).FirstOrDefault()
                                                   };

            IQueryable<MtcSparepart_Detail> Detail_2 = from obj in db.tr_maintenance_sparepart
                                                       where obj.dept_id == dept_id_system && obj.mc_id == mcid && obj.note_date == note_date && obj.shift == shift && obj.loom_no == loom && obj.block == block
                                                       select new MtcSparepart_Detail()
                                                       {
                                                           item_code = obj.item_code,
                                                           item_name = db.v_MaintenanceSparepart.Where(x => x.item_code == obj.item_code).Select(x => x.ItemName).FirstOrDefault().ToString().Trim(),
                                                           price = (decimal)obj.price,
                                                           quantity = (decimal)obj.quantity
                                                       };
            temp.Detail_1 = Detail1.ToList();
            temp.Detail_2 = Detail_2.ToList();
            return View(temp);

        }


        // GET: MaintenanceHistory/Edit/5
        public ActionResult Edit(string id)
        {
            InitializeUserAkses();
            var access_button = db.ms_user_access_button.Where(x => x.userid == User.Identity.Name.ToString().Trim() && x.menu == "TRANSACTION" && x.submenu.ToString().Trim() == "Machine Quality Trouble/History");
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


            decimal deptid, mcid;
            DateTime note_date;
            string shift, block, loom;

            if (id == null)
            {
                return RedirectToAction("index");
            }

            string[] par = id.Split(';');
            if (par.Length != 1)
            {
                deptid = Convert.ToDecimal(par[0]);
                mcid = Convert.ToDecimal(par[1]);
                note_date = Convert.ToDateTime(par[2].ToString().Replace("_", "-").ToString());
                shift = par[3].ToString();
                block = par[4].ToString();
                loom = par[5].ToString();
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var _stop_from = db.tr_shift_leader_note.Where(x => x.dept_id == deptid && x.mc_id == mcid && x.note_date == note_date && x.shift == shift && x.block == block && x.loom_no == loom).FirstOrDefault().stop_from;

            string dept_code_cim = db.sysuser_app.Where(x => x.ID == User.Identity.Name).Select(x => x.departement).FirstOrDefault();
            string dept_name_cim = db.departement_master.Where(x => x.Dept_ID == dept_code_cim).Select(x => x.Dept_Desc).FirstOrDefault().ToUpper();
            decimal dept_id_system = db.ms_dept.Where(x => x.dept_name == dept_name_cim).Select(x => x.dept_id).FirstOrDefault();

            IQueryable<MaintenanceHistoryHeader> data = from obj in db.v_MaintenanceHistory
                                                        where obj.dept_id == dept_id_system && obj.mc_id == mcid && obj.note_date == note_date && obj.shift == shift && obj.loom_no == loom && obj.block == block
                                                        select new MaintenanceHistoryHeader()
                                                        {
                                                            dept_id = obj.dept_id,
                                                            mc_id = obj.mc_id,
                                                            mc_type = obj.mc_name,
                                                            note_date = obj.note_date,
                                                            shift = obj.shift,
                                                            block = obj.block,
                                                            loom = obj.loom_no,
                                                            machine_no = obj.block + "." + obj.loom_no,
                                                            dept_name = obj.dept_name,
                                                            mtc_start = obj.mtc_start,
                                                            mtc_stop = (DateTime)obj.mtc_finish,
                                                            pic_id = obj.pic_mtc,
                                                            pic_name = obj.Fullname,
                                                            finish_sts = obj.finish_sts.Trim(),
                                                            stop_from_str = "-",
                                                            TanggalString = "-"
                                                        };

            MaintenanceHistoryHeader temp = new MaintenanceHistoryHeader();

            temp.dept_id = data.Select(x => x.dept_id).FirstOrDefault();
            temp.mc_id = data.Select(x => x.mc_id).FirstOrDefault();
            temp.mc_type = data.Select(x => x.mc_type).FirstOrDefault();
            temp.note_date = data.Select(x => x.note_date).FirstOrDefault();
            temp.shift = data.Select(x => x.shift).FirstOrDefault();
            temp.block = data.Select(x => x.block).FirstOrDefault();
            temp.loom = data.Select(x => x.loom).FirstOrDefault();
            temp.machine_no = data.Select(x => x.machine_no).FirstOrDefault();
            temp.dept_name = data.Select(x => x.dept_name).FirstOrDefault();
            temp.mtc_start = data.Select(x => x.mtc_start).FirstOrDefault();
            temp.mtc_stop = data.Select(x => x.mtc_stop).FirstOrDefault();
            temp.pic_id = data.Select(x => x.pic_id).FirstOrDefault();
            temp.pic_name = data.Select(x => x.pic_name).FirstOrDefault();
            temp.finish_sts = data.Select(x => x.finish_sts).FirstOrDefault();
            temp.stop_from_str = _stop_from.ToString("dd-MM-yyyy HH:mm");
            temp.TanggalString = data.Select(x => x.TanggalString).FirstOrDefault();
            temp.mtc_start_str = data.Select(x => x.mtc_start).FirstOrDefault().ToString("dd-MM-yyyy HH:mm");
            temp.mtc_stop_str = !data.Select(x => x.mtc_stop).FirstOrDefault().HasValue ? "" : data.Select(x => x.mtc_stop).FirstOrDefault().Value.ToString("dd-MM-yyyy HH:mm");

            IQueryable<MtcAction_Detail> Detail1 = from obj in db.tr_maintenance_action
                                                   where obj.dept_id == dept_id_system && obj.mc_id == mcid && obj.note_date == note_date && obj.shift == shift && obj.loom_no == loom && obj.block == block
                                                   select new MtcAction_Detail()
                                                   {
                                                       mtc_id = obj.mtc_id,
                                                       trouble_id = obj.trouble_id,
                                                       trouble_name = db.v_Maintenance_Action.Where(x => x.dept_id == obj.dept_id && x.mc_id == obj.mc_id && x.mtc_id == obj.mtc_id && x.trouble_id == obj.trouble_id & x.mtc_action_id == obj.mtc_action_id).Select(x => x.trouble_name).FirstOrDefault(),
                                                       mtc_type = db.v_Maintenance_Action.Where(x => x.dept_id == obj.dept_id && x.mc_id == obj.mc_id && x.mtc_id == obj.mtc_id && x.trouble_id == obj.trouble_id & x.mtc_action_id == obj.mtc_action_id).Select(x => x.mtc_name).FirstOrDefault(),
                                                       mtc_action_id = obj.mtc_action_id,
                                                       mtc_action_name = db.v_Maintenance_Action.Where(x => x.dept_id == obj.dept_id && x.mc_id == obj.mc_id && x.mtc_id == obj.mtc_id && x.trouble_id == obj.trouble_id & x.mtc_action_id == obj.mtc_action_id).Select(x => x.mtc_action_name).FirstOrDefault()
                                                   };

            IQueryable<MtcSparepart_Detail> Detail_2 = from obj in db.tr_maintenance_sparepart
                                                       where obj.dept_id == dept_id_system && obj.mc_id == mcid && obj.note_date == note_date && obj.shift == shift && obj.loom_no == loom && obj.block == block
                                                       select new MtcSparepart_Detail()
                                                       {
                                                           item_code = obj.item_code,
                                                           item_name = db.v_MaintenanceSparepart.Where(x => x.item_code == obj.item_code).Select(x => x.ItemName).FirstOrDefault().ToString().Trim(),
                                                           price = (decimal)obj.price,
                                                           quantity = (decimal)obj.quantity
                                                       };
            temp.Detail_1 = Detail1.ToList();
            temp.Detail_2 = Detail_2.ToList();

            ViewBag.pic_id = new SelectList(db.sysuser_app.Where(x => x.ID == temp.pic_id.Trim()).OrderBy(x => x.Fullname), "ID", "Fullname", temp.pic_id);

            var temp1 = from dataMtcAction in db.v_MaintenanceActionforModal
                        where dataMtcAction.dept_id == deptid && dataMtcAction.mc_id == mcid && dataMtcAction.note_date == note_date &&
                        dataMtcAction.shift == shift && dataMtcAction.block == block && dataMtcAction.loom_no == loom
                        select new MtcAction_Shift()
                        {
                            mtc_id = dataMtcAction.mtc_id,
                            trouble_id = dataMtcAction.trouble_id,
                            mtc_action_id = dataMtcAction.mtc_action_id,
                            mtc_action_name = dataMtcAction.mtc_action_name,
                            trouble_name = dataMtcAction.trouble_name,
                            mtc_type = dataMtcAction.mtc_name,
                            loom = dataMtcAction.loom_no,
                            block = dataMtcAction.block,
                            note_date = dataMtcAction.note_date,
                            shift = dataMtcAction.shift
                        };

            ViewBag.Detil_mtc_action = temp1.ToList();
            string query = "SELECT '0' AS ItemCode, '0' as ItemName, 0 as PRICE union select ItemCode COLLATE DATABASE_DEFAULT as ItemCode, ItemName COLLATE DATABASE_DEFAULT as ItemName, PRICE  from wppc.v_SAP_OITM_PRICE where PRICE > 0 order by ItemName";
            var result = db.Database.SqlQuery<SAPITEM>(query);
            ViewBag.SapItemWithPrice = result.ToList();
            ViewBag.SapItem = new SelectList(result.ToList(), "ItemCode", "ItemName");//result.ToList();

            return View(temp);

        }

        // POST: MaintenanceHistory/Edit/5
        [HttpPost]
        public ActionResult Edit([Bind(Include = "dept_id, dept_name, mc_type, mc_id, note_date, shift, machine_no, block, loom, pic_id, stop_from_str, TanggalString, mtc_start, mtc_stop, mtc_start_str, mtc_stop_str, finish_sts, Detail_1, Detail_2")] MaintenanceHistoryHeader FrmData)
        {
            InitializeUserAkses();

            tr_maintenance CheckTrM = db.tr_maintenance.Find(FrmData.dept_id, FrmData.mc_id, FrmData.note_date, FrmData.shift, FrmData.block, FrmData.loom);
            if (CheckTrM == null)
            {
                ModelState.AddModelError("", "the data not exists in the database, Create the data first using Create Button !!");
                return RedirectToAction("create");
            }

            var mtc_start = DateTime.Now;
            var mtc_finish = DateTime.Now;

            try
            {
                string[] mtc_start_input = FrmData.mtc_start_str.Trim().Split(' ');
                string[] date_mtc_start = mtc_start_input[0].Split('-');
                string[] time_mtc_start = mtc_start_input[1].Split(':');
                mtc_start = new DateTime(Convert.ToInt16(date_mtc_start[2]), Convert.ToInt16(date_mtc_start[1]), Convert.ToInt16(date_mtc_start[0]), Convert.ToInt16(time_mtc_start[0]), Convert.ToInt16(time_mtc_start[1]), 0);

                string[] mtc_finish_input = FrmData.mtc_stop_str.Trim().Split(' ');
                string[] date_mtc_finish = mtc_finish_input[0].Split('-');
                string[] time_mtc_finish = mtc_finish_input[1].Split(':');
                mtc_finish = new DateTime(Convert.ToInt16(date_mtc_finish[2]), Convert.ToInt16(date_mtc_finish[1]), Convert.ToInt16(date_mtc_finish[0]), Convert.ToInt16(time_mtc_finish[0]), Convert.ToInt16(time_mtc_finish[1]), 0);
            }
            catch (Exception ExDatetime)
            {
                ModelState.AddModelError("", "Datetime Issue - " + ExDatetime.ToString());
                initEditForm(FrmData);
                return View(FrmData);

            }

            if (ModelState.IsValid)
            {
                var SplitStopFrom = FrmData.stop_from_str.Trim().Split(' ');
                string[] date_stop_from = SplitStopFrom[0].Split('-');
                string[] time_stop_from = SplitStopFrom[1].Split(':');
                DateTime _stop_from = new DateTime(Convert.ToInt16(date_stop_from[2]), Convert.ToInt16(date_stop_from[1]), Convert.ToInt16(date_stop_from[0]), Convert.ToInt16(time_stop_from[0]), Convert.ToInt16(time_stop_from[1]), 0);

                if (mtc_start < _stop_from)
                {
                    ModelState.AddModelError("mtc_start_str", "Tanggal mulai maintenance start tidak boleh kurang dari tanggal stop mesin");
                    initEditForm(FrmData);
                    return View(FrmData);
                }

                if (mtc_finish < _stop_from)
                {
                    ModelState.AddModelError("mtc_stop_str", "Tanggal selesai maintenance tidak boleh kurang dari tanggal stop mesin");
                    initEditForm(FrmData);
                    return View(FrmData);
                }

            }


            if (ModelState.IsValid)
            {
                using (DbContextTransaction transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        tr_maintenance_hist obj = new tr_maintenance_hist();
                        obj.dept_id = CheckTrM.dept_id; obj.mc_id = CheckTrM.mc_id; obj.note_date = CheckTrM.note_date;
                        obj.shift = CheckTrM.shift; obj.block = CheckTrM.block; obj.loom_no = CheckTrM.loom_no;
                        obj.mtc_start = CheckTrM.mtc_start; obj.mtc_finish = CheckTrM.mtc_finish; obj.pic_mtc = CheckTrM.pic_mtc;
                        obj.finish_sts = CheckTrM.finish_sts; obj.user_id = CheckTrM.user_id; obj.rec_sts = CheckTrM.rec_sts;
                        obj.proc_no = CheckTrM.proc_no; obj.client_ip = CheckTrM.client_ip; obj.proc_time = CheckTrM.proc_time;
                        obj.proc_time_edit = DateTime.Now; obj.user_id_edit = User.Identity.Name.ToUpper().Trim();

                        db.tr_maintenance_hist.Add(obj);
                        db.SaveChanges();

                        CheckTrM.mtc_start = mtc_start;//FrmData.mtc_start;
                        CheckTrM.mtc_finish = mtc_finish; //FrmData.mtc_stop;
                        CheckTrM.pic_mtc = FrmData.pic_id;
                        CheckTrM.user_id = User.Identity.Name.ToUpper();
                        CheckTrM.rec_sts = "T";
                        CheckTrM.client_ip = GetIPAddress();
                        CheckTrM.proc_time = DateTime.Now;
                        CheckTrM.finish_sts = FrmData.finish_sts == null || FrmData.finish_sts == "" ? "" : "F";
                        CheckTrM.proc_no = CheckTrM.proc_no + 1;
                        db.Entry(CheckTrM).State = EntityState.Modified;
                        db.SaveChanges();

                        if (FrmData.finish_sts.ToString().Trim() == "F")
                        {
                            tr_shift_leader_note getData = db.tr_shift_leader_note.Find(FrmData.dept_id, FrmData.mc_id, FrmData.note_date, FrmData.shift, FrmData.block, FrmData.loom);

                            tr_shift_leader_note_hist History = new tr_shift_leader_note_hist();
                            History.dept_id = getData.dept_id; History.mc_id = getData.mc_id; History.note_date = getData.note_date; History.shift = getData.shift; History.block = getData.block; History.loom_no = getData.loom_no;
                            History.stop_from = getData.stop_from; History.stop_to = getData.stop_to; History.pic = getData.pic; History.user_id = getData.user_id; History.rec_sts = getData.rec_sts;
                            History.proc_no = getData.proc_no; History.client_ip = getData.client_ip; History.proc_time = getData.proc_time;
                            History.proc_time_edit = DateTime.Now; History.user_id_edit = User.Identity.Name.ToUpper().Trim();

                            db.tr_shift_leader_note_hist.Add(History);
                            db.SaveChanges();

                            getData.stop_to = mtc_finish;//FrmData.mtc_stop;
                            getData.proc_no = getData.proc_no + 1;
                            getData.proc_time = DateTime.Now;
                            getData.rec_sts = "T";
                            getData.user_id = getData.user_id;
                            getData.client_ip = GetIPAddress();
                            db.Entry(getData).State = EntityState.Modified;
                            db.SaveChanges();
                        }

                        var Trm_Action = db.tr_maintenance_action.Where(x => x.dept_id == FrmData.dept_id && x.mc_id == FrmData.mc_id && x.note_date == FrmData.note_date && x.shift == FrmData.shift && x.block == FrmData.block && x.loom_no == FrmData.loom);

                        if (Trm_Action.Count() > 0)
                        {
                            foreach (var item in Trm_Action)
                            {
                                tr_maintenance_action_hist His_Trm_Action = new tr_maintenance_action_hist();
                                His_Trm_Action.dept_id = item.dept_id; His_Trm_Action.mc_id = item.mc_id;
                                His_Trm_Action.note_date = item.note_date; His_Trm_Action.shift = item.shift;
                                His_Trm_Action.block = item.block; His_Trm_Action.loom_no = item.loom_no;
                                His_Trm_Action.mtc_id = item.mtc_id; His_Trm_Action.trouble_id = item.trouble_id;
                                His_Trm_Action.mtc_action_id = item.mtc_action_id; His_Trm_Action.proc_time = item.proc_time;
                                His_Trm_Action.proc_time_edit = DateTime.Now; His_Trm_Action.user_id_edit = User.Identity.Name.ToUpper().Trim();

                                db.tr_maintenance_action_hist.Add(His_Trm_Action);
                                db.SaveChanges();
                            }

                            db.tr_maintenance_action.RemoveRange(Trm_Action);
                            db.SaveChanges();
                        }

                        var Trm_Sparepart = db.tr_maintenance_sparepart.Where(x => x.dept_id == FrmData.dept_id && x.mc_id == FrmData.mc_id && x.note_date == FrmData.note_date && x.shift == FrmData.shift && x.block == FrmData.block && x.loom_no == FrmData.loom);
                        if (Trm_Sparepart.Count() > 0)
                        {
                            foreach (var item in Trm_Sparepart)
                            {
                                tr_maintenance_sparepart_hist His_Trm_SP = new tr_maintenance_sparepart_hist();
                                His_Trm_SP.dept_id = item.dept_id; His_Trm_SP.mc_id = item.mc_id; His_Trm_SP.note_date = item.note_date;
                                His_Trm_SP.shift = item.shift; His_Trm_SP.block = item.block; His_Trm_SP.loom_no = item.loom_no;
                                His_Trm_SP.item_code = item.item_code; His_Trm_SP.price = item.price; His_Trm_SP.quantity = item.quantity;
                                His_Trm_SP.proc_time = item.proc_time; His_Trm_SP.proc_time_edit = DateTime.Now; His_Trm_SP.user_id_edit = User.Identity.Name.ToUpper().Trim();

                                db.tr_maintenance_sparepart_hist.Add(His_Trm_SP);
                                db.SaveChanges();
                            }

                            db.tr_maintenance_sparepart.RemoveRange(Trm_Sparepart);
                            db.SaveChanges();
                        }

                        //db.tr_maintenance_action.RemoveRange(db.tr_maintenance_action.Where(x => x.dept_id == FrmData.dept_id && x.mc_id == FrmData.mc_id && x.note_date == FrmData.note_date && x.shift == FrmData.shift && x.block == FrmData.block && x.loom_no == FrmData.loom));
                        //db.SaveChanges();

                        //db.tr_maintenance_sparepart.RemoveRange(db.tr_maintenance_sparepart.Where(x => x.dept_id == FrmData.dept_id && x.mc_id == FrmData.mc_id && x.note_date == FrmData.note_date && x.shift == FrmData.shift && x.block == FrmData.block && x.loom_no == FrmData.loom));
                        //db.SaveChanges();

                        foreach (var item in FrmData.Detail_1)
                        {
                            tr_maintenance_action InsertData = new tr_maintenance_action();
                            InsertData.dept_id = FrmData.dept_id;
                            InsertData.mc_id = FrmData.mc_id;
                            InsertData.note_date = FrmData.note_date;
                            InsertData.shift = FrmData.shift;
                            InsertData.block = FrmData.block;
                            InsertData.loom_no = FrmData.loom;
                            InsertData.mtc_id = item.mtc_id.ToString().Trim();
                            InsertData.trouble_id = item.trouble_id.ToString().Trim();
                            InsertData.mtc_action_id = item.mtc_action_id.ToString().Trim();
                            InsertData.proc_time = DateTime.Now;

                            db.tr_maintenance_action.Add(InsertData);
                            db.SaveChanges();

                        }

                        if (FrmData.Detail_2 != null)
                        {
                            foreach (var item2 in FrmData.Detail_2)
                            {
                                tr_maintenance_sparepart Insert_sparepart = new tr_maintenance_sparepart();
                                Insert_sparepart.dept_id = FrmData.dept_id;
                                Insert_sparepart.mc_id = FrmData.mc_id;
                                Insert_sparepart.note_date = FrmData.note_date;
                                Insert_sparepart.shift = FrmData.shift;
                                Insert_sparepart.block = FrmData.block;
                                Insert_sparepart.loom_no = FrmData.loom;
                                Insert_sparepart.item_code = item2.item_code.ToString().Trim();
                                Insert_sparepart.price = item2.price;
                                Insert_sparepart.quantity = item2.quantity;
                                Insert_sparepart.proc_time = DateTime.Now;
                                db.tr_maintenance_sparepart.Add(Insert_sparepart);
                                db.SaveChanges();

                            }
                        }

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

                        var get_roles = db.v_user_ism_maintenance.Where(x => x.ID == User.Identity.Name.ToUpper().Trim());
                        string roles = "";

                        if (get_roles != null)
                        {
                            roles = get_roles.FirstOrDefault().roles.Trim();
                        }

                        if (roles == "ADMIN")
                        {
                            ViewBag.pic_id = new SelectList(db.sysuser_app.Where(x => x.ID == User.Identity.Name.ToUpper().Trim()).OrderBy(x => x.Fullname), "ID", "Fullname", User.Identity.Name.ToUpper());
                        }
                        else
                        {
                            ViewBag.pic_id = new SelectList(db.sysuser_app.Where(x => x.departement == "W" && x.section == "mt" && x.ID == User.Identity.Name.Trim()).OrderBy(x => x.Fullname), "ID", "Fullname", User.Identity.Name.ToUpper());
                        }

                        var temp1 = from xx in db.v_MaintenanceActionShiftLeaderNotes
                                    where xx.dept_id == FrmData.dept_id && xx.mc_id == FrmData.mc_id && xx.note_date == FrmData.note_date && xx.shift == FrmData.shift && xx.block == FrmData.block && xx.loom_no == FrmData.loom
                                    select new MtcAction_Shift()
                                    {
                                        mtc_id = xx.mtc_id,
                                        trouble_id = xx.trouble_id,
                                        mtc_action_id = xx.mtc_action_id,
                                        mtc_action_name = xx.mtc_action_name,
                                        trouble_name = xx.trouble_name,
                                        mtc_type = xx.mtc_name,
                                        loom = xx.loom_no,
                                        block = xx.block,
                                        note_date = xx.note_date,
                                        shift = xx.shift
                                    };

                        ViewBag.Detil_mtc_action = temp1.ToList();
                        string query = "select ItemCode COLLATE DATABASE_DEFAULT as ItemCode, ItemName COLLATE DATABASE_DEFAULT as ItemName, PRICE  from wppc.v_SAP_OITM_PRICE";
                        var result = db.Database.SqlQuery<SAPITEM>(query);
                        ViewBag.SapItemWithPrice = result.ToList();
                        ViewBag.SapItem = new SelectList(result.ToList(), "ItemCode", "ItemName");//result.ToList();
                        return View(FrmData);
                    }

                }
                //return RedirectToAction("Index");
                return RedirectToAction("Index", new { id = FrmData.dept_id + ";" + FrmData.mc_id + ";" + FrmData.note_date.ToString("dd-MM-yyyy") + ";" + FrmData.shift + ";" + FrmData.block + ";" + FrmData.loom });

            }

            return RedirectToAction("Edit");
        }

        // GET: MaintenanceHistory/Delete/5
        public ActionResult Delete(string id)
        {
            InitializeUserAkses();
            var access_button = db.ms_user_access_button.Where(x => x.userid == User.Identity.Name.ToString().Trim() && x.menu == "TRANSACTION" && x.submenu.ToString().Trim() == "Machine Quality Trouble/History");
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

            decimal deptid, mcid;
            DateTime note_date;
            string shift, block, loom;

            if (id == null)
            {
                return RedirectToAction("index");
            }

            string[] par = id.Split(';');
            if (par.Length != 1)
            {
                deptid = Convert.ToDecimal(par[0]);
                mcid = Convert.ToDecimal(par[1]);
                note_date = Convert.ToDateTime(par[2].ToString().Replace("_", "-").ToString());
                shift = par[3].ToString();
                block = par[4].ToString();
                loom = par[5].ToString();
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            string dept_code_cim = db.sysuser_app.Where(x => x.ID == User.Identity.Name).Select(x => x.departement).FirstOrDefault();
            string dept_name_cim = db.departement_master.Where(x => x.Dept_ID == dept_code_cim).Select(x => x.Dept_Desc).FirstOrDefault().ToUpper();
            decimal dept_id_system = db.ms_dept.Where(x => x.dept_name == dept_name_cim).Select(x => x.dept_id).FirstOrDefault();

            var data = from obj in db.v_MaintenanceHistory
                       where obj.dept_id == dept_id_system && obj.mc_id == mcid && obj.note_date == note_date && obj.shift == shift && obj.loom_no == loom && obj.block == block
                       select new MaintenanceHistoryHeader()
                       {
                           dept_id = obj.dept_id,
                           mc_id = obj.mc_id,
                           mc_type = obj.mc_name,
                           note_date = obj.note_date,
                           shift = obj.shift,
                           block = obj.block,
                           loom = obj.loom_no,
                           machine_no = obj.block + "." + obj.loom_no,
                           dept_name = obj.dept_name,
                           mtc_start = obj.mtc_start,
                           mtc_stop = (DateTime)obj.mtc_finish,
                           pic_id = obj.pic_mtc,
                           pic_name = obj.Fullname,
                           finish_sts = obj.finish_sts.Trim(),
                           //stop_from = db.tr_shift_leader_note.Where(x => x.dept_id == obj.dept_id && x.mc_id == obj.mc_id && x.note_date == obj.note_date && x.shift == obj.shift && x.loom_no == obj.loom_no && x.block == obj.block).Select(x => x.stop_from).FirstOrDefault()
                       };

            MaintenanceHistoryHeader temp = new MaintenanceHistoryHeader();

            temp.dept_id = data.Select(x => x.dept_id).FirstOrDefault();
            temp.mc_id = data.Select(x => x.mc_id).FirstOrDefault();
            temp.mc_type = data.Select(x => x.mc_type).FirstOrDefault();
            temp.note_date = data.Select(x => x.note_date).FirstOrDefault();
            temp.shift = data.Select(x => x.shift).FirstOrDefault();
            temp.block = data.Select(x => x.block).FirstOrDefault();
            temp.loom = data.Select(x => x.loom).FirstOrDefault();
            temp.machine_no = data.Select(x => x.machine_no).FirstOrDefault();
            temp.dept_name = data.Select(x => x.dept_name).FirstOrDefault();
            temp.mtc_start = data.Select(x => x.mtc_start).FirstOrDefault();
            temp.mtc_stop = data.Select(x => x.mtc_stop).FirstOrDefault();
            temp.pic_id = data.Select(x => x.pic_id).FirstOrDefault();
            temp.pic_name = data.Select(x => x.pic_name).FirstOrDefault();
            temp.finish_sts = data.Select(x => x.finish_sts).FirstOrDefault();
            //temp.stop_from = data.Select(x => x.stop_from).FirstOrDefault();

            IQueryable<MtcAction_Detail> Detail1 = from obj in db.tr_maintenance_action
                                                   where obj.dept_id == dept_id_system && obj.mc_id == mcid && obj.note_date == note_date && obj.shift == shift && obj.loom_no == loom && obj.block == block
                                                   select new MtcAction_Detail()
                                                   {
                                                       mtc_id = obj.mtc_id,
                                                       trouble_id = obj.trouble_id,
                                                       trouble_name = db.v_Maintenance_Action.Where(x => x.dept_id == obj.dept_id && x.mc_id == obj.mc_id && x.mtc_id == obj.mtc_id && x.trouble_id == obj.trouble_id & x.mtc_action_id == obj.mtc_action_id).Select(x => x.trouble_name).FirstOrDefault(),
                                                       mtc_type = db.v_Maintenance_Action.Where(x => x.dept_id == obj.dept_id && x.mc_id == obj.mc_id && x.mtc_id == obj.mtc_id && x.trouble_id == obj.trouble_id & x.mtc_action_id == obj.mtc_action_id).Select(x => x.mtc_name).FirstOrDefault(),
                                                       mtc_action_id = obj.mtc_action_id,
                                                       mtc_action_name = db.v_Maintenance_Action.Where(x => x.dept_id == obj.dept_id && x.mc_id == obj.mc_id && x.mtc_id == obj.mtc_id && x.trouble_id == obj.trouble_id & x.mtc_action_id == obj.mtc_action_id).Select(x => x.mtc_action_name).FirstOrDefault()
                                                   };

            IQueryable<MtcSparepart_Detail> Detail_2 = from obj in db.tr_maintenance_sparepart
                                                       where obj.dept_id == dept_id_system && obj.mc_id == mcid && obj.note_date == note_date && obj.shift == shift && obj.loom_no == loom && obj.block == block
                                                       select new MtcSparepart_Detail()
                                                       {
                                                           item_code = obj.item_code,
                                                           item_name = db.v_MaintenanceSparepart.Where(x => x.item_code == obj.item_code).Select(x => x.ItemName).FirstOrDefault().ToString().Trim(),
                                                           price = (decimal)obj.price,
                                                           quantity = (decimal)obj.quantity
                                                       };
            temp.Detail_1 = Detail1.ToList();
            temp.Detail_2 = Detail_2.ToList();

            var temp1 = from dataMtcAction in db.v_MaintenanceActionShiftLeaderNotes
                        where dataMtcAction.dept_id == deptid && dataMtcAction.mc_id == mcid && dataMtcAction.note_date == note_date &&
                        dataMtcAction.shift == shift && dataMtcAction.block == block && dataMtcAction.loom_no == loom
                        select new MtcAction_Shift()
                        {
                            mtc_id = dataMtcAction.mtc_id,
                            trouble_id = dataMtcAction.trouble_id,
                            mtc_action_id = dataMtcAction.mtc_action_id,
                            mtc_action_name = dataMtcAction.mtc_action_name,
                            trouble_name = dataMtcAction.trouble_name,
                            mtc_type = dataMtcAction.mtc_name,
                            loom = dataMtcAction.loom_no,
                            block = dataMtcAction.block,
                            note_date = dataMtcAction.note_date,
                            shift = dataMtcAction.shift
                        };

            return View(temp);

        }

        // POST: MaintenanceHistory/Delete/5        
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            InitializeUserAkses();

            decimal deptid, mcid;
            DateTime note_date;
            string shift, block, loom;

            if (id == null)
            {
                return RedirectToAction("index");
            }

            string[] par = id.Split(';');
            if (par.Length != 1)
            {
                deptid = Convert.ToDecimal(par[0]);
                mcid = Convert.ToDecimal(par[1]);
                note_date = Convert.ToDateTime(par[2].ToString().Replace("_", "-").ToString());
                shift = par[3].ToString();
                block = par[4].ToString();
                loom = par[5].ToString();
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    tr_maintenance Header = db.tr_maintenance.Find(deptid, mcid, note_date, shift, block, loom);
                    //tr_maintenance Header = db.tr_maintenance.Find(900, mcid, note_date, shift, block, loom);

                    if (Header != null)
                    {
                        if (Header.finish_sts == "F")
                        {
                            tr_shift_leader_note findleadernote = db.tr_shift_leader_note.Find(deptid, mcid, note_date, shift, block, loom);
                            if (findleadernote != null)
                            {

                                tr_shift_leader_note_hist LeaderNote_His = new tr_shift_leader_note_hist();
                                LeaderNote_His.dept_id = findleadernote.dept_id; LeaderNote_His.mc_id = findleadernote.mc_id; LeaderNote_His.note_date = findleadernote.note_date; LeaderNote_His.shift = findleadernote.shift; LeaderNote_His.block = findleadernote.block; LeaderNote_His.loom_no = findleadernote.loom_no;
                                LeaderNote_His.stop_from = findleadernote.stop_from; LeaderNote_His.stop_to = findleadernote.stop_to; LeaderNote_His.pic = findleadernote.pic; LeaderNote_His.user_id = findleadernote.user_id; LeaderNote_His.rec_sts = findleadernote.rec_sts;
                                LeaderNote_His.proc_no = findleadernote.proc_no; LeaderNote_His.client_ip = findleadernote.client_ip; LeaderNote_His.proc_time = findleadernote.proc_time;
                                LeaderNote_His.proc_time_edit = DateTime.Now; LeaderNote_His.user_id_edit = User.Identity.Name.ToUpper().Trim();

                                findleadernote.stop_to = null;
                                findleadernote.proc_time = DateTime.Now;
                                findleadernote.proc_no = findleadernote.proc_no + 1;
                                findleadernote.user_id = User.Identity.Name.ToUpper();
                                findleadernote.client_ip = GetIPAddress();
                                findleadernote.rec_sts = "T";

                                db.Entry(findleadernote).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                        }

                        tr_maintenance_hist TRM_His = new tr_maintenance_hist();
                        TRM_His.dept_id = Header.dept_id; TRM_His.mc_id = Header.mc_id; TRM_His.note_date = Header.note_date; TRM_His.shift = Header.shift; TRM_His.block = Header.block; TRM_His.loom_no = Header.loom_no;
                        TRM_His.mtc_start = Header.mtc_start; TRM_His.mtc_finish = Header.mtc_finish; TRM_His.pic_mtc = Header.pic_mtc; TRM_His.finish_sts = Header.finish_sts; TRM_His.user_id = Header.user_id;
                        TRM_His.rec_sts = Header.rec_sts; TRM_His.proc_no = Header.proc_no; TRM_His.client_ip = Header.client_ip; TRM_His.proc_time = Header.proc_time; TRM_His.proc_time_edit = DateTime.Now; TRM_His.user_id_edit = User.Identity.Name.ToUpper().Trim();
                        db.tr_maintenance_hist.Add(TRM_His);
                        db.SaveChanges();

                        db.tr_maintenance.Remove(Header);
                        db.SaveChanges();

                        var TRM_Act = db.tr_maintenance_action.Where(x => x.dept_id == deptid && x.mc_id == mcid &&
                                                                    x.note_date == note_date && x.shift == shift.ToString().Trim() && x.block == block && x.loom_no == loom);

                        if (TRM_Act.Count() > 0)
                        {
                            foreach (var item in TRM_Act)
                            {
                                tr_maintenance_action_hist His_Trm_Action = new tr_maintenance_action_hist();
                                His_Trm_Action.dept_id = item.dept_id; His_Trm_Action.mc_id = item.mc_id;
                                His_Trm_Action.note_date = item.note_date; His_Trm_Action.shift = item.shift;
                                His_Trm_Action.block = item.block; His_Trm_Action.loom_no = item.loom_no;
                                His_Trm_Action.mtc_id = item.mtc_id; His_Trm_Action.trouble_id = item.trouble_id;
                                His_Trm_Action.mtc_action_id = item.mtc_action_id; His_Trm_Action.proc_time = item.proc_time;
                                His_Trm_Action.proc_time_del = DateTime.Now; His_Trm_Action.user_id_del = User.Identity.Name.ToUpper().Trim();

                                db.tr_maintenance_action_hist.Add(His_Trm_Action);
                                db.SaveChanges();
                            }

                            db.tr_maintenance_action.RemoveRange(TRM_Act);
                            db.SaveChanges();
                        }

                        var TRM_SP = db.tr_maintenance_sparepart.Where(x => x.dept_id == deptid && x.mc_id == mcid &&
                                                                    x.note_date == note_date && x.shift == shift.ToString().Trim() && x.block == block && x.loom_no == loom);
                        if (TRM_SP.Count() > 0)
                        {
                            foreach (var item in TRM_SP)
                            {
                                tr_maintenance_sparepart_hist His_Trm_SP = new tr_maintenance_sparepart_hist();
                                His_Trm_SP.dept_id = item.dept_id; His_Trm_SP.mc_id = item.mc_id; His_Trm_SP.note_date = item.note_date;
                                His_Trm_SP.shift = item.shift; His_Trm_SP.block = item.block; His_Trm_SP.loom_no = item.loom_no;
                                His_Trm_SP.item_code = item.item_code; His_Trm_SP.price = item.price; His_Trm_SP.quantity = item.quantity;
                                His_Trm_SP.proc_time = item.proc_time; His_Trm_SP.proc_time_del = DateTime.Now; His_Trm_SP.user_id_del = User.Identity.Name.ToUpper().Trim();

                                db.tr_maintenance_sparepart_hist.Add(His_Trm_SP);
                                db.SaveChanges();
                            }
                        }
                        db.tr_maintenance_sparepart.RemoveRange(TRM_SP);
                        db.SaveChanges();
                    }

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

                    initform();
                    return View();
                }
            }

            return RedirectToAction("Index");

        }

    }

}
