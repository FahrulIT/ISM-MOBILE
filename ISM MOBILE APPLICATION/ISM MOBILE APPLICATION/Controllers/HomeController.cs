using ISM_MOBILE_APPLICATION.Data;
using ISM_MOBILE_APPLICATION.Models.Home;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;

namespace ISM_MOBILE_APPLICATION.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        decimal Plan = 0;
        decimal Actual = 0;

        private decimal GetPersentase(int _bulan, int _tahun, string _dept, string _data_type)
        {
            decimal _value = 0;
            decimal _plan = 0;
            decimal _actual = 0;
            decimal _actual_persentase = 0;

            using (ChartDbContext _context = new ChartDbContext())
            {
                _plan = _context.SummaryPlanActual.Where(s => s.Bulan == _bulan && s.Tahun == _tahun && s.Dept == _dept && s.Data_type == _data_type && s.Trans_Type == "PLAN")
                                    .Select(x => (decimal)x.Qty).FirstOrDefault();

                _actual = _context.SummaryPlanActual.Where(s => s.Bulan == _bulan && s.Tahun == _tahun && s.Dept == _dept && s.Data_type == _data_type && s.Trans_Type == "ACTUAL")
                                   .Select(x => (decimal)x.Qty).FirstOrDefault();

                _actual_persentase = _context.SummaryPlanActual.Where(s => s.Bulan == _bulan && s.Tahun == _tahun && s.Dept == _dept && s.Data_type == _data_type && s.Trans_Type == "ACTUAL_PRECENTAGE")
                                   .Select(x => (decimal)x.Qty).FirstOrDefault();

                Plan = _plan; Actual = _actual;

                if (_plan != 0)
                {
                    _value = Math.Round((_actual_persentase / _plan) * 100, 2);
                }
            }
            return _value;
        }


        private List<DetailHelper> GetDetail(int _bulan, int _tahun, string _dept, string _data_type)
        {
            using (ChartDbContext _context = new ChartDbContext())
            {
                IQueryable<DetailHelper> Temp = from WH_SP in _context.DetailPlanActual
                                                where WH_SP.Bulan == _bulan && WH_SP.Tahun == _tahun && WH_SP.Dept == _dept && WH_SP.Data_type == _data_type
                                                orderby WH_SP.Persentase ascending, WH_SP.Item
                                                select new DetailHelper()
                                                {
                                                    id = WH_SP.id,
                                                    Bulan = WH_SP.Bulan,
                                                    Tahun = WH_SP.Tahun,
                                                    Dept = WH_SP.Dept,
                                                    Item = WH_SP.Item,
                                                    Data_type = WH_SP.Data_type,
                                                    Persentase = WH_SP.Persentase.ToString() == null ? 0 : WH_SP.Persentase,
                                                    Plan = WH_SP.Plan,
                                                    Actual = WH_SP.Actual.ToString() == null ? 0 : WH_SP.Actual,
                                                    PlanofMonth = WH_SP.PlanofMonth,
                                                    CurrentInv = WH_SP.CurrentInv
                                                }
                                                ;
                if (Temp != null && Temp.Count() != 0)
                {
                    return Temp.ToList();
                }
                else
                {
                    return null;
                }
            }

        }

        private List<DetailHelper> GetDetailSummaryPlanOnly(int _bulan, int _tahun, string _dept, string _data_type)
        {
            using (ChartDbContext _context = new ChartDbContext())
            {
                IQueryable<DetailHelper> Temp = from Data in _context.DetailPlanActual
                                                where Data.Bulan == _bulan && Data.Tahun == _tahun && Data.Dept == _dept && Data.Data_type == _data_type
                                                group Data by new { Data.Bulan, Data.Tahun, Data.Dept, Data.Data_type } into GroupData
                                                select new DetailHelper()
                                                {
                                                    id = 1,
                                                    Bulan = 1,
                                                    Tahun = 1,
                                                    Dept = "1",
                                                    Item = "Total (Planned)",
                                                    Data_type = "-",
                                                    Persentase = (GroupData.Where(x => x.Plan > 0).Sum(x => x.Actual) / GroupData.Where(x => x.Plan > 0).Sum(x => x.Plan)) * 100,
                                                    Plan = GroupData.Where(x => x.Plan > 0).Sum(x => x.Plan),
                                                    Actual = GroupData.Where(x => x.Plan > 0).Sum(x => x.Actual),
                                                    //PlanofMonth = GroupData.Where(x => x.Plan > 0).Sum(x => x.PlanofMonth),
                                                    PlanofMonth = GroupData.Sum(x => x.PlanofMonth),
                                                    CurrentInv = GroupData.Sum(x => x.CurrentInv)
                                                }


                ;
                if (Temp != null && Temp.Count() != 0)
                {
                    return Temp.ToList();
                }
                else
                {
                    return null;
                }
            }

        }
        private List<DetailHelper> GetDetailSummaryAll(int _bulan, int _tahun, string _dept, string _data_type)
        {
            using (ChartDbContext _context = new ChartDbContext())
            {
                IQueryable<DetailHelper> Temp = from Data in _context.DetailPlanActual
                                                where Data.Bulan == _bulan && Data.Tahun == _tahun && Data.Dept == _dept && Data.Data_type == _data_type
                                                group Data by new { Data.Bulan, Data.Tahun, Data.Dept, Data.Data_type } into GroupData
                                                select new DetailHelper()
                                                {
                                                    id = 1,
                                                    Bulan = 1,
                                                    Tahun = 1,
                                                    Dept = "1",
                                                    Item = "Total (All)",
                                                    Data_type = "-",
                                                    Persentase = (GroupData.Sum(x => x.Actual) / GroupData.Sum(x => x.Plan)) * 100,
                                                    Plan = GroupData.Sum(x => x.Plan),
                                                    Actual = GroupData.Sum(x => x.Actual),
                                                    PlanofMonth = GroupData.Sum(x => x.PlanofMonth),
                                                    CurrentInv = GroupData.Sum(x => x.CurrentInv)
                                                }
                                                ;
                if (Temp != null && Temp.Count() != 0)
                {
                    return Temp.ToList();
                }
                else
                {
                    return null;
                }
            }

        }

        private object NumerikNullValueCheck(object data)
        {

            if (data == null)
            {

                return 0;
            }
            else
            {
                return data;
            }
        }

        private List<ExportToExcel> GetExcelData(string _Dept, string _Data_Type)
        {
            //string _Tanggal = "2024-06-03";
            string _Tanggal = DateTime.Now.ToString("yyyy-MM-dd");
            List<ExportToExcel> Temp = new List<ExportToExcel>();

            using (var context = new ChartDbContext())
            {
                var Parameter = new SqlParameter("@param", _Tanggal);

                var result = context.Database
                                    .SqlQuery<ExportToExcel>("[dbo].[DX_Export_to_excel_data]  @param", Parameter)
                                    .ToList();

                Temp = result.Where(x => x.DEPT.Trim() == _Dept && x.data_type.Trim() == _Data_Type).ToList();
            }
                                                            ;
            if (Temp != null && Temp.Count() != 0)
            {
                return Temp.ToList();
            }
            else
            {
                return null;
            }

        }

        public ActionResult Index()
        {

            ////FOR TEST ONLY
            //int BULAN = 5; int TAHUN = 2024;
            ////FOR TEST ONLY

            int BULAN = DateTime.Now.Month;
            int TAHUN = DateTime.Now.Year;

            ViewBag.PersentaseRMWH = NumerikNullValueCheck(GetPersentase(BULAN, TAHUN, "RMWH", "Raw Material Warehouse"));
            ViewBag.Qty_RMWH_Plan = Plan;
            ViewBag.Qty_RMWH_Actual = Actual;

            ViewBag.PersentaseUsage_RMC = NumerikNullValueCheck(GetPersentase(BULAN, TAHUN, "RMC", "Raw Material Consumption"));
            ViewBag.Qty_Usage_RMC_Plan = Plan;
            ViewBag.Qty_Usage_RMC_Actual = Actual;

            ViewBag.PersentaseWH_SP = NumerikNullValueCheck(GetPersentase(BULAN, TAHUN, "SPINNING", "Yarn Warehouse"));
            ViewBag.Qty_WH_SP_Plan = Plan;
            ViewBag.Qty_WH_SP_Actual = Actual;

            ViewBag.PersentaseUsage_SP = NumerikNullValueCheck(GetPersentase(BULAN, TAHUN, "SPINNING", "Yarn Consumption"));
            ViewBag.Qty_Usage_SP_Plan = Plan;
            ViewBag.Qty_Usage_SP_Actual = Actual;

            ViewBag.PersentaseWH_WV = NumerikNullValueCheck(GetPersentase(BULAN, TAHUN, "WEAVING", "Grey Warehouse"));
            ViewBag.Qty_WH_WV_Plan = Plan;
            ViewBag.Qty_WH_WV_Actual = Actual;

            ViewBag.PersentaseUsage_WV = NumerikNullValueCheck(GetPersentase(BULAN, TAHUN, "WEAVING", "Dyeing Input"));
            ViewBag.Qty_Usage_WV_Plan = Plan;
            ViewBag.Qty_Usage_WV_Actual = Actual;

            ViewBag.PersentaseWH_DY = NumerikNullValueCheck(GetPersentase(BULAN, TAHUN, "DYEING", "Finish Good Warehouse"));
            ViewBag.Qty_WH_DY_Plan = Plan;
            ViewBag.Qty_WH_DY_Actual = Actual;

            ViewBag.PersentaseUsage_DY = NumerikNullValueCheck(GetPersentase(BULAN, TAHUN, "DYEING", "Finish Good Delivery"));
            ViewBag.Qty_Usage_DY_Plan = Plan;
            ViewBag.Qty_Usage_DY_Actual = Actual;

            ViewBag.Detail_RMWH = GetDetail(BULAN, TAHUN, "RMWH", "Raw Material Warehouse");
            ViewBag.Detail_Usage_RMC = GetDetail(BULAN, TAHUN, "RMC", "Raw Material Consumption");
            ViewBag.Detail_WH_SP = GetDetail(BULAN, TAHUN, "SPINNING", "Yarn Warehouse");
            ViewBag.Detail_Usage_SP = GetDetail(BULAN, TAHUN, "SPINNING", "Yarn Consumption");
            ViewBag.Detail_WH_WV = GetDetail(BULAN, TAHUN, "WEAVING", "Grey Warehouse");
            ViewBag.Detail_Usage_WV = GetDetail(BULAN, TAHUN, "WEAVING", "Dyeing Input");
            ViewBag.Detail_WH_DY = GetDetail(BULAN, TAHUN, "DYEING", "Finish Good Warehouse");
            ViewBag.Detail_Usage_DY = GetDetail(BULAN, TAHUN, "DYEING", "Finish Good Delivery");

            ViewBag.SubtotalPlan_RMWH = GetDetailSummaryPlanOnly(BULAN, TAHUN, "RMWH", "Raw Material Warehouse");
            ViewBag.SubtotalPlan_Usage_RMC = GetDetailSummaryPlanOnly(BULAN, TAHUN, "RMC", "Raw Material Consumption");
            ViewBag.SubtotalPlan_WH_SP = GetDetailSummaryPlanOnly(BULAN, TAHUN, "SPINNING", "Yarn Warehouse");
            ViewBag.SubtotalPlan_Usage_SP = GetDetailSummaryPlanOnly(BULAN, TAHUN, "SPINNING", "Yarn Consumption");
            ViewBag.SubtotalPlan_WH_WV = GetDetailSummaryPlanOnly(BULAN, TAHUN, "WEAVING", "Grey Warehouse");
            ViewBag.SubtotalPlan_Usage_WV = GetDetailSummaryPlanOnly(BULAN, TAHUN, "WEAVING", "Dyeing Input");
            ViewBag.SubtotalPlan_WH_DY = GetDetailSummaryPlanOnly(BULAN, TAHUN, "DYEING", "Finish Good Warehouse");
            ViewBag.SubtotalPlan_Usage_DY = GetDetailSummaryPlanOnly(BULAN, TAHUN, "DYEING", "Finish Good Delivery");

            ViewBag.SubtotalAll_RMWH = GetDetailSummaryAll(BULAN, TAHUN, "RMWH", "Raw Material Warehouse");
            ViewBag.SubtotalAll_Usage_RMC = GetDetailSummaryAll(BULAN, TAHUN, "RMC", "Raw Material Consumption");
            ViewBag.SubtotalAll_WH_SP = GetDetailSummaryAll(BULAN, TAHUN, "SPINNING", "Yarn Warehouse");
            ViewBag.SubtotalAll_Usage_SP = GetDetailSummaryAll(BULAN, TAHUN, "SPINNING", "Yarn Consumption");
            ViewBag.SubtotalAll_WH_WV = GetDetailSummaryAll(BULAN, TAHUN, "WEAVING", "Grey Warehouse");
            ViewBag.SubtotalAll_Usage_WV = GetDetailSummaryAll(BULAN, TAHUN, "WEAVING", "Dyeing Input");
            ViewBag.SubtotalAll_WH_DY = GetDetailSummaryAll(BULAN, TAHUN, "DYEING", "Finish Good Warehouse");
            ViewBag.SubtotalAll_Usage_DY = GetDetailSummaryAll(BULAN, TAHUN, "DYEING", "Finish Good Delivery");
         
            ViewBag.ExportListRMWH = GetExcelData("RMWH", "Raw Material Warehouse");
            ViewBag.ExportListRMC = GetExcelData("RMC", "Raw Material Consumption");  
            ViewBag.ExportListDYWH = GetExcelData("DYEING", "Finish Good Warehouse"); 
            ViewBag.ExportListDYC = GetExcelData("DYEING", "Finish Good Delivery");  
            ViewBag.ExportListWVWH = GetExcelData("WEAVING", "Grey Warehouse"); 
            ViewBag.ExportListWVC = GetExcelData("WEAVING", "Dyeing Input");
            ViewBag.ExportListSPWH = GetExcelData("SPINNING", "Yarn Warehouse"); 
            ViewBag.ExportListSPC = GetExcelData("SPINNING", "Yarn Consumption"); 

            var lastDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
            ViewBag.LastDay = lastDay.Day;

            string _username = User.Identity.GetUserName();

            if (Session["S_RolesName"] is string)
            {
                ViewBag.UserRoles = Session["S_RolesName"];
            }
            else
            {
                using (ChartDbContext _context = new ChartDbContext())
                {
                    var userRoles = (from DataUSer in _context.RolesData
                                     where DataUSer.UserName == _username
                                     select DataUSer.RolesName).ToArray();

                    Session["S_RolesName"] = string.Join(",", userRoles);
                    ViewBag.UserRoles = Session["S_RolesName"];
                }

            }

            //try
            //{
            //    Response.AddHeader("Refresh", "120");
            //}
            //catch (Exception Ex)
            //{
            //    return View(Ex.Message.ToString());
            //}

            Response.AddHeader("Refresh", "300");

            return View();
        }

    }
}