using System;
using Fanex.NAP.Framework.Data;

namespace Fanex.Ares.Report.Criteria
{
    public class GetRecoverTicketCriteria : CriteriaBase
    {
        public override string GetSettingKey()
        {
            return "RecoverTicket_BettransSelRecoverSpecialBets";
        }

        public override bool IsValid()
        {
            return true;
        }

        public Int64 TransId { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }
    }
}
