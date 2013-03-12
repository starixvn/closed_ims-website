// --------------------------------------------------------------------------
// <copyright file="DeleteSingleBetController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.1.25">Created</change>
// </history>


namespace Fanex.TicketControl.Gaming.Controllers
{
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Mvc;
    using Fanex.TicketControl.Core.Web.Controllers;
    using Fanex.TicketControl.Core.Criteria;
    using Fanex.TicketControl.Gaming.Criteria;
    using Fanex.TicketControl.Gaming.Services.Interface;
    using Fanex.TicketControl.Gaming.Services.Impl;
    using Fanex.TicketControl.Core.Services.Interface;
    using Fanex.TicketControl.Core.Services.Impl;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class VoidNumberGameTicketController : BaseController
    {
        IVoidNumberGameTicketService _voidNumberGameTicketService;
        ICoreService _coreService;

        public VoidNumberGameTicketController()
        {
            _voidNumberGameTicketService = new VoidNumberGameTicketService();
            _coreService = new CoreService();
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult CheckMatchIdWithRefno(CheckMatchIdWithRefNoCriteria criteria)
        {
            criteria.Type = Core.Utils.Helper.SportType.NumberGame;
            return Json(_coreService.CheckMatchIdWithRefno(criteria));
        }

        [HttpPost]
        public JsonResult GetNumberGameTicket(GetNumberGameTicketCriteria criteria)
        {
            return Json(_voidNumberGameTicketService.GetNumberGameTicket(criteria));
        }
    }
}