// -----------------------------------------------------------------------
// <copyright file="TicketInfo.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.TicketControl.SportBook.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class BaseTicketInfo
    {
        public int Index { get; set; }

        public string TransId { get; set; }

        public string UserName { get; set; }

        public string TransTime { get; set; }

        public int BetType { get; set; }

        public string BetTypeName { get; set; }

        public string Betteam { get; set; }

        public string Stake { get; set; }

        public string Odds { get; set; }

        public string CurrencyName { get; set; }

        public string Status { get; set; }

        public string HDP { get; set; }

        public string Score { get; set; }

        public string Creator { get; set; }
        
    }
}
