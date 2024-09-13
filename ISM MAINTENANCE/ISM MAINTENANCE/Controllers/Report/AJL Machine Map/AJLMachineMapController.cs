using ISM_MAINTENANCE.Models.DB;
using ISM_MAINTENANCE.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ISM_MAINTENANCE.Controllers.Report
{
    public class AJLMachineMapController : Controller
    {
        private WvMaintenanceEntities db = new WvMaintenanceEntities();

        private void SetupData()
        {

            string str_query = "";

            str_query = "update " +
                        "A " +
                        "set A.Color = CASE " +
                                    "WHEN B.mtc_name = 'MACHINE TROUBLE' THEN 'Red' " +
                                    "WHEN B.mtc_name = 'QUALITY TROUBLE' THEN 'Yellow' " +
                                    "ELSE 'None' " +
                                "END " +
                        "FROM " +
                        "[ms_ajl_machine_color] A " +
                        "LEFT JOIN " +
                        "    (select block + '.' + loom_no AS[MC], mtc_name from WPPC.v_AjlMapTroubleCount) B " +
                        "on A.MC = B.MC ";

            ViewBag.Execute = db.Database.ExecuteSqlCommand(str_query);

            str_query = "SELECT MC, Color, RowPos, ColPos FROM ms_ajl_machine_color " +
                               "WHERE left(MC, 1) = 'A' and [All] = 0 and RowPos is not null and ColPos is not null " +
                               "ORDER BY left(MC, 1), RowPos, ColPos";

            var result = db.Database.SqlQuery<BlockA>(str_query);
            str_query = "";
            ViewBag.BlockA = result.ToList();
            ViewBag.MaxRowA = result.Max(x => x.RowPos);
            ViewBag.MaxColA = result.Max(x => x.ColPos);

            str_query = "SELECT MC, Color, RowPos, ColPos FROM ms_ajl_machine_color " +
                               "WHERE left(MC, 1) = 'B' and [All] = 0 and RowPos is not null and ColPos is not null " +
                               "ORDER BY left(MC, 1), RowPos, ColPos";

            var resultB = db.Database.SqlQuery<BlockB>(str_query);
            str_query = "";
            ViewBag.BlockB = resultB.ToList();
            ViewBag.MaxRowB = resultB.Max(x => x.RowPos);
            ViewBag.MaxColB = resultB.Max(x => x.ColPos);

            str_query = "SELECT MC, Color, RowPos, ColPos FROM ms_ajl_machine_color " +
                               "WHERE left(MC, 1) = 'C' and [All] = 0 and RowPos is not null and ColPos is not null " +
                               "ORDER BY left(MC, 1), RowPos, ColPos";

            var resultC = db.Database.SqlQuery<BlockC>(str_query);
            str_query = "";
            ViewBag.BlockC = resultC.ToList();
            ViewBag.MaxRowC = resultC.Max(x => x.RowPos);
            ViewBag.MaxColC = resultC.Max(x => x.ColPos);

            str_query = "SELECT MC, Color, RowPos, ColPos FROM ms_ajl_machine_color " +
                               "WHERE left(MC, 1) = 'D' and [All] = 0 and RowPos is not null and ColPos is not null " +
                               "ORDER BY left(MC, 1), RowPos, ColPos";

            var resultD = db.Database.SqlQuery<BlockD>(str_query);
            str_query = "";
            ViewBag.BlockD = resultD.ToList();
            ViewBag.MaxRowD = resultD.Max(x => x.RowPos);
            ViewBag.MaxColD = resultD.Max(x => x.ColPos);

            str_query = "SELECT MC, Color, RowPos, ColPos FROM ms_ajl_machine_color " +
                           "WHERE [All] = 1 and RowPos is not null and ColPos is not null " +
                           "ORDER BY left(MC, 1), RowPos, ColPos";

            var resultAll = db.Database.SqlQuery<BlockAll>(str_query);
            str_query = "";
            ViewBag.BlockAll = resultAll.ToList();
            ViewBag.MaxRowAll = resultAll.Max(x => x.RowPos);
            ViewBag.MaxColAll = resultAll.Max(x => x.ColPos);

            str_query = "select * from wppc.v_AjlMapTroubleCount";

            var Jumlahtrouble = db.Database.SqlQuery<TroubleCount>(str_query);
            str_query = "";

            ViewBag.MachineTroubleA = Jumlahtrouble.Where(x=> x.mtc_name == "MACHINE TROUBLE" && x.block == "A").Count() == 0 ? 0 : Convert.ToInt16(Jumlahtrouble.Where(x => x.mtc_name == "MACHINE TROUBLE" && x.block == "A").Sum(x => x.JumlahTrouble));
            ViewBag.QualityTroubleA = Jumlahtrouble.Where(x => x.mtc_name == "QUALITY TROUBLE" && x.block == "A").Count() == 0 ? 0 : Convert.ToInt16(Jumlahtrouble.Where(x => x.mtc_name == "QUALITY TROUBLE" && x.block == "A").Sum(x => x.JumlahTrouble));

            ViewBag.MachineTroubleB = Jumlahtrouble.Where(x => x.mtc_name == "MACHINE TROUBLE" && x.block == "B").Count() == 0 ? 0 : Convert.ToInt16(Jumlahtrouble.Where(x => x.mtc_name == "MACHINE TROUBLE" && x.block == "B").Sum(x => x.JumlahTrouble));
            ViewBag.QualityTroubleB = Jumlahtrouble.Where(x => x.mtc_name == "QUALITY TROUBLE" && x.block == "B").Count() == 0 ? 0 : Convert.ToInt16(Jumlahtrouble.Where(x => x.mtc_name == "QUALITY TROUBLE" && x.block == "B").Sum(x => x.JumlahTrouble));

            ViewBag.MachineTroubleC = Jumlahtrouble.Where(x => x.mtc_name == "MACHINE TROUBLE" && x.block == "C").Count() == 0 ? 0 : Convert.ToInt16(Jumlahtrouble.Where(x => x.mtc_name == "MACHINE TROUBLE" && x.block == "C").Sum(x => x.JumlahTrouble));
            ViewBag.QualityTroubleC = Jumlahtrouble.Where(x => x.mtc_name == "QUALITY TROUBLE" && x.block == "C").Count() == 0 ? 0 : Convert.ToInt16(Jumlahtrouble.Where(x => x.mtc_name == "QUALITY TROUBLE" && x.block == "C").Sum(x => x.JumlahTrouble));

            ViewBag.MachineTroubleD = Jumlahtrouble.Where(x => x.mtc_name == "MACHINE TROUBLE" && x.block == "D").Count() == 0 ? 0 : Convert.ToInt16(Jumlahtrouble.Where(x => x.mtc_name == "MACHINE TROUBLE" && x.block == "D").Sum(x => x.JumlahTrouble));
            ViewBag.QualityTroubleD = Jumlahtrouble.Where(x => x.mtc_name == "QUALITY TROUBLE" && x.block == "D").Count() == 0 ? 0 : Convert.ToInt16(Jumlahtrouble.Where(x => x.mtc_name == "QUALITY TROUBLE" && x.block == "D").Sum(x => x.JumlahTrouble));
            
            ViewBag.MachineTroubleAll = Jumlahtrouble.Where(x => x.mtc_name == "MACHINE TROUBLE").Count() == 0 ? 0 : Convert.ToInt16(Jumlahtrouble.Where(x => x.mtc_name == "MACHINE TROUBLE").Sum(x=> x.JumlahTrouble));
            ViewBag.QualityTroubleAll = Jumlahtrouble.Where(x => x.mtc_name == "QUALITY TROUBLE").Count() == 0 ? 0 : Convert.ToInt16(Jumlahtrouble.Where(x => x.mtc_name == "QUALITY TROUBLE").Sum(x=> x.JumlahTrouble));

        }

        // GET: AJLMachineMap
        public ActionResult Index()
        {

            Response.AddHeader("Refresh", "130");
            SetupData();
            return View();
        }

        public ActionResult AjlMachineMapBigTV()
        {

            Response.AddHeader("Refresh", "120");
            SetupData();
            return View();
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