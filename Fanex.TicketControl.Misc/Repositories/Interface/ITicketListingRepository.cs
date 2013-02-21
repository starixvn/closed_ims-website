using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fanex.TicketControl.Misc.Criteria;

namespace Fanex.TicketControl.Misc.Repositories.Interface
{
    public interface ITicketListingRepository
    {
        void GetTicketListing(GetTicketsListingCriteria criteria);
    }
}
