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
using System.Web;
using Fanex.Ares.Core.Utils;
using Fanex.Ares.Core.Web.Controllers;
using Fanex.Ares.Report.Criteria;
using Fanex.Ares.Report.Criteria.LicenseeSettlement;
using Fanex.Ares.Report.Services.Interface;
using Fanex.Ares.Report.ViewModels.LicenseeSettlement;
using Fanex.Common.Extensions;
using Fanex.Core.PdfHelper;
using Fanex.Ares.Core.Criteria;
using Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;

namespace Fanex.Ares.Report.Controllers
{
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class LicenseeSettlementController : BaseController
    {
        ILicenseeSettlementService _service;
        private readonly HtmlViewRenderer htmlViewRenderer;
        private readonly StandardPdfRenderer standardPdfRenderer;

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

        public PartialViewResult GetReport(IMS_Rpt_LicenseeSettlementCriteria criteria)
        {
            var model = _service.GenerateReport(criteria);
            model.LicenseeInfor.FromDate = criteria.FromDate;
            model.LicenseeInfor.ToDate = criteria.ToDate;
            SettlementView = "_" + criteria.ProductName;
            SessionExtensions.AddSession(System.Web.HttpContext.Current.Session, CacheServiceKey.SessionKeyLicenseeSettlementMonth, criteria.Month);
            SessionExtensions.AddSession(System.Web.HttpContext.Current.Session, CacheServiceKey.SessionKeyLicenseeSettlementYear, criteria.Year);
            SessionExtensions.AddSession(System.Web.HttpContext.Current.Session, CacheServiceKey.SessionKeyLicenseeSettlementViewName, SettlementView);
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

        public ActionResult ExportPDF(bool isSavePDF, bool isLandscape = true)
        {
            //var model = GenerateReport(criteria);
            var model = new LicenseeSettlementModel();
            SettlementView = (string)SessionExtensions.GetSession(System.Web.HttpContext.Current.Session, CacheServiceKey.SessionKeyLicenseeSettlementViewName);
            model = (LicenseeSettlementModel)SessionExtensions.GetSession(System.Web.HttpContext.Current.Session, CacheServiceKey.SessionKeyLicenseeSettlementReport);
            int month = (int)SessionExtensions.GetSession(System.Web.HttpContext.Current.Session, CacheServiceKey.SessionKeyLicenseeSettlementMonth);
            int year = (int)SessionExtensions.GetSession(System.Web.HttpContext.Current.Session, CacheServiceKey.SessionKeyLicenseeSettlementYear);
            string fileName = model.LicenseeInfor.LicenseeName + "_" + model.LicenseeInfor.ProductName + "_" + "Settlement_For_" + month + "-" + year;
            //FillImageUrl(customerList, "report.jpg");
            return this.ViewPdf("", SettlementView, model, model.Report.EntityList.Count + 1, fileName, isSavePDF, isLandscape);
        }

        public ActionResult ExportPDFUsingInterop(string exchange, string product)
        {
            var model = new LicenseeSettlementModel();
            model = (LicenseeSettlementModel)SessionExtensions.GetSession(System.Web.HttpContext.Current.Session, CacheServiceKey.SessionKeyLicenseeSettlementReport);
            int month = (int)SessionExtensions.GetSession(System.Web.HttpContext.Current.Session, CacheServiceKey.SessionKeyLicenseeSettlementMonth);
            int year = (int)SessionExtensions.GetSession(System.Web.HttpContext.Current.Session, CacheServiceKey.SessionKeyLicenseeSettlementYear);
            var reportName = model.LicenseeInfor.LicenseeName + "_" + model.LicenseeInfor.ProductName + "_" + "Settlement_For_" + month + "-" + year;
            var reportFolder = AppDomain.CurrentDomain.BaseDirectory + "ExcelFile\\" + CurrentSession.Username + "\\";
            _service.ExportReportToExcel(exchange, product, true);
            Application _excelApplication = null;
            Workbook workbook = null;
            try
            {
                _excelApplication = new Application();
                _excelApplication.Visible = false;
                workbook = _excelApplication.Workbooks.Open(reportFolder + reportName + DateTime.Now.ToString("dd-MMM-yyyy") + ".xls");
                string oldPrinter = _excelApplication.ActivePrinter;
                //_excelApplication.ActivePrinter = "Bullzip PDF Printer on Ne06:";
                //_excelApplication.ActivePrinter = string.Empty;
                Worksheet sheet = workbook.ActiveSheet;
                //sheet.PageSetup.PaperSize = XlPaperSize.xlPaperA5;
                sheet.PageSetup.Orientation = XlPageOrientation.xlLandscape;
                sheet.PageSetup.Zoom = false;
                sheet.PageSetup.FitToPagesWide = 1;
                sheet.PageSetup.FitToPagesTall = 1;
                sheet.PageSetup.LeftMargin = _excelApplication.InchesToPoints(0.7);
                sheet.PageSetup.RightMargin = _excelApplication.InchesToPoints(0.7);
                sheet.PageSetup.TopMargin = _excelApplication.InchesToPoints(0.75);
                sheet.PageSetup.BottomMargin = _excelApplication.InchesToPoints(0.75);
                sheet.PageSetup.HeaderMargin = _excelApplication.InchesToPoints(0.3);
                sheet.PageSetup.FooterMargin = _excelApplication.InchesToPoints(0.3);
                //_excelApplication.PrintCommunication = true;
                _excelApplication.Range["A1", "O32"].ExportAsFixedFormat(XlFixedFormatType.xlTypePDF, reportFolder + reportName.Replace(".xls", ".pdf"), OpenAfterPublish: false);
                //_excelApplication.Dialogs[XlBuiltInDialog.xlDialogPrint]
                // _excelApplication.ActivePrinter = oldPrinter;
                //workbook.Save();
                //workbook.SaveAs(reportFolder + reportName + ".pdf", XlFixedFormatType.xlTypePDF);
                //_excelApplication.ActiveWorkbook.SaveAs(reportFolder + reportName + ".pdf", XlFixedFormatType.xlTypePDF);
                workbook.Close(false);
                Marshal.FinalReleaseComObject(workbook);
                _excelApplication.Quit();
                Marshal.FinalReleaseComObject(_excelApplication);
            }
            catch (Exception ex)
            {
                workbook.Close(false);
                Marshal.FinalReleaseComObject(workbook);
                _excelApplication.Quit();
                Marshal.FinalReleaseComObject(_excelApplication);
            }


            System.IO.FileInfo file = new System.IO.FileInfo(reportFolder + reportName + ".pdf");
            var ms = file.OpenRead();
            ms.Position = 0;
            //System.IO.Stream stream = new System.IO.StreamReader(reportFolder + reportName + ".pdf").ReadToEnd;
            //System.IO.Stream stream = System.IO.File.OpenRead(reportFolder + reportName + ".pdf");
            //var response = HttpContext.Response;
            //response.Clear();
            //response.ContentType = "application/vnd.ms-excel";
            //response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}.xls", reportName));
            return new FileStreamResult(ms, "application/pdf") {
                FileDownloadName = reportName + ".pdf"
            };
            //return new EmptyResult();
        }

        public ActionResult ExportSettlementReportToExcel(string exchange, string product)
        {
            _service.ExportReportToExcel(exchange, product);
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
            var model = (LicenseeSettlementModel)SessionExtensions.GetSession(System.Web.HttpContext.Current.Session, CacheServiceKey.SessionKeyLicenseeSettlementReport);
            return View("PrintReport", model);
        }
    }
}