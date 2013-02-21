using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Fanex.NAP.Framework.Data;
using Fanex.TicketControl.Core.Models;

namespace Fanex.TicketControl.Misc.Criteria
{
    public class GetTicketsListingCriteria : CriteriaBase
    {        
        
        public int MatchID { get; set; }

        [Display(Name = "Licensee")]
        public int LicenseeID { get; set; }

        public string[] BetTypes { get; set; } // DeadBettypes

        public string[] LiveBetTypes { get; set; }

        [Display(Name="Event Type")]
        public int EventType { get; set; } // live type

        [Display(Name="Home")]
        public int LiveHomeScore { get; set; }

        [Display(Name = "Away")]
        public int LiveAwayScore { get; set; }

        [Display(Name="From")]
        public DateTime FromTransDate { get; set; }

        [Display(Name = "To")]
        public DateTime ToTransDate { get; set; }

        public ProductType Product { get; set; } // type

        [Display(Name="Ticket Type")]
        public int TicketType { get; set; } // SA Filter

        [SpParam(Ignored = true)]
        [Display(Name = "Group By")]
        public int GroupBy { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_Rpt_TicketsListing";
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
