// -----------------------------------------------------------------------
// <copyright file="IMS_ExchangeRateBillingSelCriteria.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//   <change who="Phillip.Truong" date="2013.4.23">Created</change>
// </history>

namespace Fanex.Ares.Report.Criteria.LicenseeSettlement
{
    using System;
    using Fanex.NAP.Framework.Data;

    public class IMS_Invoice_ExchangeRateBillingSel : IMS_ExchangeRateBillingSelCriteria
    {
     
        public override string GetSettingKey()
        {
            return "IMS_Invoice_ExchangeRateBillingSel";
        }

      
    }
}