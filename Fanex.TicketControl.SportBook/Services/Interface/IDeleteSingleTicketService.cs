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
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Diagnostics.CodeAnalysis;
using Fanex.TicketControl.Core.Models;
using Fanex.TicketControl.Core.Criteria;
using Fanex.TicketControl.SportBook.ViewModels.SingleTicket;
using Fanex.TicketControl.SportBook.Criteria;


    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public interface IDeleteSingleTicketService
    {
        SingleTicketModel GetSingleTicket(GetSingleTicketCriteria criteria);
    }
}
