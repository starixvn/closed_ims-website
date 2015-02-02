namespace Fanex.Ares.Invoice.Criteria.Invoice
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get Invoice Type Criteria.
    /// </summary>
     public class InvoiceTypesCriteria : CriteriaBase
    {
        public int LicenseeId { get; set; }

        public int PartyId { get; set; }

        public override string GetSettingKey()
        {
            return "IMS_InvoiceTypeSel";
        }

        public override bool IsValid() 
        { 
            return true; 
        }
    }
}
