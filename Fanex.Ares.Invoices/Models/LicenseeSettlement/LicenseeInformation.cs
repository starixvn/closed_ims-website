// -----------------------------------------------------------------------
// <copyright file="LicenseeInformation.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Ares.Invoice.Models.LicenseeSettlement
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [Serializable]
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Run stylecop")]
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
