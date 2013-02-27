// -----------------------------------------------------------------------
// <copyright file="MatchInfo.cs" company="Microsoft">
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
    public class MatchInfo
    {
        public int MatchId { get; set; }

        public string MatchCode { get; set; }

        public string EventDate { get; set; }

        public int HomeId { get; set; }

        public int AwayId { get; set; }

        private string _HomeName = "";
        public string HomeName
        {
            get { return _HomeName; }
            private set
            {
                //get from RemoteRef
                _HomeName = CacheRef.GetTeamName(HomeId);
            }
        }

        private string _AwayName = "";
        public string AwayName
        {
            get { return _AwayName; }
            private set
            {
                //get from Remote Ref
                _AwayName = CacheRef.GetTeamName(HomeId);
            }
        }



    }
}
