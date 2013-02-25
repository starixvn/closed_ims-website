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
    using Fanex.TicketControl.Core.Services.Interface;
    using Fanex.TicketControl.Core.Criteria;
    using Fanex.TicketControl.Misc.Repositories.Impl;
    using Fanex.TicketControl.Misc.DTO;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class TicketListingService : ServiceBase, ITicketListingService
    {
        private readonly TicketListingRepository _ticketListingRepository;
        private readonly ICoreService _coreService;

        public TicketListingService()
        {
            _ticketListingRepository = new TicketListingRepository();
            _coreService = new CoreService();
        }

        public IList<Bettypes> GetAllBettypes(GetBettypeCriteria criteria)
        {
            return _coreService.GetBettypes(criteria);
        }

        public ProductListingDTO GetTicketListing(GetTicketsListingCriteria criteria)
        {
            return _ticketListingRepository.GetTicketListing(criteria);
        }
    }
}
