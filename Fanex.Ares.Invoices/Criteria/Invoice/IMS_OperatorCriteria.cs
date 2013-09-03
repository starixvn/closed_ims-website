// -----------------------------------------------------------------------
// <copyright file="IMS_OperatorCriteria.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Ares.Invoice.Service.Imp
{
    using System.Diagnostics.CodeAnalysis;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
     [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class IMS_OperatorCriteria : CriteriaBase
    {
        public int PartyID { get; set; }

        public override string GetSettingKey()
        {
            return "IMS_PartyInfoSel";
        }

        public override bool IsValid() 
        { 
            return true; 
        }
    }
}
