namespace Fanex.Ares.Invoice.Criteria.Invoice
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Base Currency Selection Criteria.
    /// </summary>
   public class BaseCurrencySelectionCriteria : CriteriaBase
    {
        public int PartyID { get; set; }

        public string Products { get; set; }

        public int LicenseeID { get; set; }

        public override string GetSettingKey()
        {
            return "IMS_GenInv_BaseCurrencySel";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}