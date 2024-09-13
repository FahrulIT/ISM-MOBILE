using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ISM_MAINTENANCE
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            System.Globalization.CultureInfo.DefaultThreadCurrentCulture = new System.Globalization.CultureInfo("en-US");
            System.Globalization.CultureInfo.DefaultThreadCurrentUICulture = new System.Globalization.CultureInfo("en-US");
            System.Globalization.CultureInfo.DefaultThreadCurrentUICulture.NumberFormat.NumberDecimalSeparator = ".";
            System.Globalization.CultureInfo.DefaultThreadCurrentCulture.NumberFormat.NumberDecimalSeparator = ".";
      
            //System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("en-US", true);
            //culture.NumberFormat.NumberDecimalSeparator = ".";
            //System.Threading.Thread.CurrentThread.CurrentCulture = culture;
            //System.Threading.Thread.CurrentThread.CurrentUICulture = culture;


            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
