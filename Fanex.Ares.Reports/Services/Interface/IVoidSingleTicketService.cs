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
using Fanex.Ares.Core.Criteria;
using Fanex.Ares.Core.Models;
using Fanex.Ares.Report.Criteria;
using Fanex.Ares.Report.ViewModels.SingleTicket;

namespace Fanex.Ares.Report.Services.Interface
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public interface IVoidSingleTicketService
    {
        SingleTicketModel GetSingleTicket(GetSingleTicketCriteria criteria);

        ActionResult CheckMatchIDwithRefno(CheckMatchIdWithRefNoCriteria criteria);

        IList<ActionResult> VoidSingleTicket(VoidSingleTicketCritetia criteria);
    }
}
