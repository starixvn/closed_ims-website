// -----------------------------------------------------------------------
// <copyright file="IMS_ExchangeRateBillingSelCriteria.cs" company="Nexcel">
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

    public class IMS_ExchangeRateBillingSelCriteria : CriteriaBase
    {
        public string Currency { get; set; }
        public DateTime ExchangeDate { get; set; }

        public override string GetSettingKey()
        {
            return "IMS_ExchangeRateBillingSel";
        }

        public override bool IsValid() { return true; }
    }
}