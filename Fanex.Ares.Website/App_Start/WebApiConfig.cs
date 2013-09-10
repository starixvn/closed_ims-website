// --------------------------------------------------------------------------
// <copyright file="WebApiConfig.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Ares.Website.App_Start
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
