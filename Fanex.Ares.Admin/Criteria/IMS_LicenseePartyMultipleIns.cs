// --------------------------------------------------------------------------
// <copyright file="IMS_LicenseePartyMultipleIns.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Phillip.Truong" date="2013.05.31">Created</change>
// </history>

namespace Fanex.Ares.Admin.Criteria
{
    using Fanex.NAP.Framework.Data;

    public class IMS_LicenseePartyMultipleIns : CriteriaBase
    {
        public string XmlSettings { get; set; }

        [SpParam(Ignored=true)]
        public string LicenseeID { get; set; }

        [SpParam(Ignored = true)]
        public int PartyID { get; set; }
        public int ErrCode { get; set; }
        public string ErrMsg { get; set; }

        public override string GetSettingKey()
        {
            return "IMS_LicenseePartyMultipleIns";
        }

        public override bool IsValid() { return true; }
    }
}