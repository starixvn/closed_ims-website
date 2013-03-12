using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fanex.TicketControl.SportBook.ViewModels.SingleTicket;
using Fanex.TicketControl.SportBook.Criteria;
using Fanex.TicketControl.Core.Models;

namespace Fanex.TicketControl.SportBook.Services.Interface
{
    public interface IRecoverTicketService
    {
        SingleTicketModel GetTicketRecover(GetRecoverTicketCriteria criteria, ref ActionResult result);

        ActionResult RecoverTicket(RecoverTicketCriteria criteria);
    }
}
