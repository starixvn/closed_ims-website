namespace Fanex.Ares.Report.Criteria.LicenseeSettlement
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Licensee Currency Select Criteria.
    /// </summary>
    public class LicenseeCurrencySelCriteria : CriteriaBase
    {
        public int LicenseeId
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
            return "IMS_LicenseeCurrencySel";
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