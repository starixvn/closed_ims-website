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

    /// <summary>
    /// Class InvoiceController.
    /// </summary>
    [NapSuperAuth]
    public class InvoiceController : BaseController
    {
        private readonly HtmlViewRenderer _htmlViewRenderer;
        private readonly StandardPdfRenderer _standardPdfRenderer;
        private readonly string _pdfCssFilePath = HostingEnvironment.MapPath(@"~\Areas\Invoice\Views\Invoice\Styles\InvoicePDF.css");
        private ServiceBase _service;
        private InvoiceService _invoiceService;
        private ILicenseeSettlementService _reportService;

        #region View path
        private string _settlementView = string.Empty;
        #endregion

        public InvoiceController()
        {
            this._service = new ServiceBase();
            this._invoiceService = new InvoiceService();
            this._reportService = new LicenseeSettlementService();
            this._htmlViewRenderer = new HtmlViewRenderer();
            this._standardPdfRenderer = new StandardPdfRenderer();
        }

        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns>Action Result.</returns>
        [AuthenticateAction(AuthenticateAction.AuthenticationType.Read, "Index")]
        public ActionResult Index()
        {
            ViewBag.ListOperator = this._invoiceService.GetOperatorList(new IMS_OperatorCriteria() { PartyID = 0 });            
           return View();
        }

        /// <summary>
        /// Gets the invoice.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Partial View Result.</returns>
        public PartialViewResult GetInvoice(IMS_NewInvoiceItemsCriteria criteria)
        {
            var model = this._invoiceService.GetInvoice(criteria);
            model.FromDate = Converter.To<string>(criteria.FromDate);
            model.ToDate = Converter.To<string>(criteria.ToDate);

            bool isSignature = false;
            if (SessionExtensions.GetSession(System.Web.HttpContext.Current.Session, "isSignature") != null)
            {
                isSignature = (bool)SessionExtensions.GetSession(System.Web.HttpContext.Current.Session, "isSignature");
            }

            ViewBag.IsSignature = isSignature;
            return PartialView("~/Areas/Invoice/Views/Invoice/_Invoice.cshtml", model);
        }

        /// <summary>
        /// Gets the licensees by party.
        /// </summary>
        /// <param name="partyID">The party identifier.</param>
        /// <returns>Return Result.</returns>
        [HttpPost]
        public JsonResult GetLicenseesByParty(int partyID)
        {
            List<Licensee> licensees = new List<Licensee>();
            licensees = this._invoiceService.GeLicenseesList(new IMS_LicenseeByPartyCriteria() { PartyID = partyID });
            return Json(licensees);
        }

        /// <summary>
        /// Gets the type of the invoice.
        /// </summary>
        /// <param name="licenseeID">The licensee identifier.</param>
        /// <param name="partyID">The party identifier.</param>
        /// <returns>Return Result.</returns>
        [HttpPost]
        public JsonResult GetInvoiceType(int licenseeID, int partyID)
        {
            List<InvoiceType> invoiceTypeList = new List<InvoiceType>();
            invoiceTypeList = this._invoiceService.GetInvoieType(new IMS_InvoiceTypesCriteria() { LicenseeId = licenseeID, PartyId = partyID });
            return Json(invoiceTypeList);
        }

        /// <summary>
        /// Gets the base currency.
        /// </summary>
        /// <param name="licenseeID">The licensee identifier.</param>
        /// <param name="partyID">The party identifier.</param>
        /// <param name="products">The products.</param>
        /// <returns>Return Result.</returns>
        [HttpPost]
        public JsonResult GetBaseCurrency(int licenseeID, int partyID, string products)
        {
            var listExchangeRate = this._invoiceService.GetBaseCurrencyInvoiceFilter(
                new IMS_GenInv_BaseCurrencySel { LicenseeID = licenseeID, PartyID = partyID, Products = products });
            return Json(listExchangeRate);
        }

        /// <summary>
        /// Gets the invoice no.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Return Result.</returns>
        [HttpPost]
        public JsonResult GetInvoiceNo(IMS_InvoiceNoCriteria criteria)
        {
            int invoiceNo = this._invoiceService.GetInvoiceNo(criteria);
            return Json(invoiceNo);
        }

        /// <summary>
        /// Saves the invoice.
        /// </summary>
        /// <param name="invoiceModel">The invoice model.</param>
        /// <returns>Return Result.</returns>
        [HttpPost]
        [AuthenticateAction(AuthenticateAction.AuthenticationType.Host, "Index")]
        public JsonResult SaveInvoice(InvoiceModel invoiceModel)
        {
            var insertResult = this._invoiceService.SaveInvoice(invoiceModel);
            SessionExtensions.AddSession(System.Web.HttpContext.Current.Session, "invoiceModelAfterSave", invoiceModel);
            SessionExtensions.AddSession(System.Web.HttpContext.Current.Session, "isSignature", invoiceModel.IsSignature);
            return Json(insertResult);
        }

        /// <summary>
        /// Determines whether [is exist invoice no] [the specified criteria].
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Return Result.</returns>
        [HttpPost]
        public JsonResult IsExistInvoiceNo(IMS_CheckExistedInvoiceNo criteria)
        {
            return Json(this._invoiceService.IsExistInvoiceNo(criteria));
        }

        /// <summary>
        /// Gets the report.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Partial View Result.</returns>
        public PartialViewResult GetReport(IMS_Rpt_LicenseeSettlementCriteria criteria)
        {
            var model = new LicenseeSettlementModel();
            model.LicenseeInfor.FromDate = criteria.FromDate;
            model.LicenseeInfor.ToDate = criteria.ToDate;
            _settlementView = "_" + criteria.ProductName;

            return PartialView(this._settlementView, model);
        }

        /// <summary>
        /// Exports the document.
        /// </summary>
        /// <param name="isSignature">If set to <c>true</c> [is signature].</param>
        /// <returns>Action Result.</returns>
        [SuppressMessage(
            "StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed.")]
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
            dateRangeList = this._invoiceService.GetDateRange(fromDate, toDate);
            //// Crerate invoice main page
            string mainInvoicePagePath = string.Format(
                "Templates/{0}/_{1}_{2}", 
                invoice.Operator.PartyName, 
                invoice.Operator.PartyName, 
                invoice.InvoiceType.InvoiceTypeName);
            excelCssFilePath.Add(HostingEnvironment.MapPath(@"~\Areas\Invoice\Views\Invoice\Styles\InvoicePDF.css"));
            pageOrientationList.Add(WorkbookManager.PageOrientation.Portrait);
            string mainInvoicePage = HtmlHelperExtensions.RenderViewToString(this, mainInvoicePagePath, invoice, true);
            invoice.HTMLReportList.Add(mainInvoicePage);
            isAutoFitList.Add(false);
            //// Create invoice report pages
            string[] products = invoice.ProductsName.Split(',');
            string[] productIDs = invoice.Product.Split(',');
            string cssPath = HostingEnvironment.MapPath(@"~\Areas\Invoice\Views\Invoice\Styles\LicenseeSettlement.css");

            for (int i = 0; i < dateRangeList.Count; i++)
            {
                for (int j = 0; j < products.Count(); j++)
                {
                    chargeRateList = this._invoiceService.GetChargeRate(new IMS_GetChargeRateCriteria
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
                            this.CreateDocumentPagePDF(criteria, cssPath, ref invoice, ref excelCssFilePath, ref pageOrientationList, ref isAutoFitList);
                        }
                        else
                        {
                            for (int g = 0; g < chargeRateList.Count; g++)
                            {
                                criteria.ToDate = chargeRateList[g].EndDate;
                                criteria.FromDate = chargeRateList[g].StartDate;
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
                //// return the filestream. Rewind the memory stream to the beginning
                ms.Seek(0, SeekOrigin.Begin);
                string fileName = string.Format("{0}.{1}", invoice.InvoiceName, documentType.ToString().ToLower());
                return File(ms, @"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml", fileName);
            }

            return new EmptyResult();
        }

        /// <summary>
        /// Prints the screen.
        /// </summary>
        /// <param name="invoice">The invoice.</param>
        /// <returns>Action Result.</returns>
        [SuppressMessage(
            "StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed.")]
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
            dateRangeList = this._invoiceService.GetDateRange(fromDate, toDate);
            //// Crerate invoice main page
            string mainInvoicePage = string.Format(
                "Templates/{0}/_{1}_{2}", 
                invoice.Operator.PartyName,                 
                invoice.Operator.PartyName, 
                invoice.InvoiceType.InvoiceTypeName);            
            string html = HtmlHelperExtensions.RenderViewToString(this, mainInvoicePage, invoice, false);
            invoice.HTMLReportList.Add(html);
            string[] products = invoice.ProductsName.Split(',');
            string[] productID = invoice.Product.Split(',');
            for (int i = 0; i < dateRangeList.Count; i++)
            {
                for (int j = 0; j < products.Count(); j++)
                {
                    chargeRateList = this._invoiceService.GetChargeRate(new IMS_GetChargeRateCriteria
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

            return View("_Preview", invoice);
        }

        /// <summary>
        /// Details the specified invoice.
        /// </summary>
        /// <param name="invoice">The invoice.</param>
        /// <returns>Action Result.</returns>
        [SuppressMessage(
            "StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed.")]
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
            dateRangeList = this._invoiceService.GetDateRange(fromDate, toDate);
            string[] products = invoice.ProductsName.Split(',');
            string[] productID = invoice.Product.Split(',');
            for (int i = 0; i < dateRangeList.Count; i++)
            {
                for (int j = 0; j < products.Count(); j++)
                {
                    chargeRateList = this._invoiceService.GetChargeRate(new IMS_GetChargeRateCriteria
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

            return View("_Preview", invoice);
        }

        /// <summary>
        /// Reviews the invoice.
        /// </summary>
        /// <returns>Action Result.</returns>
        public ActionResult ReviewInvoice()
        {
            return new EmptyResult();
        }

        /// <summary>
        /// Creates the document page PDF.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <param name="cssPath">The CSS path.</param>
        /// <param name="invoice">The invoice.</param>
        /// <param name="excelCssFilePath">The excel CSS file path.</param>
        /// <param name="pageOrientationList">The page orientation list.</param>
        /// <param name="isAutoFitList">The is automatic fit list.</param>
        private void CreateDocumentPagePDF(
                                    IMS_Invoice_Rpt_LicenseeSettlementCriteria criteria,
                                    string cssPath,
                                    ref InvoiceModel invoice,
                                    ref List<string> excelCssFilePath,
                                    ref List<WorkbookManager.PageOrientation> pageOrientationList,
                                    ref List<bool> isAutoFitList)
        {
            string html = string.Empty;
            var model = this._reportService.GenerateReport(criteria);
            model.LicenseeInfor.FromDate = criteria.FromDate;
            model.LicenseeInfor.ToDate = criteria.ToDate;

            pageOrientationList.Add(WorkbookManager.PageOrientation.Landscape);
            html = HtmlHelperExtensions.RenderViewToString(this, "Products/_" + criteria.ProductName, model, false);
            invoice.HTMLReportList.Add(html);
            excelCssFilePath.Add(cssPath);
            isAutoFitList.Add(true);
        }

        /// <summary>
        /// Creates the document page web.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <param name="invoice">The invoice.</param>
        private void CreateDocumentPageWeb(
                                        IMS_Invoice_Rpt_LicenseeSettlementCriteria criteria,
                                        ref InvoiceModel invoice)
        {
            var model = this._reportService.GenerateReport(criteria);
            model.LicenseeInfor.FromDate = criteria.FromDate;
            model.LicenseeInfor.ToDate = criteria.ToDate;

            string html = HtmlHelperExtensions.RenderViewToString(this, "Products/_" + criteria.ProductName, model, false);
            invoice.HTMLReportList.Add(html);
        }
    }
}