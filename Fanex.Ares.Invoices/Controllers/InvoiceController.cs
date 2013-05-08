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
using Fanex.Ares.Core.Utils;
using Fanex.Ares.Core.Web.Controllers;
using Fanex.Common.Extensions;
using Fanex.Core.PdfHelper;
using Fanex.Ares.Core.Criteria;

namespace Fanex.Ares.Report.Controllers
{
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class InvoiceController : BaseController
    {
        //ILicenseeSettlementService _service;
        //private readonly HtmlViewRenderer htmlViewRenderer;
        //private readonly StandardPdfRenderer standardPdfRenderer;

        //private static readonly string PdfCssFilePath = HostingEnvironment.MapPath(@"~\Areas\Report\Views\LicenseeSettlement\Styles\LicenseeSettlementPDF.css");

        //#region View path

        //protected string SettlementView = string.Empty;

        //#endregion

        public InvoiceController()
        {
            //_service = new LicenseeSettlementService();
            //this.htmlViewRenderer = new HtmlViewRenderer();
            //this.standardPdfRenderer = new StandardPdfRenderer();
        }

        public ActionResult Index()
        {
        //    IMS_Rpt_LicenseeSettlementCriteria model = new IMS_Rpt_LicenseeSettlementCriteria();
        //    model.Year = DateTime.Now.Year;
        //    ViewBag.ListLicensee = _service.GetAllLicensees();
        //    var licenseeID = int.Parse(((List<SelectListItem>)ViewBag.ListLicensee)[0].Value);
        //    ViewBag.ListProduct = _service.GetProductsByLicensee(new IMS_LicenseeProductSelCriteria() { LicenseeId = licenseeID });
        //    var listCurrency = _service.GetCurrencyByLicensee(new IMS_LicenseeCurrencySelCriteria() { LicenseeId = licenseeID }).ToList<SelectListItem>();
        //    listCurrency.Add(new SelectListItem() { Selected = true, Text = "All", Value = "0" });
        //    ViewBag.ListCurrency = listCurrency;
        //    ViewBag.ListExchangeRate = _service.GetBaseCurrency(new IMS_BaseCurrencySel());

            return View();
        }

    }
}