using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fanex.NAP.Framework.Data;
using Fanex.TicketControl.Core.Models;

namespace Fanex.TicketControl.Misc.Criteria
{
    public class GetTicketsListingDetailCriteria : CriteriaBase
    {
        public int MatchID { get; set; }

        public int BetTypeID { get; set; }

        public int EventType { get; set; } 

        public int? LiveHomeScore { get; set; }

        public int? LiveAwayScore { get; set; }

        public DateTime? FromTransDate { get; set; }

        public DateTime? ToTransDate { get; set; }

        public ProductType Product { get; set; } // type

        public int TicketType { get; set; } // SA Filter

        public override string GetSettingKey()
        {
            return "Acc_Rpt_TicketsListingDetail";
        }

        /// <summary>
        /// Check data's constrains
        /// </summary>
        /// <returns>True: valid; False: invalid</returns>
        public override bool IsValid()
        {
            return true;
        }
    }
}
