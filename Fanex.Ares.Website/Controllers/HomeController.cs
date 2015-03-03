namespace Fanex.Ares.Website.Controllers
{
    using System.Web.Mvc;
    using Fanex.Ares.Core.Web.Controllers;
    using Fanex.NAP.Core.Web.Attribute;

    /// <summary>
    /// Home Controller.
    /// </summary>
    [NapSuperAuth]
    public class HomeController : BaseController
    {
        /// <summary>
        /// Index page.
        /// </summary>
        /// <returns>Return view.</returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// User menu.
        /// </summary>
        /// <returns>Return result.</returns>
        [ChildActionOnly]
        public PartialViewResult _Menu()
        {
            ViewBag.MenuItemList = CurrentSession.MenuItemList;
            return PartialView("~/Views/Shared/_Menu.cshtml");
        }
    }
}