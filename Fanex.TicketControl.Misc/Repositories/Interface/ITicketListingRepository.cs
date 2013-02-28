using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fanex.TicketControl.Misc.Criteria;
using Fanex.TicketControl.Misc.ViewModels;

namespace Fanex.TicketControl.Misc.Repositories.Interface
{
    public interface ITicketListingRepository
    {
        TicketListingModel GetTicketListing(GetTicketsListingCriteria criteria);

        TicketListingBetListModel GetTicketListingBetList(GetTicketsListingDetailCriteria criteria);
    }
}
