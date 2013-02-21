// -----------------------------------------------------------------------
// <copyright file="GetBettypeCriteria.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.TicketControl.Core.Criteria
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.NAP.Framework.Data;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.TicketControl.Core.Models;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class GetBettypeCriteria : CriteriaBase
    {
        public ProductType Mode { get; set; }

        public override string GetSettingKey()
        {
            return "Misc_SelBettype";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
