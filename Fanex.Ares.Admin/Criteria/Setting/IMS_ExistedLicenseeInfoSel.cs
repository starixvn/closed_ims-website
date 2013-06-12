// --------------------------------------------------------------------------
// <copyright file="IMS_AvailableLicenseeSelByParty.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Phillip.Truong" date="2013.06.03">Created</change>
// </history>

namespace Fanex.Ares.Admin.Criteria.Setting
{
    using Fanex.NAP.Framework.Data;

    public class IMS_ExistedLicenseeInfoSel : CriteriaBase
	{
        public override string GetSettingKey()
		{
			return "IMS_ExistedLicenseeInfoSel";
		}

        public override bool IsValid() { return true; }
	}
}