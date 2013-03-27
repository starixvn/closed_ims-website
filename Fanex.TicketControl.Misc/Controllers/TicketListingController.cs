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
    using Fanex.TicketControl.Core.Web.Controllers;
    using Fanex.NAP.Core.Web.Attribute;
    using Fanex.NAP.Core.Business;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class TicketListingController : BaseController
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

        public ActionResult LewisIndex()
        {
            return View();
        }

        [NapAuth(FunctionKey = NAPSpecialDeleteFunctionKeys.TicketsListing, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            return View();
        }

        [NapAuth(FunctionKey = NAPSpecialDeleteFunctionKeys.TicketsListing, Policy = Constant.Policy.Read)]
        [HttpPost]
        public JsonResult Index(GetTicketsListingCriteria criteria)
        {
            return Json(_ticketListingService.GetTicketListing(criteria));
        }

        [NapAuth(FunctionKey = NAPSpecialDeleteFunctionKeys.TicketsListing, Policy = Constant.Policy.Read)]
        [HttpPost]
        public JsonResult GetTicketListingBetList(GetTicketsListingDetailCriteria criteria)
        {
            var result = _ticketListingService.GetTicketListingBetList(criteria);
            int skip = criteria.PageIndex * criteria.PageSize;
            result.Items = result.Items.Skip(skip).Take(criteria.PageSize).ToList();

            return Json(result);
        }

        [NapAuth(FunctionKey = NAPSpecialDeleteFunctionKeys.TicketsListing, Policy = Constant.Policy.Read)]
        public ActionResult ExportTicketListingBetList(GetTicketsListingDetailCriteria criteria)
        {
            Console.Write("Export");
            _ticketListingService.ExportTicketListingBetList(criteria);
            return new EmptyResult();
        }
    }
}
