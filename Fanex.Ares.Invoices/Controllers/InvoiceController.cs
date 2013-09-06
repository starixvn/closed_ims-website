// <copyright file="InvoiceController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="">Created</change>
// </history>

namespace Fanex.Ares.Invoice.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using System.Web.Hosting;
    using System.Web.Mvc;
    using DNAide.Core;
    using Fanex.Ares.Core.Criteria;
    using Fanex.Ares.Core.Extensions;
    using Fanex.Ares.Core.Services.Impl;
    using Fanex.Ares.Core.Web.Controllers;
    using Fanex.Ares.Core.Web.Security;
    using Fanex.Ares.Invoice.Criteria.Invoice;
    using Fanex.Ares.Invoice.Models.Invoice;
    using Fanex.Ares.Invoice.Service.Imp;
    using Fanex.Ares.Report.Criteria.LicenseeSettlement;
    using Fanex.Ares.Report.Services.Interface;
    using Fanex.Ares.Report.ViewModels.LicenseeSettlement;
    using Fanex.Common;
    using Fanex.Common.Extensions;
    using Fanex.Core.PdfHelper;
    using Fanex.NAP.Core.Web.Attribute;
    using Fanex.Workbooks;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Run stylecop")]
    [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1126:PrefixCallsCorrectly", Justification = "Reviewed.")]
    [NapSuperAuth]
    public class InvoiceController : BaseController
    {
        private readonly HtmlViewRenderer htmlViewRenderer;
        private readonly StandardPdfRenderer standardPdfRenderer;
        private readonly string pdfCssFilePath = HostingEnvironment.MapPath(@"~\Areas\Invoice\Views\Invoice\Styles\InvoicePDF.css");

        private ServiceBase service;
        private InvoiceService invoiceService;
        private ILicenseeSettlementService reportService;

        #region View path
        private string settlementView = string.Empty;
        #endregion

        public InvoiceController()
        {
            this.service = new ServiceBase();
            this.invoiceService = new InvoiceService();
            this.reportService = new LicenseeSettlementService();
            this.htmlViewRenderer = new HtmlViewRenderer();
            this.standardPdfRenderer = new StandardPdfRenderer();
        }

        [AuthenticateAction(AuthenticateAction.AuthenticationType.Read, "Index")]
        public ActionResult Index()
        {
            ViewBag.ListOperator = this.invoiceService.GetOperatorList(new IMS_OperatorCriteria() { PartyID = 0 });
            ViewBag.ListExchangeRate = this.service.GetBaseCurrency(new IMS_BaseCurrencySel());
            return View();
        }

        [HttpPost]
        public JsonResult GetLicenseesByParty(int partyID)
        {
            List<Licensee> licensees = new List<Licensee>();
            licensees = this.invoiceService.GeLicenseesList(new IMS_LicenseeByPartyCriteria() { PartyID = partyID });
            return Json(licensees);
        }

        [HttpPost]
        public JsonResult GetInvoiceType(int licenseeID)
        {
            List<InvoiceType> invoiceTypeList = new List<InvoiceType>();
            invoiceTypeList = this.invoiceService.GetInvoieType(new IMS_InvoiceTypesCriteria() { LicenseeId = licenseeID });
            return Json(invoiceTypeList);
        }

        [HttpPost]
        public JsonResult GetInvoiceNo(IMS_InvoiceNoCriteria criteria)
        {
            int invoiceNo = this.invoiceService.GetInvoiceNo(criteria);
            return Json(invoiceNo);
        }

        [HttpPost]
        [AuthenticateAction(AuthenticateAction.AuthenticationType.Host, "Index")]
        public JsonResult SaveInvoice(InvoiceModel invoiceModel)
        {
            var insertResult = this.invoiceService.SaveInvoice(invoiceModel);
            SessionExtensions.AddSession(System.Web.HttpContext.Current.Session, "invoiceModelAfterSave", invoiceModel);
            return Json(insertResult);
        }

        [HttpPost]
        public JsonResult IsExistInvoiceNo(IMS_CheckExistedInvoiceNo criteria)
        {
            return Json(this.invoiceService.IsExistInvoiceNo(criteria));
        }

        public PartialViewResult GetInvoice(IMS_NewInvoiceItemsCriteria criteria)
        {
            var model = this.invoiceService.GetInvoice(criteria);
            model.FromDate = Converter.To<string>(criteria.FromDate);
            model.ToDate = Converter.To<string>(criteria.ToDate);

            return PartialView("~/Areas/Invoice/Views/Invoice/_Invoice.cshtml", model);
            //return PartialView("~/Areas/Invoice/Views/Invoice/_Invoice_Angular.cshtml", model);
        }

        public PartialViewResult GetReport(IMS_Rpt_LicenseeSettlementCriteria criteria)
        {
            var model = new LicenseeSettlementModel();
            model.LicenseeInfor.FromDate = criteria.FromDate;
            model.LicenseeInfor.ToDate = criteria.ToDate;
            settlementView = "_" + criteria.ProductName;

            return PartialView(this.settlementView, model);
        }

        public ActionResult ExportDocument(bool isSignature)
        {
            List<ChargeRate> chargeRateList = new List<ChargeRate>();
            List<DateRange> dateRangeList = new List<DateRange>();
            List<string> excelCssFilePath = new List<string>();
            List<bool> isAutoFitList = new List<bool>();
            List<WorkbookManager.PageOrientation> pageOrientationList = new List<WorkbookManager.PageOrientation>();

            IMS_Invoice_Rpt_LicenseeSettlementCriteria criteria = new IMS_Invoice_Rpt_LicenseeSettlementCriteria();

            InvoiceModel invoice = (InvoiceModel)SessionExtensions.GetSession(System.Web.HttpContext.Current.Session, "invoiceModelAfterSave");
            invoice.HTMLReportList = new List<string>(); // clear html when using ref
            invoice.IsSignature = isSignature;

            criteria.LicenseeId = invoice.Licensee.LicenseeID;
            criteria.BaseCurrency = invoice.Currency;
            criteria.BaseCurrencyName = invoice.CurrencyName;
            criteria.Currency = 0;
            criteria.PartyId = invoice.Operator.PartyID;

            DateTime fromDate = CastAide.AsDateTime(invoice.FromDate);
            DateTime toDate = CastAide.AsDateTime(invoice.ToDate);
            dateRangeList = this.invoiceService.GetDateRange(fromDate, toDate);

            // Crerate invoice main page
            string mainInvoicePagePath = "Templates/" + invoice.Operator.PartyName + "/_" + invoice.Operator.PartyName + "_" + invoice.InvoiceType.InvoiceTypeName;
            excelCssFilePath.Add(HostingEnvironment.MapPath(@"~\Areas\Invoice\Views\Invoice\Styles\InvoicePDF.css"));
            pageOrientationList.Add(WorkbookManager.PageOrientation.Portrait);
            string mainInvoicePage = HtmlHelperExtensions.RenderViewToString(this, mainInvoicePagePath, invoice, true);
            invoice.HTMLReportList.Add(mainInvoicePage);
            isAutoFitList.Add(false);

            // Create invoice report pages
            string[] products = invoice.ProductsName.Split(',');
            string[] productIDs = invoice.Product.Split(',');

            for (int i = 0; i < dateRangeList.Count; i++)
            {
                for (int j = 0; j < products.Count(); j++)
                {
                    chargeRateList = this.invoiceService.GetChargeRate(new IMS_GetChargeRateCriteria
                    {
                        PartyID = invoice.Operator.PartyID,
                        LicenseeID = invoice.Licensee.LicenseeID,
                        Products = products[j],
                        FromDate = dateRangeList[i].StartDate,
                        ToDate = dateRangeList[i].EndDate
                    });

                    if (!string.IsNullOrEmpty(productIDs[j]))
                    {
                        criteria.Product = Converter.To<int>(productIDs[j]);
                        criteria.ProductName = products[j];
                        criteria.InvoiceReport = true;

                        //// some case chargeRateList == 0 when don't have more than one basic billing
                        if (chargeRateList.Count == 0) 
                        {
                            criteria.ToDate = dateRangeList[i].EndDate;
                            criteria.FromDate = dateRangeList[i].StartDate;
                            string cssPath = HostingEnvironment.MapPath(@"~\Areas\Invoice\Views\Invoice\Styles\LicenseeSettlement.css");

                            this.CreateDocumentPagePDF(criteria, cssPath, ref invoice, ref excelCssFilePath, ref pageOrientationList, ref isAutoFitList);
                        }
                        else
                        {
                            for (int g = 0; g < chargeRateList.Count; g++)
                            {
                                criteria.ToDate = chargeRateList[g].EndDate;
                                criteria.FromDate = chargeRateList[g].StartDate;
                                string cssPath = HostingEnvironment.MapPath(@"~\Areas\Invoice\Views\Invoice\Styles\LicenseeSettlement.css");

                                this.CreateDocumentPagePDF(criteria, cssPath, ref invoice, ref excelCssFilePath, ref pageOrientationList, ref isAutoFitList);
                            }
                        }
                    }
                }
            }
           
            var workbook = new WorkbookManager();
            WorkbookManager.DocumentType documentType = (WorkbookManager.DocumentType)Enum.Parse(typeof(WorkbookManager.DocumentType), "Pdf", false);
            var ms = workbook.CreateDocument(invoice.HTMLReportList, excelCssFilePath, documentType, pageOrientationList, isAutoFitList);
            if (ms != null)
            {
                // return the filestream
                // Rewind the memory stream to the beginning
                ms.Seek(0, SeekOrigin.Begin);

                string fileName = invoice.InvoiceName + "." + documentType.ToString().ToLower();

                return File(ms, @"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml", fileName);
            }

            return new EmptyResult();
        }

        public ActionResult PrintScreen(InvoiceModel invoice)
        {
            List<ChargeRate> chargeRateList = new List<ChargeRate>();
            List<DateRange> dateRangeList = new List<DateRange>();
            var criteria = new IMS_Invoice_Rpt_LicenseeSettlementCriteria();

            criteria.LicenseeId = invoice.Licensee.LicenseeID;
            criteria.BaseCurrency = invoice.Currency;
            criteria.BaseCurrencyName = invoice.CurrencyName;
            criteria.Currency = 0;
            criteria.PartyId = invoice.Operator.PartyID;

            DateTime fromDate = CastAide.AsDateTime(invoice.FromDate);
            DateTime toDate = CastAide.AsDateTime(invoice.ToDate);
            dateRangeList = this.invoiceService.GetDateRange(fromDate, toDate);

            // Crerate invoice main page
            string mainInvoicePage = "Templates/" + invoice.Operator.PartyName + "/_" + invoice.Operator.PartyName + "_" + invoice.InvoiceType.InvoiceTypeName;
            string html = HtmlHelperExtensions.RenderViewToString(this, mainInvoicePage, invoice, false);
            invoice.HTMLReportList.Add(html);

            string[] products = invoice.ProductsName.Split(',');
            string[] productID = invoice.Product.Split(',');
            for (int i = 0; i < dateRangeList.Count; i++)
            {
                for (int j = 0; j < products.Count(); j++)
                {
                    chargeRateList = this.invoiceService.GetChargeRate(new IMS_GetChargeRateCriteria
                    {
                        PartyID = invoice.Operator.PartyID,
                        LicenseeID = invoice.Licensee.LicenseeID,
                        Products = productID[j],
                        FromDate = dateRangeList[i].StartDate,
                        ToDate = dateRangeList[i].EndDate
                    });

                    if (!string.IsNullOrEmpty(productID[j]))
                    {
                        criteria.Product = Converter.To<int>(productID[j]);
                        criteria.ProductName = products[j];
                        criteria.InvoiceReport = true;
                        if (chargeRateList.Count == 0)
                        {
                            criteria.ToDate = dateRangeList[i].EndDate;
                            criteria.FromDate = dateRangeList[i].StartDate;

                            this.CreateDocumentPageWeb(criteria, ref invoice);
                        }
                        else
                        {
                            for (int g = 0; g < chargeRateList.Count; g++)
                            {
                                criteria.ToDate = chargeRateList[g].EndDate;
                                criteria.FromDate = chargeRateList[g].StartDate;

                                this.CreateDocumentPageWeb(criteria, ref invoice);
                            }
                        }
                    }
                }
            }

            return View("_Review", invoice);
        }

        public ActionResult Detail(InvoiceModel invoice)
        {
            List<ChargeRate> chargeRateList = new List<ChargeRate>();
            List<DateRange> dateRangeList = new List<DateRange>();
            var criteria = new IMS_Invoice_Rpt_LicenseeSettlementCriteria();

            InvoiceModel imodel = new InvoiceModel();

            criteria.LicenseeId = invoice.Licensee.LicenseeID;
            criteria.BaseCurrency = invoice.Currency;
            criteria.BaseCurrencyName = invoice.CurrencyName;
            criteria.Currency = 0;
            criteria.PartyId = invoice.Operator.PartyID;
            DateTime fromDate = CastAide.AsDateTime(invoice.FromDate);
            DateTime toDate = CastAide.AsDateTime(invoice.ToDate);

            dateRangeList = this.invoiceService.GetDateRange(fromDate, toDate);

            string[] products = invoice.ProductsName.Split(',');
            string[] productID = invoice.Product.Split(',');
            for (int i = 0; i < dateRangeList.Count; i++)
            {
                for (int j = 0; j < products.Count(); j++)
                {
                    chargeRateList = this.invoiceService.GetChargeRate(new IMS_GetChargeRateCriteria
                    {
                        PartyID = invoice.Operator.PartyID,
                        LicenseeID = invoice.Licensee.LicenseeID,
                        Products = productID[j],
                        FromDate = dateRangeList[i].StartDate,
                        ToDate = dateRangeList[i].EndDate
                    });
                    if (!string.IsNullOrEmpty(productID[j]))
                    {
                        criteria.Product = Converter.To<int>(productID[j]);
                        criteria.ProductName = products[j];
                        criteria.InvoiceReport = true;
                        if (chargeRateList.Count == 0)
                        {
                            criteria.ToDate = dateRangeList[i].EndDate;
                            criteria.FromDate = dateRangeList[i].StartDate;

                            this.CreateDocumentPageWeb(criteria, ref invoice);
                        }
                        else
                        {
                            for (int g = 0; g < chargeRateList.Count; g++)
                            {
                                criteria.ToDate = chargeRateList[g].EndDate;
                                criteria.FromDate = chargeRateList[g].StartDate;

                                this.CreateDocumentPageWeb(criteria, ref invoice);
                            }
                        }
                    }
                }
            }

            return View("_Review", invoice);
        }

        public ActionResult ReviewInvoice()
        {
            return new EmptyResult();
        }

        private void CreateDocumentPagePDF(
                                    IMS_Invoice_Rpt_LicenseeSettlementCriteria criteria,
                                    string cssPath,
                                    ref InvoiceModel invoice,
                                    ref List<string> excelCssFilePath,
                                    ref List<WorkbookManager.PageOrientation> pageOrientationList,
                                    ref List<bool> isAutoFitList)
        {
            string html = string.Empty;
            var model = this.reportService.GenerateReport(criteria);
            model.LicenseeInfor.FromDate = criteria.FromDate;
            model.LicenseeInfor.ToDate = criteria.ToDate;

            pageOrientationList.Add(WorkbookManager.PageOrientation.Landscape);
            html = HtmlHelperExtensions.RenderViewToString(this, "Products/_" + criteria.ProductName, model, false);
            invoice.HTMLReportList.Add(html);
            excelCssFilePath.Add(cssPath);
            isAutoFitList.Add(true);
        }

        private void CreateDocumentPageWeb(
                                        IMS_Invoice_Rpt_LicenseeSettlementCriteria criteria,
                                        ref InvoiceModel invoice)
        {
            var model = this.reportService.GenerateReport(criteria);
            model.LicenseeInfor.FromDate = criteria.FromDate;
            model.LicenseeInfor.ToDate = criteria.ToDate;

            string html = HtmlHelperExtensions.RenderViewToString(this, "Products/_" + criteria.ProductName, model, false);
            invoice.HTMLReportList.Add(html);
        }
    }
}