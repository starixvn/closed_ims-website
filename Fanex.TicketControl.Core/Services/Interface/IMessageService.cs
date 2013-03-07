using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fanex.TicketControl.Core.Models;
using System.Data;

namespace Fanex.TicketControl.Core.Services.Interface
{
    public interface IMessageService
    {
        ActionResult SendPrivateMsgToUsers(DataTable dtMsg, string matchId, Message pmsg);
    }
}
