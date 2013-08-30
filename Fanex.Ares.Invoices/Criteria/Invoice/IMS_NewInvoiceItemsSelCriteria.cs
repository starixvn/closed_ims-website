// -----------------------------------------------------------------------
// <copyright file="IMS_NewInvoiceItemsSelCriteria.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Ares.Invoice.Criteria.Invoice
{
    using System;
    using Fanex.NAP.Framework.Data;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
     [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class IMS_NewInvoiceItemsSelCriteria : CriteriaBase
    {
        public int PartyID { get; set; }

        public int LicenseeID { get; set; }

        public string Products { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public int BillingCurrency { get; set; }

        public override string GetSettingKey()
        {
            return "IMS_NewInvoiceItemsSel";
        }

        public override bool IsValid() { return true; }
    }
}
