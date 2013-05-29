using Fanex.NAP.Framework.Data;

namespace Fanex.Ares.Report.Criteria
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
