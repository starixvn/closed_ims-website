using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Fanex.TicketControl.Gaming.Criteria;
using Fanex.TicketControl.Core.Models;

namespace Fanex.TicketControl.Gaming.Repositories.Interface
{
    public interface IRecoverNumberGameTicketRepository
    {
        DataTable GetNumberGameTicketRecover(GetRecoverNumberGameTicketCriteria criteria, ref ActionResult result);

        DataSet RecoverNumberGameTicket(RecoverNumberGameTicketCriteria criteria, ref ActionResult result);
    }
}
