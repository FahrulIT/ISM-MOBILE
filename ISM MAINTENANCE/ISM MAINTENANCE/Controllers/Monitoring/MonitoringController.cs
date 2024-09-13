using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ISM_MAINTENANCE.Models.DB;
using Newtonsoft.Json;
using System.Globalization;
using System.Data.SqlClient;
using System.Data;

namespace ISM_MAINTENANCE.Controllers
{
    public class MonitoringController : Controller
    {

        private string connString = "Server=192.168.21.10\\istem_cim;Database=istem_sms;MultipleActiveResultSets=True;User Id=sa;Password=@istem123;";
        private WvMaintenanceEntities db = new WvMaintenanceEntities();
        private void InitializeUserAkses()
        {
            if (db.v_user_ism_maintenance.Where(x => x.ID == User.Identity.Name.ToString().Trim()).FirstOrDefault() == null)
            {
                ViewBag.Section = ""; ViewBag.Roles = ""; ViewBag.UserAksesMenu = "";
            }
            else
            {
                ViewBag.UserAksesMenu = db.ms_user_access_menu.Where(x => x.userid == User.Identity.Name.ToString().Trim()).ToList();
                ViewBag.Section = db.v_user_ism_maintenance.Where(x => x.ID == User.Identity.Name.ToString().Trim()).FirstOrDefault().section.ToString().Trim();
                ViewBag.Roles = db.v_user_ism_maintenance.Where(x => x.ID == User.Identity.Name.ToString().Trim()).FirstOrDefault().roles.ToString().Trim();
            }
        }

        private class trouble
        {
            public string troubleId { get; set; }
            public string troubleName { get; set; }
        }

        private class chart
        {
            public string chartValue { get; set; }
            public string chartText { get; set; }
        }

        private List<SelectListItem> graphicType()
        {
            List<SelectListItem> grap = new List<SelectListItem>();
            grap.Add(new SelectListItem { Value = "", Text = "Daily Monitoring" });
            grap.Add(new SelectListItem { Value = "Progress", Text = "Progress Monitoring" });
            grap.Add(new SelectListItem { Value = "Trend", Text = "Trend Monitoring" });

            return grap;
        }

        //private List<SelectListItem> typeOfChart()
        //{
        //    List<SelectListItem> chart = new List<SelectListItem>();
        //    SqlConnection conn = new SqlConnection(connString);
        //    conn.Open();

        //    var sql = "EXEC [chart_maintenance_machine] " +
        //        "'" + DateTime.Now.ToString("yyyy-MM-01") + "'," +
        //        "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
        //        "''," +
        //        "''," +
        //        "'chart' ";
        //    SqlCommand comm = new SqlCommand(sql, conn);

        //    DataTable dt = new DataTable();
        //    dt.Load(comm.ExecuteReader());

        //    foreach (DataRow item in dt.Rows)
        //    {
        //        chart.Add(new SelectListItem { Value = item[0].ToString(), Text = item[1].ToString() });
        //    }

        //    comm.Dispose();
        //    conn.Close();

        //    return chart;
        //}

        private List<SelectListItem> troubleName()
        {
            List<SelectListItem> trouble = new List<SelectListItem>();
            trouble.Add(new SelectListItem { Value = "ALL", Text = "ALL" });

            SqlConnection conn = new SqlConnection(connString);
            conn.Open();

            var sql = "EXEC [chart_maintenance_machine] " +
                "'" + DateTime.Now.ToString("yyyy-MM-01") + "'," +
                "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                "''," +
                "'ALL'," +
                "'trouble' ";
            SqlCommand comm = new SqlCommand(sql, conn);

            DataTable dt = new DataTable();
            dt.Load(comm.ExecuteReader());

            foreach (DataRow item in dt.Rows)
            {
                trouble.Add(new SelectListItem { Value = item[0].ToString(), Text = item[1].ToString() });
            }

            comm.Dispose();
            conn.Close();

            return trouble;
        }

        private List<SelectListItem> maintenanceType()
        {
            List<SelectListItem> main = new List<SelectListItem>();
            main.Add(new SelectListItem { Value = "Machine", Text = "Machine Trouble" });
            main.Add(new SelectListItem { Value = "Quality", Text = "Quality Trouble" });
            main.Add(new SelectListItem { Value = "Preventive", Text = "Preventive" });

            return main;
        }

        // GET: Monitoring
        public ActionResult Index()
        {
            InitializeUserAkses(); 

            ViewBag.graphicType = graphicType();
            //ViewBag.typeOfChart = typeOfChart();
            ViewBag.troubleName = troubleName();
            ViewBag.maintenanceType = maintenanceType();

            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult getDataTrouble(string periodFrom, string periodTo, string trouble,string chk)
        {
            InitializeUserAkses();

            string[] f = periodFrom.Split('-');
            string from = f[2] + "-" + f[1] + "-" + f[0];
            periodFrom = from;

            string[] t = periodTo.Split('-');
            string to = t[2] + "-" + t[1] + "-" + t[0];
            periodTo = to;

            var sql = "";
            sql = "EXEC [chart_maintenance_" + chk + "] " +
                "'" + periodFrom + "'," +
                "'" + periodTo + "'," +
                "'" + chk + "'," +
                "'ALL'," +
                "'" + trouble + "' ";
            var troubleName = db.Database.SqlQuery<trouble>(sql).ToList();

            var settings = new JsonSerializerSettings() { ContractResolver = null };
            var a = JsonConvert.SerializeObject(troubleName, settings);
            return Content(a, "application/json");
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult getDataChart(string periodFrom, string periodTo, string trouble, string chk)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();

            var result = new Dictionary<string, object>();
            List<Dictionary<string, string>> datas = new List<Dictionary<string, string>>();

            SqlConnection conn = new SqlConnection(connString);
            conn.Open();

            string[] f = periodFrom.Split('-');
            string from = f[2] + "-" + f[1] + "-" + f[0];
            periodFrom = from;

            string[] t = periodTo.Split('-');
            string to = t[2] + "-" + t[1] + "-" + t[0];
            periodTo = to;

            var sql = "";
            sql = "EXEC [chart_maintenance_" + chk +"] " +
                "'" + periodFrom + "'," +
                "'" + periodTo + "'," +
                "'" + chk + "'," +
                "'" + trouble + "'," +
                "'' ";
            
            SqlCommand comm = new SqlCommand(sql, conn);
            da.SelectCommand = comm;
            da.Fill(ds);

            ////untuk kebutuhan checking data & closing koneksi
            dt.Load(comm.ExecuteReader());
            comm.Dispose();
            conn.Close();

            foreach (DataRow row in dt.Rows)
            {
                var cols = new Dictionary<string, string>();
                int index = 0;

                foreach (DataColumn col in dt.Columns)
                {
                    if (index != 2)
                    {
                        if (col.ColumnName.Contains("Date"))
                        {

                            cols.Add(col.ColumnName, row[index].ToString());
                        } else
                        {

                            cols.Add(col.ColumnName, row[index].ToString().Replace(",", "."));
                        }
                    } else
                    {
                        cols.Add(col.ColumnName, row[index].ToString());
                    }

                    index = index + 1;
                }

                datas.Add(cols);
            }

            result.Add("data", datas);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}