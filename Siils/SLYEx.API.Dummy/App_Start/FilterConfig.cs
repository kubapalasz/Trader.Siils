using System.Web;
using System.Web.Mvc;

namespace SLYEx.API.Dummy
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
