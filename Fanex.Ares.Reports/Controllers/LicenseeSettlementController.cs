namespace Fanex.Ares.Report.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Hosting;
    using System.Web.Mvc;
    using Fanex.Ares.Core.Criteria;
    using Fanex.Ares.Core.DataDomain;
    using Fanex.Ares.Core.Extensions;
    using Fanex.Ares.Core.Web.Controllers;
    using Fanex.Ares.Core.Web.Security;
    using Fanex.Ares.Report.Criteria.LicenseeSettlement;
    using Fanex.Ares.Report.Export.Pdf;
    using Fanex.Ares.Report.Services.Interfaces;
    using Fanex.Ares.Report.ViewModels.LicenseeSettlement;
    using Fanex.Common.Extensions;
    using Fanex.NAP.Core.Web.Attribute;

    /// <summary>
    /// Licensee Settlement Controller.
    /// </summary>
    [NapSuperAuth]
    public class LicenseeSettlementController : BaseController
    {
        #region Fields

        private static readonly string PdfCssFilePath = HostingEnvironment.MapPath(@"~\Content\Report\LicenseeSettlementPDF.css");

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
            ViewBag.ListExchangeRate = _licenseeSettlementService.GetBaseCurrency(new BaseCurrencyCriteria());

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

            SessionExtensions.AddSession(System.Web.HttpContext.Current.Session, CacheServiceKeys.SessionKeyLicenseeSettlementMonth, criteria.Month);
            SessionExtensions.AddSession(System.Web.HttpContext.Current.Session, CacheServiceKeys.SessionKeyLicenseeSettlementYear, criteria.Year);
            SessionExtensions.AddSession(System.Web.HttpContext.Current.Session, CacheServiceKeys.SessionKeyLicenseeSettlementViewName, _settlementView);

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
            var model = (LicenseeSettlementModel)SessionExtensions.GetSession(System.Web.HttpContext.Current.Session, CacheServiceKeys.SessionKeyLicenseeSettlementReport);
            var fileName = string.Empty;

            if (documentType == DocumentType.Pdf)
            {
                var reportTitle = _licenseeSettlementService.GetReportTitle(model.LicenseeInfor.LicenseeName, model.LicenseeInfor.FromDate, model.LicenseeInfor.ToDate);
                var streamContent = GetStreamContent(model);

                fileName = reportTitle.Replace(" ", ".") + "." + documentType;

                if (streamContent != null)
                {
                    return File(streamContent, @"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml", fileName);
                }
            }
            else
            {
                return _licenseeSettlementService.ExportExcel(model, productId, ExcelTemplateRootPath);
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

        /// <summary>
        /// Get Stream Content.
        /// </summary>
        /// <param name="model">Licensee Settlement Model.</param>
        /// <returns>Memory Stream.</returns>
        private byte[] GetStreamContent(LicenseeSettlementModel model)
        {
            _settlementView = (string)SessionExtensions.GetSession(System.Web.HttpContext.Current.Session, CacheServiceKeys.SessionKeyLicenseeSettlementViewName);

            var htmlString = HtmlHelperExtensions.RenderViewToString(this, _settlementView, model, false);
            var pdfRender = new PdfRender(PdfCssFilePath, htmlString);

            return pdfRender.ContentRender();
        }

        #endregion
    }
}