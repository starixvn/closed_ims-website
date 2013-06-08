// -----------------------------------------------------------------------
// <copyright file="IMS_Rpt_LicenseeSettlementCriteria.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//   <change who="Phillip.Truong" date="2013.4.23">Created</change>
// </history>
namespace Fanex.Ares.Report.Criteria.LicenseeSettlement
{
    using System;
    using Fanex.NAP.Framework.Data;
    using Fanex.Ares.Report.Criteria.LicenseeSettlement;
    [Serializable]
    public class IMS_Invoice_Rpt_LicenseeSettlementCriteria : IMS_Rpt_LicenseeSettlementCriteria
    {
        public override string GetSettingKey()
        {
            return "IMS_Invoice_Rpt_LicenseeSettlement";
        }
    }
}