using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fanex.TicketControl.Core.Web.Controllers;
using System.Web.Mvc;
using Fanex.TicketControl.Core.Models;
using Fanex.TicketControl.Misc.Services.Interface;
using Fanex.TicketControl.Misc.Services.Impl;
using Fanex.NAP.Core.Web.Attribute;
using Fanex.NAP.Core.Business;

namespace Fanex.TicketControl.Misc.Controllers
{
    public class BatchMessageController : BaseController
    {
        IBatchMessageService _service;
        public BatchMessageController()
        {
            _service = new BatchMessageService();
        }

        public System.Web.Mvc.ActionResult Index()
        {
            return View();
        }

        [NapAuth(FunctionKey = NAPSpecialDeleteFunctionKeys.UpdateBatchMsg, Policy = Constant.Policy.Host)]
        [HttpPost]
        public JsonResult UploadMsg(Message pmsg, string TransIds)
        {
            pmsg.CreatorId = CurrentSession.UserId;
            pmsg.CreatorName = CurrentSession.Username;

            return Json(_service.UploadMsg(pmsg, TransIds));
        }
    }
}
