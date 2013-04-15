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
using Fanex.Ares.Core.Criteria;
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
    public class DeleteSingleTicketController : BaseController
    {

        IVoidSingleTicketService _deleteSingleTicketService;

        public DeleteSingleTicketController()
        {
            _deleteSingleTicketService = new VoidSingleTicketService();    
            
        }

        [NapAuth(FunctionKey = NAPSpecialDeleteFunctionKeys.VoidSportsbookBet, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            return View();
        }

        [NapAuth(FunctionKey = NAPSpecialDeleteFunctionKeys.VoidSportsbookBet, Policy = Constant.Policy.Read)]
        [HttpPost]
        public JsonResult GetSingleTicket(GetSingleTicketCriteria criteria)
        {
            criteria.UserName = CurrentSession.Username;
            return Json(_deleteSingleTicketService.GetSingleTicket(criteria));
        }

        [HttpPost]
        public JsonResult CheckMatchIdWithRefno(CheckMatchIdWithRefNoCriteria criteria)
        {
            criteria.Type = Core.Utils.Helper.SportType.Sportsbook;
            return Json(_deleteSingleTicketService.CheckMatchIDwithRefno(criteria));
        }

        [NapAuth(FunctionKey = NAPSpecialDeleteFunctionKeys.VoidSportsbookBet, Policy = Constant.Policy.Host)]
        [HttpPost]
        public JsonResult VoidSingleTicket(VoidSingleTicketCritetia criteria)
        {
            criteria.UserPwd = Fanex.Common.Cryptography.CfsEnCode(criteria.UserPwd);
            criteria.UserId = CurrentSession.UserId;  
            return Json(_deleteSingleTicketService.VoidSingleTicket(criteria));
        }
    }
}