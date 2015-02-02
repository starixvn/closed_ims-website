namespace Fanex.Ares.Invoice.Criteria.Invoice
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get Licensee By Party Criteria.
    /// </summary>
    public class LicenseeByPartySelectionCriteria : CriteriaBase
    {
        public int PartyID { get; set; }

        public override string GetSettingKey()
        {
            return "IMS_LicenseeSelByParty";
        }

        public override bool IsValid() 
        { 
            return true; 
        }
    }
}