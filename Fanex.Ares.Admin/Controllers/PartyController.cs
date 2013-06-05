// --------------------------------------------------------------------------
// <copyright file="PartyController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Phillip.Truong" date="2013.06.03">Created</change>
// </history>

using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Web.Mvc;
using Fanex.Ares.Admin.Criteria;
using Fanex.Ares.Admin.Services.Party.Implement;
using Fanex.Ares.Admin.Services.Party.Interface;
using Fanex.Ares.Core.Web.Controllers;
using Fanex.Ares.Core.Web.Security;
using Fanex.NAP.Core.Web.Attribute;

namespace Fanex.Ares.Admin.Controllers
{

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    [NapSuperAuth]
    public class PartyController : BaseController
    {
        private IPartyService _partyService;

        public PartyController()
        {
            _partyService = new PartyService();
        }

        [HttpGet]
        [AuthenticateAction(AuthenticateAction.AuthenticationType.Read, "Index")]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetAllParties()
        {
            return Json(_partyService.GetAllParties(new IMS_PartyInfoSel()));
        }

        [HttpPost]
        public JsonResult GetAllPartyTypes()
        {
            return Json(_partyService.GetAllPartyTypes(new IMS_PartyInfoSel()));
        }

        public PartialViewResult _AddNew()
        {
            ViewBag.lstPartyTypes = _partyService.GetAllPartyTypes(new IMS_PartyTypeSel());
            return PartialView();
        }

        public PartialViewResult _PartyLicensee(IMS_LicenseeSelByParty criteria)
        {
            ViewBag.lstAvailableLicensee = _partyService.GetAvailableOrSelectedLicensees(new IMS_AvailableLicenseeSelByParty() { PartyID = criteria.PartyID });
            ViewBag.lstSelectedLicensee = _partyService.GetAvailableOrSelectedLicensees(new IMS_LicenseeSelByParty() { PartyID = criteria.PartyID });

            ViewBag.PartyID = criteria.PartyID;
            return PartialView();
        }

        [HttpPost]
        public ActionResult CreateNewParty(IMS_PartyIns criteria)
        {
            TempData["Result"] = _partyService.CreateUpdateParty(criteria, Request);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult UpdateParty(IMS_PartyInfoUpd criteria)
        {
            TempData["Result"] = _partyService.CreateUpdateParty(criteria, Request);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult CreatePartyLicensee(IMS_LicenseePartyMultipleIns criteria)
        {
            TempData["Result"] = _partyService.ExecuteCommand(criteria);
            return RedirectToAction("Index");
        }
    }
}