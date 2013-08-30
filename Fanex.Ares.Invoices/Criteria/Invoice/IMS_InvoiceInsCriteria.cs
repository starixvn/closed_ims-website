// -----------------------------------------------------------------------
// <copyright file="IMS_InvoiceInsCriteria.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Ares.Invoice.Criteria.Invoice
{
    using System;
    using System.Collections.Generic;
    using Fanex.Ares.Invoice.Models.Invoice;
    using Fanex.NAP.Framework.Data;
    using System.Diagnostics.CodeAnalysis;

      [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]   
    public class IMS_InvoiceInsCriteria : CriteriaBase
    {                     
        public string XMLInvoice { get; set; }

        public int AdminID { get; set; }

        public int InvoiceID { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
        {
            return "IMS_InvoiceIns";
        }

        public override bool IsValid() { return true; }
    }
}
