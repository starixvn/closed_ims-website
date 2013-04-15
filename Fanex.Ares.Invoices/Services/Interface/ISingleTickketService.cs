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
using Fanex.Ares.Invoice.Criteria;
using Fanex.Ares.Invoice.Models.SingleTicket;

namespace Fanex.Ares.Invoice.Services.Interface
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public interface ISingleTickketService
    {
        List<Bettype> SelBettype(GetBettypeCriteria criteria);
    }
}
