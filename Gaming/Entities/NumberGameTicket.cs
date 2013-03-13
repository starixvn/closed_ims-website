using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fanex.TicketControl.Core.Models;
using Fanex.TicketControl.Core.Utils;
using Fanex.TicketControl.Core.Services.Impl;

namespace Fanex.TicketControl.Gaming.Entities
{
    public class NumberGameTicket : TicketDetail
    {
        public int No { get; set; }

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

        public string Status
        {
            get
            {
                return this.Ticket.Status;
            }
        }

        public string Currency { get; set; }

        public string TransDate
        {
            get
            {
                return this.Ticket.TransDate.ToString(FormatKey.LONG_DATE_TIME_FORMAT);
            }
        }

        public string BetTypeName
        {
            get
            {
                return Helper.GetBetTypeName(this.Ticket.BetTypeId, this.Ticket.IsLive);
            }
        }
    }
}
