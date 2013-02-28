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
    using Fanex.TicketControl.Core.Services.Impl;
    

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class BaseTicketInfo
    {
        public int No { get; set; }

        public string BetId { get; set; }

        public string Player { get; set; }

        public string TransDate { get; set; }

        public int BetType { get; set; }

        private string _BetTypeName;
        public string BetTypeName {
            get 
            {
                return _BetTypeName;
            } 
            private set
            {
                _BetTypeName = CacheRef.GetBettype(BetType);
            } 
        }

        public string Choice { get; set; }

        public string Stake { get; set; }

        public string Odds { get; set; }

        public string CurrencyName { get; set; }

        public string Status { get; set; }

        public string HDP { get; set; }

        public string Score { get; set; }

        public string Creator { get; set; }

        
    }
}
