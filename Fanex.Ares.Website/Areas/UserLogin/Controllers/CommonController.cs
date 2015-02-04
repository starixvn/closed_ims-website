namespace Fanex.Ares.Website.Areas.UserLogin.Controllers
{
    using System.Web.Mvc;
    using System.Web.Security;
    using Fanex.NAP.Core.Service.Impl;
    using Fanex.NAP.Core.Service.Interface;

    /// <summary>
    /// Common Controller.
    /// </summary>
   public class CommonController : Controller
    {
        private readonly IMaintenanceService _maintenanceService;

        public CommonController() : this(null)
        {
        }

        public CommonController(IMaintenanceService maintenanceService)
        {
            _maintenanceService = maintenanceService ?? new MaintenanceService();
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
