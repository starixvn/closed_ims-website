// --------------------------------------------------------------------------
// <copyright file="IMS_LicenseeCurrencySelCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   
// </history>

namespace Fanex.Ares.Admin.Criteria
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Licensee Currency Select Criteria.
    /// </summary>
    public class IMS_LicenseeCurrencySelCriteria : CriteriaBase
    {
        public int LicenseeID { get; set; }

        /// <summary>
        /// Get setting key.
        /// </summary>
        /// <returns>Return setting key.</returns>
        public override string GetSettingKey()
        {
            return "IMS_LicenseeCurrencySel";
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