// -----------------------------------------------------------------------
// <copyright file="IMS_LicenseeBillingInfoSelCriteria.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//   <change who="Phillip.Truong" date="2013.4.23">Created</change>
// </history>

namespace Fanex.Ares.Invoice.Criteria.LicenseeSettlement
{
    using Fanex.NAP.Framework.Data;

    public class IMS_LicenseeBillingInfoSelCriteria : CriteriaBase
    {
        public int LicenseeId { get; set; }
        public int ProductId { get; set; }

        public override string GetSettingKey()
        {
            return "IMS_LicenseeBillingInfoSel";
        }

        public override bool IsValid() { return true; }
    }
}