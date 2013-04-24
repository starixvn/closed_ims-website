// -----------------------------------------------------------------------
// <copyright file="IMS_Rpt_LicenseeSettlementCriteria.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//   <change who="Phillip.Truong" date="2013.4.23">Created</change>
// </history>
namespace Fanex.Ares.Report.Criteria.LicenseeSettlement
{
    using System;
    using Fanex.NAP.Framework.Data;

    public class IMS_Rpt_LicenseeSettlementCriteria : CriteriaBase
    {
        public int LicenseeId { get; set; }
        public string LicenseeName { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int Product { get; set; }
        public int Currency { get; set; }
        //public DateTime MoveBetsDay { get; set; }
        public int BaseCurrency { get; set; }

        public override string GetSettingKey()
        {
            return "IMS_Rpt_LicenseeSettlement";
        }

        public override bool IsValid() { return true; }
    }
}