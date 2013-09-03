// -----------------------------------------------------------------------
// <copyright file="InvoiceType.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Ares.Invoice.Models.Invoice
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Run stylecop")]
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
