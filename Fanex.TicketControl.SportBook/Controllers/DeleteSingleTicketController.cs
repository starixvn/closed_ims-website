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
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Mvc;
    using Fanex.TicketControl.Core.Web.Controllers;
    using Fanex.TicketControl.SportBook.Services.Implement;
    using Fanex.TicketControl.SportBook.Services.Interface;
    using Fanex.TicketControl.Core.Criteria;

    

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class DeleteSingleTicketController : BaseController
    {

        IDeleteSingleTicketService _deleteSingleTicketService;

        public DeleteSingleTicketController()
        {
            _deleteSingleTicketService = new DeleteSingleTicketService();            
        }

        
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetMatchInfo(GetMatchInfoCriteria criteria)
        {
            return Json(_deleteSingleTicketService.GetMatchInfo(criteria));
        }
        
        
    }
}