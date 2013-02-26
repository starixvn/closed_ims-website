using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fanex.TicketControl.Core.Models;

namespace Fanex.TicketControl.Misc.DTO
{
    [Serializable]
    public class ProductListingDTO
    {
        public List<ProductListing> Items { get; set; }

        public MatchInfo MatchInfo { get; set; }

        public Result Result { get; set; }
    }

    [Serializable]
    public class MatchInfo
    {
        public string MatchCode { get; set; }

        public string Home { get; set; }

        public string Away { get; set; }

        public string EventDate { get; set; }
    }

    [Serializable]
    public class ProductListing
    {
        public string Id { get; set; }   

        public string Name { get; set; } 

        public bool LiveIndicator { get; set; }

        public int TotalTrans { get; set; }

        public List<ProductListing> Items { get; set; }
 
    }
}
