using System;
using Fanex.BetList.Core.Entities;
using Fanex.TicketControl.Misc.Services.Impl;
using Fanex.TicketControl.Core.Services.Impl;

namespace Fanex.TicketControl.Misc.Entities
{
    [Serializable]
    public class TicketListingDetail
    {
        // Convention:
        // Field TransID is Bet ID

        public ITicket Ticket { get; set; }

        public ITicket ParlayTicket { get; set; }
     
        public string AgentWinloss
        {
            get
            {
                if (this.Ticket.Status != "running" && this.Ticket.Status != "waiting")
                    return this.Ticket.Winlost.ToString();
                return string.Empty;
            }
        }

        public string TransDate { get; set; }

        public string ChoiceHtml { get; set; } // html 

        public string OddsHtml { get; set; }

        public string Currency { get; set; }

        #region additional fields for mix parlay

        public bool IsMixParlay { get; set; }

        public string BetType { get; set; }

        public string ParlayBetType { get; set; }
        
        public string Selection { get; set; }

        #endregion

        public bool SAFlag { get; set; } // second accept flag

    }
}




