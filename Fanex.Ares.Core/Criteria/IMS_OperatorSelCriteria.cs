// -----------------------------------------------------------------------
// <copyright file="IMS_OperatorSelCriteria.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Ares.Core.Criteria
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class IMS_OperatorSelCriteria : CriteriaBase
    {
        public override string GetSettingKey()
        {
            return "IMS_PartySel";
        }

        public override bool IsValid() { return true; }
    }
}
