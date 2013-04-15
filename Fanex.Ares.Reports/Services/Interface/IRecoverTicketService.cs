using Fanex.Ares.Core.Models;
using Fanex.Ares.Report.Criteria;
using Fanex.Ares.Report.ViewModels.SingleTicket;

namespace Fanex.Ares.Report.Services.Interface
{
    public interface IRecoverTicketService
    {
        SingleTicketModel GetTicketRecover(GetRecoverTicketCriteria criteria, ref ActionResult result);

        ActionResult RecoverTicket(RecoverTicketCriteria criteria);
    }
}
