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
using System.Web.Hosting;
using System.Web.Mvc;
using Fanex.Ares.Core.Utils;
using Fanex.Ares.Core.Web;
using Fanex.Ares.Core.Web.Controllers;
using Fanex.Ares.Report.Criteria;
using Fanex.Ares.Report.Services.Implement;
using Fanex.Ares.Report.Services.Interface;
using Fanex.Core.PdfHelper;
using Fanex.NAP.Core.Web.Attribute;
using Fanex.NAP.Core.Business;
using Fanex.Ares.Report.Criteria.LicenseeSettlement;
using System.Collections.Generic;
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
        private const string SessionKeyLicenseeSettlementFromDate = "Fanex.Ares.Report.Services.FromDate";
        private const string SessionKeyLicenseeSettlementToDate = "Fanex.Ares.Report.Services.ToDate";
        private const string SessionKeyLicenseeSettlementViewName = "Fanex.Ares.Report.Services.LicenseeSettlementViewName";

        private static readonly string PdfCssFilePath = HostingEnvironment.MapPath(@"~\Areas\Report\Views\LicenseeSettlement\Styles\LicenseeSettlementPDF.css");

        #region View path

        protected string SettlementView = string.Empty;

        #endregion

        public LicenseeSettlementController()
        {
            _service = new LicenseeSettlementService();
            this.htmlViewRenderer = new HtmlViewRenderer();
            this.standardPdfRenderer = new StandardPdfRenderer();
        }

        public ActionResult Index()
        {
            IMS_Rpt_LicenseeSettlementCriteria model = new IMS_Rpt_LicenseeSettlementCriteria();
            model.Year = DateTime.Now.Year;
            ViewBag.ListLicensee = _service.GetAllLicensees();
            var licenseeID = int.Parse(((List<SelectListItem>)ViewBag.ListLicensee)[0].Value);
            ViewBag.ListProduct = _service.GetProductsByLicensee(new IMS_LicenseeProductSelCriteria() { LicenseeId = licenseeID });
            var listCurrency = _service.GetCurrencyByLicensee(new IMS_LicenseeCurrencySelCriteria() { LicenseeId = licenseeID }).ToList<SelectListItem>();
            listCurrency.Add(new SelectListItem() { Selected = true, Text = "All", Value = "0" });
            ViewBag.ListCurrency = listCurrency;
            ViewBag.ListExchangeRate = _service.GetBaseCurrency(new IMS_BaseCurrencySel());

            return View(model);
        }

        public PartialViewResult GetInvoice(IMS_Rpt_LicenseeSettlementCriteria criteria)
        {
            var model = _service.GenerateReport(criteria);
            SettlementView = "_" + criteria.ProductName;
            SessionExtensions.AddSession(System.Web.HttpContext.Current.Session, SessionKeyLicenseeSettlementFromDate, criteria.FromDate);
            SessionExtensions.AddSession(System.Web.HttpContext.Current.Session, SessionKeyLicenseeSettlementToDate, criteria.ToDate);
            SessionExtensions.AddSession(System.Web.HttpContext.Current.Session, SessionKeyLicenseeSettlementViewName, SettlementView);
            return PartialView(SettlementView, model);
        }

        protected ActionResult ViewPdf(string pageTitle, string viewName, object model, int numericRows, string fileName, bool isSavePDF)
        {
            // Render the view html to a string.
            string htmlText = this.htmlViewRenderer.RenderViewToString(this, viewName, model);

            // Let the html be rendered into a PDF document through iTextSharp.

            byte[] buffer = standardPdfRenderer.Render(htmlText, pageTitle, numericRows, PdfCssFilePath);

            // Return the PDF as a binary stream to the client.
            //return new BinaryContentResult(buffer, "application/pdf");
            if (isSavePDF)
            {
                Response.Clear();
                MemoryStream ms = new MemoryStream(buffer);
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename= " + fileName + ".pdf");
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
            SettlementView = (string)SessionExtensions.GetSession(System.Web.HttpContext.Current.Session, SessionKeyLicenseeSettlementViewName);
            model = (LicenseeSettlementModel)SessionExtensions.GetSession(System.Web.HttpContext.Current.Session, SessionKeyLicenseeSettlementReport);
            DateTime fromdate = (DateTime)SessionExtensions.GetSession(System.Web.HttpContext.Current.Session, SessionKeyLicenseeSettlementFromDate);
            DateTime todate = (DateTime)SessionExtensions.GetSession(System.Web.HttpContext.Current.Session, SessionKeyLicenseeSettlementToDate);
            string fileName = model.LicenseeInfor.LicenseeName + "_" + model.LicenseeInfor.ProductName + "_" + "Settlement_From_" + fromdate.ToString("dd-MMM-yy") + "_To_" + todate.ToString("dd-MMM-yy");
            //FillImageUrl(customerList, "report.jpg");
            return this.ViewPdf("", SettlementView, model, model.Report.EntityList.Count + 1, fileName, isSavePDF);
        }

        public ActionResult ExportSettlementReportToExcel(string exchange, string product)
        {
            _service.ExportSportsBookReportToExcel(exchange, product);
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