namespace Fanex.Ares.Report.Entities
{
    /// <summary>
    /// Base Ticket Information.
    /// </summary>
    public class BaseTicketInfo
    {
        public string BetId { get;  set; }

        public int BetType { get; set; }

        public string BetTypeName { get; set; }

        public string Choice { get; set; }

        public string CurrencyName { get; set; }

        public string Hdp { get; set; }

        public int No { get; set; }

        public string Odds { get; set; }

        public string Player { get; set; }

        public string SA { get; set; }

        public string Score { get; set; }

        public string Stake { get; set; }

        public string Status { get; set; }

        public string TransDate { get; set; }
    }
}