// <copyright file="IMS_InvoiceTypesCriteria.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>

namespace Fanex.Ares.Invoice.Criteria.Invoice
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Text;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
     [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Run stylecop")]
    public class IMS_InvoiceTypesCriteria : CriteriaBase
    {
        public int LicenseeId { get; set; }

        public override string GetSettingKey()
        {
            return "IMS_InvoiceTypeSel";
        }

        public override bool IsValid() 
        { 
            return true; 
        }
    }
}
