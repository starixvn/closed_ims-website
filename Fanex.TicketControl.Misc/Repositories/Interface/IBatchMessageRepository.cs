using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Fanex.TicketControl.Misc.Criteria;

namespace Fanex.TicketControl.Misc.Repositories.Interface 
{
    public interface IBatchMessageRepository
    {
        DataTable GetBatchMessage(GetBatchMessageCriteria criteria);
    }
}
