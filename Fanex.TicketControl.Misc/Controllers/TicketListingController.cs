// --------------------------------------------------------------------------
// <copyright file="TicketListingController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.2.4">Created</change>
// </history>

namespace Fanex.TicketControl.Misc.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using Fanex.TicketControl.Misc.Services.Interface;
    using Fanex.TicketControl.Misc.Services.Impl;
    using Fanex.TicketControl.Core.Criteria;
    using Fanex.TicketControl.Misc.Criteria;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class TicketListingController : Controller
    {
        private readonly ITicketListingService _ticketListingService;

        public TicketListingController()
        {
            _ticketListingService = new TicketListingService();
        }

        public ActionResult TemplatePage()
        {
            return View();
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(GetTicketsListingCriteria criteria)
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetLicensees()
        {
            return Json(_ticketListingService.GetAllLicensees());
        }

        [HttpPost]
        public JsonResult GetBettypes(GetBettypeCriteria criteria)
        {
            return Json(_ticketListingService.GetAllBettypes(criteria));
        }

        
    }
}
