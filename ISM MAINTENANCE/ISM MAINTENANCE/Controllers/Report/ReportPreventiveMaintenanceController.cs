using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ISM_MAINTENANCE.Models.ViewModel.Report;
using ISM_MAINTENANCE.Models.DB;
using System.Text;

namespace ISM_MAINTENANCE.Controllers.Report
{
    public class ReportPreventiveMaintenanceController : Controller
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
        // GET: ReportPreventiveMaintenance
        public ActionResult Index()
        {
            InitializeUserAkses();

            string dept_code_cim = db.sysuser_app.Where(x => x.ID == User.Identity.Name).Select(x => x.departement).FirstOrDefault();
            string dept_name_cim = db.departement_master.Where(x => x.Dept_ID == dept_code_cim).Select(x => x.Dept_Desc).FirstOrDefault().ToUpper();
            decimal dept_id_system = db.ms_dept.Where(x => x.dept_name == dept_name_cim).Select(x => x.dept_id).FirstOrDefault();

            ViewBag.dept_par = new SelectList(db.ms_dept.Where(x => x.dept_id == dept_id_system), "dept_id", "dept_name", dept_id_system);
            ViewBag.mc_id_par = new SelectList(db.ms_machine_type.Where(x => x.dept_id == dept_id_system && x.mc_id == 70), "mc_id", "mc_name", 70);
            ViewBag.machine_par = new SelectList(db.v_Machine_Master_AJL, "MachineNo", "MachineNo");
            ViewBag.PIC_Mtc = new SelectList(db.sysuser_app.Where(x => x.departement == "w" && x.section == "mt"), "ID", "Fullname");
            FormParameterPrev obj = new FormParameterPrev();
            return View(obj);



        }
        public ActionResult Preview()
        {
            return RedirectToAction("index");
        }

        [HttpPost]
        public ActionResult Preview(FormParameterPrev FrmData)
        {
            InitializeUserAkses();
            decimal deptid = 0, mcid = 0;
            DateTime stop_from1 = DateTime.Now;
            DateTime stop_from2 = DateTime.Now;
            string pic_mtc, mtc_name, machineno;


            deptid = FrmData.dept_par;
            mcid = FrmData.mc_id_par;
            pic_mtc = FrmData.PIC_Mtc == "" || FrmData.PIC_Mtc == null ? "ALL" : FrmData.PIC_Mtc;
            //pic_leader = FrmData["PIC_Shift_Leader"] == "" || FrmData["PIC_Mtc"] == null ? "ALL" : FrmData["PIC_Mtc"];
            mtc_name = "PREVENTIVE";
            machineno = FrmData.machine_par == null || FrmData.machine_par == "" ? "ALL" : FrmData.machine_par;

            //stop_from1 = FrmData.start_date_par;
            //stop_from2 = FrmData.stop_date_par;
          

            if (FrmData.start_date_par == null)
            {
                ModelState.AddModelError("start_date_par", "Tanggal Awal tidak boleh kosong");
                ViewBag.dept_par = new SelectList(db.ms_dept.Where(x => x.dept_id == deptid), "dept_id", "dept_name", deptid);
                ViewBag.mc_id_par = new SelectList(db.ms_machine_type.Where(x => x.dept_id == deptid && x.mc_id == 70), "mc_id", "mc_name", 70);
                ViewBag.machine_par = new SelectList(db.v_Machine_Master_AJL, "MachineNo", "MachineNo");
                ViewBag.PIC_Mtc = new SelectList(db.sysuser_app.Where(x => x.departement == "w" && x.section == "mt"), "ID", "Fullname");
                FormParameterPrev obj1 = new FormParameterPrev();
                return View(obj1);
            }
            if (FrmData.stop_date_par == null)
            {
                ModelState.AddModelError("stop_date_par", "Tanggal Akhir tidak boleh kosong");
                ViewBag.dept_par = new SelectList(db.ms_dept.Where(x => x.dept_id == deptid), "dept_id", "dept_name", deptid);
                ViewBag.mc_id_par = new SelectList(db.ms_machine_type.Where(x => x.dept_id == deptid && x.mc_id == 70), "mc_id", "mc_name", 70);
                ViewBag.machine_par = new SelectList(db.v_Machine_Master_AJL, "MachineNo", "MachineNo");
                ViewBag.PIC_Mtc = new SelectList(db.sysuser_app.Where(x => x.departement == "w" && x.section == "mt"), "ID", "Fullname");
                FormParameterPrev obj2 = new FormParameterPrev();
                return View(obj2);
            }

            string[] tanggal1 = FrmData.start_date_par.Split('-');
            string[] tanggal2 = FrmData.stop_date_par.Split('-');

            stop_from1 = new DateTime(Convert.ToInt16(tanggal1[2]), Convert.ToInt16(tanggal1[1]), Convert.ToInt16(tanggal1[0]));
            stop_from2 = new DateTime(Convert.ToInt16(tanggal2[2]), Convert.ToInt16(tanggal2[1]), Convert.ToInt16(tanggal2[0]));

            StringBuilder query = new StringBuilder();
            query.Append("exec wppc.rpt_prev_maintenance ");
            query.Append(deptid.ToString());
            query.Append("," + mcid + " , ");
            query.Append("'" + String.Format("{0:yyyy-MM-dd}", stop_from1) + "',");
            query.Append("'" + String.Format("{0:yyyy-MM-dd}", stop_from2) + "',");
            query.Append("'" + mtc_name + "', ");
            query.Append("'" + pic_mtc + "', ");
            query.Append("'" + machineno + "'");



            var result = db.Database.SqlQuery<PreventiveMaintenance>(query.ToString());
            FormParameterPrev obj = new FormParameterPrev();
            obj.dept_par = deptid;
            obj.mc_id_par = mcid;
            //obj.start_date_par = stop_from1;
            //obj.start_date_par = stop_from2;
            obj.start_date_par = FrmData.start_date_par;
            obj.start_date_par = FrmData.stop_date_par;
            obj.Report_Data = result.OrderBy(c => c.mtc_schedule).ThenBy(c => c.mtc_action_id).ThenBy(c => c.RowNumber).ToList();

            string dept_code_cim = db.sysuser_app.Where(x => x.ID == User.Identity.Name).Select(x => x.departement).FirstOrDefault();
            string dept_name_cim = db.departement_master.Where(x => x.Dept_ID == dept_code_cim).Select(x => x.Dept_Desc).FirstOrDefault().ToUpper();
            decimal dept_id_system = db.ms_dept.Where(x => x.dept_name == dept_name_cim).Select(x => x.dept_id).FirstOrDefault();

            ViewBag.dept_par = new SelectList(db.ms_dept.Where(x => x.dept_id == dept_id_system), "dept_id", "dept_name", dept_id_system);
            ViewBag.mc_id_par = new SelectList(db.ms_machine_type.Where(x => x.dept_id == dept_id_system & x.mc_id == 70), "mc_id", "mc_name", 70);
            ViewBag.machine_par = new SelectList(db.v_Machine_Master_AJL, "MachineNo", "MachineNo");
            ViewBag.PIC_Mtc = new SelectList(db.sysuser_app.Where(x => x.departement == "w" && x.section == "mt"), "ID", "Fullname");
            return View(obj);
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