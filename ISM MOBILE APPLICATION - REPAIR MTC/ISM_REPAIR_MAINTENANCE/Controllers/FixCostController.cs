using ISM_REPAIR_MAINTENANCE.Repository.Interface;
using ISM_REPAIR_MAINTENANCE.Repository.Service;
using System;
using System.Web.Mvc;

namespace ISM_REPAIR_MAINTENANCE.Controllers
{
    [Authorize]
    public class FixCostController : Controller
    {
        private IFixCost _fixcost;


        public FixCostController()
        {
            _fixcost = new FixCostService();
        }

        private void SetViewBag()
        {
            //_fixcost.GetFixCostData();

            //var bulan = DateTime.Now.AddMonths(-1).Month;
            var bulan = DateTime.Now.Month;
            var tahun = DateTime.Now.Year;

            ViewBag.Persen_SP_FS = _fixcost.GetViewBagPersentase(700, "Factory Supplies", bulan, tahun);
            ViewBag.Persen_SP_RM = _fixcost.GetViewBagPersentase(700, "Repair & Maintenance", bulan, tahun);
            ViewBag.Persen_SP_RFE = _fixcost.GetViewBagPersentase(700, "Repair from EN", bulan, tahun);

            ViewBag.Persen_WV_FS = _fixcost.GetViewBagPersentase(800, "Factory Supplies", bulan, tahun);
            ViewBag.Persen_WV_RM = _fixcost.GetViewBagPersentase(800, "Repair & Maintenance", bulan, tahun);
            ViewBag.Persen_WV_RFE = _fixcost.GetViewBagPersentase(800, "Repair from EN", bulan, tahun);

            ViewBag.Persen_DY_FS = _fixcost.GetViewBagPersentase(900, "Factory Supplies", bulan, tahun);
            ViewBag.Persen_DY_RM = _fixcost.GetViewBagPersentase(900, "Repair & Maintenance", bulan, tahun);
            ViewBag.Persen_DY_RFE = _fixcost.GetViewBagPersentase(900, "Repair from EN", bulan, tahun);

            ViewBag.Persen_ENG_FS = _fixcost.GetViewBagPersentase(506, "Factory Supplies", bulan, tahun);
            ViewBag.Persen_ENG_RM = _fixcost.GetViewBagPersentase(506, "Repair & Maintenance", bulan, tahun);
            ViewBag.Persen_ENG_RFE = _fixcost.GetViewBagPersentase(506, "Repair from EN", bulan, tahun);

            ViewBag.Persen_GA_FS = _fixcost.GetViewBagPersentase(400, "Factory Supplies", bulan, tahun);
            ViewBag.Persen_GA_RM = _fixcost.GetViewBagPersentase(400, "Repair & Maintenance", bulan, tahun);
            ViewBag.Persen_GA_RFE = _fixcost.GetViewBagPersentase(400, "Repair from EN", bulan, tahun);

            ViewBag.Act_SP_FS = _fixcost.GetViewBagDetailAmount(700, "Factory Supplies", "ACTUAL", bulan, tahun);
            ViewBag.Act_SP_RM = _fixcost.GetViewBagDetailAmount(700, "Repair & Maintenance", "ACTUAL", bulan, tahun);
            ViewBag.Act_SP_RFE = _fixcost.GetViewBagDetailAmount(700, "Repair from EN", "ACTUAL", bulan, tahun);
            ViewBag.Act_SP_Total = _fixcost.GetViewBagDetailAmount(700, "TOTAL", "ACTUAL", bulan, tahun);

            ViewBag.BU_SP_FS = _fixcost.GetViewBagDetailAmount(700, "Factory Supplies", "BUDGET", bulan, tahun);
            ViewBag.BU_SP_RM = _fixcost.GetViewBagDetailAmount(700, "Repair & Maintenance", "BUDGET", bulan, tahun);
            ViewBag.BU_SP_RFE = _fixcost.GetViewBagDetailAmount(700, "Repair from EN", "BUDGET", bulan, tahun);
            ViewBag.BU_SP_Total = _fixcost.GetViewBagDetailAmount(700, "TOTAL", "BUDGET", bulan, tahun);

            ViewBag.Act_WV_FS = _fixcost.GetViewBagDetailAmount(800, "Factory Supplies", "ACTUAL", bulan, tahun);
            ViewBag.Act_WV_RM = _fixcost.GetViewBagDetailAmount(800, "Repair & Maintenance", "ACTUAL", bulan, tahun);
            ViewBag.Act_WV_RFE = _fixcost.GetViewBagDetailAmount(800, "Repair from EN", "ACTUAL", bulan, tahun);
            ViewBag.Act_WV_Total = _fixcost.GetViewBagDetailAmount(800, "TOTAL", "ACTUAL", bulan, tahun);

            ViewBag.BU_WV_FS = _fixcost.GetViewBagDetailAmount(800, "Factory Supplies", "BUDGET", bulan, tahun);
            ViewBag.BU_WV_RM = _fixcost.GetViewBagDetailAmount(800, "Repair & Maintenance", "BUDGET", bulan, tahun);
            ViewBag.BU_WV_RFE = _fixcost.GetViewBagDetailAmount(800, "Repair from EN", "BUDGET", bulan, tahun);
            ViewBag.BU_WV_Total = _fixcost.GetViewBagDetailAmount(800, "TOTAL", "BUDGET", bulan, tahun);

            ViewBag.Act_DY_FS = _fixcost.GetViewBagDetailAmount(900, "Factory Supplies", "ACTUAL", bulan, tahun);
            ViewBag.Act_DY_RM = _fixcost.GetViewBagDetailAmount(900, "Repair & Maintenance", "ACTUAL", bulan, tahun);
            ViewBag.Act_DY_RFE = _fixcost.GetViewBagDetailAmount(900, "Repair from EN", "ACTUAL", bulan, tahun);
            ViewBag.Act_DY_Total = _fixcost.GetViewBagDetailAmount(900, "TOTAL", "ACTUAL", bulan, tahun);

            ViewBag.BU_DY_FS = _fixcost.GetViewBagDetailAmount(900, "Factory Supplies", "BUDGET", bulan, tahun);
            ViewBag.BU_DY_RM = _fixcost.GetViewBagDetailAmount(900, "Repair & Maintenance", "BUDGET", bulan, tahun);
            ViewBag.BU_DY_RFE = _fixcost.GetViewBagDetailAmount(900, "Repair from EN", "BUDGET", bulan, tahun);
            ViewBag.BU_DY_Total = _fixcost.GetViewBagDetailAmount(900, "TOTAL", "BUDGET", bulan, tahun);

            ViewBag.Act_ENG_FS = _fixcost.GetViewBagDetailAmount(506, "Factory Supplies", "ACTUAL", bulan, tahun);
            ViewBag.Act_ENG_RM = _fixcost.GetViewBagDetailAmount(506, "Repair & Maintenance", "ACTUAL", bulan, tahun);
            ViewBag.Act_ENG_RFE = _fixcost.GetViewBagDetailAmount(506, "Repair from EN", "ACTUAL", bulan, tahun);
            ViewBag.Act_ENG_Total = _fixcost.GetViewBagDetailAmount(506, "TOTAL", "ACTUAL", bulan, tahun);

            ViewBag.BU_ENG_FS = _fixcost.GetViewBagDetailAmount(506, "Factory Supplies", "BUDGET", bulan, tahun);
            ViewBag.BU_ENG_RM = _fixcost.GetViewBagDetailAmount(506, "Repair & Maintenance", "BUDGET", bulan, tahun);
            ViewBag.BU_ENG_RFE = _fixcost.GetViewBagDetailAmount(506, "Repair from EN", "BUDGET", bulan, tahun);
            ViewBag.BU_ENG_Total = _fixcost.GetViewBagDetailAmount(506, "TOTAL", "BUDGET", bulan, tahun);

            ViewBag.Act_GA_FS = _fixcost.GetViewBagDetailAmount(400, "Factory Supplies", "ACTUAL", bulan, tahun);
            ViewBag.Act_GA_RM = _fixcost.GetViewBagDetailAmount(400, "Repair & Maintenance", "ACTUAL", bulan, tahun);
            ViewBag.Act_GA_RFE = _fixcost.GetViewBagDetailAmount(400, "Repair from EN", "ACTUAL", bulan, tahun);
            ViewBag.Act_GA_Total = _fixcost.GetViewBagDetailAmount(400, "TOTAL", "ACTUAL", bulan, tahun);

            ViewBag.BU_GA_FS = _fixcost.GetViewBagDetailAmount(400, "Factory Supplies", "BUDGET", bulan, tahun);
            ViewBag.BU_GA_RM = _fixcost.GetViewBagDetailAmount(400, "Repair & Maintenance", "BUDGET", bulan, tahun);
            ViewBag.BU_GA_RFE = _fixcost.GetViewBagDetailAmount(400, "Repair from EN", "BUDGET", bulan, tahun);
            ViewBag.BU_GA_Total = _fixcost.GetViewBagDetailAmount(400, "TOTAL", "BUDGET", bulan, tahun);


        }

        // GET: FixCost
        public ActionResult Index()
        {
            ViewBag.UserName = User.Identity.Name.ToUpper();
            SetViewBag();       
            return View();
        }
    }
}