// -----------------------------------------------------------------------
// <copyright file="IMS_CheckExistedInvoiceNo.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Ares.Invoice.Criteria.Invoice
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class IMS_CheckExistedInvoiceNo : CriteriaBase
    {
        public int PartyID { get; set; }
        public int InvoiceNo { get; set; }

        public bool IsExisted { get; set; }
        public override string GetSettingKey()
        {
            return "IMS_CheckExistedInvoiceNo";
        }

        public override bool IsValid() { return true; }
    }
}
