// -----------------------------------------------------------------------
// <copyright file="IMS_LicenseeProductSelCriteria.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//   <change who="Phillip.Truong" date="2013.4.23">Created</change>
// </history>
namespace Fanex.Ares.Report.Criteria
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.NAP.Framework.Data;

    public class IMS_LicenseeProductSelCriteria : CriteriaBase
    {
        public int LicenseeId { get; set; }

        public override string GetSettingKey()
        {
            return "IMS_LicenseeProductSel";
        }

        public override bool IsValid() { return true; }
    }
}