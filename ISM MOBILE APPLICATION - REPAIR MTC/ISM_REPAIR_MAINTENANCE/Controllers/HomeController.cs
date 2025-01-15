using ISM_REPAIR_MAINTENANCE.Repository.Interface;
using ISM_REPAIR_MAINTENANCE.Repository.Service;
using System;
using System.Web.Mvc;
using System.Linq;

namespace ISM_REPAIR_MAINTENANCE.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        //protected IDashboard _dashboard;
        protected IAgingSparepart _aging;
        protected IFixCost _fix;
        protected ISparepartStockAmount _stock_amount;


        public HomeController()
        {

        }

        private void Fixcost()
        {
            _fix = new FixCostService();
            //_fix.GetFixCostData();

            //var bulan = DateTime.Now.AddMonths(-1).Month;
            var bulan = DateTime.Now.Month;
            var tahun = DateTime.Now.Year;

            ViewBag.Persen_SP_FS = _fix.GetViewBagPersentase(700, "Factory Supplies", bulan, tahun);
            ViewBag.Persen_SP_RM = _fix.GetViewBagPersentase(700, "Repair & Maintenance", bulan, tahun);
            ViewBag.Persen_SP_RFE = _fix.GetViewBagPersentase(700, "Repair from EN", bulan, tahun);

            ViewBag.Persen_WV_FS = _fix.GetViewBagPersentase(800, "Factory Supplies", bulan, tahun);
            ViewBag.Persen_WV_RM = _fix.GetViewBagPersentase(800, "Repair & Maintenance", bulan, tahun);
            ViewBag.Persen_WV_RFE = _fix.GetViewBagPersentase(800, "Repair from EN", bulan, tahun);

            ViewBag.Persen_DY_FS = _fix.GetViewBagPersentase(900, "Factory Supplies", bulan, tahun);
            ViewBag.Persen_DY_RM = _fix.GetViewBagPersentase(900, "Repair & Maintenance", bulan, tahun);
            ViewBag.Persen_DY_RFE = _fix.GetViewBagPersentase(900, "Repair from EN", bulan, tahun);

            ViewBag.Persen_ENG_FS = _fix.GetViewBagPersentase(506, "Factory Supplies", bulan, tahun);
            ViewBag.Persen_ENG_RM = _fix.GetViewBagPersentase(506, "Repair & Maintenance", bulan, tahun);
            ViewBag.Persen_ENG_RFE = _fix.GetViewBagPersentase(506, "Repair from EN", bulan, tahun);

            ViewBag.Persen_GA_FS = _fix.GetViewBagPersentase(400, "Factory Supplies", bulan, tahun);
            ViewBag.Persen_GA_RM = _fix.GetViewBagPersentase(400, "Repair & Maintenance", bulan, tahun);
            ViewBag.Persen_GA_RFE = _fix.GetViewBagPersentase(400, "Repair from EN", bulan, tahun);

            ViewBag.Act_SP_FS = _fix.GetViewBagDetailAmount(700, "Factory Supplies", "ACTUAL", bulan, tahun);
            ViewBag.Act_SP_RM = _fix.GetViewBagDetailAmount(700, "Repair & Maintenance", "ACTUAL", bulan, tahun);
            ViewBag.Act_SP_RFE = _fix.GetViewBagDetailAmount(700, "Repair from EN", "ACTUAL", bulan, tahun);
            ViewBag.Act_SP_Total = _fix.GetViewBagDetailAmount(700, "TOTAL", "ACTUAL", bulan, tahun);

            ViewBag.BU_SP_FS = _fix.GetViewBagDetailAmount(700, "Factory Supplies", "BUDGET", bulan, tahun);
            ViewBag.BU_SP_RM = _fix.GetViewBagDetailAmount(700, "Repair & Maintenance", "BUDGET", bulan, tahun);
            ViewBag.BU_SP_RFE = _fix.GetViewBagDetailAmount(700, "Repair from EN", "BUDGET", bulan, tahun);
            ViewBag.BU_SP_Total = _fix.GetViewBagDetailAmount(700, "TOTAL", "BUDGET", bulan, tahun);

            ViewBag.Act_WV_FS = _fix.GetViewBagDetailAmount(800, "Factory Supplies", "ACTUAL", bulan, tahun);
            ViewBag.Act_WV_RM = _fix.GetViewBagDetailAmount(800, "Repair & Maintenance", "ACTUAL", bulan, tahun);
            ViewBag.Act_WV_RFE = _fix.GetViewBagDetailAmount(800, "Repair from EN", "ACTUAL", bulan, tahun);
            ViewBag.Act_WV_Total = _fix.GetViewBagDetailAmount(800, "TOTAL", "ACTUAL", bulan, tahun);

            ViewBag.BU_WV_FS = _fix.GetViewBagDetailAmount(800, "Factory Supplies", "BUDGET", bulan, tahun);
            ViewBag.BU_WV_RM = _fix.GetViewBagDetailAmount(800, "Repair & Maintenance", "BUDGET", bulan, tahun);
            ViewBag.BU_WV_RFE = _fix.GetViewBagDetailAmount(800, "Repair from EN", "BUDGET", bulan, tahun);
            ViewBag.BU_WV_Total = _fix.GetViewBagDetailAmount(800, "TOTAL", "BUDGET", bulan, tahun);

            ViewBag.Act_DY_FS = _fix.GetViewBagDetailAmount(900, "Factory Supplies", "ACTUAL", bulan, tahun);
            ViewBag.Act_DY_RM = _fix.GetViewBagDetailAmount(900, "Repair & Maintenance", "ACTUAL", bulan, tahun);
            ViewBag.Act_DY_RFE = _fix.GetViewBagDetailAmount(900, "Repair from EN", "ACTUAL", bulan, tahun);
            ViewBag.Act_DY_Total = _fix.GetViewBagDetailAmount(900, "TOTAL", "ACTUAL", bulan, tahun);

            ViewBag.BU_DY_FS = _fix.GetViewBagDetailAmount(900, "Factory Supplies", "BUDGET", bulan, tahun);
            ViewBag.BU_DY_RM = _fix.GetViewBagDetailAmount(900, "Repair & Maintenance", "BUDGET", bulan, tahun);
            ViewBag.BU_DY_RFE = _fix.GetViewBagDetailAmount(900, "Repair from EN", "BUDGET", bulan, tahun);
            ViewBag.BU_DY_Total = _fix.GetViewBagDetailAmount(900, "TOTAL", "BUDGET", bulan, tahun);

            ViewBag.Act_ENG_FS = _fix.GetViewBagDetailAmount(506, "Factory Supplies", "ACTUAL", bulan, tahun);
            ViewBag.Act_ENG_RM = _fix.GetViewBagDetailAmount(506, "Repair & Maintenance", "ACTUAL", bulan, tahun);
            ViewBag.Act_ENG_RFE = _fix.GetViewBagDetailAmount(506, "Repair from EN", "ACTUAL", bulan, tahun);
            ViewBag.Act_ENG_Total = _fix.GetViewBagDetailAmount(506, "TOTAL", "ACTUAL", bulan, tahun);

            ViewBag.BU_ENG_FS = _fix.GetViewBagDetailAmount(506, "Factory Supplies", "BUDGET", bulan, tahun);
            ViewBag.BU_ENG_RM = _fix.GetViewBagDetailAmount(506, "Repair & Maintenance", "BUDGET", bulan, tahun);
            ViewBag.BU_ENG_RFE = _fix.GetViewBagDetailAmount(506, "Repair from EN", "BUDGET", bulan, tahun);
            ViewBag.BU_ENG_Total = _fix.GetViewBagDetailAmount(506, "TOTAL", "BUDGET", bulan, tahun);

            ViewBag.Act_GA_FS = _fix.GetViewBagDetailAmount(400, "Factory Supplies", "ACTUAL", bulan, tahun);
            ViewBag.Act_GA_RM = _fix.GetViewBagDetailAmount(400, "Repair & Maintenance", "ACTUAL", bulan, tahun);
            ViewBag.Act_GA_RFE = _fix.GetViewBagDetailAmount(400, "Repair from EN", "ACTUAL", bulan, tahun);
            ViewBag.Act_GA_Total = _fix.GetViewBagDetailAmount(400, "TOTAL", "ACTUAL", bulan, tahun);

            ViewBag.BU_GA_FS = _fix.GetViewBagDetailAmount(400, "Factory Supplies", "BUDGET", bulan, tahun);
            ViewBag.BU_GA_RM = _fix.GetViewBagDetailAmount(400, "Repair & Maintenance", "BUDGET", bulan, tahun);
            ViewBag.BU_GA_RFE = _fix.GetViewBagDetailAmount(400, "Repair from EN", "BUDGET", bulan, tahun);
            ViewBag.BU_GA_Total = _fix.GetViewBagDetailAmount(400, "TOTAL", "BUDGET", bulan, tahun);
            
        }

        private void AgingSparepart()
        {
            _aging = new AgingSparepartService();
            ViewBag.AgingSparepart = _aging.GetViewBag_AgingSparepart();
        }

        private void SparepartStockAmount()
        {
            ViewBag.Budget = 600000;
            _stock_amount = new SparepartStockAmountService();

            var data_for_viewbag = _stock_amount.GetStockAmount();

            if (data_for_viewbag != null && data_for_viewbag.Count() > 0)
            {
                ViewBag.Act_Spinning = data_for_viewbag.Where(x => x.dept_name.Trim() == "Spinning").FirstOrDefault().stock_amount;
                ViewBag.Act_Weaving = data_for_viewbag.Where(x => x.dept_name.Trim() == "Weaving").FirstOrDefault().stock_amount;
                ViewBag.Act_Dyeing = data_for_viewbag.Where(x => x.dept_name.Trim() == "Dyeing").FirstOrDefault().stock_amount;
                ViewBag.Act_Eng = data_for_viewbag.Where(x => x.dept_name.Trim() == "Engineering").FirstOrDefault().stock_amount;
            }
            else
            {
                ViewBag.Act_Spinning = 0;
                ViewBag.Act_Weaving = 0;
                ViewBag.Act_Dyeing = 0;
                ViewBag.Act_Eng = 0;
            }


        }

        public ActionResult Index()
        {
            ViewBag.UserName = User.Identity.Name.ToUpper();
            Fixcost();
            AgingSparepart();
            SparepartStockAmount();

            //GetViewBag();
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}