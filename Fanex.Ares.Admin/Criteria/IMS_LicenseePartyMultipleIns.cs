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
    using System.Web.Mvc;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Licensee Party Multiple Insert.
    /// </summary>
    public class IMS_LicenseePartyMultipleIns : CriteriaBase
    {
        [AllowHtml]
        public string XmlSettings { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        /// <summary>
        /// Get setting key.
        /// </summary>
        /// <returns>Return setting key.</returns>
        public override string GetSettingKey()
        {
            return "IMS_LicenseePartyMultipleIns";
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