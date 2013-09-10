// --------------------------------------------------------------------------
// <copyright file="LicenseeProductDiscountModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Phillip.Truong" date="2013.06.03">Created</change>
// </history>

namespace Fanex.Ares.Admin.Models.Setting
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class LicenseeProductDiscountModel
    {
        public int ID { get; set; }

        public int PartyID { get; set; }

        public int LicenseeID { get; set; }

        public int ProductID { get; set; }

        public int Currency { get; set; }

        public double FromTotalAmount { get; set; }

        public double ToTotalAmount { get; set; }

        public double DiscountRate { get; set; }
    }
}