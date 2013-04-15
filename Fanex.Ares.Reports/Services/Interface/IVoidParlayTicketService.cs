// -----------------------------------------------------------------------
// <copyright file="ISingleTickketService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//   <change who="Robert.Tran" date="2013.2.05">Created</change>
// </history>

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Fanex.Ares.Core.Models;
using Fanex.Ares.Report.Criteria;
using Fanex.Ares.Report.Entities.ParlayTicket;
using Fanex.Ares.Report.ViewModels.ParlayTicket;

namespace Fanex.Ares.Report.Services.Interface
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public interface IVoidParlayTicketService
    {
        ParlayTicketModel GetParlayTicket(GetParlayTicketCriteria criteria);

        List<SubTicket> GetParlaySubTicket(GetParlaySubTicketCriteria criteria);

        IList<ActionResult> VoidParlayTicket(VoidParlayTicketCritetia criteria);
    }
}
