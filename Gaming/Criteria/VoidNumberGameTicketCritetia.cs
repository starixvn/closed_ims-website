using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fanex.NAP.Framework.Data;
using Fanex.TicketControl.Core.Models;

namespace Fanex.TicketControl.Gaming.Criteria
{
    public class VoidNumberGameTicketCritetia : CriteriaBase
    {
        public override string GetSettingKey()
        {
            return "NumberGameTicket_BettransDelSpecial";
        }

        public override bool IsValid()
        {
            return true;
        }

        public int MatchId { get; set; }

        public Int64 F_transid { get; set; }

        public Int64 T_transid { get; set; }

        public string TransIds { get; set; }

        // Note: will be deleted when integrate with new sp 
        public string TransIds_N { get; set; }

        public string Dead { get; set; }

        public string Live { get; set; }

        public string UserName { get; set; }

        public string UserPwd { get; set; }

        public int UserId { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public string IP
        {
            get
            {
                return Fanex.TicketControl.Core.Utils.Helper.GetClientIP();
            }
        }

        public Message Msg { get; set; }

        // hard-core for testing
        public int DelOption {
            get { return 2; }
        }

    }
}
