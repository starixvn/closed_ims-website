using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Fanex.TicketControl.SportBook.Criteria;

namespace Fanex.TicketControl.SportBook.Repositories.Interface
{
    public interface IVoidOutrightTicketRepository
    {
        DataSet GetOutrightTicket(GetOutrightTicketCriteria criteria);
    }
}
