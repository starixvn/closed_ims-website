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
    using System.Collections;
    using System.Collections.Generic;
    using System.Web;
    using System.Web.Mvc;
    using Fanex.Ares.Admin.Criteria;
    using Fanex.Ares.Admin.Models.Party;
    using Fanex.NAP.Framework.Data;

    public interface IPartyService
    {
        IEnumerable<PartyModel> GetAllParties(ICriteria criteria);
        IEnumerable<SelectListItem> GetAllPartyTypes(ICriteria criteria);
        Hashtable ExecuteCommand(ICriteria criteria);
        IEnumerable<SelectListItem> GetAvailableOrSelectedLicensees(ICriteria criteria);
        Hashtable CreateUpdateParty(IMS_PartyIns criteria, HttpRequestBase Request);
    }
}
