// --------------------------------------------------------------------------
// <copyright file="IMS_PartyInsUpCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Phillip.Truong" date="2013.05.28">Created</change>
// </history>

namespace Fanex.Ares.Admin.Criteria
{
    using System;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Party Insert/Update Criteria.
    /// </summary>
    public class IMS_PartyInsUpCriteria : CriteriaBase
    {
        public int PartyID { get; set; }

        /// <summary>
        /// Get setting key.
        /// </summary>
        /// <returns>Return setting key.</returns>
        public override string GetSettingKey()
        {
            return "IMS_ExchangeRateBillingSel";
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
