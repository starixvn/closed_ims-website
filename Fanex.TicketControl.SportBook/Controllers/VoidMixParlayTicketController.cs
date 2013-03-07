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

    

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class VoidMixParlayTicketController : BaseController
    {

        IVoidSingleTicketService _deleteSingleTicketService;

        public VoidMixParlayTicketController()
        {
            _deleteSingleTicketService = new VoidSingleTicketService();    
            
        }

        
        public ActionResult Index()
        {
            return View();
        }

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

        [HttpPost]
        public JsonResult VoidSingleTicket(VoidSingleTicketCritetia criteria)
        {
            criteria.UserPwd = Fanex.Common.Cryptography.CfsEnCode(criteria.UserPwd);
            criteria.UserId = CurrentSession.UserId;

            return Json(_deleteSingleTicketService.VoidSingleTicket(criteria));
        }
    }
}