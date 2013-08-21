﻿// --------------------------------------------------------------------------
// <copyright file="InvoiceController.cs" company="Nexcel Solutions Vietnam">
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
using Fanex.Core.PdfHelper;
using Fanex.NAP.Core.Web.Attribute;
using Fanex.Workbooks;
using Newtonsoft.Json;
using DNAide.Core;
using Fanex.Common;
using Fanex.Common.Extensions;
namespace Fanex.Ares.Invoice.Controllers
{
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    [NapSuperAuth]
    public class InvoiceController : BaseController
    {
        ServiceBase _service;
        InvoiceService _invoiceService;
        ILicenseeSettlementService _reportService;
        private readonly HtmlViewRenderer htmlViewRenderer;
        private readonly StandardPdfRenderer standardPdfRenderer;

        private static readonly string PdfCssFilePath = HostingEnvironment.MapPath(@"~\Areas\Invoice\Views\Invoice\Styles\InvoicePDF.css");

        #region View path

        protected string SettlementView = string.Empty;

        #endregion

        public InvoiceController()
        {
            _service = new ServiceBase();
            _invoiceService = new InvoiceService();
            _reportService = new LicenseeSettlementService();
            this.htmlViewRenderer = new HtmlViewRenderer();
            this.standardPdfRenderer = new StandardPdfRenderer();
        }

        [AuthenticateAction(AuthenticateAction.AuthenticationType.Read, "Index")]
        public ActionResult Index()
        {
            ViewBag.ListOperator = _invoiceService.GetOperatorList(new IMS_OperatorSelCriteria() { PartyID = 0 });
            ViewBag.ListExchangeRate = _service.GetBaseCurrency(new IMS_BaseCurrencySel());
            return View();
        }

        [HttpPost]
        public JsonResult GetLicenseesByParty(int partyID)
        {
            List<Licensee> licensees = new List<Licensee>();
            licensees = _invoiceService.GeLicenseesList(new IMS_LicenseeSelByPartyCriteria() { PartyID = partyID });
            return Json(licensees);
        }

        [HttpPost]
        public JsonResult GetInvoiceType(int licenseeID)
        {
            List<InvoiceType> invoiceTypeList = new List<InvoiceType>();
            invoiceTypeList = _invoiceService.GetInvoieType(new IMS_InvoieTypesSelCriteria() { LicenseeId = licenseeID });
            return Json(invoiceTypeList);
        }

        [HttpPost]
        public JsonResult GetInvoiceNo(IMS_InvoiceNoSelCriteria criteria)
        {
            int invoiceNo = _invoiceService.GetInvoieNo(criteria);
            return Json(invoiceNo);
        }

        [HttpPost]
        [AuthenticateAction(AuthenticateAction.AuthenticationType.Host, "Index")]
        public JsonResult SaveInvoice(InvoiceModel invoiceModel)
        {
            var insertResult = _invoiceService.SaveInvoice(invoiceModel);
            SessionExtensions.AddSession(System.Web.HttpContext.Current.Session, "invoiceModelAfterSave", invoiceModel);
            return Json(insertResult);
        }

        [HttpPost]
        public JsonResult IsExistInvoiceNo(IMS_CheckExistedInvoiceNo criteria)
        {
            return Json(_invoiceService.IsExistInvoiceNo(criteria));
        }

        public PartialViewResult GetInvoice(IMS_NewInvoiceItemsSelCriteria criteria)
        {
            var model = _invoiceService.GetInvoice(criteria);
            model.FromDate = Converter.To<string>(criteria.FromDate);
            model.ToDate = Converter.To<string>(criteria.ToDate);

            //SessionExtensions.AddSession(System.Web.HttpContext.Current.Session, "chargeRateList", chargeRateList);
            return PartialView("~/Areas/Invoice/Views/Invoice/_Invoice.cshtml", model);
        }

        public PartialViewResult GetReport(IMS_Rpt_LicenseeSettlementCriteria criteria)
        {
            var model = new LicenseeSettlementModel();// _service.GenerateReport(criteria);
            model.LicenseeInfor.FromDate = criteria.FromDate;
            model.LicenseeInfor.ToDate = criteria.ToDate;
            SettlementView = "_" + criteria.ProductName;

            return PartialView(SettlementView, model);
        }

        public ActionResult ExportDocument(bool isSignature)
        {
            List<ChargeRate> chargeRateList = new List<ChargeRate>();
            List<DateRange> dateRangeList = new List<DateRange>();
            List<string> excelCssFilePath = new List<string>();
            List<bool> isAutoFitList = new List<bool>();
            List<WorkbookManager.PageOrientation> PageOrientationList = new List<WorkbookManager.PageOrientation>();

            IMS_Invoice_Rpt_LicenseeSettlementCriteria criteria = new IMS_Invoice_Rpt_LicenseeSettlementCriteria();

            InvoiceModel invoice = (InvoiceModel)SessionExtensions.GetSession(System.Web.HttpContext.Current.Session, "invoiceModelAfterSave");
            invoice.HTMLReportList = new List<string>(); // clear html when using ref
            invoice.IsSignature = isSignature;

            criteria.LicenseeId = invoice.Licensee.LicenseeID;
            criteria.BaseCurrency = invoice.Currency;
            criteria.BaseCurrencyName = invoice.CurrencyName;
            criteria.Currency = 0;
            criteria.PartyId = invoice.Operator.PartyID;

            DateTime FromDate = CastAide.AsDateTime(invoice.FromDate);
            DateTime ToDate = CastAide.AsDateTime(invoice.ToDate);
            dateRangeList = _invoiceService.GetDateRange(FromDate, ToDate);

            // Crerate invoice main page
            string mainInvoicePagePath ="Template/" + invoice.Operator.PartyName + "/_" + invoice.Operator.PartyName + "_" + invoice.InvoiceType.InvoiceTypeName;
            excelCssFilePath.Add(HostingEnvironment.MapPath(@"~\Areas\Invoice\Views\Invoice\Styles\InvoicePDF.css"));
            PageOrientationList.Add(WorkbookManager.PageOrientation.Portrait);
            string mainInvoicePage = HtmlHelperExtensions.RenderViewToString(this, mainInvoicePagePath, invoice, true);
            invoice.HTMLReportList.Add(mainInvoicePage);
            isAutoFitList.Add(false);

            // Create invoice report pages
            string[] Products = invoice.ProductsName.Split(',');
            string[] ProductIDs = invoice.Product.Split(',');

            for (int i = 0; i < dateRangeList.Count; i++)
            {
                for (int j = 0; j < Products.Count(); j++)
                {
                    chargeRateList = _invoiceService.GetChargeRate(new IMS_GetChargeRateCriteria
                    {
                        partyID = invoice.Operator.PartyID,
                        licenseeID = invoice.Licensee.LicenseeID,
                        Products = invoice.Product,
                        FromDate = dateRangeList[i].StartDate,
                        ToDate = dateRangeList[i].EndDate
                    });

                    if (!string.IsNullOrEmpty(ProductIDs[j]))
                    {
                        criteria.Product = Converter.To<int>(ProductIDs[j]);
                        criteria.ProductName = Products[j];
                        criteria.InvoiceReport = true;
                        if (chargeRateList.Count == 0) // some case chargeRateList == 0 when don't have more than one basic billing
                        {
                            criteria.ToDate = dateRangeList[i].EndDate;
                            criteria.FromDate = dateRangeList[i].StartDate;
                            string cssPath = HostingEnvironment.MapPath(@"~\Areas\Invoice\Views\Invoice\Styles\LicenseeSettlement.css");

                            CreateDocumentPagePDF(criteria, cssPath, ref invoice, ref excelCssFilePath, ref PageOrientationList, ref isAutoFitList);
                        }
                        else
                        {
                            for (int g = 0; g < chargeRateList.Count; g++)
                            {
                                criteria.ToDate = chargeRateList[g].EndDate;
                                criteria.FromDate = chargeRateList[g].StartDate;
                                string cssPath = HostingEnvironment.MapPath(@"~\Areas\Invoice\Views\Invoice\Styles\LicenseeSettlement.css");

                                CreateDocumentPagePDF(criteria, cssPath, ref invoice, ref excelCssFilePath, ref PageOrientationList, ref isAutoFitList);
                            }
                        }
                    }
                }
            }
            //Xlsx
            var workbook = new WorkbookManager();
            WorkbookManager.DocumentType documentType = (WorkbookManager.DocumentType)Enum.Parse(typeof(WorkbookManager.DocumentType), "Pdf", false);
            var ms = workbook.CreateDocument(invoice.HTMLReportList, excelCssFilePath, documentType, PageOrientationList, isAutoFitList);
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

            DateTime FromDate = CastAide.AsDateTime(invoice.FromDate);
            DateTime ToDate = CastAide.AsDateTime(invoice.ToDate);
            dateRangeList = _invoiceService.GetDateRange(FromDate, ToDate);

            // Crerate invoice main page
            string mainInvoicePage ="Template/" + invoice.Operator.PartyName + "/_" + invoice.Operator.PartyName + "_" + invoice.InvoiceType.InvoiceTypeName;
            string html = HtmlHelperExtensions.RenderViewToString(this, mainInvoicePage, invoice, false);
            invoice.HTMLReportList.Add(html);

            string[] Products = invoice.ProductsName.Split(',');
            string[] ProductID = invoice.Product.Split(',');
            for (int i = 0; i < dateRangeList.Count; i++)
            {
                for (int j = 0; j < Products.Count(); j++)
                {
                    chargeRateList = _invoiceService.GetChargeRate(new IMS_GetChargeRateCriteria
                    {
                        partyID = invoice.Operator.PartyID,
                        licenseeID = invoice.Licensee.LicenseeID,
                        Products = invoice.Product,
                        FromDate = dateRangeList[i].StartDate,
                        ToDate = dateRangeList[i].EndDate
                    });

                    if (!string.IsNullOrEmpty(ProductID[j]))
                    {
                        criteria.Product = Converter.To<int>(ProductID[j]);
                        criteria.ProductName = Products[j];
                        criteria.InvoiceReport = true;
                        if (chargeRateList.Count == 0)
                        {
                            criteria.ToDate = dateRangeList[i].EndDate;
                            criteria.FromDate = dateRangeList[i].StartDate;

                            CreateDocumentPageWeb(criteria,ref invoice);
                        }
                        else
                        {
                            for (int g = 0; g < chargeRateList.Count; g++)
                            {
                                criteria.ToDate = chargeRateList[g].EndDate;
                                criteria.FromDate = chargeRateList[g].StartDate;

                                CreateDocumentPageWeb(criteria, ref invoice);
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
            DateTime FromDate = CastAide.AsDateTime(invoice.FromDate);
            DateTime ToDate = CastAide.AsDateTime(invoice.ToDate);

            dateRangeList = _invoiceService.GetDateRange(FromDate, ToDate);

            string[] Products = invoice.ProductsName.Split(',');
            string[] ProductID = invoice.Product.Split(',');
            for (int i = 0; i < dateRangeList.Count; i++)
            {
                for (int j = 0; j < Products.Count(); j++)
                {
                    chargeRateList = _invoiceService.GetChargeRate(new IMS_GetChargeRateCriteria
                    {
                        partyID = invoice.Operator.PartyID,
                        licenseeID = invoice.Licensee.LicenseeID,
                        Products = invoice.Product,
                        FromDate = dateRangeList[i].StartDate,
                        ToDate = dateRangeList[i].EndDate
                    });
                    if (!string.IsNullOrEmpty(ProductID[j]))
                    {
                        criteria.Product = Converter.To<int>(ProductID[j]);
                        criteria.ProductName = Products[j];
                        criteria.InvoiceReport = true;
                        if (chargeRateList.Count == 0)
                        {
                            criteria.ToDate = dateRangeList[i].EndDate;
                            criteria.FromDate = dateRangeList[i].StartDate;

                            CreateDocumentPageWeb(criteria, ref invoice);
                        }
                        else
                        {
                            for (int g = 0; g < chargeRateList.Count; g++)
                            {
                                criteria.ToDate = chargeRateList[g].EndDate;
                                criteria.FromDate = chargeRateList[g].StartDate;

                                CreateDocumentPageWeb(criteria, ref invoice);
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

        private void CreateDocumentPagePDF(IMS_Invoice_Rpt_LicenseeSettlementCriteria criteria,
                                    string cssPath,
                                    ref  InvoiceModel invoice,
                                    ref  List<string> excelCssFilePath,
                                    ref List<WorkbookManager.PageOrientation> PageOrientationList,
                                    ref List<bool> isAutoFitList)
        {
            string html = string.Empty;
            var model = _reportService.GenerateReport(criteria);
            model.LicenseeInfor.FromDate = criteria.FromDate;
            model.LicenseeInfor.ToDate = criteria.ToDate;

            PageOrientationList.Add(WorkbookManager.PageOrientation.Landscape);
            html = HtmlHelperExtensions.RenderViewToString(this, "_" + criteria.ProductName, model, false);
            invoice.HTMLReportList.Add(html);
            excelCssFilePath.Add(cssPath);
            isAutoFitList.Add(true);
        }

        private void CreateDocumentPageWeb(IMS_Invoice_Rpt_LicenseeSettlementCriteria criteria,
                                        ref  InvoiceModel invoice)
        {
            var model = _reportService.GenerateReport(criteria);
            model.LicenseeInfor.FromDate = criteria.FromDate;
            model.LicenseeInfor.ToDate = criteria.ToDate;

            string html = HtmlHelperExtensions.RenderViewToString(this, "_" + criteria.ProductName, model, false);
            invoice.HTMLReportList.Add(html);
        }
    }
}