namespace Fanex.Ares.Report.Criteria.LicenseeSettlement
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Licensee Product Select Criteria.
    /// </summary>
    public class LicenseeProductSelCriteria : CriteriaBase
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
            return "IMS_LicenseeProductSel";
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