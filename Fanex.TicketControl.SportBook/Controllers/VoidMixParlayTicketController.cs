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

        IVoidParlayTicketService _voidParlayTicketService;

        public VoidMixParlayTicketController()
        {
            _voidParlayTicketService = new VoidParlayTicketService();    
            
        }

        
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetParlayTicket(GetParlayTicketCriteria criteria)
        {
            return Json(_voidParlayTicketService.GetParlayTicket(criteria));
        }

        [HttpPost]
        public JsonResult GetParlaySubTicket(GetParlaySubTicketCriteria criteria)
        {
            return Json(_voidParlayTicketService.GetParlaySubTicket(criteria));
        }

      
        [HttpPost]
        public JsonResult VoidParlayTicket(VoidParlayTicketCritetia criteria)
        {
            criteria.UserPwd = Fanex.Common.Cryptography.CfsEnCode(criteria.UserPwd);
            criteria.UserId = CurrentSession.UserId;

            return Json(_voidParlayTicketService.VoidParlayTicket(criteria));
        }
    }
}