// --------------------------------------------------------------------------
// <copyright file="IMS_LicenseeIns.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Phillip.Truong" date="2013.05.31">Created</change>
// </history>

namespace Fanex.Ares.Admin.Criteria
{
    using Fanex.NAP.Framework.Data;

    public class IMS_LicenseeIns : CriteriaBase
    {
        public string LicenseeName { get; set; }
        public string BrandName { get; set; }
        public string CompanyName { get; set; }
        public bool IsLabel { get; set; }
        public bool IsActive { get; set; }
        public bool IsOneWorkActive { get; set; }
        public string AttnName { get; set; }
        public string To1 { get; set; }
        public string To2 { get; set; }
        public int ErrCode { get; set; }
        public string ErrMsg { get; set; }

        public override string GetSettingKey()
        {
            return "IMS_LicenseeIns";
        }

        public override bool IsValid() { return true; }
    }
}