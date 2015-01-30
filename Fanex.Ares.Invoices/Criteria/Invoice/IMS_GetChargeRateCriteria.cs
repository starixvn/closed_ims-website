namespace Fanex.Ares.Invoice.Criteria.Invoice
{
    using System;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get Charge Rate Criteria.
    /// </summary>
    public class IMS_GetChargeRateCriteria : CriteriaBase
    {
        public int PartyID { get; set; }

        public int LicenseeID { get; set; }

        public string Products { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        /// <summary>
        /// Gets the setting key.
        /// </summary>
        /// <returns>System String.</returns>
        public override string GetSettingKey()
        {
            return "IMS_LicenseeProductChargeRateSel";
        }

        /// <summary>
        /// Determines whether this instance is valid.
        /// </summary>
        /// <returns><c>True</c> if this instance is valid; otherwise, <c>false</c>.</returns>
        public override bool IsValid() 
        { 
            return true; 
        }
    }
}
