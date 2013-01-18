// --------------------------------------------------------------------------
// <copyright file="GetExchangeCurrenciesCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.1.2">Created</change>
// </history>


using Fanex.NAP.Framework.Data;

namespace Fanex.TicketControl.Core.Criteria
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class GetExchangeCurrenciesCriteria : CriteriaBase, ICriteria
    {
        /// <summary>
        /// Get the reference SP's Name which is specified in Config file.
        /// </summary>
        /// <returns>Store procedure alias name</returns>
        public override string GetSettingKey()
        {
            return "Licensee_SelExchangeCurrency";
        }

        /// <summary>
        /// Check data's constrains
        /// </summary>
        /// <returns>True: valid; False: invalid</returns>
        public override bool IsValid()
        {
            return true;
        }
    }
}
