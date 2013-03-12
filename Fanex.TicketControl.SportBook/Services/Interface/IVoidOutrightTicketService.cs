using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fanex.TicketControl.SportBook.Entities.Outright;
using Fanex.TicketControl.SportBook.Criteria;
using Fanex.TicketControl.SportBook.ViewModels.Outright;

namespace Fanex.TicketControl.SportBook.Services.Interface
{
    public interface IVoidOutrightTicketService
    {
        IList<League> GetOutrightLeague();

        IList<Team> GetTeamByLeague(GetOutrightTeamCriteria criteria);

        OutrightTicketModel GetOutrightTicket(GetOutrightTicketCriteria criteria);
    }
}
