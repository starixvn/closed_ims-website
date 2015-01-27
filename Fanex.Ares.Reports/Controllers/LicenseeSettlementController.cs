namespace Fanex.Ares.Report.Controllers
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Web.Hosting;
    using System.Web.Mvc;
    using Fanex.Ares.Core;
    using Fanex.Ares.Core.Criteria;
    using Fanex.Ares.Core.DataDomain;
    using Fanex.Ares.Core.Export;
    using Fanex.Ares.Core.Extensions;
    using Fanex.Ares.Core.Utils;
    using Fanex.Ares.Core.Web.Controllers;
    using Fanex.Ares.Core.Web.Security;
    using Fanex.Ares.Report.Criteria.LicenseeSettlement;
    using Fanex.Ares.Report.Export.Excel;
    using Fanex.Ares.Report.Export.Pdf;
    using Fanex.Ares.Report.Services.Interfaces;
    using Fanex.Ares.Report.ViewModels.LicenseeSettlement;
    using Fanex.Common.Extensions;
    using Fanex.NAP.Core.Web.Attribute;
    using NPOI.HSSF.UserModel;

    /// <summary>
    /// Licensee Settlement Controller.
    /// </summary>
    [NapSuperAuth]
    public class LicenseeSettlementController : BaseController
    {
        #region Fields

        private static readonly string PdfCssFilePath = HostingEnvironment.MapPath(@"~\Areas\Report\Views\LicenseeSettlement\Styles\LicenseeSettlementPDF.css");

        private string _settlementView = string.Empty;

        private ILicenseeSettlementService _licenseeSettlementService;

        #endregion

        #region Constructor

        public LicenseeSettlementController()
        {
            _licenseeSettlementService = new LicenseeSettlementService();
        }

        public LicenseeSettlementController(ILicenseeSettlementService licenseeSettlementService)
        {
            _licenseeSettlementService = licenseeSettlementService ?? new LicenseeSettlementService();
        }

        #endregion

        /// <summary>
        /// The index page.
        /// </summary>
        /// <returns>Return view.</returns>
        [AuthenticateAction(AuthenticateAction.AuthenticationType.Read, "Index")]
        public ActionResult Index()
        {
            var model = new ReportLicenseeSettlementCriteria();

            model.Year = DateTime.Now.Year;

            ViewBag.ListLicenseeEx = _licenseeSettlementService.AllLicenseesExension;
            ViewBag.ListProduct = _licenseeSettlementService.GetProductsByLicensee(new LicenseeProductSelCriteria() { LicenseeId = 0 });
            ViewBag.ListCurrency = _licenseeSettlementService.GetCurrencyByLicensee(new LicenseeCurrencySelCriteria() { LicenseeId = 0 }).ToList<SelectListItem>();
            ViewBag.ListExchangeRate = _licenseeSettlementService.GetBaseCurrency(new IMS_BaseCurrencySel());

            return View(model);
        }

        /// <summary>
        /// Get Settlement report.
        /// </summary>
        /// <param name="criteria">Report criteria.</param>
        /// <returns>Report result.</returns>
        public PartialViewResult GetReport(ReportLicenseeSettlementCriteria criteria)
        {
            var model = _licenseeSettlementService.GenerateReport(criteria);

            model.LicenseeInfor.FromDate = criteria.FromDate;
            model.LicenseeInfor.ToDate = criteria.ToDate;
            _settlementView = "_" + criteria.ProductName;

            SessionExtensions.AddSession(System.Web.HttpContext.Current.Session, CacheServiceKey.SessionKeyLicenseeSettlementMonth, criteria.Month);
            SessionExtensions.AddSession(System.Web.HttpContext.Current.Session, CacheServiceKey.SessionKeyLicenseeSettlementYear, criteria.Year);
            SessionExtensions.AddSession(System.Web.HttpContext.Current.Session, CacheServiceKey.SessionKeyLicenseeSettlementViewName, _settlementView);

            return PartialView(_settlementView, model);
        }

        /// <summary>
        /// Export document.
        /// </summary>
        /// <param name="productId">Product Identify.</param>
        /// <param name="documentType">Document Type.</param>
        /// <returns>Export result.</returns>
        public ActionResult ExportDocument(int productId, string documentType)
        {
            var model = (LicenseeSettlementModel)SessionExtensions.GetSession(System.Web.HttpContext.Current.Session, CacheServiceKey.SessionKeyLicenseeSettlementReport);
            var fileName = string.Empty;

            if (documentType == DocumentType.Pdf)
            {
                var reportTitle = GetReportTitle(model.LicenseeInfor.LicenseeName, model.LicenseeInfor.FromDate, model.LicenseeInfor.ToDate);
                var streamContent = GetStreamContent(model);

                fileName = reportTitle.Replace(" ", ".") + "." + documentType;

                if (streamContent != null)
                {
                    return File(streamContent, @"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml", fileName);
                }
            }
            else
            {
                var excel = ExportExcel(model, productId);
                fileName = excel.ReportTitle.Replace(' ', '.');

                return new NPOIResult((HSSFWorkbook)excel.Workbook, fileName);
            }

            return new EmptyResult();
        }

        /// <summary>
        /// Get Product By LicenseeID.
        /// </summary>
        /// <param name="licenseeID">Licensee id.</param>
        /// <returns>Product result.</returns>
        public JsonResult GetProductByLicenseeID(int licenseeID)
        {
            return Json(_licenseeSettlementService.GetProductsByLicensee(new LicenseeProductSelCriteria() { LicenseeId = licenseeID }));
        }

        /// <summary>
        /// Get Currency By LicenseeID.
        /// </summary>
        /// <param name="licenseeID">Licensee id.</param>
        /// <returns>Currency result.</returns>
        public JsonResult GetCurrencyByLicenseeID(int licenseeID)
        {
            return Json(_licenseeSettlementService.GetCurrencyByLicensee(new LicenseeCurrencySelCriteria() { LicenseeId = licenseeID }));
        }

        #region Utilities

        private static string GetReportTitle(string licenseeName, DateTime fromDate, DateTime toDate)
        {
            return string.Format(
                CultureInfo.CurrentCulture,
                "Settlement Report for {0} from {1} to {2}",
                licenseeName,
                fromDate.ToAresDayMonthYear(),
                toDate.ToAresDayMonthYear());
        }

        /// <summary>
        /// Get Stream Content.
        /// </summary>
        /// <param name="model">Licensee Settlement Model.</param>
        /// <returns>Memory Stream.</returns>
        private byte[] GetStreamContent(LicenseeSettlementModel model)
        {
            _settlementView = (string)SessionExtensions.GetSession(System.Web.HttpContext.Current.Session, CacheServiceKey.SessionKeyLicenseeSettlementViewName);

            var htmlString = HtmlHelperExtensions.RenderViewToString(this, _settlementView, model, false);
            var pdfRender = new PdfRender(PdfCssFilePath, htmlString);

            return pdfRender.ContentRender();
        }

        /// <summary>
        /// Export Excel.
        /// </summary>
        /// <param name="model">Licensee Settlement Model.</param>
        /// <param name="productId">Product Identify.</param>
        /// <returns>Excel Base.</returns>
        private ExcelBase ExportExcel(LicenseeSettlementModel model, int productId)
        {
            var templatePath = GetExcelTemplatePath(productId);
            ExcelBase excel = null;

            switch (productId)
            {
                case Products.Sportsbook:
                case Products.NumberGame:
                    excel = new Sportsbook(templatePath, model);
                    break;
                case Products.BA:
                    excel = new BetAggregator(templatePath, model);
                    break;
                default:
                    excel = new Sportsbook(templatePath, model);
                    break;
            }

            excel.ReportTitle = GetReportTitle(model.LicenseeInfor.LicenseeName, model.LicenseeInfor.FromDate, model.LicenseeInfor.ToDate);
            excel.Export();

            return excel;
        }

        /// <summary>
        /// Get Excel Template Path.
        /// </summary>
        /// <param name="productId">Product Identify.</param>
        /// <returns>Template Path.</returns>
        private string GetExcelTemplatePath(int productId)
        {
            var templatePath = string.Empty;

            switch (productId)
            {
                case Products.Sportsbook:
                case Products.NumberGame:
                    templatePath = TemplatePath.LicenseeBillingForSportsBook;
                    break;
                case Products.BA:
                    templatePath = TemplatePath.LicenseeBillingForBA;
                    break;
                default:
                    templatePath = TemplatePath.LicenseeBillingForSportsBook;
                    break;
            }

            return Path.Combine(ExcelTemplateRootPath, templatePath);
        }

        #endregion
    }
}