// -----------------------------------------------------------------------
// <copyright file="CoreController.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System.Web.Mvc;
using Fanex.Ares.Core.Criteria;
using Fanex.Ares.Core.Services.Impl;
using Fanex.Ares.Core.Services.Interface;
using Fanex.Ares.Core.Web.Controllers;

namespace Fanex.Ares.Core.Controllers
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class CommonController : BaseController
    {
        private ICoreService _coreService;

        public CommonController() {
            _coreService = new CoreService();
        }

        [HttpPost]
        public JsonResult GetLicensees()
        {
            return Json(_coreService.GetAllLicensees());
        }

        [HttpPost]
        public JsonResult GetBettypes(GetBettypeCriteria criteria)
        {
            return Json(_coreService.GetBettypes(criteria));
        }
    }
}
