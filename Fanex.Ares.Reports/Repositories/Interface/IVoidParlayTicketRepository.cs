using System.Collections.Generic;
using System.Data;
using Fanex.Ares.Core.Models;
using Fanex.Ares.Report.Criteria;

namespace Fanex.Ares.Report.Repositories.Interface
{
    public interface IVoidParlayTicketRepository
    {
        DataSet GetParlayTicket(GetParlayTicketCriteria criteria);

        DataTable GetParlaySubTicket(GetParlaySubTicketCriteria criteria);

        IList<ActionResult> VoidParlayTicket(VoidParlayTicketCritetia criteria, ref DataSet dsOutData);
    }
}
