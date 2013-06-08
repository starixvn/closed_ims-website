// --------------------------------------------------------------------------
// <copyright file="SettingController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Phillip.Truong" date="2013.06.03">Created</change>
// </history>

using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Web.Mvc;
using Fanex.Ares.Admin.Criteria;
using Fanex.Ares.Admin.Criteria.Setting;
using Fanex.Ares.Admin.Services.Party.Implement;
using Fanex.Ares.Admin.Services.Party.Interface;
using Fanex.Ares.Core.Extensions;
using Fanex.Ares.Core.Web.Controllers;
using Fanex.Ares.Core.Web.Security;
using Fanex.NAP.Core.Web.Attribute;

namespace Fanex.Ares.Admin.Controllers
{

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    [NapSuperAuth]
    public class SettingController : BaseController
    {
        private ISettingService _settingService;
        private ILicenseeService _licenseeService;
        private IProductService _productService;
        private IPartyService _partyService;

        public SettingController()
        {
            _settingService = new SettingService();
            _licenseeService = new LicenseeService();
            _productService = new ProductService();
            _partyService = new PartyService();
        }

        [HttpGet]
        [AuthenticateAction(AuthenticateAction.AuthenticationType.Read, "Index")]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [AuthenticateAction(AuthenticateAction.AuthenticationType.Read, "Index")]
        public JsonResult GetAllSettings()
        {
            return Json(_settingService.GetAllSettings(new IMS_LicenseeProductSettingInfoSel()));
        }

        [HttpPost]
        [AuthenticateAction(AuthenticateAction.AuthenticationType.Read, "Index")]
        public JsonResult GetAllLicenseeProductCharge(IMS_LicenseeProductChargeInfoSel criteria)
        {
            return Json(_settingService.GetAllLicenseeProductCharge(criteria));
        }

        [HttpPost]
        [AuthenticateAction(AuthenticateAction.AuthenticationType.Read, "Index")]
        public JsonResult GetAllLicenseeProductDiscount(IMS_LicenseeProductDiscountInfoSel criteria)
        {
            return Json(_settingService.GetAllLicenseeProductDiscount(criteria));
        }

        [AuthenticateAction(AuthenticateAction.AuthenticationType.Read, "Index")]
        public PartialViewResult _AddNew()
        {
            ViewBag.lstLicensees = _licenseeService.GetAllLicensees(new IMS_LicenseeInfoSel()).ToSelectListItem("LicenseeID","LicenseeName", "BrandName");
            ViewBag.lstProducts = _productService.GetAllProducts(new IMS_ProductInfoSel()).ToSelectListItem("ProductID", "ProductName");
            ViewBag.lstParties = _partyService.GetAllParties(new IMS_PartyInfoSel()).ToSelectListItem("PartyID", "PartyName");
            ViewBag.lstCurrencies = _settingService.GetCurrencyByLicensee(new Core.Criteria.IMS_LicenseeCurrencySelCriteria());

            return PartialView();
        }

        [AuthenticateAction(AuthenticateAction.AuthenticationType.Read, "Index")]
        public PartialViewResult _LicenseeProductCharge()
        {
            ViewBag.lstCurrencies = _settingService.GetCurrencyByLicensee(new Core.Criteria.IMS_LicenseeCurrencySelCriteria());
            return PartialView();
        }

        [AuthenticateAction(AuthenticateAction.AuthenticationType.Read, "Index")]
        public PartialViewResult _LicenseeProductDiscount()
        {
            ViewBag.lstCurrencies = _settingService.GetCurrencyByLicensee(new Core.Criteria.IMS_LicenseeCurrencySelCriteria());
            return PartialView();
        }

        [HttpPost]
        [AuthenticateAction(AuthenticateAction.AuthenticationType.Host, "Index")]
        public ActionResult CreateNewSetting(IMS_LicenseeProductSettingIns criteria)
        {
            TempData["Result"] = _settingService.ExecuteCommand(criteria);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [AuthenticateAction(AuthenticateAction.AuthenticationType.Host, "Index")]
        public ActionResult UpdateSetting(IMS_LicenseeProductSettingUpd criteria)
        {
            TempData["Result"] = _settingService.ExecuteCommand(criteria);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [AuthenticateAction(AuthenticateAction.AuthenticationType.Host, "Index")]
        public ActionResult CreateLicenseeProductCharge(IMS_LicenseeProductChargeMultipleIns criteria)
        {
            var rs = new Hashtable();
            if (!string.IsNullOrEmpty(criteria.XmlSettings))
            {
                rs = _settingService.ExecuteCommand(criteria);
            }

            TempData["Result"] = rs;
            return RedirectToAction("Index");
        }

        [HttpPost]
        [AuthenticateAction(AuthenticateAction.AuthenticationType.Host, "Index")]
        public ActionResult CreateLicenseeProductDiscount(IMS_LicenseeProductDiscountMultipleIns criteria)
        {
            var rs = new Hashtable();
            if (!string.IsNullOrEmpty(criteria.XmlSettings))
            {
                rs = _settingService.ExecuteCommand(criteria);
            }

            TempData["Result"] = rs;
            return RedirectToAction("Index");
        }

    }
}