using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fanex.TicketControl.Misc.Repositories.Interface;
using Fanex.TicketControl.Core.Repository.Impl;
using Fanex.TicketControl.Misc.Criteria;

namespace Fanex.TicketControl.Misc.Repositories.Impl
{
    public class TicketListingRepository : BaseRepository, ITicketListingRepository
    {
        public void GetTicketListing(GetTicketsListingCriteria criteria)
        {

            switch (criteria.Product)
            {
                case Fanex.TicketControl.Core.Models.Enums.TicketControlMode.Sportsbook:
                    break;
                case Fanex.TicketControl.Core.Models.Enums.TicketControlMode.NumberGame:
                    break;
                case Fanex.TicketControl.Core.Models.Enums.TicketControlMode.VirtualSports:
                    break;
                case Fanex.TicketControl.Core.Models.Enums.TicketControlMode.LiveCasino:
                    break;
                default:
                    break;
            }

            throw new NotImplementedException();
        }
    }
}
