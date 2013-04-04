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
    using Fanex.TicketControl.Core.Utils;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class GetParlayTicketCriteria : CriteriaBase
    {
        public int MatchId { get; set; }

        public int Bettype { get; set; }

        public string Bettypes { get; set; }

        public decimal Hdp { get; set; }

        public int Type { get; set; }

        public int ? LiveHomeScore { get; set; }

        public int ? LiveAwayScore { get; set; }

        DateTime? _FromTransdate;
        public DateTime? FromTransdate
        {
            get
            {
                if (_FromTransdate.HasValue)
                {
                    _FromTransdate = Helper.ConvertDatetimeZoneCanada(Convert.ToDateTime(_FromTransdate));
                }
                return _FromTransdate;
            }
            set { _FromTransdate = value; }
        }

        DateTime? _ToTransdate;
        public DateTime? ToTransdate
        {
            get
            {
                if (_ToTransdate.HasValue)
                {
                    _ToTransdate = Helper.ConvertDatetimeZoneCanada(Convert.ToDateTime(_ToTransdate));
                }
                return _ToTransdate;

            }
            set { _ToTransdate = value; }
        }

        public string Betteam1378 { get; set; }

        public string Betteam515 { get; set; }

        public string Betteam430 { get; set; }

        public string Betteam6 { get; set; }

        public string Betteam14 { get; set; }

        public string Betteam16 { get; set; }


        public override string GetSettingKey()
        {
            return "ParlayTicket_BettransSelDelSpecial_MixParlay";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
