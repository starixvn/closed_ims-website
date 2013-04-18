// -----------------------------------------------------------------------
// <copyright file="SportBookModel.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Ares.Report.ViewModels.SportBook
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SportBookModel
    {
        public LicenseeInformation LicenseeInfor { get; set; }

        public Report Report { get; set; }

        public List<ExchangeInformation> ExchangeList { get; set; }

        public SportBookModel()
        {
            ExchangeList = new List<ExchangeInformation>();

            for (int i = 0; i < 10; i++)
            {
                ExchangeList.Add(new ExchangeInformation{
                CurrencyID = i,
                CurrencyName = "AUD",
                CurrencyPerUnit = 0.81340,
                UnitsPerCurrency = 1.22941
                });
            }

            Report.GrandTotal = 23406.55;

            for (int i = 0; i < 10; i++)
            {
                Report.EntityList.Add(new ReportEntity
                {
                    Date = new DateTime(2013, 4, 18),
                    CurrencyID = i,
                    CurrencyName = "AUD",
                    Turnover = 38891725.25,
                    TotalTrans = 412365.25,
                    MemberWinloss = 1963456.98,
                    MemberComm = 0,
                    MemberTotal = 1963456.98,
                    LicenseeWinloss = 4123697.88,
                    LicenseeComm = 0,
                    LicenseeTotal = 4123697.88,
                    Company = 5613565.59,
                    Total = 5613565.59
                });
            }

            LicenseeInfor.SettlementDate = new DateTime(2013, 4,10);
            LicenseeInfor.LicenseeID = 8;
            LicenseeInfor.LicenseeName = "Macaubet";
            LicenseeInfor.BillingDate = new DateTime(2013, 4, 15);
            LicenseeInfor.BasicBilling = "15% of Net Win";
            LicenseeInfor.MinimumFees = "EUR15000 (minimum fees waived 3 mos from date of launch 08Jun2012)";
            LicenseeInfor.HeadOfTermDate = new DateTime(2013, 3, 10);
        }
    }

    public class LicenseeInformation
    {
        public DateTime SettlementDate { get; set; }

        public int LicenseeID { get; set; }

        public string LicenseeName { get; set; }

        public DateTime BillingDate { get; set; }

        public string BasicBilling { get; set; }

        public string MinimumFees { get; set; }

        public DateTime HeadOfTermDate { get; set; }
    }

    public class ExchangeInformation
    {
        public int CurrencyID { get; set; }

        public string CurrencyName { get; set; }

        public double CurrencyPerUnit { get; set; }

        public double UnitsPerCurrency { get; set; }
    }

    public class Report
    {
        public List<ReportEntity> EntityList { get; set; }

        public double GrandTotal { get; set; }

        public Report()
        {
            EntityList = new List<ReportEntity>();
        }
    }

    public class ReportEntity
    {
        public DateTime Date { get; set; }

        public int CurrencyID { get; set; }

        public string CurrencyName { get; set; }

        public double Turnover { get; set; }

        public double TotalTrans { get; set; }

        public double MemberWinloss { get; set; }

        public double MemberComm { get; set; }

        public double MemberTotal { get; set; }

        public double LicenseeWinloss { get; set; }

        public double LicenseeComm { get; set; }

        public double LicenseeTotal { get; set; }

        public double Company { get; set; }

        public double Total { get; set; } 
    }
}
