using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fanex.TicketControl.Core.Models;
using Fanex.TicketControl.Core.Services.Impl;

namespace Fanex.TicketControl.Misc.Entities
{
    [Serializable]
    public class TicketListing
    {
        public int BetTypeID { get; set; }

        public string BetTypeName
        {
            get
            {
                var betName = CacheRef.GetBettype(BetTypeID);

                switch (BetTypeID)
                {
                    case 86:
                        betName = LiveIndicator ? "Next Odd/Even" : "FT. Odd/Even";
                        break;
                    case 85:
                        betName = "Next Over/Under";
                        break;
                    case 90:
                        betName += LiveIndicator ? " (Live Game)" : " (Pre Game)";
                        break;
                    default:
                        break;
                }

                return betName;
            }
        }

        public bool LiveIndicator { get; set; }

        public int TotalTrans { get; set; }
    }
}
