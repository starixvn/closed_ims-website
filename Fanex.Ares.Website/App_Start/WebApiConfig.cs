namespace Fanex.Ares.Website
{
    using System.Web.Http;

    /// <summary>
    /// Web Config.
    /// </summary>
    public static class WebApiConfig
    {
        /// <summary>
        /// Register Router.
        /// </summary>
        /// <param name="config">Router Configuration.</param>
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });
        }
    }
}
