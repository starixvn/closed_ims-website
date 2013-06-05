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
    using System;
    using Fanex.NAP.Framework.Data;

    public class IMS_LicenseeProductChargeIns : CriteriaBase
    {
        public int LicenseeID { get; set; }
        public int PartyID { get; set; }
        public int ProductID { get; set; }
        public double DistributorRate { get; set; }
        public double PartnerRate { get; set; }
        public double ONEWorksRate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int ChargeType { get; set; }
        public int BillingCurrency { get; set; }
        public int ErrCode { get; set; }
        public string ErrMsg { get; set; }

        public override string GetSettingKey()
        {
            return "IMS_LicenseeProductChargeIns";
        }

        public override bool IsValid() { return true; }
    }
}