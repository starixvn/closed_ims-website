namespace Fanex.Ares.Invoice.Criteria.Invoice
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get Operator.
    /// </summary>
    public class OperatorSelectionCriteria : CriteriaBase
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
