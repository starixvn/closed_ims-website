using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fanex.NAP.Framework.Data;

namespace Fanex.TicketControl.Misc.Criteria
{
    public class GetBatchMessageCriteria : CriteriaBase
    {
        public override bool IsValid()
        {
            return true;
        }

        public string TransIds { get; set; }

        public override string GetSettingKey()
        {
            return "Misc_BettransSelBet_BatchMsg";
        }
    }
}
