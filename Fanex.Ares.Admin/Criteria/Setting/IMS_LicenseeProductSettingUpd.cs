// --------------------------------------------------------------------------
// <copyright file="IMS_LicenseeProductSettingUpd.cs" company="Nexcel Solutions Vietnam">
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

    /// <summary>
    /// Licensee Product Setting Update.
    /// </summary>
    public class IMS_LicenseeProductSettingUpd : CriteriaBase
    {
        public int LicenseeID { get; set; }

        public int ProductID { get; set; }

        public int PartyID { get; set; }

        public DateTime FirstBillingDate { get; set; }

        public DateTime HeadOfTerms { get; set; }

        public double MinimumFee { get; set; }

        public string MinimumFeeTerm { get; set; }

        public DateTime WaivedDate { get; set; }

        public int WaivedMonth { get; set; }

        public int Currency { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        /// <summary>
        /// Get setting key.
        /// </summary>
        /// <returns>Return setting key.</returns>
        public override string GetSettingKey()
        {
            return "IMS_LicenseeProductSettingUpd";
        }

        /// <summary>
        /// Check is valid.
        /// </summary>
        /// <returns>Return result.</returns>
        public override bool IsValid()
        {
            return true;
        }
    }
}