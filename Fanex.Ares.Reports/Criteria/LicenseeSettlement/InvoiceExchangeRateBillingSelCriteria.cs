namespace Fanex.Ares.Report.Criteria.LicenseeSettlement
{
    /// <summary>
    /// Invoice Exchange Rate Billing Select.
    /// </summary>
    public class InvoiceExchangeRateBillingSelCriteria : ExchangeRateBillingSelCriteria
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
