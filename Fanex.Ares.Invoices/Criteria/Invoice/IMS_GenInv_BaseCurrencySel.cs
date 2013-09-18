// -----------------------------------------------------------------------
// <copyright file="IMS_GenInv_BaseCurrencySel.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Ares.Invoice.Criteria.Invoice
{
   using System;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Run stylecop")]
    public class IMS_GenInv_BaseCurrencySel : CriteriaBase
    {
        public int PartyID { get; set; }

        public string Products { get; set; }

        public int LicenseeID { get; set; }

        public override string GetSettingKey()
        {
            return "IMS_GenInv_BaseCurrencySel";
        }

        public override bool IsValid() { return true; }
    }
}
