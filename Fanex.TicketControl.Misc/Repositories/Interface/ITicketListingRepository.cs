using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fanex.TicketControl.Misc.Criteria;
using Fanex.TicketControl.Misc.ViewModels;
using System.Data;

namespace Fanex.TicketControl.Misc.Repositories.Interface
{
    public interface ITicketListingRepository
    {
        TicketListingModel GetTicketListing(GetTicketsListingCriteria criteria);

        DataSet GetTicketListingBetList(GetTicketsListingDetailCriteria criteria);

        
    }
}
