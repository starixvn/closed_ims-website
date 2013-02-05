// -----------------------------------------------------------------------
// <copyright file="SingleTickketService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//   <change who="Robert.Tran" date="2013.2.05">Created</change>
// </history>

namespace Fanex.TicketControl.SportBook.Services.Implement
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.TicketControl.SportBook.Services.Interface;
    using Fanex.TicketControl.SportBook.Models.SingleTicket;
    using Fanex.TicketControl.SportBook.Criteria;
    using Fanex.TicketControl.Core.Services.Impl;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class SingleTickketService : ServiceBase, ISingleTickketService
    {
        public List<Bettype> SelBettype(GetBettypeCriteria criteria)
        {
            List<Bettype> bettypes = new List<Bettype>();
            try
            {
                var rawData = base.GetReportData<dynamic>(criteria);
                foreach (var row in rawData)
                {
                    bettypes.Add(new Bettype() { 
                        Id = Convert.ToInt32(row.typeid),
                        Name = Convert.ToString(row.bettypename),
                        LiveIndicator = Convert.ToBoolean(row.liveindicator)
                    });
                }
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }
            return bettypes;
        }
    }
}
