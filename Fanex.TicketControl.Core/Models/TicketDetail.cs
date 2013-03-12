using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fanex.BetList.Core.Entities;
using System.Web.Script.Serialization;

namespace Fanex.TicketControl.Core.Models
{
    public class TicketDetail
    {
        [ScriptIgnore]
        public ITicket Ticket { get; set; }

        public string OddHtml { get; set; }

        public string ChoiceHtml { get; set; }

        public string StakeHtml { get; set; }
    }
}
