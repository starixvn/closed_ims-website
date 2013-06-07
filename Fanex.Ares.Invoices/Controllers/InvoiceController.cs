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


        public PartialViewResult GetInvoice(IMS_NewInvoiceItemsSelCriteria criteria)
        {
            var model = _invoiceService.GetInvoice(criteria);
            model.FromDate = Converter.To<string>(criteria.FromDate);
            model.ToDate = Converter.To<string>(criteria.ToDate);
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

        public List<DateRange> GetDateRange(DateTime FromDate, DateTime ToDate)
        {
            List<DateRange> dateRangeList = new List<DateRange>();

            int numericMonth = ToDate.Month - FromDate.Month + 1;

            for (int i = 0; i < numericMonth; i++)
            {
                DateRange dateRange = new DateRange();
                DateTime StartDateTmp = new DateTime();
                DateTime EndDateTmp = new DateTime();
                if (i == 0)
                {
                    StartDateTmp = FromDate;
                    DateTime firstOfNextMonth = new DateTime(FromDate.Year, FromDate.Month, 1).AddMonths(1);
                    //DateTime lastOfThisMonth = firstOfNextMonth.AddDays(-1);
                    EndDateTmp = firstOfNextMonth.AddDays(-1);
                }
                else if (i == numericMonth)
                {
                    StartDateTmp = new DateTime(FromDate.Year, ToDate.Month, 1);
                    //DateTime firstOfNextMonth = new DateTime(FromDate.Year + i - 1, FromDate.Month + i - 1, 1).AddMonths(1);
                    //DateTime lastOfThisMonth = firstOfNextMonth.AddDays(-1);
                    EndDateTmp = ToDate;
                }
                else
                {
                    StartDateTmp = new DateTime(FromDate.Year, FromDate.Month + i, 1);
                    DateTime firstOfNextMonth = new DateTime(FromDate.Year, FromDate.Month + i, 1).AddMonths(1);
                    //DateTime lastOfThisMonth = firstOfNextMonth.AddDays(-1);
                    EndDateTmp = firstOfNextMonth.AddDays(-1);
                }

                dateRange.StartDate = StartDateTmp;
                dateRange.EndDate = EndDateTmp;
                dateRangeList.Add(dateRange);
            }

            return dateRangeList;
        }
        public ActionResult ExportDocument(string invoiceModel)
        {
            List<DateRange> dateRangeList = new List<DateRange>();
            IMS_Invoice_Rpt_LicenseeSettlementCriteria criteria = new IMS_Invoice_Rpt_LicenseeSettlementCriteria();

            criteria.LicenseeId = 4;
            criteria.BaseCurrency = 6;
            criteria.BaseCurrencyName = "EUR";
            criteria.Currency = 0;
            //criteria.FromDate = new DateTime(2013, 5, 01);
            //criteria.ToDate = new DateTime(2013, 5, 31);
            //criteria.Product = 0;
            //criteria.ProductName = "Sportsbook";

            InvoiceModel imodel = new InvoiceModel();

            InvoiceModel invoice = JsonConvert.DeserializeObject<InvoiceModel>(invoiceModel);

            //criteria.LicenseeId = invoice.Licensee.LicenseeID;
            //criteria.BaseCurrency = invoice.Currency;
            //criteria.BaseCurrencyName = invoice.CurrencyName;
            //criteria.Currency = 0;

            DateTime FromDate = CastAide.AsDateTime(invoice.FromDate);
            DateTime ToDate = CastAide.AsDateTime(invoice.ToDate);

            dateRangeList = GetDateRange(FromDate, ToDate);
            var excelCssFilePath = new List<string>();
            List<string> htmlList = new List<string>();
            string[] arrayView = new string[] { "Template//BCH/_BCH-DataFees" };
            List<WorkbookManager.PageOrientation> PageOrientationList = new List<WorkbookManager.PageOrientation>();
            excelCssFilePath.Add(HostingEnvironment.MapPath(@"~\Areas\Invoice\Views\Invoice\Styles\InvoicePDF.css"));
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


                    criteria.ToDate = dateRangeList[i].EndDate;
                    criteria.FromDate = dateRangeList[i].StartDate;
                    criteria.Product = Converter.To<int>(ProductID[j]);
                    criteria.ProductName = Products[j];

                    var model = _reportService.GenerateReport(criteria);

                    model.LicenseeInfor.FromDate = criteria.FromDate;
                    model.LicenseeInfor.ToDate = criteria.ToDate;
                    if (model.Report.EntityList.Count > 0)
                    {
                        PageOrientationList.Add(WorkbookManager.PageOrientation.Landscape);
                        html = HtmlHelperExtensions.RenderViewToString(this, "_" + Products[j], model);
                        htmlList.Add(html);
                        invoice.HTMLReportList.Add(html);
                        excelCssFilePath.Add(HostingEnvironment.MapPath(@"~\Areas\Invoice\Views\Invoice\Styles\LicenseeSettlement.css"));
                    }
                }

            }

            var workbook = new WorkbookManager();
            WorkbookManager.DocumentType documentType = (WorkbookManager.DocumentType)Enum.Parse(typeof(WorkbookManager.DocumentType), "Pdf", false);
            var ms = workbook.CreateDocument(htmlList, excelCssFilePath, documentType, PageOrientationList);
            if (ms != null)
            {
                // return the filestream
                // Rewind the memory stream to the beginning
                ms.Seek(0, SeekOrigin.Begin);

                string fileName = "invoice" + "." + documentType.ToString().ToLower();

                return File(ms, @"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml", fileName);
            }

            return new EmptyResult();
        }

        public ActionResult PrintScreen(string invoiceModel)
        {
            List<DateRange> dateRangeList = new List<DateRange>();
            IMS_Rpt_LicenseeSettlementCriteria criteria = new IMS_Rpt_LicenseeSettlementCriteria();

            criteria.LicenseeId = 1;
            criteria.BaseCurrency = 6;
            criteria.BaseCurrencyName = "EUR";
            criteria.Currency = 0;
            //criteria.FromDate = new DateTime(2013, 5, 01);
            //criteria.ToDate = new DateTime(2013, 5, 31);
            //criteria.Product = 0;
            //criteria.ProductName = "Sportsbook";

            InvoiceModel imodel = new InvoiceModel();

            InvoiceModel invoice = JsonConvert.DeserializeObject<InvoiceModel>(invoiceModel);

            //criteria.LicenseeId = invoice.Licensee.LicenseeID;
            //criteria.BaseCurrency = invoice.Currency;
            //criteria.BaseCurrencyName = invoice.CurrencyName;
            //criteria.Currency = 0;

            DateTime FromDate = CastAide.AsDateTime(invoice.FromDate);
            DateTime ToDate = CastAide.AsDateTime(invoice.ToDate);

            dateRangeList = GetDateRange(FromDate, ToDate);

            List<string> htmlList = new List<string>();
            string[] arrayView = new string[] { "Template//BCH/_BCH-DataFees" };
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
                   

                    criteria.ToDate = dateRangeList[i].EndDate;
                    criteria.FromDate = dateRangeList[i].StartDate;
                    criteria.Product =  Converter.To<int>( ProductID[j]);
                    criteria.ProductName = Products[j];

                    var model = _reportService.GenerateReport(criteria);

                    model.LicenseeInfor.FromDate = criteria.FromDate;
                    model.LicenseeInfor.ToDate = criteria.ToDate;
                    if (model.Report.EntityList.Count > 0)
                    {
                        PageOrientationList.Add(WorkbookManager.PageOrientation.Landscape);
                        html = HtmlHelperExtensions.RenderViewToString(this, "_" + Products[j], model);
                        htmlList.Add(html);
                        invoice.HTMLReportList.Add(html);

                    }
                }

            }

            return View("_Review", invoice);
        }

        [HttpPost]
        public JsonResult SaveInvoice(InvoiceModel invoiceModel)
        {
            var insertResult = _invoiceService.SaveInvoice(invoiceModel);
            return Json(insertResult);
        }
    }
}