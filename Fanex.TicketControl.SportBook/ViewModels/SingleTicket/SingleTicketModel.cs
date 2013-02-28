// -----------------------------------------------------------------------
// <copyright file="SingleTicketModel.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.TicketControl.SportBook.ViewModels.SingleTicket
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.TicketControl.SportBook.Models.SingleTicket;
    using Fanex.TicketControl.SportBook.Models;
    using Fanex.TicketControl.Core.Models;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SingleTicketModel
    {
        public SingleTicketModel()
        {
            MatchInfo = new MatchInfo();
            Groups = new List<Group>();
        }

        public MatchInfo MatchInfo { get; set; }

        public List<Group> Groups { get; set; }

    }

    /// <summary>
    /// We have 4 groups: Normal, betdaq, 12eu, mhc
    /// </summary>
    public class Group
    {
        public Group()
        {
            Tickets = new List<TicketInfo>();
            Name = string.Empty;
            TicketInfo c = new TicketInfo();
        }

        public string Name { get; set; }

        public bool IsAllowOptionDelete { get; set; }

        public bool DeleteOtionDefault { get; set; }

        public List<TicketInfo> Tickets { get; set; }
    }
}
