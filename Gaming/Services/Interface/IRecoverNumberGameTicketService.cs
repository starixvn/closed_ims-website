using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fanex.TicketControl.Gaming.ViewModels;
using Fanex.TicketControl.Core.Models;
using Fanex.TicketControl.Gaming.Criteria;

namespace Fanex.TicketControl.Gaming.Services.Interface
{
    public interface IRecoverNumberGameTicketService
    {
        NumberGameTicketModel GetNumberGameTicketRecover(GetRecoverNumberGameTicketCriteria criteria, ref ActionResult result);

        ActionResult RecoverNumberGameTicket(RecoverNumberGameTicketCriteria criteria);
    }
}
