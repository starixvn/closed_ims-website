using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fanex.TicketControl.Core.Models;

namespace Fanex.TicketControl.Misc.Entities
{
    [Serializable]
    public class TicketListing
    {
        public string Id { get; set; }   

        public string Name { get; set; } 

        public bool LiveIndicator { get; set; }

        public int TotalTrans { get; set; }

        public List<TicketListing> Items { get; set; }
    }
}
