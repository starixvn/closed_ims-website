// -----------------------------------------------------------------------
// <copyright file="TicketInfo.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.TicketControl.SportBook.Models.ParlayTicket
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class TicketInfo:BaseTicketInfo
    {
        public bool IsDelete { get; set; }

        public string WinLoss { get; set; }

        public string SubTransId { get; set; }
                
    }
}
