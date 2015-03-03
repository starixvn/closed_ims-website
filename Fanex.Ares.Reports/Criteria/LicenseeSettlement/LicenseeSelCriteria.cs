namespace Fanex.Ares.Report.Criteria.LicenseeSettlement
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Licensee Select Criteria.
    /// </summary>
    public class LicenseeSelCriteria : CriteriaBase
    {
        /// <summary>
        /// Get Setting Key.
        /// </summary>
        /// <returns>Return Setting Key.</returns>
        public override string GetSettingKey()
        {
            return "IMS_LicenseeSel";
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