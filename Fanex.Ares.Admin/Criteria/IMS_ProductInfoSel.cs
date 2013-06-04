// --------------------------------------------------------------------------
// <copyright file="IMS_ProductInfoSel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Phillip.Truong" date="2013.05.30">Created</change>
// </history>

namespace Fanex.Ares.Admin.Criteria
{
    using Fanex.NAP.Framework.Data;

    public class IMS_ProductInfoSel : CriteriaBase
    {
        public int ProductID { get; set; }

        public override string GetSettingKey()
        {
            return "IMS_ProductInfoSel";
        }

        public override bool IsValid() { return true; }
    }
}