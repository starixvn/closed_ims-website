using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fanex.TicketControl.Core.Models;
using Fanex.TicketControl.Misc.Entities;

namespace Fanex.TicketControl.Misc.ViewModels
{
    [Serializable]
    public class TicketListingModel
    {
        public List<TicketListing> Items { get; set; }

        public MatchInfo MatchInfo { get; set; }

        public ActionResult Result { get; set; }
    }
}
