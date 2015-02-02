namespace Fanex.Ares.Invoice.Criteria.Invoice
{
    using System;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Product Charge Rate Criteria.
    /// </summary>
    public class ProductChargeRateCriteria : CriteriaBase
    {
        public int PartyID { get; set; }

        public int LicenseeID { get; set; }

        public string Products { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public override string GetSettingKey()
        {
            return "IMS_LicenseeProductChargeRateSel";
        }

        public override bool IsValid() 
        { 
            return true; 
        }
    }
}
