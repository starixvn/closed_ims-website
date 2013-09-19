// --------------------------------------------------------------------------
// <copyright file="IMS_LicenseeInfoUpd.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Phillip.Truong" date="2013.05.31">Created</change>
// </history>

namespace Fanex.Ares.Admin.Criteria
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Licensee Info Update.
    /// </summary>
    public class IMS_LicenseeInfoUpd : IMS_LicenseeIns
    {
        public string LicenseeName { get; set; }

        public string BrandName { get; set; }

        public string CompanyName { get; set; }

        public bool IsDeposit { get; set; }

        public bool IsActive { get; set; }

        /// <summary>
        /// Get setting key.
        /// </summary>
        /// <returns>Return setting key.</returns>
        public override string GetSettingKey()
        {
            return "IMS_LicenseeInfoUpd";
        }
    }
}