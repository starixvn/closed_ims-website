// -----------------------------------------------------------------------
// <copyright file="IMS_CheckExistedInvoiceNo.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Ares.Invoice.Criteria.Invoice
{
    using System.Diagnostics.CodeAnalysis;
    using Fanex.NAP.Framework.Data;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Run stylecop")]
    public class IMS_CheckExistedInvoiceNo : CriteriaBase
    {
        public int PartyID { get; set; }

        public int InvoiceNo { get; set; }

        public bool IsExisted { get; set; }

        public override string GetSettingKey()
        {
            return "IMS_CheckExistedInvoiceNo";
        }

        public override bool IsValid() 
        { 
            return true; 
        }
    }
}
