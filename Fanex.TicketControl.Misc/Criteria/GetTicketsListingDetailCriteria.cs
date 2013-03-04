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
        #region main params for sp

        public int MatchID { get; set; }

        public string LicenseeNames { get; set; } // NOTE: will delete when integrate with new DB

        public string BetTypes { get; set; } // Dead Bettypes

        public string BetTypesLive { get; set; } // Live Bettypes

        public int LiveType { get; set; }

        public int? LiveHomeScore { get; set; }

        public int? LiveAwayScore { get; set; }

        public DateTime? FromTransDate { get; set; }

        public DateTime? ToTransDate { get; set; }

        public ProductType Type { get; set; } // type

        public int SAFilter { get; set; } // SA Filter 

        #endregion

        #region params for paging

        [SpParam(Ignored = true)]
        public int PageIndex { get; set; }

        [SpParam(Ignored = true)]
        public int PageSize { get; set; }

        #endregion

        #region params contain match info and bet type name

        [SpParam(Ignored = true)]
        public string MatchCode { get; set; }

        [SpParam(Ignored = true)]
        public string AwayName { get; set; }

        [SpParam(Ignored = true)]
        public string HomeName { get; set; }

        [SpParam(Ignored = true)]
        public DateTime EventDate { get; set; }

        [SpParam(Ignored = true)]
        public string Title { get; set; }

        [SpParam(Ignored = true)]
        public string BetTypeName { get; set; }

        #endregion

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

        public GetTicketsListingDetailCriteria() 
        {
            LicenseeNames = "IBC";
            BetTypes = string.Empty;
            BetTypesLive = string.Empty;
        }
    }
}
