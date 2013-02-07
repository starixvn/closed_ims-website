// -----------------------------------------------------------------------
// <copyright file="BaseModel.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.TicketControl.Core.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [Serializable]
    public class BaseModel
    {
        public string Name { get; set; }
        public int Id { get; set; }
    }
}
