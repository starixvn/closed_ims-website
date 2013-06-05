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
        public JsonResult GetAllLicensees()
        {
            return Json(_licenseeService.GetAllLicensees(new IMS_LicenseeInfoSel()));
        }

        public PartialViewResult _AddNew()
        {
            return PartialView();
        }

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
            var rs = new Hashtable();
            rs = _licenseeService.ExecuteCommand(criteria);
            TempData["Result"] = rs;
            return RedirectToAction("Index");
        }

        [HttpPost]
        [AuthenticateAction(AuthenticateAction.AuthenticationType.Host, "Index")]
        public ActionResult UpdateLicensee(IMS_LicenseeInfoUpd criteria)
        {
            var rs = new Hashtable();
            rs = _licenseeService.ExecuteCommand(criteria);

            TempData["Result"] = rs;
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult CreatePartyLicensee(IMS_LicenseePartyMultipleIns criteria)
        {
            var rs = new Hashtable();
            if (!string.IsNullOrEmpty(criteria.ListPartyID))
            {
                var arrayParties = criteria.ListPartyID.Trim(',').Split(',');

                var xml = "<Settings>";
                foreach (var party in arrayParties)
                {
                    xml += "<Setting>";
                    xml += "<Party>";
                    xml += party.ToString();
                    xml += "</Party>";
                    xml += "<Licensee>";
                    xml += criteria.LicenseeID.ToString();
                    xml += "</Licensee>";
                    xml += "</Setting>";
                }

                xml += "</Settings>";

                criteria.XmlSettings = xml;
                rs = _licenseeService.ExecuteCommand(criteria);
            }

            TempData["Result"] = rs;
            return RedirectToAction("Index");
        }
    }
}