namespace Fanex.TicketControl.Gaming.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using Fanex.TicketControl.Core.Web.Controllers;
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class VoidNumberGameTicketController : BaseController
    {

        //IVoidSingleTicketService _deleteSingleTicketService;

        public VoidNumberGameTicketController()
        {
            //_deleteSingleTicketService = new VoidSingleTicketService();
        }


        public ActionResult Index()
        {
            return View();
        }

        //[HttpPost]
        //public JsonResult GetSingleTicket(GetSingleTicketCriteria criteria)
        //{
        //    criteria.UserName = CurrentSession.Username;
        //    return Json(_deleteSingleTicketService.GetSingleTicket(criteria));
        //}

        //[HttpPost]
        //public JsonResult CheckMatchIdWithRefno(CheckMatchIdWithRefNoCriteria criteria)
        //{
        //    criteria.Type = Core.Utils.Helper.SportType.Sportsbook;
        //    return Json(_deleteSingleTicketService.CheckMatchIDwithRefno(criteria));
        //}

        //[HttpPost]
        //public JsonResult VoidSingleTicket(VoidSingleTicketCritetia criteria)
        //{
        //    criteria.UserPwd = Fanex.Common.Cryptography.CfsEnCode(criteria.UserPwd);
        //    criteria.UserId = CurrentSession.UserId;

        //    return Json(_deleteSingleTicketService.VoidSingleTicket(criteria));
        //}
    }
}
