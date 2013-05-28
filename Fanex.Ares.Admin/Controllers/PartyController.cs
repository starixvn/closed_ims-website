// -----------------------------------------------------------------------
// <copyright file="PartyController.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

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
using System.Collections.Generic;
using System.Linq;

namespace Fanex.Ares.Admin.Controllers
{

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    [NapSuperAuth]
    public class PartyController : Controller
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
            List<PartyModel> model = _partyService.GetAllParties(new IMS_PartyInfoSelCriteria()).ToList();
            return View(model);
        }

        [HttpPost]
        public JsonResult GetAllParties()
        {
            return Json(_partyService.GetAllParties(new IMS_PartyInfoSelCriteria()));
        }
    }
}
