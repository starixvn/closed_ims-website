using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Fanex.NAP.Framework.Data;
using Fanex.TicketControl.Core.Models;

namespace Fanex.TicketControl.Gaming.Criteria
{
    public class GetNumberGameTicketCriteria : CriteriaBase
    {
        [Display(Name = "Match ID")]
        public int MatchID { get; set; }

        [Display(Name = "From BetID")]
        public Int64 FromRefNo { get; set; }

        [Display(Name = "To BetID")]
        public Int64 ToRefNo { get; set; }

        public string Live { get; set; } // Live Bettypes

        public string Dead { get; set; } // Dead Bettypes

        public override string GetSettingKey()
        {
            return "SingleTicket_BettransSelSpecialDel";
        }

        /// <summary>
        /// Check data's constrains
        /// </summary>
        /// <returns>True: valid; False: invalid</returns>
        public override bool IsValid()
        {
            return true;
        }

        public GetNumberGameTicketCriteria() 
        {
            Live = string.Empty;
            Dead = string.Empty;
        }
    }
}
