// -----------------------------------------------------------------------
// <copyright file="LicenseeSettlementModel.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Ares.Invoice.ViewModels.LicenseeSettlement
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.Ares.Report.ViewModels;

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

    /// <summary>
    /// Report entity.
    /// </summary>
    [Serializable]
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Run stylecop")]
    public class ReportEntity
    {
        public DateTime WinlostDate { get; set; }

        public int Currency { get; set; }

        public string CurrencyName { get; set; }

        public double Turnover { get; set; }

        public double TurnoverBase { get; set; }

        public double Betcount { get; set; }

        public double MemberWinloss { get; set; }

        public double MemberWinlossBase { get; set; }

        public double MemberComm { get; set; }

        public double MemberTotal { get; set; }

        public double LicenseeWL { get; set; }

        public double LicenseeWLBase { get; set; }

        public double LicenseeComm { get; set; }

        public double LicenseeCommBase { get; set; }

        public double LicenseeTotal { get; set; }

        public double Company { get; set; }

        public double CompanyBaseCurrency { get; set; }

        public double BaseCurrencyExchangeRate { get; set; }
    }
}
