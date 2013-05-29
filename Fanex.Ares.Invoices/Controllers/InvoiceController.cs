// --------------------------------------------------------------------------
// <copyright file="LicenseeSettlementController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="">Created</change>
// </history>

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using System.Web.Mvc;
using Fanex.Ares.Core.Utils;
using Fanex.Ares.Core.Web.Controllers;
using Fanex.Ares.Core.Web.Security;
using Fanex.Common.Extensions;
using Fanex.Core.PdfHelper;
using Fanex.Ares.Core.Criteria;
using Fanex.Ares.Report.Criteria;
using Fanex.Ares.Report.Criteria.LicenseeSettlement;
using Fanex.Ares.Report.Services.Interface;
using Fanex.Ares.Report.ViewModels.LicenseeSettlement;
using Fanex.Ares.Core.Services.Impl;
using Fanex.Ares.Core.Extensions;
using Fanex.Workbooks;
using Fanex.NAP.Core.Web.Attribute;

using Fanex.Ares.Invoice.Models.Invoice;
namespace Fanex.Ares.Invoice.Controllers
{
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    [NapSuperAuth]
    public class InvoiceController : BaseController
    {
        ServiceBase _service;
        private readonly HtmlViewRenderer htmlViewRenderer;
        private readonly StandardPdfRenderer standardPdfRenderer;

        private static readonly string PdfCssFilePath = HostingEnvironment.MapPath(@"~\Areas\Invoice\Views\Invoice\Styles\InvoicePDF.css");

        #region View path

        protected string SettlementView = string.Empty;

        #endregion

        public InvoiceController()
        {
            _service = new ServiceBase();
            this.htmlViewRenderer = new HtmlViewRenderer();
            this.standardPdfRenderer = new StandardPdfRenderer();
        }

        [AuthenticateAction(AuthenticateAction.AuthenticationType.Read, "Index")]
        public ActionResult Index()
        {
            ViewBag.ListLicenseeEx = _service.GetAllLicenseesEx();
            ViewBag.ListProduct = _service.GetProductsByLicensee(new IMS_LicenseeProductSelCriteria() { LicenseeId = 0 });
            ViewBag.ListCurrency = _service.GetCurrencyByLicensee(new IMS_LicenseeCurrencySelCriteria() { LicenseeId = 0 }).ToList<SelectListItem>();
            ViewBag.ListExchangeRate = _service.GetBaseCurrency(new IMS_BaseCurrencySel());
            return View();
        }


        public PartialViewResult GetReport(IMS_Rpt_LicenseeSettlementCriteria criteria)
        {
            var model = new LicenseeSettlementModel();// _service.GenerateReport(criteria);
            model.LicenseeInfor.FromDate = criteria.FromDate;
            model.LicenseeInfor.ToDate = criteria.ToDate;
            SettlementView = "_" + criteria.ProductName;
           
            return PartialView(SettlementView, model);
        }

        protected ActionResult ViewPdf(string pageTitle, string viewName, object model, int numericRows, string fileName, bool isSavePDF, bool isLandscape = false)
        {
            // Render the view html to a string.
            string htmlText = this.htmlViewRenderer.RenderViewToString(this, viewName, model);

            // Let the html be rendered into a PDF document through iTextSharp.

            byte[] buffer = standardPdfRenderer.Render(htmlText, pageTitle, numericRows, PdfCssFilePath, isLandscape);

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

        public ActionResult ExportPDF(bool isSavePDF, bool isLandscape = false)
        {
            //var model = GenerateReport(criteria);
            var model = new LicenseeSettlementModel();
            //SettlementView = (string)SessionExtensions.GetSession(System.Web.HttpContext.Current.Session, CacheServiceKey.SessionKeyLicenseeSettlementViewName);
            SettlementView = "_USD";
            //model = (LicenseeSettlementModel)SessionExtensions.GetSession(System.Web.HttpContext.Current.Session, CacheServiceKey.SessionKeyLicenseeSettlementReport);
            int month = (int)SessionExtensions.GetSession(System.Web.HttpContext.Current.Session, CacheServiceKey.SessionKeyLicenseeSettlementMonth);
            int year = (int)SessionExtensions.GetSession(System.Web.HttpContext.Current.Session, CacheServiceKey.SessionKeyLicenseeSettlementYear);
            //string fileName = model.LicenseeInfor.LicenseeName + "_" + model.LicenseeInfor.ProductName + "_" + "Settlement_For_" + month + "-" + year;
            string fileName = "test";
            //FillImageUrl(customerList, "report.jpg");
            return this.ViewPdf("", "_Sportsbook", model, model.Report.EntityList.Count + 1, fileName, isSavePDF, isLandscape);
        }

        //public ActionResult ExportDocument(WorkbookManager.DocumentType documentType)
        //{
        //    var model = new LicenseeSettlementModel();
        //    model = (LicenseeSettlementModel)SessionExtensions.GetSession(System.Web.HttpContext.Current.Session, CacheServiceKey.SessionKeyLicenseeSettlementReport);
        //    SettlementView = (string)SessionExtensions.GetSession(System.Web.HttpContext.Current.Session, CacheServiceKey.SessionKeyLicenseeSettlementViewName);

        //    List<string> htmlList = new List<string>();
        //    string[] arrayView = new string[] { "_Number Game", "_Sportsbook", "_BA", "_Number Game", "_Sportsbook", "_BA", "_Number Game", "_Sportsbook", "_BA", "_Number Game", "_Sportsbook", "_BA", "_Number Game", "_Sportsbook", "_BA", "_Number Game", "_Sportsbook", "_BA" };
        //    List<WorkbookManager.PageOrientation> PageOrientationList = new List<WorkbookManager.PageOrientation>();

        //    PageOrientationList.Add(WorkbookManager.PageOrientation.Portrait);
        //    PageOrientationList.Add(WorkbookManager.PageOrientation.Landscape);
        //    PageOrientationList.Add(WorkbookManager.PageOrientation.Portrait);
        //    PageOrientationList.Add(WorkbookManager.PageOrientation.Portrait);
        //    PageOrientationList.Add(WorkbookManager.PageOrientation.Landscape);
        //    PageOrientationList.Add(WorkbookManager.PageOrientation.Portrait);
        //    PageOrientationList.Add(WorkbookManager.PageOrientation.Portrait);
        //    PageOrientationList.Add(WorkbookManager.PageOrientation.Landscape);
        //    PageOrientationList.Add(WorkbookManager.PageOrientation.Portrait);
        //    PageOrientationList.Add(WorkbookManager.PageOrientation.Portrait);
        //    PageOrientationList.Add(WorkbookManager.PageOrientation.Landscape);
        //    PageOrientationList.Add(WorkbookManager.PageOrientation.Portrait);
        //    PageOrientationList.Add(WorkbookManager.PageOrientation.Portrait);
        //    PageOrientationList.Add(WorkbookManager.PageOrientation.Landscape);
        //    PageOrientationList.Add(WorkbookManager.PageOrientation.Portrait);
        //    PageOrientationList.Add(WorkbookManager.PageOrientation.Portrait);
        //    PageOrientationList.Add(WorkbookManager.PageOrientation.Landscape);
        //    PageOrientationList.Add(WorkbookManager.PageOrientation.Portrait);
        //    foreach (string view in arrayView)
        //    {
        //        string html = HtmlHelperExtensions.RenderViewToString(this, view, model);
        //        htmlList.Add(html);
        //    }

        //    var excelCssFilePath = new List<string>();
        //    excelCssFilePath.Add(HostingEnvironment.MapPath(@"~\Areas\Report\Views\LicenseeSettlement\Styles\LicenseeSettlement.css"));

        //    var workbook = new WorkbookManager();
        //    var ms = workbook.CreateDocument(htmlList, excelCssFilePath, documentType, PageOrientationList);
        //    if (ms != null)
        //    {
        //        // return the filestream
        //        // Rewind the memory stream to the beginning
        //        ms.Seek(0, SeekOrigin.Begin);

        //        string fileName = "invoice" + "." + documentType.ToString().ToLower();

        //        return File(ms, @"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml", fileName);
        //    }

        //    return new EmptyResult();
        //}
    }
}