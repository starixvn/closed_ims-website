// --------------------------------------------------------------------------
// <copyright file="DeleteSingleBetController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.1.25">Created</change>
// </history>


using System.Diagnostics.CodeAnalysis;
using System.Web.Mvc;
using Fanex.Ares.Core.Web;
using Fanex.Ares.Core.Web.Controllers;
using Fanex.Ares.Report.Criteria;
using Fanex.Ares.Report.Services.Implement;
using Fanex.Ares.Report.Services.Interface;
using Fanex.NAP.Core.Web.Attribute;
using Fanex.NAP.Core.Business;

namespace Fanex.Ares.Report.Controllers
{
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class VoidOutrightTicketController : BaseController
    {

        IVoidOutrightTicketService _voidOutrightTicketService;
        IVoidSingleTicketService _voidSingleTicketService;

        public VoidOutrightTicketController()
        {
            _voidOutrightTicketService = new VoidOutrightTicketService();
            _voidSingleTicketService = new VoidSingleTicketService();     
        }

        [NapAuth(FunctionKey = NAPSpecialDeleteFunctionKeys.VoidSportsbookBet, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetOutrightLeague()
        {
            return Json(_voidOutrightTicketService.GetOutrightLeague());
        }


        [HttpPost]
        public JsonResult GetTeamByLeague(GetOutrightTeamCriteria criteria)
        {
            return Json(_voidOutrightTicketService.GetTeamByLeague(criteria));
        }

        [NapAuth(FunctionKey = NAPSpecialDeleteFunctionKeys.VoidSportsbookBet, Policy = Constant.Policy.Read)]
        [HttpPost]
        public JsonResult GetOutrightTicket(GetOutrightTicketCriteria criteria) 
        {
            criteria.UserName = CurrentSession.Username;
            return Json(_voidOutrightTicketService.GetOutrightTicket(criteria));
        }

        [NapAuth(FunctionKey = NAPSpecialDeleteFunctionKeys.VoidSportsbookBet, Policy = Constant.Policy.Host)]
        [HttpPost]
        public JsonResult VoidOutrightTicket(VoidSingleTicketCritetia criteria)
        {
            criteria.UserPwd = Fanex.Common.Cryptography.CfsEnCode(criteria.UserPwd);
            criteria.UserId = CurrentSession.UserId;
            criteria.Betteam1378 = "";
            criteria.Betteam515 = "";
            criteria.HDP = 9999;

            return Json(_voidSingleTicketService.VoidSingleTicket(criteria));
        }
    }
}