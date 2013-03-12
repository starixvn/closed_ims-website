using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Fanex.TicketControl.SportBook.Criteria;
using Fanex.TicketControl.Core.Models;

namespace Fanex.TicketControl.SportBook.Repositories.Interface
{
    public interface IRecoverTicketRepository
    {
        DataTable GetTicketRecover(GetRecoverTicketCriteria criteria, ref ActionResult result);

        DataSet RecoverTicket(RecoverTicketCriteria criteria, ref ActionResult result);

    }
}
