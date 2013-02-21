using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fanex.TicketControl.Misc.Repositories.Interface;
using Fanex.TicketControl.Core.Repository.Impl;
using Fanex.TicketControl.Misc.Criteria;
using Fanex.TicketControl.Core.Models;

namespace Fanex.TicketControl.Misc.Repositories.Impl
{
    public class TicketListingRepository : BaseRepository, ITicketListingRepository
    {
        public void GetTicketListing(GetTicketsListingCriteria criteria)
        {

            switch (criteria.Product)
            {
                case ProductType.Sportsbook:
                    break;
                case ProductType.NumberGame:
                    break;
                case ProductType.VirtualSports:
                    break;
                case ProductType.LiveCasino:
                    break;
                default:
                    break;
            }

            throw new NotImplementedException();
        }
    }
}
