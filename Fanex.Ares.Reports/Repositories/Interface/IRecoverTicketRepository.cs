using System.Data;
using Fanex.Ares.Core.Models;
using Fanex.Ares.Report.Criteria;

namespace Fanex.Ares.Report.Repositories.Interface
{
    public interface IRecoverTicketRepository
    {
        DataTable GetTicketRecover(GetRecoverTicketCriteria criteria, ref ActionResult result);

        DataSet RecoverTicket(RecoverTicketCriteria criteria, ref ActionResult result);

    }
}
