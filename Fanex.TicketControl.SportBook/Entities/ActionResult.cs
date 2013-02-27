// -----------------------------------------------------------------------
// <copyright file="Result.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.TicketControl.SportBook.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ActionResult
    {
        public int ErrorCode { get; set; }

        public string ErrorMsg { get; set; }

        public dynamic ExtendContent { get; set; }

        public ActionResult ()
        {
            ErrorCode = 0;
            ErrorMsg = string.Empty;
        }


    }
}
