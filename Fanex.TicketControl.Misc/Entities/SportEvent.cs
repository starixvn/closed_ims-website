using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fanex.TicketControl.Misc.Entities
{
    [Serializable]
    public class SportEvent
    {
        public string Choice { get; set; }

        public string Hdp { get; set; }

        public string LiveScore { get; set; }

        public string BetType { get; set; }

        public string Selection { get; set; } // only used for system parlay

        public string Event { get; set; }

        public string SportType { get; set; }

        public string LeagueName { get; set; }

        public string MoreBetInfo { get; set; }
    }
}
