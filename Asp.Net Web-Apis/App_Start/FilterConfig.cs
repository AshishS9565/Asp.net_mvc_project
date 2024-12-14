using System.Web;
using System.Web.Mvc;

namespace Asp.Net_Web_Apis
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
