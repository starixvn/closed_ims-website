using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fanex.TicketControl.Core.Models;

namespace Fanex.TicketControl.Core.Services.Interface
{
    public interface IMessageService
    {
        int SetMessage(Message Msg, int[] TargetId, string[] TargetName, int MatchId, string[] TransId);

        void AddTargt2Message(int MessageId, int[] TargetId, string[] TargetName, string[] TransId);
    }
}
