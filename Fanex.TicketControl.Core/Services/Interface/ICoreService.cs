// -----------------------------------------------------------------------
// <copyright file="ICommonService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.TicketControl.Core.Services.Interface
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
using Fanex.TicketControl.Core.Models;
using Fanex.TicketControl.Core.Criteria;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ICoreService
    {
        IList<Bettypes> GetBettypes(GetBettypeCriteria criteria);

        IList<Licensee> GetAllLicensees();
    }
}
