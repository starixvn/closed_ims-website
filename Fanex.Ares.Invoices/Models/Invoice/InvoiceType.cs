// -----------------------------------------------------------------------
// <copyright file="InvoiceType.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Ares.Invoice.Models.Invoice
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [Serializable]
    public class InvoiceType
    {
        public int InvoiceTypeID { get; set; }

        public string InvoiceTypeName { get; set; }

        public List<Product> ProductList { get; set; }

        public InvoiceType()
        {
            ProductList = new List<Product>();
        }
    }
}
