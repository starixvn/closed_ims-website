// -----------------------------------------------------------------------
// <copyright file="PartyController.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

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
            return Json(_partyService.GetAllParties(new IMS_PartyInfoSelCriteria()));
        }

        [HttpPost]
        public JsonResult GetAllPartyTypes()
        {
            return Json(_partyService.GetAllPartyTypes(new IMS_PartyInfoSelCriteria()));
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
        public ActionResult CreateNewparty(IMS_PartyIns criteria)
        {
            var rs = new Hashtable();
            if (Request.Files.Count <= 0)
            {
                criteria.Logo = string.Empty;
                rs = _partyService.ExecuteCommand(criteria);
            }
            else
            {
                string directory = Server.MapPath("~\\App_Data\\Logo\\") + criteria.PartyName + "\\";
                string fileName = "Logo" + Path.GetExtension(Request.Files[0].FileName);
                criteria.Logo = fileName;
                rs = _partyService.ExecuteCommand(criteria);

                if (Convert.ToInt32(rs["ErrCode"]) == 0)
                {
                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }

                    Request.Files[0].SaveAs(Path.Combine(directory, fileName));
                }
            }

            TempData["Result"] = rs;
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult UpdateParty(IMS_PartyInfoUpd criteria)
        {
            var rs = new Hashtable();
            if (Request.Files.Count <= 0)
            {
                criteria.Logo = string.Empty;
                rs = _partyService.ExecuteCommand(criteria);
            }
            else
            {
                string directory = Server.MapPath("~\\App_Data\\Logo\\") + criteria.PartyName + "\\";
                string fileName = "Logo" + Path.GetExtension(Request.Files[0].FileName);
                string fullFileName = Path.Combine(directory, fileName);
                criteria.Logo = fileName;
                rs = _partyService.ExecuteCommand(criteria);

                if (Convert.ToInt32(rs["ErrCode"]) == 0)
                {
                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }

                    if (System.IO.File.Exists(fullFileName))
                    {
                        System.IO.File.Delete(fullFileName);
                    }

                    Request.Files[0].SaveAs(fullFileName);
                }
            }



            TempData["Result"] = rs;
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult CreatePartyLicensee(IMS_LicenseePartyMultipleIns criteria)
        {
            var rs = new Hashtable();

            var arrayLicensees = criteria.LicenseeID.Trim(',').Split(',');

            var xml = "<Settings>";
            foreach (var licensee in arrayLicensees)
            {
                xml += "<Setting>";
                xml += "<Party>";
                xml += criteria.PartyID.ToString();
                xml += "</Party>";
                xml += "<Licensee>";
                xml += licensee.ToString();
                xml += "</Licensee>";
                xml += "</Setting>";
            }

            xml += "</Settings>";

            criteria.XmlSettings = xml;
            rs = _partyService.ExecuteCommand(criteria);

            TempData["Result"] = rs;
            return RedirectToAction("Index");
        }
    }
}