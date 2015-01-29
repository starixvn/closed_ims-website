namespace Fanex.Ares.Invoice.Models.Invoice
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Statement Detail.
    /// </summary>
    [Serializable]
    public class StatementDetail 
    {
        public StatementDetail()
        {
            this.StatementProductDetail = new List<StatementDetail>();
        }

        public int ID { get; set; }

        public string Description { get; set; }

        public string Reference { get; set; }

        public double Amount { get; set; }

        public string AmountFormated { get; set; }

        public int AdjustItem { get; set; }

        public string InvoiceMonth { get; set; }

        public bool DiscountItem { get; set; }

        public int ProductType { get; set; }

        public List<StatementDetail> StatementProductDetail { get; set; }
    }
}
