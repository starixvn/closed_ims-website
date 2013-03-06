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
        [Display(Name = "MatchId")]
        public int MatchID { get; set; }

        // NOTE: will delete when intergrate with new DB
        //public int LicenseeID { get; set; }

        public string BetTypes { get; set; } // Dead Bettypes

        public string BetTypesLive { get; set; } // Live Bettypes

        [Display(Name="Event Type")]
        public int LiveType { get; set; }

        [Display(Name="Home")]
        public int? LiveHomeScore { get; set; }

        [Display(Name = "Away")]
        public int? LiveAwayScore { get; set; }

        [Display(Name="From")]
        public DateTime? FromTransDate { get; set; }

        [Display(Name = "To")]
        public DateTime? ToTransDate { get; set; }

        public ProductType Type { get; set; } // product

        [Display(Name="Ticket Type")]
        public int SAFilter { get; set; }

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

        public GetTicketsListingCriteria() 
        {
            BetTypes = string.Empty;
            BetTypesLive = string.Empty;
            //LicenseeID = 0;
        }
    }
}
