namespace Fanex.Ares.Report.Criteria.LicenseeSettlement
{
    using System;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Exchange Rate Billing Select Criteria.
    /// </summary>
    public class ExchangeRateBillingSelCriteria : CriteriaBase
    {
        public string Currency
        {
            get;
            set;
        }

        public DateTime ExchangeDate
        {
            get;
            set;
        }

        [SpParam(Ignored = true)]
        public int BaseCurrencyId
        {
            get;
            set;
        }

        [SpParam(Ignored = true)]
        public string BaseCurrencyName
        {
            get;
            set;
        }

        /// <summary>
        /// Get Setting Key.
        /// </summary>
        /// <returns>Return Setting Key.</returns>
        public override string GetSettingKey()
        {
            return "IMS_ExchangeRateBillingSel";
        }

        /// <summary>
        /// Check is valid.
        /// </summary>
        /// <returns>Return result.</returns>
        public override bool IsValid()
        {
            return true;
        }
    }
}