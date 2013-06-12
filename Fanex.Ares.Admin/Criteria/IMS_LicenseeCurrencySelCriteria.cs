// --------------------------------------------------------------------------
// <copyright file="IMS_LicenseeProductCurrencySelCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   
// </history>

namespace Fanex.Ares.Admin.Criteria
{
    using Fanex.NAP.Framework.Data;

    public class IMS_LicenseeCurrencySelCriteria : CriteriaBase
    {
        public int LicenseeID { get; set; }

        public override string GetSettingKey()
        {
            return "IMS_LicenseeCurrencySel";
        }

        public override bool IsValid() { return true; }
    }
}