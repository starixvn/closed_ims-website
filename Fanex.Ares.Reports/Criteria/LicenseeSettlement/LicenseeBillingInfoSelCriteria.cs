namespace Fanex.Ares.Report.Criteria.LicenseeSettlement
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Licensee Billing Info Select Criteria.
    /// </summary>
    public class LicenseeBillingInfoSelCriteria : CriteriaBase
    {
        public int LicenseeId 
        { 
            get; 
            set; 
        }

        public int ProductId 
        { 
            get; 
            set; 
        }

        public int PartyId 
        { 
            get; 
            set; 
        }

        /// <summary>
        /// Get setting key.
        /// </summary>
        /// <returns>Return setting key.</returns>
        public override string GetSettingKey()
        {
            return "IMS_Invoice_LicenseeBillingInfoSel";
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