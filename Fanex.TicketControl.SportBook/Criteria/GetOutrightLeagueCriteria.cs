using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fanex.NAP.Framework.Data;

namespace Fanex.TicketControl.SportBook.Criteria
{
    public class GetOutrightLeagueCriteria : CriteriaBase
    {
        public override string GetSettingKey()
        {
            return "OutRightTicket_BettransDelSpecial_LeagueSelOutright";
        }

        public override bool IsValid()
        {
            return true;
        }      
    }
}
