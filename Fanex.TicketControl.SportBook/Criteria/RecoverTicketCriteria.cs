using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fanex.NAP.Framework.Data;
using Fanex.TicketControl.Core.Models;

namespace Fanex.TicketControl.SportBook.Criteria
{
    public class RecoverTicketCriteria : CriteriaBase
    {
        public override string GetSettingKey()
        {
            return "RecoverTicket_BettransRecoverSpecialBets";
        }

        public override bool IsValid()
        {
            return true;
        }

        public Int64 TransId { get; set; }

        public int UserId { get; set; }

        public string Ip { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public Message Msg { get; set; }
    }
}
