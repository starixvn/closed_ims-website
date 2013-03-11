// -----------------------------------------------------------------------
// <copyright file="ISingleTickketService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//   <change who="Robert.Tran" date="2013.2.05">Created</change>
// </history>

namespace Fanex.TicketControl.Gaming.Services.Interface
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.TicketControl.Core.Criteria;
    using Fanex.TicketControl.Core.Models;
    using Fanex.TicketControl.Gaming.Criteria;
    using Fanex.TicketControl.Gaming.ViewModels;


    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public interface IVoidNumberGameTicketService
    {
        NumberGameTicketModel GetNumberGameTicket(GetNumberGameTicketCriteria criteria);

        //ActionResult CheckMatchIDwithRefno(CheckMatchIdWithRefNoCriteria criteria);

        //IList<ActionResult> VoidSingleTicket(VoidSingleTicketCritetia criteria);
    }
}
