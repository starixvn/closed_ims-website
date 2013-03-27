// --------------------------------------------------------------------------
// <copyright file="DeleteSingleBetController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.1.25">Created</change>
// </history>


namespace Fanex.TicketControl.SportBook.Controllers
{
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Mvc;
    using Fanex.TicketControl.Core.Web.Controllers;
    using Fanex.TicketControl.SportBook.Services.Implement;
    using Fanex.TicketControl.SportBook.Services.Interface;
    using Fanex.TicketControl.Core.Criteria;
    using Fanex.TicketControl.SportBook.Criteria;
    using Fanex.NAP.Core.Web.Attribute;
    using Fanex.NAP.Core.Business;

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