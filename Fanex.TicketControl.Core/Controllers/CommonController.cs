// -----------------------------------------------------------------------
// <copyright file="CoreController.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.TicketControl.Core.Controllers
{
    using System.Web.Mvc;
    using Fanex.TicketControl.Core.Services.Impl;
    using Fanex.TicketControl.Core.Services.Interface;
    using Fanex.TicketControl.Core.Web.Controllers;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class CommonController : BaseController
    {
        ICoreService _coreService;
        public CommonController() {
            _coreService = new CoreService();
        }

        [HttpPost]
        public JsonResult GetLicensees()
        {
            return Json(_coreService.GetAllLicensees());
        }
    }
}
