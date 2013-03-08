// -----------------------------------------------------------------------
// <copyright file="GetSingleTicket.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.TicketControl.SportBook.Criteria
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class GetParlaySubTicketCriteria : CriteriaBase
    {
        public string RefNo { get; set; }

        public Int64 TransId { get; set; }

        public int CurrencyId { get; set; }

        public override string GetSettingKey()
        {
            return "ParlayTicket_SubTicket_BettransSelSpecialDel_MixParlay";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
