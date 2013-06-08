// --------------------------------------------------------------------------
// <copyright file="LicenseeSettlementController.cs" company="Nexcel Solutions Vietnam">
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
using Fanex.Ares.Core.Models;
using Fanex.Ares.Core.Utils;
using Fanex.Ares.Core.Web.Controllers;
using Fanex.Ares.Core.Web.Security;
using Fanex.Ares.Report.Criteria.LicenseeSettlement;

using Fanex.Ares.Report.Services.Interface;
using Fanex.Ares.Report.ViewModels.LicenseeSettlement;
using Fanex.Common.Extensions;
using Fanex.Core.PdfHelper;
using Fanex.NAP.Core.Web.Attribute;
using Fanex.Workbooks;
using Newtonsoft.Json;

namespace Fanex.Ares.Report.Controllers
{
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    [NapSuperAuth]
    public class LicenseeSettlementController : BaseController
    {
        ILicenseeSettlementService _service;

        private readonly StandardPdfRenderer standardPdfRenderer;

        private static readonly string PdfCssFilePath = HostingEnvironment.MapPath(@"~\Areas\Report\Views\LicenseeSettlement\Styles\LicenseeSettlementPDF.css");

        #region View path

        protected string SettlementView = string.Empty;

        #endregion

        public LicenseeSettlementController()
        {
            _service = new LicenseeSettlementService();

            this.standardPdfRenderer = new StandardPdfRenderer();
        }

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

        public PartialViewResult GetReport(IMS_Rpt_LicenseeSettlementCriteria criteria)
        {
            var model = _service.GenerateReport(criteria);
            model.LicenseeInfor.FromDate = criteria.FromDate;
            model.LicenseeInfor.ToDate = criteria.ToDate;
            SettlementView = "_" + criteria.ProductName;
            SessionExtensions.AddSession(System.Web.HttpContext.Current.Session, CacheServiceKey.SessionKeyLicenseeSettlementMonth, criteria.Month);
            SessionExtensions.AddSession(System.Web.HttpContext.Current.Session, CacheServiceKey.SessionKeyLicenseeSettlementYear, criteria.Year);
            SessionExtensions.AddSession(System.Web.HttpContext.Current.Session, CacheServiceKey.SessionKeyLicenseeSettlementViewName, SettlementView);
            return PartialView(SettlementView, model);
        }

        public ActionResult ExportDocument(WorkbookManager.DocumentType documentType)
        {
            var model = new LicenseeSettlementModel();
            List<string> htmlList = new List<string>();
            List<WorkbookManager.PageOrientation> PageOrientationList = new List<WorkbookManager.PageOrientation>();
            model = (LicenseeSettlementModel)SessionExtensions.GetSession(System.Web.HttpContext.Current.Session, CacheServiceKey.SessionKeyLicenseeSettlementReport);
            SettlementView = (string)SessionExtensions.GetSession(System.Web.HttpContext.Current.Session, CacheServiceKey.SessionKeyLicenseeSettlementViewName);
            string html = HtmlHelperExtensions.RenderViewToString(this, SettlementView, model);
            htmlList.Add(html);
            PageOrientationList.Add(WorkbookManager.PageOrientation.Landscape);
            var excelCssFilePath = new List<string>();
            excelCssFilePath.Add(HostingEnvironment.MapPath(@"~\Areas\Report\Views\LicenseeSettlement\Styles\LicenseeSettlement.css"));

            var workbook = new WorkbookManager();

            var ms = workbook.CreateDocument(htmlList, excelCssFilePath, documentType, PageOrientationList);
            if (ms != null)
            {
                // return the filestream
                // Rewind the memory stream to the beginning
                ms.Seek(0, SeekOrigin.Begin);

                string fileName = _service.GetFileName() + "." + documentType.ToString().ToLower();

                return File(ms, @"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml", fileName);
            }

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

        public ActionResult PrintScreen()
        {
            var model = (LicenseeSettlementModel)SessionExtensions.GetSession(System.Web.HttpContext.Current.Session, CacheServiceKey.SessionKeyLicenseeSettlementReport);
            return View("PrintReport", model);
        }


        public ActionResult ExportDocument1(string documentType1)
        {
            
            var student = JsonConvert.DeserializeObject<Product>(documentType1);
            return new EmptyResult();


        }
    }

    
}