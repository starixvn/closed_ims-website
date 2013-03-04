using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fanex.TicketControl.Core.Models;
using Fanex.TicketControl.Misc.Entities;

namespace Fanex.TicketControl.Misc.Services.Interface
{
    public interface INumberWheelService
    {
        List<NumberWheelChoice> GetChoiceList();

        ActionResult InsertNewChoice(NumberWheelChoice newNumberWheelChoice);

        ActionResult UpdateChoice(int hdp1, int betTeam);

        string GetChoiceName(int hdp1, int betTeam);
    }
}
