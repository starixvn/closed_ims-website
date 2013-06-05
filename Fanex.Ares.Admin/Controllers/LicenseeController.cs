// --------------------------------------------------------------------------
// <copyright file="LicenseeController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Phillip.Truong" date="2013.06.03">Created</change>
// </history>

using System.Web;
using System.Web.Mvc;
using System.Diagnostics.CodeAnalysis;
using Fanex.Ares.Admin.Criteria;
using Fanex.Ares.Admin.Services.Implement;
using Fanex.Ares.Admin.Services.Interface;
using Fanex.Ares.Core.Web;
using Fanex.NAP.Core.Web.Attribute;
using Fanex.NAP.Core.Business;
using Fanex.Ares.Core.Web.Security;
using Fanex.Ares.Admin.Models.Party;
using Fanex.Ares.Admin.Services.Party.Implement;
using Fanex.Ares.Admin.Services.Party.Interface;
using Fanex.Ares.Admin.Common;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using Fanex.Ares.Core.Web.Controllers;
using System.IO;
using System;

namespace Fanex.Ares.Admin.Controllers
{

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    [NapSuperAuth]
    public class LicenseeController : BaseController
    {
        private ILicenseeService _licenseeService;

        public LicenseeController()
        {
            _licenseeService = new LicenseeService();
        }

        [HttpGet]
        [AuthenticateAction(AuthenticateAction.AuthenticationType.Read, "Index")]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [AuthenticateAction(AuthenticateAction.AuthenticationType.Read, "Index")]
        public JsonResult GetAllLicensees()
        {
            return Json(_licenseeService.GetAllLicensees(new IMS_LicenseeInfoSel()));
        }

        [AuthenticateAction(AuthenticateAction.AuthenticationType.Read, "Index")]
        public PartialViewResult _AddNew()
        {
            return PartialView();
        }

        [AuthenticateAction(AuthenticateAction.AuthenticationType.Read, "Index")]
        public PartialViewResult _PartyLicensee(IMS_PartySelByLicensee criteria)
        {
            ViewBag.lstAvailableParties = _licenseeService.GetAvailableOrSelectedParties(new IMS_AvailablePartySelByLicensee() { LinceseeID = criteria.LicenseeID });
            ViewBag.lstSelectedParties = _licenseeService.GetAvailableOrSelectedParties(criteria);

            ViewBag.LicenseeID = criteria.LicenseeID;
            return PartialView();
        }

        [HttpPost]
        [AuthenticateAction(AuthenticateAction.AuthenticationType.Host, "Index")]
        public ActionResult CreateNewLicensee(IMS_LicenseeIns criteria)
        {
            TempData["Result"] = _licenseeService.ExecuteCommand(criteria);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [AuthenticateAction(AuthenticateAction.AuthenticationType.Host, "Index")]
        public ActionResult UpdateLicensee(IMS_LicenseeInfoUpd criteria)
        {
            TempData["Result"] = _licenseeService.ExecuteCommand(criteria);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [AuthenticateAction(AuthenticateAction.AuthenticationType.Host, "Index")]
        public ActionResult CreatePartyLicensee(IMS_LicenseePartyMultipleIns criteria)
        {
            TempData["Result"] = _licenseeService.ExecuteCommand(criteria);
            return RedirectToAction("Index");
        }
    }
}