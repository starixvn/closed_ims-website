using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fanex.NAP.Framework.Data;
using Fanex.TicketControl.Core.Models;

namespace Fanex.TicketControl.Gaming.Criteria
{
    public class RecoverNumberGameTicketCriteria : CriteriaBase
    {
        public long TransId { get; set; }

        public int UserId { get; set; }

        public bool IsNegativeBal { get; set; }

        public string IP
        {
            get
            {
                return Fanex.TicketControl.Core.Utils.Helper.GetClientIP();
            }
        }

        public Message Msg { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
        {
            return "RecoverNumberGameTicket_Acc_BettransRecoverBet_NumberGame";
        }

        /// <summary>
        /// Check data's constrains
        /// </summary>
        /// <returns>True: valid; False: invalid</returns>
        public override bool IsValid()
        {
            return true;
        }
    }
}
