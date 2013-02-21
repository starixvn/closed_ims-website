using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Fanex.NAP.Framework.Data;

namespace Fanex.TicketControl.Misc.Criteria
{
    public class GetTicketsListingCriteria : CriteriaBase
    {        
        public int MatchID { get; set; }

        [Display(Name = "Licensee")]
        public int LicenseeID { get; set; }

        public string BetTypes { get; set; }

        public string BetTypesLive { get; set; }

        [Display(Name="Type")]
        public int LiveType { get; set; }

        [Display(Name="Home")]
        public int LiveHomeScore { get; set; }

        [Display(Name = "Away")]
        public int LiveAwayScore { get; set; }

        [Display(Name="From")]
        public DateTime FromTransDate { get; set; }

        [Display(Name = "To")]
        public DateTime ToTransDate { get; set; }

        public int Type { get; set; }

        [Display(Name="Filter By")]
        public int SAFilter { get; set; }

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
