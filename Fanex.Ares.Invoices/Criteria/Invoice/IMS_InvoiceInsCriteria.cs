namespace Fanex.Ares.Invoice.Criteria.Invoice
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Invoice Insert Criteria.
    /// </summary>
    public class IMS_InvoiceInsCriteria : CriteriaBase
    {                     
        public string XMLInvoice { get; set; }

        public int AdminID { get; set; }

        public int InvoiceID { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        /// <summary>
        /// Gets the setting key.
        /// </summary>
        /// <returns>System String.</returns>
        public override string GetSettingKey()
        {
            return "IMS_InvoiceIns";
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
