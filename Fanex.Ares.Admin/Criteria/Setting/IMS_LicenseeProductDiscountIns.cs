// --------------------------------------------------------------------------
// <copyright file="IMS_AvailableLicenseeSelByParty.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Phillip.Truong" date="2013.06.03">Created</change>
// </history>

namespace Fanex.Ares.Admin.Criteria.Setting
{
    using Fanex.NAP.Framework.Data;

    public class IMS_LicenseeProductDiscountIns : CriteriaBase
    {
        public int LicenseeID { get; set; }
        public int PartyID { get; set; }
        public int ProductID { get; set; }
        public int Currency { get; set; }
        public double FromTotalAmount { get; set; }
        public double ToTotalAmount { get; set; }
        public double DiscountRate { get; set; }
        public int ErrCode { get; set; }
        public string ErrMsg { get; set; }

        public override string GetSettingKey()
        {
            return "IMS_LicenseeProductDiscountIns";
        }

        public override bool IsValid() { return true; }
    }
}