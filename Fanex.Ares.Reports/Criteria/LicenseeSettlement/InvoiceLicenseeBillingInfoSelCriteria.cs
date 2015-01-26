namespace Fanex.Ares.Report.Criteria.LicenseeSettlement
{
    /// <summary>
    /// Invoice LicenseeBilling Info Select Criteria.
    /// </summary>
    public class InvoiceLicenseeBillingInfoSelCriteria : LicenseeBillingInfoSelCriteria
    {
        /// <summary>
        /// Get Setting Key.
        /// </summary>
        /// <returns>Return value.</returns>
        public override string GetSettingKey()
        {
            return "IMS_Invoice_LicenseeBillingInfoSel";
        }
    }
}