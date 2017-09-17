using System.Web;
using System.Web.Mvc;

namespace MW2KT_ASP.NET_NET_framework
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
