using ISM_MAINTENANCE.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ISM_MAINTENANCE.Models.ViewModel.Report;
using System.Text;

namespace ISM_MAINTENANCE.Controllers.Report
{
    [Authorize]
    public class ReportMachineQualityTroubleController : Controller
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


        private List<MachineQualityTrouble> ReportData(string mtc_id, DateTime stop_from1, DateTime stop_from2)
        {
            //var result = db.Database.SqlQuery<MachineQualityTrouble>(query.ToString()).ToList();

            var maintenance_sparepart = db.Database.SqlQuery<SPAREPART>("select * from wppc.v_rpt_sparepart_row").ToList();
            var maintenance_action = db.Database.SqlQuery<MAINTENANCE_ACTION>("select * from wppc.v_rpt_maintenance_action_row where mtc_id = '" + mtc_id.ToString().Trim() + "'").ToList();
            var maintenance_trouble = db.Database.SqlQuery<MAINTENANCE_TROUBLE>("select * from wppc.v_rpt_maintenance_trouble_row where mtc_id = '" + mtc_id.ToString().Trim() + "'").ToList();

            var maintenance_maxrow = db.Database.SqlQuery<MAXROW_MAINTENANCE>("select * from wppc.v_rpt_maintenance_max_row where  CAST(CONVERT(VARCHAR,stop_from,110) AS DATE)  " +
                "between '" + String.Format("{0:yyyy-MM-dd}", stop_from1) + "' and '" + String.Format("{0:yyyy-MM-dd}", stop_from2) + "' and mtc_id = '" + mtc_id.ToString().Trim() + "'").ToList();
            List<MachineQualityTrouble> obj = new List<MachineQualityTrouble>();
            int counter = 1;
            int RowNumber = 1;

            //List<string> TroubleName = new List<string>();
            //List<string> ActionName = new List<string>();

            foreach (var item in maintenance_maxrow)
            {


                for (int i = 1; i <= item.Maxrow; i++)
                {
                    MachineQualityTrouble Dt_mtc = new MachineQualityTrouble();

                    Dt_mtc.RowNo = RowNumber;
                    if (i > 1)
                    {
                        Dt_mtc.No = null;
                    }
                    else
                    {
                        Dt_mtc.No = counter;                       
                    }

                    var trouble_item = maintenance_trouble.Where(x => x.dept_id == item.dept_id && x.mc_id == item.mc_id && x.note_date == item.note_date && x.block == item.block & x.loom_no == item.loom_no && x.row == i).FirstOrDefault(); ;
                    if (trouble_item != null)
                    {

                        if (i > 1)
                        {
                            Dt_mtc.trouble_id = trouble_item.trouble_id;
                            Dt_mtc.trouble_name = db.v_Maintenance_Action.Where(x => x.mtc_id != "MTC30" && x.dept_id == item.dept_id && x.mc_id == item.mc_id && x.mtc_id == item.mtc_id && x.trouble_id == trouble_item.trouble_id).FirstOrDefault().trouble_name;
                            Dt_mtc.No = null;
                            Dt_mtc.dept_id = null;
                            Dt_mtc.mc_id = null;
                            Dt_mtc.mc_name = null;
                            Dt_mtc.block = null;
                            Dt_mtc.loom_no = null;
                            Dt_mtc.mc_name = null;
                            Dt_mtc.note_date = null;
                            Dt_mtc.shift = null;
                            Dt_mtc.Pic_Maintenance = null;
                            Dt_mtc.mtc_id = null;
                            Dt_mtc.mtc_name = null;
                            Dt_mtc.mtc_start = null;
                            Dt_mtc.mtc_finish = null;
                            Dt_mtc.Pic_LeaderShift = null;
                            Dt_mtc.Pic_LeaderShift_Name = null;
                            Dt_mtc.Pic_Maintenance = null;
                            Dt_mtc.Pic_Maintenance_Name = null;
                            Dt_mtc.stop_from = null;
                        }
                        else
                        {
                            Dt_mtc.dept_id = item.dept_id;
                            Dt_mtc.mc_id = item.mc_id;
                            Dt_mtc.block = item.block;
                            Dt_mtc.loom_no = item.loom_no;
                            Dt_mtc.mc_name = db.ms_machine_type.Where(x => x.dept_id == item.dept_id && x.mc_id == item.mc_id).Select(x => x.mc_name).FirstOrDefault();
                            Dt_mtc.note_date = item.note_date;
                            Dt_mtc.shift = item.shift;
                            Dt_mtc.mtc_id = item.mtc_id;
                            Dt_mtc.mtc_name = db.ms_maintenance.Where(x => x.mtc_id == item.mtc_id).FirstOrDefault().mtc_name;

                            Dt_mtc.trouble_id = trouble_item.trouble_id;
                            Dt_mtc.trouble_name = db.v_Maintenance_Action.Where(x => x.mtc_id != "MTC30" && x.dept_id == item.dept_id && x.mc_id == item.mc_id && x.mtc_id == item.mtc_id && x.trouble_id == trouble_item.trouble_id).FirstOrDefault().trouble_name;
                        }

                    }

                    else
                    {
                        Dt_mtc.trouble_id = "";
                        Dt_mtc.trouble_name = "";
                        Dt_mtc.No = null;
                        Dt_mtc.dept_id = null;
                        Dt_mtc.mc_id = null;
                        Dt_mtc.mc_name = null;
                        Dt_mtc.block = null;
                        Dt_mtc.loom_no = null;
                        Dt_mtc.mc_name = null;
                        Dt_mtc.note_date = null;
                        Dt_mtc.shift = null;
                        Dt_mtc.Pic_Maintenance = null;
                        Dt_mtc.mtc_id = null;
                        Dt_mtc.mtc_name = null;
                        Dt_mtc.trouble_id = null;
                        Dt_mtc.trouble_name = null;
                        Dt_mtc.mtc_start = null;
                        Dt_mtc.mtc_finish = null;
                        Dt_mtc.Pic_LeaderShift = null;
                        Dt_mtc.Pic_LeaderShift_Name = null;
                        Dt_mtc.Pic_Maintenance = null;
                        Dt_mtc.Pic_Maintenance_Name = null;
                        Dt_mtc.stop_from = null;
                    }




                    var action_item = maintenance_action.Where(x => x.dept_id == item.dept_id && x.mc_id == item.mc_id && x.note_date == item.note_date && x.block == item.block & x.loom_no == item.loom_no && x.row == i).FirstOrDefault();


                    if (action_item != null)
                    {
                        var LeaderShift = db.tr_shift_leader_note.Where(x => x.dept_id == action_item.dept_id && x.mc_id == action_item.mc_id && x.note_date == action_item.note_date && x.shift == action_item.shift &&
                                                                   x.block == action_item.block && x.loom_no == action_item.loom_no).FirstOrDefault();


                        if (i > 1)
                        {
                            Dt_mtc.mtc_action_id = action_item.mtc_action_id;

                            Dt_mtc.mtc_action_name = db.v_Maintenance_Action.Where(x => x.mtc_id != "MTC30" && x.dept_id == item.dept_id && x.mc_id == item.mc_id && x.mtc_id == item.mtc_id && x.mtc_action_id == action_item.mtc_action_id).FirstOrDefault().mtc_action_name;
                            Dt_mtc.No = null;
                            Dt_mtc.dept_id = null;
                            Dt_mtc.mc_id = null;
                            Dt_mtc.mc_name = null;
                            Dt_mtc.block = null;
                            Dt_mtc.loom_no = null;
                            Dt_mtc.mc_name = null;
                            Dt_mtc.note_date = null;
                            Dt_mtc.shift = null;
                            Dt_mtc.Pic_Maintenance = null;
                            Dt_mtc.mtc_id = null;
                            Dt_mtc.mtc_name = null;
                            Dt_mtc.mtc_start = null;
                            Dt_mtc.mtc_finish = null;
                            Dt_mtc.Pic_LeaderShift = null;
                            Dt_mtc.Pic_LeaderShift_Name = null;
                            Dt_mtc.Pic_Maintenance = null;
                            Dt_mtc.Pic_Maintenance_Name = null;
                            Dt_mtc.stop_from = null;
                        }
                        else
                        {
                            Dt_mtc.mtc_name = db.v_Maintenance_Action.Where(x => x.mtc_id != "MTC30" && x.dept_id == item.dept_id && x.mc_id == item.mc_id && x.mtc_id == item.mtc_id && x.trouble_id == action_item.trouble_id).FirstOrDefault().mtc_name;
                            Dt_mtc.mtc_start = action_item.mtc_start;
                            Dt_mtc.mtc_finish = action_item.mtc_finish;
                            Dt_mtc.Pic_LeaderShift = LeaderShift.pic;
                            Dt_mtc.Pic_LeaderShift_Name = db.sysuser_app.Where(x => x.ID == LeaderShift.pic).FirstOrDefault().Fullname.ToString();
                            Dt_mtc.Pic_Maintenance = action_item.pic_mtc;
                            Dt_mtc.Pic_Maintenance_Name = db.sysuser_app.Where(x => x.ID == action_item.pic_mtc).FirstOrDefault().Fullname.ToString();
                            Dt_mtc.mtc_action_id = action_item.mtc_action_id;
                            Dt_mtc.mtc_action_name = db.v_Maintenance_Action.Where(x => x.mtc_id != "MTC30" && x.dept_id == item.dept_id && x.mc_id == item.mc_id && x.mtc_id == item.mtc_id && x.trouble_id == action_item.trouble_id).FirstOrDefault().mtc_action_name;
                            Dt_mtc.stop_from = LeaderShift.stop_from;
                            Dt_mtc.stop_to = LeaderShift.stop_to;

                        }


                    }
                    else
                    {
                        Dt_mtc.No = null;
                        Dt_mtc.dept_id = null;
                        Dt_mtc.mc_id = null;
                        Dt_mtc.mc_name = null;
                        Dt_mtc.block = null;
                        Dt_mtc.loom_no = null;
                        Dt_mtc.mc_name = null;
                        Dt_mtc.note_date = null;
                        Dt_mtc.shift = null;
                        Dt_mtc.Pic_Maintenance = null;
                        Dt_mtc.mtc_id = null;
                        Dt_mtc.mtc_name = null;
                        Dt_mtc.trouble_id = null;
                        Dt_mtc.trouble_name = null;
                        Dt_mtc.mtc_start = null;
                        Dt_mtc.mtc_finish = null;
                        Dt_mtc.Pic_LeaderShift = null;
                        Dt_mtc.Pic_LeaderShift_Name = null;
                        Dt_mtc.Pic_Maintenance = null;
                        Dt_mtc.Pic_Maintenance_Name = null;
                        Dt_mtc.stop_from = null;

                    }



                    var sparepart_item = maintenance_sparepart.Where(x => x.dept_id == item.dept_id && x.mc_id == item.mc_id && x.note_date == item.note_date && x.block == item.block & x.loom_no == item.loom_no && x.row == i).FirstOrDefault();

                    if (sparepart_item != null)
                    {
                        Dt_mtc.item_code = sparepart_item.ITEM_CODE;
                        Dt_mtc.sparepart_name = sparepart_item.sparepart_name;
                        Dt_mtc.no_catalog = sparepart_item.no_catalog;
                        Dt_mtc.price = sparepart_item.price;
                        Dt_mtc.quantity = sparepart_item.quantity;
                    }
                    else
                    {
                        Dt_mtc.item_code = null;
                        Dt_mtc.price = null;
                        Dt_mtc.quantity = null;
                        Dt_mtc.sparepart_name = null;
                        Dt_mtc.no_catalog = null;
                    }

                    obj.Add(Dt_mtc);
                    RowNumber += 1;
                }


                RowNumber += 1;
                counter += 1;
            }
                     
            return obj;
        }


        public ActionResult Index()
        {
            InitializeUserAkses();
            string dept_code_cim = db.sysuser_app.Where(x => x.ID == User.Identity.Name).Select(x => x.departement).FirstOrDefault();
            string dept_name_cim = db.departement_master.Where(x => x.Dept_ID == dept_code_cim).Select(x => x.Dept_Desc).FirstOrDefault().ToUpper();
            decimal dept_id_system = db.ms_dept.Where(x => x.dept_name == dept_name_cim).Select(x => x.dept_id).FirstOrDefault();

            FormParameter obj = new FormParameter();
            ViewBag.dept_par = new SelectList(db.ms_dept.Where(x => x.dept_id == dept_id_system && x.dept_id == dept_id_system), "dept_id", "dept_name", dept_id_system);
            ViewBag.mc_id_par = new SelectList(db.ms_machine_type.Where(x => x.dept_id == dept_id_system && x.mc_id == 70), "mc_id", "mc_name", 70);
            ViewBag.machine_par = new SelectList(db.v_Machine_Master_AJL, "MachineNo", "MachineNo");
            ViewBag.PIC_Mtc = new SelectList(db.sysuser_app.Where(x => x.departement == "w" && x.section == "mt"), "ID", "Fullname");
            obj.start_date = DateTime.Today.AddDays(-1).ToString("dd-MM-yyyy");
            obj.stop_date = DateTime.Today.AddDays(-1).ToString("dd-MM-yyyy");

            return View(obj);
        }

        public ActionResult Preview()
        {
            return RedirectToAction("index");
        }

        [HttpPost]
        public ActionResult Preview(FormParameter FrmData)
        {
            InitializeUserAkses();
            decimal deptid = 0, mcid = 0;
            DateTime stop_from1 = DateTime.Now;
            DateTime stop_from2 = DateTime.Now;
            string pic_mtc, pic_leader, mtc_name, machineno;


            deptid = FrmData.dept_par;
            mcid = FrmData.mc_id_par;
            pic_mtc = FrmData.PIC_Mtc == "" || FrmData.PIC_Mtc == null ? "ALL" : FrmData.PIC_Mtc;
            mtc_name = FrmData.trouble_type_par == "Q" ? "MTC20" : "MTC10";
            machineno = FrmData.machine_par == null || FrmData.machine_par == "" ? "ALL" : FrmData.machine_par;

            string[] tanggal1 = FrmData.start_date.Split('-');
            string[] tanggal2 = FrmData.stop_date.Split('-');

            stop_from1 = new DateTime(Convert.ToInt16(tanggal1[2]), Convert.ToInt16(tanggal1[1]), Convert.ToInt16(tanggal1[0]));
            stop_from2 = new DateTime(Convert.ToInt16(tanggal2[2]), Convert.ToInt16(tanggal2[1]), Convert.ToInt16(tanggal2[0]));

            
            //StringBuilder query = new StringBuilder();
            //query.Append("exec wppc.rpt_machine_quality_trouble ");
            //query.Append(deptid.ToString());
            //query.Append("," + mcid + " , ");
            //query.Append("'" + String.Format("{0:yyyy-MM-dd}", stop_from1) + "',");
            //query.Append("'" + String.Format("{0:yyyy-MM-dd}", stop_from2) + "',");
            //query.Append("'" + mtc_name + "', ");
            //query.Append("'" + pic_mtc + "', ");
            //query.Append("'" + "ALL" + "', ");
            //query.Append("'" + machineno + "'");

            var result2 = ReportData(mtc_name, stop_from1, stop_from2); //db.Database.SqlQuery<MachineQualityTrouble>(query.ToString());
            //var result = db.Database.SqlQuery<MachineQualityTrouble>(query.ToString());
            //Session["Parameter"] = query.ToString();

            //return RedirectToAction("index/1");
            FormParameter obj = new FormParameter();
            obj.dept_par = deptid;
            obj.mc_id_par = mcid;
            obj.start_date_par = stop_from1;
            obj.start_date_par = stop_from2;
            obj.trouble_type_par = FrmData.trouble_type_par;
            obj.Report_Data = result2.ToList();

            string dept_code_cim = db.sysuser_app.Where(x => x.ID == User.Identity.Name).Select(x => x.departement).FirstOrDefault();
            string dept_name_cim = db.departement_master.Where(x => x.Dept_ID == dept_code_cim).Select(x => x.Dept_Desc).FirstOrDefault().ToUpper();
            decimal dept_id_system = db.ms_dept.Where(x => x.dept_name == dept_name_cim).Select(x => x.dept_id).FirstOrDefault();

            ViewBag.dept_par = new SelectList(db.ms_dept.Where(x => x.dept_id == dept_id_system && x.dept_id == dept_id_system), "dept_id", "dept_name", dept_id_system);
            ViewBag.mc_id_par = new SelectList(db.ms_machine_type.Where(x => x.dept_id == dept_id_system && x.mc_id == 70), "mc_id", "mc_name", 70);
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