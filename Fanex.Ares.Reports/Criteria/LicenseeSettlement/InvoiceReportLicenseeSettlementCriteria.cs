namespace Fanex.Ares.Report.Criteria.LicenseeSettlement
{
    using System;

    /// <summary>
    /// Invoice Report Licensee Settlement Criteria.
    /// </summary>
    [Serializable]
    public class InvoiceReportLicenseeSettlementCriteria : ReportLicenseeSettlementCriteria
    {
        /// <summary>
        /// Get Setting Key.
        /// </summary>
        /// <returns>Return value.</returns>
        public override string GetSettingKey()
        {
            return "IMS_Invoice_Rpt_LicenseeSettlement";
        }
    }
}