// -----------------------------------------------------------------------
// <copyright file="LicenseeSettlementModel.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Ares.Invoice.Models.LicenseeSettlement
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [Serializable]
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Run stylecop")]
    public class LicenseeSettlementModel
    {
        public LicenseeSettlementModel()
        {
            this.ExchangeList = new List<ExchangeRate>();
            this.Report = new Report();
            this.LicenseeInfor = new LicenseeInformation();
        }

        public LicenseeInformation LicenseeInfor { get; set; }

        public Report Report { get; set; }

        public List<ExchangeRate> ExchangeList { get; set; }
    }
}
