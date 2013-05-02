// -----------------------------------------------------------------------
// <copyright file="IMS_BaseCurrencySel.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//   <change who="Phillip.Truong" date="2013.4.25">Created</change>
// </history>

namespace Fanex.Ares.Report.Criteria
{
    using Fanex.NAP.Framework.Data;

    public class IMS_BaseCurrencySel : CriteriaBase
    {
        public override string GetSettingKey()
        {
            return "IMS_BaseCurrencySel";
        }

        public override bool IsValid() { return true; }
    }
}