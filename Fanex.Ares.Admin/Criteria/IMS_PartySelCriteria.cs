// -----------------------------------------------------------------------
// <copyright file="PartySel.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
using System;
using Fanex.NAP.Framework.Data;
namespace Fanex.Ares.Admin.Criteria
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class IMS_PartySelCriteria :  CriteriaBase
    {
        public override string GetSettingKey()
        {
            return "IMS_PartyInfoSel";
        }

        public override bool IsValid() { return true; }
    }
}
