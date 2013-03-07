// -----------------------------------------------------------------------
// <copyright file="ISingleTickketService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//   <change who="Robert.Tran" date="2013.2.05">Created</change>
// </history>

namespace Fanex.TicketControl.SportBook.Services.Interface
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.TicketControl.Core.Criteria;
    using Fanex.TicketControl.Core.Models;
    using Fanex.TicketControl.SportBook.Criteria;
    using Fanex.TicketControl.SportBook.ViewModels.ParlayTicket;


    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public interface IVoidParlayTicketService
    {
        ParlayTicketModel GetParlayTicket(GetParlayTicketCriteria criteria);

        ActionResult CheckMatchIDwithRefno(CheckMatchIdWithRefNoCriteria criteria);

        IList<ActionResult> VoidParlayTicket(VoidParlayTicketCritetia criteria);
    }
}
