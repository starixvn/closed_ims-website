// --------------------------------------------------------------------------
// <copyright file="IMS_PartyInfoSelCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Phillip.Truong" date="2013.05.28">Created</change>
// </history>

using System;
using Fanex.NAP.Framework.Data;
namespace Fanex.Ares.Admin.Criteria
{

    public class IMS_PartyInsUpCriteria : CriteriaBase
    {
        public int partyID { get; set; }

        public override string GetSettingKey()
        {
            return "IMS_ExchangeRateBillingSel";
        }

        public override bool IsValid() { return true; }
    }
}
