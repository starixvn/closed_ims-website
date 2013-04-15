using System.Collections.Generic;
using System.Data;
using Fanex.Ares.Core.Models;
using Fanex.Ares.Report.Criteria;

namespace Fanex.Ares.Report.Repositories.Interface
{
    public interface IVoidSingleTicketRepository
    {
        DataSet GetSingleTicket(GetSingleTicketCriteria criteria);

        IList<ActionResult> VoidSingleTicket(VoidSingleTicketCritetia criteria, ref DataSet dsOutData);
    }
}
