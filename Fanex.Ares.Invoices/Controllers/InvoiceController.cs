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
            ViewBag.ListOperator = _invoiceService.GetOperatorList(new IMS_OperatorSelCriteria() { PartyID = 0});           
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


        private static IEnumerable<DateTime> ByMonths(DateTime startDate, DateTime endDate)
        {
            DateTime cur = startDate;

            for (int i = 0; cur <= endDate; cur = startDate.AddMonths(++i))
            {
                yield return cur;
            }
        }


        public ActionResult ExportDocument( string invoiceModel)
        {
            var invoice = JsonConvert.DeserializeObject<InvoiceModel>(invoiceModel);

            IMS_Rpt_LicenseeSettlementCriteria criteria = new IMS_Rpt_LicenseeSettlementCriteria();
            criteria.LicenseeId = 1;
            criteria.BaseCurrency = 6;
            criteria.BaseCurrencyName = "EUR";
            criteria.Currency = 0;
            criteria.FromDate = new DateTime(2013, 5, 01);
            criteria.ToDate = new DateTime(2013, 5, 31);
            criteria.Product = 0;
            criteria.ProductName = "Sportsbook";

            var model = _reportService.GenerateReport(criteria);

            List<string> htmlList = new List<string>();
            string[] arrayView = new string[] { "Template/BCH/_BCH-DataFees" };
            List<WorkbookManager.PageOrientation> PageOrientationList = new List<WorkbookManager.PageOrientation>();

            PageOrientationList.Add(WorkbookManager.PageOrientation.Portrait);
            //string[] arrayView = new string[] { "_InvoiceSportsBook", "_Number Game", "_Sportsbook", "_BA" };
            //List<WorkbookManager.PageOrientation> PageOrientationList = new List<WorkbookManager.PageOrientation>();

            //PageOrientationList.Add(WorkbookManager.PageOrientation.Portrait);
            //PageOrientationList.Add(WorkbookManager.PageOrientation.Landscape);
            //PageOrientationList.Add(WorkbookManager.PageOrientation.Landscape);
            //PageOrientationList.Add(WorkbookManager.PageOrientation.Landscape);
            foreach (string view in arrayView)
            {
                string html = HtmlHelperExtensions.RenderViewToString(this, view, model);
                htmlList.Add(html);

                //invoiceModel.HTMLReportList.Add(html);
            }

            //SessionExtensions.AddSession(System.Web.HttpContext.Current.Session,"InvoiceHTML", invoiceModel);

            var excelCssFilePath = new List<string>();
            excelCssFilePath.Add(HostingEnvironment.MapPath(@"~\Areas\Invoice\Views\Invoice\Styles\InvoicePDF.css"));

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
            criteria.Product = 0;
            criteria.ProductName = "Sportsbook";

            InvoiceModel imodel = new InvoiceModel();

            InvoiceModel invoice = JsonConvert.DeserializeObject<InvoiceModel>(invoiceModel);


            DateTime  FromDate= CastAide.AsDateTime(invoice.FromDate);
            DateTime ToDate = CastAide.AsDateTime(invoice.ToDate);

            int startMonth = FromDate.Month;
            int endMonth = ToDate.Month;

            int count = endMonth - startMonth + 1;
            for (int i = 0; i <count; i++)
            {
                DateRange range = new DateRange();
                DateTime begin = new DateTime();
                DateTime end = new DateTime();
                if (i == 0)
                {
                    begin = FromDate;
                    DateTime firstOfNextMonth = new DateTime(FromDate.Year, FromDate.Month, 1).AddMonths(1);
                    //DateTime lastOfThisMonth = firstOfNextMonth.AddDays(-1);
                    end = firstOfNextMonth.AddDays(-1);
                }
                else if (i == count)
                {
                    begin = new DateTime(FromDate.Year, ToDate.Month, 1);
                    //DateTime firstOfNextMonth = new DateTime(FromDate.Year + i - 1, FromDate.Month + i - 1, 1).AddMonths(1);
                    //DateTime lastOfThisMonth = firstOfNextMonth.AddDays(-1);
                    end = ToDate;
                }
                else
                {
                    begin = new DateTime(FromDate.Year , FromDate.Month + i , 1);
                    DateTime firstOfNextMonth = new DateTime(FromDate.Year, FromDate.Month + i, 1).AddMonths(1);
                    //DateTime lastOfThisMonth = firstOfNextMonth.AddDays(-1);
                    end = firstOfNextMonth.AddDays(-1);

                   
                }

                range.StartDate = begin;
                range.EndDate = end;
                dateRangeList.Add(range);
            }

            List<string> htmlList = new List<string>();
            string[] arrayView = new string[] { "Template//BCH/_BCH-DataFees" };
            List<WorkbookManager.PageOrientation> PageOrientationList = new List<WorkbookManager.PageOrientation>();
            PageOrientationList.Add(WorkbookManager.PageOrientation.Portrait);
            string html = HtmlHelperExtensions.RenderViewToString(this, arrayView[0], invoice);
            htmlList.Add(html);

            invoice.HTMLReportList.Add(html);
            for (int i = 0; i < dateRangeList.Count; i++)
            {
                criteria.ToDate = dateRangeList[i].EndDate;
                criteria.FromDate = dateRangeList[i].StartDate;

                var model = _reportService.GenerateReport(criteria);


                if (model.Report.EntityList.Count > 0)
                {


                    PageOrientationList.Add(WorkbookManager.PageOrientation.Landscape);
                    //string[] arrayView = new string[] { "_InvoiceSportsBook", "_Number Game", "_Sportsbook", "_BA" };
                    //List<WorkbookManager.PageOrientation> PageOrientationList = new List<WorkbookManager.PageOrientation>();

                    //PageOrientationList.Add(WorkbookManager.PageOrientation.Portrait);
                    //PageOrientationList.Add(WorkbookManager.PageOrientation.Landscape);
                    //PageOrientationList.Add(WorkbookManager.PageOrientation.Landscape);
                    //PageOrientationList.Add(WorkbookManager.PageOrientation.Landscape);

                    html = HtmlHelperExtensions.RenderViewToString(this, "_Sportsbook", model);
                    htmlList.Add(html);

                    invoice.HTMLReportList.Add(html);

                }
            }


            //List<string> htmlList = new List<string>();
            //string[] arrayView = new string[] { "Template//BCH/_BCH-DataFees" };
            //List<WorkbookManager.PageOrientation> PageOrientationList = new List<WorkbookManager.PageOrientation>();

            //PageOrientationList.Add(WorkbookManager.PageOrientation.Portrait);
            ////string[] arrayView = new string[] { "_InvoiceSportsBook", "_Number Game", "_Sportsbook", "_BA" };
            ////List<WorkbookManager.PageOrientation> PageOrientationList = new List<WorkbookManager.PageOrientation>();

            ////PageOrientationList.Add(WorkbookManager.PageOrientation.Portrait);
            ////PageOrientationList.Add(WorkbookManager.PageOrientation.Landscape);
            ////PageOrientationList.Add(WorkbookManager.PageOrientation.Landscape);
            ////PageOrientationList.Add(WorkbookManager.PageOrientation.Landscape);
            //foreach (string view in arrayView)
            //{
            //    string html = HtmlHelperExtensions.RenderViewToString(this, view, model);
            //    htmlList.Add(html);

            //    imodel.HTMLReportList.Add(html);
            //}
          
            //InvoiceModel invoiceModel = new InvoiceModel();

            //invoiceModel = (InvoiceModel)SessionExtensions.GetSession(System.Web.HttpContext.Current.Session, "InvoiceHTML");
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