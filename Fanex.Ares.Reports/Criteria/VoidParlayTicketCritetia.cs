using System;
using Fanex.Ares.Core.Models;
using Fanex.Ares.Core.Utils;
using Fanex.NAP.Framework.Data;

namespace Fanex.Ares.Report.Criteria
{
    public class VoidParlayTicketCritetia : CriteriaBase
    {
        public override string GetSettingKey()
        {
            return "ParlayTicket_BettransDelSpecialDel_MixParlay";
        }

        public override bool IsValid()
        {
            return true;
        }

        public int MatchId { get; set; }

        public int Type { get; set; }//LiveIndicator

        public string TransIds { get; set; }

        public string Bettypes { get; set; }

        public string Betteam1378 { get; set; }

        public string Betteam515 { get; set; }

        public string Betteam430 { get; set; }

        public string Betteam6 { get; set; }

        public string Betteam14 { get; set; }

        public string Betteam16 { get; set; }

        public decimal HDP { get; set; }

        public string UserName { get; set; }

        public string UserPwd { get; set; }

        public int UserId { get; set; }

        public int ErrCode { get; set; }

        public string Errmsg { get; set; }

        public int LiveHomeScore { get; set; }

        public int LiveAwayScore { get; set; }

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

        public string IP {
            get {
                return Fanex.Ares.Core.Utils.Helper.GetClientIP();
            }
        }               

        public Message Msg { get; set; }

        // hard-core for testing
        public int DelOption {
            get { return 2; }
        }

    }
}
