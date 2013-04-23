// --------------------------------------------------------------------------
// <copyright file="LicenseeSettlementController.cs" company="Nexcel Solutions Vietnam">
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
using PdfHelper;
using Fanex.Ares.Report.ViewModels.SportBook;

namespace Fanex.Ares.Report.Controllers
{
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class LicenseeSettlementController : BaseController
    {
        IRecoverTicketService _service;
         private readonly HtmlViewRenderer htmlViewRenderer;
        private readonly StandardPdfRenderer standardPdfRenderer;
        public LicenseeSettlementController()
        {
            _service = new RecoverTicketService();
            this.htmlViewRenderer = new HtmlViewRenderer();
            this.standardPdfRenderer = new StandardPdfRenderer();
        }

        protected ActionResult ViewPdf(string pageTitle, string viewName, object model, int numericRows)
        {
            // Render the view html to a string.
            string htmlText = this.htmlViewRenderer.RenderViewToString(this, viewName, model);

            // Let the html be rendered into a PDF document through iTextSharp.
            byte[] buffer = standardPdfRenderer.Render(htmlText, pageTitle, numericRows);

            // Return the PDF as a binary stream to the client.
            return new BinaryContentResult(buffer, "application/pdf");
        }

        //[NapAuth(FunctionKey = NAPSpecialDeleteFunctionKeys.VoidSportsbookBet, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            return View();
        }

        //[NapAuth(FunctionKey = NAPSpecialDeleteFunctionKeys.VoidSportsbookBet, Policy = Constant.Policy.Read)]
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

        public ActionResult PrintInvoice(int numericRows)
        {
            var model = new LicenseeSettlementModel();
            //FillImageUrl(customerList, "report.jpg");
            return this.ViewPdf("", "InvoiceDetail", model, numericRows);
        }


        //[NapAuth(FunctionKey = NAPSpecialDeleteFunctionKeys.VoidSportsbookBet, Policy = Constant.Policy.Host)]
        //[HttpPost]
        //public JsonResult RecoverTicket(RecoverTicketCriteria criteria)
        //{
        //    criteria.UserId = CurrentSession.UserId;
        //    criteria.Ip = Helper.GetClientIP();
        //    criteria.ErrCode = 0;
        //    criteria.ErrMsg = string.Empty;

        //    criteria.Msg.CreatorId = criteria.UserId;
        //    criteria.Msg.CreatorName = CurrentSession.Username;

        //    var rs = _service.RecoverTicket(criteria);
        //    return Json(rs);
        //}
    }
}