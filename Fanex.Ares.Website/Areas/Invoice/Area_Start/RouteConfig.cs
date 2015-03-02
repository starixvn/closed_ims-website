namespace Fanex.Ares.Website.Areas.Invoice
{
    using System.Web.Mvc;

    /// <summary>
    /// Route Config.
    /// </summary>
    public static class RouteConfig
    {
        /// <summary>
        /// Register Routes.
        /// </summary>
        /// <param name="areaName">Area Name.</param>
        /// <param name="context">Area Registration Context.</param>
        public static void RegisterRoutes(string areaName, AreaRegistrationContext context)
        {
            context.MapRoute(
                areaName + "_Default",
                "Invoice/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "Fanex.Ares.Invoice.Controllers" });
        }
    }
}