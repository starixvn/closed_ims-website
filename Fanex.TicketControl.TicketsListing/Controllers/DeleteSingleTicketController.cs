// --------------------------------------------------------------------------
// <copyright file="DeleteSingleBetController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.1.25">Created</change>
// </history>


namespace Fanex.TicketControl.SportBook.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.NAP.Core.Web.Attribute;
    using Fanex.NAP.Core.Business;
    using Fanex.TicketControl.SportBook.Criteria;
    using Fanex.TicketControl.SportBook.Services.Interface;
    using Fanex.TicketControl.SportBook.Services.Implement;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    [NapSuperAuth]
    public class DeleteSingleTicketController : Controller
    {
        ISingleTickketService _service;

        public DeleteSingleTicketController()
        {
            _service = new SingleTickketService();
        }

        [HttpGet]
        [NapAuth(FunctionKey = NAPSpecialDeleteFunctionKeys.SpecialDeleteSportBook, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            return View();
        }
               

        [HttpPost]
        public JsonResult SelBettype(GetBettypeCriteria criteria)
        {
            var bettypes = _service.SelBettype(criteria);
            return base.Json(bettypes, JsonRequestBehavior.AllowGet); 
        }
    }
}