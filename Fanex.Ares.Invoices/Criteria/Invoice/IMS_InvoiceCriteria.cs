// -----------------------------------------------------------------------
// <copyright file="IMS_InvoiceCriteria.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Ares.Invoice.Criteria.Invoice
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
     [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Run stylecop")]
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

    /// <summary>
    /// Product criteria
    /// </summary>
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Run stylecop")]
    public class ProductCriteria
    {
        public string ProductName { get; set; }

        public int ProductID { get; set; }
    }
}
