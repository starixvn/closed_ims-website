namespace Fanex.TicketControl.Gaming
{
    using System.Web.Mvc;

    public class RouteConfig
    {
        public static void RegisterRoutes(string areaName, AreaRegistrationContext context)
        {
            context.MapRoute(
                areaName + "_Default",
                "Gaming/{controller}/{action}/{id}",                
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "Fanex.TicketControl.Gaming.Controllers" });
        }
    }
}