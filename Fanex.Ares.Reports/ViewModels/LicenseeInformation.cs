// -----------------------------------------------------------------------
// <copyright file="LicenseeInformation.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Ares.Report.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
   [Serializable]
    public class LicenseeInformation
    {
        public DateTime SettlementDate { get; set; }

        public int LicenseeID { get; set; }

        public string LicenseeName { get; set; }

        public DateTime BillingDate { get; set; }

        public string BasicBilling { get; set; }

        public string MinimumFees { get; set; }

        public DateTime HeadOfTermDate { get; set; }
    }
}
