// --------------------------------------------------------------------------
// <copyright file="InvoiceAreaRegistration.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Ares.Website.Areas.Invoice
{
    using System.Web.Mvc;
    using System.Web.Optimization;
    using Fanex.Ares.Website.Areas.Invoice.Area_Start;

    /// <summary>
    /// Invoice Area Registration.
    /// </summary>
    public class InvoiceAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Invoice";
            }
        }

        /// <summary>
        /// Register Area.
        /// </summary>
        /// <param name="context">AreaRegistration Context.</param>
        public override void RegisterArea(AreaRegistrationContext context)
        {
            RouteConfig.RegisterRoutes(this.AreaName, context);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}