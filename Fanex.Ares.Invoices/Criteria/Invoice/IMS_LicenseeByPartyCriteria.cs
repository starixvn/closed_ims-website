// <copyright file="IMS_LicenseeByPartyCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>

namespace Fanex.Ares.Invoice.Criteria.Invoice
{
    using System.Diagnostics.CodeAnalysis;
    using Fanex.NAP.Framework.Data;

     [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Run stylecop")]
    public class IMS_LicenseeByPartyCriteria : CriteriaBase
    {
        public int PartyID { get; set; }

        public override string GetSettingKey()
        {
            return "IMS_LicenseeSelByParty";
        }

        public override bool IsValid() 
        { 
            return true; 
        }
    }
}