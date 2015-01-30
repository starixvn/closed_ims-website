namespace Fanex.Ares.Invoice.Criteria.Invoice
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Class IMS Check Existed Invoice No.
    /// </summary>
    public class IMS_CheckExistedInvoiceNo : CriteriaBase
    {
        public int PartyID { get; set; }

        public int InvoiceNo { get; set; }

        public bool IsExisted { get; set; }

        /// <summary>
        /// Gets the setting key.
        /// </summary>
        /// <returns>System String.</returns>
        public override string GetSettingKey()
        {
            return "IMS_CheckExistedInvoiceNo";
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
