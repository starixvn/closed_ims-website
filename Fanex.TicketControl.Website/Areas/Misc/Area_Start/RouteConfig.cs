namespace Fanex.TicketControl.Misc
{
    using System.Web.Mvc;

    public class RouteConfig
    {
        public static void RegisterRoutes(string areaName, AreaRegistrationContext context)
        {
            context.MapRoute(
                areaName + "_Default",
                "Misc/{controller}/{action}/{id}",                
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "Fanex.TicketControl.Misc.Controllers" });
        }
    }
}