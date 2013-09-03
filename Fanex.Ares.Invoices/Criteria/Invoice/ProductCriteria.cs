// -----------------------------------------------------------------------
// <copyright file="ProductCriteria.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Ares.Invoice.Criteria.Invoice
{
    using System.Diagnostics.CodeAnalysis;

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
