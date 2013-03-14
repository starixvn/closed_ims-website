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
    public class GetSingleTicketCriteria : CriteriaBase
    {
        public int MatchId { get; set; }

        public Int64 FromRefNo { get; set; }

        public Int64 ToRefNo { get; set; }

        public decimal Hdp { get; set; }

        public int LiveIndicator { get; set; }

        public int? LiveHomeScore { get; set; }

        public int? LiveAwayScore { get; set; }

        public DateTime? FromTransdate { get; set; }

        public DateTime? ToTransdate { get; set; }

        public string Bettypes { get; set; }

        public string Betteam1378 { get; set; }

        public string Betteam515 { get; set; }

        public string Betteam430 { get; set; }

        public string Betteam6 { get; set; }

        public string Betteam14 { get; set; }

        public string Betteam16 { get; set; }

        public string UserName { get; set; }

        public string SAFilter { get; set; }

        public override string GetSettingKey()
        {
            return "SingleTicket_BettransSelSpecialDel";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
