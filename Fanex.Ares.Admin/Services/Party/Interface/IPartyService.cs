// --------------------------------------------------------------------------
// <copyright file="IPartyService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Phillip.Truong" date="2013.05.28">Created</change>
// </history>

namespace Fanex.Ares.Admin.Services.Party.Interface
{
    using System.Collections.Generic;
    using Fanex.Ares.Admin.Models.Party;
    using Fanex.NAP.Framework.Data;

    public interface IPartyService
    {
        IEnumerable<PartyModel> GetAllParties(ICriteria criteria);
    }
}
