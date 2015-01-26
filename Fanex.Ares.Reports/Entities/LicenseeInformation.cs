namespace Fanex.Ares.Report.Entities
{
    using System;

    /// <summary>
    /// Licensee Information.
    /// </summary>
   [Serializable]
    public class LicenseeInformation
    {
        public DateTime SettlementDate { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public int LicenseeID { get; set; }

        public string LicenseeName { get; set; }

        public string ProductName { get; set; }

        public DateTime FirstBilling { get; set; }

        public string BasisOfBilling { get; set; }

        public string MinimumFees { get; set; }

        public string HeadOfTerms { get; set; }

        public string BaseCurrencyName { get; set; }
    }
}
