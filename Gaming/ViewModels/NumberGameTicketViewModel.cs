// -----------------------------------------------------------------------
// <copyright file="NumberGameTicketModel.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.TicketControl.Gaming.ViewModels
{
    using System.Collections.Generic;
    using Fanex.TicketControl.Core.Models;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class NumberGameTicketModel
    {
        public NumberGameTicketModel()
        {
            MatchInfo = new MatchInfo();
            //Groups = new List<Group>();
        }

        public MatchInfo MatchInfo { get; set; }

        //public List<Group> Groups { get; set; }

    }

    /// <summary>
    /// We have 4 groups: Normal, betdaq, 12eu, mhc
    /// </summary>
    //public class Group
    //{
    //    public Group()
    //    {
    //        Tickets = new List<TicketInfo>();
    //        Name = string.Empty;
    //        TicketInfo c = new TicketInfo();
    //    }

    //    public string Name { get; set; }

    //    public bool IsAllowOptionDelete { get; set; }

    //    public bool DeleteOtionDefault { get; set; }

    //    public List<TicketInfo> Tickets { get; set; }
    //}
}
