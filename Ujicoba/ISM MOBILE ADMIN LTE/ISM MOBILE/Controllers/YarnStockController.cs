using ISM_MOBILE.Data;
using ISM_MOBILE.Models.Chart;
using Newtonsoft.Json;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ISM_MOBILE.Controllers
{
    public class YarnStockController : Controller
    {
        private AppDbContext db = new AppDbContext();
        // GET: YarnStock
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
                BarChart_obj[j] = new object[] { i.Item.ToString(), i.Value, i.Annotation };
                j = j + 1;
            }

            string datastr2 = JsonConvert.SerializeObject(BarChart_obj, Formatting.None);
            ViewBag.strBarHorzChart = new HtmlString(datastr2);
            return View();
        }

        // GET: YarnStock/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: YarnStock/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: YarnStock/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: YarnStock/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: YarnStock/Edit/5
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

        // GET: YarnStock/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: YarnStock/Delete/5
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
