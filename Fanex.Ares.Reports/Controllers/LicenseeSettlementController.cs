// --------------------------------------------------------------------------
// <copyright file="LicenseeSettlementController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.1.25">Created</change>
// </history>


using System.Diagnostics.CodeAnalysis;
using System.Web.Mvc;
using Fanex.Ares.Core.Utils;
using Fanex.Ares.Core.Web;
using Fanex.Ares.Core.Web.Controllers;
using Fanex.Ares.Report.Criteria;
using Fanex.Ares.Report.Services.Implement;
using Fanex.Ares.Report.Services.Interface;
using Fanex.NAP.Core.Web.Attribute;
using Fanex.NAP.Core.Business;
using Fanex.Ares.Report.Criteria.LicenseeSettlement;
using System.Collections.Generic;

namespace Fanex.Ares.Report.Controllers
{
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class LicenseeSettlementController : BaseController
    {
        ILicenseeSettlementService _service;

        public LicenseeSettlementController()
        {
            _service = new LicenseeSettlementService();
        }

        //[NapAuth(FunctionKey = NAPSpecialDeleteFunctionKeys.VoidSportsbookBet, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            ViewBag.ListLicensee = _service.GetAllLicensees();
            var licenseeID = int.Parse(((List<SelectListItem>)ViewBag.ListLicensee)[0].Value);
            ViewBag.ListProduct = _service.GetProductsByLicensee(new IMS_LicenseeProductSelCriteria() { LicenseeId = licenseeID });
            var listCurrency = _service.GetCurrencyByLicensee(new IMS_LicenseeCurrencySelCriteria() { LicenseeId = licenseeID });
            listCurrency.Add(new SelectListItem() { Selected = true, Text = "All", Value = "0" });
            ViewBag.ListCurrency = listCurrency;
            var listExchangeRate = new List<SelectListItem>();
            listExchangeRate.Add(new SelectListItem() { Selected = true, Text = "EUR", Value = "6" });
            listExchangeRate.Add(new SelectListItem() { Selected = false, Text = "USD", Value = "3" });
            ViewBag.ListExchangeRate = listExchangeRate;

            return View();
        }

        //[NapAuth(FunctionKey = NAPSpecialDeleteFunctionKeys.VoidSportsbookBet, Policy = Constant.Policy.Host)]
        //[HttpPost]
        //public JsonResult RecoverTicket(RecoverTicketCriteria criteria)
        //{
        //    criteria.UserId = CurrentSession.UserId;
        //    criteria.Ip = Helper.GetClientIP();
        //    criteria.ErrCode = 0;
        //    criteria.ErrMsg = string.Empty;

        //    criteria.Msg.CreatorId = criteria.UserId;
        //    criteria.Msg.CreatorName = CurrentSession.Username;

        //    var rs = _service.RecoverTicket(criteria);
        //    return Json(rs);
        //}
    }
}