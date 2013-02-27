// -----------------------------------------------------------------------
// <copyright file="SingleTicket.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.TicketControl.SportBook.Models.SingleTicket
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    /// 

    public class SingleTicketModel
    {
        public SingleTicketModel()
        {
            MatchInfo = new MatchInfo();
        }

        public MatchInfo MatchInfo { get; set; }

        public List<Licensee> Licensees { get; set; }
        
    }

    public class Licensee
    {
        public Licensee ()
        {
            Tickets = new List<TicketInfo>();
            Name = string.Empty;
            TicketInfo c = new TicketInfo();           
        }

        public string Name { get; set; }

        public bool IsAllowOptionDelete { get; set; }

        public bool DeleteOtionDefault { get; set; }

        public List<TicketInfo> Tickets {get;set;}
    }
}
