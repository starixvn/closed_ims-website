namespace Fanex.Ares.Report.Controllers
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Web.Hosting;
    using System.Web.Mvc;
    using Fanex.Ares.Core;
    using Fanex.Ares.Core.Attributes;
    using Fanex.Ares.Core.Criteria;
    using Fanex.Ares.Core.DataDomain;
    using Fanex.Ares.Core.Export;
    using Fanex.Ares.Core.Extensions;
    using Fanex.Ares.Core.Utils;
    using Fanex.Ares.Core.Web.Controllers;
    using Fanex.Ares.Core.Web.Security;
    using Fanex.Ares.Report.Criteria.LicenseeSettlement;
    using Fanex.Ares.Report.Export.Excel;
    using Fanex.Ares.Report.Models;
    using Fanex.Ares.Report.Services.Interface;
    using Fanex.Ares.Report.ViewModels.LicenseeSettlement;
    using Fanex.Common.Extensions;
    using Fanex.Core.PdfHelper;
    using Fanex.NAP.Core.Web.Attribute;
    using Newtonsoft.Json;
    using NPOI.HSSF.UserModel;

    /// <summary>
    /// Licensee Settlement Controller.
    /// </summary>
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Licensee Settlement Controller.")]
    [NapSuperAuth]
    public class LicenseeSettlementController : BaseController
    {
        private static readonly string PdfCssFilePath = HostingEnvironment.MapPath(@"~\Areas\Report\Views\LicenseeSettlement\Styles\LicenseeSettlementPDF.css");

        private readonly StandardPdfRenderer _standardPdfRenderer;

        private ILicenseeSettlementService _service;

        #region View path

        private string _settlementView = string.Empty;

        #endregion

        public LicenseeSettlementController()
        {
            _service = new LicenseeSettlementService();

            this._standardPdfRenderer = new StandardPdfRenderer();
        }

        /// <summary>
        /// The index page.
        /// </summary>
        /// <returns>Return view.</returns>
        [AuthenticateAction(AuthenticateAction.AuthenticationType.Read, "Index")]
        public ActionResult Index()
        {
            IMS_Rpt_LicenseeSettlementCriteria model = new IMS_Rpt_LicenseeSettlementCriteria();
            model.Year = DateTime.Now.Year;
            ViewBag.ListLicenseeEx = _service.GetAllLicenseesEx();
            ViewBag.ListProduct = _service.GetProductsByLicensee(new IMS_LicenseeProductSelCriteria() { LicenseeId = 0 });
            ViewBag.ListCurrency = _service.GetCurrencyByLicensee(new IMS_LicenseeCurrencySelCriteria() { LicenseeId = 0 }).ToList<SelectListItem>();
            ViewBag.ListExchangeRate = _service.GetBaseCurrency(new IMS_BaseCurrencySel());
            return View(model);
        }

        /// <summary>
        /// Get Settlement report.
        /// </summary>
        /// <param name="criteria">Report criteria.</param>
        /// <returns>Report result.</returns>
        public PartialViewResult GetReport(IMS_Rpt_LicenseeSettlementCriteria criteria)
        {
            var model = _service.GenerateReport(criteria);
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
        public NPOIResult ExportDocument(int productId, string documentType)
        {
            /*var model = new LicenseeSettlementModel();
            List<string> htmlList = new List<string>();
            List<WorkbookManager.PageOrientation> pageOrientationList = new List<WorkbookManager.PageOrientation>();
            model = (LicenseeSettlementModel)SessionExtensions.GetSession(System.Web.HttpContext.Current.Session, CacheServiceKey.SessionKeyLicenseeSettlementReport);
            _settlementView = (string)SessionExtensions.GetSession(System.Web.HttpContext.Current.Session, CacheServiceKey.SessionKeyLicenseeSettlementViewName);
            string html = HtmlHelperExtensions.RenderViewToString(this, _settlementView, model, false);
            htmlList.Add(html);
            pageOrientationList.Add(WorkbookManager.PageOrientation.Landscape);
            var excelCssFilePath = new List<string>();
            excelCssFilePath.Add(HostingEnvironment.MapPath(@"~\Areas\Report\Views\LicenseeSettlement\Styles\LicenseeSettlement.css"));
            List<bool> isAutoFitList = new List<bool>();
            isAutoFitList.Add(true);

            string mainPagePath = "_Excel";
            excelCssFilePath.Add(HostingEnvironment.MapPath(@"~\Areas\Report\Views\LicenseeSettlement\Styles\LicenseeSettlement.css"));
            pageOrientationList.Insert(0, WorkbookManager.PageOrientation.Portrait);
            string mainPage = HtmlHelperExtensions.RenderViewToString(this, mainPagePath, model, true);
            htmlList.Insert(0, mainPage);
            isAutoFitList.Insert(0, false);

            var workbook = new WorkbookManager();

            var ms = workbook.CreateDocument(htmlList, excelCssFilePath, documentType, pageOrientationList, isAutoFitList, model.LicenseeInfor.LicenseeName);
            if (ms != null)
            {
                // return the filestream
                // Rewind the memory stream to the beginning
                ms.Seek(0, SeekOrigin.Begin);

                string fileName = _service.GetFileName() + "." + documentType.ToString().ToLower();

                return File(ms, @"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml", fileName);
            }*/

            var model = (LicenseeSettlementModel)SessionExtensions.GetSession(System.Web.HttpContext.Current.Session, CacheServiceKey.SessionKeyLicenseeSettlementReport);
            var excel = GetReport(model, productId);

            var fileName = excel.ReportTitle.Replace(' ', '.');

            return new NPOIResult((HSSFWorkbook)excel.Workbook, fileName);
        }

        /// <summary>
        /// Get Product By LicenseeID.
        /// </summary>
        /// <param name="licenseeID">Licensee id.</param>
        /// <returns>Product result.</returns>
        public JsonResult GetProductByLicenseeID(int licenseeID)
        {
            return Json(_service.GetProductsByLicensee(new IMS_LicenseeProductSelCriteria() { LicenseeId = licenseeID }));
        }

        /// <summary>
        /// Get Currency By LicenseeID.
        /// </summary>
        /// <param name="licenseeID">Licensee id.</param>
        /// <returns>Currency result.</returns>
        public JsonResult GetCurrencyByLicenseeID(int licenseeID)
        {
            return Json(_service.GetCurrencyByLicensee(new IMS_LicenseeCurrencySelCriteria() { LicenseeId = licenseeID }));
        }

        /// <summary>
        /// Print screen.
        /// </summary>
        /// <returns>Return result.</returns>
        public ActionResult PrintScreen()
        {
            var model = (LicenseeSettlementModel)SessionExtensions.GetSession(System.Web.HttpContext.Current.Session, CacheServiceKey.SessionKeyLicenseeSettlementReport);
            return View("PrintReport", model);
        }

        /// <summary>
        /// Export Document.
        /// </summary>
        /// <param name="documentType1">Document type.</param>
        /// <returns>Return result.</returns>
        public ActionResult ExportDocument1(string documentType1)
        {
            var student = JsonConvert.DeserializeObject<Product>(documentType1);
            return new EmptyResult();
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
        /// Get Report.
        /// </summary>
        /// <param name="model">Licensee Settlement Model.</param>
        /// <param name="productId">Product Identify.</param>
        /// <returns>Excel Base.</returns>
        private ExcelBase GetReport(LicenseeSettlementModel model, int productId)
        {
            var templatePath = GetReportTemplatePath(productId);
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
        /// Get Report Template Path.
        /// </summary>
        /// <param name="productId">Product Identify.</param>
        /// <returns>Template Path.</returns>
        private string GetReportTemplatePath(int productId)
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