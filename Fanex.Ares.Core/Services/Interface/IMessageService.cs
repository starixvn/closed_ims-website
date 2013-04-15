using System.Collections.Generic;
using System.Data;
using Fanex.Ares.Core.Models;

namespace Fanex.Ares.Core.Services.Interface
{
    public interface IMessageService
    {
        ActionResult SendPrivateMsgToUsers(DataTable dtMsg, string matchId, Message pmsg, ref List<string> listTransIdDone);
    }
}
