// -----------------------------------------------------------------------
// <copyright file="GetBettypeCriteria.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//   <change who="Robert.Tran" date="2013.2.05">Created</change>
// </history>


using Fanex.Ares.Invoice.Common;
using Fanex.NAP.Framework.Data;
using System.Diagnostics.CodeAnalysis;

namespace Fanex.Ares.Invoice.Criteria
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class GetBettypeCriteria : CriteriaBase
    {

        public Enums.TicketControlMode Mode { get; set; }

        public override string GetSettingKey()
        {
            return "Common_SelBettype";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
