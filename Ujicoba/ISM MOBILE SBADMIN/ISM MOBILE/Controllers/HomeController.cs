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
    public class HomeController : Controller
    {
       
        private AppDbContext db = new AppDbContext();

       
        public ActionResult Index()
        {
            IQueryable<YarnStockDonutChart> DonutChart_dt =
             from RawData in db.YarnStocks
             group RawData by RawData.type into Temp
             select new YarnStockDonutChart()
             {
                 Type = Temp.Key,
                 Value = Temp.Sum(p => p.lbs)
             };

            var DataModel = DonutChart_dt.ToList();
            var datachart = new object[DataModel.Count];
            int j = 0;

            foreach (var i in DataModel)
            {
                datachart[j] = new object[] { i.Type.ToString(), i.Value };
                j = j + 1;
            }

            string datastr = JsonConvert.SerializeObject(datachart, Formatting.None);
            ViewBag.strDonutChart = new HtmlString(datastr);
            ViewBag.StrTotalYarn = (DataModel.Sum(x => x.Value)).ToString("#,##0.00");


            //GREY STOCK
            IQueryable<GreyStockDonutChart> DonutChartGreyStock_dt =
                from Grey in db.GreyStocks
                group Grey by Grey.grade into temp
                select new GreyStockDonutChart()
                {
                    Grade = temp.Key,
                    Value = temp.Sum(p => p.meter)
                };

            //.Where(s => s.grade == "A" || s.grade == "B" || s.grade == "C" || s.grade == "A2" || s.grade == "A3")
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

            List<string> codeValueSortOrder = new List<String> { "A3", "A2", "A", "B", "C" };

            var SortResults = Results.OrderBy(i => codeValueSortOrder.IndexOf(i.Grade)).ToList();

            var Grey_obj = new object[SortResults.ToList().Count];
            j = 0;

            foreach (var i in SortResults.ToList())
            {
                Grey_obj[j] = new object[] { i.Grade.ToString(), i.Value };
                j = j + 1;
            }

            string str_grey = JsonConvert.SerializeObject(Grey_obj, Formatting.None);
            ViewBag.strDonutChart_Grey = new HtmlString(str_grey);
            ViewBag.strTotalGrey = Grey_list.Sum(x=>x.Value).ToString("#,##0.00");

            IQueryable<FinishGoodDonutChart> DonutChart =
                from RawData in db.FGStocks
                group RawData by RawData.category into Temp
                select new FinishGoodDonutChart()
                {
                    Item = Temp.Key,
                    Value = Temp.Sum(p => p.meter)
                };

            var Model_Pass_NonPass = DonutChart.ToList();
            var datachartNonPass = new object[Model_Pass_NonPass.Count];
            j = 0;

            foreach (var i in Model_Pass_NonPass)
            {
                datachartNonPass[j] = new object[] { i.Item.ToString(), i.Value };
                j = j + 1;
            }

            datastr = JsonConvert.SerializeObject(datachartNonPass, Formatting.None);
            ViewBag.strDonutChart_Pass_NonPass = new HtmlString(datastr);
            ViewBag.strTotalFinishgood = Model_Pass_NonPass.Sum(x => x.Value).ToString("#,##0.00");
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