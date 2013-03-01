using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Fanex.TicketControl.SportBook.Criteria;



namespace Fanex.TicketControl.Misc.Repositories.Interface
{
    public interface IVoidSingleTicketRepository
    {
        DataSet GetSingleTicket(GetSingleTicketCriteria criteria);
    }
}
