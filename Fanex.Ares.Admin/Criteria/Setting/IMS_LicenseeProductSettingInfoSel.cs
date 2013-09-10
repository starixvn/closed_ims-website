// --------------------------------------------------------------------------
// <copyright file="IMS_LicenseeProductSettingInfoSel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Phillip.Truong" date="2013.06.03">Created</change>
// </history>

namespace Fanex.Ares.Admin.Criteria.Setting
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Licensee Product Setting Info Select.
    /// </summary>
    public class IMS_LicenseeProductSettingInfoSel : CriteriaBase
    {
        public int PartyID { get; set; }

        public int LicenseeID { get; set; }

        public int ProductID { get; set; }

        /// <summary>
        /// Get setting key.
        /// </summary>
        /// <returns>Return setting key.</returns>
        public override string GetSettingKey()
        {
            return "IMS_LicenseeProductSettingInfoSel";
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