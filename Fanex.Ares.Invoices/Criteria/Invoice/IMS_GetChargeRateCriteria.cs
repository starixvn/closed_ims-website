// -----------------------------------------------------------------------
// <copyright file="IMS_GetChargeRateCriteria.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Ares.Invoice.Criteria.Invoice
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class IMS_GetChargeRateCriteria : CriteriaBase
    {
        public int partyID { get; set; }
        public int licenseeID { get; set; }
        public string Products { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }
        public override string GetSettingKey()
        {
            return "IMS_LicenseeProductChargeRateSel";
        }

        public override bool IsValid() { return true; }
    }
}
