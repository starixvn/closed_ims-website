// --------------------------------------------------------------------------
// <copyright file="UserControlAreaRegistration.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Ares.Website.Areas.UserControl
{
    using System.Web.Mvc;
    using System.Web.Optimization;
    using Fanex.Ares.Website.App_Start;

    /// <summary>
    /// User Control Area Registration.
    /// </summary>
    public class UserControlAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "UserControl";
            }
        }

        /// <summary>
        /// Register Area.
        /// </summary>
        /// <param name="context">Area Registration Context.</param>
        public override void RegisterArea(AreaRegistrationContext context)
        {
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            context.MapRoute(
                this.AreaName + "_Default",
                "UserControl/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "Fanex.Ares.Website.Areas.UserControl.Controllers" });
        }
    }
}
