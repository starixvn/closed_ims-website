using System.Web.Mvc;

namespace Fanex.Ares.Website.Areas.Report.Area_Start
{
    public class RouteConfig
    {
        public static void RegisterRoutes(string areaName, AreaRegistrationContext context)
        {
            context.MapRoute(
                areaName + "_Default",
                "Report/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "Fanex.Ares.Report.Controllers" });
        }
    }
}