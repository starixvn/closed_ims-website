// -----------------------------------------------------------------------
// <copyright file="MatchInfo.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.TicketControl.Core.Models
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;
    using Fanex.Common;
    using Fanex.Data;
    using Fanex.TicketControl.Core.Models;
    using Fanex.TicketControl.Core.Repository.Impl;
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

        public string HomeName
        {
            get
            {
                return CacheRef.GetTeamName(HomeId) ?? string.Empty;
            }
        }

        public string AwayName
        {
            get
            {
                return CacheRef.GetTeamName(AwayId) ?? string.Empty;
            }
        }

    }
}
