// -----------------------------------------------------------------------
// <copyright file="IMS_Invoice_ExchangeRateBillingSelCriteria.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//   <change who="Phillip.Truong" date="2013.4.23">Created</change>
// </history>

namespace Fanex.Ares.Report.Criteria.LicenseeSettlement
{
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// IMS Invoice Exchange Rate Billing Select.
    /// </summary>
    public class IMS_Invoice_ExchangeRateBillingSelCriteria : IMS_ExchangeRateBillingSelCriteria
    {
         /// <summary>
        /// Get Setting Key.
         /// </summary>
         /// <returns>Return value.</returns>
        public override string GetSettingKey()
        {
            return "IMS_Invoice_ExchangeRateBillingSel";
        }
    }
}
