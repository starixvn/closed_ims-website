using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fanex.TicketControl.Misc.Entities;

namespace Fanex.TicketControl.Misc.ViewModels
{
    public class TicketListingBetListModel
    {
        public List<TicketListingDetail> Items { get; set; }

        public int TotalItem { get; set; }
    }
}
