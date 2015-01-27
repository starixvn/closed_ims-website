namespace Fanex.Ares.Invoice.Models.Invoice
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Invoice Type.
    /// </summary>
    [Serializable]
    public class InvoiceType
    {
        public InvoiceType()
        {
            this.ProductList = new List<Product>();
        }

        public int InvoiceTypeID { get; set; }

        public string InvoiceTypeName { get; set; }

        public List<Product> ProductList { get; set; }
    }
}
