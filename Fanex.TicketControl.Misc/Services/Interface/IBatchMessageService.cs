using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fanex.TicketControl.Core.Models;

namespace Fanex.TicketControl.Misc.Services.Interface
{
    public interface IBatchMessageService
    {
        List<ActionResult> UploadMsg(Message pmsg, string TransIds);
    }
}
