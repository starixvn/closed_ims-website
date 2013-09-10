// --------------------------------------------------------------------------
// <copyright file="ILicenseeService.cs" company="Nexcel Solutions Vietnam">
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
    using System.Web.Mvc;
    using Fanex.Ares.Admin.Models.Licensee;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Licensee Service.
    /// </summary>
    public interface ILicenseeService
    {
        /// <summary>
        /// Get All Licensees.
        /// </summary>
        /// <param name="criteria">Licensee criteria.</param>
        /// <returns>Return result.</returns>
        IEnumerable<LicenseeModel> GetAllLicensees(ICriteria criteria);
        
        /// <summary>
        /// Get available Licensees.
        /// </summary>
        /// <param name="criteria">Licensee criteria.</param>
        /// <returns>Return result.</returns>
        IEnumerable<LicenseeModel> GetAvailableLicensees(ICriteria criteria);
        
        /// <summary>
        /// Get Licensees by party.
        /// </summary>
        /// <param name="criteria">Licensee criteria.</param>
        /// <returns>Return result.</returns>
        IEnumerable<SelectListItem> GetLicenseesByPartyID(ICriteria criteria);
        
        /// <summary>
        /// Execute Command.
        /// </summary>
        /// <param name="criteria">Licensee criteria.</param>
        /// <returns>Return result.</returns>
        Hashtable ExecuteCommand(ICriteria criteria);
                
        /// <summary>
        /// Get Available Or Selected Parties.
        /// </summary>
        /// <param name="criteria">Party criteria.</param>
        /// <returns>Return result.</returns>
        IEnumerable<SelectListItem> GetAvailableOrSelectedParties(ICriteria criteria);
    }
}