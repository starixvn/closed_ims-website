// --------------------------------------------------------------------------
// <copyright file="LicenseeSettlementController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="">Created</change>
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
using Fanex.Ares.Report.Criteria.LicenseeSettlement;
using System.Collections.Generic;
using PdfHelper;
using Fanex.Ares.Report.ViewModels.LicenseeSettlement;

namespace Fanex.Ares.Report.Controllers
{
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class LicenseeSettlementController : BaseController
    {
        ILicenseeSettlementService _service;
         private readonly HtmlViewRenderer htmlViewRenderer;
        private readonly StandardPdfRenderer standardPdfRenderer;
        public LicenseeSettlementController()
        {
            _service = new LicenseeSettlementService();
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
            ViewBag.ListLicensee = _service.GetAllLicensees();
            var licenseeID = int.Parse(((List<SelectListItem>)ViewBag.ListLicensee)[0].Value);
            ViewBag.ListProduct = _service.GetProductsByLicensee(new IMS_LicenseeProductSelCriteria() { LicenseeId = licenseeID });
            var listCurrency = _service.GetCurrencyByLicensee(new IMS_LicenseeCurrencySelCriteria() { LicenseeId = licenseeID });
            listCurrency.Add(new SelectListItem() { Selected = true, Text = "All", Value = "0" });
            ViewBag.ListCurrency = listCurrency;
            var listExchangeRate = new List<SelectListItem>();
            listExchangeRate.Add(new SelectListItem() { Selected = true, Text = "EUR", Value = "6" });
            listExchangeRate.Add(new SelectListItem() { Selected = false, Text = "USD", Value = "3" });
            ViewBag.ListExchangeRate = listExchangeRate;

            return View();
        }

        public ActionResult PrintInvoice()
        {
            var model = new LicenseeSettlementModel();
            //FillImageUrl(customerList, "report.jpg");
            return this.ViewPdf("", "LicenseeSettlement", model, model.Report.EntityList.Count + 1);
        }

        public PartialViewResult GetInvoice(IMS_Rpt_LicenseeSettlementCriteria criteria)
        {
            
            var model = new LicenseeSettlementModel();

            return PartialView("LicenseeSettlement", model);
        }

        public ActionResult ExportSettlementReportToExcel(string exchange, string product)
        {
            _service.ExportSettlementReportToExcel(exchange, product);
            return new EmptyResult();
        }
    }
}