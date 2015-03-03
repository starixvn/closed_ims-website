namespace Fanex.Ares.Report.Entities
{
    using System;

    /// <summary>
    /// Licensee Report.
    /// </summary>
    [Serializable]
    public class LicenseeReport
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