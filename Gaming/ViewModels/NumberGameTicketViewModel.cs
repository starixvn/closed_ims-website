// -----------------------------------------------------------------------
// <copyright file="NumberGameTicketModel.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.TicketControl.Gaming.ViewModels
{
    using System.Collections.Generic;
    using Fanex.TicketControl.Core.Models;
    using Fanex.TicketControl.Gaming.Entities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class NumberGameTicketModel
    {
        public NumberGameTicketModel()
        {
            Groups = new List<Group>();
        }

        public List<Group> Groups { get; set; }
    }

    /// <summary>
    /// We have 4 groups: Normal, betdaq, 12eu, mhc
    /// </summary>
    public class Group
    {
        public Group()
        {
            Tickets = new List<NumberGameTicket>();
        }

        public string Name { get; set; }

        public bool IsAllowOptionDelete { get; set; }

        public bool DeleteOtionDefault { get; set; }

        public List<NumberGameTicket> Tickets { get; set; }
    }
}
