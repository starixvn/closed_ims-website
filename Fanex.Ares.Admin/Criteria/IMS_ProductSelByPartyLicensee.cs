// --------------------------------------------------------------------------
// <copyright file="IMS_PartyInfoSelCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Phillip.Truong" date="2013.05.30">Created</change>
// </history>

using Fanex.NAP.Framework.Data;
namespace Fanex.Ares.Admin.Criteria
{
    public class IMS_ProductSelByPartyLicensee : CriteriaBase
    {
        public int PartyID { get; set; }
        public int LicenseeID { get; set; }

        public override string GetSettingKey()
        {
            return "IMS_ProductSelByPartyLicensee";
        }

        public override bool IsValid() { return true; }
    }
}