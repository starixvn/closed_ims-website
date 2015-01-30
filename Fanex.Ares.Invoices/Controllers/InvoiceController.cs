namespace Fanex.Ares.Invoice.Controllers
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Web.Hosting;
    using System.Web.Mvc;
    using DNAide.Core;
    using Fanex.Ares.Core.DataDomain;
    using Fanex.Ares.Core.Extensions;
    using Fanex.Ares.Core.Web;
    using Fanex.Ares.Core.Web.Controllers;
    using Fanex.Ares.Core.Web.Security;
    using Fanex.Ares.Invoice.Criteria.Invoice;
    using Fanex.Ares.Invoice.Export;
    using Fanex.Ares.Invoice.Models.Invoice;
    using Fanex.Ares.Invoice.Service.Imp;
    using Fanex.Ares.Invoice.Service.Interface;
    using Fanex.Ares.Report.Criteria.LicenseeSettlement;
    using Fanex.Ares.Report.Services.Interfaces;
    using Fanex.Ares.Report.ViewModels.LicenseeSettlement;
    using Fanex.Common;
    using Fanex.Common.Extensions;
    using Fanex.NAP.Core.Web.Attribute;

    /// <summary>
    /// Invoice Controller.
    /// </summary>
    [NapSuperAuth]
    public class InvoiceController : BaseController
    {
        #region Fields

        private readonly string _invoiceCSS = HostingEnvironment.MapPath(@"~\Content\Invoice\InvoicePDF.css");
        private readonly string _productCss = HostingEnvironment.MapPath(@"~\Content\Report\LicenseeSettlementPDF.css");
        private string _settlementView = string.Empty;
        private IInvoiceService _invoiceService;
        private ILicenseeSettlementService _licenseeSettlementService;

        #endregion

        #region Constructor

        public InvoiceController()
        {
            _invoiceService = new InvoiceService();
            _licenseeSettlementService = new LicenseeSettlementService();
        }

        public InvoiceController(IInvoiceService invoiceService, ILicenseeSettlementService licenseeSettlementService)
        {
            _invoiceService = invoiceService ?? new InvoiceService();
            _licenseeSettlementService = licenseeSettlementService ?? new LicenseeSettlementService();
        }

        #endregion

        #region Methods

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
            var licensees = new List<Licensee>();
            licensees = this._invoiceService.GeLicenseesList(new IMS_LicenseeByPartyCriteria() { PartyID = partyID }).ToList();
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
            var invoiceTypeList = new List<InvoiceType>();
            invoiceTypeList = this._invoiceService.GetInvoieType(new IMS_InvoiceTypesCriteria() { LicenseeId = licenseeID, PartyId = partyID }).ToList();
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
        public JsonResult SaveInvoice([ModelBinder(typeof(EmptyStringModelBinder))] InvoiceModel invoiceModel)
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
        public PartialViewResult GetReport(ReportLicenseeSettlementCriteria criteria)
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
        public ActionResult ExportDocument(bool isSignature)
        {
            var cssPathList = AssignCssPathList();
            var model = (InvoiceModel)SessionExtensions.GetSession(System.Web.HttpContext.Current.Session, "invoiceModelAfterSave");

            model.IsSignature = isSignature;

            var invoiceMainPageText = GetInvoiceMainPageText(model);
            var productPageList = GetProductPageList(model);

            model.HTMLReportList.Add(invoiceMainPageText);
            model.HTMLReportList.AddRange(productPageList);

            return ExportPdf(cssPathList, model.HTMLReportList);
        }

        /// <summary>
        /// Prints the screen.
        /// </summary>
        /// <param name="model">Invoice Model.</param>
        /// <returns>Action Result.</returns>
        public ActionResult PrintScreen(InvoiceModel model)
        {
            var invoiceMainPageText = GetInvoiceMainPageText(model);
            var productPageList = GetProductPageList(model);

            model.HTMLReportList.Add(invoiceMainPageText);
            model.HTMLReportList.AddRange(productPageList);

            return View("_Preview", model);
        }

        /// <summary>
        /// Details the specified invoice.
        /// </summary>
        /// <param name="model">The invoice.</param>
        /// <returns>Action Result.</returns>
        public ActionResult Detail(InvoiceModel model)
        {
            var productPageList = GetProductPageList(model);

            model.HTMLReportList.AddRange(productPageList);

            return View("_Preview", model);
        }

        /// <summary>
        /// Reviews the invoice.
        /// </summary>
        /// <returns>Action Result.</returns>
        public ActionResult ReviewInvoice()
        {
            return new EmptyResult();
        }

        #endregion

        /// <summary>
        /// Get Invoice Limited Row Number.
        /// </summary>
        /// <param name="partyName">Party Name.</param>
        /// <returns>Row Number.</returns>
        private static int GetInvoiceLimitedRowNumber(string partyName)
        {
            switch (partyName)
            {
                case PartyName.Bch:
                case PartyName.Cozni:
                case PartyName.Ldl:
                    return 5;
                case PartyName.Boping:
                    return 1;
                case PartyName.Lupine:
                    return 6;
                case PartyName.StarExpress:
                    return 6;
                default:
                    return 5;
            }
        }

        /// <summary>
        /// Re-fill Statement List.
        /// </summary>
        /// <param name="model">Invoice Model.</param>
        /// <param name="limitedRow">Limited Row.</param>
        private static void RefillStatementList(ref InvoiceModel model, int limitedRow)
        {
            var emptyStatementList = new List<StatementDetail>();
            var rowCount = 0;

            emptyStatementList.Add(new StatementDetail
            {
                DiscountItem = false,
                AdjustItem = 0
            });

            foreach (var statement in model.StatementList)
            {
                rowCount += statement.StatementDetailList.Count;
            }

            while (rowCount < 5)
            {
                model.StatementList.Add(new Statement
                {
                    StatementID = 100000,
                    StatementDetailList = emptyStatementList
                });

                rowCount++;
            }
        }

        /// <summary>
        /// Assign CSS Path List.
        /// </summary>
        /// <returns>List of String.</returns>
        private List<string> AssignCssPathList()
        {
            var cssPathList = new List<string>();
            var mainPageCss = _invoiceCSS;
            var productCss = _productCss;

            cssPathList.Add(mainPageCss);
            cssPathList.Add(productCss);

            return cssPathList;
        }

        /// <summary>
        /// Get Invoice Main Page Text.
        /// </summary>
        /// <param name="model">Invoice Model.</param>
        /// <returns>HTML Text.</returns>
        private string GetInvoiceMainPageText(InvoiceModel model)
        {
            var mainInvoicePagePath = string.Format(
                    CultureInfo.CurrentCulture,
                    "Templates/{0}/_{1}_{2}",
                    model.Operator.PartyName,
                    model.Operator.PartyName,
                    model.InvoiceType.InvoiceTypeName);

            var limitedRow = GetInvoiceLimitedRowNumber(model.Operator.PartyName);

            RefillStatementList(ref model, limitedRow);

            return HtmlHelperExtensions.RenderViewToString(this, mainInvoicePagePath, model, true);
        }

        /// <summary>
        /// Get Product Page List.
        /// </summary>
        /// <param name="model">Invoice Model.</param>
        /// <returns>List Of String.</returns>
        private List<string> GetProductPageList(InvoiceModel model)
        {
            var fromDate = CastAide.AsDateTime(model.FromDate);
            var toDate = CastAide.AsDateTime(model.ToDate);
            var productPageList = new List<string>();
            var chargeRateList = new List<ChargeRate>();
            var dateRangeList = _invoiceService.GetDateRange(fromDate, toDate).ToList();
            var criteria = new InvoiceReportLicenseeSettlementCriteria();
            var products = model.ProductsName.Split(',');
            var productIDs = model.Product.Split(',');

            criteria.LicenseeId = model.Licensee.LicenseeID;
            criteria.BaseCurrency = model.Currency;
            criteria.BaseCurrencyName = model.CurrencyName;
            criteria.Currency = 0;
            criteria.PartyId = model.Operator.PartyID;

            foreach (var dateRange in dateRangeList)
            {
                for (int productIndex = 0; productIndex < products.Count(); productIndex++)
                {
                    var productId = productIDs[productIndex];
                    var productName = products[productIndex];
                    var getChargeRateCriteria = new IMS_GetChargeRateCriteria
                    {
                        PartyID = model.Operator.PartyID,
                        LicenseeID = model.Licensee.LicenseeID,
                        Products = productId,
                        FromDate = dateRange.StartDate,
                        ToDate = dateRange.EndDate
                    };

                    chargeRateList = this._invoiceService.GetChargeRate(getChargeRateCriteria).ToList();

                    criteria.Product = Converter.To<int>(productId);
                    criteria.ProductName = productName;
                    criteria.InvoiceReport = true;

                    var renderedProductPages = RenderProductPages(criteria, dateRange, chargeRateList);

                    productPageList.AddRange(renderedProductPages);
                }
            }

            return productPageList;
        }

        /// <summary>
        /// Render Product Pages.
        /// </summary>
        /// <param name="criteria">Invoice Report Licensee Settlement Criteria.</param>
        /// <param name="dateRange">Date Range.</param>
        /// <param name="chargeRateList">Charge Rate List.</param>
        /// <returns>Rendered Product Pages.</returns>
        private List<string> RenderProductPages(InvoiceReportLicenseeSettlementCriteria criteria, DateRange dateRange, List<ChargeRate> chargeRateList)
        {
            var productPageList = new List<string>();

            if (chargeRateList.Count == 0)
            {
                criteria.ToDate = dateRange.EndDate;
                criteria.FromDate = dateRange.StartDate;
                productPageList.Add(GetProductPageText(criteria));
            }
            else
            {
                foreach (var chargeRate in chargeRateList)
                {
                    criteria.ToDate = chargeRate.EndDate;
                    criteria.FromDate = chargeRate.StartDate;
                    productPageList.Add(GetProductPageText(criteria));
                }
            }

            return productPageList;
        }

        /// <summary>
        /// Get Product Page Text.
        /// </summary>
        /// <param name="criteria">Invoice Report Licensee Settlement Criteria.</param>
        /// <returns>HTML Text.</returns>
        private string GetProductPageText(InvoiceReportLicenseeSettlementCriteria criteria)
        {
            var model = _licenseeSettlementService.GenerateReport(criteria);

            model.LicenseeInfor.FromDate = criteria.FromDate;
            model.LicenseeInfor.ToDate = criteria.ToDate;

            return HtmlHelperExtensions.RenderViewToString(this, "Products/_" + criteria.ProductName, model, false);
        }

        /// <summary>
        /// Export PDF.
        /// </summary>
        /// <param name="cssPathList">CSS Path List.</param>
        /// <param name="htmlTextList">HTML Text List.</param>
        /// <returns>Action Result.</returns>
        private ActionResult ExportPdf(List<string> cssPathList, List<string> htmlTextList)
        {
            var pdfRender = new PdfRender(cssPathList, htmlTextList);
            var streamContent = pdfRender.ContentRender();

            if (streamContent != null)
            {
                return File(streamContent, @"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml", "abc.pdf");
            }

            return new EmptyResult();
        }
    }
}