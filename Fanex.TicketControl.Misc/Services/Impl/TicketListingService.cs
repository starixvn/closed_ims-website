// --------------------------------------------------------------------------
// <copyright file="TicketListingService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.2.4">Created</change>
// </history>

namespace Fanex.TicketControl.Misc.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.TicketControl.Misc.Services.Interface;
    using Fanex.TicketControl.Core.Services.Impl;
    using Fanex.TicketControl.Core.Models;
    using Fanex.TicketControl.Core.Repository.Impl;
    using System.Data;
    using Fanex.TicketControl.Misc.Criteria;
    using Fanex.Common.Extensions;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class TicketListingService : ServiceBase, ITicketListingService
    {
        private readonly BaseRepository _ticketListingRepository;
        public TicketListingService()
        {
            _ticketListingRepository = new BaseRepository();
        }

        public IList<Licensee> GetAllLicensees()
        {
            Func<IDataReader, Licensee> mappingFunc = r =>
                {
                    return new Licensee { 
                        Id = r.ToInt("LicenseeID"),
                        Name = r.ToString("LicenseeName")
                    };
                };

            IList<Licensee> licensees = _ticketListingRepository.Fetch<Licensee>(new GetAllLicenseeCriteria(), mappingFunc);
            return licensees;
        }
    }
}
