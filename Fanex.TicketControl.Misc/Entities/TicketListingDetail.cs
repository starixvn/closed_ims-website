using System;
using Fanex.BetList.Core.Entities;
using Fanex.TicketControl.Misc.Services.Impl;
using Fanex.TicketControl.Core.Services.Impl;
using Fanex.TicketControl.Core.Models;
using System.Web.Script.Serialization;

namespace Fanex.TicketControl.Misc.Entities
{
    [Serializable]
    public class TicketListingDetail : TicketDetail
    {
        [ScriptIgnore]
        public ITicket ParlayTicket { get; set; }

        public long BetId
        {
            get
            {
                return this.Ticket.TransId;
            }
        }

        public string Member
        {
            get
            {
                return this.Ticket.UserName;
            }
        }

        public decimal Stake
        {
            get
            {
                return this.Ticket.Stake;
            }
        }

        public string Status
        {
            get
            {
                return this.Ticket.Status;
            }
        }

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

        public string Currency { get; set; }

        public int BetTypeId
        {
            get
            {
                return this.Ticket.BetTypeId;
            }
        }

        #region additional fields for mix parlay

        public bool IsMixParlay { get; set; }

        public string BetType { get; set; }

        public string ParlayBetType { get; set; }

        public string Selection { get; set; }

        #endregion

        public bool SAFlag { get; set; } // second accept flag

    }
}




