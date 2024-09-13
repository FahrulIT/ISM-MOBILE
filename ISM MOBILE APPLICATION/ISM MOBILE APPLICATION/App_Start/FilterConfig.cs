using System.Web;
using System.Web.Mvc;

namespace ISM_MOBILE_APPLICATION
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
