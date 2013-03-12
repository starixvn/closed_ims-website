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
    using Fanex.TicketControl.SportBook.Criteria;
using Fanex.TicketControl.SportBook.Services.Interface;
    using Fanex.TicketControl.SportBook.Services.Implement;
    using Fanex.TicketControl.Core.Utils;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class RecoverTicketController : BaseController
    {
        IRecoverTicketService _service;

        public RecoverTicketController()
        {
            _service = new RecoverTicketService();
        }
        
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public JsonResult GetTicketRecover(GetRecoverTicketCriteria criteria)
        {
            Core.Models.ActionResult rs = new Core.Models.ActionResult();

            var data = _service.GetTicketRecover(criteria, ref rs);
            if (rs.ErrorCode == 0)
            {
                return Json(data);
            }

            return Json(rs);
        }

        [HttpPost]
        public JsonResult RecoverTicket(RecoverTicketCriteria criteria)
        {
            criteria.UserId = CurrentSession.UserId;
            criteria.Ip = Helper.GetClientIP();
            criteria.ErrCode = 0;
            criteria.ErrMsg = string.Empty;

            criteria.Msg.CreatorId = criteria.UserId;
            criteria.Msg.CreatorName = CurrentSession.Username;

            var rs = _service.RecoverTicket(criteria);
            return Json(rs);
        }
    }
}