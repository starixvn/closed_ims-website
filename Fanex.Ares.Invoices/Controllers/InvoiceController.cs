// --------------------------------------------------------------------------
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


        public ActionResult ExportDocument(string invoiceModel)
        {
            List<ChargeRate> chargeRateList = new List<ChargeRate>();
            List<DateRange> dateRangeList = new List<DateRange>();
            IMS_Invoice_Rpt_LicenseeSettlementCriteria criteria = new IMS_Invoice_Rpt_LicenseeSettlementCriteria();

            InvoiceModel invoice = (InvoiceModel)SessionExtensions.GetSession(System.Web.HttpContext.Current.Session, "invoiceModelAfterSave");

            criteria.LicenseeId = invoice.Licensee.LicenseeID;
            criteria.BaseCurrency = invoice.Currency;
            criteria.BaseCurrencyName = invoice.CurrencyName;
            criteria.Currency = 0;
            criteria.PartyId = invoice.Operator.PartyID;
            DateTime FromDate = CastAide.AsDateTime(invoice.FromDate);
            DateTime ToDate = CastAide.AsDateTime(invoice.ToDate);
            dateRangeList = _invoiceService.GetDateRange(FromDate, ToDate);
            var excelCssFilePath = new List<string>();
            List<string> htmlList = new List<string>();
            string[] arrayView = new string[] { "Template/" + invoice.Operator.PartyName + "/_" + invoice.Operator.PartyName + "_" + invoice.InvoiceType.InvoiceTypeName };
            List<WorkbookManager.PageOrientation> PageOrientationList = new List<WorkbookManager.PageOrientation>();
            excelCssFilePath.Add(HostingEnvironment.MapPath(@"~\Areas\Invoice\Views\Invoice\Styles\InvoicePDF.css"));
            PageOrientationList.Add(WorkbookManager.PageOrientation.Portrait);
            string html = HtmlHelperExtensions.RenderViewToString(this, arrayView[0], invoice);
            htmlList.Add(html);

            List<bool> isAutoFitList = new List<bool>();
            isAutoFitList.Add(true);

            invoice.HTMLReportList.Add(html);

            string[] Products = invoice.ProductsName.Split(',');
            string[] ProductID = invoice.Product.Split(',');
            for (int i = dateRangeList.Count - 1; i >= 0; i--)
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

                    criteria.Product = Converter.To<int>(ProductID[j]);
                    criteria.ProductName = Products[j];
                    if (chargeRateList.Count == 0)
                    {
                        criteria.ToDate = dateRangeList[i].EndDate;
                        criteria.FromDate = dateRangeList[i].StartDate;
                        var model = _reportService.GenerateReport(criteria);
                        model.LicenseeInfor.FromDate = criteria.FromDate;
                        model.LicenseeInfor.ToDate = criteria.ToDate;

                        PageOrientationList.Add(WorkbookManager.PageOrientation.Landscape);
                        html = HtmlHelperExtensions.RenderViewToString(this, "_" + Products[j], model);
                        htmlList.Add(html);
                        invoice.HTMLReportList.Add(html);
                        excelCssFilePath.Add(HostingEnvironment.MapPath(@"~\Areas\Invoice\Views\Invoice\Styles\LicenseeSettlement.css"));
                        isAutoFitList.Add(true);

                    }
                    else
                    {
                        for (int g = chargeRateList.Count - 1; g >= 0; g--)
                        {
                            criteria.ToDate = chargeRateList[g].EndDate;
                            criteria.FromDate = chargeRateList[g].StartDate;

                            var model = _reportService.GenerateReport(criteria);
                            model.LicenseeInfor.FromDate = criteria.FromDate;
                            model.LicenseeInfor.ToDate = criteria.ToDate;

                            PageOrientationList.Add(WorkbookManager.PageOrientation.Landscape);
                            html = HtmlHelperExtensions.RenderViewToString(this, "_" + Products[j], model);
                            htmlList.Add(html);
                            invoice.HTMLReportList.Add(html);
                            excelCssFilePath.Add(HostingEnvironment.MapPath(@"~\Areas\Invoice\Views\Invoice\Styles\LicenseeSettlement.css"));
                            isAutoFitList.Add(true);

                        }
                    }
                }
            }

            var workbook = new WorkbookManager();
            WorkbookManager.DocumentType documentType = (WorkbookManager.DocumentType)Enum.Parse(typeof(WorkbookManager.DocumentType), "Pdf", false);
            var ms = workbook.CreateDocument(htmlList, excelCssFilePath, documentType, PageOrientationList, isAutoFitList);
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

            List<string> htmlList = new List<string>();
            string[] arrayView = new string[] { "Template/" + invoice.Operator.PartyName + "/_" + invoice.Operator.PartyName + "_" + invoice.InvoiceType.InvoiceTypeName };
            List<WorkbookManager.PageOrientation> PageOrientationList = new List<WorkbookManager.PageOrientation>();
            PageOrientationList.Add(WorkbookManager.PageOrientation.Portrait);
            string html = HtmlHelperExtensions.RenderViewToString(this, arrayView[0], invoice);
            htmlList.Add(html);

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

                    criteria.Product = Converter.To<int>(ProductID[j]);
                    criteria.ProductName = Products[j];

                    if (chargeRateList.Count == 0)
                    {
                        criteria.ToDate = dateRangeList[i].EndDate;
                        criteria.FromDate = dateRangeList[i].StartDate;
                        var model = _reportService.GenerateReport(criteria);
                        model.LicenseeInfor.FromDate = criteria.FromDate;
                        model.LicenseeInfor.ToDate = criteria.ToDate;

                        html = HtmlHelperExtensions.RenderViewToString(this, "_" + Products[j], model);
                        htmlList.Add(html);
                        invoice.HTMLReportList.Add(html);

                    }
                    else
                    {
                        for (int g = 0; g < chargeRateList.Count; g++)
                        {
                            criteria.ToDate = chargeRateList[g].EndDate;
                            criteria.FromDate = chargeRateList[g].StartDate;
                            var model = _reportService.GenerateReport(criteria);
                            model.LicenseeInfor.FromDate = criteria.FromDate;
                            model.LicenseeInfor.ToDate = criteria.ToDate;

                            PageOrientationList.Add(WorkbookManager.PageOrientation.Landscape);
                            html = HtmlHelperExtensions.RenderViewToString(this, "_" + Products[j], model);
                            htmlList.Add(html);
                            invoice.HTMLReportList.Add(html);

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

            List<string> htmlList = new List<string>();
            string html = string.Empty;

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

                    criteria.Product = Converter.To<int>(ProductID[j]);
                    criteria.ProductName = Products[j];

                    if (chargeRateList.Count == 0)
                    {
                        criteria.ToDate = dateRangeList[i].EndDate;
                        criteria.FromDate = dateRangeList[i].StartDate;
                        var model = _reportService.GenerateReport(criteria);
                        model.LicenseeInfor.FromDate = criteria.FromDate;
                        model.LicenseeInfor.ToDate = criteria.ToDate;

                        html = HtmlHelperExtensions.RenderViewToString(this, "_" + Products[j], model);
                        htmlList.Add(html);
                        invoice.HTMLReportList.Add(html);

                    }
                    else
                    {
                        for (int g = 0; g < chargeRateList.Count; g++)
                        {
                            criteria.ToDate = chargeRateList[g].EndDate;
                            criteria.FromDate = chargeRateList[g].StartDate;
                            var model = _reportService.GenerateReport(criteria);
                            model.LicenseeInfor.FromDate = criteria.FromDate;
                            model.LicenseeInfor.ToDate = criteria.ToDate;

                            html = HtmlHelperExtensions.RenderViewToString(this, "_" + Products[j], model);
                            htmlList.Add(html);
                            invoice.HTMLReportList.Add(html);

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
    }
}