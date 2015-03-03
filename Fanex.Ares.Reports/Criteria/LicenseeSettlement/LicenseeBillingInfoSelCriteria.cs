namespace Fanex.Ares.Report.Criteria.LicenseeSettlement
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Licensee Billing Info Select Criteria.
    /// </summary>
    public class LicenseeBillingInfoSelCriteria : CriteriaBase
    {
        public int LicenseeId
        {
            get;
            set;
        }

        public int ProductId
        {
            get;
            set;
        }

        public int PartyId
        {
            get;
            set;
        }

        public override string GetSettingKey()
        {
            return "IMS_Invoice_LicenseeBillingInfoSel";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}