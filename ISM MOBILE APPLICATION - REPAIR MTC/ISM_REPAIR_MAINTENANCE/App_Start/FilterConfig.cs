using System.Web;
using System.Web.Mvc;

namespace ISM_REPAIR_MAINTENANCE
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
