namespace Fanex.Ares.Invoice.Criteria.Invoice
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Invoice Insertion Criteria.
    /// </summary>
    public class InvoiceInsertionCriteria : CriteriaBase
    {                     
        public string XMLInvoice { get; set; }

        public int AdminID { get; set; }

        public int InvoiceID { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
        {
            return "IMS_InvoiceIns";
        }

        public override bool IsValid() 
        {
            return true; 
        }
    }
}
