namespace Fanex.Ares.Report.Criteria.LicenseeSettlement
{
    using System;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Report Licensee Settlement Criteria.
    /// </summary>
    [Serializable]
    public class ReportLicenseeSettlementCriteria : CriteriaBase
    {
        public ReportLicenseeSettlementCriteria()
            : base()
        {
            this.Month = DateTime.Now.Month - 1;
            this.Year = DateTime.Now.Year;
        }
   
        public int LicenseeId 
        { 
            get; 
            set; 
        }

        public string LicenseeName 
        { 
            get; 
            set; 
        }

        public DateTime FromDate 
        { 
            get; 
            set; 
        }

        public DateTime ToDate 
        { 
            get; 
            set; 
        }

        public int Product 
        { 
            get; 
            set; 
        }

        public string ProductName 
        { 
            get; 
            set; 
        }

        public int Currency 
        { 
            get; 
            set; 
        }

        public int BaseCurrency 
        { 
            get; 
            set; 
        }

        public string BaseCurrencyName 
        { 
            get; 
            set; 
        }

        [SpParam(Ignored = true)]
        public int PartyId 
        { 
            get; 
            set; 
        }

        [SpParam(Ignored = true)]
        public int Month 
        { 
            get; 
            set; 
        }

        [SpParam(Ignored = true)]
        public int Year 
        { 
            get; 
            set; 
        }

        [SpParam(Ignored = true)]
        public bool InvoiceReport 
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
            return "IMS_Rpt_LicenseeSettlement";
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