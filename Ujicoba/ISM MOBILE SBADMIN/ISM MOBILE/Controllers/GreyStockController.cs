using ISM_MOBILE.Data;
using ISM_MOBILE.Models.Chart;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ISM_MOBILE.Controllers
{
    [Authorize]
    public class GreyStockController : Controller
    {
        private AppDbContext db = new AppDbContext();

        // GET: GreyStock
        public ActionResult Index()
        {
            //GREY STOCK
            IQueryable<GreyStockDonutChart> DonutChartGreyStock_dt =
                from Grey in db.GreyStocks
                group Grey by Grey.grade into temp
                select new GreyStockDonutChart()
                {
                    Grade = temp.Key,
                    Value = temp.Sum(p => p.meter)
                };

            var Grey_list = DonutChartGreyStock_dt.ToList();

            foreach (var item in Grey_list.Where(s => s.Grade == "AS" || s.Grade == "BS" || s.Grade == "CS" || s.Grade == "A2S" || s.Grade == "A3S"))
            {

                if (item.Grade.Length < 3) { item.Grade = item.Grade.Substring(0, 1).ToString(); }
                else { item.Grade = item.Grade.Substring(0, 2).ToString(); }
            }
            
            var Results = from A in Grey_list
                          group A by A.Grade into hallo                         
                          select new GreyStockDonutChart
                          {
                              Grade = hallo.First().Grade,
                              Value = hallo.Sum(p => p.Value)
                          };

            List<string> codeValueSortOrder = new List<String> { "A3", "A2", "A", "B", "C"};
     
            var SortResults = Results.OrderBy(i => codeValueSortOrder.IndexOf(i.Grade)).ToList();

            //var Grey_obj = new object[Results.ToList().Count];
            var Grey_obj = new object[SortResults.ToList().Count];
            int j = 0;

            foreach (var i in SortResults.ToList())
            {
                Grey_obj[j] = new object[] { i.Grade.ToString(), i.Value };
                j = j + 1;
            }

            string str_grey = JsonConvert.SerializeObject(Grey_obj, Formatting.None);
            ViewBag.strDonutChart_Grey = new HtmlString(str_grey);
            ViewBag.strTotalGrey = Grey_list.Sum(x => x.Value).ToString("#,##0.00");

            ////grey stock stack bar
            IQueryable<GreyStockStackBarHorizontalChart> StackBarChartGreyStock_dt =
                from Grey in db.GreyStocks
                group Grey by Grey.grey_no into temp
                select new GreyStockStackBarHorizontalChart()
                {
                    //Grade = temp.FirstOrDefault().grade.Length < 3 ? temp.FirstOrDefault().grade.Substring(0, 1).ToString() : temp.FirstOrDefault().grade.Substring(0, 2).ToString(),
                    Item = temp.FirstOrDefault().grey_no,
                    A = string.IsNullOrEmpty(temp.Where(c => c.grade == "A" || c.grade == "AS").Sum(c => c.meter).ToString()) ? 0 : temp.Where(c => c.grade == "A" || c.grade == "AS").Sum(c => c.meter),
                    A2 = string.IsNullOrEmpty(temp.Where(c => c.grade == "A2" || c.grade == "A2S").Sum(c => c.meter).ToString()) ? 0 : temp.Where(c => c.grade == "A2" || c.grade == "A2S").Sum(c => c.meter),
                    A3 = string.IsNullOrEmpty(temp.Where(c => c.grade == "A3" || c.grade == "A3S").Sum(c => c.meter).ToString()) ? 0 : temp.Where(c => c.grade == "A3" || c.grade == "A3S").Sum(c => c.meter),
                    B = string.IsNullOrEmpty(temp.Where(c => c.grade == "B" || c.grade == "BS").Sum(c => c.meter).ToString()) ? 0 : temp.Where(c => c.grade == "B" || c.grade == "BS").Sum(c => c.meter),
                    C = string.IsNullOrEmpty(temp.Where(c => c.grade == "C" || c.grade == "CS").Sum(c => c.meter).ToString()) ? 0 : temp.Where(c => c.grade == "C" || c.grade == "CS").Sum(c => c.meter)
                };

            //IQueryable<GreyStockStackBarHorizontalChart> StackBarChartGreyStock_dt = from Data in db.GreyStocks group Data by Data.grade into temp select new GreyStockStackBarHorizontalChart() { Grade = temp.Key, Value = temp.Sum(s=> s.meter) };

            var results = StackBarChartGreyStock_dt.ToList();
            var GreyBarChart_obj = new object[results.Count];
            j = 0;

            foreach (var i in results)
            {
                GreyBarChart_obj[j] = new object[] { i.Item, i.A3, i.A2, i.A, i.B, i.C, i.A3+i.A2+i.A+i.B+i.C  };
                j = j + 1;
            }

            string str_grey_barchart = JsonConvert.SerializeObject(GreyBarChart_obj, Formatting.None);
            ViewBag.strBarChart_Grey = new HtmlString(str_grey_barchart);

            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (db != null)
                {
                    db.Dispose();
                    db = null;
                }
            }

            base.Dispose(disposing);
        }
    }
}