using ISM_MAINTENANCE.Models.ViewModel;
using ISM_MAINTENANCE.Models.DB;
using System.Web.Mvc;
using System.Linq;
using System;
using System.Collections.Generic;

namespace ISM_MAINTENANCE.Controllers.Maintenance
{
    [Authorize]
    public class MaintenanceResultController : Controller
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

        private int ReplaceNullValueInteger(object obj)
        {
            int _value = 0;
            if (obj == null)
            {
                _value = 0;
            }
            else
            {
                _value = Convert.ToInt32(obj);
            }

            return _value;
        }

        // GET: MaintenanceResult
        //public ActionResult Index()
        //{
        //    string dept_code_cim = db.sysuser_app.Where(x => x.ID == User.Identity.Name).Select(x => x.departement).FirstOrDefault();
        //    string dept_name_cim = db.departement_master.Where(x => x.Dept_ID == dept_code_cim).Select(x => x.Dept_Desc).FirstOrDefault().ToUpper();
        //    decimal dept_id_system = db.ms_dept.Where(x => x.dept_name == dept_name_cim).Select(x => x.dept_id).FirstOrDefault();

        //    var DATA = from query in db.v_MaintenanceResult_Group
        //               where query.dept_id == dept_id_system
        //               select new MaintenanceResultView()
        //               {
        //                   dept_id = query.dept_id,
        //                   mc_id = query.mc_id,
        //                   mtc_slip_no1 = query.mtc_slip_no1,
        //                   mtc_slip_no2 = query.mtc_slip_no2,
        //                   mtc_id = query.mtc_id,
        //                   mtc_name = query.mtc_name,
        //                   machine_no = query.block + "." + query.loom_no,
        //                   loom_no = query.loom_no,
        //                   block = query.block,
        //                   MaintenanceSlip = query.mtc_slip_no1 + "-" + query.mtc_slip_no2,
        //                   mtc_ser_no = query.mtc_ser_no == null ? 0 : (int)query.mtc_ser_no,
        //                   mtc_status = query.mtc_status,
        //                   mtc_pic = query.mtc_pic,
        //                   mtc_pic_name = query.Fullname
        //               };

        //    return View(DATA);
        //}

        ////GET : Details1/[id]
        //public ActionResult Details1(string id)
        //{
        //    string[] param = id.Split('-');
        //    decimal _deptid = Convert.ToDecimal(param[0]);
        //    decimal _mcid = Convert.ToDecimal(param[1]);
        //    string _slip1 = param[2];
        //    string _slip2 = param[3];
        //    int _ser_no = Convert.ToInt16(param[4]);

        //    //var SlipData = from data in db.v_MaintenanceResult
        //    //               where data.dept_id == _deptid
        //    //               select new VoucherSlip()
        //    //               {
        //    //                   mtc_slip_no1 = data.mtc_slip_no1,
        //    //                   mtc_slip_no2 = data.mtc_slip_no2,
        //    //                   MaintenanceSlip = data.mtc_slip_no1 + "-" + data.mtc_slip_no2,
        //    //                   machine_no = data.block + "." + data.loom_no,
        //    //                   block = data.block,
        //    //                   loom_no = data.loom_no,
        //    //                   mtc_name = data.mtc_name
        //    //               };

        //    //ViewBag.List_slip = SlipData.ToList();

        //    string block = db.tr_maintenance.Where(x => x.dept_id == _deptid && x.mc_id == _mcid && x.mtc_slip_no1 == _slip1 && x.mtc_slip_no2 == _slip2).Select(x => x.block).FirstOrDefault().ToString().Trim();
        //    string loom = db.tr_maintenance.Where(x => x.dept_id == _deptid && x.mc_id == _mcid && x.mtc_slip_no1 == _slip1 && x.mtc_slip_no2 == _slip2).Select(x => x.loom_no).FirstOrDefault().ToString().Trim();
        //    ViewBag.machine_no = block + "." + loom;
        //    ViewBag.SlipNo = _slip1 + "-" + _slip2;

        //    string pic_no = db.tr_maintenance_detail_1.Where(x => x.dept_id == _deptid && x.mc_id == _mcid &&
        //                    x.mtc_slip_no1 == _slip1 && x.mtc_slip_no2 == _slip2 && x.mtc_ser_no == _ser_no).Select(x => x.mtc_pic).FirstOrDefault().ToString().Trim();

        //    ViewBag.deptname = db.ms_dept.Where(x => x.dept_id == _deptid).Select(x => x.dept_name).FirstOrDefault().ToString().Trim();
        //    ViewBag.machinetype = db.ms_machine_type.Where(x => x.dept_id == _deptid && x.mc_id == _mcid).Select(x => x.mc_name).FirstOrDefault().ToString().Trim();
        //    ViewBag.mtc_pic = new SelectList(db.sysuser_app.Where(x => x.departement == "W").OrderBy(x => x.Fullname), "ID", "Fullname", pic_no);
        //    ViewBag.mtc_type = db.v_MaintenanceResult.Where(x => x.dept_id == _deptid && x.mc_id == _mcid && x.mtc_slip_no1 == _slip1 && x.mtc_slip_no2 == _slip2).Select(x => x.mtc_name).FirstOrDefault().ToString().Trim();
        //    ViewBag.mtcid = db.tr_maintenance.Where(x => x.dept_id == _deptid && x.mc_id == _mcid && x.mtc_slip_no1 == _slip1 && x.mtc_slip_no2 == _slip2).Select(x => x.mtc_id).FirstOrDefault().Trim();

        //    IQueryable<TRM_detail_1> temp = from data in db.tr_maintenance_detail_1
        //                                    where data.dept_id == _deptid && data.mc_id == _mcid && data.mtc_slip_no1 == _slip1 && data.mtc_slip_no2 == _slip2 && data.mtc_ser_no == _ser_no
        //                                    select new TRM_detail_1()
        //                                    {
        //                                        dept_id = data.dept_id,
        //                                        mc_id = data.mc_id,
        //                                        mtc_slip_no1 = data.mtc_slip_no1,
        //                                        mtc_slip_no2 = data.mtc_slip_no2,
        //                                        mtc_ser_no = data.mtc_ser_no,
        //                                        mtc_start = (DateTime)data.mtc_start,
        //                                        mtc_end = (DateTime)data.mtc_end,
        //                                        mtc_pic = data.mtc_pic,
        //                                        mtc_pic_name = db.sysuser_app.Where(x => x.ID == data.mtc_pic).Select(x => x.Fullname.ToString()).FirstOrDefault(),
        //                                        mtc_status = data.mtc_status,
        //                                        mtc_status_name = ""
        //                                    };

        //    TRM_detail_1 obj = new TRM_detail_1();
        //    obj.dept_id = temp.Select(x => x.dept_id).FirstOrDefault();
        //    obj.mc_id = temp.Select(x => x.mc_id).FirstOrDefault();
        //    obj.mtc_slip_no1 = temp.Select(x => x.mtc_slip_no1).FirstOrDefault();
        //    obj.mtc_slip_no2 = temp.Select(x => x.mtc_slip_no2).FirstOrDefault();
        //    obj.mtc_ser_no = temp.Select(x => x.mtc_ser_no).FirstOrDefault();
        //    obj.mtc_start = temp.Select(x => x.mtc_start).FirstOrDefault();
        //    obj.mtc_end = temp.Select(x => x.mtc_end).FirstOrDefault();
        //    obj.mtc_pic = temp.Select(x => x.mtc_pic).FirstOrDefault();
        //    obj.mtc_pic_name = temp.Select(x => x.mtc_pic_name).FirstOrDefault();
        //    obj.mtc_status = temp.Select(x => x.mtc_status).FirstOrDefault();
        //    obj.mtc_status_name = temp.Select(x => x.mtc_status_name).FirstOrDefault();

        //    IQueryable<TRM_detail_2> detail2 = from data in db.tr_maintenance_detail_2
        //                                       where data.dept_id == _deptid && data.mc_id == _mcid && data.mtc_slip_no1 == _slip1 && data.mtc_slip_no2 == _slip2 && data.mtc_ser_no == _ser_no
        //                                       select new TRM_detail_2()
        //                                       {
        //                                           dept_id = data.dept_id,
        //                                           mc_id = data.mc_id,
        //                                           mtc_slip_no1 = data.mtc_slip_no1,
        //                                           mtc_slip_no2 = data.mtc_slip_no2,
        //                                           mtc_ser_no = data.mtc_ser_no,
        //                                           trouble_id = data.trouble_id,
        //                                           trouble_name = db.ms_machine_trouble.Where(x => x.dept_id == data.dept_id && x.mc_id == data.mc_id && x.trouble_id == data.trouble_id).Select(x => x.trouble_name).FirstOrDefault()
        //                                       };
        //    obj.Detail2 = detail2.ToList();
        //    ViewBag.MaintenanceAction = db.v_Maintenance_Action.Where(x => x.dept_id == _deptid && x.mc_id == _mcid).ToList();
        //    return View(obj);


        //}

        //public ActionResult Details2(string id)
        //{
        //    if (id == null)
        //    {
        //        return RedirectToAction("index");
        //    }

        //    string[] param = id.Split(';');
        //    decimal _deptid = Convert.ToDecimal(param[0]);
        //    decimal _mcid = Convert.ToDecimal(param[1]);
        //    string _slip1 = param[2];
        //    string _slip2 = param[3];
        //    int _ser_no = Convert.ToInt16(param[4]);
        //    string _troubleid = param[5];
        //    string _mtcid = param[6];

        //    ViewBag.trouble_name = db.ms_machine_trouble.Where(x => x.dept_id == _deptid && x.mc_id == _mcid && x.trouble_id == _troubleid).Select(x => x.trouble_name).FirstOrDefault().Trim();
        //    ViewBag.BackParameter = _deptid.ToString() + "-" + _mcid.ToString() + "-" + _slip1 + "-" + _slip2 + "-" + _ser_no;

        //    ViewBag.dept_id = _deptid;
        //    ViewBag.mc_id = _mcid;
        //    ViewBag.slip1 = _slip1;
        //    ViewBag.slip2 = _slip2;
        //    ViewBag.ser_no = _ser_no;
        //    ViewBag.trouble_id = _troubleid;
        //    ViewBag.mtcid = _mtcid;

        //    var check = db.tr_maintenance_detail_3.Where(data => data.dept_id == _deptid && data.mc_id == _mcid && data.mtc_slip_no1 == _slip1 && data.mtc_slip_no2 == _slip2 && data.mtc_ser_no == _ser_no && data.trouble_id == _troubleid).ToList();
        //    if (check.Count != 0)
        //    {
        //        ViewBag.StatusInput = "Done";
        //        IQueryable<TRM_detail_3> obj = from data in db.tr_maintenance_detail_3
        //                                       where data.dept_id == _deptid && data.mc_id == _mcid && data.mtc_slip_no1 == _slip1 && data.mtc_slip_no2 == _slip2 && data.mtc_ser_no == _ser_no &&
        //                                       data.trouble_id == _troubleid
        //                                       select new TRM_detail_3()
        //                                       {
        //                                           dept_id = data.dept_id,
        //                                           mc_id = data.mc_id,
        //                                           mtc_slip_no1 = data.mtc_slip_no1,
        //                                           mtc_slip_no2 = data.mtc_slip_no2,
        //                                           mtc_ser_no = data.mtc_ser_no,
        //                                           trouble_id = data.trouble_id,
        //                                           mtc_action_id = data.mtc_action_id,
        //                                           mtc_action_name = db.ms_maintenance_action.Where(x => x.dept_id == _deptid && x.mc_id == _mcid && x.mtc_id == _mtcid && x.trouble_id == _troubleid && x.mtc_action_id == data.mtc_action_id).Select(x => x.mtc_action_name).FirstOrDefault().ToString().Trim()
        //                                       };


        //        ViewBag.mtc_action = new SelectList(db.v_Details2.Where(x => x.dept_id == _deptid && x.mc_id == _mcid && x.mtc_slip_no1 == _slip1 && x.mtc_slip_no2 == _slip2 &&
        //        x.mtc_ser_no == _ser_no && x.trouble_id == _troubleid).Select(x => new { x.mtc_action_id, x.mtc_action_name }), "mtc_action_id", "mtc_action_name");

        //        return View(obj.ToList());
        //    }

        //    else
        //    {
        //        ViewBag.StatusInput = "Not Yet";
        //        IQueryable<TRM_detail_3> obj2 = from data in db.v_Details2
        //                                        where data.dept_id == _deptid && data.mc_id == _mcid && data.mtc_slip_no1 == _slip1 && data.mtc_slip_no2 == _slip2 && data.mtc_ser_no == _ser_no &&
        //                                        data.trouble_id == _troubleid && data.mtc_id == _mtcid
        //                                        select new TRM_detail_3()
        //                                        {
        //                                            dept_id = data.dept_id,
        //                                            mc_id = data.mc_id,
        //                                            mtc_slip_no1 = data.mtc_slip_no1,
        //                                            mtc_slip_no2 = data.mtc_slip_no2,
        //                                            mtc_ser_no = data.mtc_ser_no,
        //                                            trouble_id = data.trouble_id,
        //                                            mtc_action_id = data.mtc_action_id,
        //                                            mtc_action_name = data.mtc_action_name
        //                                        };

        //        ViewBag.mtc_action = new SelectList(db.v_Details2.Where(x => x.dept_id == _deptid && x.mc_id == _mcid && x.mtc_slip_no1 == _slip1 && x.mtc_slip_no2 == _slip2 && x.mtc_ser_no == _ser_no && x.trouble_id == _troubleid).Select(x => new { x.mtc_action_id, x.mtc_action_name }), "mtc_action_id", "mtc_action_name");
        //        return View(obj2.ToList());
        //    }



        //}

        //[HttpPost]
        //public ActionResult Details2(List<TRM_detail_3> Frmdata)
        //{
        //    foreach (var duplicate in Frmdata)
        //    {
        //        tr_maintenance_detail_3 Check = db.tr_maintenance_detail_3.Find(duplicate.dept_id, duplicate.mc_id, duplicate.mtc_slip_no1, duplicate.mtc_slip_no2, duplicate.mtc_ser_no, duplicate.trouble_id, duplicate.mtc_action_id);
        //        if (Check != null)
        //        {
        //            ModelState.AddModelError("", "Duplicate Data");
        //            return View();
        //        }
        //    }

        //    foreach (var item in Frmdata)
        //    {
        //        tr_maintenance_detail_3 obj = new tr_maintenance_detail_3();
        //        obj.dept_id = (int)item.dept_id;
        //        obj.mc_id = (int)item.mc_id;
        //        obj.mtc_slip_no1 = item.mtc_slip_no1;
        //        obj.mtc_slip_no2 = item.mtc_slip_no2;
        //        obj.mtc_ser_no = item.mtc_ser_no;
        //        obj.trouble_id = item.trouble_id;
        //        obj.mtc_action_id = item.mtc_action_id;
        //        obj.user_id = User.Identity.Name.ToString().ToUpper();
        //        obj.rec_sts = "A";
        //        obj.proc_no = 1;
        //        obj.client_ip = GetIPAddress();
        //        obj.proc_time = DateTime.Now;
        //        db.tr_maintenance_detail_3.Add(obj);
        //        db.SaveChanges();

        //    }
        //    var param = ((int)Frmdata[0].dept_id).ToString() + "-" + ((int)Frmdata[0].mc_id).ToString() + "-" + Frmdata[0].mtc_slip_no1.ToString().Trim() + "-" + Frmdata[0].mtc_slip_no2.ToString().Trim() + "-" + Frmdata[0].mtc_ser_no.ToString().Trim();
        //    return RedirectToAction("Details1/" + param);
        //}

        ////GET : Details2/[id]
        //public ActionResult Details3(string id)
        //{
        //    if (id == null)
        //    {
        //        return RedirectToAction("index");
        //    }
        //    string[] param = id.Split('-');
        //    decimal _deptid = Convert.ToDecimal(param[0]);
        //    decimal _mcid = Convert.ToDecimal(param[1]);
        //    string _slip1 = param[2];
        //    string _slip2 = param[3];
        //    int _ser_no = Convert.ToInt16(param[4]);
        //    string _troubleid = param[5];
        //    string _mtcid = param[6];
        //    string _mtcActionId = param[7];

        //    ViewBag.dept_id = _deptid;
        //    ViewBag.mc_id = _mcid;
        //    ViewBag.slip1 = _slip1;
        //    ViewBag.slip2 = _slip2;
        //    ViewBag.ser_no = _ser_no;
        //    ViewBag.trouble_id = _troubleid;
        //    ViewBag.mtcActionName = db.v_Maintenance_Action.Where(x => x.dept_id == _deptid && x.mc_id == _mcid && x.mtc_id == _mtcid && x.trouble_id == _troubleid && x.mtc_action_id == _mtcActionId).Select(x => x.mtc_action_name).FirstOrDefault().ToString().Trim();

        //    string query = "select ItemCode COLLATE DATABASE_DEFAULT as ItemCode, ItemName COLLATE DATABASE_DEFAULT as ItemName  from wppc.v_SAP_OITM";
        //    var result = db.Database.SqlQuery<SAPITEM>(query);
        //    ViewBag.SapItem = new SelectList(result.ToList(), "ItemCode", "ItemName");//result.ToList();

        //    IQueryable<TRM_detail_4> obj = from data in db.tr_maintenance_detail_4
        //                                   where data.dept_id == _deptid && data.mc_id == _mcid && data.mtc_slip_no1 == _slip1 && data.mtc_slip_no2 == _slip2 &&
        //                                          data.mtc_ser_no == _ser_no && data.trouble_id == _troubleid && data.mtc_action_id == _mtcActionId
        //                                   select new TRM_detail_4()
        //                                   {
        //                                       dept_id = data.dept_id,
        //                                       mc_id = data.mc_id,
        //                                       mtc_slip_no1 = data.mtc_slip_no1,
        //                                       mtc_slip_no2 = data.mtc_slip_no2,
        //                                       mtc_ser_no = data.mtc_ser_no,
        //                                       trouble_id = data.trouble_id,
        //                                       mtc_action_id = data.mtc_action_id,
        //                                       item_code = data.item_code,
        //                                       item_name = "",//result.Where(x => x.item_code == data.item_code).Select(x => x.item_name).FirstOrDefault().ToString().Trim(),
        //                                       price = (decimal)data.price,
        //                                       quantity = (decimal)data.quantity
        //                                   };



        //    return View(obj.ToList());
        //}

        // GET: MaintenanceResult/Create
        //public ActionResult Create(string id)
        //{
        //    if (id == null)
        //    {
        //        string dept_code_cim = db.sysuser_app.Where(x => x.ID == User.Identity.Name).Select(x => x.departement).FirstOrDefault();
        //        string dept_name_cim = db.departement_master.Where(x => x.Dept_ID == dept_code_cim).Select(x => x.Dept_Desc).FirstOrDefault().ToUpper();
        //        decimal dept_id_system = db.ms_dept.Where(x => x.dept_name == dept_name_cim).Select(x => x.dept_id).FirstOrDefault();

        //        ViewBag.TRM_header = from header in db.tr_maintenance
        //                             where header.dept_id == dept_id_system
        //                             select new MaintenanceResultView()
        //                             {
        //                                 dept_id = header.dept_id,
        //                                 mc_id = header.mc_id,
        //                                 mtc_slip_no1 = header.mtc_slip_no1,
        //                                 mtc_slip_no2 = header.mtc_slip_no2,
        //                                 machine_no = header.mtc_slip_no1 + "." + header.mtc_slip_no2,
        //                                 mtc_id = header.mtc_id
        //                             };
        //        ViewBag.dept_id = new SelectList(db.ms_dept.Where(x => x.dept_id == dept_id_system), "dept_id", "dept_name");
        //        ViewBag.mc_id = new SelectList(db.ms_machine_type.Where(x => x.dept_id == dept_id_system && x.mc_id == 70), "mc_id", "mc_name");
        //        ViewBag.mtc_pic = new SelectList(db.sysuser_app.Where(x => x.departement == "W").OrderBy(x => x.Fullname), "ID", "Fullname");
        //        ViewBag.trouble_id = new SelectList(db.ms_machine_trouble.Where(x => x.dept_id == 0), "trouble_id", "trouble_name");
        //        ViewBag.machine_trouble = db.v_Machine_Trouble.Where(x => x.dept_id == dept_id_system);
        //        //ViewBag.FilterMachineTrouble = db.v_Machine_Trouble.Where(x => x.dept_id == dept_id_system);

        //        var SlipData = from data in db.v_MaintenanceResult_Group
        //                       where data.dept_id == dept_id_system
        //                       select new VoucherSlip()
        //                       {
        //                           mtc_slip_no1 = data.mtc_slip_no1,
        //                           mtc_slip_no2 = data.mtc_slip_no2,
        //                           MaintenanceSlip = data.mtc_slip_no1 + "-" + data.mtc_slip_no2,
        //                           machine_no = data.block + "." + data.loom_no,
        //                           block = data.block,
        //                           loom_no = data.loom_no,
        //                           mtc_name = data.mtc_name
        //                       };

        //        ViewBag.List_slip = SlipData.ToList();


        //        var MtcActionData = from data in db.v_Mtc_Action_For_Mtc_Result
        //                            where data.dept_id == dept_id_system
        //                            select new MaintenanceAction()
        //                            {
        //                                dept_id = data.dept_id,
        //                                mc_id = data.mc_id,
        //                                trouble_id = data.trouble_id,
        //                                trouble_name = data.trouble_name,
        //                                dept_name = data.dept_name,
        //                                mc_name = data.mc_name,
        //                                mtc_name = data.mtc_name
        //                            };

        //        ViewBag.ListMaintenanceAction = MtcActionData.ToList();

        //        TRM_detail_1 obj = new TRM_detail_1();

        //        return View(obj);
        //    }




        //    return View();


        //}

        // POST: MaintenanceResult/Create
        //[HttpPost]
        ////[Bind(Include = "dept_id, mc_id, mtc_slip_no1, mtc_slip_no2, mtc_ser_no, mtc_start, mtc_end, mtc_pic, mtc_status, Detail2")]
        //public ActionResult Create(TRM_detail_1 FrmData)
        //{
        //    try
        //    {
        //        tr_maintenance_detail_1 Duplicate = db.tr_maintenance_detail_1.Find(FrmData.dept_id, FrmData.mc_id, FrmData.mtc_slip_no1, FrmData.mtc_slip_no2, FrmData.mtc_ser_no);
        //        if (Duplicate != null)
        //        {
        //            ModelState.AddModelError("", "Duplicate Data");
        //            string dept_code_cim = db.sysuser_app.Where(x => x.ID == User.Identity.Name).Select(x => x.departement).FirstOrDefault();
        //            string dept_name_cim = db.departement_master.Where(x => x.Dept_ID == dept_code_cim).Select(x => x.Dept_Desc).FirstOrDefault().ToUpper();
        //            decimal dept_id_system = db.ms_dept.Where(x => x.dept_name == dept_name_cim).Select(x => x.dept_id).FirstOrDefault();

        //            ViewBag.TRM_header = from header in db.tr_maintenance
        //                                 where header.dept_id == dept_id_system
        //                                 select new MaintenanceResultView()
        //                                 {
        //                                     dept_id = header.dept_id,
        //                                     mc_id = header.mc_id,
        //                                     mtc_slip_no1 = header.mtc_slip_no1,
        //                                     mtc_slip_no2 = header.mtc_slip_no2,
        //                                     machine_no = header.mtc_slip_no1 + "." + header.mtc_slip_no2,
        //                                     mtc_id = header.mtc_id
        //                                 };
        //            ViewBag.dept_id = new SelectList(db.ms_dept.Where(x => x.dept_id == dept_id_system), "dept_id", "dept_name");
        //            ViewBag.mc_id = new SelectList(db.ms_machine_type.Where(x => x.dept_id == dept_id_system), "mc_id", "mc_name");
        //            ViewBag.pic = new SelectList(db.sysuser_app.Where(x => x.departement == "W").OrderBy(x => x.Fullname), "ID", "Fullname");

        //            var SlipData = from data in db.v_MaintenanceResult
        //                           where data.dept_id == dept_id_system
        //                           select new VoucherSlip()
        //                           {
        //                               mtc_slip_no1 = data.mtc_slip_no1,
        //                               mtc_slip_no2 = data.mtc_slip_no2,
        //                               MaintenanceSlip = data.mtc_slip_no1 + "-" + data.mtc_slip_no2,
        //                               machine_no = data.block + "." + data.loom_no,
        //                               block = data.block,
        //                               loom_no = data.loom_no,
        //                               mtc_name = data.mtc_name
        //                           };

        //            ViewBag.List_slip = SlipData.ToList();
        //            return View();
        //        }

        //        if (ModelState.IsValid)
        //        {
        //            tr_maintenance_detail_1 detail1 = new tr_maintenance_detail_1();
        //            detail1.dept_id = FrmData.dept_id;
        //            detail1.mc_id = FrmData.mc_id;
        //            detail1.mtc_slip_no1 = FrmData.mtc_slip_no1;
        //            detail1.mtc_slip_no2 = FrmData.mtc_slip_no2;
        //            detail1.mtc_ser_no = FrmData.mtc_ser_no;
        //            detail1.mtc_start = FrmData.mtc_start;
        //            detail1.mtc_end = FrmData.mtc_end;
        //            detail1.mtc_pic = FrmData.mtc_pic;
        //            detail1.mtc_status = FrmData.mtc_status;
        //            detail1.user_id = User.Identity.Name.ToUpper();
        //            detail1.rec_sts = "A";
        //            detail1.proc_no = 1;
        //            detail1.client_ip = GetIPAddress();
        //            detail1.proc_time = DateTime.Now;
        //            db.tr_maintenance_detail_1.Add(detail1);
        //            db.SaveChanges();

        //            foreach (var item in FrmData.Detail2)
        //            {
        //                tr_maintenance_detail_2 detail2 = new tr_maintenance_detail_2();
        //                detail2.dept_id = FrmData.dept_id;
        //                detail2.mc_id = FrmData.mc_id;
        //                detail2.mtc_slip_no1 = FrmData.mtc_slip_no1;
        //                detail2.mtc_slip_no2 = FrmData.mtc_slip_no2;
        //                detail2.mtc_ser_no = FrmData.mtc_ser_no;
        //                detail2.trouble_id = item.trouble_id;
        //                detail2.user_id = User.Identity.Name.ToUpper();
        //                detail2.rec_sts = "A";
        //                detail2.proc_no = 1;
        //                detail2.client_ip = GetIPAddress();
        //                detail2.proc_time = DateTime.Now;
        //                db.tr_maintenance_detail_2.Add(detail2);
        //                db.SaveChanges();
        //            }

        //            var parameter = FrmData.dept_id.ToString() + "-" + FrmData.mc_id.ToString() + "-" + FrmData.mtc_slip_no1.ToString() + "-" + FrmData.mtc_slip_no2.ToString() + "-" + FrmData.mtc_ser_no.ToString();
        //            return RedirectToAction("Details1/" + parameter.ToString());
        //        }

        //        return View("");



        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}


        //GET:Maintenance Action Detail
        public ActionResult Create_Mtc_action_detail(string id)
        {

            if (id == null)
            {

            }
            else
            {

            }
            return View();
        }



        //POST:Maintenance Action Detail
        [HttpPost]
        public ActionResult Create_Mtc_action_detail(FormCollection fc)
        {


            return View();
        }

        // GET: MaintenanceResult/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: MaintenanceResult/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: MaintenanceResult/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: MaintenanceResult/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
