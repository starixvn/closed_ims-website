namespace Fanex.Ares.Invoice.Criteria.Invoice
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// IMS Base Currency Select.
    /// </summary>
   public class IMS_GenInv_BaseCurrencySel : CriteriaBase
    {
        public int PartyID { get; set; }

        public string Products { get; set; }

        public int LicenseeID { get; set; }

        /// <summary>
        /// Gets the setting key.
        /// </summary>
        /// <returns>System String.</returns>
        public override string GetSettingKey()
        {
            return "IMS_GenInv_BaseCurrencySel";
        }

        /// <summary>
        /// Determines whether this instance is valid.
        /// </summary>
        /// <returns><c>true</c> if this instance is valid; otherwise, <c>false</c>.</returns>
        public override bool IsValid()
        {
            return true;
        }
    }
}