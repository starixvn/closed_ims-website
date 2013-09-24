// -----------------------------------------------------------------------
// <copyright file="ExchangeRate.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Ares.Invoice.Models.LicenseeSettlement
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [Serializable]
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Run stylecop")]
    public class ExchangeRate
    {
        public int Currency { get; set; }

        public string CurrencyName { get; set; }

        public double BasePerUnit { get; set; }

        public double UnitsPerBase { get; set; }
    }
}
