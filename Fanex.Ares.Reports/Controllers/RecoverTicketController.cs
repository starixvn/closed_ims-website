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
using Fanex.Ares.Core.Utils;
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
    public class RecoverTicketController : BaseController
    {
        IRecoverTicketService _service;

        public RecoverTicketController()
        {
            _service = new RecoverTicketService();
        }

        [NapAuth(FunctionKey = NAPSpecialDeleteFunctionKeys.VoidSportsbookBet, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            return View();
        }

        [NapAuth(FunctionKey = NAPSpecialDeleteFunctionKeys.VoidSportsbookBet, Policy = Constant.Policy.Read)]
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

        [NapAuth(FunctionKey = NAPSpecialDeleteFunctionKeys.VoidSportsbookBet, Policy = Constant.Policy.Host)]
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