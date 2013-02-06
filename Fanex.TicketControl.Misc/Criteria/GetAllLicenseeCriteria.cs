// --------------------------------------------------------------------------
// <copyright file="GetAllLicensee.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.2.4">Created</change>
// </history>


namespace Fanex.TicketControl.Misc.Criteria
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class GetAllLicenseeCriteria : CriteriaBase
    {
        /// <summary>
        /// Get the reference SP's Name which is specified in Config file.
        /// </summary>
        /// <returns>Store procedure alias name</returns>
        public override string GetSettingKey()
        {
            return "Misc_Acc_SiteSel_Licensee";
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
