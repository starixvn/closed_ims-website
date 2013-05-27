// -----------------------------------------------------------------------
// <copyright file="PartyCriteria.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
using System;
using Fanex.NAP.Framework.Data;
namespace Fanex.Ares.Admin.Criteria
{

    public class IMS_PartyInsUpCriteria : CriteriaBase
    {
        public int partyID { get; set; }

        public override string GetSettingKey()
        {
            return "IMS_ExchangeRateBillingSel";
        }

        public override bool IsValid() { return true; }
    }
}
