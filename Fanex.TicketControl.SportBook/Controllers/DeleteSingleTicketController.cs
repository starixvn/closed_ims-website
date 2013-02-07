// --------------------------------------------------------------------------
// <copyright file="DeleteSingleBetController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.1.25">Created</change>
// </history>


namespace Fanex.TicketControl.SportBook.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.NAP.Core.Web.Attribute;
    using Fanex.NAP.Core.Business;

    

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    [NapSuperAuth]
    public class DeleteSingleTicketController : Controller
    {
        

        public DeleteSingleTicketController()
        {
            
        }

        [HttpGet]
        [NapAuth(FunctionKey = NAPSpecialDeleteFunctionKeys.SpecialDeleteSportBook, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            return View();
        }
               

        
        
    }
}