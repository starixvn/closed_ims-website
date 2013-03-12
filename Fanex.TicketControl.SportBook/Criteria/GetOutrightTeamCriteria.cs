using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fanex.NAP.Framework.Data;

namespace Fanex.TicketControl.SportBook.Criteria
{
    public class GetOutrightTeamCriteria : CriteriaBase
    {
        public override string GetSettingKey()
        {
            return "OutRightTicket_BettransDelSpecial_OutrightSel";
        }

        public override bool IsValid()
        {
            return true;
        }

        public int LeagueId { get; set; }
    }
}
