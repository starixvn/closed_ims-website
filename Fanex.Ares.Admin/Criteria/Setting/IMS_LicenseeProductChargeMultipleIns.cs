// --------------------------------------------------------------------------
// <copyright file="IMS_LicenseeProductChargeMultipleIns.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Phillip.Truong" date="2013.06.03">Created</change>
// </history>

namespace Fanex.Ares.Admin.Criteria.Setting
{
    using Fanex.NAP.Framework.Data;
    using System.Web.Mvc;

    public class IMS_LicenseeProductChargeMultipleIns : CriteriaBase
    {
        [AllowHtml]
        public string XmlSettings { get; set; }
        public int ErrCode { get; set; }
        public string ErrMsg { get; set; }

        public override string GetSettingKey()
        {
            return "IMS_LicenseeProductChargeMultipleIns";
        }

        public override bool IsValid() { return true; }
    }
}


