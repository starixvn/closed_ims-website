namespace Fanex.Ares.Invoice.Criteria.Invoice
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get Invoice Number Criteria.
    /// </summary>
     public class InvoiceNumberCriteria : CriteriaBase
    {
        public int PartyId { get; set; }

        /// <summary>
        /// Gets the setting key.
        /// </summary>
        /// <returns>System String.</returns>
        public override string GetSettingKey()
        {
            return "IMS_NewInvoiceNoSel";
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
