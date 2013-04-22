using System;
using Fanex.NAP.Framework.Data;

namespace Fanex.Ares.Report.Criteria
{
    public class SportBookSettlementCriteria : CriteriaBase
    {
        public override string GetSettingKey()
        {
            return "IMS_Rpt_LicenseeSettlement";
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
