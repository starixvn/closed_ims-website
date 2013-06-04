// -----------------------------------------------------------------------
// <copyright file="IMS_InvoiceNo.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Ares.Invoice.Criteria.Invoice
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class IMS_InvoiceNoSelCriteria : CriteriaBase
    {
        public int LicenseeId { get; set; }

        public int PartyId { get; set; }

        public int InvoiceTypeID { get; set; }

        public override string GetSettingKey()
        {
            return "IMS_NewInvoiceNoSel";
        }

        public override bool IsValid() { return true; }
    }
}
