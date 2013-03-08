using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Fanex.TicketControl.SportBook.Criteria;
using Fanex.TicketControl.Core.Models;



namespace Fanex.TicketControl.SportBook.Repositories.Interface
{
    public interface IVoidParlayTicketRepository
    {
        DataSet GetParlayTicket(GetParlayTicketCriteria criteria);

        DataTable GetParlaySubTicket(GetParlaySubTicketCriteria criteria);

        IList<ActionResult> VoidParlayTicket(VoidParlayTicketCritetia criteria, ref DataSet dsOutData);
    }
}
