namespace Fanex.Ares.Invoice.Criteria.Invoice
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Existed Invoice Number Checking Criteria.
    /// </summary>
    public class ExistedInvoiceNumberCheckingCriteria : CriteriaBase
    {
        public int PartyID { get; set; }

        public int InvoiceNo { get; set; }

        public bool IsExisted { get; set; }

        public override string GetSettingKey()
        {
            return "IMS_CheckExistedInvoiceNo";
        }

        public override bool IsValid() 
        { 
            return true; 
        }
    }
}
