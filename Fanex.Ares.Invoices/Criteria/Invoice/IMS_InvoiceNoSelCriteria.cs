// -----------------------------------------------------------------------
// <copyright file="IMS_InvoiceNoSelCriteria.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Ares.Invoice.Criteria.Invoice
{
    using Fanex.NAP.Framework.Data;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
     [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class IMS_InvoiceNoSelCriteria : CriteriaBase
    {
        public int PartyId { get; set; }

        public override string GetSettingKey()
        {
            return "IMS_NewInvoiceNoSel";
        }

        public override bool IsValid() { return true; }
    }
}
