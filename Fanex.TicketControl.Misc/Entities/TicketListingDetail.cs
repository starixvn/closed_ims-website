using System;
using Fanex.BetList.Core.Entities;
using Fanex.TicketControl.Misc.Services.Impl;

namespace Fanex.TicketControl.Misc.Entities
{
    [Serializable]
    public class TicketListingDetail
    {
        // Convention:
        // Field TransID is Bet ID

        public ITicket Ticket { get; set; }

        public SportEvent SportEvent
        {
            get
            {
                var sportEventService = new SportEventService(this);
                return sportEventService.GetSportEvent();
            }
        }

        public string TransDate { get; set; }

        public string ChoiceHtml { get; set; } // html 

        public string OddsHtml { get; set; }

        public string Currency { get; set; }

        #region additional fields for mix parlay

        public bool IsMixParlay { get; set; }

        public int BettypeM { get; set; }

        public string Selection { get; set; }

        #endregion

        public bool SAFlag { get; set; } // second accept flag

    }
}




