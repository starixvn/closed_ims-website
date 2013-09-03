// -----------------------------------------------------------------------
// <copyright file="ChargeRate.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Ares.Invoice.Models.Invoice
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Run stylecop")]
    public class ChargeRate
    {
        public int ProductID { get; set; }

        public decimal DistributorRate { get; set; }

        public decimal PartnerRate { get; set; }

        public decimal ONEworksRate { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}
