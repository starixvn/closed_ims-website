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
    using Fanex.TicketControl.Core.Web.Controllers;

    

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class DeleteSingleTicketController : BaseController
    {
        

        public DeleteSingleTicketController()
        {
            
        }

        
        public ActionResult Index()
        {
            return View();
        }
               

        
        
    }
}