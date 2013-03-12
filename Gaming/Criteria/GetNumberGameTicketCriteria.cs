using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fanex.NAP.Framework.Data;
using System.ComponentModel.DataAnnotations;

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
            return "NumberGameTicket_BettransSelSpecialDel";
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
