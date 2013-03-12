using Fanex.TicketControl.Gaming.Criteria;
using System.Data;
using System.Collections.Generic;
using Fanex.TicketControl.Core.Models;

namespace Fanex.TicketControl.Gaming.Repositories.Interface
{
    public interface IVoidNumberGameTicketRepository
    {
        DataSet GetNumberGameTicket(GetNumberGameTicketCriteria criteria);

        IList<ActionResult> VoidNumberGameTicket(VoidNumberGameTicketCritetia criteria, ref DataSet dsOutData);
    }
}
