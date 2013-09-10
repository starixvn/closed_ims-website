// --------------------------------------------------------------------------
// <copyright file="AccountingUserCheckOnlineStatusCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Ares.Admin.Criteria
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Accounting User Check Online Status Criteria.
    /// </summary>
    public class AccountingUserCheckOnlineStatusCriteria : CriteriaBase
    {
        public int On_UserID { get; set; }

        public bool IsKickedOut { get; set; }

        /// <summary>
        /// Get setting key.
        /// </summary>
        /// <returns>Return setting key.</returns>
        public override string GetSettingKey()
        {
            return "Acc_Recheckin";
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
