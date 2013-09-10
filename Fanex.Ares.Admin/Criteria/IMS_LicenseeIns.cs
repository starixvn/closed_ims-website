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

    /// <summary>
    /// Licensee Insert.
    /// </summary>
    public class IMS_LicenseeIns : CriteriaBase
    {
        public int LicenseeID { get; set; }

        public bool IsOneWorkActive { get; set; }

        public string AttnName { get; set; }

        public string To1 { get; set; }

        public string To2 { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        /// <summary>
        /// Get setting key.
        /// </summary>
        /// <returns>Return setting key.</returns>
        public override string GetSettingKey()
        {
            return "IMS_LicenseeIns";
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