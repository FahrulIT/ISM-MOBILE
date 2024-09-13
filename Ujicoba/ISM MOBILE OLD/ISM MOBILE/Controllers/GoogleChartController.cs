using ISM_MOBILE.Data;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity.Infrastructure;
using ISM_MOBILE.Models.Chart;
using Newtonsoft.Json;
using System.Web;
using System.Collections.Generic;
using System;

namespace ISM_MOBILE.Controllers
{
    public class GoogleChartController : Controller
    {
        private AppDbContext db = new AppDbContext();

        // GET: GoogleChart

        public ActionResult Index()
        {

            IQueryable<YarnStockDonutChart> DonutChart_dt =
                from RawData in db.YarnStocks
                group RawData by RawData.type into Temp
                select new YarnStockDonutChart()
                {
                    Type = Temp.Key,
                    Value = Temp.Sum(p=> p.lbs)
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

            IQueryable<YarnStockBarHorizontalChart> BarHorzChart_dt =
                from student in db.YarnStocks
                select new YarnStockBarHorizontalChart()
                {
                    Item = student.item_no.ToString(),
                    Value = student.lbs,
                    Annotation = student.type == "Production" ? "#dc3912" : "#3366cc"
                };

            var BarChart_list = BarHorzChart_dt.ToList();
            var BarChart_obj = new object[BarChart_list.Count];
            j = 0;

            foreach (var i in BarChart_list)
            {
                BarChart_obj[j] = new object[] { i.Item.ToString(), i.Value,  i.Annotation };
                j = j + 1;
            }

            string datastr2 = JsonConvert.SerializeObject(BarChart_obj, Formatting.None);
            ViewBag.strBarHorzChart = new HtmlString(datastr2);

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

            var Results = from A in Grey_list group A by A.Grade into hallo select new GreyStockDonutChart
            {
                Grade = hallo.First().Grade,
                Value = hallo.Sum(p => p.Value)
            };

            var Grey_obj = new object[Results.ToList().Count];
           j = 0;

            foreach (var i in Results.ToList())
            {
                Grey_obj[j] = new object[] { i.Grade.ToString(), i.Value };
               j = j + 1;
            }
            
            string str_grey = JsonConvert.SerializeObject(Grey_obj, Formatting.None);
            ViewBag.strDonutChart_Grey = new HtmlString(str_grey);

            ////grey stock stack bar
            IQueryable<GreyStockStackBarHorizontalChart> StackBarChartGreyStock_dt =
                from Grey in db.GreyStocks
                group Grey by Grey.grade into temp
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
                GreyBarChart_obj[j] = new object[] {i.Item, i.A, i.A2, i.A3, i.B, i.C };
                j = j + 1;
            }

            string str_grey_barchart = JsonConvert.SerializeObject(GreyBarChart_obj, Formatting.None);
            ViewBag.strBarChart_Grey = new HtmlString(str_grey_barchart);

            return View();

            

        }
    }
}