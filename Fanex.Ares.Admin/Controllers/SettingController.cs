// --------------------------------------------------------------------------
// <copyright file="SettingController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Phillip.Truong" date="2013.06.03">Created</change>
// </history>

namespace Fanex.Ares.Admin.Controllers
{
    using System;
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

    /// <summary>
    /// Setting Controller.
    /// </summary>
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Setting Controller")]
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

        /// <summary>
        /// Index page.
        /// </summary>
        /// <returns>Return view.</returns>
        [HttpGet]
        [AuthenticateAction(AuthenticateAction.AuthenticationType.Read, "Index")]
        public ActionResult Index()
        {
            ViewBag.lstLicensees = _licenseeService.GetAllLicensees(new IMS_LicenseeInfoSel()).ToSelectListItem("LicenseeID", "LicenseeName", "BrandName");
            ViewBag.lstParties = _partyService.GetAllParties(new IMS_PartyInfoSelCriteria()).ToSelectListItem("PartyID", "PartyName");
            return View();
        }

        /// <summary>
        /// Get all settings.
        /// </summary>
        /// <param name="criteria">Licensee - Product setting.</param>
        /// <returns>Return result.</returns>
        [HttpPost]
        [AuthenticateAction(AuthenticateAction.AuthenticationType.Read, "Index")]
        public JsonResult GetAllSettings(IMS_LicenseeProductSettingInfoSel criteria)
        {
            var rs = Json(_settingService.GetAllSettings(criteria));
            return rs;
        }

        /// <summary>
        /// Get all licensee product charge.
        /// </summary>
        /// <param name="criteria">Licensee - product.</param>
        /// <returns>Return result.</returns>
        [HttpPost]
        [AuthenticateAction(AuthenticateAction.AuthenticationType.Read, "Index")]
        public JsonResult GetAllLicenseeProductCharge(IMS_LicenseeProductChargeInfoSel criteria)
        {
            return Json(_settingService.GetAllLicenseeProductCharge(criteria));
        }

        /// <summary>
        /// Get all licensee product discount.
        /// </summary>
        /// <param name="criteria">Licensee product.</param>
        /// <returns>Return result.</returns>
        [HttpPost]
        [AuthenticateAction(AuthenticateAction.AuthenticationType.Read, "Index")]
        public JsonResult GetAllLicenseeProductDiscount(IMS_LicenseeProductDiscountInfoSel criteria)
        {
            return Json(_settingService.GetAllLicenseeProductDiscount(criteria));
        }

        /// <summary>
        /// Get Licensee by party id.
        /// </summary>
        /// <param name="criteria">The Party id.</param>
        /// <returns>Return result.</returns>
        [HttpPost]
        [AuthenticateAction(AuthenticateAction.AuthenticationType.Read, "Index")]
        public JsonResult GetLicenseesByPartyID(IMS_LicenseeSelByParty criteria)
        {
            return Json(_licenseeService.GetLicenseesByPartyID(criteria));
        }

        /// <summary>
        /// Get product by licensee and party.
        /// </summary>
        /// <param name="criteria">Licensee - party.</param>
        /// <returns>Return result.</returns>
        [HttpPost]
        [AuthenticateAction(AuthenticateAction.AuthenticationType.Read, "Index")]
        public JsonResult GetProductsByLicenseeIDPartyID(IMS_ProductSelByPartyLicensee criteria)
        {
            return Json(_settingService.GetProductsByLicenseeParty(criteria));
        }

        /// <summary>
        /// Add new setting.
        /// </summary>
        /// <returns>Return result.</returns>
        [AuthenticateAction(AuthenticateAction.AuthenticationType.Read, "Index")]
        public PartialViewResult _AddNew()
        {
            ViewBag.lstLicensees = _licenseeService.GetAllLicensees(new IMS_LicenseeInfoSel()).ToSelectListItem("LicenseeID", "LicenseeName", "BrandName");
            ViewBag.lstProducts = _productService.GetAllProducts(new IMS_ProductInfoSel()).ToSelectListItem("ProductID", "ProductName");
            ViewBag.lstParties = _partyService.GetAllParties(new IMS_PartyInfoSelCriteria()).ToSelectListItem("PartyID", "PartyName");
            ViewBag.lstCurrencies = _settingService.GetAllCurrenciesByLicensee(new IMS_LicenseeCurrencySelCriteria());

            return PartialView();
        }

        /// <summary>
        /// Licensee product charge page.
        /// </summary>
        /// <returns>Return view.</returns>
        [AuthenticateAction(AuthenticateAction.AuthenticationType.Read, "Index")]
        public PartialViewResult _LicenseeProductCharge()
        {
            ViewBag.lstCurrencies = _settingService.GetAllCurrenciesByLicensee(new IMS_LicenseeCurrencySelCriteria());
            return PartialView();
        }

        /// <summary>
        /// Licensee - product discount page.
        /// </summary>
        /// <returns>Return view.</returns>
        [AuthenticateAction(AuthenticateAction.AuthenticationType.Read, "Index")]
        public PartialViewResult _LicenseeProductDiscount()
        {
            ViewBag.lstCurrencies = _settingService.GetAllCurrenciesByLicensee(new IMS_LicenseeCurrencySelCriteria());
            return PartialView();
        }

        /// <summary>
        /// Create new setting.
        /// </summary>
        /// <param name="criteria">Licensee - product setting.</param>
        /// <returns>Redirect to index page.</returns>
        [HttpPost]
        [AuthenticateAction(AuthenticateAction.AuthenticationType.Host, "Index")]
        public ActionResult CreateNewSetting(IMS_LicenseeProductSettingIns criteria)
        {
            criteria.WaivedDate = criteria.WaivedDate == Convert.ToDateTime("1/1/0001") ? DateTime.Now.AddYears(50) : criteria.WaivedDate;
            criteria.FirstBillingDate = criteria.FirstBillingDate == Convert.ToDateTime("1/1/0001") ? DateTime.Now.AddYears(50) : criteria.FirstBillingDate;
            criteria.HeadOfTerms = criteria.HeadOfTerms == Convert.ToDateTime("1/1/0001") ? DateTime.Now.AddYears(50) : criteria.HeadOfTerms;
            TempData["Result"] = _settingService.ExecuteCommand(criteria);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Update setting.
        /// </summary>
        /// <param name="criteria">Licensee - product setting.</param>
        /// <returns>Redirect to index page.</returns>
        [HttpPost]
        [AuthenticateAction(AuthenticateAction.AuthenticationType.Host, "Index")]
        public ActionResult UpdateSetting(IMS_LicenseeProductSettingUpd criteria)
        {
            criteria.WaivedDate = criteria.WaivedDate == Convert.ToDateTime("1/1/0001") ? DateTime.Now.AddYears(50) : criteria.WaivedDate;
            criteria.FirstBillingDate = criteria.FirstBillingDate == Convert.ToDateTime("1/1/0001") ? DateTime.Now.AddYears(50) : criteria.FirstBillingDate;
            criteria.HeadOfTerms = criteria.HeadOfTerms == Convert.ToDateTime("1/1/0001") ? DateTime.Now.AddYears(50) : criteria.HeadOfTerms;
            TempData["Result"] = _settingService.ExecuteCommand(criteria);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Create licensee product charge.
        /// </summary>
        /// <param name="criteria">Licensee - product setting.</param>
        /// <returns>Redirect to index page.</returns>
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

        /// <summary>
        /// Create licensee - product discount.
        /// </summary>
        /// <param name="criteria">Licensee - product discount.</param>
        /// <returns>Redirect to index page.</returns>
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