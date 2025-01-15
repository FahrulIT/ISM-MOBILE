using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ISM_MAINTENANCE.Models.ViewModel;
using ISM_MAINTENANCE.Models.DB;
using System.Net;
using System.Data.Entity;
using Helper;

namespace ISM_MAINTENANCE.Controllers
{
    [Authorize]
    public class ShiftLeaderNotesController : Controller
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

                ViewBag.UserAksesMenu = db.ms_user_access_menu.Where(x => x.userid == User.Identity.Name.ToString().Trim()).AsNoTracking().ToList();
                ViewBag.Section = db.v_user_ism_maintenance.Where(x => x.ID == User.Identity.Name.ToString().Trim()).FirstOrDefault().section.ToString().Trim();
                ViewBag.Roles = db.v_user_ism_maintenance.Where(x => x.ID == User.Identity.Name.ToString().Trim()).FirstOrDefault().roles.ToString().Trim();
            }
        }

        private string ReplaceNullValueString(object obj)
        {
            string _value = "";

            if (obj == null)
            {
                _value = "";
            }
            else
            {
                _value = obj.ToString();
            }

            return _value;
        }

        private decimal ReplaceNullValueNumeric(object obj)
        {
            decimal _value = 0;
            if (obj == null)
            {
                _value = 0;
            }
            else
            {
                _value = Convert.ToDecimal(obj);
            }

            return _value;
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

        private void InitCreatePage()
        {
            string dept_code_cim = db.sysuser_app.Where(x => x.ID == User.Identity.Name).Select(x => x.departement).FirstOrDefault();
            string dept_name_cim = db.departement_master.Where(x => x.Dept_ID == dept_code_cim).Select(x => x.Dept_Desc).FirstOrDefault().ToUpper();
            decimal dept_id_system = db.ms_dept.Where(x => x.dept_name == dept_name_cim).Select(x => x.dept_id).FirstOrDefault();

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
                ViewBag.pic = new SelectList(db.sysuser_app.Where(x => x.ID == User.Identity.Name.ToUpper().Trim()).OrderBy(x => x.Fullname), "ID", "Fullname", User.Identity.Name.ToUpper());
            }
            else
            {
                //ViewBag.pic = new SelectList(db.sysuser_app.Where(x => x.departement == "W" && x.section == "pa" & x.ID == User.Identity.Name.ToUpper()).OrderBy(x => x.Fullname), "ID", "Fullname", User.Identity.Name.ToUpper());
                ViewBag.pic = new SelectList(db.sysuser_app.Where(x => x.ID == User.Identity.Name.ToUpper()).OrderBy(x => x.Fullname), "ID", "Fullname", User.Identity.Name.ToUpper());
            }

            ViewBag.machine_no = new SelectList(db.v_Machine_Master_AJL, "MachineNo", "MachineNo");
            ViewBag.TroubleItem = db.v_Maintenance_Type_for_Maintenance_Action.OrderBy(x => x.trouble_name).AsNoTracking();
            ViewBag.MaintenanceType = new SelectList(db.ms_maintenance.Where(x => x.mtc_id != "MTC30"), "mtc_id", "mtc_name", "MTC10");


            List<SelectListItem> ShiftList = new List<SelectListItem>();
            ShiftList.Add(new SelectListItem
            {
                Text = "A",
                Value = "A"
            });
            ShiftList.Add(new SelectListItem
            {
                Text = "B",
                Value = "B"
            });
            ShiftList.Add(new SelectListItem
            {
                Text = "C",
                Value = "C"
            });
            ViewBag.Shift = ShiftList;

        }



        // GET: ShiftLeaderNotes
        public ActionResult Index(string _block, string id)
        {
            InitializeUserAkses();

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

                IQueryable<ShiftLeaderNotesView> obj = from data in db.v_Shift_Leader_Header
                                                       where data.dept_id == _dept_id && data.mc_id == _mc_id && data.note_date == _note_date && data.shift == _shift && data.block == block && data.loom_no == _loom
                                                       select new ShiftLeaderNotesView()
                                                       {
                                                           dept_id = data.dept_id,
                                                           mc_id = data.mc_id,
                                                           note_date = data.note_date,
                                                           shift = data.shift,
                                                           pic = data.pic,
                                                           dept_name = data.dept_name,
                                                           mc_name = data.mc_name,
                                                           PicName = data.Fullname,
                                                           machine_no = data.block + "." + data.loom_no,
                                                           block = data.block,
                                                           loom_no = data.loom_no,
                                                           stop_from = data.stop_from,
                                                           stop_to = data.stop_to,
                                                           mtc_name = data.mtc_name
                                                       };
                return View(obj.AsNoTracking());
            }
            else
            {
                IQueryable<ShiftLeaderNotesView> obj = from data in db.v_Shift_Leader_Header
                                                       where _block == "All" ? (data.block.Equals("A") || data.block.Equals("B") || data.block.Equals("C") || data.block.Equals("D")) && data.stop_to == null : data.block.Equals(_block) && data.stop_to == null
                                                       orderby data.note_date descending, data.dept_id
                                                       select new ShiftLeaderNotesView()
                                                       {
                                                           dept_id = data.dept_id,
                                                           mc_id = data.mc_id,
                                                           note_date = data.note_date,
                                                           shift = data.shift,
                                                           pic = data.pic,
                                                           dept_name = data.dept_name,
                                                           mc_name = data.mc_name,
                                                           PicName = data.Fullname,
                                                           machine_no = data.block + "." + data.loom_no,
                                                           block = data.block,
                                                           loom_no = data.loom_no,
                                                           stop_from = data.stop_from,
                                                           stop_to = data.stop_to,
                                                           mtc_name = data.mtc_name
                                                       };
                return View(obj.AsNoTracking());
            }


        }

        public ActionResult Show(string id)
        {
            InitializeUserAkses();
            var access_button = db.ms_user_access_button.Where(x => x.userid == User.Identity.Name.ToString().Trim() && x.menu == "TRANSACTION" && x.submenu.ToString().Trim() == "Machine Quality Trouble/Shift Leader Notes");
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

            string dept_code_cim = db.sysuser_app.Where(x => x.ID == User.Identity.Name).Select(x => x.departement).FirstOrDefault();
            string dept_name_cim = db.departement_master.Where(x => x.Dept_ID == dept_code_cim).Select(x => x.Dept_Desc).FirstOrDefault().ToUpper();
            decimal dept_id_system = db.ms_dept.Where(x => x.dept_name == dept_name_cim).Select(x => x.dept_id).FirstOrDefault();

            ViewBag.TroubleItem = db.v_Maintenance_Type_for_Maintenance_Action.AsNoTracking();
            ViewBag.MaintenanceType = new SelectList(db.ms_maintenance.Where(x => x.mtc_id != "MTC30"), "mtc_id", "mtc_name", "MTC10");


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

            var query = from data in db.v_Shift_Leader
                        where data.dept_id == deptid && data.note_date == note_date && data.shift == shift && data.block == block && data.loom_no == loom
                        select new ShiftLeaderNotes
                        {
                            dept_id = data.dept_id,
                            mc_id = data.mc_id,
                            note_date = data.note_date,
                            shift = data.shift,
                            pic = data.pic,
                            PicName = data.Fullname,
                            dept_name = data.dept_name,
                            mc_name = data.mc_name,
                            block = data.block,
                            loom_no = data.loom_no,
                            machine_no = data.block + "." + data.loom_no,
                            stop_from = data.stop_from,
                            stop_to = (DateTime)data.stop_to
                        };

            ShiftLeaderNotes obj = new ShiftLeaderNotes();
            obj.dept_id = ReplaceNullValueNumeric(query.Select(x => x.dept_id).FirstOrDefault());
            obj.mc_id = ReplaceNullValueNumeric(query.Select(x => x.mc_id).FirstOrDefault());
            obj.note_date = query.Select(x => x.note_date).FirstOrDefault();
            obj.shift = ReplaceNullValueString(query.Select(x => x.shift).FirstOrDefault());
            obj.pic = ReplaceNullValueString(query.Select(x => x.pic).FirstOrDefault());
            obj.dept_name = ReplaceNullValueString(query.Select(x => x.dept_name).FirstOrDefault());
            obj.mc_name = ReplaceNullValueString(query.Select(x => x.mc_name).FirstOrDefault());
            obj.PicName = ReplaceNullValueString(query.Select(x => x.PicName).FirstOrDefault());
            obj.block = ReplaceNullValueString(query.Select(x => x.block).FirstOrDefault());
            obj.loom_no = ReplaceNullValueString(query.Select(x => x.loom_no).FirstOrDefault());
            obj.stop_from = query.Select(x => x.stop_from).FirstOrDefault();
            obj.stop_to = query.Select(x => x.stop_to).FirstOrDefault();
            obj.machine_no = ReplaceNullValueString(query.Select(x => x.machine_no).FirstOrDefault());

            IQueryable<ShiftLeaderNotesDetail> obj2 = from data in db.v_Shift_Leader
                                                      where data.dept_id == deptid && data.note_date == note_date && data.shift == shift && data.block == block && data.loom_no == loom
                                                      select new ShiftLeaderNotesDetail()
                                                      {
                                                          dept_id = data.dept_id,
                                                          mc_id = data.mc_id,
                                                          note_date = data.note_date,
                                                          shift = data.shift,
                                                          block = data.block,
                                                          loom_no = data.loom_no,
                                                          trouble_id = data.trouble_id,
                                                          trouble_name = data.trouble_name,
                                                          mtc_name = data.mtc_name,
                                                          mtc_id = data.mtc_id
                                                      };

            obj.ShiftLeaderNotesDetails = obj2.AsNoTracking().ToList();
            ViewBag.pic = new SelectList(db.sysuser_app.Where(x => x.departement == "W").OrderBy(x => x.Fullname), "ID", "Fullname", obj.pic.ToString().Trim());

            return View(obj);

        }

        ////GET
        public ActionResult Create()
        {
            InitializeUserAkses();
            var access_button = db.ms_user_access_button.Where(x => x.userid == User.Identity.Name.ToString().Trim() && x.menu == "TRANSACTION" && x.submenu.ToString().Trim() == "Machine Quality Trouble/Shift Leader Notes");
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

            InitCreatePage();
            //string time = DateTime.Now.AddDays(-1).ToString("dd-MM-yyyy HH:mm");
            string time = DateTime.Now.AddDays(-1).ToString("HH:mm");

            ShiftLeaderNotes obj = new ShiftLeaderNotes();
            obj.note_date = DateTime.Today.AddDays(-1);
            obj.DateInput = DateTime.Today.ToString("dd-MM-yyyy");
            obj.stop_from_str = time;
            return View(obj);
        }

        [HttpPost]
        public ActionResult Create([Bind(Include = "dept_id, mc_id, DateInput, shift, pic, machine_no, stop_from_str, ShiftLeaderNotesDetails")] ShiftLeaderNotes FrmData)
        {
            InitializeUserAkses();

            DateTime note_date, stop_from;
            note_date = DateTime.Now;
            stop_from = DateTime.Now;

            try
            {
                string[] tanggal = FrmData.DateInput.Split('-');
                note_date = new DateTime(Convert.ToInt16(tanggal[2]), Convert.ToInt16(tanggal[1]), Convert.ToInt16(tanggal[0]));

                //string[] stop_from_input = FrmData.stop_from_str.Trim().Split(' ');
                //string[] date_stop_from = stop_from_input[0].Split('-');
                //string[] time_stop_from = stop_from_input[1].Split(':');
                //stop_from = new DateTime(Convert.ToInt16(date_stop_from[2]), Convert.ToInt16(date_stop_from[1]), Convert.ToInt16(date_stop_from[0]), Convert.ToInt16(time_stop_from[0]), Convert.ToInt16(time_stop_from[1]), 0);

                string[] time_stop_from = FrmData.stop_from_str.Split(':');
                stop_from = new DateTime(Convert.ToInt16(tanggal[2]), Convert.ToInt16(tanggal[1]), Convert.ToInt16(tanggal[0]), Convert.ToInt16(time_stop_from[0]), Convert.ToInt16(time_stop_from[1]), 0);


            }
            catch (Exception ExDatetime)
            {
                ModelState.AddModelError("", "Datetime Issue - " + ExDatetime.ToString());
                InitCreatePage();
                InitializeUserAkses();
                return View();
            }

            //Note date harus == today date
            //check date > today
            var today = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
           
           if (note_date > today)
           {
                ModelState.AddModelError("DateInput", "Kolom Date tidak boleh kurang dari today date");
                InitCreatePage();
                InitializeUserAkses();
                return View();
            }

           //check - 3 hari       
            var selisih = today - note_date;           
            if (selisih.Days > 2)
            {
                ModelState.AddModelError("DateInput", "selisih dengan today date lebih dari 3 hari");
                InitCreatePage();
                InitializeUserAkses();
                return View();
            }

            DateTime Today = DateTime.Now;

            if (note_date.Day > Today.Day && note_date.Month >= Today.Month && note_date.Year >= Today.Year)
            {
                ModelState.AddModelError("DateInput", "Date lebih dari tanggal sekarang");
                InitCreatePage();            
                return View();
            }


            if (ModelState.IsValid)
            {
                string[] par = FrmData.machine_no.ToString().Trim().Split('.');
                var block = par[0];
                var loom = par[1];

                //check duplicate based on primary key
                tr_shift_leader_note Duplicate = db.tr_shift_leader_note.Find(FrmData.dept_id, FrmData.mc_id, note_date, FrmData.shift, block, loom);
                if (Duplicate != null)
                {
                    ModelState.AddModelError("", "Duplicate Data");
                    InitCreatePage();                  
                    return View();
                }

                //Check mesin sudah selesai dimantenance atau belum
                var CekMesin = db.tr_shift_leader_note.Where(x => x.dept_id == FrmData.dept_id && x.mc_id == FrmData.mc_id && x.block == block && x.loom_no == loom && x.stop_to.HasValue == false);
                if (CekMesin.Count() > 0)
                {
                    ModelState.AddModelError("machine_no", "Proses maintenance mesin " + CekMesin.FirstOrDefault().block.ToString() + "." + CekMesin.FirstOrDefault().loom_no + " sebelumnya belum diselesaikan");
                    InitCreatePage();              
                    return View();
                }
                using (DbContextTransaction trans = db.Database.BeginTransaction())
                {
                    try
                    {
                        //throw new System.Exception("Tes error" + Environment.NewLine);

                        foreach (var data in FrmData.ShiftLeaderNotesDetails)
                        {
                            tr_shift_leader_note_detail dataDl = new tr_shift_leader_note_detail();
                            dataDl.dept_id = FrmData.dept_id;
                            dataDl.mc_id = FrmData.mc_id;
                            dataDl.note_date = note_date;
                            dataDl.shift = FrmData.shift;
                            dataDl.block = par[0];
                            dataDl.loom_no = par[1];
                            dataDl.mtc_id = data.mtc_id.ToString().Trim();
                            dataDl.trouble_id = data.trouble_id.ToString().Trim();
                            dataDl.proc_time = DateTime.Now;
                            db.tr_shift_leader_note_detail.Add(dataDl);
                            db.SaveChanges();
                        }


                        tr_shift_leader_note dataHeader = new tr_shift_leader_note();
                        dataHeader.dept_id = FrmData.dept_id;
                        dataHeader.mc_id = FrmData.mc_id;
                        dataHeader.note_date = note_date;
                        dataHeader.shift = FrmData.shift;
                        dataHeader.block = par[0];
                        dataHeader.loom_no = par[1];
                        dataHeader.stop_from = stop_from;
                        dataHeader.stop_to = FrmData.stop_to;
                        dataHeader.pic = FrmData.pic;
                        dataHeader.user_id = User.Identity.Name.ToUpper();
                        dataHeader.rec_sts = "A";
                        dataHeader.proc_no = 1;
                        dataHeader.client_ip = GetIPAddress();
                        dataHeader.proc_time = DateTime.Now;

                        db.tr_shift_leader_note.Add(dataHeader);
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
                            throw new System.Exception("Rollback Issue - " + InnerEx.Message.ToString() + " " + Environment.NewLine);
                        }
                        ModelState.AddModelError("", "Failed Transaction because " + ex.Message.ToString());
                        InitCreatePage();                   
                        return View(FrmData);
                    }
                }

                return RedirectToAction("Index", new { id = FrmData.dept_id + ";" + FrmData.mc_id + ";" + note_date.ToString("dd-MM-yyyy") + ";" + FrmData.shift + ";" + block + ";" + loom });
                //return RedirectToAction("index");
            }

            InitCreatePage();
            return View();
        }

        [Authorize]
        public ActionResult Edit(string id)
        {
            InitializeUserAkses();
            var access_button = db.ms_user_access_button.Where(x => x.userid == User.Identity.Name.ToString().Trim() && x.menu == "TRANSACTION" && x.submenu.ToString().Trim() == "Machine Quality Trouble/Shift Leader Notes");
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

            string dept_code_cim = db.sysuser_app.Where(x => x.ID == User.Identity.Name).Select(x => x.departement).FirstOrDefault();
            string dept_name_cim = db.departement_master.Where(x => x.Dept_ID == dept_code_cim).Select(x => x.Dept_Desc).FirstOrDefault().ToUpper();
            decimal dept_id_system = db.ms_dept.Where(x => x.dept_name == dept_name_cim).Select(x => x.dept_id).FirstOrDefault();

            //ViewBag.MachineNo = new SelectList(db.v_Machine_Master_AJL, "MachineNo", "MachineNo");
            //ViewBag.MachineTrouble = db.v_Machine_Trouble.Where(x => x.dept_id == dept_id_system);

            ViewBag.TroubleItem = db.v_Maintenance_Type_for_Maintenance_Action.OrderBy(x => x.trouble_name).AsNoTracking();
            ViewBag.MaintenanceType = new SelectList(db.ms_maintenance.Where(x => x.mtc_id != "MTC30"), "mtc_id", "mtc_name", "MTC10");

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

            tr_maintenance cari = db.tr_maintenance.Find(deptid, mcid, note_date, shift, block, loom);

            if (cari != null)
            {
                ViewBag.Hide = "True";

                var _get_roles = db.v_user_ism_maintenance.Where(x => x.ID == User.Identity.Name.ToUpper().Trim());
                string _roles = "";

                if (_get_roles != null)
                {
                    _roles = _get_roles.FirstOrDefault().roles.Trim();
                }

                if (_roles == "ADMIN")
                {
                    ViewBag.pic = new SelectList(db.sysuser_app.Where(x => x.ID == User.Identity.Name.ToUpper().Trim()).OrderBy(x => x.Fullname), "ID", "Fullname", User.Identity.Name.ToUpper());
                }
                else
                {
                    //ViewBag.pic = new SelectList(db.sysuser_app.Where(x => x.departement == "W" && x.section == "pa" & x.ID == User.Identity.Name.ToUpper()).OrderBy(x => x.Fullname), "ID", "Fullname", User.Identity.Name.ToUpper()); ViewBag.pic = new SelectList(db.sysuser_app.Where(x => x.departement == "W" && x.section == "pa" & x.ID == User.Identity.Name.ToUpper()).OrderBy(x => x.Fullname), "ID", "Fullname", User.Identity.Name.ToUpper());
                    ViewBag.pic = new SelectList(db.sysuser_app.Where(x => x.ID == User.Identity.Name.ToUpper()).OrderBy(x => x.Fullname), "ID", "Fullname", User.Identity.Name.ToUpper()); ViewBag.pic = new SelectList(db.sysuser_app.Where(x => x.ID == User.Identity.Name.ToUpper()).OrderBy(x => x.Fullname), "ID", "Fullname", User.Identity.Name.ToUpper());
                }

                ModelState.AddModelError("", "Data tidak bisa dirubah, Catatan pimpinan shift sudah ada di maintenance history");
                return View();
            }

            var query = from data in db.v_Shift_Leader
                        where data.dept_id == deptid && data.note_date == note_date && data.shift == shift && data.block == block && data.loom_no == loom
                        select new ShiftLeaderNotes
                        {
                            dept_id = data.dept_id,
                            mc_id = data.mc_id,
                            note_date = data.note_date,
                            shift = data.shift,
                            pic = data.pic,
                            PicName = data.Fullname,
                            dept_name = data.dept_name,
                            mc_name = data.mc_name,
                            block = data.block,
                            loom_no = data.loom_no,
                            machine_no = data.block + "." + data.loom_no,
                            stop_from = data.stop_from,
                            stop_to = (DateTime)data.stop_to,
                            //stop_from_str = data.stop_from.ToString("dd-MM-yyyy HH:mm")
                        };


            ShiftLeaderNotes obj = new ShiftLeaderNotes();
            obj.dept_id = ReplaceNullValueNumeric(query.Select(x => x.dept_id).FirstOrDefault());
            obj.mc_id = ReplaceNullValueNumeric(query.Select(x => x.mc_id).FirstOrDefault());
            obj.note_date = query.Select(x => x.note_date).FirstOrDefault();
            obj.shift = ReplaceNullValueString(query.Select(x => x.shift).FirstOrDefault());
            obj.pic = ReplaceNullValueString(query.Select(x => x.pic).FirstOrDefault());
            obj.dept_name = ReplaceNullValueString(query.Select(x => x.dept_name).FirstOrDefault());
            obj.mc_name = ReplaceNullValueString(query.Select(x => x.mc_name).FirstOrDefault());
            obj.PicName = ReplaceNullValueString(query.Select(x => x.PicName).FirstOrDefault());
            obj.block = ReplaceNullValueString(query.Select(x => x.block).FirstOrDefault());
            obj.loom_no = ReplaceNullValueString(query.Select(x => x.loom_no).FirstOrDefault());
            obj.stop_from = query.Select(x => x.stop_from).FirstOrDefault();
            obj.stop_to = query.Select(x => x.stop_to).FirstOrDefault();
            obj.machine_no = ReplaceNullValueString(query.Select(x => x.machine_no).FirstOrDefault());
            //obj.stop_from_str = query.Select(x => x.stop_from_str).FirstOrDefault();
            DateTime _stop_from = db.v_Shift_Leader.Where(x => x.dept_id == deptid && x.note_date == note_date && x.shift == shift && x.block == block && x.loom_no == loom).FirstOrDefault().stop_from;
            obj.stop_from_str = _stop_from.ToString("HH:mm");

            IQueryable<ShiftLeaderNotesDetail> obj2 = from data in db.v_Shift_Leader
                                                      where data.dept_id == deptid && data.note_date == note_date && data.shift == shift && data.block == block && data.loom_no == loom
                                                      select new ShiftLeaderNotesDetail()
                                                      {
                                                          dept_id = data.dept_id,
                                                          mc_id = data.mc_id,
                                                          note_date = data.note_date,
                                                          shift = data.shift,
                                                          block = data.block,
                                                          loom_no = data.loom_no,
                                                          trouble_id = data.trouble_id,
                                                          trouble_name = data.trouble_name,
                                                          mtc_name = data.mtc_name,
                                                          mtc_id = data.mtc_id
                                                      };

            obj.ShiftLeaderNotesDetails = obj2.AsNoTracking().ToList();

            var get_roles = db.v_user_ism_maintenance.Where(x => x.ID == User.Identity.Name.ToUpper().Trim());
            string roles = "";

            if (get_roles != null)
            {
                roles = get_roles.FirstOrDefault().roles.Trim();
            }

            if (roles == "ADMIN")
            {
                ViewBag.pic = new SelectList(db.sysuser_app.Where(x => x.ID == User.Identity.Name.ToUpper().Trim()).OrderBy(x => x.Fullname), "ID", "Fullname", User.Identity.Name.ToUpper());
            }
            else
            {
                //ViewBag.pic = new SelectList(db.sysuser_app.Where(x => x.departement == "W" && x.section == "pa" & x.ID == User.Identity.Name.ToUpper()).OrderBy(x => x.Fullname), "ID", "Fullname", User.Identity.Name.ToUpper());
                ViewBag.pic = new SelectList(db.sysuser_app.Where(x => x.ID == User.Identity.Name.ToUpper()).OrderBy(x => x.Fullname), "ID", "Fullname", User.Identity.Name.ToUpper());                
            }
            return View(obj);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Edit_add_remove_detail(ShiftLeaderNotes FrmData)
        {
            InitializeUserAkses();

            var stop_from = DateTime.Now;

            try
            {

                //string[] stop_from_input = FrmData.stop_from_str.Trim().Split(' ');
                //string[] date_stop_from = stop_from_input[0].Split('-');
                //string[] time_stop_from = stop_from_input[1].Split(':');
                //stop_from = new DateTime(Convert.ToInt16(date_stop_from[2]), Convert.ToInt16(date_stop_from[1]), Convert.ToInt16(date_stop_from[0]), Convert.ToInt16(time_stop_from[0]), Convert.ToInt16(time_stop_from[1]), 0);

                string[] date_stop_from = FrmData.note_date.ToString("yyyy-MM-dd").Split('-');
                string[] time_stop_from = FrmData.stop_from_str.Trim().Split(':');
                stop_from = new DateTime(FrmData.note_date.Year, FrmData.note_date.Month, FrmData.note_date.Day, Convert.ToInt16(time_stop_from[0]), Convert.ToInt16(time_stop_from[1]), 0);
            }
            catch (Exception ExDatetime)
            {
                ModelState.AddModelError("", "Datetime Issue - " + ExDatetime.ToString());
                InitCreatePage();
                return View();
            }

            using (DbContextTransaction trans = db.Database.BeginTransaction())
            {
                try
                {
                    var Header = db.tr_shift_leader_note.SingleOrDefault(x => x.dept_id == FrmData.dept_id && x.mc_id == FrmData.mc_id && x.note_date == FrmData.note_date && x.shift == FrmData.shift && x.block == FrmData.block && x.loom_no == FrmData.loom_no);

                    tr_shift_leader_note_hist History = new tr_shift_leader_note_hist();
                    History.dept_id = Header.dept_id; History.mc_id = Header.mc_id; History.note_date = Header.note_date; History.shift = Header.shift; History.block = Header.block; History.loom_no = Header.loom_no;
                    History.stop_from = Header.stop_from; History.stop_to = Header.stop_to; History.pic = Header.pic; History.user_id = Header.user_id; History.rec_sts = Header.rec_sts;
                    History.proc_no = Header.proc_no; History.client_ip = Header.client_ip; History.proc_time = Header.proc_time;
                    History.proc_time_edit = DateTime.Now; History.user_id_edit = User.Identity.Name.ToUpper().Trim();

                    db.tr_shift_leader_note_hist.Add(History);
                    db.SaveChanges();

                    var ShiftleaderNotesDetail = db.tr_shift_leader_note_detail.Where(x => x.dept_id == FrmData.dept_id && x.mc_id == FrmData.mc_id && x.note_date == FrmData.note_date && x.shift == FrmData.shift.ToString().Trim() && x.block == FrmData.block && x.loom_no == FrmData.loom_no);
                    foreach (var item in ShiftleaderNotesDetail)
                    {
                        if (item.mtc_id == null)
                        {
                            continue;
                        }

                        tr_shift_leader_note_detail_hist HistoryDetail = new tr_shift_leader_note_detail_hist();
                        HistoryDetail.dept_id = item.dept_id; HistoryDetail.mc_id = item.mc_id; HistoryDetail.note_date = item.note_date;
                        HistoryDetail.shift = item.shift; HistoryDetail.block = item.block; HistoryDetail.loom_no = item.loom_no;
                        HistoryDetail.mtc_id = item.mtc_id; HistoryDetail.trouble_id = item.trouble_id; HistoryDetail.proc_time = item.proc_time;
                        HistoryDetail.proc_time_edit = DateTime.Now; HistoryDetail.user_id_edit = User.Identity.Name.ToUpper().Trim();

                        db.tr_shift_leader_note_detail_hist.Add(HistoryDetail);
                        db.SaveChanges();
                    }

                    Header.pic = FrmData.pic.ToString().Trim();
                    Header.stop_from = stop_from;
                    Header.proc_no = Header.proc_no + 1;
                    Header.proc_time = DateTime.Now;
                    Header.user_id = User.Identity.Name.ToString().Trim().ToUpper();
                    Header.rec_sts = "T";
                    Header.client_ip = GetIPAddress();
                    db.SaveChanges();

                    db.tr_shift_leader_note_detail.RemoveRange(db.tr_shift_leader_note_detail.Where(x => x.dept_id == FrmData.dept_id && x.mc_id == FrmData.mc_id && x.note_date == FrmData.note_date && x.shift == FrmData.shift.ToString().Trim() && x.block == FrmData.block && x.loom_no == FrmData.loom_no));
                    db.SaveChanges();

                    foreach (var item in FrmData.ShiftLeaderNotesDetails)
                    {
                        if (item.mtc_id == null)
                        {
                            continue;
                        }

                        tr_shift_leader_note_detail detailNotesleader = new tr_shift_leader_note_detail();
                        detailNotesleader.dept_id = FrmData.dept_id;
                        detailNotesleader.mc_id = FrmData.mc_id;
                        detailNotesleader.note_date = FrmData.note_date;
                        detailNotesleader.shift = FrmData.shift;
                        detailNotesleader.block = FrmData.block.ToString().Trim();
                        detailNotesleader.loom_no = FrmData.loom_no.ToString().Trim();
                        detailNotesleader.mtc_id = item.mtc_id.ToString();
                        detailNotesleader.trouble_id = item.trouble_id.ToString().Trim();
                        detailNotesleader.proc_time = DateTime.Now;
                        db.tr_shift_leader_note_detail.Add(detailNotesleader);
                        db.SaveChanges();
                    }

                    trans.Commit();
                }
                catch (Exception Ex)
                {
                    try
                    {
                        trans.Rollback();
                    }
                    catch (Exception InnerEx)
                    {
                        throw new System.Exception("Rollback Issue - " + InnerEx.Message.ToString() + " " + Environment.NewLine);
                    }
                    ModelState.AddModelError("", "Failed Transaction because " + Ex.Message.ToString());
                    return View(FrmData);
                }

            }

            return RedirectToAction("Index", new { id = FrmData.dept_id + ";" + FrmData.mc_id + ";" + FrmData.note_date.ToString("dd-MM-yyyy") + ";" + FrmData.shift + ";" + FrmData.block + ";" + FrmData.loom_no });
            //return RedirectToAction("index");
        }

        [Authorize]
        //GET
        public ActionResult Delete(string id)
        {
            InitializeUserAkses();
            var access_button = db.ms_user_access_button.Where(x => x.userid == User.Identity.Name.ToString().Trim() && x.menu == "TRANSACTION" && x.submenu.ToString().Trim() == "Machine Quality Trouble/Shift Leader Notes");
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
                return RedirectToAction("index");
            }

            decimal deptid, mcid;
            DateTime note_date;
            string shift, block, loom;

            string dept_code_cim = db.sysuser_app.Where(x => x.ID == User.Identity.Name).Select(x => x.departement).FirstOrDefault();
            string dept_name_cim = db.departement_master.Where(x => x.Dept_ID == dept_code_cim).Select(x => x.Dept_Desc).FirstOrDefault().ToUpper();
            decimal dept_id_system = db.ms_dept.Where(x => x.dept_name == dept_name_cim).Select(x => x.dept_id).FirstOrDefault();

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

            tr_maintenance cari = db.tr_maintenance.Find(deptid, mcid, note_date, shift, block, loom);
            if (cari != null)
            {
                ViewBag.Hide = "True";
                ModelState.AddModelError("", "Data tidak bisa dihapus, Catatan pimpinan shift sudah ada di maintenance history");
                return View();
            }

            var query = from data in db.v_Shift_Leader
                        where data.dept_id == deptid && data.note_date == note_date && data.shift == shift
                        select new ShiftLeaderNotes
                        {
                            dept_id = data.dept_id,
                            mc_id = data.mc_id,
                            note_date = data.note_date,
                            shift = data.shift,
                            pic = data.pic,
                            PicName = data.Fullname,
                            dept_name = data.dept_name,
                            mc_name = data.mc_name,
                            block = data.block,
                            loom_no = data.loom_no,
                            machine_no = data.block + "." + data.loom_no,
                            stop_from = data.stop_from,
                            stop_to = (DateTime)data.stop_to
                        };

            ShiftLeaderNotes obj = new ShiftLeaderNotes();
            obj.dept_id = ReplaceNullValueNumeric(query.Select(x => x.dept_id).FirstOrDefault());
            obj.mc_id = ReplaceNullValueNumeric(query.Select(x => x.mc_id).FirstOrDefault());
            obj.note_date = query.Select(x => x.note_date).FirstOrDefault();
            obj.shift = ReplaceNullValueString(query.Select(x => x.shift).FirstOrDefault());
            obj.pic = ReplaceNullValueString(query.Select(x => x.pic).FirstOrDefault());
            obj.dept_name = ReplaceNullValueString(query.Select(x => x.dept_name).FirstOrDefault());
            obj.mc_name = ReplaceNullValueString(query.Select(x => x.mc_name).FirstOrDefault());
            obj.PicName = ReplaceNullValueString(query.Select(x => x.PicName).FirstOrDefault());
            obj.block = ReplaceNullValueString(query.Select(x => x.block).FirstOrDefault());
            obj.loom_no = ReplaceNullValueString(query.Select(x => x.loom_no).FirstOrDefault());
            obj.stop_from = query.Select(x => x.stop_from).FirstOrDefault();
            obj.stop_to = query.Select(x => x.stop_to).FirstOrDefault();
            obj.machine_no = ReplaceNullValueString(query.Select(x => x.machine_no).FirstOrDefault());

            IQueryable<ShiftLeaderNotesDetail> obj2 = from data in db.v_Shift_Leader
                                                      where data.dept_id == deptid && data.note_date == note_date && data.shift == shift
                                                      select new ShiftLeaderNotesDetail()
                                                      {
                                                          dept_id = data.dept_id,
                                                          mc_id = data.mc_id,
                                                          note_date = data.note_date,
                                                          shift = data.shift,
                                                          block = data.block,
                                                          loom_no = data.loom_no,
                                                          trouble_id = data.trouble_id,
                                                          trouble_name = data.trouble_name,
                                                          mtc_id = data.mtc_id,
                                                          mtc_name = data.mtc_name

                                                      };

            obj.ShiftLeaderNotesDetails = obj2.ToList();

            if (obj == null)
            {
                return HttpNotFound();
            }

            return View(obj);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
        {
            InitializeUserAkses();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            decimal deptid, mcid;
            DateTime note_date;
            string shift, block, loom;

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


            using (DbContextTransaction trans = db.Database.BeginTransaction())
            {
                try
                {
                    //throw new System.Exception("test delete");

                    tr_shift_leader_note Header = db.tr_shift_leader_note.Find(deptid, mcid, note_date, shift, block, loom);

                    tr_shift_leader_note_hist History = new tr_shift_leader_note_hist();
                    History.dept_id = Header.dept_id; History.mc_id = Header.mc_id; History.note_date = Header.note_date;
                    History.shift = Header.shift; History.block = Header.block; History.loom_no = Header.loom_no;
                    History.stop_from = Header.stop_from; History.stop_to = Header.stop_to; History.pic = Header.pic;
                    History.user_id = Header.user_id; History.rec_sts = Header.rec_sts; History.proc_no = Header.proc_no;
                    History.client_ip = Header.client_ip; History.proc_time = Header.proc_time;
                    History.user_id_del = User.Identity.Name.ToUpper().Trim();
                    History.proc_time_del = DateTime.Now;

                    db.tr_shift_leader_note_hist.Add(History);
                    db.SaveChanges();

                    var Detail = db.tr_shift_leader_note_detail.Where(x => x.dept_id == deptid && x.mc_id == mcid &&
                                                                x.note_date == note_date && x.shift == shift.ToString().Trim() && x.block == block && x.loom_no == loom);
                    if (Detail.Count() > 0)
                    {
                        foreach (var item in Detail)
                        {
                            tr_shift_leader_note_detail_hist DetailHistory = new tr_shift_leader_note_detail_hist();
                            DetailHistory.dept_id = item.dept_id; DetailHistory.mc_id = item.mc_id; DetailHistory.note_date = item.note_date; DetailHistory.shift = item.shift;
                            DetailHistory.block = item.block; DetailHistory.loom_no = item.loom_no; DetailHistory.mtc_id = item.mtc_id; DetailHistory.trouble_id = item.trouble_id;
                            DetailHistory.proc_time = item.proc_time; DetailHistory.proc_time_del = DateTime.Now; DetailHistory.user_id_del = User.Identity.Name.ToUpper().Trim();
                            db.tr_shift_leader_note_detail_hist.Add(DetailHistory);
                            db.SaveChanges();
                        }

                    }

                    db.tr_shift_leader_note.Remove(Header);
                    db.SaveChanges();

                    db.tr_shift_leader_note_detail.RemoveRange(db.tr_shift_leader_note_detail.Where(x => x.dept_id == deptid && x.mc_id == mcid &&
                                                                x.note_date == note_date && x.shift == shift.ToString().Trim() && x.block == block && x.loom_no == loom));
                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception Ex)
                {
                    try
                    {
                        trans.Rollback();
                    }

                    catch (Exception InnerEx)
                    {
                        throw new System.Exception("Rollback Issue - " + InnerEx.Message.ToString() + " " + Environment.NewLine);
                    }
                    ModelState.AddModelError("", "Failed Transaction because " + Ex.Message.ToString());

                    var query = from data in db.v_Shift_Leader
                                where data.dept_id == deptid && data.note_date == note_date && data.shift == shift
                                select new ShiftLeaderNotes
                                {
                                    dept_id = data.dept_id,
                                    mc_id = data.mc_id,
                                    note_date = data.note_date,
                                    shift = data.shift,
                                    pic = data.pic,
                                    PicName = data.Fullname,
                                    dept_name = data.dept_name,
                                    mc_name = data.mc_name,
                                    block = data.block,
                                    loom_no = data.loom_no,
                                    machine_no = data.block + "." + data.loom_no,
                                    stop_from = data.stop_from,
                                    stop_to = (DateTime)data.stop_to
                                };

                    ShiftLeaderNotes obj = new ShiftLeaderNotes();
                    obj.dept_id = ReplaceNullValueNumeric(query.Select(x => x.dept_id).FirstOrDefault());
                    obj.mc_id = ReplaceNullValueNumeric(query.Select(x => x.mc_id).FirstOrDefault());
                    obj.note_date = query.Select(x => x.note_date).FirstOrDefault();
                    obj.shift = ReplaceNullValueString(query.Select(x => x.shift).FirstOrDefault());
                    obj.pic = ReplaceNullValueString(query.Select(x => x.pic).FirstOrDefault());
                    obj.dept_name = ReplaceNullValueString(query.Select(x => x.dept_name).FirstOrDefault());
                    obj.mc_name = ReplaceNullValueString(query.Select(x => x.mc_name).FirstOrDefault());
                    obj.PicName = ReplaceNullValueString(query.Select(x => x.PicName).FirstOrDefault());
                    obj.block = ReplaceNullValueString(query.Select(x => x.block).FirstOrDefault());
                    obj.loom_no = ReplaceNullValueString(query.Select(x => x.loom_no).FirstOrDefault());
                    obj.stop_from = query.Select(x => x.stop_from).FirstOrDefault();
                    obj.stop_to = query.Select(x => x.stop_to).FirstOrDefault();
                    obj.machine_no = ReplaceNullValueString(query.Select(x => x.machine_no).FirstOrDefault());

                    IQueryable<ShiftLeaderNotesDetail> obj2 = from data in db.v_Shift_Leader
                                                              where data.dept_id == deptid && data.note_date == note_date && data.shift == shift
                                                              select new ShiftLeaderNotesDetail()
                                                              {
                                                                  dept_id = data.dept_id,
                                                                  mc_id = data.mc_id,
                                                                  note_date = data.note_date,
                                                                  shift = data.shift,
                                                                  block = data.block,
                                                                  loom_no = data.loom_no,
                                                                  trouble_id = data.trouble_id,
                                                                  trouble_name = data.trouble_name,
                                                                  mtc_id = data.mtc_id,
                                                                  mtc_name = data.mtc_name

                                                              };

                    obj.ShiftLeaderNotesDetails = obj2.ToList();

                    return View(obj);
                }
            }

            return RedirectToAction("Index");
        }




    }
}
