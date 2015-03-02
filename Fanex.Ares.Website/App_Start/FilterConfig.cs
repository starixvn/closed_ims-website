namespace Fanex.Ares.Website
{
    using System.Web.Mvc;

    /// <summary>
    /// Filter Config.
    /// </summary>
    public static class FilterConfig
    {
        /// <summary>
        /// Register Global Filters.
        /// </summary>
        /// <param name="filters">Global Filters collection.</param>
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}