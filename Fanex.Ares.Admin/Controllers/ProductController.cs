// --------------------------------------------------------------------------
// <copyright file="ProductController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Phillip.Truong" date="2013.06.03">Created</change>
// </history>

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
        [AuthenticateAction(AuthenticateAction.AuthenticationType.Read, "Index")]
        public JsonResult GetAllProducts()
        {
            return Json(_productService.GetAllProducts(new IMS_ProductInfoSel()));
        }

        [AuthenticateAction(AuthenticateAction.AuthenticationType.Read, "Index")]
        public PartialViewResult _AddNew()
        {
            return PartialView();
        }

        [HttpPost]
        [AuthenticateAction(AuthenticateAction.AuthenticationType.Host, "Index")]
        public ActionResult CreateNewProduct(IMS_ProductIns criteria)
        {
            TempData["Result"] = _productService.ExecuteCommand(criteria);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [AuthenticateAction(AuthenticateAction.AuthenticationType.Host, "Index")]
        public ActionResult UpdateProduct(IMS_ProductnfoUpd criteria)
        {
            TempData["Result"] = _productService.ExecuteCommand(criteria);
            return RedirectToAction("Index");
        }
    }
}