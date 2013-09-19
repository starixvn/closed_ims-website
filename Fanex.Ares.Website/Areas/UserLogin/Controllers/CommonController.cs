// --------------------------------------------------------------------------
// <copyright file="CommonController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Ares.Website.Areas.UserLogin.Controllers
{
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Mvc;
    using System.Web.Security;
    using Fanex.NAP.Core.Service.Impl;
    using Fanex.NAP.Core.Service.Interface;

    /// <summary>
    /// Common Controller.
    /// </summary>
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Common Controller")]
    public class CommonController : Controller
    {
        private readonly IMaintainanceService _maintenanceService;

        public CommonController() : this(null)
        {
        }

        public CommonController(IMaintainanceService maintenanceService)
        {
            _maintenanceService = maintenanceService ?? new MaintainanceService();
        }

        /// <summary>
        /// Checking down time.
        /// </summary>
        /// <returns>Return result.</returns>
        public ActionResult Offline()
        {
            var downTime = _maintenanceService.GetDowntimeInfo();

            if (!downTime.IsDowntime)
            {
                FormsAuthentication.RedirectToLoginPage();
            }

            ViewBag.UmtFrom = downTime.From;
            ViewBag.UmtTo = downTime.To;

            return View();
        }

        /// <summary>
        /// Access denied.
        /// </summary>
        /// <returns>Return view.</returns>
        public ActionResult AccessDenied()
        {
            return View("AccessDenied");
        }

        /// <summary>
        /// Page not found.
        /// </summary>
        /// <returns>Return view.</returns>
        public ActionResult PageNotFound()
        {
            return View("~/Views/Common/PageNotFound.cshtml");
        }

        /// <summary>
        /// Error page.
        /// </summary>
        /// <returns>Return view.</returns>
        public ActionResult Error()
        {
            return View("~/Views/Common/Error.cshtml");
        }
    }
}
