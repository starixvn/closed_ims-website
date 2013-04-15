// --------------------------------------------------------------------------
// <copyright file="DeleteSingleBetController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.1.25">Created</change>
// </history>


using System.Web.Mvc;
using System.Diagnostics.CodeAnalysis;
using Fanex.Ares.Core.Web;
using Fanex.Ares.Invoice.Criteria;
using Fanex.Ares.Invoice.Services.Implement;
using Fanex.Ares.Invoice.Services.Interface;
using Fanex.NAP.Core.Web.Attribute;
using Fanex.NAP.Core.Business;

namespace Fanex.Ares.Invoice.Controllers
{
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
        [NapAuth(FunctionKey = NAPSpecialDeleteFunctionKeys.SpecialDelete, Policy = Constant.Policy.Read)]
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