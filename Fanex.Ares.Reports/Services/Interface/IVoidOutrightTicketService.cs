using System.Collections.Generic;
using Fanex.Ares.Report.Criteria;
using Fanex.Ares.Report.Entities.Outright;
using Fanex.Ares.Report.ViewModels.Outright;

namespace Fanex.Ares.Report.Services.Interface
{
    public interface IVoidOutrightTicketService
    {
        IList<League> GetOutrightLeague();

        IList<Team> GetTeamByLeague(GetOutrightTeamCriteria criteria);

        OutrightTicketModel GetOutrightTicket(GetOutrightTicketCriteria criteria);
    }
}
