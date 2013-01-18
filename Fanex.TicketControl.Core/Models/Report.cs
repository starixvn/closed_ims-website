// --------------------------------------------------------------------------
// <copyright file="Report.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.10.18">Created</change>
// </history>

using System.Collections.Generic;

namespace Fanex.TicketControl.Core.Models
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class Report<TDetail> : EntityBase<int, string>
    {
        /// <summary>
        /// Childs: sub report (use to group report by resellers, status, ...)
        /// </summary>
        public IList<Report<TDetail>> Childs { get; set; }

        /// <summary>
        /// Details: information of report (day, win loss, ...)
        /// </summary>
        public TDetail Details { get; set; }
    }
}
