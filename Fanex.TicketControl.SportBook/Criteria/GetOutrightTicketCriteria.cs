using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fanex.NAP.Framework.Data;

namespace Fanex.TicketControl.SportBook.Criteria
{
    public class GetOutrightTicketCriteria : CriteriaBase
    {

        public override string GetSettingKey()
        {
            return "OutRightTicket_BettransSelDelSpecial_Outright";
        }

        public override bool IsValid()
        {
            return true;
        }

        public int OutrightId { get; set; }

        public Int64 FromRefNo { get; set; }

        public Int64 ToRefNo { get; set; }

        public string UserName { get; set; }
    }
}
