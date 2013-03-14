
namespace Fanex.TicketControl.Gaming.Controllers
{
    using System.Web.Mvc;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.TicketControl.Core.Web.Controllers;
    using Fanex.TicketControl.Gaming.Services.Interface;
    using Fanex.TicketControl.Gaming.Services.Impl;
    using Fanex.TicketControl.Gaming.Criteria;
    using Fanex.TicketControl.Core.Utils;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class RecoverNumberGameTicketController : BaseController
    {
        IRecoverNumberGameTicketService _service;

        public RecoverNumberGameTicketController()
        {
            _service = new RecoverNumberGameTicketService();
        }
        
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetNumberGameTicketRecover(GetRecoverNumberGameTicketCriteria criteria)
        {
            Core.Models.ActionResult rs = new Core.Models.ActionResult();

            var data = _service.GetNumberGameTicketRecover(criteria, ref rs);
            if (rs.ErrorCode == 0)
            {
                return Json(data);
            }

            return Json(rs);
        }

        [HttpPost]
        public JsonResult RecoverNumberGameTicket(RecoverNumberGameTicketCriteria criteria)
        {
            criteria.UserId = CurrentSession.UserId;
            criteria.ErrCode = 0;
            criteria.ErrMsg = string.Empty;

            criteria.Msg.CreatorId = criteria.UserId;
            criteria.Msg.CreatorName = CurrentSession.Username;

            var rs = _service.RecoverNumberGameTicket(criteria);
            return Json(rs);
        }
    }
}
