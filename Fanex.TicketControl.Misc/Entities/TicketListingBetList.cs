using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fanex.TicketControl.Misc.Entities
{
    [Serializable]
    public class TicketListingBetList
    {
        public int TransID { get; set; }

        public DateTime TransDate { get; set; }

        public string Player { get; set; }

        public string Choice { get; set; }

        public float Odds { get; set; }

        public float Stake { get; set; }

        public float Winloss { get; set; }

        public string Currence { get; set; }

        public string Status { get; set; }

        public bool SA { get; set; }
    }
}




