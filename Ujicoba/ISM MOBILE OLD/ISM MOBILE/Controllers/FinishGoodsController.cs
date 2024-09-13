﻿using ISM_MOBILE.Data;
using ISM_MOBILE.Models.Chart;
using Newtonsoft.Json;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ISM_MOBILE.Controllers
{
    public class FinishGoodsController : Controller
    {
        private AppDbContext db = new AppDbContext();

        // GET: FinnishGoods
        public ActionResult Index()
        {
            IQueryable<FinishGoodDonutChart> DonutChart =
                  from RawData in db.FGStocks.Where(s => s.item_category== "SpunPoly" || s.item_category=="TR")
                  group RawData by RawData.item_category into Temp
                  select new FinishGoodDonutChart()
                  {
                      Item = Temp.Key,
                      Value = Temp.Sum(p => p.meter)
                  };

            var Model_TR_SpunPoly = DonutChart.ToList();

            DonutChart = from RawData in db.FGStocks
                                     group RawData by RawData.de into Temp
                                     select new FinishGoodDonutChart()
                                     {
                                         Item = Temp.Key,
                                         Value = Temp.Sum(p => p.meter)
                                     };

            var Model_Dom_Exp = DonutChart.ToList();

            DonutChart = from RawData in db.FGStocks
                         group RawData by RawData.category into Temp
                         select new FinishGoodDonutChart()
                         {
                             Item = Temp.Key,
                             Value = Temp.Sum(p => p.meter)
                         };

            var Model_Pass_NonPass = DonutChart.ToList();

            var datachart = new object[Model_TR_SpunPoly.Count];
            int j = 0;

            foreach (var i in Model_TR_SpunPoly)
            {
                datachart[j] = new object[] { i.Item.ToString(), i.Value };
                j = j + 1;
            }

            string datastr = JsonConvert.SerializeObject(datachart, Formatting.None);
            ViewBag.strDonutChart_TR_SpunPoly = new HtmlString(datastr);
            
            datachart = new object[Model_Dom_Exp.Count];
            j = 0;

            foreach (var i in Model_Dom_Exp)
            {
                datachart[j] = new object[] { i.Item.ToString(), i.Value };
                j = j + 1;
            }
            
            datastr = JsonConvert.SerializeObject(datachart, Formatting.None);
            ViewBag.strDonutChart_Dom_Exp = new HtmlString(datastr);

            datachart = new object[Model_Pass_NonPass.Count];
            j = 0;

            foreach (var i in Model_Pass_NonPass)
            {
                datachart[j] = new object[] { i.Item.ToString(), i.Value };
                j = j + 1;
            }

            datastr = JsonConvert.SerializeObject(datachart, Formatting.None);
            ViewBag.strDonutChart_Pass_NonPass = new HtmlString(datastr);

            return View();
        }
    }
}