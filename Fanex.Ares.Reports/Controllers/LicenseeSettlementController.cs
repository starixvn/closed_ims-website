// --------------------------------------------------------------------------
// <copyright file="LicenseeSettlementController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="">Created</change>
// </history>


using System.Diagnostics.CodeAnalysis;
using System.Web;
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
using System.Linq;
using System;
using Fanex.Ares.Report.ViewModels;
using System.IO;
using Fanex.Common.Extensions;

namespace Fanex.Ares.Report.Controllers
{
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class LicenseeSettlementController : BaseController
    {
        ILicenseeSettlementService _service;
        private readonly HtmlViewRenderer htmlViewRenderer;
        private readonly StandardPdfRenderer standardPdfRenderer;
        private const string SessionKeyLicenseeSettlementReport = "Fanex.Ares.Report.Services.LicenseeSettlementReport";
        private const string SessionKeyLicenseeFromDate = "Fanex.Ares.Report.Services.FromDate";
        private const string SessionKeyLicenseeToDate = "Fanex.Ares.Report.Services.ToDate";
        public LicenseeSettlementController()
        {
            _service = new LicenseeSettlementService();
            this.htmlViewRenderer = new HtmlViewRenderer();
            this.standardPdfRenderer = new StandardPdfRenderer();
        }

        //[NapAuth(FunctionKey = NAPSpecialDeleteFunctionKeys.VoidSportsbookBet, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            ViewBag.ListLicensee = _service.GetAllLicensees();
            var licenseeID = int.Parse(((List<SelectListItem>)ViewBag.ListLicensee)[0].Value);
            ViewBag.ListProduct = _service.GetProductsByLicensee(new IMS_LicenseeProductSelCriteria() { LicenseeId = licenseeID });
            var listCurrency = _service.GetCurrencyByLicensee(new IMS_LicenseeCurrencySelCriteria() { LicenseeId = licenseeID }).ToList<SelectListItem>();
            listCurrency.Add(new SelectListItem() { Selected = true, Text = "All", Value = "0" });
            ViewBag.ListCurrency = listCurrency;
            //var listExchangeRate = new List<SelectListItem>();
            //listExchangeRate.Add(new SelectListItem() { Selected = true, Text = "EUR", Value = "6" });
            //listExchangeRate.Add(new SelectListItem() { Selected = false, Text = "USD", Value = "3" });
            ViewBag.ListExchangeRate = _service.GetBaseCurrency(new IMS_BaseCurrencySel());

            return View();
        }

        public PartialViewResult GetInvoice(IMS_Rpt_LicenseeSettlementCriteria criteria)
        {
            var model = _service.GenerateReport(criteria);
            SessionExtensions.AddSession(System.Web.HttpContext.Current.Session, SessionKeyLicenseeFromDate, criteria.FromDate);
            SessionExtensions.AddSession(System.Web.HttpContext.Current.Session, SessionKeyLicenseeToDate, criteria.ToDate); 
            return PartialView("LicenseeSettlement", model);
        }

        protected ActionResult ViewPdf(string pageTitle, string viewName, object model, int numericRows, string fileName, bool isSavePDF)
        {
           
            // Render the view html to a string.
            string htmlText = this.htmlViewRenderer.RenderViewToString(this, viewName, model);

            // Let the html be rendered into a PDF document through iTextSharp.
            byte[] buffer = standardPdfRenderer.Render(htmlText, pageTitle, numericRows);

            // Return the PDF as a binary stream to the client.
            //return new BinaryContentResult(buffer, "application/pdf");
            if (isSavePDF)
            {
                Response.Clear();
                MemoryStream ms = new MemoryStream(buffer);
                Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename= "+fileName+ ".pdf");
                Response.Buffer = true;
                ms.WriteTo(Response.OutputStream);
                Response.End();

                // Return the PDF as a binary stream to the client.
                return new EmptyResult();
            }
            else
            {
                return new BinaryContentResult(buffer, "application/pdf");
            }
        }

        public ActionResult ExportPDF(bool isSavePDF)
        {
            //var model = GenerateReport(criteria);
            var model = new LicenseeSettlementModel();
            model = (LicenseeSettlementModel)SessionExtensions.GetSession(System.Web.HttpContext.Current.Session, SessionKeyLicenseeSettlementReport);
            DateTime fromdate = (DateTime)SessionExtensions.GetSession(System.Web.HttpContext.Current.Session, SessionKeyLicenseeFromDate);
            DateTime todate = (DateTime)SessionExtensions.GetSession(System.Web.HttpContext.Current.Session, SessionKeyLicenseeToDate);
            string fileName = model.LicenseeInfor.LicenseeName + "_" + model.LicenseeInfor.ProductName + "_" + "Settlement_From_" + fromdate.ToString("dd-MMM-yy") + "_To_" + todate.ToString("dd-MMM-yy");
            //FillImageUrl(customerList, "report.jpg");
            return this.ViewPdf("", "LicenseeSettlementPDF", model, model.Report.EntityList.Count + 1, fileName, isSavePDF);
        }
        
        public ActionResult ExportSettlementReportToExcel(string exchange, string product)
        {
            _service.ExportSettlementReportToExcel(exchange, product);
            return new EmptyResult();
        }

        public JsonResult GetProductByLicenseeID(int licenseeID)
        {
            return Json(_service.GetProductsByLicensee(new IMS_LicenseeProductSelCriteria() { LicenseeId = licenseeID }));
        }

        public JsonResult GetCurencyByLicenseeID(int licenseeID)
        {
            return Json(_service.GetCurrencyByLicensee(new IMS_LicenseeCurrencySelCriteria() { LicenseeId = licenseeID }));
        }

        public ActionResult PrintScreen()
        {
            var model = (LicenseeSettlementModel)SessionExtensions.GetSession(System.Web.HttpContext.Current.Session, SessionKeyLicenseeSettlementReport);
            return View("PrintReport", model);
        }
    }
}