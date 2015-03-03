namespace Fanex.Ares.Core.Criteria
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Base Currency Criteria.
    /// </summary>
    public class BaseCurrencyCriteria : CriteriaBase
    {
        public override string GetSettingKey()
        {
            return "IMS_BaseCurrencySel";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}