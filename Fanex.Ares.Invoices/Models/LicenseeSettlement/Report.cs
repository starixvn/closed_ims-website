// -----------------------------------------------------------------------
// <copyright file="Report.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Ares.Invoice.Models.LicenseeSettlement
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Initializes static members of the Report class.
    /// </summary>
    [Serializable]
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Run stylecop")]
    public class Report
    {
        public Report()
        {
            this.EntityList = new List<ReportEntity>();
            this.GrandTotal = new ReportEntity();
        }

        public List<ReportEntity> EntityList { get; set; }

        public ReportEntity GrandTotal { get; set; }
    }
}
