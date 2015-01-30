namespace Fanex.Ares.Invoice.Criteria.Invoice
{
    using System;

    /// <summary>
    /// Invoice Criteria.
    /// </summary>
    public class IMS_InvoiceCriteria
    {
        public int OperatorID { get; set; }

        public string OperatorName { get; set; }

        public int LicenseeID { get; set; }

        public string LicenseeName { get; set; }

        public int CurrencyID { get; set; }

        public string CurrencyName { get; set; }

        public int InvoiceTypeID { get; set; }

        public string InvoiceTypeName { get; set; }

        public int InvoiceNo { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public DateTime DueDate { get; set; }
    }
}
