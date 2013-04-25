// --------------------------------------------------------------------------
// <copyright file="LicenseeSettlementController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="">Created</change>
// </history>


using System.Diagnostics.CodeAnalysis;
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

namespace Fanex.Ares.Report.Controllers
{
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class LicenseeSettlementController : BaseController
    {
        ILicenseeSettlementService _service;
        private readonly HtmlViewRenderer htmlViewRenderer;
        private readonly StandardPdfRenderer standardPdfRenderer;
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
            var model = GenerateReport(criteria);
            ViewBag.BaseCurrency = criteria.BaseCurrency == 3 ? "USD" : "EUR";
            return PartialView("LicenseeSettlement", model);
        }

        protected ActionResult ViewPdf(string pageTitle, string viewName, object model, int numericRows)
        {
            // Render the view html to a string.
            string htmlText = this.htmlViewRenderer.RenderViewToString(this, viewName, model);

            // Let the html be rendered into a PDF document through iTextSharp.
            byte[] buffer = standardPdfRenderer.Render(htmlText, pageTitle, numericRows);

            // Return the PDF as a binary stream to the client.
            return new BinaryContentResult(buffer, "application/pdf");
        }

        public ActionResult PrintInvoice(IMS_Rpt_LicenseeSettlementCriteria criteria)
        {
            //var model = GenerateReport(criteria);
            var model = new LicenseeSettlementModel();
            //FillImageUrl(customerList, "report.jpg");
            return this.ViewPdf("", "LicenseeSettlement", model, model.Report.EntityList.Count + 1);
        }

        public LicenseeSettlementModel GenerateReport(IMS_Rpt_LicenseeSettlementCriteria criteria) 
        {
            // Licensee Information
            var listLicensee = _service.GetAllLicensees();
            var listProduct = _service.GetProductsByLicensee(new IMS_LicenseeProductSelCriteria() { LicenseeId = criteria.LicenseeId });
            var listCurrency = _service.GetCurrencyByLicensee(new IMS_LicenseeCurrencySelCriteria() { LicenseeId = criteria.LicenseeId });
            var model = new LicenseeSettlementModel();
            var licenseeInfoCriteria = new IMS_LicenseeBillingInfoSelCriteria();
            licenseeInfoCriteria.LicenseeId = criteria.LicenseeId;
            licenseeInfoCriteria.ProductId = criteria.Product;
            model.LicenseeInfor = _service.GetLicenseeInformation(licenseeInfoCriteria);
            model.LicenseeInfor.LicenseeID = criteria.LicenseeId;
            model.LicenseeInfor.LicenseeName = listLicensee.Where(m => m.Value == criteria.LicenseeId.ToString()).FirstOrDefault<SelectListItem>().Text;
            model.LicenseeInfor.ProductName = listProduct.Where(m => m.Value == criteria.Product.ToString()).FirstOrDefault<SelectListItem>().Text;
            //model.LicenseeInfor.SettlementDate = DateTime.Now;
            model.LicenseeInfor.SettlementDate = criteria.ToDate;

            // Licensee Report
            var reportCriteria = new IMS_Rpt_LicenseeSettlementCriteria();
            reportCriteria.LicenseeId = criteria.LicenseeId;
            reportCriteria.LicenseeName = model.LicenseeInfor.LicenseeName;
            reportCriteria.Product = criteria.Product;
            reportCriteria.Currency = criteria.Currency;
            reportCriteria.BaseCurrency = criteria.BaseCurrency;
            reportCriteria.FromDate = criteria.FromDate;
            reportCriteria.ToDate = criteria.ToDate;

            model.Report = _service.GetLicenseeReport(reportCriteria);

            // Exchange Rate
            var exchangeRateCriteria = new IMS_ExchangeRateBillingSelCriteria();
            model.ExchangeList = new List<ViewModels.ExchangeRate>();
            
            var exchangeRatecurrencies = string.Empty;
            foreach (ReportEntity item in model.Report.EntityList)
            {
                // Calculate membertotal = memberwinloss + membercomm
                item.MemberTotal = item.MemberWinloss + item.MemberComm;

                // Calculate licenseetotal = licenseewinloss + licenseecomm
                item.LicenseeTotal = item.LicenseeWL + item.LicenseeComm;

                if (criteria.Currency == 0) // All Currency
                {
                    if (!exchangeRatecurrencies.Contains(item.Currency.ToString()))
                    {
                        exchangeRatecurrencies += item.Currency.ToString() + ",";
                    }
                }
                else // Single currency
                {
                    exchangeRatecurrencies = item.Currency.ToString();
                }
            }

            exchangeRatecurrencies.Trim(',');

            exchangeRateCriteria.Currency = exchangeRatecurrencies;
            exchangeRateCriteria.ExchangeDate = criteria.ToDate;
            exchangeRateCriteria.BaseCurrencyId = criteria.BaseCurrency;
            exchangeRateCriteria.BaseCurrencyName = listCurrency.Where(m => m.Value == criteria.BaseCurrency.ToString()).FirstOrDefault<SelectListItem>().Text;

            var exchangeList = _service.GetLicenseeExchangeRate(exchangeRateCriteria).ToList();

            model.ExchangeList = (from Item1 in model.Report.EntityList
                                  join Item2 in exchangeList
                                  on Item1.CurrencyName equals Item2.CurrencyName
                                  select Item2)
                                  .ToList();

            return model;
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
    }
}