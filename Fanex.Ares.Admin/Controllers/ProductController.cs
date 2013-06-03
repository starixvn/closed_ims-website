// -----------------------------------------------------------------------
// <copyright file="ProductController.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System.Web;
using System.Web.Mvc;
using System.Diagnostics.CodeAnalysis;
using Fanex.Ares.Admin.Criteria;
using Fanex.Ares.Admin.Services.Implement;
using Fanex.Ares.Admin.Services.Interface;
using Fanex.Ares.Core.Web;
using Fanex.NAP.Core.Web.Attribute;
using Fanex.NAP.Core.Business;
using Fanex.Ares.Core.Web.Security;
using Fanex.Ares.Admin.Models.Party;
using Fanex.Ares.Admin.Services.Party.Implement;
using Fanex.Ares.Admin.Services.Party.Interface;
using Fanex.Ares.Admin.Common;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using Fanex.Ares.Core.Web.Controllers;
using System.IO;
using System;

namespace Fanex.Ares.Admin.Controllers
{

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    [NapSuperAuth]
    public class ProductController : BaseController
    {
        private IProductService _productService;

        public ProductController()
        {
            _productService = new ProductService();
        }

        [HttpGet]
        [AuthenticateAction(AuthenticateAction.AuthenticationType.Read, "Index")]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetAllProducts()
        {
            return Json(_productService.GetAllProducts(new IMS_ProductInfoSel()));
        }

        public PartialViewResult _AddNew()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult CreateNewProduct(IMS_ProductIns criteria)
        {
            var rs = new Hashtable();
            rs = _productService.ExecuteCommand(criteria);
            TempData["Result"] = rs;
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult UpdateProduct(IMS_ProductnfoUpd criteria)
        {
            var rs = new Hashtable();
            rs = _productService.ExecuteCommand(criteria);

            TempData["Result"] = rs;
            return RedirectToAction("Index");
        }
    }
}