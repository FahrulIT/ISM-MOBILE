using System.Web;
using System.Web.Optimization;

namespace ISM_REPAIR_MAINTENANCE
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
            //         "~/Content/template/Modernize/libs/bootstrap/dist/js/bootstrap.bundle.min.js"));

            //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(                        
            //            "~/Content/template/Modernize/libs/jquery/dist/jquery.min.js",
            //            "~/Content/template/Modernize/libs/jquery/dist/jquery.js"));

            //bundles.Add(new StyleBundle("~/Content/css").Include(
            //         "~/Content/template/Modernize/css/styles.min.css"
            //          /*"~/Content/site.css"*/));

            //bundles.Add(new StyleBundle("~/datatable/css").Include(
            //         "~/Content/template/Modernize/libs/DataTables/css/dataTables.bootstrap5.css",
            //          "~/Content/template/Modernize/libs/DataTables/css/fixedColumns.bootstrap5.css",
            //          "~/Content/template/Modernize/libs/DataTables/css/buttons.bootstrap5.css"
            //          ));

            //bundles.Add(new ScriptBundle("~/Modernize/js").Include(
            //         "~/Content/template/Modernize/js/sidebarmenu.js",
            //         "~/Content/template/Modernize/js/app.min.js",
            //         "~/Content/template/Modernize/libs/simplebar/dist/simplebar.js"
            //         ));

            //bundles.Add(new ScriptBundle("~/datatable/js").Include(
            //       "~/Content/template/Modernize/libs/DataTables/js/dataTables.js",
            //      "~/Content/template/Modernize/libs/DataTables/js/dataTables.bootstrap5.js",
            //       "~/Content/template/Modernize/libs/DataTables/js/dataTables.fixedColumns.js",
            //      "~/Content/template/Modernize/libs/DataTables/js/fixedColumns.bootstrap5.js",
            //      "~/Content/template/Modernize/libs/DataTables/js/dataTables.buttons.js",
            //      "~/Content/template/Modernize/libs/DataTables/js/buttons.bootstrap5.js",
            //      "~/Content/template/Modernize/libs/DataTables/js/jszip.min.js",
            //      "~/Content/template/Modernize/libs/DataTables/js/pdfmake.min.js",
            //      "~/Content/template/Modernize/libs/DataTables/js/vfs_fonts.js",
            //      "~/Content/template/Modernize/libs/DataTables/js/buttons.html5.min.js",
            //      "~/Content/template/Modernize/libs/DataTables/js/buttons.print.min.js"
            //       ));

            //bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
            //      "~/Content/template/Modernize/libs/bootstrap/dist/js/bootstrap.bundle.min.js"));

            bundles.Add(new StyleBundle("~/bundles/AdminKit/css").Include(
                     "~/Content/template/AdminKit/css/app_colored.css"
                      /*"~/Content/site.css"*/));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery.min.js",
                        "~/Scripts/jquery.js"));


            bundles.Add(new ScriptBundle("~/bundles/AdminKit/js").Include(
                "~/Scripts/app.js"));


            bundles.Add(new ScriptBundle("~/bundles/ApexChart").Include(
                  "~/Scripts/apexcharts.js"));

            //bundles.Add(new ScriptBundle("~/bundles/AdminKit/ApexChart").Include(
            //            "~/Content/template/AdminKit/apexcharts/dist/apexcharts.js"));

            BundleTable.EnableOptimizations = true;
        }
    }
}
