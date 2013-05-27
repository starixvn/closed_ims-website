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

namespace Fanex.Ares.Admin.Controllers
{

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    [NapSuperAuth]
    public class PartyController : Controller
    {
        [HttpGet]
        [AuthenticateAction(AuthenticateAction.AuthenticationType.Read, "Index")]
        public ActionResult Index()
        {
            PartyModel model = new PartyModel();
            return View(model);
        }
    }
}
