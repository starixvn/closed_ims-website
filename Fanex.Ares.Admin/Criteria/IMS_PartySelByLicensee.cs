// --------------------------------------------------------------------------
// <copyright file="IMS_PartySelByLicensee.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Phillip.Truong" date="2013.05.31">Created</change>
// </history>

namespace Fanex.Ares.Admin.Criteria
{
    using Fanex.NAP.Framework.Data;

    public class IMS_PartySelByLicensee : CriteriaBase
    {
        public int LicenseeID { get; set; }

        public override string GetSettingKey()
        {
            return "IMS_PartySelByLicensee";
        }

        public override bool IsValid() { return true; }
    }
}