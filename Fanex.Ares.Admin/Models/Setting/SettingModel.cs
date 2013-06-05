// --------------------------------------------------------------------------
// <copyright file="SettingModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Phillip.Truong" date="2013.06.03">Created</change>
// </history>

namespace Fanex.Ares.Admin.Models.Setting
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SettingModel
    {
        public int PartyID { get; set; }

        public int LicenseeID { get; set; }

        public int ProductID { get; set; }

        public DateTime FirstBillingDate { get; set; }

        public DateTime HeadOfTerms { get; set; }

        public double MinimumFee{ get; set; }

        public DateTime WaivedDate { get; set; }

        public int WaivedMonth { get; set; }

        public int Currency { get; set; }
    }
}