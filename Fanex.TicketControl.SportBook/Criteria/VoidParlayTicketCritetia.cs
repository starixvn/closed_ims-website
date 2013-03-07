using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fanex.NAP.Framework.Data;
using Fanex.TicketControl.Core.Models;

namespace Fanex.TicketControl.SportBook.Criteria
{
    public class VoidParlayTicketCritetia : CriteriaBase
    {
        public override string GetSettingKey()
        {
            return "SingleTicket_BettransDelSpecial";
        }

        public override bool IsValid()
        {
            return true;
        }

        public int MatchId { get; set; }

        public Int64 f_transid { get; set; }

        public Int64 t_transid { get; set; }

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

        public int LiveIndicator { get; set; }

        public int LiveHomeScore { get; set; }

        public int LiveAwayScore { get; set; }

        public DateTime? FromDateTime { get; set; }

        public DateTime? ToDateTime { get; set; }

        public string IP {
            get {
                return Fanex.TicketControl.Core.Utils.Helper.GetClientIP();
            }
        }

        public int SAFilter { get; set; }

        public Message Msg { get; set; }

        // hard-core for testing
        public int DelOption {
            get { return 2; }
        }

    }
}
