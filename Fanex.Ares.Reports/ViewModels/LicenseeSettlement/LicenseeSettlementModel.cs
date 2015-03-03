namespace Fanex.Ares.Report.ViewModels.LicenseeSettlement
{
    using System;
    using System.Collections.Generic;
    using Fanex.Ares.Report.Entities;

    /// <summary>
    /// Licensee Settlement Model.
    /// </summary>
    [Serializable]
    public class LicenseeSettlementModel
    {
        private List<ExchangeRate> _exchangeList;
        private List<LicenseeReport> _reportList;

        public LicenseeSettlementModel()
        {
            _exchangeList = new List<ExchangeRate>();
            _reportList = new List<LicenseeReport>();
            LicenseeInfor = new LicenseeInformation();
            GrandTotal = new LicenseeReport();
            ForexTime = string.Empty;
        }

        public LicenseeInformation LicenseeInfor { get; set; }

        public LicenseeReport GrandTotal { get; set; }

        public List<LicenseeReport> ReportList
        {
            get { return _reportList; }
        }

        public List<ExchangeRate> ExchangeList
        {
            get { return _exchangeList; }
        }

        public string ForexTime { get; set; }
    }
}